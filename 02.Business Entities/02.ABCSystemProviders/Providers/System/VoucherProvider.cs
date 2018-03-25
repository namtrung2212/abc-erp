using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using ABCProvider;
using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class VoucherProvider
    {
        #region  Factory
        public static Dictionary<String , BaseVoucher> VouchersList=new Dictionary<String , BaseVoucher>();
        public static BaseVoucher GetVoucher ( String strTableName , Guid ID )
        {
            GEVouchersInfo config=GetConfig( strTableName , ID );
            if ( config!=null )
            {
                BaseVoucher voucher=GetVoucher( config.No );
                if ( voucher!=null )
                {
                    voucher.Config=config;
                    voucher.TableName=strTableName;
                    voucher.ConfigItems=new GEVoucherItemsController().GetListByForeignKey( "FK_GEVoucherID" , config.GetID() ).Cast<GEVoucherItemsInfo>().ToList();
                    return voucher;
                }
            }
            return null;
        }
        public static BaseVoucher GetVoucher ( String strVoucherName )
        {
            if ( VouchersList.Count<=0 )
            {
                AppDomain domain=AppDomain.CreateDomain( "ABCBusinessObject" );
                GetAllVoucherObjects( domain , Application.StartupPath+"\\ABCAppModules.dll" );
                GetAllVoucherObjects( domain , Application.StartupPath+"\\ABCAppProvider.dll" );
                AppDomain.Unload( domain );
            }

            BaseVoucher voucher=null;
            VouchersList.TryGetValue( strVoucherName+"Voucher" , out voucher );
            if ( voucher!=null )
                return voucher;

            return null;
        }

     
        public static void GetAllVoucherObjects ( AppDomain domain , String strAssFileName )
        {

            try
            {
                Assembly assEntities=domain.Load( AssemblyName.GetAssemblyName( strAssFileName ) );
                if ( assEntities==null )
                    return;

                foreach ( Type type in assEntities.GetTypes() )
                {
                    if ( typeof( BaseVoucher ).IsAssignableFrom( type ) )
                    {
                        BaseVoucher Ctrl=(BaseVoucher)ABCDynamicInvoker.CreateInstanceObject( type );
                        if ( Ctrl!=null )
                            VouchersList.Add( type.Name , Ctrl );
                    }
                }
            }
            catch ( Exception ex )
            {
            }

        }
        #endregion

        #region Config

        public static List<GEVouchersInfo> GetConfigs ( String strItemTableName )
        {
            String strQuery=String.Format( "SELECT * FROM GEVouchers WHERE GEVoucherID IN ( SELECT FK_GEVoucherID FROM GEVoucherItems WHERE ItemTableName='{0}')" , strItemTableName );
            return new GEVouchersController().GetList( strQuery ).Cast<GEVouchersInfo>().ToList();
        }
        public static List<GEVoucherItemsInfo> GetConfigItems ( String strTableName , String strItemTableName )
        {
            if ( !String.IsNullOrWhiteSpace( strTableName )&&String.IsNullOrWhiteSpace( strItemTableName ) )
            {
                String strQuery=String.Format( "SELECT * FROM GEVoucherItems WHERE FK_GEVoucherID IN ( SELECT GEVoucherID FROM GEVouchers WHERE TableName='{0}')" , strTableName );
                return new GEVoucherItemsController().GetList( strQuery ).Cast<GEVoucherItemsInfo>().ToList();
            }
            else if ( String.IsNullOrWhiteSpace( strTableName )&&!String.IsNullOrWhiteSpace( strItemTableName ) )
            {
                return new GEVoucherItemsController().GetListByColumn( "ItemTableName" , strItemTableName ).Cast<GEVoucherItemsInfo>().ToList();
            }
            else
            {
                String strQuery=String.Format( "SELECT * FROM GEVoucherItems WHERE FK_GEVoucherID IN ( SELECT GEVoucherID FROM GEVouchers WHERE TableName='{0}') AND ItemTableName='{1}'" , strTableName , strItemTableName );
                return new GEVoucherItemsController().GetList( strQuery ).Cast<GEVoucherItemsInfo>().ToList();
            }
        }
        public static GEVouchersInfo GetConfig ( String strTableName , String strScreenNo )
        {
            return new GEVouchersController().GetObject( String.Format( "SELECT * FROM GEVouchers WHERE TableName ='{0}' AND ViewNo ='{1}' " , strTableName , strScreenNo ) ) as GEVouchersInfo;
        }
        public static GEVouchersInfo GetConfig ( String strTableName )
        {
            return new GEVouchersController().GetObjectByColumn( "TableName" , strTableName ) as GEVouchersInfo;
        }
        public static GEVouchersInfo GetConfig ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return GetConfig( strTableName );

            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strTableName , ID );
            if ( obj!=null )
                return GetConfig( obj );

            return null;
        }
        public static GEVouchersInfo GetConfig ( BusinessObject obj )
        {
            if ( obj==null )
                return null;

            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName );
            if ( string.IsNullOrWhiteSpace( strIDCol ) )
                return null;

            List<BusinessObject> lstConfigs=new GEVouchersController().GetListByColumn( "TableName" , obj.AATableName );
            foreach ( GEVouchersInfo config in lstConfigs )
            {
                String strQuery=QueryGenerator.GenSelect( obj.AATableName , strIDCol , false );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                strQuery=QueryGenerator.AddEqualCondition( strQuery , strIDCol , obj.GetID() );

                object objID=BusinessObjectController.GetData( strQuery );
                if ( objID!=null&&ABCHelper.DataConverter.ConvertToGuid( objID )==obj.GetID() )
                    return config;
            }

            return null;
        }

        public static List<BusinessObject> GetVouchersByItemID ( String strItemTableName , Guid itemID )
        {
            BusinessObject itemObj=BusinessObjectHelper.GetBusinessObject( strItemTableName , itemID );
            if ( itemObj==null )
                return null;

            Dictionary<Guid , BusinessObject> lstVouchers=new Dictionary<Guid , BusinessObject>();

            List<BusinessObject> lstTypeConfigs=new GEVoucherItemsController().GetListByColumn( "ItemTableName" , strItemTableName );
            foreach ( GEVoucherItemsInfo typeconfig in lstTypeConfigs )
            {
                if ( typeconfig.FK_GEVoucherID.HasValue )
                {
                    GEVouchersInfo type=new GEVouchersController().GetObjectByID( typeconfig.FK_GEVoucherID.Value ) as GEVouchersInfo;
                    if ( type!=null )
                    {
                        BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( type.TableName );
                        if ( ctrl==null )
                            continue;

                        BusinessObject obj=ctrl.GetObjectByID( ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( itemObj , typeconfig.ItemFKField ) ) );
                        if ( obj!=null&&!lstVouchers.ContainsKey( obj.GetID() ) )
                            lstVouchers.Add( obj.GetID() , obj );
                    }
                }
            }

            return lstVouchers.Values.ToList<BusinessObject>();

        }

        public static String GetViewNo ( BusinessObject obj )
        {
            if ( obj==null )
                return null;

            GEVouchersInfo config=GetConfig( obj );
            if ( config!=null )
                return config.ViewNo;

            return null;
        }
        public static String GetViewNo ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
                return GetViewNo( ctrl.GetObjectByID( ID ) );

            return null;
        }
        public static List<BusinessObject> GetVouchersByViewNo ( String strTableName , String strcScreenNo )
        {
            GEVouchersInfo config=GetConfig( strTableName , strcScreenNo );
            if ( config==null )
                return null;

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( config.TableName );
            if ( ctrl==null )
                return null;
            return ctrl.GetListByCondition( config.ConditionString );
        }
        #endregion

        public static void ReCalculateVoucher ( BusinessObject obj )
        {
            if ( obj==null )
                return;
            ReCalculateVoucher( obj.AATableName , obj.GetID() );
        }
        public static void ReCalculateVoucher ( String strTableName , Guid ID )
        {
            CheckApprovalStatus( strTableName , ID );

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    foreach ( GEVoucherItemsInfo voucherItem in GetConfigItems( strTableName , "" ) )
                        BusinessObjectHelper.CopyFKFields( obj , voucherItem.ItemTableName );

                    BaseVoucher voucher=GetVoucher( strTableName , ID );
                    if ( voucher!=null )
                        voucher.ReCalculate( obj , true );
                }
            }

            ReCalculateRelation( strTableName , ID );
        }
   
        #region Actions

        #region Permission

        public static bool CanView ( String strTableName )
        {
            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ABCCommon.VoucherPermission.AllowView );
        }
        public static bool CanView ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanView( obj.AATableName , obj.GetID() );
        }
        public static bool CanView ( String strTableName , Guid ID )
        {
            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowView );
        }
        public static bool CanNew ( String strTableName )
        {
            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ABCCommon.VoucherPermission.AllowNew );
        }
        public static bool CanEdit ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanEdit( obj.AATableName , obj.GetID() );
        }
        public static bool CanEdit ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( BusinessObjectHelper.IsPostedObject( strTableName , ID )||BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsApprovedObject( strTableName , ID ) )
                return false;

            if ( IsApprovedByCurrentUserGroup( strTableName , ID ) )
                return false;

            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowEdit );
        }
        public static bool CanDelete ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanDelete( obj.AATableName , obj.GetID() );
        }
        public static bool CanDelete ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( BusinessObjectHelper.IsPostedObject( strTableName , ID )||BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsApprovedObject( strTableName , ID ) )
                return false;

            if ( IsApprovedByCurrentUserGroup( strTableName , ID ) )
                return false;

            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowDelete );
        }
        public static bool CanLock ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanLock( obj.AATableName , obj.GetID() );
        }
        public static bool CanLock ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colLockStatus ) )
                return false;

            if ( !BusinessObjectHelper.IsLockedObject( strTableName , ID ) )
                return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowLock );
            return false;
        }
        public static bool CanUnLock ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanUnLock( obj.AATableName , obj.GetID() );
        }
        public static bool CanUnLock ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colLockStatus ) )
                return false;

            if ( BusinessObjectHelper.IsLockedObject( strTableName , ID ) )
                return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowLock );
            return false;
        }
        public static bool CanApprove ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanApprove( obj.AATableName , obj.GetID() );
        }
        public static bool CanApprove ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                return false;

            if ( BusinessObjectHelper.IsApprovedObject( strTableName , ID )||BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsPostedObject( strTableName , ID ) )
                return false;

            if ( ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowApproval ) )
            {
                if ( IsApprovedByCurrentUserGroup( strTableName , ID ) )
                    return false;

                if ( IsRejectedByCurrentUserGroup( strTableName , ID ) )
                    return true;

                return (GetCurrentApproveLevel( strTableName , ID )+1)==GetApproveLevel( strTableName , ID , ABCUserProvider.CurrentUser.FK_ADUserGroupID.Value );
            }
            return false;
        }
        public static bool CanReject ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanReject( obj.AATableName , obj.GetID() );
        }
        public static bool CanReject ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                return false;

            if ( BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsPostedObject( strTableName , ID ) )
                return false;

            if ( ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowApproval ) )
            {
                if ( IsRejectedByCurrentUserGroup( strTableName , ID ) )
                    return false;

                if ( IsApprovedByCurrentUserGroup( strTableName , ID ) )
                    return true;

                return GetCurrentApproveLevel( strTableName , ID )>=GetApproveLevel( strTableName , ID , ABCUserProvider.CurrentUser.FK_ADUserGroupID.Value );
            }

            return false;
        }
        public static bool CanPost ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanPost( obj.AATableName , obj.GetID() );
        }
        public static bool CanPost ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colJournalStatus ) )
                return false;

            if ( BusinessObjectHelper.IsPostedObject( strTableName , ID )||BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsApprovedObject( strTableName , ID )==false )
                return false;

            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowPost );
        }
        public static bool CanUnPost ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanUnPost( obj.AATableName , obj.GetID() );
        }
        public static bool CanUnPost ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( !DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colJournalStatus ) )
                return false;

            if ( !BusinessObjectHelper.IsPostedObject( strTableName , ID )||BusinessObjectHelper.IsLockedObject( strTableName , ID )||BusinessObjectHelper.IsApprovedObject( strTableName , ID )==false )
                return false;

            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowPost );
        }
        public static bool CanPrint ( BusinessObject obj )
        {
            if ( obj==null )
                return false;
            return CanPrint( obj.AATableName , obj.GetID() );
        }
        public static bool CanPrint ( String strTableName , Guid ID )
        {
            if ( ID==Guid.Empty )
                return false;

            if ( BusinessObjectHelper.IsLockedObject( strTableName , ID ) )
                return false;

            return ABCUserProvider.CheckVoucherPermission( ABCUserProvider.CurrentUser.ADUserID , strTableName , ID , ABCCommon.VoucherPermission.AllowPrint );
        }


        #endregion

        public static void BeforeSaveVoucher ( String strTableName , Guid ID )
        {
            BaseVoucher voucher=GetVoucher( strTableName , ID );
            if ( voucher!=null )
                voucher.BeforeSave( ID );
        }
        public static void AfterSavedVoucher ( String strTableName , Guid ID )
        {
            GenerateNo( strTableName , ID );

            BaseVoucher voucher=GetVoucher( strTableName , ID );
            if ( voucher!=null )
                voucher.AfterSaved( ID );
        }

        public static void BeforeDeleteVoucher ( String strTableName , Guid ID )
        {
            BaseVoucher voucher=GetVoucher( strTableName , ID );
            if ( voucher!=null )
                voucher.BeforeDelete( ID );
        }
        public static void AfterDeletedVoucher ( String strTableName , Guid ID )
        {
            BaseVoucher voucher=GetVoucher( strTableName , ID );
            if ( voucher!=null )
                voucher.AfterDeleted( ID );
        }

        public static void ApproveVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.BeforeApprove( ID );

                DoApprove( strTableName , ID );

                if ( voucher!=null )
                    voucher.AfterApproved( ID );

                scope.Complete();
            }
        }
        public static void RejectVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.BeforeReject( ID );

                DoReject( strTableName , ID );

                if ( voucher!=null )
                    voucher.AfterRejected( ID );

                scope.Complete();
            }
        }

        public static void PostVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {

                DoPost( strTableName , ID );

                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.Post( ID );

                scope.Complete();
            }
        }
        public static void UnPostVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                DoUnPost( strTableName , ID );

                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.UnPost( ID );

                scope.Complete();
            }
        }

        public static void LockVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.BeforeLock( ID );

                DoLock( strTableName , ID );

                if ( voucher!=null )
                    voucher.AfterLocked( ID );
                scope.Complete();
            }
        }
        public static void UnLockVoucher ( String strTableName , Guid ID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                BaseVoucher voucher=GetVoucher( strTableName , ID );
                if ( voucher!=null )
                    voucher.BeforeUnLock( ID );

                DoUnLock( strTableName , ID );

                if ( voucher!=null )
                    voucher.AfterUnLocked( ID );

                scope.Complete();
            }
        }

        public static void PrintVoucher ( String strTableName , Guid ID )
        {

        }


        #region Approve

        public static void DoApprove ( String strTableName , Guid ID )
        {
            #region DoApprove
            GEVouchersInfo voucher=GetConfig( strTableName , ID );

            ADApprovalActionsInfo action=new ADApprovalActionsInfo();
            action.ActionDate=DateTime.Now;
            action.ActionType=ABCCommon.ABCConstString.ApprovalTypeApproved;
            action.FK_ADUserID=ABCUserProvider.CurrentUser.ADUserID;
            action.FK_ADUserGroupID=ABCUserProvider.CurrentUser.FK_ADUserGroupID;
            action.TableName=strTableName;
            action.ID=ID;
            if ( voucher!=null )
                action.FK_GEVoucherID=voucher.GEVoucherID;

            new ADApprovalActionsController().CreateObject( action );

            CheckApprovalStatus( strTableName , ID );
            #endregion

            ReCalculateVoucher( strTableName , ID );

            NotifyToRelationMembers( strTableName , ID , " đã phê duyệt." );

            if ( IsApproved( strTableName , ID ) )
            {
                InventoryProvider.DoInventoryAction( strTableName , ID , false );
                CreditProvider.CalculateCredit( strTableName , ID );
            }
        }
        public static void DoReject ( String strTableName , Guid ID )
        {
            bool isApproved=IsApproved( strTableName , ID );

            #region DoReject
            GEVouchersInfo voucher=GetConfig( strTableName , ID );

            ADApprovalActionsInfo action=new ADApprovalActionsInfo();
            action.ActionDate=DateTime.Now;
            action.ActionType=ABCCommon.ABCConstString.ApprovalTypeRejected;
            action.FK_ADUserID=ABCUserProvider.CurrentUser.ADUserID;
            action.FK_ADUserGroupID=ABCUserProvider.CurrentUser.FK_ADUserGroupID;
            action.TableName=strTableName;
            action.ID=ID;
            if ( voucher!=null )
                action.FK_GEVoucherID=voucher.GEVoucherID;

            new ADApprovalActionsController().CreateObject( action );

            CheckApprovalStatus( strTableName , ID );
            #endregion

            ReCalculateVoucher( strTableName , ID );

            NotifyToRelationMembers( strTableName , ID , " đã bác bỏ." );

            if ( isApproved&&!IsApproved( strTableName , ID ) )
            {
                InventoryProvider.DoInventoryAction( strTableName , ID , true );
                CreditProvider.CalculateCredit( strTableName , ID );
            }
        }
        public static void CheckApprovalStatus ( BusinessObject obj )
        {
            if ( obj==null||obj.GetID()==Guid.Empty )
                return;

            CheckApprovalStatus( obj.AATableName , obj.GetID() );
        }
        public static void CheckApprovalStatus ( String strTableName , Guid ID )
        {
            if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colApprovalStatus ) )
            {
                BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
                if ( ctrl!=null )
                {
                    BusinessObject obj=ctrl.GetObjectByID( ID );
                    if ( obj!=null )
                    {
                        String strCurrentStatus=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colApprovalStatus ).ToString();
                        Boolean isApproved=IsApproved( strTableName , ID );

                        if ( ( isApproved&&( strCurrentStatus!=ABCCommon.ABCConstString.ApprovalTypeApproved ) )
                        ||( !isApproved&&( strCurrentStatus==ABCCommon.ABCConstString.ApprovalTypeApproved ) ) )
                        {

                            if ( isApproved )
                            {
                                #region Approved
                                DateTime? approvalDate=GetApprovalDate( strTableName , ID );
                                strCurrentStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;

                                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovalStatus , strCurrentStatus );
                                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovedDate , approvalDate );
                                ctrl.UpdateObject( obj );

                                #region Items
                                foreach ( GEVoucherItemsInfo config in VoucherProvider.GetConfigItems( strTableName , "" ) )
                                {
                                    if ( !DataStructureProvider.IsTableColumn( config.ItemTableName , config.ItemFKField ) )
                                        continue;

                                    if ( DataStructureProvider.IsTableColumn( config.ItemTableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                                    {
                                        String strQuery=String.Format( "UPDATE {0} SET {1}='{2}'" , config.ItemTableName , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeApproved );
                                        strQuery=QueryGenerator.FilterWithAliveRecords( config.ItemTableName , strQuery );
                                        strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemFKField , ID );
                                        BusinessObjectController.RunQuery( strQuery );
                                    }

                                    if ( DataStructureProvider.IsTableColumn( config.ItemTableName , ABCCommon.ABCConstString.colApprovedDate ) )
                                    {
                                        String strQuery=String.Empty;
                                        if ( approvalDate.HasValue )
                                            strQuery=String.Format( "UPDATE {0} SET {1}='{2}'" , config.ItemTableName , ABCCommon.ABCConstString.colApprovedDate , approvalDate.Value.ToString( "yyyy-MM-dd HH:mm:ss" ) );
                                        else
                                            strQuery=String.Format( "UPDATE {0} SET {1}=NULL" , config.ItemTableName , ABCCommon.ABCConstString.colApprovedDate );
                                        strQuery=QueryGenerator.FilterWithAliveRecords( config.ItemTableName , strQuery );
                                        strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemFKField , ID );
                                        BusinessObjectController.RunQuery( strQuery );
                                    }
                                }
                                #endregion

                                #endregion
                            }
                            else
                            {
                                #region Not Approved
                                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovedDate , null );
                                if ( IsRejected( strTableName , ID ) )
                                {
                                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeRejected );
                                    strCurrentStatus=ABCCommon.ABCConstString.ApprovalTypeRejected;
                                }
                                else
                                {
                                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
                                    strCurrentStatus=ABCCommon.ABCConstString.ApprovalTypeNew;
                                }
                                ctrl.UpdateObject( obj );

                                #region Items
                                foreach ( GEVoucherItemsInfo config in VoucherProvider.GetConfigItems( strTableName , "" ) )
                                {
                                    if ( !DataStructureProvider.IsTableColumn( config.ItemTableName , config.ItemFKField ) )
                                        continue;

                                    if ( DataStructureProvider.IsTableColumn( config.ItemTableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                                    {
                                        String strQuery=String.Format( "UPDATE {0} SET {1}='{2}'" , config.ItemTableName , ABCCommon.ABCConstString.colApprovalStatus , strCurrentStatus );
                                        strQuery=QueryGenerator.FilterWithAliveRecords( config.ItemTableName , strQuery );
                                        strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemFKField , ID );
                                        BusinessObjectController.RunQuery( strQuery );
                                    }

                                    if ( DataStructureProvider.IsTableColumn( config.ItemTableName , ABCCommon.ABCConstString.colApprovedDate ) )
                                    {
                                        String strQuery=String.Format( "UPDATE {0} SET {1}= NULL" , config.ItemTableName , ABCCommon.ABCConstString.colApprovedDate );
                                        strQuery=QueryGenerator.FilterWithAliveRecords( config.ItemTableName , strQuery );
                                        strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemFKField , ID );
                                        BusinessObjectController.RunQuery( strQuery );
                                    }
                                }
                                #endregion

                                #endregion
                            }
                        }
                    }
                }
            }
        }

        public static bool IsApprovedByCurrentUserGroup ( BusinessObject obj )
        {
            return IsApprovedByCurrentUserGroup( obj.AATableName , obj.GetID() );
        }
        public static bool IsApprovedByCurrentUserGroup ( String strTableName , Guid ID )
        {
            String strQuery=String.Format( "SELECT TOP 1 ActionType FROM ADApprovalActions WHERE TableName='{0}' AND ID='{1}' AND FK_ADUserGroupID='{2}' ORDER  BY ActionDate DESC" , strTableName , ID , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
            object obj=BusinessObjectController.GetData( strQuery );
            return obj!=null&&obj.ToString().Equals( ABCCommon.ABCConstString.ApprovalTypeApproved );
        }
        public static bool IsRejectedByCurrentUserGroup ( BusinessObject obj )
        {
            return IsRejectedByCurrentUserGroup( obj.AATableName , obj.GetID() );
        }
        public static bool IsRejectedByCurrentUserGroup ( String strTableName , Guid ID )
        {
            String strQuery=String.Format( "SELECT TOP 1 ActionType FROM ADApprovalActions WHERE TableName='{0}' AND ID='{1}' AND FK_ADUserGroupID='{2}' ORDER BY ActionDate DESC" , strTableName , ID , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
            object obj=BusinessObjectController.GetData( strQuery );
            return obj!=null&&obj.ToString().Equals( ABCCommon.ABCConstString.ApprovalTypeRejected );

        }

        public static bool IsRejected ( BusinessObject obj )
        {
            return IsRejected( obj.AATableName , obj.GetID() );
        }
        public static bool IsRejected ( String strTableName , Guid ID )
        {
            String strQuery=String.Format( "SELECT TOP 1 ActionType FROM ADApprovalActions WHERE TableName='{0}' AND ID='{1}' ORDER BY ActionDate DESC" , strTableName , ID );
            object obj=BusinessObjectController.GetData( strQuery );
            return obj!=null&&obj.ToString().Equals( ABCCommon.ABCConstString.ApprovalTypeRejected );

        }

        public static bool IsApproved ( BusinessObject obj )
        {
            return IsApproved( obj.AATableName , obj.GetID() );
        }
        public static bool IsApproved ( String strTableName , Guid ID )
        {
            int max=GetMaxApproveLevel( strTableName , ID );
            if ( max<=0 )
                max=1;

            return GetCurrentApproveLevel( strTableName , ID )>=max;
        }

        public static int GetCurrentApproveLevel ( String strTableName , Guid ID )
        {
            String strQuery=String.Format( @"SELECT COUNT(*) FROM	 ADApprovalActions as A, 
				                                                                        (SELECT FK_ADUserGroupID,MAX(ActionDate) as MaxDate
				                                                                         FROM ADApprovalActions WHERE TableName='{0}' AND ID ='{1}'
				                                                                         GROUP BY FK_ADUserGroupID) as B
                                                              WHERE	A.[FK_ADUserGroupID] =B.[FK_ADUserGroupID] 
	                                                                AND A.[ActionDate]=B.MaxDate 
	                                                                AND A.[ActionType]='Approved'
                                                                    AND A.[TableName]='{0}' AND A.[ID] ='{1}'" , strTableName , ID );

            return Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
        }
        public static DateTime? GetApprovalDate ( String strTableName , Guid ID )
        {
            String strQuery=String.Format( @"SELECT A.[ActionDate] FROM	 ADApprovalActions as A, 
				                                                                        (SELECT FK_ADUserGroupID,MAX(ActionDate) as MaxDate
				                                                                         FROM ADApprovalActions WHERE TableName='{0}' AND ID ='{1}'
				                                                                         GROUP BY FK_ADUserGroupID) as B
                                                              WHERE	A.[FK_ADUserGroupID] =B.[FK_ADUserGroupID] 
	                                                                AND A.[ActionDate]=B.MaxDate 
	                                                                AND A.[ActionType]='Approved'
                                                                    AND A.[TableName]='{0}' AND A.[ID] ='{1}'" , strTableName , ID );
            object obj=BusinessObjectController.GetData( strQuery );
            if ( obj!=null&&obj!=DBNull.Value )
                return Convert.ToDateTime( obj );

            return null;
        }

        public static int GetMaxApproveLevel ( String strTableName , Guid ID )
        {
            GEVouchersInfo config=GetConfig( strTableName , ID );
            if ( config==null )
                return 1;
            String strQuery=String.Format( "SELECT COUNT( DISTINCT  ApprovalIndex) FROM GEPermissionVouchers WHERE FK_GEVoucherID='{0}' AND AllowApproval='TRUE'" , config.GEVoucherID );
            return Convert.ToInt32( BusinessObjectController.GetData( strQuery ) );
        }
        public static int GetApproveLevel ( String strTableName , Guid ID , Guid userGroupID )
        {
            GEVouchersInfo config=GetConfig( strTableName , ID );
            if ( config==null )
                return -1;
            String strQuery=String.Format( @"SELECT ApprovalIndex FROM GEPermissionVouchers A,GEPermissions B,ADUserPermissions C WHERE A.FK_GEPermissionID=B.GEPermissionID AND C.FK_GEPermissionID=B.GEPermissionID 
                                                            AND A.FK_GEPermissionID ='{0}' AND C.FK_ADUserGroupID='{1}' " , config.GEVoucherID , userGroupID );
            object obj=BusinessObjectController.GetData( strQuery );
            if ( obj==null||obj==DBNull.Value )
                return 1;
            return Convert.ToInt32( obj );
        }

        public static void NotifyToRelationMembers ( String strTableName , Guid ID , String strContent )
        {
            BusinessObject obj=BusinessControllerFactory.GetBusinessController( strTableName ).GetObjectByID( ID );
            if ( obj==null )
                return;

            #region Get Relation Members
            List<String> lstUserNos=new List<String>();

            if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colCreateUser ) )
                lstUserNos.Add( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colCreateUser ).ToString() );

            if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colUpdateUser ) )
                lstUserNos.Add( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colUpdateUser ).ToString() );

            GEVouchersInfo config=GetConfig( strTableName , ID );
            if ( config!=null )
            {

                #region From Config
                String strQuery=QueryGenerator.GenSelect( "ADUsers" , "No" , false );

                int nextLevel=GetCurrentApproveLevel( strTableName , ID )+1;
                String strCondition=String.Empty;
                if ( nextLevel<=1 )
                    strCondition=String.Format( "Active='TRUE' AND FK_ADUserGroupID IN ( SELECT  DISTINCT  C.FK_ADUserGroupID FROM GEPermissionVouchers A,GEPermissions B,ADUserPermissions C  WHERE A.FK_GEPermissionID=B.GEPermissionID AND C.FK_GEPermissionID=B.GEPermissionID  AND A.FK_GEVoucherID='{0}' AND AllowApproval='TRUE' AND (ApprovalIndex <= {1} OR ApprovalIndex IS NULL))" , config.GEVoucherID , nextLevel );
                else
                    strCondition=String.Format( "Active='TRUE' AND FK_ADUserGroupID IN ( SELECT  DISTINCT  C.FK_ADUserGroupID FROM GEPermissionVouchers A,GEPermissions B,ADUserPermissions C  WHERE A.FK_GEPermissionID=B.GEPermissionID AND C.FK_GEPermissionID=B.GEPermissionID  AND A.FK_GEVoucherID='{0}' AND AllowApproval='TRUE' AND ApprovalIndex <= {1})" , config.GEVoucherID , nextLevel );

                strQuery=QueryGenerator.AddCondition( strQuery , strCondition );
                DataSet ds=DataQueryProvider.RunQuery( strQuery );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        if ( lstUserNos.Contains( dr[0].ToString() )==false )
                            lstUserNos.Add( dr[0].ToString() );
                }
                #endregion

                strQuery=QueryGenerator.GenSelect( "ADUsers" , "No" , false );
                strCondition=String.Format( "Active='TRUE' AND FK_ADUserGroupID IN ( SELECT  DISTINCT  FK_ADUserGroupID FROM ADApprovalActions WHERE FK_GEVoucherID='{0}')" , config.GEVoucherID );
                strQuery=QueryGenerator.AddCondition( strQuery , strCondition );

                ds=DataQueryProvider.RunQuery( strQuery );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        if ( lstUserNos.Contains( dr[0].ToString() )==false )
                            lstUserNos.Add( dr[0].ToString() );
                }

            }

            #endregion

            String strDisplayCol=DataStructureProvider.GetDisplayColumn( strTableName );
            String strTitle=DataConfigProvider.GetTableCaption( strTableName );
            strTitle=strTitle+":"+ABCDynamicInvoker.GetValue( obj , strDisplayCol ).ToString();

            foreach ( String userNo in lstUserNos.Distinct().ToList() )
            {
                if ( !String.IsNullOrWhiteSpace( ABCUserProvider.CurrentEmployeeName ) )
                    NotifyProvider.CreateNewNotify( userNo , strTitle , String.Format( " {0} {1}" , ABCUserProvider.CurrentEmployeeName , strContent ) , strTableName , ID , "" );
                else
                    NotifyProvider.CreateNewNotify( userNo , strTitle , String.Format( " {0} {1}" , ABCUserProvider.CurrentUserName , strContent ) , strTableName , ID , "" );
            }

        }


        #endregion

        #region Lock
        public static void DoLock ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colLockStatus , ABCCommon.ABCConstString.LockStatusLocked );
                    ctrl.UpdateObject( obj );
                }
            }

            ReCalculateVoucher( strTableName , ID );
        }
        public static void DoUnLock ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colLockStatus , ABCCommon.ABCConstString.LockStatusUnlocked );
                    ctrl.UpdateObject( obj );
                }
            }
            ReCalculateVoucher( strTableName , ID );
        }
        #endregion

        #region Post
        public static void DoPost ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colJournalStatus , ABCCommon.ABCConstString.PostStatusPosted );
                    ctrl.UpdateObject( obj );
                }
            }
            ReCalculateVoucher( strTableName , ID );
        }
        public static void DoUnPost ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colJournalStatus , ABCCommon.ABCConstString.PostStatusUnPost );
                    ctrl.UpdateObject( obj );
                }
            }
            ReCalculateVoucher( strTableName , ID );
        }
        #endregion

        #endregion

        public static bool IsCalcOnClient ( String strTableName )
        {
            return false;
        }

        #region Relation
        public static List<GERelationConfigsInfo> GetRelationConfigs ( String strDestinyTableName )
        {
            return new GERelationConfigsController().GetListByColumn( "DestinyTableName" , strDestinyTableName ).Cast<GERelationConfigsInfo>().ToList();
        }

        public static List<BusinessObject> GetOpenningRelationObjects (Guid configID , bool isFromItem )
        {
            GERelationConfigsInfo config=new GERelationConfigsController().GetObjectByID( configID ) as GERelationConfigsInfo;
            if ( config==null )
                return new List<BusinessObject>();

            if ( isFromItem )
            {
                String strQuery=QueryGenerator.GenSelect( config.SourceItemTableName , "*" , true );
                String strTemp=String.Format( @"{0} JOIN GERelationStatuss ON ({0}.{1}=GERelationStatuss.SourceItemID AND GERelationStatuss.RemainWeight>0 AND GERelationStatuss.FK_GERelationConfigID='{2}')" , config.SourceItemTableName , DataStructureProvider.GetPrimaryKeyColumn( config.SourceItemTableName ) , configID );
                strQuery=strQuery.Replace( "["+config.SourceItemTableName+"]" , strTemp );
                strQuery=QueryGenerator.AddCondition( strQuery , config.SourceItemConditionString );
                return BusinessControllerFactory.GetBusinessController( config.SourceItemTableName ).GetListByQuery( strQuery );
            }
            else
            {
                String strQuery=QueryGenerator.GenSelect( config.SourceItemTableName , DataStructureProvider.GetForeignKeyOfTableName( config.SourceItemTableName , config.SourceTableName ) , true );
                String strTemp=String.Format( @"{0} JOIN GERelationStatuss ON ({0}.{1}=GERelationStatuss.SourceItemID AND GERelationStatuss.RemainWeight>0 AND GERelationStatuss.FK_GERelationConfigID='{2}')" , config.SourceItemTableName , DataStructureProvider.GetPrimaryKeyColumn( config.SourceItemTableName ) , configID );
                strQuery=strQuery.Replace( "["+config.SourceItemTableName+"]" , strTemp );
                strQuery=QueryGenerator.AddCondition( strQuery , config.SourceItemConditionString );

                String strQuery2=QueryGenerator.GenSelect( config.SourceTableName , "*" , true );
                strQuery2=QueryGenerator.AddCondition( strQuery2 , String.Format( "{0} IN ({1})" , DataStructureProvider.GetPrimaryKeyColumn( config.SourceTableName ) , strQuery ) );
                strQuery2=QueryGenerator.AddCondition( strQuery2 , config.SourceConditionString );
                return BusinessControllerFactory.GetBusinessController( config.SourceTableName ).GetListByQuery( strQuery2 );
            }
        }

        public static BusinessObject CreateVoucherFromRelation ( Guid configID , bool isFromItem , List<BusinessObject> lstObjects )
        {
            if ( lstObjects.Count<=0 )
                return null;

            if ( configID==Guid.Empty )
                return null;

            GERelationConfigsInfo config=new GERelationConfigsController().GetObjectByID( configID ) as GERelationConfigsInfo;
            if ( config==null )
                return null;

            BusinessObjectController sourceCtrl=BusinessControllerFactory.GetBusinessController( config.SourceTableName );
            BusinessObjectController sourceItemCtrl=BusinessControllerFactory.GetBusinessController( config.SourceItemTableName );
            BusinessObjectController destinyCtrl=BusinessControllerFactory.GetBusinessController( config.DestinyTableName );
            BusinessObjectController destinyItemCtrl=BusinessControllerFactory.GetBusinessController( config.DestinyItemTableName );
            GERelationsController relationCtrl=new GERelationsController();

            if ( sourceCtrl==null||sourceItemCtrl==null||destinyCtrl==null||destinyItemCtrl==null )
                return null;

            String strFKSourceCol=DataStructureProvider.GetForeignKeyOfTableName( config.SourceItemTableName , config.SourceTableName );
            if ( String.IsNullOrWhiteSpace( strFKSourceCol ) )
                return null;

            String strFKDestinyCol=DataStructureProvider.GetForeignKeyOfTableName( config.DestinyItemTableName , config.DestinyTableName );
            if ( String.IsNullOrWhiteSpace( strFKDestinyCol ) )
                return null;

            BusinessObject destiny=BusinessObjectFactory.GetBusinessObject( config.DestinyTableName );
            BusinessObjectHelper.SetDefaultValue( destiny );
            GenerateNo( destiny );

            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                destinyCtrl.CreateObject( destiny );

                List<BusinessObject> lstSourceItems=new List<BusinessObject>();

                if ( isFromItem )
                {
                    Dictionary<Guid , BusinessObject> lstMainObjects=new Dictionary<Guid , BusinessObject>();

                    #region
                    foreach ( BusinessObject sourceItem in lstObjects )
                    {
                        if ( sourceItem.Selected )
                        {
                            BusinessObject destinyItem=BusinessObjectFactory.GetBusinessObject( config.DestinyItemTableName );
                            BusinessObjectHelper.CopyObject( sourceItem , destinyItem , false );
                            BusinessObjectHelper.SetDefaultValue( destinyItem );
                            ABCDynamicInvoker.SetValue( destinyItem , strFKDestinyCol , destiny.GetID() );
                            String strTemp=DataStructureProvider.GetForeignKeyOfTableName( destinyItem.AATableName , sourceItem.AATableName );
                            if ( !String.IsNullOrWhiteSpace( strTemp ) )
                                ABCDynamicInvoker.SetValue( destinyItem , strTemp , sourceItem.GetID() );

                            object objQty=BusinessObjectController.GetData( String.Format( "SELECT RemainWeight FROM GERelationStatuss WHERE FK_GERelationConfigID='{0}' AND SourceItemID='{1}'" , configID , sourceItem.GetID() ) );
                            if ( Convert.ToDouble( objQty )>0 )
                            {
                                ABCDynamicInvoker.SetValue( destinyItem , config.DestinyItemWeightField , Convert.ToDouble( objQty ) );
                                destinyItemCtrl.CreateObject( destinyItem );

                                GERelationsInfo relation=new GERelationsInfo();
                                relation.FK_GERelationConfigID=configID;
                                relation.SourceID=sourceItem.GetID();
                                relation.DestinyID=destinyItem.GetID();
                                relationCtrl.CreateObject( relation );

                                lstSourceItems.Add( sourceItem );


                                Guid sourceID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( sourceItem , strFKSourceCol ) );
                                if ( sourceID!=null&&!lstMainObjects.ContainsKey( sourceID ) )
                                    lstMainObjects.Add( sourceID , sourceCtrl.GetObjectByID( sourceID ) );
                            }
                        }
                    }
                    #endregion

                    Dictionary<string , object> lstSamePros=BusinessObjectHelper.GetSameColumnValues( lstMainObjects.Values.ToList() );
                    foreach ( String strPro in lstSamePros.Keys )
                        ABCDynamicInvoker.SetValue( destiny , strPro , lstSamePros[strPro] );

                    if ( lstMainObjects.Count==1 )
                        ABCDynamicInvoker.SetValue( destiny , DataStructureProvider.GetForeignKeyOfTableName( destiny.AATableName , config.SourceTableName ) , lstMainObjects.First().Value.GetID() );

                    foreach ( BusinessObject objSource in lstMainObjects.Values )
                    {
                        ABCDynamicInvoker.SetValue( objSource , DataStructureProvider.GetForeignKeyOfTableName( config.SourceTableName , destiny.AATableName ) , destiny.GetID() );
                        sourceCtrl.UpdateObject( objSource );
                    }
                }
                else
                {
                    #region
                    foreach ( BusinessObject source in lstObjects )
                    {
                        if ( source.Selected )
                        {
                            BusinessObjectHelper.CopyObject( source , destiny , false );
                            ABCDynamicInvoker.SetValue( destiny , DataStructureProvider.GetForeignKeyOfTableName( destiny.AATableName , source.AATableName ) , source.GetID() );
                            foreach ( BusinessObject sourceItem in sourceItemCtrl.GetListByForeignKey( strFKSourceCol , source.GetID() ) )
                            {
                                BusinessObject destinyItem=BusinessObjectFactory.GetBusinessObject( config.DestinyItemTableName );
                                BusinessObjectHelper.CopyObject( sourceItem , destinyItem , false );
                                BusinessObjectHelper.SetDefaultValue( destinyItem );
                                ABCDynamicInvoker.SetValue( destinyItem , strFKDestinyCol , destiny.GetID() );
                                String strTemp=DataStructureProvider.GetForeignKeyOfTableName( destinyItem.AATableName , sourceItem.AATableName );
                                if ( !String.IsNullOrWhiteSpace( strTemp ) )
                                    ABCDynamicInvoker.SetValue( destinyItem , strTemp , sourceItem.GetID() );

                                object objQty=BusinessObjectController.GetData( String.Format( "SELECT RemainWeight FROM GERelationStatuss WHERE FK_GERelationConfigID='{0}' AND SourceItemID='{1}'" , configID , sourceItem.GetID() ) );
                                if ( Convert.ToDouble( objQty )>0 )
                                {
                                    ABCDynamicInvoker.SetValue( destinyItem , config.DestinyItemWeightField , Convert.ToDouble( objQty ) );
                                    destinyItemCtrl.CreateObject( destinyItem );

                                    GERelationsInfo relation=new GERelationsInfo();
                                    relation.FK_GERelationConfigID=configID;
                                    relation.SourceID=sourceItem.GetID();
                                    relation.DestinyID=destinyItem.GetID();
                                    relationCtrl.CreateObject( relation );

                                    lstSourceItems.Add( sourceItem );
                                }
                            }
                        }
                    }

                    Dictionary<string , object> lstSamePros=BusinessObjectHelper.GetSameColumnValues( lstObjects.Where( t => t.Selected ).ToList() );
                    foreach ( String strPro in lstSamePros.Keys )
                        ABCDynamicInvoker.SetValue( destiny , strPro , lstSamePros[strPro] );

                    if ( lstObjects.Where( t => t.Selected ).ToList().Count==1 )
                        ABCDynamicInvoker.SetValue( destiny , DataStructureProvider.GetForeignKeyOfTableName( destiny.AATableName , config.SourceTableName ) , lstObjects.Where( t => t.Selected ).ToList()[0].GetID() );

                    foreach ( BusinessObject objSource in lstObjects.Where( t => t.Selected ).ToList() )
                    {
                        ABCDynamicInvoker.SetValue( objSource , DataStructureProvider.GetForeignKeyOfTableName( config.SourceTableName , destiny.AATableName ) , destiny.GetID() );
                        sourceCtrl.UpdateObject( objSource );
                    }

                    #endregion
                }

                BusinessObjectHelper.SetDefaultValue( destiny );
                destinyCtrl.UpdateObject( destiny );

                ReCalculateVoucher( destiny );
                scope.Complete();
                return destiny;
            }

            return null;
        }

        public static void ReCalculateRelation ( Guid configID )
        {
            GERelationConfigsInfo config=new GERelationConfigsController().GetObjectByID( configID ) as GERelationConfigsInfo;
            if ( config==null )
                return;

            GERelationStatussController statusCtrl=new GERelationStatussController();
            String strQuery=QueryGenerator.GenSelect( config.SourceItemTableName , "*" , true );
            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( " {0} NOT IN (SELECT SourceItemID FROM GERelationStatuss WHERE FK_GERelationConfigID='{1}')" , DataStructureProvider.GetPrimaryKeyColumn( config.SourceItemTableName ) , configID ) );
            foreach ( BusinessObject obj in BusinessControllerFactory.GetBusinessController( config.SourceItemTableName ).GetListByQuery( strQuery ) )
            {
                GERelationStatussInfo status=new GERelationStatussInfo();
                status.FK_GERelationConfigID=configID;
                status.SourceItemID=obj.GetID();
                status.RelationCount=0;
                status.OriginalWeight=Convert.ToDouble( ABCDynamicInvoker.GetValue( obj , config.SourceItemWeightField ) );
                status.RelationWeight=0;
                status.RemainWeight=status.OriginalWeight;
                statusCtrl.CreateObject( status );
            }

            #region Clean records
            strQuery=QueryGenerator.GenSelect( config.DestinyItemTableName , DataStructureProvider.GetPrimaryKeyColumn( config.DestinyItemTableName ) , false );
            strQuery=String.Format( "DELETE FROM GERelations WHERE FK_GERelationConfigID ='{0}' AND DestinyID NOT IN ({1})" , configID , strQuery );
            BusinessObjectController.RunQuery( strQuery );

            strQuery=QueryGenerator.GenSelect( config.SourceItemTableName , DataStructureProvider.GetPrimaryKeyColumn( config.SourceItemTableName ) , false );
            strQuery=String.Format( "DELETE FROM GERelations WHERE FK_GERelationConfigID ='{0}' AND SourceID NOT IN ({1})" , configID , strQuery );
            BusinessObjectController.RunQuery( strQuery );
            #endregion

            #region Update Qty
            strQuery=QueryGenerator.GenSelect( config.DestinyItemTableName , "ISNULL(SUM("+config.DestinyItemWeightField+"),0)" , false );
            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN (SELECT DestinyID FROM GERelations WHERE GERelations.SourceID = SourceItemID AND GERelations.FK_GERelationConfigID='{1}')  " , DataStructureProvider.GetPrimaryKeyColumn( config.DestinyItemTableName ) , configID ) );
            strQuery=String.Format( "UPDATE GERelationStatuss SET RelationWeight = ({0}), UpdateTime=GETDATE() WHERE FK_GERelationConfigID ='{1}' " , strQuery , configID );
            BusinessObjectController.RunQuery( strQuery );

            strQuery=String.Format( "UPDATE GERelationStatuss SET RemainWeight=OriginalWeight-RelationWeight , UpdateTime=GETDATE() WHERE FK_GERelationConfigID ='{0}' " , configID );
            BusinessObjectController.RunQuery( strQuery ); 
            #endregion

        }
        public static void ReCalculateRelation ( BusinessObject destiny )
        {
            ReCalculateRelation( destiny.AATableName , destiny.GetID() );
        }
        public static void ReCalculateRelation ( String strTableName , Guid ID )
        {
            GERelationStatussController statusCtrl = new GERelationStatussController();

            foreach ( GERelationConfigsInfo config in GetRelationConfigs( strTableName ) )
            {
                ReCalculateRelation( config.GERelationConfigID );

                BusinessObjectController sourceCtrl = BusinessControllerFactory.GetBusinessController(config.SourceItemTableName);

                String strFKCol=DataStructureProvider.GetForeignKeyOfTableName( config.DestinyItemTableName , config.DestinyTableName );
                if ( String.IsNullOrWhiteSpace( strFKCol ) )
                    continue;

                String strQuery=QueryGenerator.GenSelect( config.DestinyItemTableName , DataStructureProvider.GetPrimaryKeyColumn( config.DestinyItemTableName ) , false );
                strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0}='{1}'" , strFKCol , ID ) );
                strQuery=String.Format( "SELECT DISTINCT SourceID FROM GERelations WHERE FK_GERelationConfigID='{0}' AND DestinyID IN ({1})" , config.GERelationConfigID , strQuery );

                DataSet ds=BusinessObjectController.RunQuery( strQuery );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                    {
                        Guid sourceID=ABCHelper.DataConverter.ConvertToGuid( dr[0] );
                        if ( sourceID!=Guid.Empty )
                        {
                            BusinessObject sourceInfo=sourceCtrl.GetObjectByID( sourceID );
                            if ( sourceInfo==null )
                                continue;

                            #region Update Relation Status
                            BusinessObject objStatus=statusCtrl.GetObjectByCondition( String.Format( "FK_GERelationConfigID='{0}' AND SourceItemID='{1}'" , config.GERelationConfigID , sourceID ) );
                            if ( objStatus==null )
                                continue;

                            GERelationStatussInfo status=objStatus as GERelationStatussInfo;
                            status.OriginalWeight=Convert.ToDouble( ABCDynamicInvoker.GetValue( sourceInfo , config.SourceItemWeightField ) );
                            status.RelationCount=0;
                            status.RelationWeight=0;

                            strQuery=String.Format( "SELECT COUNT(*) FROM GERelations WHERE FK_GERelationConfigID='{0}' AND SourceID ='{1}'" , config.GERelationConfigID , sourceID );
                            object objRelationCount=BusinessObjectController.GetData( strQuery );
                            if ( objRelationCount!=null&&objRelationCount!=DBNull.Value )
                                status.RelationCount=Convert.ToInt32( objRelationCount );

                            strQuery=QueryGenerator.GenSelect( config.DestinyItemTableName , "ISNULL(SUM("+config.DestinyItemWeightField+"),0)" , false );
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN (SELECT DestinyID FROM GERelations WHERE FK_GERelationConfigID='{1}' AND SourceID ='{2}')" , DataStructureProvider.GetPrimaryKeyColumn( config.DestinyItemTableName ) , config.GERelationConfigID , sourceID ) );
                            object objRelationWeight=BusinessObjectController.GetData( strQuery );
                            if ( objRelationWeight!=null&&objRelationWeight!=DBNull.Value )
                                status.RelationWeight=Convert.ToDouble( objRelationWeight );

                            status.RemainWeight=status.OriginalWeight-status.RelationWeight;
                            if ( status.RemainWeight<0 )
                                status.RemainWeight=0;

                            statusCtrl.UpdateObject( status );
                            #endregion
                    
                        }
                    }
                }
            }

        }
    
        #endregion

        public static String GenerateNo ( BusinessObject obj )
        {
            String strNoCol=DataStructureProvider.GetNOColumn( obj.AATableName );
            if ( String.IsNullOrWhiteSpace( strNoCol )==false )
            {
                String strNo=NumberingProvider.GenerateNo( obj );
                if ( String.IsNullOrWhiteSpace( strNo ) )
                {
                    ABCDynamicInvoker.SetValue( obj , strNoCol , strNo );
                    BusinessControllerFactory.GetBusinessController( obj.AATableName ).UpdateObject( obj );
                    return strNo;
                }
            }

            return String.Empty;
        }
        public static String GenerateNo ( String strTableName , Guid ID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                    return GenerateNo( obj );
            }
            return String.Empty;
        }
    }
}
