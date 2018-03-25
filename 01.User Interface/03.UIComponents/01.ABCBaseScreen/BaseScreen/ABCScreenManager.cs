using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using ABCControls;

using ABCCommon;
using ABCBusinessEntities;
using ABCProvider;
namespace ABCScreen
{
    public class ABCScreenManager : System.MarshalByRefObject , IABCScreenManager
    {
        public static IABCScreenManager Instance
        {
            get { return ABCScreenHelper.Instance; }
        }

        public void CallInitialize ( )
        {
            Initialize();
        }
        public static void Initialize ( )
        {
            if ( ABCScreenHelper.Instance==null )
                ABCScreenHelper.Instance=new ABCScreenManager();

            SkinRegister();

            ABCScreen.ABCScreenFactory.LoadAllScreenTypes();

        }

        public void RunLink ( String strTableName , ViewMode mode , bool isShowDialog , Guid iMainID , ABCScreenAction action )
        {
            STViewsInfo viewResult=null;
            STViewsController viewCtrl=new STViewsController();

            String strViewNo=VoucherProvider.GetViewNo( strTableName , iMainID );
            if ( !String.IsNullOrWhiteSpace( strViewNo ) )
                viewResult=viewCtrl.GetObjectByNo( strViewNo ) as STViewsInfo;
            if ( viewResult==null )
            {
                #region Without Voucher
                BusinessObjectController controller=BusinessControllerFactory.GetBusinessController( strTableName );
                if ( controller==null )
                    return;

                BusinessObject obj=controller.GetObjectByID( iMainID );
                if ( obj==null )
                    return;


                List<BusinessObject> lstViews=viewCtrl.GetListFromDataset( viewCtrl.GetDataSet( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , strTableName ) ) );
                foreach ( STViewsInfo viewIfo in lstViews )
                {
                    if ( String.IsNullOrWhiteSpace( viewIfo.MainFieldName )==false&&DataStructureProvider.IsTableColumn( strTableName , viewIfo.MainFieldName ) )
                    {
                        object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , viewIfo.MainFieldName );
                        if ( objValue!=null&&objValue.ToString().ToUpper().Trim()==viewIfo.MainValue.ToUpper() )
                        {
                            viewResult=viewIfo;
                            break;
                        }
                    }
                }

                if ( viewResult==null&&lstViews.Count>0 )
                    viewResult=lstViews[0] as STViewsInfo;

                #endregion
            }

            if ( viewResult!=null )
                ABCScreenManager.Instance.RunLink( viewResult , mode , isShowDialog , iMainID , action );

        }
        public void RunLink ( STViewsInfo viewIfo , ViewMode mode , bool isShowDialog , Guid iMainID , ABCScreenAction action )
        {
            if ( viewIfo!=null )
            {
                ABCHelper.ABCWaitingDialog.Show( "" , "Đang mở  . . .!" );

                ABCScreen.ABCBaseScreen scr=ABCScreenFactory.GetABCScreen( viewIfo , mode );
                if ( scr!=null )
                {
                    scr.InvalidateData( iMainID );

                    ABCHelper.ABCWaitingDialog.Close();

                    if ( action!=ABCScreenAction.None )
                        scr.DoAction( action , true );
                    if ( action==ABCScreenAction.Disable )
                        scr.UIManager.View.ShowToolbar=false;

                    if ( isShowDialog )
                        scr.ShowDialog();
                    else
                        scr.Show();
                }

                ABCHelper.ABCWaitingDialog.Close();

            }
        }
        public void RunLink ( String strScreenName , ViewMode mode , bool isShowDialog , ABCScreenAction action , params object[] objParams )
        {
            ABCHelper.ABCWaitingDialog.Show( "" , "Đang mở . . .!" );

            ABCScreen.ABCBaseScreen scr=ABCScreenFactory.GetABCScreen( strScreenName , mode );
            if ( scr!=null )
                scr.InvalidateData( objParams );

            ABCHelper.ABCWaitingDialog.Close();

            if ( scr!=null )
            {
                if ( action!=ABCScreenAction.None )
                    scr.DoAction( action , true );
                if ( action==ABCScreenAction.Disable )
                    scr.UIManager.View.ShowToolbar=false;

                if ( isShowDialog )
                    scr.ShowDialog();
                else
                    scr.Show();
            }


        }

        public void ShowForm ( Form form , bool isShowDialog )
        {
            try
            {
                if ( ABCStudio.ABCStudioHelper.Instance!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
                    if ( isShowDialog )
                        form.ShowDialog();
                    else
                        form.Show();
                else
                {
                    if ( isShowDialog )
                    {
                        form.ShowDialog( ABCApp.ABCAppHelper.Instance.MainForm );
                    }
                    else
                    {
                        if ( form.Visible==false )
                        {
                            SetParent( form.Handle , ABCApp.ABCAppHelper.Instance.MainForm.Handle );
                            form.WindowState=FormWindowState.Normal;
                            form.Show( ABCApp.ABCAppHelper.Instance.MainForm );
                            //           SetParent( form.Handle , ABCApp.ABCAppProvider.AppInstance.MainForm.Handle );
                        }
                        else
                        {
                            form.WindowState=FormWindowState.Normal;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( ex.Message , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
            }
            GC.Collect();
        }

        [DllImport( "User32" , CharSet=CharSet.Auto , ExactSpelling=true )]
        internal static extern IntPtr SetParent ( IntPtr hWndChild , IntPtr hWndParent );


        public void OpenScreenForNew ( String strTableName , ViewMode mode , bool isShowDialog )
        {

            #region Get View
            STViewsInfo viewResult=null;
            STViewsController viewCtrl=new STViewsController();
            List<BusinessObject> lstViews=viewCtrl.GetListFromDataset( viewCtrl.GetDataSet( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , strTableName ) ) );
            foreach ( STViewsInfo viewIfo in lstViews )
            {
                if ( String.IsNullOrWhiteSpace( viewIfo.MainFieldName )==false&&DataStructureProvider.IsTableColumn( strTableName , viewIfo.MainFieldName ) )
                {
                    viewResult=viewIfo;
                    break;
                }
            }
            if ( viewResult==null&&lstViews.Count>0 )
                viewResult=lstViews[0] as STViewsInfo;

            #endregion

            if ( viewResult!=null )
            {
                ABCHelper.ABCWaitingDialog.Show( "" , "Đang mở . . .!" );

                ABCScreen.ABCBaseScreen scr=ABCScreenFactory.GetABCScreen( viewResult , mode );
                scr.DoAction( ABCScreenAction.New , true );

                ABCHelper.ABCWaitingDialog.Close();

                if ( isShowDialog )
                    scr.ShowDialog();
                else
                    scr.Show();
            }
        }

        public static void SkinRegister ( )
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.Default.RegisterAssembly( typeof( DevExpress.UserSkins.ABCSkinBLue ).Assembly ); //Register!

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName="ABCSkinBLue";
        }

        public static Form GetMainScreen ( )
        {
            STViewsInfo info=(STViewsInfo)new STViewsController().GetObjectByNo( "View0" );
            if ( info==null )
                return null;

            ABCScreen.ABCBaseScreen scr=ABCScreen.ABCScreenFactory.GetABCScreen( info , ViewMode.Runtime );
            if ( scr!=null )
                return scr.GetDialog();

            return null;
        }

        #region Permissions
        public Guid GetCurrentUserID ( )
        {
            if ( ABCUserProvider.CurrentUser==null )
                return Guid.Empty;

            return ABCUserProvider.CurrentUser.ADUserID;
        }

        public bool CheckViewPermission ( Guid viewID , ViewPermission permission )
        {
            Guid iUserID=GetCurrentUserID();
            if ( iUserID==Guid.Empty )
                return false;

            return ABCUserProvider.CheckViewPermission( iUserID , viewID , permission );
        }
        public bool CheckTablePermission ( String strTableName , TablePermission permission )
        {
            Guid? iUserID=GetCurrentUserID();
            if ( iUserID.HasValue==false )
                return false;

            return ABCUserProvider.CheckTablePermission( iUserID.Value , strTableName , permission );
        }
        public bool CheckFieldPermission ( String strTableName , String strFieldName , FieldPermission permission )
        {
            Guid? iUserID=GetCurrentUserID();
            if ( iUserID.HasValue==false )
                return false;

            return ABCUserProvider.CheckFieldPermission( iUserID.Value , strTableName , strFieldName , permission );
        }
        public bool CheckVoucherPermission ( String strTableName , Guid ID , VoucherPermission permission )
        {
            Guid? iUserID=GetCurrentUserID();
            if ( iUserID.HasValue==false )
                return false;

            return ABCUserProvider.CheckVoucherPermission( iUserID.Value , strTableName , ID , permission );
        }
        public bool CheckVoucherPermission ( BusinessObject obj , VoucherPermission permission )
        {
            return CheckVoucherPermission( obj.AATableName , obj.GetID() , permission );
        }

        #endregion
    }
}
