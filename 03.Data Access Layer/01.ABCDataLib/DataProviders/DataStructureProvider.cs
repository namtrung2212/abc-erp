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
using ABCProvider;

namespace ABCProvider
{

    public class TableColumnData
    {
        public String Name;
        public String DBType;
        public bool Nullable;
        public int Index;
    }
    public class TableStructureData
    {
        public String TableName=String.Empty;
        public String PrimaryColumn=String.Empty;
        public String NOColumn=String.Empty;
        public String NAMEColumn=String.Empty;

        public Dictionary<String , String> ForeignColumnsList=new Dictionary<String , String>();
        public Dictionary<String , TableColumnData> ColumnsList=new Dictionary<String , TableColumnData>();

        public TableStructureData ( String strTableName )
        {
            TableName=strTableName;
        }

        public bool IsForeignKey ( string strColumnName )
        {
            if ( ForeignColumnsList.ContainsKey( strColumnName ) )
                return true;

            return false;
        }
        public bool IsPrimaryKey ( string strColumnName )
        {
            return PrimaryColumn.Trim().ToUpper()==strColumnName.Trim().ToUpper();
        }
        public bool IsNOColumn ( string strNoColumn )
        {
            return strNoColumn.ToUpper().Equals( NOColumn.ToUpper() );
        }
        public bool IsNAMEColumn ( string strNAMEColumn )
        {
            return strNAMEColumn.ToUpper().Equals( NAMEColumn.ToUpper() );
        }
        public bool IsDisplayColumn ( string strColName )
        {
            if ( DataConfigProvider.TableConfigList.ContainsKey( this.TableName )
                &&DataConfigProvider.TableConfigList[this.TableName].FieldConfigList.ContainsKey( strColName ) )
                return DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strColName].IsDisplayField;

            return false;
        }
        public bool IsGroupingColumn ( string strColName )
        {
            if ( DataConfigProvider.TableConfigList.ContainsKey( this.TableName )
                &&DataConfigProvider.TableConfigList[this.TableName].FieldConfigList.ContainsKey( strColName ) )
                return DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strColName].IsGrouping;

            return false;
        }


        public bool IsColumnAllowNull ( string strColumnName )
        {
            if ( strColumnName!=null&&ColumnsList.ContainsKey( strColumnName ) )
                return  ColumnsList[strColumnName].Nullable;

            return false;
        }
        public bool IsTableColumn ( string strColumnName )
        {
            if ( strColumnName!=null&&ColumnsList.ContainsKey( strColumnName ) )
                return true;

            return false;
        }
       
        public TableColumnData GetTableColumn ( string strColumnName )
        {
            if ( ColumnsList.ContainsKey( strColumnName )==false )
                return null;

            return ColumnsList[strColumnName];
        }
        public String GetTableNameOfForeignKey ( string strColumnName )
        {
            if ( ForeignColumnsList.ContainsKey( strColumnName ) )
                return ForeignColumnsList[strColumnName];

            return String.Empty;
        }
        public String GetForeignKeyOfTableName ( string strTableName )
        {
            foreach(String strKey in ForeignColumnsList.Keys)
            if ( ForeignColumnsList[strKey]==strTableName) 
                return strKey;

            return String.Empty;
        }
        public Dictionary<String , TableColumnData> GetAllColumns ( )
        {
            return ColumnsList;
        }
        public Dictionary<String , String> GetAllForeignColumns ( )
        {
            return ForeignColumnsList;
        }
    }

    public class DataStructureProvider
    {
        public static Dictionary<String , TableStructureData> DataTablesList=new Dictionary<String , TableStructureData>();

        #region Table -Columns

        public static void InitDataTableList ( )
        {
            DataTablesList.Clear();

            InitDataTableList(true);

            InitDataTableList( false );

            DataConfigProvider.InvalidateConfigList();
        }

        private static void InitDataTableList ( bool isSystemTable )
        {
            bool isSQL=( isSystemTable&&DataQueryProvider.IsSystemSQLConnection )||( isSystemTable==false&&DataQueryProvider.IsCompanySQLConnection );

            String strQuery;

            Dictionary<String , TableStructureData> lstTables=new Dictionary<string , TableStructureData>();
            if ( isSystemTable )
                SystemTables=new List<string>();

            #region Table
            String strCol;

            if ( isSQL )
            {
                strQuery=@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
                                        WHERE (TABLE_TYPE='BASE TABLE') AND (TABLE_NAME<>'sysdiagrams')
		                                ORDER BY TABLE_NAME";
                strCol="TABLE_NAME";
            }
            else
            {
                strQuery=@"SELECT * FROM sqlite_master WHERE type='table'";
                strCol="tbl_name";
            }

            DataSet ds;
            if ( isSystemTable )
                ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( strQuery );
            else
                ds=DataQueryProvider.CompanyDatabaseHelper.RunQuery( strQuery );

            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow row in ds.Tables[0].Rows )
                {
                    String strTableName=row[strCol].ToString();
                    if ( lstTables.ContainsKey( strTableName )==false )
                    {
                        TableStructureData table=new TableStructureData( strTableName );                     
                        lstTables.Add( strTableName , table );
                        if ( isSystemTable )
                            SystemTables.Add( strTableName );
                    }
                }
            }
            #endregion

            if ( isSQL )
            {                              
                #region All Columns
                strQuery=String.Format( @"SELECT TABLE_NAME, COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,IS_NULLABLE
	                                                            FROM INFORMATION_SCHEMA.COLUMNS " );

                ds=null;
                if ( isSystemTable )
                    ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( strQuery );
                else
                    ds=DataQueryProvider.CompanyDatabaseHelper.RunQuery( strQuery );

                int iCount=-1;
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                    {
                        TableStructureData table=null;
                        if ( lstTables.TryGetValue( dr["TABLE_NAME"].ToString() , out table ) )
                        {
                            iCount++;

                            String strColumnName=dr["COLUMN_NAME"].ToString();
                            if ( table.ColumnsList.ContainsKey( strColumnName )==false )
                            {
                                #region ColumnData

                                TableColumnData colData=new TableColumnData();
                                colData.Index=iCount;
                                colData.Name=strColumnName;

                                String strMaxLength=dr["CHARACTER_MAXIMUM_LENGTH"].ToString();
                                if ( String.IsNullOrWhiteSpace( strMaxLength )==false&&strMaxLength!="NULL"&&dr["DATA_TYPE"].ToString()!="ntext" )
                                    colData.DBType=String.Format( @"{0}({1})" , dr["DATA_TYPE"].ToString() , strMaxLength );
                                else
                                    colData.DBType=dr["DATA_TYPE"].ToString();

                                colData.Nullable=( dr["IS_NULLABLE"].ToString()=="YES" );

                                #region Primary Column
                                if ( strColumnName.EndsWith( "ID" ) )
                                {
                                    String strTableName=strColumnName.Substring( 0 , strColumnName.Length-2 )+"s";
                                    if ( table.TableName.Equals( strTableName ) )
                                        table.PrimaryColumn=strColumnName;
                                }                                
                                #endregion

                                #region NO column
                                if ( strColumnName.ToUpper().Equals( "NO" ) )
                                {
                                    table.NOColumn=strColumnName;
                                }
                                else if ( strColumnName.ToUpper().EndsWith( "NO" ) )
                                {
                                    String stTemp=table.TableName.ToUpper().Substring( 0 , table.TableName.Length-1 )+"NO";
                                    if ( strColumnName.ToUpper().Equals( stTemp ) )
                                        table.NOColumn=strColumnName;
                                }
                                #endregion

                                #region NAME column
                                if ( strColumnName.ToUpper().Equals( "NAME" ) )
                                {
                                    table.NAMEColumn=strColumnName;
                                }
                                if ( strColumnName.ToUpper().EndsWith( "NAME" ) )
                                {
                                    String stTemp=table.TableName.ToUpper().Substring( 0 , table.TableName.Length-1 )+"NAME";
                                    if ( strColumnName.ToUpper().Equals( stTemp ) )
                                        table.NAMEColumn=strColumnName;
                                }

                                #endregion                       

                                #endregion

                                table.ColumnsList.Add( strColumnName , colData );
                                if ( strColumnName.Contains( "FK_" ) )
                                {
                                    String strTemp=strColumnName.Split( '_' )[1];
                                    String strPrimaryTableName=strTemp.Substring( 0 , strTemp.Length-2 )+"s";
                                    if ( table.ForeignColumnsList==null )
                                        table.ForeignColumnsList=new Dictionary<String , String>();
                                    table.ForeignColumnsList.Add( strColumnName , strPrimaryTableName );
                                }
                            }
                        }

                    }

                }
                #endregion                             
            }

            foreach ( String strTableName in lstTables.Keys )
            {
                if ( DataTablesList.ContainsKey( strTableName )==false )
                    DataTablesList.Add( strTableName , lstTables[strTableName] );
            }
        }

        public static TableStructureData GetTable ( string strTableName )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return null;

            TableStructureData dtTable=null;
            DataTablesList.TryGetValue( strTableName , out dtTable );
            return dtTable;
        }

        public static string GetPrimaryKeyColumn ( string strTableName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return null;

            return dtTable.PrimaryColumn;
        }
        public static String GetForeignKeyOfTableName ( string strFKTableName , string strPKTableName )
        {
            TableStructureData dtTable=GetTable( strFKTableName );
            if ( dtTable==null )
                return null;
            return dtTable.GetForeignKeyOfTableName(strPKTableName);
        }
        public static String GetTableNameOfForeignKey( string strFKTableName , string strFK )
        {
            TableStructureData dtTable=GetTable( strFKTableName );
            if ( dtTable==null )
                return null;
            return dtTable.GetTableNameOfForeignKey( strFK );
        }

        public static String GetTableNameOfPrimaryKey ( String strPK )
        {
            foreach ( TableStructureData dtTable in DataTablesList.Values )
                if ( dtTable.PrimaryColumn==strPK )
                    return dtTable.TableName;

            return String.Empty;
        }
        public static string GetDisplayColumn ( string strTableName )
        {
            String strResult=String.Empty;
            if ( DataConfigProvider.TableConfigList.ContainsKey( strTableName ) )
            {
                foreach ( DataConfigProvider.FieldConfig fieldConfig in DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values )
                {
                    if ( fieldConfig.IsDisplayField )
                    {
                        strResult=fieldConfig.FieldName;
                        break;
                    }
                }
            }

            if ( String.IsNullOrWhiteSpace( strResult ) )
                strResult=DataStructureProvider.GetNAMEColumn( strTableName );
          
            if ( String.IsNullOrWhiteSpace( strResult ) )
                strResult=DataStructureProvider.GetNOColumn( strTableName );
          
            if ( String.IsNullOrWhiteSpace( strResult ) )
            {
                if ( DataStructureProvider.IsTableColumn( strTableName , "Remark" ) )
                    strResult="Remark";
            }
            if ( String.IsNullOrWhiteSpace( strResult ) )
                strResult=DataStructureProvider.GetPrimaryKeyColumn( strTableName );

            return strResult;
        }
        public static string GetNOColumn ( string strTableName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return null;

            return dtTable.NOColumn;
        }
        public static string GetNAMEColumn ( string strTableName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return null;

            return dtTable.NAMEColumn;
        }

        public static Dictionary<String , TableColumnData> GetAllTableColumns ( string strTableName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return null;

            return dtTable.GetAllColumns();

        }
        public static Dictionary<String , String> GetAllTableForeignColumns ( string strTableName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return null;

            return dtTable.GetAllForeignColumns();

        }
        public static TableColumnData GetTableColumn ( string strTableName , string strColumnName )
        {
            if ( IsExistedTable( strTableName )==false )
                return null;

            return DataTablesList[strTableName].GetTableColumn( strColumnName );
        }

        public static bool IsForeignKey ( string strTableName , string strColumnName )
        {
            if ( String.IsNullOrWhiteSpace( strTableName )||String.IsNullOrWhiteSpace( strColumnName ) )
                return false;

            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsForeignKey( strColumnName );
        }
        public static bool IsPrimaryKey ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsPrimaryKey( strColumnName );
        }
        public static bool IsNOColumn ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsNOColumn( strColumnName );
        }
        public static bool IsNAMEColumn ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsNAMEColumn( strColumnName );
        }
        public static bool IsDisplayColumn ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsDisplayColumn( strColumnName );
        }
        public static bool IsTableColumn ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsTableColumn( strColumnName );
        }
     
        public static bool IsExistABCStatus ( string strTableName )
        {
            return IsTableColumn( strTableName , ABCCommon.ABCConstString.colABCStatus );
        }
        public static bool IsExistApprovalStatus ( string strTableName )
        {
            return IsTableColumn( strTableName , ABCCommon.ABCConstString.colApprovalStatus );
        }
        public static bool IsColumnAllowNull ( string strTableName , string strColumnName )
        {
            TableStructureData dtTable=GetTable( strTableName );
            if ( dtTable==null )
                return false;

            return dtTable.IsColumnAllowNull( strColumnName );
        }
        public static bool IsExistedTable ( string strTableName )
        {
            return DataTablesList.ContainsKey( strTableName );
        }

        public static String GetColumnDbType ( String strTableName , string strCol )
        {
            TableColumnData col=GetTableColumn( strTableName , strCol );
            if ( col==null )
                return String.Empty;

            return col.DBType;

        }
        public static String GetColumnDataType ( String strTableName , String strColumnName )
        {
            String strColumnDataType=GetColumnDbType( strTableName , strColumnName );

            return strColumnDataType.Split( '(' )[0];
        }

        public static String GetAccrossCodingType ( String strTableName , String strFieldString )
        {
            DataCachingProvider.AccrossStructInfo structInfo=DataCachingProvider.GetAccrossStructInfo( strTableName , strFieldString );
            return GetCodingType( structInfo.TableName , structInfo.FieldName );
        }
        public static String GetCodingType ( String strTableName , String strCol )
        {
            string typestr;

            String strColumnDataType=DataStructureProvider.GetColumnDataType( strTableName , strCol );
            strColumnDataType=strColumnDataType.ToLower();

            if ( strColumnDataType.Contains( "char" )||strColumnDataType.Contains( "text" ) )
                return "String";

            if ( strColumnDataType.Contains( "uniqueidentifier" ) )
            {
                if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol ) )
                    return "Nullable<Guid>";         
                return "Guid";
            }

            if ( strColumnDataType.Contains( "int" )||strColumnDataType.Contains( "numeric" ) )
            {
                if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol ) )
                    return "Nullable<int>";

                return "int";
            }
            if ( strColumnDataType.Contains( "float" )||strColumnDataType.Contains( "double" )||strColumnDataType.Contains( "decimal" )||strColumnDataType.Contains( "real" ) )
            {
                //if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol ) )
                //    return "Nullable<double>";
                return "double";
            }

            if ( strColumnDataType.Contains( "date" )||strColumnDataType.Contains( "time" ) )
            {
                if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol ) )
                    return "Nullable<DateTime>";

                return "DateTime";
            }

            if ( strColumnDataType.Contains( "bit" )||strColumnDataType.Contains( "bool" ) )
            {
                //if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol ) )
                //    return "Nullable<bool>";
                return "bool";
            }

            if ( strColumnDataType.Contains( "image" )||strColumnDataType.Contains( "binary" ) )
                return "byte[]";

            switch ( strColumnDataType )
            {
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "char":
                    typestr="String";
                    break;
                case "int":
                case "bigint":
                case "numeric":
                    typestr="int";
                    break;
                case "float":
                case "real":
                    typestr="double";
                    break;
                case "decimal":
                    typestr="decimal";
                    break;
                case "datetime":
                case "smalldatetime":
                    typestr="DateTime";
                    break;
                case "bit":
                    typestr="bool";
                    break;
                case "image":
                case "varbinary":
                    typestr="byte[]";
                    break;
                default:
                    typestr="_UNKNOWN_";
                    break;


            }

            if ( DataStructureProvider.IsColumnAllowNull( strTableName , strCol )&&typestr.Equals( "DateTime" ) )
                return string.Format( "Nullable<{0}>" , typestr );
            else
                return typestr;



        }

        #endregion

        #region SystemTables
        public static List<String> SystemTables=new List<string>();
        public static bool IsSystemTable ( String strTableName )
        {
            if ( SystemTables.Contains( strTableName )==false )
                if ( strTableName.StartsWith( "ST" ) )
                    SystemTables.Add( strTableName );

            //if ( SystemTables.Count<=0 )
            //{
            //    SystemTables.AddRange( new String[] { "STTableConfigs" , "STFieldConfigs" , "STViews" , "STViewGroups" , "STLanguages" , "STEnumDefines"  , "STDictionarys", 
            //        "STLoggings" } );
            //}
            return SystemTables.Contains( strTableName );
        }
        public static bool IsQuerySystemDB ( String strQuery )
        {
            if ( SystemTables.Count<=0 )
                InitDataTableList( true );

            foreach ( String tableName in SystemTables )
                if ( strQuery.Contains( tableName ) )
                    return true;

            return false;
        }
        #endregion

    }
}

