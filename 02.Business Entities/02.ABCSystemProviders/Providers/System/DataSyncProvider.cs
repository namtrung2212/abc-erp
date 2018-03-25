using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ABCBusinessEntities;
using ABCProvider;

namespace ABCProvider
{
    public class DataSyncProvider
    {
        public static List<STDataServerSyncsInfo> DataSyncList;

        public static Boolean IsNeedSync ( )
        {
            if ( DataSynTimer==null )
            {
                DataSynTimer=new System.Timers.Timer();
                DataSynTimer.Interval=1000*60;
                DataSynTimer.Elapsed+=new System.Timers.ElapsedEventHandler( DataSynTimer_Tick );
                DataSynTimer.Start();
            }

            foreach ( STDataServerSyncsInfo syncInfo in new STDataServerSyncsController().GetListAllObjects() )
            {
                if(syncInfo.IsPull || syncInfo.IsPush)
                    if(!syncInfo.LastSyncDate.HasValue || !syncInfo.IntervalMinute.HasValue || 
                        (DateTime.Now > syncInfo.LastSyncDate.Value && DateTime.Now.Subtract(syncInfo.LastSyncDate.Value).TotalMinutes >=syncInfo.IntervalMinute.Value))
                    {
                        return true;
                    }
            }

            return false;
        }
        static System.Timers.Timer DataSynTimer=null;
        static void DataSynTimer_Tick ( object sender , System.Timers.ElapsedEventArgs e )
        {
            if ( IsNeedSync() )
                Synchronize();
        }

        public static void Synchronize ( )
        {
            ABCHelper.ABCWaitingDialog.Show( "Đồng bộ dữ liệu..." , "Vui lòng đợi.." );
          
            InitConnections();

            foreach ( STDataServerSyncsInfo syncInfo in new STDataServerSyncsController().GetListAllObjects().Cast<STDataServerSyncsInfo>().ToList().OrderBy(s=>s.OrderIndex ).OrderBy(s=>s.FK_STDataServerID.Value).ToList())
                Synchronize( syncInfo );

            ABCHelper.ABCWaitingDialog.Close();
        }
        public static void Synchronize ( STDataServersInfo server )
        {
            InitCompanyConnection( server.STDataServerID );
            InitSystemConnection( server.STDataServerID );

            foreach ( STDataServerSyncsInfo syncInfo in new STDataServerSyncsController().GetListByForeignKey( "FK_STDataServerID" , server.STDataServerID ).Cast<STDataServerSyncsInfo>().ToList().OrderBy( s => s.OrderIndex ).OrderBy( s => s.FK_STDataServerID.Value ).ToList() )
                Synchronize( syncInfo );
        }
        public static void Synchronize ( STDataServerSyncsInfo syncInfo )
        {
            if ( !syncInfo.FK_STDataServerID.HasValue )
                return;

              if(!syncInfo.IsPull && !syncInfo.IsPush)
                  return;

              if ( syncInfo.LastSyncDate.HasValue&&syncInfo.IntervalMinute.HasValue&&
                  ( DateTime.Now<syncInfo.LastSyncDate.Value||DateTime.Now.Subtract( syncInfo.LastSyncDate.Value ).TotalMinutes<syncInfo.IntervalMinute.Value ) )
                  return;

            STDataServersController serverCtrl=new STDataServersController();
            STDataServersInfo server=serverCtrl.GetObjectByID( syncInfo.FK_STDataServerID.Value ) as STDataServersInfo;
            if ( server==null )
                return;

            DBConnectionController connection=null;
            if ( DataStructureProvider.IsSystemTable( syncInfo.TableName ) )
                connection=InitSystemConnection( server.STDataServerID );
            else
                connection=InitCompanyConnection( server.STDataServerID );

            if ( connection!=null )
            {
                using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
                {
                    String strRunQuery=@"SET XACT_ABORT ON;BEGIN TRANSACTION ABCSYNC; ";
                    String strQuery=String.Empty;

                    if ( syncInfo.IsPush )
                    {
                        if ( syncInfo.IsPushMatched )
                        {
                            #region Delete
                            strQuery=String.Format( @"DELETE [{0}].[{1}].[dbo].[{2}] WHERE [{3}]  NOT IN (SELECT [{3}] FROM [dbo].[{2}] )" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) );
                            strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );
                            #endregion
                        }

                        #region Insert New Records
                        strQuery=QueryGenerator.GenSelect( syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , false , false );
                        if ( syncInfo.LastSyncDate.HasValue&&DataStructureProvider.IsTableColumn( syncInfo.TableName , ABCCommon.ABCConstString.colCreateTime ) )
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} > '{1}'" , ABCCommon.ABCConstString.colCreateTime , syncInfo.LastSyncDate.Value.ToString( "yyyy-MM-dd HH:mm:ss" ) ) );

                        strQuery=String.Format( @"INSERT INTO [{0}].[{1}].[dbo].[{2}] SELECT * FROM [dbo].[{2}] WHERE [dbo].[{2}].[{3}] IN ({4}) AND [dbo].[{2}].[{3}] NOT IN (SELECT {3} FROM  [{0}].[{1}].[dbo].[{2}])" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , strQuery );
                        strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );
                        #endregion

                        #region Update Modified Records

                        strQuery=QueryGenerator.GenSelect( syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , false , false );
                        if ( syncInfo.LastSyncDate.HasValue&&DataStructureProvider.IsTableColumn( syncInfo.TableName , ABCCommon.ABCConstString.colUpdateTime ) )
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} > '{1}'" , ABCCommon.ABCConstString.colUpdateTime , syncInfo.LastSyncDate.Value.ToString( "yyyy-MM-dd HH:mm:ss" ) ) );

                        strQuery=String.Format( @"UPDATE [{0}].[{1}].[dbo].[{2}] SET #FIELD# FROM [{0}].[{1}].[dbo].[{2}] Target,[dbo].[{2}] Source WHERE Target.[{3}]=Source.[{3}] AND Source.[{3}] IN ({4}) AND Source.[{3}] IN (SELECT {3} FROM  [{0}].[{1}].[dbo].[{2}])" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , strQuery );

                        List<String> lstSetFields=new List<string>();
                        foreach ( String strField in DataStructureProvider.GetAllTableColumns( syncInfo.TableName ).Keys )
                        {
                            if ( DataStructureProvider.IsPrimaryKey( syncInfo.TableName , strField )==false )
                                lstSetFields.Add( String.Format( " [{0}]=Source.[{0}] " , strField ) );
                        }
                        strQuery=strQuery.Replace( "#FIELD#" , String.Join( "," , lstSetFields ) );

                        strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );

                        #endregion

                    }
                    if ( syncInfo.IsPull )
                    {
                        if ( syncInfo.IsPullMatched )
                        {
                            #region Delete
                            strQuery=String.Format( @"DELETE [dbo].[{2}] WHERE [{3}]  NOT IN (SELECT [{3}] FROM [{0}].[{1}].[dbo].[{2}] )" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) );
                            strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );
                            #endregion
                        }

                        #region Insert New Records
                        strQuery=QueryGenerator.GenSelect( syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , false , false );
                        if ( syncInfo.LastSyncDate.HasValue&&DataStructureProvider.IsTableColumn( syncInfo.TableName , ABCCommon.ABCConstString.colCreateTime ) )
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} > '{1}'" , ABCCommon.ABCConstString.colCreateTime , syncInfo.LastSyncDate.Value.ToString( "yyyy-MM-dd HH:mm:ss" ) ) );
                        strQuery=strQuery.Replace( "[dbo]" , String.Format( @"[{0}].[{1}].[dbo]" , server.Name , connection.Connection.DatabaseName ) );

                        strQuery=String.Format( @"INSERT INTO [dbo].[{2}] SELECT * FROM [{0}].[{1}].[dbo].[{2}] WHERE [{3}] IN ({4}) AND [{3}] NOT IN (SELECT {3} FROM [dbo].[{2}])" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , strQuery );
                        strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );
                        #endregion

                        #region Update Modified Records

                        strQuery=QueryGenerator.GenSelect( syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , false , false );
                        if ( syncInfo.LastSyncDate.HasValue&&DataStructureProvider.IsTableColumn( syncInfo.TableName , ABCCommon.ABCConstString.colUpdateTime ) )
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} > '{1}'" , ABCCommon.ABCConstString.colUpdateTime , syncInfo.LastSyncDate.Value.ToString( "yyyy-MM-dd HH:mm:ss" ) ) );

                        strQuery=strQuery.Replace( "[dbo]" , String.Format( @"[{0}].[{1}].[dbo]" , server.Name , connection.Connection.DatabaseName ) );
                        strQuery=String.Format( @"UPDATE [dbo].[{2}] SET #FIELD# FROM [dbo].[{2}] Target,[{0}].[{1}].[dbo].[{2}] Source WHERE Target.[{3}]=Source.[{3}] AND Source.[{3}] IN ({4}) AND Source.[{3}] IN (SELECT {3} FROM [dbo].[{2}])" , server.Name , connection.Connection.DatabaseName , syncInfo.TableName , DataStructureProvider.GetPrimaryKeyColumn( syncInfo.TableName ) , strQuery );

                        List<String> lstSetFields=new List<string>();
                        foreach ( String strField in DataStructureProvider.GetAllTableColumns( syncInfo.TableName ).Keys )
                        {
                            if ( DataStructureProvider.IsPrimaryKey( syncInfo.TableName , strField )==false )
                                lstSetFields.Add( String.Format( " [{0}]=Source.[{0}] " , strField ) );
                        }
                        strQuery=strQuery.Replace( "#FIELD#" , String.Join( "," , lstSetFields ) );

                        strRunQuery=String.Format( @"{0} {1}" , strRunQuery , Environment.NewLine+strQuery );

                        #endregion

                    }

                    strRunQuery=String.Format( @"{0} COMMIT TRANSACTION ABCSYNC; SET XACT_ABORT OFF;" , strRunQuery+Environment.NewLine );
                    BusinessObjectController.RunQuery( strRunQuery , syncInfo.TableName );

                    BusinessObjectController.RunQuery( String.Format( @"UPDATE STDataServerSyncs SET LastSyncDate =GetDate() WHERE STDataServerSyncID='{0}'" , syncInfo.GetID() ) , syncInfo.AATableName );

                    scope.Complete();
                }
            }
        }

        #region InitConnection
        static Dictionary<Guid , DBConnectionController> CompanyConnections=new Dictionary<Guid , DBConnectionController>();
        static Dictionary<Guid , DBConnectionController> SystemConnections=new Dictionary<Guid , DBConnectionController>();

        public static DBConnectionController InitCompanyConnection ( Guid serverID )
        {
            STDataServersController serverCtrl=new STDataServersController();
            STDataServersInfo server=serverCtrl.GetObjectByID( serverID ) as STDataServersInfo;
            if ( server==null )
                return null;

            if ( CompanyConnections.ContainsKey( serverID ) )
                if ( CompanyConnections[serverID].Connection.TestConnection() )
                    return CompanyConnections[serverID];

            DBConnectionController connection=new DBConnectionController( server.Name , server.Name );
            connection.InitNewConnection( server.Name , DatabaseType.MSSQL , server.ServerAddress , server.CompanyDatabase , server.UserName , server.EncryptedPassword , false );
            connection.Connect();
            if ( connection.Connection.TestConnection() )
            {
                CompanyConnections.Add( serverID , connection );
                return connection;
            }

            return null;
        }
        public static DBConnectionController InitSystemConnection ( Guid serverID )
        {
            STDataServersController serverCtrl=new STDataServersController();
            STDataServersInfo server=serverCtrl.GetObjectByID( serverID ) as STDataServersInfo;
            if ( server==null )
                return null;

            if ( SystemConnections.ContainsKey( serverID ) )
                if ( SystemConnections[serverID].Connection.TestConnection() )
                    return SystemConnections[serverID];

            DBConnectionController connection=new DBConnectionController( server.Name , server.Name );
            connection.InitNewConnection( server.Name , DatabaseType.MSSQL , server.ServerAddress , server.SystemDatabase , server.UserName , server.EncryptedPassword , false );
            connection.Connect();
            if ( connection.Connection.TestConnection() )
            {
                SystemConnections.Add( serverID , connection );
                return connection;
            }

            return null;
        }
     
        [ABCRefreshTable( "STDataServers" ,"STDataServerSyncs" )]
        public static void InitConnections ( )
        {
            STDataServersController serverCtrl=new STDataServersController();
            foreach ( STDataServerSyncsInfo syncInfo in new STDataServerSyncsController().GetListAllObjects().Cast<STDataServerSyncsInfo>().ToList() )
            {
                if ( !syncInfo.FK_STDataServerID.HasValue )
                    continue;

                STDataServersInfo server=serverCtrl.GetObjectByID( syncInfo.FK_STDataServerID.Value ) as STDataServersInfo;
                if ( server==null )
                    return;

                #region Init Linked Server
                Security.Cryptography cryp=new Security.Cryptography();
                String strPassword=cryp.Decrypt( server.EncryptedPassword );

                String strQuery=String.Format( @"IF EXISTS(SELECT * FROM sys.servers WHERE name = '{0}') EXEC master.sys.sp_dropserver '{0}','droplogins'  
                                                                EXEC master.dbo.sp_addlinkedserver
                                                                    @server = '{0}',
                                                                    @srvproduct=N'MSSQL',
                                                                    @provider=N'SQLNCLI',
                                                                    @provstr=N'PROVIDER=SQLOLEDB;SERVER={1}'
 
                                                                EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname='{0}',
                                                                @useself=N'False',@locallogin=NULL,@rmtuser=N'{2}',@rmtpassword='{3}'" , server.Name , server.ServerAddress , server.UserName , strPassword );
                BusinessObjectController.RunQuery( strQuery );

                #endregion

                InitCompanyConnection( server.STDataServerID );
                InitSystemConnection( server.STDataServerID );
            }
        }
        #endregion
    }
}
