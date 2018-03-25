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
using ABCBusinessEntities;
using ABCProvider;
using ABCProvider;
using System.Xml;
using System.Text.RegularExpressions;

namespace ABCProvider
{
   public class NumberingProvider
    {
        #region Configuration
    
       public class Numbering
        {
            public Guid ID { get; set; }
            public String TableName { get; set; }

            public String ConditionString { get; set; }
            public String FieldCondition { get; set; }
            public String FieldValue { get; set; }
           
           public String SeperateChar { get; set; }
            public String Prefix { get; set; }
            public String Suffix { get; set; }
            public bool IsUsePattern { get; set; }
            public String NumberPattern { get; set; }

            public List<NumberingType> MiddleConfigs { get; set; }
        }
        public class NumberingType
        {
            public Guid ID { get; set; }
            public String PatternType { get; set; }

            public bool IsByUser { get; set; }
            public bool IsByUserGroup { get; set; }
            public bool IsByEmployee { get; set; }
            public bool IsByCompanyUnit { get; set; }
            public bool IsYYMMCount { get; set; }
            public bool IsYYMMDDCount { get; set; }
            public bool IsMMDDCount { get; set; }
            public int CountingSpace { get; set; }
            
            public bool IsByField { get; set; }
            public String FieldName { get; set; }
        }

        public static List<Numbering> NumberingConfigs;
        public static Dictionary<Guid , NumberingType> NumberingTypes;

        [ABCRefreshTable( "GENumberingTypes" , "GENumberings" , "GENumberingMiddleItems" )]
        public static void InitializeNumberings ( )
        {
            #region NumberingTypes
            NumberingTypes=new Dictionary<Guid , NumberingType>();

            DataSet ds=BusinessObjectController.RunQuery( @"SELECT * FROM GENumberingTypes" );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    if ( dr["GENumberingTypeID"]==DBNull.Value )
                        continue;

                    NumberingType type=new NumberingType();

                    #region type
                    type.ID=ABCHelper.DataConverter.ConvertToGuid( dr["GENumberingTypeID"] );
                    if ( type.ID==Guid.Empty )
                        continue;

                    if ( dr["PatternType"]!=DBNull.Value )
                        type.PatternType=dr["PatternType"].ToString();

                    type.IsByUser=false;
                    if ( dr["IsByUser"]!=DBNull.Value )
                        type.IsByUser=Convert.ToBoolean( dr["IsByUser"] );

                    type.IsByUserGroup=false;
                    if ( dr["IsByUserGroup"]!=DBNull.Value )
                        type.IsByUserGroup=Convert.ToBoolean( dr["IsByUserGroup"] );

                    type.IsByEmployee=false;
                    if ( dr["IsByEmployee"]!=DBNull.Value )
                        type.IsByEmployee=Convert.ToBoolean( dr["IsByEmployee"] );

                    type.IsByCompanyUnit=false;
                    if ( dr["IsByCompanyUnit"]!=DBNull.Value )
                        type.IsByCompanyUnit=Convert.ToBoolean( dr["IsByCompanyUnit"] );

                    type.IsYYMMCount=false;
                    if ( dr["IsYYMMCount"]!=DBNull.Value )
                        type.IsYYMMCount=Convert.ToBoolean( dr["IsYYMMCount"] );

                    type.IsYYMMDDCount=false;
                    if ( dr["IsYYMMDDCount"]!=DBNull.Value )
                        type.IsYYMMDDCount=Convert.ToBoolean( dr["IsYYMMDDCount"] );

                    type.IsMMDDCount=false;
                    if ( dr["IsMMDDCount"]!=DBNull.Value )
                        type.IsMMDDCount=Convert.ToBoolean( dr["IsMMDDCount"] );

                    type.CountingSpace=2;
                    if ( dr["CountingSpace"]!=DBNull.Value )
                        type.CountingSpace=Convert.ToInt32( dr["CountingSpace"] );


                    type.IsByField=false;
                    if ( dr["IsByField"]!=DBNull.Value )
                        type.IsByField=Convert.ToBoolean( dr["IsByField"] );

                    if ( dr["FieldName"]!=DBNull.Value )
                        type.FieldName=dr["FieldName"].ToString();
                    #endregion


                    if ( !NumberingTypes.ContainsKey( type.ID ) )
                        NumberingTypes.Add( type.ID , type );
                }
            }
            #endregion

            #region NumberingConfigs
            NumberingConfigs=new List<Numbering>();

            ds=BusinessObjectController.RunQuery( @"SELECT * FROM GENumberings" );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    if ( dr["GENumberingID"]==DBNull.Value )
                        continue;

                    Numbering number=new Numbering();

                    #region Number

                    number.ID=ABCHelper.DataConverter.ConvertToGuid( dr["GENumberingID"] );
                    if ( number.ID==Guid.Empty )
                        continue;

                    if ( dr["TableName"]!=DBNull.Value )
                        number.TableName=dr["TableName"].ToString();

                    if ( dr["ConditionString"]!=DBNull.Value )
                        number.ConditionString=dr["ConditionString"].ToString();

                    if ( dr["FieldCondition"]!=DBNull.Value )
                        number.FieldCondition=dr["FieldCondition"].ToString();

                    if ( dr["FieldValue"]!=DBNull.Value )
                        number.FieldValue=dr["FieldValue"].ToString();

                    if ( dr["SeperateChar"]!=DBNull.Value )
                        number.SeperateChar=dr["SeperateChar"].ToString();

                    if ( dr["Prefix"]!=DBNull.Value )
                        number.Prefix=dr["Prefix"].ToString();

                    if ( dr["Suffix"]!=DBNull.Value )
                        number.Suffix=dr["Suffix"].ToString();

                    number.IsUsePattern=false;
                    if ( dr["IsUsePattern"]!=DBNull.Value )
                        number.IsUsePattern=Convert.ToBoolean( dr["IsUsePattern"] );

                    if ( dr["NumberPattern"]!=DBNull.Value )
                        number.NumberPattern=dr["NumberPattern"].ToString();
                    #endregion

                    #region   number.MiddleConfigs

                    number.MiddleConfigs=new List<NumberingType>();

                    List<Object> lstTypeIDs=BusinessObjectController.GetListObjects( String.Format( @"SELECT FK_GENumberingTypeID FROM GENumberingMiddleItems WHERE FK_GENumberingID ='{0}' ORDER BY MiddleIndex" , number.ID ) );
                    foreach ( Object objID in lstTypeIDs )
                    {
                        Guid typeID=ABCHelper.DataConverter.ConvertToGuid( objID );
                        if ( typeID==Guid.Empty )
                            continue;

                        if ( NumberingTypes.ContainsKey( typeID ) )
                            number.MiddleConfigs.Add( NumberingTypes[typeID] );
                    }
                    #endregion

                    NumberingConfigs.Add( number );
                }
            }
            #endregion
        }

        public static Numbering GetNumberingConfig ( BusinessObject obj )
        {
            if ( NumberingConfigs==null||NumberingTypes==null )
                InitializeNumberings();

            foreach ( Numbering config in NumberingConfigs )
            {
                if ( config.TableName!=obj.AATableName )
                    continue;

                if ( !String.IsNullOrWhiteSpace( config.FieldCondition )&&!String.IsNullOrWhiteSpace( config.FieldValue ) )
                {
                    object fileValue=ABCDynamicInvoker.GetValue( obj , config.FieldCondition );
                    if ( fileValue==null||fileValue==DBNull.Value||fileValue.ToString()!=config.FieldValue )
                        continue;
                }

                if ( !String.IsNullOrWhiteSpace( config.ConditionString ) )
                {
                    String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false );
                    strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                    strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @"{0}='{1}'" , DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName ) , obj.GetID() ) );
                    object objCount=BusinessObjectController.GetData( strQuery );
                    if ( objCount==null||objCount==DBNull.Value||Convert.ToInt32( objCount.ToString() )<=0 )
                        continue;
                }

                return config;
            }

            return null;
        }

        
        #endregion
   
       #region No
        public static String GenerateNo ( BusinessObject obj )
        {
            if ( obj.GetID()==Guid.Empty )
            {
                obj.SetNoValue( String.Empty );
                return String.Empty;
            }

            String strNoValue=String.Empty;

            Numbering numbering=GetNumberingConfig( obj );
            if ( numbering!=null )
            {
                if ( !numbering.IsUsePattern )
                {
                    #region Not UsePattern
                    if ( numbering.MiddleConfigs.Count>0 )
                    {
                        #region Have Parts
                        List<String> lstParts=new List<string>();
                        foreach ( NumberingType numberType in numbering.MiddleConfigs )
                        {
                            #region Others
                            if ( numberType.IsByUser )
                            {

                            }
                            if ( numberType.IsByUserGroup )
                            {

                            }
                            if ( numberType.IsByEmployee )
                            {

                            }
                            if ( numberType.IsByCompanyUnit )
                            {

                            }
                            #endregion

                            if ( ( numberType.IsYYMMCount||numberType.IsYYMMDDCount||numberType.IsMMDDCount ) )
                            {
                                String strDateCol=String.Empty;
                                if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateTime ) )
                                    strDateCol=ABCCommon.ABCConstString.colCreateTime;
                                if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colDocumentDate ) )
                                    strDateCol=ABCCommon.ABCConstString.colDocumentDate;

                                if ( !String.IsNullOrWhiteSpace( strDateCol ) )
                                {
                                    object objValue=ABCDynamicInvoker.GetValue( obj , strDateCol );
                                    if ( objValue!=null&&( objValue is DateTime||( objValue is Nullable<DateTime>&&( objValue as Nullable<DateTime> ).HasValue ) ) )
                                    {
                                        #region With DateTime
                                        DateTime createTime=DateTime.MinValue;
                                        if ( objValue is Nullable<DateTime> )
                                            createTime=( objValue as Nullable<DateTime> ).Value;
                                        else
                                            createTime=Convert.ToDateTime( objValue );

                                        if ( numberType.IsYYMMCount )
                                        {
                                            String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false , false );
                                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @" YEAR({0}) = {1} AND MONTH({0}) = {2} AND {0} < {3}" , strDateCol , createTime.Year , createTime.Month , TimeProvider.GenDateTimeString( createTime ) ) );
                                            int iCount=Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
                                            if ( iCount<=0 )
                                                iCount=0;
                                            iCount++;


                                            int iCountSpace=numberType.CountingSpace;
                                            if ( iCountSpace<iCount.ToString().Length )
                                                iCountSpace=iCount.ToString().Length+2;

                                            String strTemp=createTime.ToString( "yyMM" )+String.Format( "{0:D"+iCountSpace+"}" , iCount );
                                            lstParts.Add( strTemp );
                                        }
                                        if ( numberType.IsYYMMDDCount )
                                        {
                                            String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false , false );
                                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @" YEAR({0}) = {1} AND MONTH({0}) = {2} AND DAY({0}) = {3} AND {0} < {4}" , strDateCol , createTime.Year , createTime.Month , createTime.Day , TimeProvider.GenDateTimeString( createTime ) ) );
                                            int iCount=Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
                                            if ( iCount<=0 )
                                                iCount=0;
                                            iCount++;

                                            int iCountSpace=numberType.CountingSpace;
                                            if ( iCountSpace<iCount.ToString().Length )
                                                iCountSpace=iCount.ToString().Length+2;

                                            String strTemp=createTime.ToString( "yyMMdd" )+String.Format( "{0:D"+iCountSpace+"}" , iCount );
                                            lstParts.Add( strTemp );
                                        }
                                        if ( numberType.IsMMDDCount )
                                        {
                                            String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false , false );
                                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @" YEAR({0}) = {1} AND MONTH({0}) = {2} AND DAY({0}) = {3} AND {0} < {4}" , strDateCol , createTime.Year , createTime.Month , createTime.Day , TimeProvider.GenDateTimeString( createTime ) ) );
                                            int iCount=Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
                                            if ( iCount<=0 )
                                                iCount=0;
                                            iCount++;

                                            int iCountSpace=numberType.CountingSpace;
                                            if ( iCountSpace<iCount.ToString().Length )
                                                iCountSpace=iCount.ToString().Length+2;

                                            String strTemp=createTime.ToString( "MMdd" )+String.Format( "{0:D"+iCountSpace+"}" , iCount );
                                            lstParts.Add( strTemp );
                                        }
                                        #endregion
                                    }
                                }
                            }

                            #region By Field
                            if ( numberType.IsByField&&!String.IsNullOrWhiteSpace( numberType.FieldName ) )
                            {
                                if ( DataStructureProvider.IsTableColumn( obj.AATableName , numberType.FieldName ) )
                                {
                                    object objValue=ABCDynamicInvoker.GetValue( obj , numberType.FieldName );
                                    if ( objValue!=null )
                                    {
                                        if ( DataStructureProvider.IsForeignKey( obj.AATableName , numberType.FieldName ) )
                                        {
                                            String strFieldName=numberType.FieldName+":"+DataStructureProvider.GetDisplayColumn( obj.AATableName );
                                            objValue=DataCachingProvider.GetCachingObjectAccrossTable( obj , ABCHelper.DataConverter.ConvertToGuid( objValue ) , strFieldName );
                                        }

                                        lstParts.Add( objValue.ToString() );
                                    }
                                }
                            }
                            #endregion
                        }

                        strNoValue=numbering.Prefix+String.Join( numbering.SeperateChar , lstParts )+numbering.Suffix;

                        #endregion
                    }
                    else
                    {
                        String strDateCol=String.Empty;
                        if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateTime ) )
                            strDateCol=ABCCommon.ABCConstString.colCreateTime;
                        if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colDocumentDate ) )
                            strDateCol=ABCCommon.ABCConstString.colDocumentDate;

                        if ( !String.IsNullOrWhiteSpace( strDateCol ) )
                        {
                            object objValue=ABCDynamicInvoker.GetValue( obj , strDateCol );
                            if ( objValue!=null&&( objValue is DateTime||( objValue is Nullable<DateTime>&&( objValue as Nullable<DateTime> ).HasValue ) ) )
                            {
                                #region With DateTime
                                DateTime createTime=DateTime.MinValue;
                                if ( objValue is Nullable<DateTime> )
                                    createTime=( objValue as Nullable<DateTime> ).Value;
                                else
                                    createTime=Convert.ToDateTime( objValue );

                                String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false , false );
                                strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @" YEAR({0}) = {1} AND MONTH({0}) = {2} AND {0} < {3}" , strDateCol , createTime.Year , createTime.Month , TimeProvider.GenDateTimeString( createTime ) ) );
                                int iCount=Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
                                if ( iCount<=0 )
                                    iCount=0;
                                iCount++;

                                int iCountSpace=3;
                                if ( iCountSpace<iCount.ToString().Length )
                                    iCountSpace=iCount.ToString().Length+2;

                                strNoValue=numbering.Prefix+createTime.ToString( "yyMM" )+String.Format( "{0:D"+iCountSpace+"}" , iCount )+numbering.Suffix;

                                #endregion
                            }
                        }
                        else if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colNoIndex ) )
                        {
                            int iNoIndex=Convert.ToInt32( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colNoIndex ) );
                            int iCountSpace=4;
                            if ( iNoIndex>=10000 )
                                iCountSpace=iNoIndex.ToString().Length+2;
                            strNoValue=numbering.Prefix+String.Format( "{0:D"+iCountSpace+"}" , iNoIndex )+numbering.Suffix;

                        }
                    }
                    #endregion
                }
                else
                {
                    #region UsePattern

                    #endregion
                }
            }
            else
            {
                #region Have No Config
                if ( !String.IsNullOrWhiteSpace( DataConfigProvider.TableConfigList[obj.AATableName].PrefixNo ) )
                    strNoValue=DataConfigProvider.TableConfigList[obj.AATableName].PrefixNo;
                else
                    strNoValue=new Regex( "[^A-Z]+" ).Replace( DataConfigProvider.GetTableCaption( obj.AATableName ) , "" );


                String strDateCol=String.Empty;
                if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateTime ) )
                    strDateCol=ABCCommon.ABCConstString.colCreateTime;
                if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colDocumentDate ) )
                    strDateCol=ABCCommon.ABCConstString.colDocumentDate;

                if ( !String.IsNullOrWhiteSpace( strDateCol ) )
                {
                    object objValue=ABCDynamicInvoker.GetValue( obj , strDateCol );
                    if ( objValue!=null&&( objValue is DateTime||( objValue is Nullable<DateTime>&&( objValue as Nullable<DateTime> ).HasValue ) ) )
                    {
                        #region With DateTime
                        DateTime createTime=DateTime.MinValue;
                        if ( objValue is Nullable<DateTime> )
                            createTime=( objValue as Nullable<DateTime> ).Value;
                        else
                            createTime=Convert.ToDateTime( objValue );

                        String strQuery=QueryGenerator.GenSelect( obj.AATableName , "COUNT(*)" , false , false );
                        strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @" YEAR({0}) = {1} AND MONTH({0}) = {2} AND {0} < {3}" , strDateCol , createTime.Year , createTime.Month , TimeProvider.GenDateTimeString( createTime ) ) );
                        int iCount=Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
                        if ( iCount<=0 )
                            iCount=0;
                        iCount++;

                        int iCountSpace=3;
                        if ( iCountSpace<iCount.ToString().Length )
                            iCountSpace=iCount.ToString().Length+2;

                        strNoValue+=createTime.ToString( "yyMM" )+String.Format( "{0:D"+iCountSpace+"}" , iCount );

                        #endregion
                    }
                }
                else if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colNoIndex ) )
                {
                    int iNoIndex=Convert.ToInt32( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colNoIndex ) );
                    int iCountSpace=4;
                    if ( iNoIndex>=10000 )
                        iCountSpace=iNoIndex.ToString().Length+2;
                    strNoValue+=String.Format( "{0:D"+iCountSpace+"}" , iNoIndex );
                }

                #endregion
            }

            obj.SetNoValue( strNoValue );
            return strNoValue;
        }
        public static String GenerateNo ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
                return GenerateNo( ctrl.GetObjectByID( ID ) );
            return String.Empty;
        }
        public static void AutomaticUpdateNo ( String strTableName )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            foreach ( BusinessObject obj in ctrl.GetListAllObjects() )
            {
                GenerateNo( obj );
                ctrl.UpdateObject( obj );
            }
        }
        #endregion
    }
}
