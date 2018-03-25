using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;

using System.Xml;

namespace ABCProvider
{
    public class GenerationProvider
    {
        public static void GenerateAll ( )
        {
            EnumGenerator.GetEnumListFromDB();
            EnumGenerator.GenerateEnumToDLL();
            EnumGenerator.WriteToXML( @"EnumDefine.xml" );
         
       //     Generation.BusinessObjectGenerator.GenerateToBaseObjectDLL();
            BusinessObjectGenerator.GenerateToBusinessObjectDLL();
            WriteTableListToXML( @"TableList.xml" );
        }

        public static bool DetectModifyDatabase ( )
        {
            bool isModified=EnumGenerator.DetectModifyEnumDefineTable();

            ModifiedType modifyType=IsDatabaseModified();
            //if ( modifyType!=ModifiedType.None )
            //{
            //    foreach ( String strTableName in TableStructureProvider.DataTablesList.Keys )
            //        Generation.StoredProcedureGenerator.GenSP( strTableName );
            //}
            if ( modifyType==ModifiedType.All||modifyType==ModifiedType.Base )
            {
                if ( System.IO.File.Exists( @"BaseObjects.dll" )==false )
                    BusinessObjectGenerator.GenerateToBaseObjectDLL();
            }
            if ( modifyType==ModifiedType.All||modifyType==ModifiedType.Business )
                BusinessObjectGenerator.GenerateToBusinessObjectDLL();

            if ( modifyType!=ModifiedType.None )
            {
                WriteTableListToXML( @"TableList.xml" );
                if ( !isModified )
                    isModified=true;
            }

            DataConfigProvider.GenerateDefaultTableConfig();

            return isModified;
        }

        public enum ModifiedType
        {
            None,
            Base,
            Business,
            All
        }
        public static ModifiedType IsDatabaseModified ( )
        {
            Dictionary<String , List<String>> XMLDataList=new Dictionary<String , List<String>>();

            String strFileName=@"TableList.xml";
            if ( System.IO.File.Exists( strFileName )==false )
                return ModifiedType.All;

            ModifiedType result=ModifiedType.None;

            XmlDocument doc=new XmlDocument();
            doc.Load( strFileName );

            XmlNodeList nodeTableList=doc.GetElementsByTagName( "Table" );
            foreach ( XmlNode nodeTbl in nodeTableList )
            {
                XmlNode nodeCols=nodeTbl.ChildNodes[0];
                XmlNode nodeFKs=nodeTbl.ChildNodes[1];
                String strTableName=nodeTbl.Attributes["Name"].Value.ToString();
                String strID=nodeTbl.Attributes["PK"].Value.ToString();

                if ( DataStructureProvider.DataTablesList.ContainsKey( strTableName )==false )
                {
                    if ( DataStructureProvider.IsSystemTable( strTableName ) )
                    {
                        if ( result==ModifiedType.None )
                            result=ModifiedType.Base;
                        else if ( result==ModifiedType.Business )
                            result=ModifiedType.All;
                    }
                    else
                    {
                        if ( result==ModifiedType.None )
                            result=ModifiedType.Business;
                        else if ( result==ModifiedType.Base )
                            result=ModifiedType.All;
                    }
                    if ( result==ModifiedType.All )
                        return result;
                }

                #region Table has been Modified
                TableStructureData dataTable=DataStructureProvider.DataTablesList[strTableName];
                if ( strID!=dataTable.PrimaryColumn||nodeCols.ChildNodes.Count!=dataTable.ColumnsList.Count||nodeFKs.ChildNodes.Count!=dataTable.ForeignColumnsList.Count )
                {
                    if ( DataStructureProvider.IsSystemTable( strTableName ) )
                    {
                        if ( result==ModifiedType.None )
                            result=ModifiedType.Base;
                        else if ( result==ModifiedType.Business )
                            result=ModifiedType.All;
                    }
                    else
                    {
                        if ( result==ModifiedType.None )
                            result=ModifiedType.Business;
                        else if ( result==ModifiedType.Base )
                            result=ModifiedType.All;
                    }
                    if ( result==ModifiedType.All )
                        return result;
                }

                #region Check Columns
                foreach ( XmlNode nodeCol in nodeCols.ChildNodes )
                {
                    String strCol=nodeCol.InnerText;
                    String strType=nodeCol.Attributes["type"].Value.ToString();
                    if ( dataTable.ColumnsList.ContainsKey( strCol )==false||DataStructureProvider.GetColumnDbType( strTableName , strCol )!=strType )
                    {
                        if ( DataStructureProvider.IsSystemTable( strTableName ) )
                        {
                            if ( result==ModifiedType.None )
                                result=ModifiedType.Base;
                            else if ( result==ModifiedType.Business )
                                result=ModifiedType.All;
                        }
                        else
                        {
                            if ( result==ModifiedType.None )
                                result=ModifiedType.Business;
                            else if ( result==ModifiedType.Base )
                                result=ModifiedType.All;
                        }
                        if ( result==ModifiedType.All )
                            return result;
                    }
                }
                #endregion

                #region Check FK
                foreach ( XmlNode nodeFK in nodeFKs.ChildNodes )
                {
                    String strCol=nodeFK.InnerText;
                    String strPKtable=nodeFK.Attributes["PKTable"].Value.ToString();
                    if ( dataTable.ForeignColumnsList.ContainsKey( strCol )==false||dataTable.ForeignColumnsList[strCol]!=strPKtable )
                    {
                        if ( DataStructureProvider.IsSystemTable( strTableName ) )
                        {
                            if ( result==ModifiedType.None )
                                result=ModifiedType.Base;
                            else if ( result==ModifiedType.Business )
                                result=ModifiedType.All;
                        }
                        else
                        {
                            if ( result==ModifiedType.None )
                                result=ModifiedType.Business;
                            else if ( result==ModifiedType.Base )
                                result=ModifiedType.All;
                        }
                        if ( result==ModifiedType.All )
                            return result;
                    }
                }
                #endregion

                #endregion
            }

            return result;

        }

        public static void WriteTableListToXML ( String strFileName  )
        {
            XmlDocument doc=new XmlDocument();
            XmlDeclaration dec=doc.CreateXmlDeclaration( "1.0" , null , null );
            doc.AppendChild( dec );

            XmlElement root=doc.CreateElement( "Tables" );
            doc.AppendChild( root );

            foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
            {
                XmlElement elTables=GetTableXmlElement( doc , strTableName );

                root.AppendChild( elTables );
            }
            doc.Save( strFileName );
        }
        private static XmlElement GetTableXmlElement ( XmlDocument doc , String strTable )
        {
            XmlElement elTables=doc.CreateElement( "Table" );
            elTables.SetAttribute( "Name" , strTable );
            elTables.SetAttribute( "PK" , DataStructureProvider.GetPrimaryKeyColumn( strTable ) );

            #region Columns
            XmlElement elCols=doc.CreateElement( "Columns" );
            foreach ( String strCol in DataStructureProvider.DataTablesList[strTable].ColumnsList.Keys )
            {
                XmlElement elField=doc.CreateElement( "Field" );
                elField.InnerText=strCol;
                elField.SetAttribute( "type" , DataStructureProvider.GetColumnDbType( strTable , strCol ) );
                elCols.AppendChild( elField );
            }
            elTables.AppendChild( elCols );
            #endregion

            #region FK Columns
            XmlElement elFKCols=doc.CreateElement( "FK" );
            foreach ( String strCol in DataStructureProvider.DataTablesList[strTable].ForeignColumnsList.Keys )
            {
                XmlElement elField=doc.CreateElement( "Field" );
                elField.InnerText=strCol;
                elField.SetAttribute( "PKTable" , DataStructureProvider.DataTablesList[strTable].ForeignColumnsList[strCol] );
                elFKCols.AppendChild( elField );
            }
            elTables.AppendChild( elFKCols );
            #endregion

            return elTables;
        }

    }

    public class BaseGenerator
    {
        public static String NewLine
        {
            get
            {
                return "\n";
            }
        }
        public static String Tab
        {
            get
            {
                return "\t";
            }
        }
    }

    public class StoredProcedureGenerator : BaseGenerator
    {

        #region Generate SPs
        public static void GenSP ( String strTableName )
        {
            DeleteAllSP( strTableName );

            GenSPInsert( strTableName );
            GenSPUpdate( strTableName );

            GenSPSelect( strTableName );
            GenSPSelectByName( strTableName );
            GenSPSelectByNo( strTableName );
            GenSPSelectAll( strTableName );
            GenSPSelectByFK( strTableName );

            GenSPDelete( strTableName );
            GenSPDeleteAll( strTableName );
            GenSPDeleteByFK( strTableName );

            if ( DataStructureProvider.IsSystemTable( strTableName )==false )
            {
                GenSPRealDelete( strTableName );
                GenSPRealDeleteAll( strTableName );
                GenSPRealDeleteByFK( strTableName );
            }

            String strQuery=String.Format("DROP INDEX {0} ON {0}",strTableName);
            ABCBusinessEntities.BusinessObjectController.RunQuery(strQuery);

            strQuery=String.Format( "CREATE INDEX {0} ON {0} ({1})" , strTableName , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) );
            ABCBusinessEntities.BusinessObjectController.RunQuery( strQuery );

        }

        public static void DeleteAllSP ( String strTableName )
        {
             DeleteSP( GetSPName( SPType.Insert , strTableName ) );
             DeleteSP( GetSPName( SPType.Update , strTableName ) );
             DeleteSP( GetSPName( SPType.Select , strTableName  ) );
             DeleteSP( GetSPName( SPType.SelectByName , strTableName  ) );
             DeleteSP( GetSPName( SPType.SelectByNo , strTableName ) );
             DeleteSP( GetSPName( SPType.SelectAll , strTableName  ) );
             DeleteSP( GetSPName( SPType.Delete , strTableName  ) );
             DeleteSP( GetSPName( SPType.DeleteAll , strTableName  ) );


             if ( DataStructureProvider.IsSystemTable( strTableName )==false )
             {
                 DeleteSP( GetSPName( SPType.RealDelete , strTableName ) );
                 DeleteSP( GetSPName( SPType.RealDeleteAll , strTableName ) );
             }
             Dictionary<String , String> lstFkColumns=DataStructureProvider.GetAllTableForeignColumns( strTableName );
             if ( lstFkColumns==null )
                 return;

             foreach ( String strCol in lstFkColumns.Keys )
             {
                 DeleteSP( GetSPName( SPType.SelectByFK , strTableName , strCol ) );
                 DeleteSP( GetSPName( SPType.DeleteByFK , strTableName , strCol ) );
                 if ( DataStructureProvider.IsSystemTable( strTableName )==false )
                     DeleteSP( GetSPName( SPType.RealDeleteByFK , strTableName , strCol ) );
             }
        }
        public static String DeleteSP ( String strSPName )
        {
            StringBuilder builder=new StringBuilder();
            builder.Append( String.Format( "IF OBJECT_ID(N'[{0}]') IS NOT NULL" , strSPName ) );
            builder.Append( NewLine );
            builder.Append( String.Format( "DROP PROCEDURE [{0}]" , strSPName ) );
            return DataQueryProvider.ExecuteScript( builder.ToString() );
        }

        #region Insert - Update

        private static String GenInsertUpdateParams ( SPType type,String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();
            Dictionary<String , TableColumnData> lstColumns=DataStructureProvider.GetAllTableColumns( strTableName );

            String strColID=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            int i=0;
            foreach ( String strCol in lstColumns.Keys )
            {

                String strParameter=Tab+GenParam( strTableName , strCol );
                if ( type==SPType.Insert&&strCol==strColID )
                    strParameter+="  OUTPUT ";

                if ( i<lstColumns.Count-1 )
                    strParameter+=",";

                strBuilder.Append( strParameter );
                strBuilder.Append( NewLine );

                i++;
            }

            return strBuilder.ToString();
        }

        public static String GenSPInsert ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.Insert , strTableName ) ) );
            strBuilder.Append( GenInsertUpdateParams( SPType.Insert , strTableName ) );

            strBuilder.Append( GenSPBeginQuery() );

            if ( DataStructureProvider.IsTableColumn( strTableName , "NoIndex" ) )
            {
                strBuilder.Append( String.Format( @"SET @NoIndex=(SELECT MAX(NoIndex) +1 FROM [{0}]);" , strTableName ) );
                strBuilder.Append( NewLine );
            }

            String strPKCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            if ( !String.IsNullOrWhiteSpace( strPKCol ) )
            {
                strBuilder.Append( String.Format( @"IF ((@{0} IS NULL) OR (@{0} = cast(cast(0 as binary) as uniqueidentifier))) BEGIN  SET @{0} =NEWID(); END" , strPKCol ) );
                strBuilder.Append( NewLine );
            }
            strBuilder.Append( QueryTemplateGenerator.GenInsert( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPUpdate ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.Update , strTableName ) ) );
            strBuilder.Append( GenInsertUpdateParams( SPType.Update , strTableName ) );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenUpdate( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }

        #endregion

        public static String GenSPSelect ( String strTableName )
        {
            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strIDCol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.Select , strTableName ) ) );
        
            strBuilder.Append( Tab+GenParam( strTableName , strIDCol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenSelectAllByID( strTableName , false , true ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPSelectByName ( String strTableName )
        {
            String strNAMECol=DataStructureProvider.GetNAMEColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strNAMECol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.SelectByName , strTableName ) ) );
          
            strBuilder.Append( Tab+GenParam( strTableName , strNAMECol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenSelectAllByName( strTableName , false , true ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPSelectByNo ( String strTableName )
        {
            String strNOCol=DataStructureProvider.GetNOColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strNOCol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.SelectByNo , strTableName ) ) );

           strBuilder.Append( Tab+GenParam( strTableName , strNOCol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenSelectAllByNo( strTableName , false , true ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPSelectAll ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.SelectAll , strTableName ) ) );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenSelectAll( strTableName,false,true ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static void GenSPSelectByFK ( String strTableName )
        {
        
            Dictionary<String , String> lstFkColumns=DataStructureProvider.GetAllTableForeignColumns( strTableName );
            foreach ( String strCol in lstFkColumns.Keys )
            {
                StringBuilder strBuilder=new StringBuilder();

                strBuilder.Append( GenSPHeader( GetSPName( SPType.SelectByFK , strTableName , strCol ) ) );

                strBuilder.Append( Tab+GenParam( strTableName , strCol )+NewLine );

                strBuilder.Append( GenSPBeginQuery() );

                strBuilder.Append( QueryTemplateGenerator.GenSelectAllByColumn( strTableName , strCol,false,true ) );

                strBuilder.Append( GenSPAfterQuery() );

                DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );
            }

        }
        public static String GenSPSelectDeleted ( String strTableName )
        {
            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strIDCol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.Select , strTableName ) ) );
         
            strBuilder.Append( Tab+GenParam( strTableName , strIDCol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenSelectAllDeletedRecords( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }

        public static String GenSPDelete ( String strTableName )
        {
            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strIDCol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( GenSPHeader( GetSPName( SPType.Delete , strTableName ) ) );

            strBuilder.Append( Tab+GenParam( strTableName , strIDCol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenDeleteByID( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPDeleteAll ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.DeleteAll , strTableName ) ) );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenDeleteByID( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static void GenSPDeleteByFK ( String strTableName )
        {
           
            Dictionary<String , String> lstFkColumns=DataStructureProvider.GetAllTableForeignColumns( strTableName );
            foreach ( String strCol in lstFkColumns.Keys )
            {
                StringBuilder strBuilder=new StringBuilder();

                strBuilder.Append( GenSPHeader( GetSPName( SPType.DeleteByFK , strTableName , strCol ) ) );

                strBuilder.Append( Tab+GenParam( strTableName , strCol )+NewLine );

                strBuilder.Append( GenSPBeginQuery() );

                strBuilder.Append( QueryTemplateGenerator.GenDeleteByColumn( strTableName , strCol ) );

                strBuilder.Append( GenSPAfterQuery() );

                DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );
            }

        }

        public static String GenSPRealDelete ( String strTableName )
        {
            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strIDCol ) )
                return String.Empty;

            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( GenSPHeader( GetSPName( SPType.RealDelete, strTableName ) ) );

            strBuilder.Append( Tab+GenParam( strTableName , strIDCol )+NewLine );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenRealDeleteByID( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static String GenSPRealDeleteAll ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            strBuilder.Append( GenSPHeader( GetSPName( SPType.RealDeleteAll , strTableName ) ) );

            strBuilder.Append( GenSPBeginQuery() );

            strBuilder.Append( QueryTemplateGenerator.GenRealDeleteByID( strTableName ) );

            strBuilder.Append( GenSPAfterQuery() );

            DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );

            return strBuilder.ToString().Trim();
        }
        public static void GenSPRealDeleteByFK ( String strTableName )
        {
        
            Dictionary<String , String> lstFkColumns=DataStructureProvider.GetAllTableForeignColumns( strTableName );
            foreach ( String strCol in lstFkColumns.Keys )
            {
                StringBuilder strBuilder=new StringBuilder();

                strBuilder.Append( GenSPHeader( GetSPName( SPType.RealDeleteByFK , strTableName , strCol ) ) );

                strBuilder.Append( Tab+GenParam( strTableName , strCol )+NewLine );

                strBuilder.Append( GenSPBeginQuery() );

                strBuilder.Append( QueryTemplateGenerator.GenRealDeleteByColumn( strTableName , strCol ) );

                strBuilder.Append( GenSPAfterQuery() );

                DataQueryProvider.ExecuteScript( strBuilder.ToString().Trim() );
            }

        }

        #endregion

        #region  private


        #region Generate SP Name
        public enum SPType
        {
            Insert=0 ,
            Update=1 ,

            Delete=2 ,
            DeleteAll=3 ,
            DeleteByFK=4 ,

            RealDelete=5 ,
            RealDeleteAll=6 ,
            RealDeleteByFK=7 ,

            Select=8 ,
            SelectByNo=9 ,
            SelectByName=10 ,
            SelectAll=11 ,
            SelectByFK=12 ,

            SelectDeleted=13
        }

        public static List<String> GetSPNameList ( String strTableName )
        {
            List<String> lstResult=new List<string>();
            lstResult.Add( GetSPName( SPType.Insert , strTableName ) );
            lstResult.Add( GetSPName( SPType.Update , strTableName ) );
            lstResult.Add( GetSPName( SPType.Delete , strTableName ) );
            lstResult.Add( GetSPName( SPType.DeleteAll , strTableName ) );
            lstResult.Add( GetSPName( SPType.RealDelete , strTableName ) );
            lstResult.Add( GetSPName( SPType.RealDeleteAll , strTableName ) );
            lstResult.Add( GetSPName( SPType.Select , strTableName ) );
            lstResult.Add( GetSPName( SPType.SelectByNo , strTableName ) );
            lstResult.Add( GetSPName( SPType.SelectByName , strTableName ) );
            lstResult.Add( GetSPName( SPType.SelectAll , strTableName ) );
            lstResult.Add( GetSPName( SPType.SelectDeleted , strTableName ) );

            Dictionary<String , String> lstFkColumns=DataStructureProvider.GetAllTableForeignColumns( strTableName );
            if ( lstFkColumns==null )
                return null;
            foreach ( String strCol in lstFkColumns.Keys )
            {
                lstResult.Add( GetSPName( SPType.SelectByFK , strTableName , strCol ) );
                lstResult.Add( GetSPName( SPType.DeleteByFK , strTableName , strCol ) );
                lstResult.Add( GetSPName( SPType.RealDeleteByFK , strTableName , strCol ) );
                if ( DataStructureProvider.IsSystemTable( strTableName )==false )
                    lstResult.Add( GetSPName( SPType.RealDeleteByFK , strTableName , strCol ) );
            }

            return lstResult;
        }

        public static String GetSPName ( SPType type , String strTableName )
        {
            if ( type==SPType.Insert )
                return String.Format( "{0}_Insert" , strTableName );
            if ( type==SPType.Update )
                return String.Format( "{0}_Update" , strTableName );
            if ( type==SPType.Delete )
                return String.Format( "{0}_Delete" , strTableName );
            if ( type==SPType.DeleteAll )
                return String.Format( "{0}_DeleteAll" , strTableName );
            if ( type==SPType.RealDelete )
                return String.Format( "{0}_RealDelete" , strTableName );
            if ( type==SPType.RealDeleteAll )
                return String.Format( "{0}_RealDeleteAll" , strTableName );
            if ( type==SPType.Select )
                return String.Format( "{0}_Select" , strTableName );
            if ( type==SPType.SelectByNo )
                return String.Format( "{0}_SelectByNo" , strTableName );
            if ( type==SPType.SelectByName )
                return String.Format( "{0}_SelectByName" , strTableName );
            if ( type==SPType.SelectAll )
                return String.Format( "{0}_SelectAll" , strTableName );
            if ( type==SPType.SelectDeleted )
                return String.Format( "{0}_SelectDeleted" , strTableName );
            return String.Empty;
        }
        public static String GetSPName ( SPType type , String strTableName , String strCol )
        {

            if ( type==SPType.DeleteByFK )
                return String.Format( "{0}_DeleteBy{1}" , strTableName , strCol );

            if ( type==SPType.RealDeleteByFK )
                return String.Format( "{0}_RealDeleteBy{1}" , strTableName , strCol );

            if ( type==SPType.SelectByFK )
                return String.Format( "{0}_SelectBy{1}" , strTableName , strCol );

            return String.Empty;
        }
        #endregion

        private static String GenSPHeader ( String strSPName )
        {
            StringBuilder strSPHeaderBuilder=new StringBuilder();
            strSPHeaderBuilder.Append( "-- ======================================================================" );
            strSPHeaderBuilder.Append( NewLine );
            strSPHeaderBuilder.Append( "-- Generated By: ABC Studio" );
            strSPHeaderBuilder.Append( NewLine );
            strSPHeaderBuilder.Append( String.Format( "-- Procedure Name:{0}" , strSPName ) );
            strSPHeaderBuilder.Append( NewLine );
            strSPHeaderBuilder.Append( String.Format( "-- Generate date:{0}" , DateTime.Now.ToString( "dd/MM/yyyy hh:ss" ) ) );
            strSPHeaderBuilder.Append( NewLine );
            strSPHeaderBuilder.Append( "-- ======================================================================" );
            strSPHeaderBuilder.Append( NewLine );

            strSPHeaderBuilder.Append( String.Format( "CREATE PROCEDURE [{0}]" , strSPName ) );
            strSPHeaderBuilder.Append( NewLine );
            return strSPHeaderBuilder.ToString();
        }

        private static String GenSPBeginQuery ( )
        {
            StringBuilder strBeginQueryBuilder=new StringBuilder();
            strBeginQueryBuilder.Append( NewLine+"AS"+NewLine+"BEGIN"+NewLine+"SET NOCOUNT ON"+NewLine );
            return strBeginQueryBuilder.ToString();
        }
        private static String GenSPAfterQuery ( )
        {
            StringBuilder strAfterQueryBuilder=new StringBuilder();
            strAfterQueryBuilder.Append( NewLine+"END"+NewLine );
            return strAfterQueryBuilder.ToString();
        }

        private static String GenParam ( String strTableName , String strCol )
        {
            String strDBType=DataStructureProvider.GetColumnDbType( strTableName , strCol );

            return String.Format( "@{0} {1}" , strCol , strDBType );
        }


        #endregion

    }
    public class QueryGenerator : BaseGenerator
    {
        #region Utils
        public static String FilterWithApprovedRecords ( String strTableName , String strQuery )
        {
            if ( DataStructureProvider.IsExistApprovalStatus( strTableName )==false )
                return strQuery;

            return AddCondition( strQuery , String.Format( " {0}='{1}'" , ABCCommon.ABCConstString.colApprovalStatus, ABCCommon.ABCConstString.ApprovalTypeApproved ) );
        }
        public static String FilterWithAliveRecords ( String strTableName , String strQuery )
        {
            if ( DataStructureProvider.IsExistABCStatus( strTableName )==false )
                return strQuery;

            return AddCondition( strQuery , String.Format( " {0}='{1}'" , ABCCommon.ABCColumnType.ABCStatus , ABCCommon.ABCConstString.ABCStatusAlive ) );
        }
        public static String FilterWithDeletedRecords ( String strTableName , String strQuery )
        {
            if ( DataStructureProvider.IsExistABCStatus( strTableName )==false )
                return strQuery;

            return AddCondition( strQuery , String.Format( " {0}='{1}'" , ABCCommon.ABCColumnType.ABCStatus , ABCCommon.ABCConstString.ABCStatusDeleted) );
        }
        
        public static String AddCondition ( String strQuery , string strCondition )
        {
            if ( String.IsNullOrWhiteSpace( strCondition ) )
                return strQuery;

            if ( strQuery.Contains( "WHERE" ) )
                strQuery+=String.Format( " AND {0}" , strCondition );
            else
                strQuery+=String.Format( " WHERE {0}" , strCondition );
            return strQuery;
        }
        public static String AddEqualCondition ( String strQuery , String strFieldName , object objValue )
        {
            if ( String.IsNullOrWhiteSpace( strFieldName ) )
                return strQuery;

            if ( objValue.GetType()==typeof( bool )||objValue.GetType()==typeof( string )||objValue.GetType()==typeof( DateTime )||objValue.GetType()==typeof( Guid )||objValue.GetType()==typeof( Nullable<Guid> ) )
                return AddCondition( strQuery , String.Format( "{0}='{1}'" , strFieldName , objValue.ToString() ) );
            else
                return AddCondition( strQuery , String.Format( "{0}={1}" , strFieldName , objValue.ToString() ) );
        }

        public static String GenerateCondition ( String strTableName , ABCCommon.ABCColumnType type )
        {
            if ( type==ABCCommon.ABCColumnType.ID )
                return Tab+String.Format( "[{0}]=@{0}" , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) );
            if ( type==ABCCommon.ABCColumnType.NO )
                return Tab+String.Format( "[{0}]=@{0}" , DataStructureProvider.GetNOColumn( strTableName ) );
            if ( type==ABCCommon.ABCColumnType.NAME )
                return Tab+String.Format( "[{0}]=@{0}" , DataStructureProvider.GetNAMEColumn( strTableName ) );
            if ( type==ABCCommon.ABCColumnType.ABCStatus )
                return Tab+String.Format( "[{0}]='{1}'" , ABCCommon.ABCConstString.colABCStatus , ABCCommon.ABCConstString.ABCStatusAlive );

            return String.Empty;
        }
        public static String GenerateCondition ( String strTableName , params string[] strColumNames )
        {
            String strCondition=String.Empty;
            for ( int i=0; i<strColumNames.Length; i++ )
            {
                if ( i>0 )
                    strCondition+=( Tab+" AND " );
                strCondition+=( Tab+String.Format( "[{0}]=@{0}" , strColumNames[i] ) );
            }
            return strCondition;
        }
        #endregion

      

        public static String GenSelect ( String strTableName , String strReturnFields , bool approvedOnly )
        {
            return GenSelect( strTableName , strReturnFields , approvedOnly , true );
        }
        public static String GenSelect ( String strTableName , String strReturnFields , bool approvedOnly , bool aliveOnly )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( "SELECT"+NewLine );
            strBuilder.Append( Tab+strReturnFields+NewLine );
            strBuilder.Append( "FROM"+NewLine );
            strBuilder.Append( Tab+String.Format( "[dbo].[{0}]" , strTableName )+NewLine );

            String strQuery=strBuilder.ToString();
            if ( aliveOnly )
                strQuery=FilterWithAliveRecords( strTableName , strQuery );
            if ( approvedOnly )
                strQuery=FilterWithApprovedRecords( strTableName , strQuery );
            return strQuery;
        }
        public static String GenSelectAll ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            return GenSelect( strTableName , "*" , approvedOnly , aliveOnly );
        }
        public static String GenSelectAll ( String strTableName , bool approvedOnly )
        {
            return GenSelect( strTableName , "*" , approvedOnly , true );
        }
        public static String GenSelectIDs ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            return GenSelect( strTableName , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) , approvedOnly , aliveOnly );
        }
        public static String GenSelectIDs ( String strTableName , bool approvedOnly )
        {
            return GenSelect( strTableName , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) , approvedOnly , true );
        }
     
        public static String GenSelectByID ( String strTableName , Guid id )
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByID( strTableName,false,false );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) ) , "'"+id.ToString()+"'" );
        }
        public static String GenSelectByName ( String strTableName , String strName , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByName( strTableName , approvedOnly , aliveOnly );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetNAMEColumn( strTableName ) ) , "'"+strName+"'" );
        }
        public static String GenSelectByName ( String strTableName , String strName , bool approvedOnly)
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByName( strTableName , approvedOnly , true );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetNAMEColumn( strTableName ) ) , "'"+strName+"'" );
        }
        public static String GenSelectByNo ( String strTableName , String strNo , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByNo( strTableName , approvedOnly , aliveOnly );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetNOColumn( strTableName ) ) , "'"+strNo+"'" );
        }
        public static String GenSelectByNo ( String strTableName , String strNo , bool approvedOnly)
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByNo( strTableName , approvedOnly , true );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetNOColumn( strTableName ) ) , "'"+strNo+"'" );
        }
        public static String GenSelectByColumn ( String strTableName , String strCol , object objValue , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByColumn( strTableName , strCol , approvedOnly , aliveOnly );

            if ( objValue.GetType()==typeof( bool )||objValue.GetType()==typeof( string )||objValue.GetType()==typeof( DateTime )||objValue.GetType()==typeof( Guid )||objValue.GetType()==typeof( Nullable<Guid> ) )
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , "'"+objValue.ToString()+"'" );
            else
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , objValue.ToString() );
        }
        public static String GenSelectByColumn ( String strTableName , String strCol , object objValue , bool approvedOnly  )
        {
            String strQuery=QueryTemplateGenerator.GenSelectAllByColumn( strTableName , strCol ,approvedOnly , true );
            if ( objValue.GetType()==typeof( bool )||objValue.GetType()==typeof( string )||objValue.GetType()==typeof( DateTime )||objValue.GetType()==typeof( Guid )||objValue.GetType()==typeof( Nullable<Guid> ) )
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , "'"+objValue.ToString()+"'" );
            else
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , objValue.ToString() );
        }

        public static String GenDeleteAll ( String strTableName )
        {
            return GenDeleteAll( strTableName , false , true );
        }
        public static String GenDeleteAll ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            StringBuilder strBuilder=new StringBuilder();

            if ( DataStructureProvider.IsExistABCStatus( strTableName ) )
            {
                strBuilder.Append( String.Format( "UPDATE [{0}]" , strTableName )+NewLine );
                strBuilder.Append( String.Format( "SET [{0}]='{1}'" , ABCCommon.ABCConstString.colABCStatus , ABCCommon.ABCConstString.ABCStatusDeleted ) );
                if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colUpdateTime ) )
                {
                    if ( DataQueryProvider.IsCompanySQLConnection )
                        strBuilder.Append( String.Format( " , [{0}] = GETDATE()" , ABCCommon.ABCConstString.colUpdateTime )+NewLine );
                    else
                        strBuilder.Append( String.Format( " , [{0}] = DATETIME('now', 'localtime')" , ABCCommon.ABCConstString.colUpdateTime )+NewLine );
                }
            }
            else
            {
                strBuilder.Append( String.Format( "DELETE FROM {0}" , strTableName )+NewLine );
            }


            String strQuery=strBuilder.ToString();
            if ( aliveOnly )
                strQuery=FilterWithAliveRecords( strTableName , strQuery );
            if ( approvedOnly )
                strQuery=FilterWithApprovedRecords( strTableName , strQuery );
            return strQuery;
        }
        public static String GenDeleteByID ( String strTableName , Guid id  )
        {
            String strQuery=QueryTemplateGenerator.GenDeleteByID( strTableName  );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) ) ,"'"+ id.ToString()+"'" );
        }
        public static String GenDeleteByColumn ( String strTableName , String strCol , object objValue  )
        {
            String strQuery=QueryTemplateGenerator.GenDeleteByColumn( strTableName , strCol  );
            if ( objValue.GetType()==typeof( bool )||objValue.GetType()==typeof( string )||objValue.GetType()==typeof( DateTime )||objValue.GetType()==typeof( Guid )||objValue.GetType()==typeof( Nullable<Guid> ) )
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , "'"+objValue.ToString()+"'" );
            else
                return strQuery.Replace( String.Format( "@{0}" , strCol ) , objValue.ToString() );
        }

        public static String GenRealDeleteAll ( String strTableName )
        {
            return GenRealDeleteAll( strTableName , false , true );
        }
        public static String GenRealDeleteAll ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=String.Format( "DELETE FROM {0}" , strTableName );
            if ( aliveOnly )
                strQuery=FilterWithAliveRecords( strTableName , strQuery );
            if ( approvedOnly )
                strQuery=FilterWithApprovedRecords( strTableName , strQuery );
            return strQuery;
        }
        public static String GenRealDeleteByID ( String strTableName , Guid id )
        {
            String strQuery=QueryTemplateGenerator.GenRealDeleteByID( strTableName );
            return strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) ) ,"'"+ id.ToString()+"'" );
        }
        public static String GenRealDeleteByColumn ( String strTableName , String strCol , object objValue )
        {
            String strQuery=QueryTemplateGenerator.GenRealDeleteByColumn( strTableName , strCol );
            return strQuery.Replace( String.Format( "@{0}" , strCol ) ,"'"+ objValue.ToString() +"'");
        }
    }

    public class QueryTemplateGenerator : BaseGenerator
    {
        public static String GenInsert ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            #region Generate Query
            strBuilder.Append( String.Format( "INSERT INTO [{0}](" , strTableName )+NewLine );

            #region  Columns
            Dictionary<String , TableColumnData> lstColumns=DataStructureProvider.GetAllTableColumns( strTableName );

            int i=0;
            foreach ( String strCol in lstColumns.Keys )
            {
                strBuilder.Append( Tab );
                strBuilder.Append( String.Format( "[{0}]" , strCol ) );

                if ( i<lstColumns.Count-1 )
                    strBuilder.Append( "," );
                strBuilder.Append( NewLine );

                i++;
            }
            #endregion

            strBuilder.Append( ") VALUES ( "+NewLine );

            #region Values
            i=0;
            foreach ( String strCol in lstColumns.Keys )
            {
                strBuilder.Append( Tab );
                if ( strCol==ABCCommon.ABCConstString.colCreateTime||strCol==ABCCommon.ABCConstString.colUpdateTime )
                {
                    if ( DataQueryProvider.IsCompanySQLConnection )
                        strBuilder.Append( "GetDate() " );
                    else
                        strBuilder.Append( "DATETIME('now', 'localtime') " );
                }
                else
                    strBuilder.Append( String.Format( "@{0}" , strCol ) );

                if ( i<lstColumns.Count-1 )
                    strBuilder.Append( "," );
                strBuilder.Append( NewLine );

                i++;
            }
            #endregion

            strBuilder.Append( ")"+NewLine );
            #endregion

            return strBuilder.ToString();
        }
        public static String GenUpdate ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();

            #region Generate Query
            strBuilder.Append( String.Format( "UPDATE [{0}] SET" , strTableName )+NewLine );

            #region  Columns
            Dictionary<String , TableColumnData> lstColumns=DataStructureProvider.GetAllTableColumns( strTableName );

            int i=0;
            foreach ( String strCol in lstColumns.Keys )
            {
                if ( !DataStructureProvider.IsPrimaryKey( strTableName , strCol ) )
                {
                    strBuilder.Append( Tab );
                    if ( strCol==ABCCommon.ABCConstString.colUpdateTime )
                    {
                        if ( DataQueryProvider.IsCompanySQLConnection )
                            strBuilder.Append( String.Format( "[{0}]=GetDate()" , strCol ) );
                        else
                            strBuilder.Append( String.Format( "[{0}]=DATETIME('now', 'localtime')" , strCol ) );
                    }
                    else
                        strBuilder.Append( String.Format( "[{0}]=@{0}" , strCol ) );

                    if ( i<lstColumns.Count-1 )
                        strBuilder.Append( "," );
                    strBuilder.Append( NewLine );
                }

                i++;
            }
            #endregion

            strBuilder.Append( "WHERE "+NewLine+Tab+QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.ID ) );

            strBuilder.Append( NewLine );
            #endregion

            return strBuilder.ToString();
        }

        public static String GenSelectAll ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            return QueryGenerator.GenSelectAll( strTableName , approvedOnly , aliveOnly );
        }
        public static String GenSelectByID ( String strTableName , String strReturnFields , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryGenerator.GenSelect( strTableName , strReturnFields , approvedOnly , aliveOnly );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.ID ) );
            return strQuery;
        }

        public static String GenSelectAllByID ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryGenerator.GenSelectAll( strTableName , approvedOnly , aliveOnly );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.ID ) );
            return strQuery;
        }
        public static String GenSelectAllByName ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryGenerator.GenSelectAll( strTableName , approvedOnly , aliveOnly );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.NAME ) );
            return strQuery;
        }
        public static String GenSelectAllByNo ( String strTableName , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryGenerator.GenSelectAll( strTableName , approvedOnly , aliveOnly );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.NO ) );
            return strQuery;
        }
        public static String GenSelectAllByColumn ( String strTableName , string strCol , bool approvedOnly , bool aliveOnly )
        {
            String strQuery=QueryGenerator.GenSelectAll( strTableName , approvedOnly,aliveOnly );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , strCol ) );
            return strQuery;
        }
        public static String GenSelectAllDeletedRecords ( String strTableName )
        {
            String strQuery=GenSelectAll( strTableName ,false,false);
            strQuery=QueryGenerator.FilterWithDeletedRecords( strTableName , strQuery );
            return strQuery;
        }

        public static String GenDeleteByID ( String strTableName )
        {
            String strQuery=QueryGenerator.GenDeleteAll( strTableName );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.ID ) );
            return strQuery;
        }
        public static String GenDeleteByColumn ( String strTableName , String strCol )
        {
            String strQuery=QueryGenerator.GenDeleteAll( strTableName );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , strCol ) );
            return strQuery;
        }
     
        public static String GenRealDeleteByID ( String strTableName )
        {
            String strQuery=QueryGenerator.GenRealDeleteAll( strTableName );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , ABCCommon.ABCColumnType.ID ) );
            return strQuery;
        }
        public static String GenRealDeleteByColumn ( String strTableName , String strCol )
        {
            String strQuery=QueryGenerator.GenRealDeleteAll( strTableName );
            strQuery=QueryGenerator.AddCondition( strQuery , QueryGenerator.GenerateCondition( strTableName , strCol ) );
            return strQuery;
        }

      
    }

    public class BusinessObjectGenerator : BaseGenerator
    {
        public static String GenerateHeaderUsing ( )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( "using System;"+NewLine );
            strBuilder.Append( "using System.Text;"+NewLine );
            strBuilder.Append( "using System.Collections.Generic;"+NewLine );
            strBuilder.Append( "using ABCProvider;"+NewLine );
            return strBuilder.ToString();
        }
    
        #region  Info

        public static String GenerateInfoClass ( String strTableName,bool isIncludeUsing )
        {
            StringBuilder strBuilder=new StringBuilder();

            if ( isIncludeUsing )
                strBuilder.Append( GenerateHeaderUsing() );

            strBuilder.Append( GenClassInfoHeader( strTableName ) );

            strBuilder.Append( GenClassInfoConstructor( strTableName ) );

            strBuilder.Append( Tab+Tab+"#region Variables"+NewLine );
            strBuilder.Append( GenVariables( strTableName ) );
            strBuilder.Append( Tab+Tab+"#endregion"+NewLine+NewLine );

            strBuilder.Append( Tab+Tab+"#region Public properties"+NewLine );
            strBuilder.Append( GenerateProperties( strTableName ) );
            strBuilder.Append( Tab+Tab+"#endregion"+NewLine );

            strBuilder.Append( GenClassInfoFooter() );

            return strBuilder.ToString();

        }

        #region private
        private static String GenClassInfoHeader ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();          
              
            strBuilder.Append( "namespace ABCBusinessEntities" );          

          
            strBuilder.Append( NewLine+"{"+NewLine );
            strBuilder.Append( Tab+String.Format( "#region {0}" , strTableName )+NewLine );
            strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
            strBuilder.Append( Tab+"//Generated By: ABC Studio"+NewLine );
            strBuilder.Append( Tab+String.Format( "//Class:{0}Info" , strTableName )+NewLine );
            strBuilder.Append( Tab+String.Format( "//Created Date:{0}" , DateTime.Today.ToLongDateString() )+NewLine );
            strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
            strBuilder.Append( Tab+NewLine );

            strBuilder.Append( Tab+String.Format( "public class {0}Info:BusinessObject" , strTableName )+NewLine );

            strBuilder.Append( Tab+"{"+NewLine );

            return strBuilder.ToString();
        }
        private static String GenClassInfoConstructor ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( Tab+Tab+String.Format( "public {0}Info()" , strTableName )+NewLine );
            strBuilder.Append( Tab+Tab+"{"+NewLine );
            strBuilder.Append( Tab+Tab+String.Format( "AATableName =  \"{0}\"; " , strTableName )+NewLine );
            strBuilder.Append( Tab+Tab+"}"+NewLine );

            return strBuilder.ToString();
        }
        private static String GenClassInfoFooter ( )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( Tab+"}"+NewLine );
            strBuilder.Append( Tab+"#endregion"+NewLine );
            strBuilder.Append( "}" );
            return strBuilder.ToString();
        }

        #region Variable
        private static String GenVariables ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();
            Dictionary<String , TableColumnData> lstColumns=DataStructureProvider.GetAllTableColumns( strTableName );
            foreach ( String strCol in lstColumns.Keys )
            {

                strBuilder.Append( Tab+Tab+String.Format( "protected {0} " , DataStructureProvider.GetCodingType( strTableName , strCol ) ) );

                String strDefaultValue=GenVariableDefautValue( strTableName , strCol );

                if ( !String.IsNullOrWhiteSpace( strDefaultValue ) )
                    strBuilder.Append( GenVariable( strCol )+"="+strDefaultValue+";" );
                else
                    strBuilder.Append( GenVariable( strCol )+";" );

                strBuilder.Append( NewLine );

            }

            return strBuilder.ToString();
        }
        private static String GenVariableDefautValue ( String strTableName , String strCol )
        {
            string strCSharpVar=DataStructureProvider.GetCodingType( strTableName , strCol );

            if ( strCSharpVar=="String" )
            {
                if (strCol.Equals( ABCCommon.ABCConstString.colABCStatus ) )
                    return "ABCCommon.ABCConstString.ABCStatusAlive";
                else
                    return "String.Empty";

            }

            if ( strCSharpVar=="DateTime" )
                return "ABCApp.ABCDataGlobal.WorkingDate";


            if ( strCSharpVar=="bool" )
                return "true";

            return String.Empty;
        }
        private static String GenVariable ( String strCol )
        {
            return "_"+strCol.Substring( 0 , 1 ).ToLower()+strCol.Substring( 1 );
        }
        #endregion

        #region Property
        private static String GenerateProperties ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();
            Dictionary<String , TableColumnData> lstColumns=DataStructureProvider.GetAllTableColumns( strTableName );
            foreach ( String strCol in lstColumns.Keys )
            {
                strBuilder.Append( Tab+Tab+String.Format( "public {0} {1}" , DataStructureProvider.GetCodingType( strTableName , strCol ) , strCol ) );

                strBuilder.Append( NewLine );
                strBuilder.Append( Tab+Tab+"{"+NewLine );

                //Generate Get Property
                strBuilder.Append( Tab+Tab+Tab+GenGetProperty( strTableName , strCol ) );
                strBuilder.Append( NewLine );

                //Generate Set Property
                strBuilder.Append( Tab+Tab+Tab+GenSetProperty( strTableName , strCol ) );
                strBuilder.Append( NewLine );
                strBuilder.Append( Tab+Tab+"}" );
                strBuilder.Append( NewLine );
            }

            return strBuilder.ToString();
        }
        private static String GenGetProperty ( String strTableName , String strCol )
        {
            return "get{return "+GenVariable( strCol )+";}";
        }
        private static String GenSetProperty ( String strTableName , String strCol )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( "set"+NewLine );
            strBuilder.Append( Tab+Tab+Tab+"{"+NewLine );
            strBuilder.Append( Tab+Tab+Tab+Tab+String.Format( "if (value != this.{0})" , GenVariable( strCol ) ) );
            strBuilder.Append( NewLine );
            strBuilder.Append( Tab+Tab+Tab+Tab+"{"+NewLine );
            strBuilder.Append( Tab+Tab+Tab+Tab+String.Format( "{0}=value;" , GenVariable( strCol ) ) );
            strBuilder.Append( NewLine );

            strBuilder.Append( Tab+Tab+Tab+Tab+"NotifyChanged("+"\""+strCol+"\""+");" );
            strBuilder.Append( NewLine );

            strBuilder.Append( Tab+Tab+Tab+Tab+"}" );
            strBuilder.Append( NewLine );
            strBuilder.Append( Tab+Tab+Tab+"}" );

            return strBuilder.ToString();
        }
        #endregion

        #endregion

        #endregion

        #region  Controller
        public static String GenerateControllerClass ( String strTableName , bool isIncludeUsing )
        {
            StringBuilder strClassBuilder=new StringBuilder();

            if ( isIncludeUsing )
                strClassBuilder.Append( GenerateHeaderUsing() );

            strClassBuilder.Append( GenClassControllerHeader( strTableName ) );

            strClassBuilder.Append( GenClassControllerConstructor( strTableName ) );

            strClassBuilder.Append( GenClassControllerFooter() );

            return strClassBuilder.ToString();
        }

        #region private
        private static String GenClassControllerHeader ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();               
            strBuilder.Append( "namespace ABCBusinessEntities" );        
            
            strBuilder.Append( NewLine+"{"+NewLine );
            strBuilder.Append( Tab+String.Format( "#region {0}" , strTableName )+NewLine );
            strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
            strBuilder.Append( Tab+"//Generated By: ABC Studio"+NewLine );
            strBuilder.Append( Tab+String.Format( "//Class:{0}Controller" , strTableName )+NewLine );
            strBuilder.Append( Tab+String.Format( "//Created Date:{0}" , DateTime.Today.ToLongDateString() )+NewLine );
            strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
            strBuilder.Append( Tab+NewLine );

            strBuilder.Append( Tab+String.Format( "public class {0}Controller:BusinessObjectController" , strTableName )+NewLine );

            strBuilder.Append( Tab+"{"+NewLine );

            return strBuilder.ToString();
        }
        private static String GenClassControllerConstructor ( String strTableName )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( Tab+Tab+String.Format( "public {0}Controller()" , strTableName )+NewLine );
            strBuilder.Append( Tab+Tab+"{"+NewLine );
            strBuilder.Append( Tab+Tab+Tab+"TableName=" + "\"" +strTableName + "\"" +" ; " +NewLine );
            strBuilder.Append( Tab+Tab+"}"+NewLine );

            return strBuilder.ToString();
        }
        private static String GenClassControllerFooter ( )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( Tab+"}"+NewLine );
            strBuilder.Append( Tab+"#endregion"+NewLine );
            strBuilder.Append( "}" );
            return strBuilder.ToString();
        }
        #endregion

        #endregion


        public static void GenerateToBusinessObjectDLL ( )
        {
            StringBuilder strClassBuilder=new StringBuilder();

            strClassBuilder.Append( GenerateHeaderUsing()+NewLine );
            foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
            {
                if ( DataStructureProvider.IsSystemTable( strTableName )==false )
                {
                    strClassBuilder.Append( GenerateInfoClass( strTableName , false )+NewLine );
                    strClassBuilder.Append( GenerateControllerClass( strTableName , false )+NewLine );
                }
            }

            CompileProvider.CompiledAssembly ass=(CompileProvider.CompiledAssembly)CompileProvider.Compiler.CompileAssembly( strClassBuilder.ToString() , CompileProvider.CodeType.CSharp , @"BusinessObjects.dll" , new string[] { "System.dll" , "ABCDataLib.dll" , "ABCCommon.dll" } );
        }

        public static void GenerateToBaseObjectDLL ( )
        {
            StringBuilder strClassBuilder=new StringBuilder();

            strClassBuilder.Append( GenerateHeaderUsing()+NewLine );
            foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
            {
                if ( DataStructureProvider.IsSystemTable( strTableName ) )
                {
                    strClassBuilder.Append( GenerateInfoClass( strTableName , false )+NewLine );
                    strClassBuilder.Append( GenerateControllerClass( strTableName , false )+NewLine );
                }
            }

            CompileProvider.CompiledAssembly ass=(CompileProvider.CompiledAssembly)CompileProvider.Compiler.CompileAssembly( strClassBuilder.ToString() , CompileProvider.CodeType.CSharp , @"BaseObjects.dll" , new string[] { "System.dll" , "ABCDataLib.dll" , "ABCCommon.dll" } );
        }

    }
}