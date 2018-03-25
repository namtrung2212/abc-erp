using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;
using DevExpress.Utils;
using ABCProvider;

namespace ABCProvider
{
    public class DataConfigProvider
    {

        public class FieldConfig
        {
            public Guid ConfigID { get; set; }
            public String FieldName { get; set; }
            public String CaptionVN { get; set; }
            public String CaptionEN { get; set; }
            public String DescVN { get; set; }
            public String DescEN { get; set; }
            public String TypeName { get; set; }
            public Boolean InUse { get; set; }
            public Boolean IsDefault { get; set; }
            public Boolean IsDisplayField { get; set; }
            public Boolean IsGrouping { get; set; }
            public String AssignedEnum { get; set; }
            public String FilterString { get; set; }
            public int SortOrder { get; set; }
            public DataFormatProvider.FieldFormat Format { get; set; }

            public FieldConfig ( )
            {
                IsDefault=false;
                IsGrouping=false;
                IsDisplayField=false;
                Format=DataFormatProvider.FieldFormat.None;
            }
        }
        public class TableConfig
        {
            public Guid ConfigID { get; set; }
            public String TableName { get; set; }
            public String CaptionVN { get; set; }
            public String CaptionEN { get; set; }
            public String DescVN { get; set; }
            public String DescEN { get; set; }
            public Boolean IsCaching { get; set; }
            public String PrefixNo { get; set; }
            public Dictionary<String , FieldConfig> FieldConfigList=new Dictionary<string , FieldConfig>();
        }
        public static Dictionary<String , TableConfig> TableConfigList=new Dictionary<string , TableConfig>();


        public static void SynchronizeTableConfigs ( )
        {
            GenerateDefaultTableConfig();
            InvalidateConfigList();
        }

        [ABCRefreshTable( "STFieldConfigs" ,"STTableConfigs" )]
        public static void InvalidateConfigList ( )
        {

            String strCheckCondition=String.Empty;
            if ( DataQueryProvider.IsSQLConnection( "STTableConfigs" ) )
                strCheckCondition="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='STTableConfigs'";
            else
                strCheckCondition="SELECT tbl_name FROM sqlite_master WHERE type='table' AND tbl_name='STTableConfigs'";

            DataSet dsTemp=DataQueryProvider.RunQuery( strCheckCondition );
            if ( dsTemp==null||dsTemp.Tables.Count<=0||dsTemp.Tables[0].Rows.Count<=0 )
                return;

            TableConfigList.Clear();

            #region Init ConfigList

            #region Field Config
            DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( @"SELECT * FROM STFieldConfigs" );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    TableConfig tableConfig=null;
                    String strTableName=dr["TableName"].ToString();
                    if ( TableConfigList.TryGetValue( strTableName , out tableConfig )==false )
                    {
                        tableConfig=new TableConfig();
                        TableConfigList.Add( strTableName , tableConfig );
                    }
                    if ( tableConfig.FieldConfigList.ContainsKey( dr["FieldName"].ToString() )==false )
                    {
                        #region Field
                        FieldConfig config=new FieldConfig();
                        config.ConfigID=ABCHelper.DataConverter.ConvertToGuid( dr["STFieldConfigID"] );
                        config.FieldName=dr["FieldName"].ToString();
                        config.CaptionVN=dr["CaptionVN"].ToString();
                        config.CaptionEN=dr["CaptionEN"].ToString();
                        config.DescVN=dr["DescriptionVN"].ToString();
                        config.DescEN=dr["DescriptionEN"].ToString();
                        config.TypeName=DataStructureProvider.GetCodingType( strTableName , config.FieldName );
                     
                        config.InUse=false;
                        if ( dr["InUse"]!=DBNull.Value )
                            config.InUse=Convert.ToBoolean( dr["InUse"] );

                        if ( dr["IsDefault"]!=DBNull.Value )
                            config.IsDefault=Convert.ToBoolean( dr["IsDefault"] );

                        if ( dr["IsDisplayField"]!=DBNull.Value )
                            config.IsDisplayField=Convert.ToBoolean( dr["IsDisplayField"] );

                        if ( dr["IsGrouping"]!=DBNull.Value )
                            config.IsGrouping=Convert.ToBoolean( dr["IsGrouping"] );

                        if ( dr["AssignedEnum"]!=DBNull.Value )
                            config.AssignedEnum=dr["AssignedEnum"].ToString();

                        if ( dr["FilterString"]!=DBNull.Value )
                            config.FilterString=dr["FilterString"].ToString();

                        if ( dr["SortOrder"]!=DBNull.Value )
                            config.SortOrder=Convert.ToInt32( dr["SortOrder"] );

                        #region FieldFormat
                        if ( Enum.IsDefined( typeof( DataFormatProvider.FieldFormat ) , dr["Format"].ToString() ) )
                        {
                            DataFormatProvider.FieldFormat format=(DataFormatProvider.FieldFormat)Enum.Parse( typeof( DataFormatProvider.FieldFormat ) , dr["Format"].ToString() );
                            if ( format!=DataFormatProvider.FieldFormat.None )
                                config.Format=format;
                            else
                            {
                                if ( config.TypeName=="DateTime"||config.TypeName=="Nullable<DateTime>" )
                                {
                                    config.Format=DataFormatProvider.FieldFormat.Date;
                                }
                                if ( ( config.TypeName=="int"||config.TypeName=="Nullable<int>" )&&config.FieldName.ToUpper().Contains( "FK_" )==false
                                    &&DataStructureProvider.IsForeignKey( strTableName , config.FieldName )==false
                                    &&DataStructureProvider.IsPrimaryKey( strTableName , config.FieldName )==false )
                                {
                                    config.Format=DataFormatProvider.FieldFormat.Quantity;
                                }
                                if ( config.TypeName=="double"||config.TypeName=="decimal" )
                                {
                                    if ( config.FieldName.ToLower().Contains( "amt" )||config.FieldName.ToLower().Contains( "amount" )||config.FieldName.ToLower().Contains( "exchange" ) )
                                        config.Format=DataFormatProvider.FieldFormat.Currency;
                                    else
                                        config.Format=DataFormatProvider.FieldFormat.Amount;
                                }
                            }
                        }
                        #endregion

                        tableConfig.FieldConfigList.Add( config.FieldName , config );
                        #endregion
                    }
                }
            }
            #endregion

            #region Table Config
            ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( @"SELECT * FROM STTableConfigs" );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    TableConfig tableConfig=null;
                    String strTableName=dr["TableName"].ToString();
                    if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
                    {
                        tableConfig.ConfigID=ABCHelper.DataConverter.ConvertToGuid( dr["STTableConfigID"] );
                        tableConfig.TableName=strTableName;
                        tableConfig.CaptionVN=dr["CaptionVN"].ToString();
                        tableConfig.CaptionEN=dr["CaptionEN"].ToString();
                        tableConfig.DescVN=dr["DescriptionVN"].ToString();
                        tableConfig.DescEN=dr["DescriptionEN"].ToString();

                        if ( dr["IsCaching"]!=DBNull.Value )
                            tableConfig.IsCaching=Convert.ToBoolean( dr["IsCaching"] );
                        else
                            tableConfig.IsCaching=false;
                    }
                }
            }
            #endregion

            #endregion

        }

        public static void GenerateDefaultTableConfig ( )
        {
            String strCheckCondition=String.Empty;
            if ( DataQueryProvider.IsSQLConnection( "STTableConfigs" ) )
                strCheckCondition="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='STTableConfigs'";
            else
                strCheckCondition="SELECT tbl_name FROM sqlite_master WHERE type='table' AND tbl_name='STTableConfigs'";

            DataSet dsTemp=DataQueryProvider.RunQuery( strCheckCondition );
            if ( dsTemp==null||dsTemp.Tables.Count<=0||dsTemp.Tables[0].Rows.Count<=0 )
                return;

            #region Clean First
            DataSet ds2=DataQueryProvider.SystemDatabaseHelper.RunQuery( "SELECT STTableConfigID,TableName FROM  STTableConfigs " );
            if ( ds2!=null&&ds2.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds2.Tables[0].Rows )
                {
                    if ( String.IsNullOrWhiteSpace( dr[0].ToString() )==false )
                    {
                        Guid iID=Guid.Empty;
                        Guid.TryParse( dr[0].ToString() , out iID );

                        String strTableName=dr[1].ToString();
                        if ( DataStructureProvider.DataTablesList.ContainsKey( strTableName )==false )
                            DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "DELETE FROM STTableConfigs WHERE STTableConfigID='{0}'" , iID.ToString() ) );
                    }
                }
            }

            ds2=DataQueryProvider.SystemDatabaseHelper.RunQuery( "SELECT STFieldConfigID,TableName,FieldName FROM  STFieldConfigs " );
            if ( ds2!=null&&ds2.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds2.Tables[0].Rows )
                {
                    if ( String.IsNullOrWhiteSpace( dr[0].ToString() )==false )
                    {
                        Guid iID=Guid.Empty;
                        Guid.TryParse( dr[0].ToString() , out iID );
                        String strTableName=dr[1].ToString();
                        String strFieldName=dr[2].ToString();
                        if ( DataStructureProvider.DataTablesList.ContainsKey( strTableName )==false||
                            DataStructureProvider.DataTablesList[strTableName].ColumnsList.ContainsKey( strFieldName )==false )
                        {
                            DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "DELETE FROM STFieldConfigs WHERE STFieldConfigID='{0}'" , iID.ToString() ) );
                        }

                    }
                }
            }
            #endregion

            foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
            {
                #region Table Config
                DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT COUNT(*) FROM  STTableConfigs WHERE TableName='{0}'" , strTableName ) );
                if ( ds==null||ds.Tables.Count<=0||ds.Tables[0].Rows.Count<=0||Convert.ToInt32( ds.Tables[0].Rows[0][0] )<=0 )
                {

                    String strQuery=QueryTemplateGenerator.GenInsert( "STTableConfigs" );
                    strQuery=strQuery.Replace( "@STTableConfigID" , "'"+Guid.Empty.ToString()+"'" );
                    strQuery=strQuery.Replace( "@TableName" , "'"+strTableName+"'" );

                    String s=strTableName.Substring( 2 , strTableName.Length-2 );
                    //var r=new System.Text.RegularExpressions.Regex( @"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])" , System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace );
                    //s=r.Replace( s , " " );


                    strQuery=strQuery.Replace( "@CaptionVN" , "'"+s+"'" );
                    strQuery=strQuery.Replace( "@CaptionEN" , "'"+s+"'" );
                    strQuery=strQuery.Replace( "@DescriptionVN" , "''" );
                    strQuery=strQuery.Replace( "@DescriptionEN" , "''" );
                    strQuery=strQuery.Replace( "@IsCaching" , "0" );
                    strQuery=strQuery.Replace( "@CalcOnClient" , "1" );
                    strQuery=strQuery.Replace( "@DeleteByOwnerOnly" , "0" );
                    strQuery=strQuery.Replace( "@EditByOwnerOnly" , "0" );
                    DataQueryProvider.SystemDatabaseHelper.RunScript( strQuery );
                }
                #endregion

                #region Field Config
                List<String> lstConfig=new List<String>();

                ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT STFieldConfigID,FieldName FROM  STFieldConfigs WHERE TableName='{0}'" , strTableName ) );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                    {
                        if ( String.IsNullOrWhiteSpace( dr[1].ToString() )==false )
                        {
                            if ( lstConfig.Contains( dr[1].ToString() )==false )
                                lstConfig.Add( dr[1].ToString() );
                            else
                            {
                                Guid iID=Guid.Empty;
                                Guid.TryParse( dr[0].ToString() , out iID );
                                DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "DELETE FROM STFieldConfigs WHERE STFieldConfigID='{0}'" , strTableName ) );
                            }
                        }
                    }
                }

                int iCount=-1;
                foreach ( String strColName in DataStructureProvider.DataTablesList[strTableName].ColumnsList.Keys )
                {
                    if ( DataStructureProvider.IsPrimaryKey( strTableName , strColName ) )
                        continue;

                    iCount++;
                    if ( lstConfig.Contains( strColName )==false )
                    {
                        String strQuery=QueryTemplateGenerator.GenInsert( "STFieldConfigs" );
                        Guid iID=Guid.NewGuid();
                        strQuery=strQuery.Replace( "@STFieldConfigID" , "'"+iID.ToString()+"'" );
                        strQuery=strQuery.Replace( "@TableName" , "'"+strTableName+"'" );
                        strQuery=strQuery.Replace( "@FieldName" , "'"+strColName+"'" );
                        if ( DataStructureProvider.IsForeignKey( strTableName , strColName ) )
                        {
                            String strFKTableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , strColName );
                            String strCaptionVN=GetTableCaption( strFKTableName );
                            if ( String.IsNullOrWhiteSpace( strCaptionVN ) )
                                strCaptionVN=strFKTableName.Substring( 2 , strFKTableName.Length-3 );
                            //var r=new System.Text.RegularExpressions.Regex( @"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])" , System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace );
                            //strCaptionVN=r.Replace( strCaptionVN , " " );

                            strQuery=strQuery.Replace( "@CaptionVN" , String.Format( "N'{0}'" , strCaptionVN ) );
                            strQuery=strQuery.Replace( "@CaptionEN" , "'"+strFKTableName.Substring( 2 , strFKTableName.Length-3 )+"'" );
                        }
                        else
                        {
                            strQuery=strQuery.Replace( "@CaptionVN" , String.Format( "N'{0}'" , GetFieldCaptionFromDictionary( strColName ) ) );
                            strQuery=strQuery.Replace( "@CaptionEN" , "'"+strColName+"'" );
                            if ( DataStructureProvider.IsNOColumn( strTableName , strColName ) )
                            {
                                String strTableCap=GetTableCaption( strTableName );
                                if ( !String.IsNullOrWhiteSpace( strTableCap ) )
                                    strQuery=strQuery.Replace( "@CaptionVN" , String.Format( "N'{0}'" , strTableCap ) );
                            }
                        }
                        strQuery=strQuery.Replace( "@DescriptionVN" , "''" );
                        strQuery=strQuery.Replace( "@DescriptionEN" , "''" );
                        if ( strColName.Equals( "ABCStatus" )||strColName.Equals( "EditCount" )||
                            strColName.Equals( "CreateTime" )||strColName.Equals( "CreateUser" )||
                            strColName.Equals( "UpdateTime" )||strColName.Equals( "UpdateUser" )||
                            strColName.Equals( "NoIndex" )||strColName.Equals( "LastCalcDate" ) )
                            strQuery=strQuery.Replace( "@InUse" , "0" );
                        else
                            strQuery=strQuery.Replace( "@InUse" , "1" );

                        if ( DataStructureProvider.IsNOColumn( strTableName , strColName )
                            ||DataStructureProvider.IsNAMEColumn( strTableName , strColName )
                            ||DataStructureProvider.IsDisplayColumn( strTableName , strColName ) )
                            strQuery=strQuery.Replace( "@IsDefault" , "1" );
                        else
                            strQuery=strQuery.Replace( "@IsDefault" , "0" );

                        strQuery=strQuery.Replace( "@AssignedEnum" , "''" );
                        strQuery=strQuery.Replace( "@FilterString" , "''" );
                        strQuery=strQuery.Replace( "@SortOrder" , String.Format( "{0}" , iCount ) );

                        if ( DataStructureProvider.IsNOColumn( strTableName , strColName )
                         ||DataStructureProvider.IsNAMEColumn( strTableName , strColName ) )
                            strQuery=strQuery.Replace( "@IsDisplayField" , "1" );
                        else
                            strQuery=strQuery.Replace( "@IsDisplayField" , "0" );

                        strQuery=strQuery.Replace( "@IsDisplayField" , "0" );
                        strQuery=strQuery.Replace( "@IsGrouping" , "0" );

                        #region DataFormatProvider.FieldFormat
                        String strTypeName=DataStructureProvider.GetCodingType( strTableName , strColName );
                        DataFormatProvider.FieldFormat format=DataFormatProvider.FieldFormat.None;

                        if ( strTypeName=="DateTime"||strTypeName=="Nullable<DateTime>" )
                            format=DataFormatProvider.FieldFormat.Date;

                        if ( ( strTypeName=="int"||strTypeName=="Nullable<int>"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )&&strColName.ToUpper().Contains( "FK_" )==false
                            &&DataStructureProvider.IsForeignKey( strTableName , strColName )==false
                            &&DataStructureProvider.IsPrimaryKey( strTableName , strColName )==false )
                            format=DataFormatProvider.FieldFormat.Quantity;

                        if ( strTypeName=="double"||strTypeName=="decimal" )
                        {
                            if ( strColName.ToLower().Contains( "amt" )||strColName.ToLower().Contains( "fee" )||strColName.ToLower().Contains( "cost" )||
                                strColName.ToLower().Contains( "expense" )||strColName.ToLower().Contains( "pay" )||strColName.ToLower().Contains( "tax" )||
                                strColName.ToLower().Contains( "gross" )||strColName.ToLower().Contains( "net" )||
                                strColName.ToLower().Contains( "amount" )||strColName.ToLower().Contains( "exchange" ) )
                                format=DataFormatProvider.FieldFormat.Currency;
                            else
                                format=DataFormatProvider.FieldFormat.Amount;
                        }

                        if ( format!=DataFormatProvider.FieldFormat.None )
                            strQuery=strQuery.Replace( "@Format" , String.Format( "'{0}'" , format.ToString() ) );
                        else
                            strQuery=strQuery.Replace( "@Format" , "''" );

                        #endregion

                        DataQueryProvider.SystemDatabaseHelper.RunScript( strQuery );
                    }


                }

                foreach ( String strColName in lstConfig )
                {
                    if ( DataStructureProvider.DataTablesList[strTableName].ColumnsList.ContainsKey( strColName )==false )
                        DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "DELETE  FROM  STFieldConfigs WHERE TableName='{0}' AND FieldName='{1}'" , strTableName , strColName ) );
                }
                #endregion
            }
        }

        public static String GetTableCaption ( String strTableName )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return String.Empty;

            TableConfig tableConfig=null;
            if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
            {
                if ( ABCApp.ABCDataGlobal.Language=="VN"&&String.IsNullOrWhiteSpace( tableConfig.CaptionVN )==false )
                    return tableConfig.CaptionVN;
                if ( ABCApp.ABCDataGlobal.Language=="EN"&&String.IsNullOrWhiteSpace( tableConfig.CaptionEN )==false )
                    return tableConfig.CaptionEN;
            }

            return strTableName;
        }
        public static String GetTableDesc ( String strTableName )
        {
            TableConfig tableConfig=null;
            if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
            {
                if ( ABCApp.ABCDataGlobal.Language=="VN"&&String.IsNullOrWhiteSpace( tableConfig.DescVN )==false )
                    return tableConfig.DescVN;
                if ( ABCApp.ABCDataGlobal.Language=="EN"&&String.IsNullOrWhiteSpace( tableConfig.DescEN )==false )
                    return tableConfig.DescEN;
            }

            return String.Empty;
        }

        public static int GetFieldSortOrder ( String strTableName , String strColName )
        {
            TableConfig tableConfig=null;
            if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
            {
                FieldConfig colConfig=null;
                if ( tableConfig.FieldConfigList.TryGetValue( strColName , out colConfig ) )
                    return colConfig.SortOrder;
            }

            return -1;
        }

        public static String GetFieldCaption ( String strTableName , String strColName )
        {
            TableConfig tableConfig=null;
            if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
            {
                FieldConfig colConfig=null;
                if ( tableConfig.FieldConfigList.TryGetValue( strColName , out colConfig ) )
                {
                    if ( ABCApp.ABCDataGlobal.Language=="VN"&&String.IsNullOrWhiteSpace( colConfig.CaptionVN )==false )
                        return colConfig.CaptionVN;
                    if ( ABCApp.ABCDataGlobal.Language=="EN"&&String.IsNullOrWhiteSpace( colConfig.CaptionEN )==false )
                        return colConfig.CaptionEN;
                }
            }

            return strColName;
        }
        public static String GetFieldDesc ( String strTableName , String strColName )
        {
            TableConfig tableConfig=null;
            if ( TableConfigList.TryGetValue( strTableName , out tableConfig ) )
            {
                FieldConfig colConfig=null;
                if ( tableConfig.FieldConfigList.TryGetValue( strColName , out colConfig ) )
                {
                    if ( ABCApp.ABCDataGlobal.Language=="VN"&&String.IsNullOrWhiteSpace( colConfig.DescVN )==false )
                        return colConfig.DescVN;
                    if ( ABCApp.ABCDataGlobal.Language=="EN"&&String.IsNullOrWhiteSpace( colConfig.DescEN )==false )
                        return colConfig.DescEN;
                }
            }

            return String.Empty;
        }

        public static String GetFieldCaptionFromDictionary ( String strColName , bool isTranslateVN )
        {
            String strQuery=String.Empty;
            if(isTranslateVN)
                strQuery=String.Format( "SELECT TranslateVN FROM  STDictionarys WHERE (KeyString='%{0}%' AND IsContain='TRUE') OR (KeyString='{0}%' AND IsStartWith='TRUE') OR (KeyString='%{0}' AND IsEndWith='TRUE') OR (KeyString='{0}' AND (IsContain='FALSE' OR IsContain IS NUL) AND (IsStartWith='FALSE' OR IsStartWith IS NUL) AND (IsEndWith='FALSE' OR IsEndWith IS NUL) )" , strColName );
            else
                strQuery=String.Format( "SELECT TranslateEN FROM  STDictionarys WHERE (KeyString='%{0}%' AND IsContain='TRUE') OR (KeyString='{0}%' AND IsStartWith='TRUE') OR (KeyString='%{0}' AND IsEndWith='TRUE') OR (KeyString='{0}' AND (IsContain='FALSE' OR IsContain IS NUL) AND (IsStartWith='FALSE' OR IsStartWith IS NUL) AND (IsEndWith='FALSE' OR IsEndWith IS NUL) )" , strColName );

            DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0&&String.IsNullOrWhiteSpace( ds.Tables[0].Rows[0][0].ToString() )==false )
                return ds.Tables[0].Rows[0][0].ToString();

            return String.Empty;
        }
        public static String GetFieldCaptionFromDictionary ( String strColName  )
        {
            return GetFieldCaptionFromDictionary( strColName , true );
        }
    }
}
