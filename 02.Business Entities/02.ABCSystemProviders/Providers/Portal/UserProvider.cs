using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using ABCProvider;
using ABCProvider;
using ABCCommon;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class ABCUserInfo
    {
        public bool Select { get; set; }
        public Guid UserID { get; set; }
        public String User { get; set; }
        public String Employee { get; set; }
        public String CompanyUnit { get; set; }
        public bool IsOnline { get; set; }
    }

    public class ABCUserProvider
    {
        public static ADUsersInfo CurrentUser;
        public static ADUserGroupsInfo CurrentUserGroup;
        public static HREmployeesInfo CurrentEmployee;
        public static GECompanyUnitsInfo CurrentCompanyUnit;

        static String _CurrentUser;
        public static String CurrentUserName
        {
            get { return _CurrentUser; }
            set
            {
                _CurrentUser=value;
                ABCBaseUserProvider.CurrentUserName=value;
            }
        }

        static String _CurrentEmployeeNamer;
        public static String CurrentEmployeeName
        {
            get { return _CurrentEmployeeNamer; }
            set
            {
                _CurrentEmployeeNamer=value;
                ABCBaseUserProvider.CurrentEmployeeName=value;
            }
        }

        public static List<ABCUserInfo> UserList=new List<ABCUserInfo>();
        [ABCRefreshTable( "ABCUsers" )]
        public static List<ABCUserInfo> GetAllUsers ( bool isCheckOnline , bool isExceptCurrentUser )
        {
            UserList.Clear();
            String strQuery=String.Empty;
            if ( isCheckOnline==false )
                strQuery=@"SELECT ADUserID,ADUsers.No,HREmployees.Name,GECompanyUnits.No FROM ADUsers,HREmployees,GECompanyUnits
                                        WHERE ADUsers.ABCStatus ='Alive' AND ADUsers.Active =1 
                                        AND FK_HREmployeeID =HREmployeeID
                                        AND HREmployees.FK_GECompanyUnitID	= GECompanyUnitID";
            else
                strQuery=@"SELECT ADUserID,ADUsers.No as UserNo,HREmployees.Name as Employee,GECompanyUnits.No as CompanyUnit, IsOnline FROM ADUsers,HREmployees,GECompanyUnits,ADUserStatuss
                                        WHERE ADUsers.ABCStatus ='Alive' AND ADUsers.Active =1 
                                        AND FK_HREmployeeID =HREmployeeID
                                        AND HREmployees.FK_GECompanyUnitID	= GECompanyUnitID
                                        AND FK_ADUserID=ADUserID";

            DataSet ds=ABCBusinessEntities.BusinessObjectController.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    ABCUserInfo user=new ABCUserInfo();
                    user.Select=false;
                    user.UserID=ABCHelper.DataConverter.ConvertToGuid( dr[0] );
                    user.User=dr[1].ToString();
                    user.Employee=dr[2].ToString();
                    user.CompanyUnit=dr[3].ToString();
                    if ( isCheckOnline )
                        user.IsOnline=Convert.ToBoolean( dr[4] );
                    if ( isExceptCurrentUser==false||user.User!=CurrentUserName )
                        UserList.Add( user );
                }
            }

            return UserList;
        }
        public static bool IsOnline ( String strUser )
        {
            foreach ( ABCUserInfo user in UserList )
                if ( user.User==strUser )
                    return user.IsOnline;

            return false;
        }

        #region Check Permissions

        static Dictionary<String , bool> ViewPermissionList=new Dictionary<string , bool>();
        static Dictionary<String , bool> TablePermissionList=new Dictionary<string , bool>();
        static Dictionary<String , bool> FieldPermissionList=new Dictionary<string , bool>();
        static Dictionary<String , bool> VoucherPermissionList=new Dictionary<string , bool>();

        public static bool CheckVoucherPermission ( Guid iUserID , String strTableName , VoucherPermission permission )
        {
            BusinessObject obj=VoucherProvider.GetConfig( strTableName );
            if ( obj!=null )
                return CheckVoucherPermission( iUserID , obj.GetID() , permission );

            return true;
        }
        public static bool CheckVoucherPermission ( Guid iUserID , String strTableName , Guid ID , VoucherPermission permission )
        {
            BusinessObject obj=VoucherProvider.GetConfig( strTableName , ID );
            if ( obj!=null )
                return CheckVoucherPermission( iUserID , obj.GetID() , permission );

            return true;
        }
        public static bool CheckVoucherPermission ( Guid iUserID , Guid voucherTypeID , VoucherPermission permission )
        {
            bool result=true;

            String strKey=iUserID.ToString()+voucherTypeID.ToString()+permission.ToString();
            if ( VoucherPermissionList.TryGetValue( strKey , out result ) )
                return result;

            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    result=( user.No=="sysadmin" );
                    String strQuery=String.Format( @"SELECT A.* FROM  GEPermissionVouchers A JOIN ADUserPermissions B ON A.FK_GEVoucherID ='{0}' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{1}' OR B.FK_ADUserID ='{2}') ORDER BY B.FK_ADUserID  DESC" , voucherTypeID , user.FK_ADUserGroupID.Value , user.ADUserID );
                    foreach ( GEPermissionVouchersInfo voucherPermission in new GEPermissionVouchersController().GetList( strQuery ).Cast<GEPermissionVouchersInfo>().ToList() )
                    {

                        switch ( permission )
                        {
                            case VoucherPermission.AllowView:
                                result=( result||voucherPermission.AllowView );
                                break;

                            case VoucherPermission.AllowNew:
                                result=( result||voucherPermission.AllowNew );
                                break;

                            case VoucherPermission.AllowEdit:
                                result=( result||voucherPermission.AllowEdit );
                                break;

                            case VoucherPermission.AllowDelete:
                                result=( result||voucherPermission.AllowDelete );
                                break;

                            case VoucherPermission.AllowApproval:
                                result=( result||voucherPermission.AllowApproval );
                                break;

                            case VoucherPermission.AllowLock:
                                result=( result||voucherPermission.AllowLock );
                                break;

                            case VoucherPermission.AllowPost:
                                result=( result||voucherPermission.AllowPost );
                                break;

                            case VoucherPermission.AllowPrint:
                                result=( result||( voucherPermission.AllowPrint&&!String.IsNullOrWhiteSpace( voucherPermission.ReportName ) ) );
                                break;
                        }
                    }
                }
            }
            if ( SystemProvider.SystemConfig.IsRelease )
                VoucherPermissionList.Add( strKey , result );
            return result;
        }

        public static bool CheckViewPermission ( Guid iUserID , Guid viewID , ViewPermission permission )
        {
            bool result=false;

            String strKey=iUserID.ToString()+viewID.ToString()+permission.ToString();
            if ( ViewPermissionList.TryGetValue( strKey , out result ) )
                return result;

            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    result=( user.No=="sysadmin" );
                    String strQuery=String.Format( @"SELECT  A.* FROM  GEPermissionViews A JOIN ADUserPermissions B ON A.FK_STViewID ='{0}' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{1}' OR B.FK_ADUserID ='{2}') ORDER BY B.FK_ADUserID  DESC" , viewID , user.FK_ADUserGroupID.Value , user.ADUserID );
                    foreach ( GEPermissionViewsInfo viewPermission in new GEPermissionViewsController().GetList( strQuery ).Cast<GEPermissionViewsInfo>().ToList() )
                    {

                        switch ( permission )
                        {
                            case ViewPermission.AllowView:
                                result=( result||viewPermission.AllowView );
                                break;
                        }
                    }
                }
            }

            if ( SystemProvider.SystemConfig.IsRelease )
                ViewPermissionList.Add( strKey , result );
            return result;
        }

        public static List<Guid> GetViews ( Guid iUserID )
        {
            List<Guid> lstViewIDs=new List<Guid>();
            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    String strQuery=String.Format( @"SELECT A.FK_STViewID FROM  GEPermissionViews A JOIN ADUserPermissions B ON A.AllowView ='TRUE' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{0}' OR B.FK_ADUserID ='{1}') ORDER BY B.FK_ADUserID  DESC" , user.FK_ADUserGroupID.Value , user.ADUserID );
                    DataSet ds=new GEPermissionViewsController().GetDataSet( strQuery );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        foreach ( DataRow dr in ds.Tables[0].Rows )
                            if ( dr[0]!=DBNull.Value&&!lstViewIDs.Contains( ABCHelper.DataConverter.ConvertToGuid( dr[0].ToString() ) ) )
                                lstViewIDs.Add( ABCHelper.DataConverter.ConvertToGuid( dr[0].ToString() ) );
                }
            }

            return lstViewIDs;
        }

        public static bool CheckTablePermission ( Guid iUserID , String strTableName , TablePermission permission )
        {
            bool result=false;

            String strKey=iUserID.ToString()+strTableName+permission.ToString();
            if ( TablePermissionList.TryGetValue( strKey , out result ) )
                return result;

            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    result=( user.No=="sysadmin" );
                    String strQuery=String.Format( @"SELECT A.* FROM  GEPermissionTables A JOIN ADUserPermissions B ON A.TableName ='{0}' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{1}' OR B.FK_ADUserID ='{2}') ORDER BY B.FK_ADUserID  DESC" , strTableName , user.FK_ADUserGroupID.Value , user.ADUserID );
                    foreach ( GEPermissionTablesInfo tablePermission in new GEPermissionTablesController().GetList( strQuery ).Cast<GEPermissionTablesInfo>().ToList() )
                    {
                        switch ( permission )
                        {
                            case TablePermission.AllowView:
                                result=( result||tablePermission.AllowView );
                                break;

                            case TablePermission.AllowNew:
                                result=( result||tablePermission.AllowNew );
                                break;

                            case TablePermission.AllowEdit:
                                result=( result||tablePermission.AllowEdit );
                                break;

                            case TablePermission.AllowDelete:
                                result=( result||tablePermission.AllowDelete );
                                break;

                        }
                    }
                }
            }
            if ( SystemProvider.SystemConfig.IsRelease )
                TablePermissionList.Add( strKey , result );
            return result;
        }

        public static bool CheckFieldPermission ( Guid iUserID , String strTableName , String strFieldName , FieldPermission permission )
        {
            bool result=false;


            String strKey=iUserID.ToString()+strTableName+strFieldName+permission.ToString();
            if ( FieldPermissionList.TryGetValue( strKey , out result ) )
                return result;

            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    result=( user.No=="sysadmin" );
                    String strQuery=String.Format( @"SELECT TOP 1 A.* FROM  GEPermissionFields A JOIN ADUserPermissions B ON A.TableName ='{0}' AND A.FieldName ='{1}' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{2}' OR B.FK_ADUserID ='{3}') ORDER BY B.FK_ADUserID  DESC" , strTableName , strFieldName , user.FK_ADUserGroupID.Value , user.ADUserID );
                    foreach ( GEPermissionFieldsInfo fieldPermission in new GEPermissionFieldsController().GetList( strQuery ).Cast<GEPermissionFieldsInfo>().ToList() )
                    {

                        switch ( permission )
                        {
                            case FieldPermission.AllowView:
                                result=( result||fieldPermission.AllowView );
                                break;

                            case FieldPermission.AllowEdit:
                                result=( result||fieldPermission.AllowEdit );
                                break;

                        }
                    }
                }
            }
            if ( SystemProvider.SystemConfig.IsRelease )
                FieldPermissionList.Add( strKey , result );
            return result;
        }


        #endregion

        #region Generate
        [ABCRefreshTable( "GEPermissions" , "GEPermissionTables" )]
        public static void SynchronizeTablePermission ( )
        {
            GEPermissionTablesController permissionCtrl=new GEPermissionTablesController();
            String strQuery=String.Format( @"DELETE FROM GEPermissionTables WHERE FK_GEPermissionID NOT IN (SELECT GEPermissionID FROM GEPermissions)" );
            BusinessObjectController.RunQuery( strQuery );

            foreach ( GEPermissionsInfo permission in new GEPermissionsController().GetListAllObjects() )
            {
                #region Table
                Dictionary<String , GEPermissionTablesInfo> lstTables=new Dictionary<string , GEPermissionTablesInfo>();

                foreach ( GEPermissionTablesInfo tableInfo in permissionCtrl.GetListByForeignKey( "FK_GEPermissionID" , permission.GEPermissionID ).Cast<GEPermissionTablesInfo>().ToList() )
                {
                    if ( lstTables.ContainsKey( tableInfo.TableName )==false )
                    {
                        if ( DataStructureProvider.IsExistedTable( tableInfo.TableName ) )
                            lstTables.Add( tableInfo.TableName , tableInfo );
                        else
                            permissionCtrl.DeleteObject( tableInfo );
                    }
                }


                foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
                {
                    if ( lstTables.ContainsKey( strTableName )==false )
                    {
                        GEPermissionTablesInfo tableInfo=new GEPermissionTablesInfo();
                        tableInfo.FK_STTableConfigID=DataConfigProvider.TableConfigList[strTableName].ConfigID;
                        tableInfo.TableName=strTableName;
                        tableInfo.FK_GEPermissionID=permission.GEPermissionID;
                        tableInfo.AllowView=true;
                        tableInfo.AllowNew=true;
                        tableInfo.AllowEdit=true;
                        tableInfo.AllowDelete=true;

                        permissionCtrl.CreateObject( tableInfo );
                        lstTables.Add( tableInfo.TableName , tableInfo );
                    }
                }
                #endregion

            }
        }

        [ABCRefreshTable( "GEPermissions" , "GEPermissionFields" )]
        public static void SynchronizeFieldPermission ( )
        {
            GEPermissionFieldsController permissionCtrl=new GEPermissionFieldsController();
            String strQuery=String.Format( @"DELETE FROM GEPermissionFields WHERE FK_GEPermissionID NOT IN (SELECT GEPermissionID FROM GEPermissions)" );
            BusinessObjectController.RunQuery( strQuery );

            foreach ( GEPermissionsInfo permission in new GEPermissionsController().GetListAllObjects() )
            {
                #region Field
                Dictionary<String , GEPermissionFieldsInfo> lstFields=new Dictionary<string , GEPermissionFieldsInfo>();

                foreach ( GEPermissionFieldsInfo fieldInfo in permissionCtrl.GetListByForeignKey( "FK_GEPermissionID" , permission.GEPermissionID ).Cast<GEPermissionFieldsInfo>().ToList() )
                {
                    if ( lstFields.ContainsKey( fieldInfo.FieldName )==false )
                    {

                        String strTableCaption=DataConfigProvider.GetTableCaption( fieldInfo.TableName );
                        String strFieldCaption=DataConfigProvider.GetFieldCaption( fieldInfo.TableName , fieldInfo.FieldName );

                        if ( DataStructureProvider.IsTableColumn( fieldInfo.TableName , fieldInfo.FieldName )
                            &&strTableCaption!=String.Empty&&strTableCaption!=fieldInfo.TableName
                            &&strFieldCaption!=String.Empty&&strFieldCaption!=fieldInfo.FieldName )
                            lstFields.Add( fieldInfo.TableName+fieldInfo.FieldName , fieldInfo );
                        else
                            permissionCtrl.DeleteObject( fieldInfo );
                    }
                }


                foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
                {
                    foreach ( String strFieldName in DataStructureProvider.DataTablesList[strTableName].ColumnsList.Keys )
                    {
                        if ( DataStructureProvider.IsPrimaryKey( strTableName , strFieldName ) )
                            continue;

                        if ( lstFields.ContainsKey( strTableName+strFieldName )==false )
                        {
                            GEPermissionFieldsInfo fieldInfo=new GEPermissionFieldsInfo();
                            fieldInfo.FK_STFieldConfigID=DataConfigProvider.TableConfigList[strTableName].FieldConfigList[strFieldName].ConfigID;
                            fieldInfo.FK_STTableConfigID=DataConfigProvider.TableConfigList[strTableName].ConfigID;
                            fieldInfo.TableName=strTableName;
                            fieldInfo.FieldName=strFieldName;
                            fieldInfo.FK_GEPermissionID=permission.GEPermissionID;
                            fieldInfo.AllowView=true;
                            fieldInfo.AllowEdit=true;

                            permissionCtrl.CreateObject( fieldInfo );
                            lstFields.Add( fieldInfo.TableName+fieldInfo.FieldName , fieldInfo );
                        }
                    }
                }
                #endregion
            }
        }
        [ABCRefreshTable( "GEPermissions" , "GEPermissionViews" )]
        public static void SynchronizeViewPermission ( )
        {
            GEPermissionViewsController permissionCtrl=new GEPermissionViewsController();
            String strQuery=String.Format( @"DELETE FROM GEPermissionViews WHERE FK_GEPermissionID NOT IN (SELECT GEPermissionID FROM GEPermissions)" );
            BusinessObjectController.RunQuery( strQuery );


            #region Init Views
            STViewsController viewCtrl=new STViewsController();

            Dictionary<Guid , STViewsInfo> lstSTViews=new Dictionary<Guid , STViewsInfo>();
            DataSet ds=viewCtrl.GetDataSetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    STViewsInfo viewInfo=viewCtrl.GetObjectFromDataRow( dr ) as STViewsInfo;
                    if ( lstSTViews.ContainsKey( viewInfo.STViewID )==false )
                        lstSTViews.Add( viewInfo.STViewID , viewInfo );
                }
            }
            #endregion

            foreach ( GEPermissionsInfo permission in new GEPermissionsController().GetListAllObjects() )
            {
                #region View
                Dictionary<Guid , GEPermissionViewsInfo> lstGroupViews=new Dictionary<Guid , GEPermissionViewsInfo>();
                foreach ( GEPermissionViewsInfo viewInfo in permissionCtrl.GetListByForeignKey( "FK_GEPermissionID" , permission.GEPermissionID ).Cast<GEPermissionViewsInfo>().ToList() )
                {
                    if ( viewInfo.FK_STViewID.HasValue&&lstGroupViews.ContainsKey( viewInfo.FK_STViewID.Value )==false )
                    {
                        if ( lstSTViews.ContainsKey( viewInfo.FK_STViewID.Value ) )
                            lstGroupViews.Add( viewInfo.FK_STViewID.Value , viewInfo );
                        else
                            permissionCtrl.DeleteObject( viewInfo );
                    }

                }


                foreach ( Guid strViewID in lstSTViews.Keys )
                {
                    if ( lstGroupViews.ContainsKey( strViewID )==false )
                    {
                        GEPermissionViewsInfo viewInfo=new GEPermissionViewsInfo();
                        viewInfo.FK_STViewID=lstSTViews[strViewID].STViewID;

                        if ( lstSTViews[strViewID].FK_STViewGroupID.HasValue )
                            viewInfo.ViewGroup=GetViewGroupCaption( lstSTViews[strViewID].FK_STViewGroupID.Value );
                        viewInfo.FK_GEPermissionID=permission.GEPermissionID;

                        viewInfo.AllowView=true;
                        viewInfo.IsHomePage=false;

                        permissionCtrl.CreateObject( viewInfo );
                        lstGroupViews.Add( viewInfo.FK_STViewID.Value , viewInfo );
                    }
                }
                #endregion
            }
        }
        [ABCRefreshTable( "GEPermissions" , "GEPermissionVouchers" )]
        public static void SynchronizeVoucherPermission ( )
        {
            GEPermissionVouchersController voucherPermissionCtrl=new GEPermissionVouchersController();

            String strQuery=String.Format( @"DELETE FROM GEPermissionVouchers WHERE FK_GEPermissionID NOT IN (SELECT GEPermissionID FROM GEPermissions)" );
            BusinessObjectController.RunQuery( strQuery );

            strQuery=String.Format( @"DELETE FROM GEPermissionVouchers WHERE FK_GEVoucherID NOT IN ({0})" , QueryGenerator.GenSelect( "GEVouchers" , "GEVoucherID" , false ) );
            BusinessObjectController.RunQuery( strQuery );

            #region Init Vouchers
            GEVouchersController voucherCtrl=new GEVouchersController();

            Dictionary<Guid , GEVouchersInfo> lstGEVouchers=new Dictionary<Guid , GEVouchersInfo>();
            DataSet ds=voucherCtrl.GetDataSetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    GEVouchersInfo voucherInfo=voucherCtrl.GetObjectFromDataRow( dr ) as GEVouchersInfo;
                    if ( lstGEVouchers.ContainsKey( voucherInfo.GEVoucherID )==false )
                        lstGEVouchers.Add( voucherInfo.GEVoucherID , voucherInfo );
                }
            }
            #endregion

            foreach ( GEPermissionsInfo permission in new GEPermissionsController().GetListAllObjects() )
            {
                #region Voucher
                Dictionary<Guid , GEPermissionVouchersInfo> lstGroupVouchers=new Dictionary<Guid , GEPermissionVouchersInfo>();
                foreach ( GEPermissionVouchersInfo voucherInfo in voucherPermissionCtrl.GetListByForeignKey( "FK_GEPermissionID" , permission.GEPermissionID ).Cast<GEPermissionVouchersInfo>().ToList() )
                {
                    if ( voucherInfo.FK_GEVoucherID.HasValue&&lstGroupVouchers.ContainsKey( voucherInfo.FK_GEVoucherID.Value )==false )
                    {
                        if ( lstGEVouchers.ContainsKey( voucherInfo.FK_GEVoucherID.Value ) )
                            lstGroupVouchers.Add( voucherInfo.FK_GEVoucherID.Value , voucherInfo );
                        else
                            voucherPermissionCtrl.DeleteObject( voucherInfo );
                    }
                }


                foreach ( Guid voucherTypeID in lstGEVouchers.Keys )
                {
                    if ( lstGroupVouchers.ContainsKey( voucherTypeID )==false&&lstGEVouchers[voucherTypeID].Title!=String.Empty )
                    {
                        GEPermissionVouchersInfo voucherInfo=new GEPermissionVouchersInfo();
                        voucherInfo.FK_GEVoucherID=voucherTypeID;
                        voucherInfo.FK_GEPermissionID=permission.GEPermissionID;

                        voucherInfo.AllowView=true;

                        voucherInfo.AllowNew=true;
                        voucherInfo.AllowEdit=true;
                        voucherInfo.AllowDelete=true;
                        voucherInfo.AllowApproval=true;
                        voucherInfo.AllowLock=true;
                        voucherInfo.AllowPost=true;

                        voucherPermissionCtrl.CreateObject( voucherInfo );
                        lstGroupVouchers.Add( voucherTypeID , voucherInfo );
                    }
                }
                #endregion
            }

        }

        public static void SynchronizePermission ( )
        {
            SynchronizeViewPermission();
            SynchronizeVoucherPermission();
            SynchronizeTablePermission();
            SynchronizeFieldPermission();
        }

        static Dictionary<Guid , String> lstViewGroupCaptions=new Dictionary<Guid , string>();
        public static String GetViewGroupCaption ( Guid iViewGroupID )
        {
            if ( lstViewGroupCaptions.ContainsKey( iViewGroupID ) )
                return lstViewGroupCaptions[iViewGroupID];

            STViewGroupsInfo groupInfo=new STViewGroupsController().GetObjectByID( iViewGroupID ) as STViewGroupsInfo;
            if ( groupInfo==null )
                return String.Empty;

            String strCaption=groupInfo.Name;
            if ( groupInfo.FK_STViewGroupID.HasValue )
                strCaption=GetViewGroupCaption( groupInfo.FK_STViewGroupID.Value )+" > "+groupInfo.Name;

            lstViewGroupCaptions.Add( iViewGroupID , strCaption );
            return strCaption;
        }
        #endregion

        public static List<Guid> GetHomepages ( Guid iUserID )
        {
            List<Guid> lstViewIDs=new List<Guid>();
            ADUsersInfo user=new ADUsersController().GetObjectByID( iUserID ) as ADUsersInfo;
            if ( user!=null&&user.FK_ADUserGroupID.HasValue )
            {
                ADUserGroupsInfo group=new ADUserGroupsController().GetObjectByID( user.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
                if ( group!=null )
                {
                    String strQuery=String.Format( @"SELECT A.FK_STViewID FROM  GEPermissionViews A JOIN ADUserPermissions B ON A.IsHomePage ='TRUE' AND B.FK_GEPermissionID = A.FK_GEPermissionID AND  (B.FK_ADUserGroupID ='{0}' OR B.FK_ADUserID ='{1}') ORDER BY B.FK_ADUserID  DESC" , user.FK_ADUserGroupID.Value , user.ADUserID );
                    DataSet ds=new GEPermissionViewsController().GetDataSet( strQuery );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        foreach ( DataRow dr in ds.Tables[0].Rows )
                            if ( dr[0]!=DBNull.Value&&!lstViewIDs.Contains( ABCHelper.DataConverter.ConvertToGuid( dr[0].ToString() ) ) )
                                lstViewIDs.Add( ABCHelper.DataConverter.ConvertToGuid( dr[0].ToString() ) );
                }
            }

            return lstViewIDs;
        }
    }
}
