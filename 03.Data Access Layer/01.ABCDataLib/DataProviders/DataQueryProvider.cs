using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibContrib.Data.SQLite;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;
using System.Xml;

using ABCProvider;
using Security;
using ABCBusinessEntities;

namespace ABCProvider
{

    public enum DatabaseType
    {
        MSSQL ,
        SQLite ,
        Oracle ,
        MySQL
    }
    public class ConnectionInfo
    {
        public String ConnectionString=String.Empty;
        public String Name=String.Empty;
        public DatabaseType DBType=DatabaseType.MSSQL;

        public String Server=String.Empty;
        public String DatabaseName=String.Empty;
        public String User=String.Empty;
        public String EncryptedPassword=String.Empty;
        public bool IsDefault=false;
        public Database Database=null;

        public ConnectionInfo ( String strName , DatabaseType dbType , String strServer , String strDatabase , String strUser , String strPassword , bool isDefault )
        {
            Name=strName;
            DBType=dbType;
            Server=strServer;
            DatabaseName=strDatabase;
            User=strUser;
            EncryptedPassword=strPassword;
            IsDefault=isDefault;

            Cryptography cryp=new Cryptography();
            strPassword=cryp.Decrypt( strPassword );
            if ( dbType==DatabaseType.MSSQL )
                ConnectionString=String.Format( "Data Source={0};Initial Catalog={1};User ID={2};Password={3}" , strServer , strDatabase , strUser , strPassword );

            if ( dbType==DatabaseType.SQLite )
                ConnectionString=String.Format( "Data Source={0};Version=3;Password={1};" , strDatabase , strPassword );

        }

        public bool Connect ( )
        {
            try
            {
                if ( String.IsNullOrWhiteSpace( ConnectionString ) )
                    return false;

                if ( this.Database==null )
                {
                    if ( DBType==DatabaseType.MSSQL )
                        this.Database=new SqlDatabase( ConnectionString );
                    if ( DBType==DatabaseType.SQLite )
                        this.Database=new SQLiteDatabase( ConnectionString );
                }
                return TestConnection();
            }
            catch ( Exception ex )
            {
                return false;
            }

            return true;
        }

        public bool TestConnection ( )
        {
            try
            {
                String strTestQuery=String.Empty;
                if ( DBType==DatabaseType.MSSQL )
                    strTestQuery="Select COUNT(*) From  INFORMATION_SCHEMA.KEY_COLUMN_USAGE ";
                if ( DBType==DatabaseType.SQLite )
                    strTestQuery="Select COUNT(*) From sqlite_master ";

                DbCommand cmd=this.Database.GetSqlStringCommand( strTestQuery );
                this.Database.ExecuteDataSet( cmd );
                return true;
            }
            catch ( Exception ex )
            {
                return false;
            }
        }

    }

    public class DBConnectionController
    {
        public String CompanyName=String.Empty;
        public String CompanyDesc=String.Empty;

        public ConnectionInfo Connection=null;

        public DBConnectionController ( String strCompanyName , String strCompanyDesc )
        {
            CompanyName=strCompanyName;
            CompanyDesc=strCompanyDesc;
        }

        public void InitNewConnection ( String strName , DatabaseType dbType , String strServer , String strDatabase , String strUser , String strPassword , bool isDefault )
        {
            Connection=new ConnectionInfo( strName , dbType , strServer , strDatabase , strUser , strPassword , isDefault );
        }

        public bool Connect ( )
        {
            return Connection.Connect();
        }

        public DataSet RunQuery ( String strQuery )
        {
            if ( Connection==null||Connection.Database==null )
                return null;

            DbCommand cmd=Connection.Database.GetSqlStringCommand( strQuery );
            if ( cmd==null )
                return null;

            return Connection.Database.ExecuteDataSet( cmd );
        }
    }

    public class DataQueryProvider
    {
        public static Dictionary<String , DBConnectionController> CompanyCollection=new Dictionary<string , DBConnectionController>();
        public static Dictionary<String , DBConnectionController> SystemCollection=new Dictionary<string , DBConnectionController>();

        static DBConnectionController CompanyConnection=null;
        static DBConnectionController SystemConnection=null;

        public static Database CompanyDatabase
        {
            get
            {
                if ( CompanyConnection==null||CompanyConnection.Connection==null )
                    return null;
                return CompanyConnection.Connection.Database;
            }
        }
        public static Database SystemDatabase
        {
            get
            {
                if ( SystemConnection==null||SystemConnection.Connection==null )
                    return null;
                return SystemConnection.Connection.Database;
            }
        }


        public static ABCDatabaseHelper DatabaseHelper=new ABCDatabaseHelper( null );
        public static ABCDatabaseHelper CompanyDatabaseHelper;
        public static ABCDatabaseHelper SystemDatabaseHelper;

        public static bool IsSQLConnection ( Database db )
        {
            return ( db is Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase );
        }
        public static bool IsSQLConnection ( String strTableName )
        {
            if ( ( DataStructureProvider.IsSystemTable( strTableName )&&DataQueryProvider.IsSystemSQLConnection )
            ||( DataStructureProvider.IsSystemTable( strTableName )==false&&DataQueryProvider.IsCompanySQLConnection ) )
                return true;

            return false;
        }
        public static bool IsCompanySQLConnection
        {
            get
            {
                return ( CompanyDatabase is Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase );
            }
        }
        public static bool IsSystemSQLConnection
        {
            get
            {
                return ( SystemDatabase is Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase );
            }
        }

        static DataQueryProvider ( )
        {
            InitConnectionsFromXML();
            //   WriteCompanyListToXML();
        }

        #region Connection

        #region Connection Config

        public static void GetConnectionStringFromRegistry ( )
        {
            //RegistryWorker regW=new RegistryWorker();
            //regW.SubKey="SOFTWARE\\ABC";
            //regW.SubKey=regW.SubKey+"\\ConnectionString";
            //String strDatabase=regW.Read( "Database" );
            //String strServer=regW.Read( "Server" );
            //String strUser=regW.Read( "User" );
            //string strPassword=regW.Read( "Password" );

            //CompanyCollection.Clear();
            //CompanyInfo company=new CompanyInfo( "ABC" , "ABCDEsc" );
            //company.InitConnection( strServer , strDatabase , strUser , strPassword );
            //if ( CompanyCollection.ContainsKey( "ABC" )==false )
            //    CompanyCollection.Add( "ABC" , company );
        }
        public static void InitConnectionsFromXML ( )
        {
            CompanyCollection.Clear();
            SystemCollection.Clear();

            String strFileName=@"Config.xml";
            XmlDocument doc=new XmlDocument();
            doc.Load( strFileName );

            #region Companys
            XmlNodeList nodeCompanyList=doc.GetElementsByTagName( "Company" );
            foreach ( XmlNode nodeCompany in nodeCompanyList )
            {
                String strCompany=nodeCompany.Attributes["Name"].Value.ToString();
                String strDesc=nodeCompany.Attributes["Desc"].Value.ToString();

                DBConnectionController company=new DBConnectionController( strCompany , strDesc );

                #region Connection

                DatabaseType DBType=(DatabaseType)Enum.Parse( typeof( DatabaseType ) , nodeCompany.Attributes["DBType"].Value.ToString() );
                String strServer=nodeCompany.Attributes["Server"].Value.ToString();
                String strDatabase=nodeCompany.Attributes["Database"].Value.ToString();
                String strUser=nodeCompany.Attributes["User"].Value.ToString();
                String strPassword=nodeCompany.Attributes["Password"].Value.ToString();
                bool isDefault=Convert.ToBoolean( nodeCompany.Attributes["IsDefault"].Value.ToString() );
                company.InitNewConnection( strCompany , DBType , strServer , strDatabase , strUser , strPassword , isDefault );
                #endregion

                if ( CompanyCollection.ContainsKey( strCompany )==false )
                    CompanyCollection.Add( strCompany , company );
            }
            #endregion

            #region System
            XmlNodeList nodeSystemList=doc.GetElementsByTagName( "System" );
            foreach ( XmlNode nodeSystem in nodeSystemList )
            {
                String strSystem=nodeSystem.Attributes["Name"].Value.ToString();
                String strDesc=nodeSystem.Attributes["Desc"].Value.ToString();

                DBConnectionController System=new DBConnectionController( strSystem , strDesc );

                #region Connection

                DatabaseType DBType=(DatabaseType)Enum.Parse( typeof( DatabaseType ) , nodeSystem.Attributes["DBType"].Value.ToString() );
                String strServer=nodeSystem.Attributes["Server"].Value.ToString();
                String strDatabase=nodeSystem.Attributes["Database"].Value.ToString();
                String strUser=nodeSystem.Attributes["User"].Value.ToString();
                String strPassword=nodeSystem.Attributes["Password"].Value.ToString();
                bool isDefault=Convert.ToBoolean( nodeSystem.Attributes["IsDefault"].Value.ToString() );
                System.InitNewConnection( strSystem , DBType , strServer , strDatabase , strUser , strPassword , isDefault );
                #endregion

                if ( SystemCollection.ContainsKey( strSystem )==false )
                    SystemCollection.Add( strSystem , System );
            }
            #endregion
        }
        public static void WriteCompanyListToXML ( )
        {
            //XmlDocument doc=new XmlDocument();
            //XmlDeclaration dec=doc.CreateXmlDeclaration( "1.0" , "utf-16" , null );
            //doc.AppendChild( dec );

            //XmlElement root=doc.CreateElement( "Companys" );
            //doc.AppendChild( root );

            //foreach ( String strCompany in CompanyCollection.Keys )
            //{
            //    XmlElement elCompany=doc.CreateElement( "Company" );
            //    elCompany.SetAttribute( "Name" , CompanyCollection[strCompany].CompanyName );
            //    elCompany.SetAttribute( "Desc" , CompanyCollection[strCompany].CompanyDesc );


            //    elCompany.SetAttribute( "DBType" , CompanyCollection[strCompany].CurrentConnection.DBType.ToString() );
            //    elCompany.SetAttribute( "Server" , CompanyCollection[strCompany].CurrentConnection.Server );
            //    elCompany.SetAttribute( "Database" , CompanyCollection[strCompany].CurrentConnection.DatabaseName );
            //    elCompany.SetAttribute( "User" , CompanyCollection[strCompany].CurrentConnection.User );
            //    elCompany.SetAttribute( "Password" , CompanyCollection[strCompany].CurrentConnection.EncryptedPassword );
            //    elCompany.SetAttribute( "IsDefault" , CompanyCollection[strCompany].CurrentConnection.IsDefault.ToString() );
            //    root.AppendChild( elCompany );
            //}

            //String strFileName=@"Config.xml";
            //doc.Save( strFileName );
        }

        #endregion

        public static bool Connect ( )
        {
            #region Connect
            DBConnectionController system=null;
            DBConnectionController company=ConnectCompany();
            if ( company!=null )
                system=ConnectSystem();

            if ( company==null||system==null )
                return false;

            CompanyConnection=company;
            SystemConnection=system;

            if ( IsCompanySQLConnection )
                CompanyDatabaseHelper=new SqlDatabaseHelper( CompanyDatabase );
            else
                CompanyDatabaseHelper=new SQLiteDatabaseHelper( CompanyDatabase );

            if ( IsSystemSQLConnection )
                SystemDatabaseHelper=new SqlDatabaseHelper( SystemDatabase );
            else
                SystemDatabaseHelper=new SQLiteDatabaseHelper( SystemDatabase );
            #endregion

            if ( TestCompanyConnection()&&TestSystemConnection() )
                return true;
            return false;
        }
        public static bool Connect ( String strCompanyName )
        {
            #region Connect
            DBConnectionController system=null;
            DBConnectionController company=ConnectCompany( strCompanyName );
            if ( company!=null )
                system=ConnectSystem();

            if ( company==null||system==null )
                return false;

            CompanyConnection=company;
            SystemConnection=system;

            if ( IsCompanySQLConnection )
                CompanyDatabaseHelper=new SqlDatabaseHelper( CompanyDatabase );
            else
                CompanyDatabaseHelper=new SQLiteDatabaseHelper( CompanyDatabase );

            if ( IsSystemSQLConnection )
                SystemDatabaseHelper=new SqlDatabaseHelper( SystemDatabase );
            else
                SystemDatabaseHelper=new SQLiteDatabaseHelper( SystemDatabase );
            #endregion

            if ( TestCompanyConnection()&&TestSystemConnection() )
                return true;
        
            return false;
        }
        public static void InitDataTables ( )
        {
            DataStructureProvider.InitDataTableList();
            DataCachingProvider.InitTableCachings();

        }
        public static DBConnectionController ConnectCompany ( )
        {
            foreach ( DBConnectionController comp in CompanyCollection.Values )
            {
                if ( comp.Connection.IsDefault )
                    return ConnectCompany( comp.CompanyName );
            }
            return null;
        }
        public static DBConnectionController ConnectCompany ( String strCompanyName )
        {
            if ( CompanyCollection.ContainsKey( strCompanyName )==false )
                return null;

            try
            {
                if ( CompanyCollection[strCompanyName].Connect() )
                    return CompanyCollection[strCompanyName];
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
            }
            return null;
        }

        public static DBConnectionController ConnectSystem ( )
        {
            foreach ( DBConnectionController system in SystemCollection.Values )
            {
                if ( system.Connection.IsDefault )
                {
                    if ( system.Connect() )
                        return system;
                }
            }
            return null;
        }

        public static bool TestCompanyConnection ( )
        {
            try
            {
                String strTestQuery="Select COUNT(*) From  INFORMATION_SCHEMA.KEY_COLUMN_USAGE ";
                if(IsCompanySQLConnection==false)
                    strTestQuery="Select COUNT(*) From  sqlite_master ";

                DbCommand cmd=CompanyDatabase.GetSqlStringCommand( strTestQuery );
                CompanyDatabase.ExecuteDataSet( cmd );
                return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }
        public static bool TestSystemConnection ( )
        {
            try
            {
                String strTestQuery="Select COUNT(*) From  INFORMATION_SCHEMA.KEY_COLUMN_USAGE ";
                if ( IsSystemSQLConnection==false )
                    strTestQuery="Select COUNT(*) From  sqlite_master ";

                DbCommand cmd=SystemDatabase.GetSqlStringCommand( strTestQuery );
                SystemDatabase.ExecuteDataSet( cmd );
                return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }

        #endregion

        public static DataSet RunQuery ( String strQueryCommand )
        {
            return DatabaseHelper.RunQuery( strQueryCommand );
        }
        public static string ExecuteScript ( String strScript )
        {
            return DatabaseHelper.ExecuteScript( strScript );
        }
       
    }


    public class SqlDatabaseHelper : ABCDatabaseHelper
    {
        public SqlDatabaseHelper ( Database database )
        {
            CurrentDatabase=database;
        }

        #region SPCommandList
        public Dictionary<String , DbCommand> SPCommandList=new Dictionary<string , DbCommand>();
        public DbCommand GetSPCommand ( String strSPName )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strSPName ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            DbCommand cmd=null;
            if ( SPCommandList.TryGetValue( strSPName , out cmd ) )
                return cmd;

            cmd=db.GetStoredProcCommand( strSPName );
            SPCommandList.Add( strSPName , cmd );
            return cmd;
        }

        #endregion

        public DataSet RunStoredProcedure ( string strSPName )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strSPName ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( String.IsNullOrWhiteSpace( strSPName ) )
                return null;

            try
            {
                DbCommand cmd=GetSPCommand( strSPName );
                return db.ExecuteDataSet( cmd );
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                    {
                        ShowDisconnectWaitingDialog();
                        return null;
                    }
                }
                ABCHelper.ABCMessageBox.Show( ex.Message );
                return null;
            }

        }

        public DataSet RunStoredProcedure ( string strSPName , params object[] values )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strSPName ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( String.IsNullOrWhiteSpace( strSPName ) )
                return null;

            try
            {
                return db.ExecuteDataSet( strSPName , values );
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                    {
                        ShowDisconnectWaitingDialog();
                        return null;
                    }
                }
                //ABCHelper.ABCMessageBox.Show( ex.Message );
                return null;
            }

        }
        public object RunStoredProcedure ( string strSPName , BusinessObject obj )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strSPName ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( String.IsNullOrWhiteSpace( strSPName ) )
                return null;
            try
            {
                DbCommand cmd=GetSPCommand( strSPName );
                AddParameterForObject( obj , cmd );

                db.ExecuteNonQuery( cmd );
                String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName );
                if ( !String.IsNullOrWhiteSpace( strIDCol ) )
                {
                    Guid ret=ABCHelper.DataConverter.ConvertToGuid( db.GetParameterValue( cmd , strIDCol ) );
                    return ret;
                }
                return null;
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                    {
                        ShowDisconnectWaitingDialog();
                        return 0;
                    }
                }

                ABCHelper.ABCMessageBox.Show( ex.Source+ex.Message , "Database Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return 0;
            }
        }

    }

    public class SQLiteDatabaseHelper : ABCDatabaseHelper
    {
        public SQLiteDatabaseHelper ( Database database )
        {
            CurrentDatabase=database;
        }

        public static String RepairSelectSQLite ( String strQuery )
        {
            if ( String.IsNullOrWhiteSpace( strQuery ) )
                return String.Empty;

            strQuery=strQuery.Replace( "GetDate()" , "DATETIME('now', 'localtime')" );

            #region RepairSelectTOP
            if ( strQuery.ToUpper().Contains( "TOP" )&&strQuery.Contains( "*" ) )
            {
                String strTemp=RepairSelectTOP( strQuery , "TOP" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;

                strTemp=RepairSelectTOP( strQuery , "Top" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;

                strTemp=RepairSelectTOP( strQuery , "TOp" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;

                strTemp=RepairSelectTOP( strQuery , "ToP" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;


                strTemp=RepairSelectTOP( strQuery , "tOP" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;


                strTemp=RepairSelectTOP( strQuery , "tOp" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;

                strTemp=RepairSelectTOP( strQuery , "toP" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;


                strTemp=RepairSelectTOP( strQuery , "top" );
                if ( String.IsNullOrWhiteSpace( strTemp )==false )
                    return strTemp;
            } 
            #endregion

            //strQuery=strQuery.Replace( ",N'" , ",'" );
            //strQuery=strQuery.Replace( ", N'" , ",'" );
            //strQuery=strQuery.Replace( "N'" , ",'" );
            return strQuery;
        }

        public static String RepairSelectTOP ( String strQuery , String strTop )
        {
            String[] lst1=strQuery.Split( new String[] { "*" } , StringSplitOptions.None );
            if ( lst1.Length<1 )
                return String.Empty;
            String strTemp=lst1[0];

            String[] lst2=strTemp.Split( new String[] { strTop } , StringSplitOptions.None );
            if ( lst2.Length<2 )
                return String.Empty;

            int iLIMIT=Convert.ToInt32( lst2[1].Replace( "(" , String.Empty ).Replace( ")" , String.Empty ) );
            if ( iLIMIT>0 )
            {
                strTemp=lst2[0]+"*"+lst1[1];
                strTemp+=String.Format( " LIMIT {0} " , iLIMIT );
                return strTemp;
            }

            return String.Empty;
        }

    }

    public class ABCDatabaseHelper
    {
        public Database CurrentDatabase;
        public ABCDatabaseHelper ( )
        {
        }
        public ABCDatabaseHelper ( Database database )
        {
            CurrentDatabase=database;
        }

        public bool IsSQLConnection ( )
        {
            return ( CurrentDatabase is Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase );
        }

        #region Query

        public DbCommand GetQuery ( String strQueryCommand )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strQueryCommand ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }
            try
            {
                return db.GetSqlStringCommand( strQueryCommand );
            }
            catch ( Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( "Have some mistakes with Database connection." );
            }
            return null;
        }
        // not caching Command
        public DataSet RunQuery ( String strQueryCommand )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strQueryCommand ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( DataQueryProvider.IsSQLConnection( db )==false )
            {
                strQueryCommand=SQLiteDatabaseHelper.RepairSelectSQLite( strQueryCommand );
                strQueryCommand=strQueryCommand.Replace( "N'" , "'" );
            }
            DbCommand cmd=GetQuery( strQueryCommand );
            if ( cmd==null )
                return null;

            return RunCommand( cmd );
        }
        public DataSet RunCommand ( DbCommand cmd )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( cmd.CommandText ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            try
            {
                return db.ExecuteDataSet( cmd );
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                    {
                        ShowDisconnectWaitingDialog();
                        return null;
                    }
                }
                //ABCHelper.ABCMessageBox.Show( ex.Message , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Stop );
                return null;
            }

        }
        public object RunCommand ( DbCommand cmd , string retVariable )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( cmd.CommandText ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }


            try
            {
                db.ExecuteNonQuery( cmd );
                return (object)db.GetParameterValue( cmd , retVariable );
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                    {
                        ShowDisconnectWaitingDialog();
                        return null;
                    }
                }
                ABCHelper.ABCMessageBox.Show( ex.Message );
                return null;
            }

        }
        public string ExecuteScript ( String strScript )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strScript ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            try
            {
                if ( DataQueryProvider.IsSQLConnection( db )==false )
                    strScript=SQLiteDatabaseHelper.RepairSelectSQLite( strScript );

                db.ExecuteNonQuery( CommandType.Text , strScript );
                return "Command excute succesfully!";
            }
            catch ( System.Exception ex )
            {
                if ( ex is System.Data.SqlClient.SqlException )
                {
                    if ( ( (System.Data.SqlClient.SqlException)ex ).ErrorCode==-2146232060&&ex.Message.Contains( "TCP Provider" ) )
                        ShowDisconnectWaitingDialog();
                }

                return ex.Message;
            }


        }

        public void AddParameterForObject ( BusinessObject obj , DbCommand cmd )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( cmd.CommandText ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( cmd.Parameters.Count==0 )
            {
                BusinessObjectHelper.InitPropertyList( obj.AATableName );
                foreach ( PropertyInfo property in BusinessObjectHelper.PropertyList[obj.AATableName].Values )
                {

                    if ( property.Name==DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName )&&cmd.CommandText.Contains( "Insert" ) )
                    {
                        db.AddOutParameter( cmd , property.Name , DbType.Guid , 64 );
                        //     AddOutParameter( cmd , property.Name , SqlDbType.Int , 8 );
                    }
                    else
                    {
                        object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , property );
                        if ( DataStructureProvider.IsForeignKey( obj.AATableName , property.Name ) )
                            if ( objValue==null||ABCHelper.DataConverter.ConvertToGuid( objValue)==Guid.Empty )
                                objValue=DBNull.Value;

                        if ( objValue==null )
                            objValue=DBNull.Value;

                        AddInParameter( cmd , property , objValue );
                    }
                }
            }
            else
            {
                foreach ( DbParameter param in cmd.Parameters )
                {
                    if ( param.Direction==ParameterDirection.Input )
                    {
                        String strFieldName=param.ParameterName.Replace( "@" , "" );

                        object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , strFieldName );
                        if ( DataStructureProvider.IsForeignKey( obj.AATableName , strFieldName ) )
                            if ( objValue==null||ABCHelper.DataConverter.ConvertToGuid( objValue )==Guid.Empty )
                                objValue=DBNull.Value;

                        if ( objValue==null )
                            objValue=DBNull.Value;

                        if ( param.Value!=objValue&&objValue.ToString()!=param.Value.ToString() )
                            db.SetParameterValue( cmd , param.ParameterName , objValue );
                    }
                }
            }

        }
        public void AddInParameter ( DbCommand cmd , PropertyInfo property , object objValue )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( cmd.CommandText ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            String strProperty=property.Name;

            if ( BusinessObjectHelper.IsBaseProperty( strProperty ) )
                return;
            if ( property.PropertyType.Equals( typeof( Guid ) )||property.PropertyType.Equals( typeof( Nullable<Guid> ) ) )
            {
                if ( objValue==null||objValue==DBNull.Value||ABCHelper.DataConverter.ConvertToGuid( objValue)==Guid.Empty )
                    db.AddInParameter( cmd , strProperty , DbType.Guid , null );
                else
                    db.AddInParameter( cmd , strProperty , DbType.Guid , objValue );
            }
            else if ( property.PropertyType.Equals( typeof( Int32 ) )||property.PropertyType.Equals( typeof( Nullable<int> ) ) )
            {
                if ( objValue==null||objValue==DBNull.Value||(int)objValue==int.MinValue )
                    db.AddInParameter( cmd , strProperty , DbType.Int32 , null );
                else
                    db.AddInParameter( cmd , strProperty , DbType.Int32 , objValue );
            }
            else if ( property.PropertyType.Equals( typeof( Boolean ) ) )
                db.AddInParameter( cmd , strProperty , DbType.Boolean , objValue );

            else if ( property.PropertyType.Equals( typeof( short ) ) )
                db.AddInParameter( cmd , strProperty , DbType.Int16 , objValue );

            else if ( property.PropertyType.Equals( typeof( double ) ) )
                db.AddInParameter( cmd , strProperty , DbType.Double , objValue );

            else if ( property.PropertyType.Equals( typeof( decimal ) ) )
                db.AddInParameter( cmd , strProperty , DbType.Decimal , objValue );

            else if ( property.PropertyType.Equals( typeof( byte[] ) ) )
                db.AddInParameter( cmd , strProperty , DbType.Binary , objValue );

            else if ( ( property.PropertyType.Equals( typeof( String ) ) )||( property.PropertyType.Equals( typeof( string ) ) ) )
                db.AddInParameter( cmd , strProperty , DbType.String , objValue );

            else if ( property.PropertyType.Equals( typeof( DateTime ) ) )
            {
                if ( (DateTime)objValue==DateTime.MinValue )
                    db.AddInParameter( cmd , strProperty , DbType.DateTime , new DateTime( 1 , 1 , 1 ) );
                else
                    db.AddInParameter( cmd , strProperty , DbType.DateTime , objValue );
            }

            else if ( property.PropertyType.Equals( typeof( Nullable<DateTime> ) ) )
            {
                if ( objValue==null||objValue==DBNull.Value||(DateTime)objValue==DateTime.MinValue )
                    db.AddInParameter( cmd , strProperty , DbType.DateTime , null );
                else
                    db.AddInParameter( cmd , strProperty , DbType.DateTime , objValue );
            }

        }

        #region  Stored Procedure

        #region GetMaxNoIndex

        public Dictionary<String , DbCommand> MaxIndexCommandList=new Dictionary<string , DbCommand>();
        public int GetMaxNoIndex ( string tableName )
        {
            if ( DataStructureProvider.IsTableColumn( tableName , "NoIndex" ) ==false)
                return -1;

            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsSystemTable( tableName ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            DbCommand cmd=null;
            if ( MaxIndexCommandList.TryGetValue( tableName , out cmd )==false )
            {
                String sqlCommand=String.Format( "SELECT Max(NoIndex) FROM [{0}]" , tableName );
                cmd=db.GetSqlStringCommand( sqlCommand );
                MaxIndexCommandList.Add( tableName , cmd );
            }

            DataSet ds=db.ExecuteDataSet( cmd );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                if ( ds.Tables[0].Rows[0][0]==System.DBNull.Value )
                    return 0;
                else
                    return Convert.ToInt32( ds.Tables[0].Rows[0][0] );
            }
            return -1;
        }

        #endregion


        #endregion

        #region Script

        #region SPCommandList
        public Dictionary<String , DbCommand> ScriptCommandList=new Dictionary<string , DbCommand>();
        public DbCommand GetScriptCommand ( String strScriptQuery )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strScriptQuery ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            DbCommand cmd=null;
            if ( ScriptCommandList.TryGetValue( strScriptQuery , out cmd ) )
                return cmd;

            cmd=db.GetSqlStringCommand( strScriptQuery );
            cmd.CommandType=CommandType.Text;

            ScriptCommandList.Add( strScriptQuery , cmd );
            return cmd;
        }

        #endregion


        public void SetParameterValues ( DbCommand cmd , params DbParameter[] values )
        {
            for ( int i=0; i<values.Length; i++ )
                cmd.Parameters.Add( values[i] );
        }


        // caching Command
        public DataSet RunScript ( string strScriptQuery )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strScriptQuery ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( String.IsNullOrWhiteSpace( strScriptQuery ) )
                return null;

            if ( DataQueryProvider.IsSQLConnection( db )==false )
                strScriptQuery=SQLiteDatabaseHelper.RepairSelectSQLite( strScriptQuery );


            try
            {
                DbCommand cmd=GetScriptCommand( strScriptQuery );
                return db.ExecuteDataSet( cmd );
            }
            catch ( System.Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( ex.Message );
                return null;
            }

        }
        public object RunScript ( string strScriptQuery , BusinessObject obj )
        {
            Database db=CurrentDatabase;
            if ( CurrentDatabase==null )
            {
                if ( DataStructureProvider.IsQuerySystemDB( strScriptQuery ) )
                    db=DataQueryProvider.SystemDatabase;
                else
                    db=DataQueryProvider.CompanyDatabase;
            }

            if ( String.IsNullOrWhiteSpace( strScriptQuery ) )
                return null;

            if ( DataQueryProvider.IsSQLConnection( db )==false )
                strScriptQuery=SQLiteDatabaseHelper.RepairSelectSQLite( strScriptQuery );


            try
            {
                DbCommand cmd=GetScriptCommand( strScriptQuery );
                AddParameterForObject( obj , cmd );

                db.ExecuteNonQuery( cmd );
                Guid ret=ABCHelper.DataConverter.ConvertToGuid( db.GetParameterValue( cmd , DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName ) ));
                return ret;
            }
            catch ( System.Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( ex.Source+ex.Message , "Database Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return 0;
            }
        }
        #endregion

        #endregion

        #region Disconnect Timer

        static Timer autoTestConTimer;
        public void ShowDisconnectWaitingDialog ( )
        {
            //ABCWaitingDialog.Caption="Waiting or pressing ESC to exit application.";
            //ABCWaitingDialog.Title="Disconnect to server....";
            //try
            //{

            //    ABCWaitingDialog.CleanStopEventHandler();
            //    ABCWaitingDialog.StopDialogEvent+=new ABCWaitingDialog.StopDialogDelegate( ABCWaitingDialog_StopDialogEvent );
            //}
            //catch ( System.Exception ex )
            //{

            //}

            //ABCWaitingDialog.Show();
            //Application.DoEvents();

            //if ( autoTestConTimer==null )
            //{
            //    autoTestConTimer=new Timer();
            //    autoTestConTimer.Tick+=new EventHandler( AutoTestConnectionTimer_Tick );
            //    autoTestConTimer.Interval=3000;
            //}
            //autoTestConTimer.Enabled=true;
        }

        void ABCWaitingDialog_StopDialogEvent ( )
        {
            //Process.GetCurrentProcess().Kill();
        }

        void AutoTestConnectionTimer_Tick ( object sender , EventArgs e )
        {
            //Application.DoEvents();
            //bool testDb=TestConnection();
            //if ( testDb )
            //{
            //    ABCWaitingDialog.Close();
            //    autoTestConTimer.Enabled=false;
            //}
        }
        #endregion

    }
}
