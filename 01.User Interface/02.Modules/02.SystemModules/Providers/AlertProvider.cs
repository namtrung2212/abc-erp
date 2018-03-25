using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ABCBusinessEntities;
using ABCProvider;
using ABCProvider;

namespace ABCProvider
{
    public  class AlertProvider
    {

        public static Dictionary<Guid , GEAlertsInfo> AlertList;
        public static Dictionary<Guid , String> AlertQueryList;

        public static ABCScreen.UI.AlertView ViewInstance=null;
        static System.Windows.Forms.Timer timer;
        public static void ShowAlerts ( )
        {
              if ( ViewInstance==null )
            ViewInstance=new ABCScreen.UI.AlertView();

            //if ( timer==null )
            //{
            //    timer=new System.Windows.Forms.Timer();
            //    timer.Interval=5000;
            //    timer.Tick+=new EventHandler(timer_Tick);
            //    timer.Start();
            //}
            if ( IsNeedAlerts() )
            {
            
                ABCScreen.ABCScreenManager.Instance.ShowForm( ViewInstance , false );
            }
        }

        //static void timer_Tick ( object sender , EventArgs e )
        //{
        //    ShowAlerts();           
        //}

        public static Dictionary<Guid , GEAlertsInfo> GetAlertConfigs ( Guid iUserID )
        {
            if ( AlertList!=null )
                return AlertList;

            AlertList=new Dictionary<Guid , GEAlertsInfo>();
            AlertQueryList=new Dictionary<Guid , string>();

            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    String strQuery=String.Format( @"SELECT  A.* FROM  GEAlerts A JOIN GEAlertUsers B ON A.GEAlertID = B.FK_GEAlertID AND  (B.FK_ADUserGroupID ='{0}' OR B.FK_ADUserID ='{1}') ORDER BY B.FK_ADUserID  DESC" , user.FK_ADUserGroupID.Value , user.ADUserID );
                    foreach( GEAlertsInfo alertInfo in new GEAlertsController().GetListByQuery( strQuery ).Cast<GEAlertsInfo>().ToList() )
                    {
                        if ( AlertList.ContainsKey( alertInfo.GEAlertID )==false )
                            AlertList.Add( alertInfo.GEAlertID , alertInfo );
                    }
                }
            }
            return AlertList;
        }

        public static String GetAlertQueryString ( Guid alertID )
        {
            if ( AlertList==null )
                GetAlertConfigs( ABCUserProvider.CurrentUser.ADUserID );

            if ( AlertList.ContainsKey( alertID )==false )
                return String.Empty;

            if ( AlertQueryList.ContainsKey( alertID ) )
                return AlertQueryList[alertID];

            GEAlertsInfo alertInfo=AlertList[alertID];
            if ( alertInfo==null||alertInfo.GetID()==null )
                return String.Empty;

            String strQuery=QueryGenerator.GenSelect( alertInfo.TableName , "*" , false );
            strQuery=QueryGenerator.AddCondition( strQuery , alertInfo.ConditionString );

            #region Filter with current User

            if ( alertInfo.ByUser )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( alertInfo.TableName , "ADUsers" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUser!=null )
                    strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentUser.ADUserID ) );
            }
            if ( alertInfo.ByUserGroup )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( alertInfo.TableName , "ADUsers" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUser!=null )
                {
                    if ( ABCUserProvider.CurrentUserGroup!=null )
                    {
                        String strQueryUserID=QueryGenerator.AddEqualCondition( QueryGenerator.GenSelect( "ADUsers" , "ADUserID" , false ) , "FK_ADUserGroupID" , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
                        strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN ({1})" , strFK , strQueryUserID ) );
                    }
                }
                else
                {
                    strFK=DataStructureProvider.GetForeignKeyOfTableName( alertInfo.TableName , "ADUserGroups" );
                    if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUserGroup!=null )
                        strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentUserGroup.ADUserGroupID ) );
                }
            }
            if ( alertInfo.ByEmployee )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( alertInfo.TableName , "HREmployees" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentEmployee!=null )
                    strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentEmployee.HREmployeeID ) );
            }
            if ( alertInfo.ByCompanyUnit )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( alertInfo.TableName , "GECompanyUnits" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentCompanyUnit!=null )
                    strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID ) );
                else
                {
                    if ( !String.IsNullOrWhiteSpace( strFK )&&!String.IsNullOrWhiteSpace( DataStructureProvider.GetForeignKeyOfTableName( "HREmployees" , "GECompanyUnits" ) ) )
                    {
                        String strTemp=QueryGenerator.GenSelect( "GECompanyUnits" , DataStructureProvider.GetForeignKeyOfTableName( "HREmployees" , "GECompanyUnits" ) , false );
                        strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} IN ({1})" , strFK , strTemp ) );
                    }
                }
            }
            #endregion
            
            
            if ( DataStructureProvider.IsTableColumn( alertInfo.TableName , ABCCommon.ABCConstString.colDocumentDate ) )
                strQuery=strQuery+String.Format( @" ORDER BY {0} DESC" , ABCCommon.ABCConstString.colDocumentDate );
            
            AlertQueryList.Add( alertID , strQuery );

            return strQuery;
        }
        public static Boolean IsNeedAlerts ( )
        {
            if ( AlertList==null )
                GetAlertConfigs( ABCUserProvider.CurrentUser.ADUserID );
        
            foreach ( Guid id in AlertList.Keys )
                if ( IsNeedAlert( id ) )
                    return true;

            return false;
        }
        public static Boolean IsNeedAlert ( Guid alertID )
        {
            if ( AlertList==null )
                GetAlertConfigs( ABCUserProvider.CurrentUser.ADUserID );

            if ( AlertList.ContainsKey( alertID )==false )
                return false;

            GEAlertsInfo alertInfo=AlertList[alertID];
            if ( alertInfo==null||alertInfo.GetID()==null )
                return false;

            String strQuery=QueryGenerator.GenSelect( alertInfo.TableName , "COUNT(*)" , false );
            strQuery=QueryGenerator.AddCondition( strQuery , alertInfo.ConditionString );

            object obj=BusinessObjectController.GetData( strQuery );
            if ( obj!=null&&obj!=DBNull.Value )
                return Convert.ToInt32( obj )>0;

            return false;
        }

        public static List<BusinessObject> GetAlertData ( Guid alertID )
        {
            if ( AlertList.ContainsKey( alertID )==false )
                return null;

            String strQuery=GetAlertQueryString( alertID );
            GEAlertsInfo alertInfo=AlertList[alertID];

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( alertInfo.TableName );
            if ( ctrl==null )
                return null;

            return ctrl.GetListByQuery( strQuery );
        }

        public static DataSet GetAlertDataSet ( Guid alertID )
        {
            if ( AlertList.ContainsKey( alertID )==false )
                return null;

            String strQuery=GetAlertQueryString( alertID );
            GEAlertsInfo alertInfo=AlertList[alertID];

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( alertInfo.TableName );
            if ( ctrl==null )
                return null;

            return ctrl.GetDataSet( strQuery );
        }

    }
}
