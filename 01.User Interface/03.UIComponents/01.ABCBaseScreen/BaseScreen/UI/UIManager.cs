using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;

using ABCProvider;
using ABCCommon;
using ABCControls;
using ABCBusinessEntities;
namespace ABCScreen.UI
{
    public class UIManager
    {
        public ABCBaseScreen Screen;
        public ABCView View;

        public UIManager ( ABCBaseScreen scr )
        {
            Screen=scr;
        }

        #region Utils
        public Control this[String strControlName]
        {
            get { return GetControl( strControlName ); }
        }
        public Control GetControl ( String strControlName )
        {
            Control[] controls=View.Controls.Find( strControlName , true );
            if ( controls.Length>0 )
                return controls[0];
            return null;
        }
        public Control GetControl ( String strDataObjectName , String strFieldName )
        {
            return null;
        }
        #endregion

        #region LoadView
        public void LoadView ( STViewsInfo viewInfo , ViewMode mode )
        {
            if ( View==null )
            {
                View=new ABCView();
                View.Load( viewInfo , mode );
            }
            else
            {
                Control ctrlParent=View.Parent;
                View.Parent=null;
                View=new ABCView();
                View.Load( viewInfo , mode );

                View.Parent=ctrlParent;
                View.Dock=DockStyle.Fill;
                ctrlParent.ClientSize=View.Size;
            }
            InitializeEvents();
        }
        public void LoadView ( XmlDocument doc , ViewMode mode )
        {
            View=new ABCView();
            View.Load( doc , mode );
            InitializeEvents();
        }
        public void LoadView ( string strFileName , ViewMode mode )
        {
            XmlDocument doc=new XmlDocument();
            doc.Load( strFileName );
            LoadView( doc , mode );
        }
        #endregion

        #region Toolbar

        public class ToolbarStatus
        {
            public bool ShowNewItem;
            public bool ShowEditItem;
            public bool ShowDuplicateItem;
            public bool ShowDeleteItem;
            public bool ShowSaveItem;
            public bool ShowCancelItem;
         
        }
        public ToolbarStatus BackupToolbarStatus=new ToolbarStatus();
        public void InitializeToolbar ( )
        {

            if ( this.Screen.DataManager.MainObject==null )
            {
                this.View.ShowRefreshItem=false;
                this.View.ShowCancelItem=false;
                this.View.ShowSaveItem=false;
                this.View.ShowEditItem=false;
                this.View.ShowDuplicateItem=false;
                this.View.ShowNewItem=false;
                this.View.ShowDeleteItem=false;
                this.View.ShowSearchItem=false;
                this.View.ShowPostItem=false;
                this.View.ShowApproveItem=false;
                this.View.ShowLockItem=false;
                this.View.ShowPrintItem=false;
                this.View.ShowInfoItem=false;
            }
            else
            {
                this.View.ShowSaveItem=false;
                this.View.ShowCancelItem=false;
                this.View.ShowInfoItem=true;
            }

            BackupToolbarStatus.ShowNewItem=this.View.ShowNewItem;
            BackupToolbarStatus.ShowEditItem=this.View.ShowEditItem;
            BackupToolbarStatus.ShowDuplicateItem=this.View.ShowDuplicateItem;
            BackupToolbarStatus.ShowDeleteItem=this.View.ShowDeleteItem;
            BackupToolbarStatus.ShowSaveItem=this.View.ShowSaveItem;
            BackupToolbarStatus.ShowCancelItem=this.View.ShowCancelItem;

            SetToolBarButtonVisibility( ABCView.ScreenBarButton.New , this.View.ShowNewItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Duplicate , this.View.ShowDuplicateItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Edit , this.View.ShowEditItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Delete , this.View.ShowDeleteItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Approve , this.View.ShowApproveItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Reject , false );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Lock , this.View.ShowLockItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.UnLock , false );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Post , this.View.ShowPostItem );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.UnPost ,false );
            SetToolBarButtonVisibility( ABCView.ScreenBarButton.Print , this.View.ShowPrintItem );

            if ( this.Screen.DataManager.MainObject!=null )
            {
                foreach ( GERelationConfigsInfo config in VoucherProvider.GetRelationConfigs( this.Screen.DataManager.MainObject.TableName ) )
                {
                    AddNewToolbarButton( "UtilitiesItem" , String.Format( "Tạo từ {0}" , DataConfigProvider.GetTableCaption( config.SourceTableName ) ) , config.GetID() , "NewFromRelation-"+config.SourceTableName , 46 );
                    SetToolBarButtonVisibility( ABCView.ScreenBarButton.Utilities , true );
                }
            }

            this.View.ToolbarClickEvent+=new ABCView.ABCToobarClickEventHandler( View_ToolbarClickEvent );
        }

        void View_ToolbarClickEvent ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {

            if ( e.Item.Tag!=null)
            {
                if ( e.Item.Tag is ABCView.ScreenBarButton )
                {
                    ABCView.ScreenBarButton button=(ABCView.ScreenBarButton)Enum.Parse( typeof( ABCView.ScreenBarButton ) , e.Item.Tag.ToString() );
                    if ( button!=null )
                    {
                        #region Standard Actions
                        if ( button==ABCView.ScreenBarButton.New )
                            Screen.DoAction( ABCScreenAction.New , true );

                        if ( button==ABCView.ScreenBarButton.Edit )
                            Screen.DoAction( ABCScreenAction.Edit , true );

                        if ( button==ABCView.ScreenBarButton.Duplicate )
                            Screen.DoAction( ABCScreenAction.Duplicate , true );

                        if ( button==ABCView.ScreenBarButton.Delete )
                            Screen.DoAction( ABCScreenAction.Delete , true );

                        if ( button==ABCView.ScreenBarButton.Cancel )
                            Screen.DoAction( ABCScreenAction.Cancel , true );

                        if ( button==ABCView.ScreenBarButton.Save )
                        {
                            View.SelectNextControl( View.ActiveControl , true , true , true , true );
                            Application.DoEvents();

                            Screen.DoAction( ABCScreenAction.Save , true );
                        }
                        if ( button==ABCView.ScreenBarButton.Refresh )
                            Screen.DoAction( ABCScreenAction.Refresh , true );

                        if ( button==ABCView.ScreenBarButton.Post )
                            Screen.DoAction( ABCScreenAction.Post , true );

                        if ( button==ABCView.ScreenBarButton.UnPost )
                            Screen.DoAction( ABCScreenAction.UnPost , true );

                        if ( button==ABCView.ScreenBarButton.Approve )
                            Screen.DoAction( ABCScreenAction.Approve , true );

                        if ( button==ABCView.ScreenBarButton.Reject )
                            Screen.DoAction( ABCScreenAction.Reject , true );

                        if ( button==ABCView.ScreenBarButton.Lock )
                            Screen.DoAction( ABCScreenAction.Lock , true );

                        if ( button==ABCView.ScreenBarButton.UnLock )
                            Screen.DoAction( ABCScreenAction.UnLock , true );

                        if ( button==ABCView.ScreenBarButton.Search )
                            Screen.DoAction( ABCScreenAction.Search , true );

                        if ( button==ABCView.ScreenBarButton.Studio )
                            Screen.DoAction( ABCScreenAction.Custom , true );

                        if ( button==ABCView.ScreenBarButton.Print )
                            Screen.DoAction( ABCScreenAction.Print , true );

                        if ( button==ABCView.ScreenBarButton.Info )
                            Screen.DoAction( ABCScreenAction.Info , true );

                        #endregion
                    }
                }
                else if ( e.Item.Tag is Guid )
                {
                    if ( e.Item.Name.StartsWith( "NewFromRelation" ) )
                        Screen.DoActionNewFromRelation( new ABCStandardEventArg( (Guid)e.Item.Tag ) );
                }
            }
            else
            {
                //Extend Actions : user define
                Screen.DoActionEx( sender , e );
            }
        }
       
        public bool IsToolBarButtonVisibility ( ABCView.ScreenBarButton button )
        {
            return this.View.IsToolBarButtonVisibility( button );
        }
        public void HiddenAllToolBarButtons( )
        {
            this.View.HiddenAllToolBarButtons();
        }
        public void SetToolBarButtonVisibility ( ABCView.ScreenBarButton button , bool isVisible )
        {
            if ( this.View.IsToolBarButtonVisibility( button )==isVisible )
                return;

            this.View.SetToolBarButtonVisibility( button , isVisible );
        }
        public void SetToolBarButtonVisibility ( object tag , bool isVisible )
        {
            this.View.SetToolBarButtonVisibility( tag , isVisible );
        }
        public void SetToolBarButtonCaption ( object tag , String strCaption )
        {
            this.View.SetToolBarButtonCaption( tag , strCaption );
        }
        public void SetToolBarButtonEnable ( object tag , bool isEnable )
        {
            this.View.SetToolBarButtonEnable( tag , isEnable );
        }

        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag , String strName , int iImageIndex )
        {
            return this.View.AddNewToolbarButton( strCaption , tag , strName , iImageIndex );
        }
        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            return this.View.AddNewToolbarButton( strCaption , tag , strName , img );
        }
        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag )
        {
            return this.View.AddNewToolbarButton( strCaption , tag );
        }
        
        #region WithParent
        #region ParentToolbarButton
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag , String strName , int iImageIndex )
        {
            return this.View.AddNewParentToolbarButton( strCaption , tag , strName , iImageIndex );
        }
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            return this.View.AddNewParentToolbarButton( strCaption , tag , strName , img );
        }
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag )
        {
            return this.View.AddNewParentToolbarButton( strCaption , tag );
        }

        #endregion
        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag , String strName , int iImageIndex )
        {
            return this.View.AddNewToolbarButton( strParentName , strCaption , tag , strName , iImageIndex );
        }
        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            return this.View.AddNewToolbarButton( strParentName , strCaption , tag , strName , img );
        }
        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag )
        {
            return this.View.AddNewToolbarButton( strParentName , strCaption , tag );
        }
        #endregion
       
        #endregion

        #region Events
        public virtual void InitializeEvents ( )
        {
            foreach ( ABCSimpleButton button in this.View.ButtonList )
            {
                if ( button.ButtonType!=ABCSimpleButton.ABCButtonType.None )
                    button.Click+=new EventHandler( Button_Click );
            }
            foreach ( ABCComment comment in this.View.CommentList )
                comment.AddCommentEvent+=new ABCComment.ABCAddCommentEventHandler( OnAddCommentEvent );
        }

        public void OnAddCommentEvent ( ABCComment.CommentObject comment , ABCStandardEventArg e )
        {
        }

        void Button_Click ( object sender , EventArgs e )
        {
            ABCSimpleButton button=sender as ABCSimpleButton;

            if ( button.ButtonType==ABCSimpleButton.ABCButtonType.Save )
            {
                if ( String.IsNullOrWhiteSpace( button.DataSource )==false&&this.Screen.DataManager.DataObjectsList.ContainsKey( button.DataSource ) )
                {
                    DialogResult result=ABCHelper.ABCMessageBox.Show( "Bạn có muốn lưu dữ liệu vào hệ thống ?" , "Xác nhận" , MessageBoxButtons.YesNo , MessageBoxIcon.Warning );
                    if ( result==DialogResult.Yes )
                    {
                        this.Screen.DataManager.DataObjectsList[button.DataSource].Save(true,true);
                        this.Screen.DataManager.DataObjectsList[button.DataSource].Refresh();
                    }
                }
            }
            else if ( button.ButtonType==ABCSimpleButton.ABCButtonType.Delete )
            {
                if ( String.IsNullOrWhiteSpace( button.DataSource )==false&&this.Screen.DataManager.DataObjectsList.ContainsKey( button.DataSource ) )
                {
                    DialogResult result=ABCHelper.ABCMessageBox.Show( "Bạn có thực sự muốn xóa dữ liệu ?" , "Xác nhận" , MessageBoxButtons.YesNo , MessageBoxIcon.Warning );
                    if ( result==DialogResult.Yes )
                    {
                        this.Screen.DataManager.DataObjectsList[button.DataSource].Delete();
                        this.Screen.DataManager.DataObjectsList[button.DataSource].Refresh();
                    }
                }
            }

            else if ( button.ButtonType==ABCSimpleButton.ABCButtonType.Cancel )
            {
                this.Screen.UIManager.View.FindForm().Close();
            }
        }

        #endregion
    }
}
