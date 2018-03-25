using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using DevExpress.XtraEditors.DXErrorProvider;

using ABCControls;
using ABCBusinessEntities;
using ABCScreen.Data;
using ABCScreen.UI;
using ABCProvider;
using ABCProvider;
using ABCProvider;
using ABCCommon;
using System.Reflection;
namespace ABCScreen
{


    public class ABCBaseScreen : System.MarshalByRefObject
    {
        public UIManager UIManager;
        public DataManager DataManager;
        public ABCScreenStatus ScreenStatus=ABCScreenStatus.None;

        public ABCBaseScreen ( )
        {
            UIManager=new UIManager( this );
            DataManager=new DataManager( this );

        }
        public ABCBaseScreen ( String strViewNo , ViewMode mode )
            : this()
        {
            STViewsInfo viewInfo=(STViewsInfo)new STViewsController().GetObjectByNo( strViewNo );
            LoadScreen( viewInfo , mode );
        }
        public ABCBaseScreen ( STViewsInfo viewInfo , ViewMode mode )
            : this()
        {
            LoadScreen( viewInfo , mode );
        }


        #region LoadScreen - Show
        public virtual void LoadScreen ( STViewsInfo viewInfo , ViewMode mode )
        {
                        ABCStandardEventArg arg=new ABCStandardEventArg();
                        OnScreenLoading( arg );
                        if ( arg.Cancel )
                            return;
            
                        OnUILoading( arg );
                        if ( arg.Cancel )
                            return;

            UIManager.LoadView( viewInfo , mode );
          
                        OnUILoaded();

                        OnDataLoading( arg );
                        if ( arg.Cancel )
                            return;

            LoadData();

                        OnDataLoaded();

            LoadChildrenScreen( mode );

                        OnScreenLoaded();
        }
        public void LoadScreen ( String strFileName , ViewMode mode )
        {
                        ABCStandardEventArg arg=new ABCStandardEventArg();
                        OnScreenLoading( arg );
                        if ( arg.Cancel )
                            return;

                        OnUILoading( arg );
                        if ( arg.Cancel )
                            return;

            UIManager.LoadView( strFileName , mode );

                        OnUILoaded();

                        OnDataLoading( arg );
                        if ( arg.Cancel )
                            return;

            LoadData();

                        OnDataLoaded();

             LoadChildrenScreen( mode );

                        OnScreenLoaded();
        }

        public virtual void LoadChildrenScreen ( ViewMode mode )
        {
            foreach ( ABCView childView in UIManager.View.ChildrenView )
            {
                ABCScreen.ABCBaseScreen childScr=ABCScreenFactory.GetABCScreen( childView.DataField , mode );
                if ( childScr!=null )
                {
                    childScr.UIManager.View.ShowToolbar=childView.ShowToolbar;
                    childScr.UIManager.View.Dock=DockStyle.Fill;

                    int iIndex=childView.Parent.Controls.GetChildIndex( childView );
                    ABCCollapseGroupControl collapse=new ABCCollapseGroupControl();
                    collapse.Location=childView.Location;
                    collapse.Size=childView.Size;
                    collapse.Controls.Add( childScr.UIManager.View );
                    childView.Parent.Controls.Add( collapse );
                  
                    childView.Parent.Controls.Remove( childView );

                    collapse.Parent.Controls.SetChildIndex( collapse , iIndex );
                   
                }
            }
        }

        #region LoadView
        public virtual void LoadToolbar ( )
        {
            UIManager.InitializeToolbar();
        }
     
        #endregion

        #region LoadData
        public virtual void LoadData ( )
        {
            InitDataConfig();

            ImplementBinding();

            LoadToolbar();

            InvalidateData();

            DataManager.InitSearchPanels();

        }
        public virtual void InitDataConfig ( )
        {
            DataManager.InitDataConfig( UIManager.View.DataConfig );

        }
        public virtual void ImplementBinding ( )
        {
            DataManager.ImplementBinding( UIManager.View , null );
        }
        #endregion

        #region Show

        public DialogResult DialogResult=DialogResult.Cancel;

        public ABCControls.ABCViewDlg ViewForm;
        public virtual void Show ( )
        {
            ViewForm=new ABCViewDlg( UIManager.View );
            ViewForm.FormClosing+=new FormClosingEventHandler( Dialog_FormClosing );
            ViewForm.FormClosed+=new FormClosedEventHandler( Dialog_FormClosed );
            ABCScreenManager.Instance.ShowForm( ViewForm , false );
        }
        public virtual void Show (String strCaption )
        {
            ViewForm=new ABCViewDlg( UIManager.View );
            ViewForm.FormClosing+=new FormClosingEventHandler( Dialog_FormClosing );
            ViewForm.FormClosed+=new FormClosedEventHandler( Dialog_FormClosed );
            ViewForm.Text=strCaption;
            ABCScreenManager.Instance.ShowForm( ViewForm , false );
        }

        public virtual void ShowDialog ( )
        {
            ViewForm=new ABCViewDlg( UIManager.View );
            ViewForm.FormClosing+=new FormClosingEventHandler( Dialog_FormClosing );
            ViewForm.FormClosed+=new FormClosedEventHandler( Dialog_FormClosed );
            ABCScreenManager.Instance.ShowForm( ViewForm , true );
        }

        public ABCControls.ABCViewDlg GetDialog ( )
        {
            ViewForm=new ABCViewDlg( UIManager.View );
            ViewForm.FormClosing+=new FormClosingEventHandler( Dialog_FormClosing );
            ViewForm.FormClosed+=new FormClosedEventHandler( Dialog_FormClosed );
            return ViewForm;
        }


        void Dialog_FormClosing ( object sender , FormClosingEventArgs e )
        {
            ABCStandardEventArg arg=new ABCStandardEventArg();
            OnScreenClosing( arg );
            e.Cancel=e.Cancel;
            if ( arg.Cancel )
                return;

            if ( this.ScreenStatus==ABCScreenStatus.Edit||this.ScreenStatus==ABCScreenStatus.New )
            {
                String strMessage=String.Empty;
                String strCaption=String.Empty;
                if ( ABCApp.ABCDataGlobal.Language=="EN" )
                {
                    strMessage=String.Format( "Do you  want to Save '{0}' before close?" , ViewForm.Text );
                    strCaption="Saving Comfirmation";
                }
                else
                {
                    strMessage=String.Format( "Bạn có muốn lưu '{0}' vào hệ thống ?" , ViewForm.Text );
                    strCaption="Xác nhận";
                }
                DialogResult result=ABCHelper.ABCMessageBox.Show( strMessage , strCaption , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                if ( result==DialogResult.Yes )
                    DoAction( ABCScreenAction.Save , false );

                if ( result==DialogResult.Cancel )
                {
                    e.Cancel=true;
                    return;
                }

            }
            if ( this.DataManager.MainObject==null )
            {
                bool isNeedConfim=false;
                foreach ( ABCDataObject data in this.DataManager.DataObjectsList.Values )
                {
                    if ( data.IsModified )
                    {
                        isNeedConfim=true;
                        break;
                    }
                }
                if ( isNeedConfim )
                {
                    DialogResult result=ABCHelper.ABCMessageBox.Show( "Bạn có muốn lưu hay không?" , "Thông báo" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                    if ( result==DialogResult.Yes )
                    {
                        foreach ( ABCDataObject objData in this.DataManager.DataObjectsList.Values )
                        {
                            if ( objData.IsModified||( objData.Config.Children.Count>0&&objData.IsChildrendModified ) )
                                objData.Save( true , true );
                        }
                    }

                    if ( result==DialogResult.Cancel )
                    {
                        e.Cancel=true;
                        return;
                    }
                }
            }
            if ( SearchView!=null )
                SearchView.Close();

        }

        void Dialog_FormClosed ( object sender , FormClosedEventArgs e )
        {
            OnScreenClosed();
        }
       

        #endregion

        #endregion

        #region Invalidate - Refresh
        public virtual void InvalidateData ( )
        {
            foreach ( ABCDataObject obj in DataManager.DataObjectsList.Values )
            {
                if ( obj.Config.Parent==null )
                    obj.ReloadObject();
            }

            ChangeStatus( ABCScreenStatus.LoadedData );
        }

        public virtual void InvalidateData ( Guid iMainID )
        {
            foreach ( ABCDataObject obj in DataManager.DataObjectsList.Values )
            {
                if ( obj.Config.IsMainObject )
                    obj.ReloadObject( iMainID );
            }
            ChangeStatus( ABCScreenStatus.LoadedData );
        }

        public virtual void InvalidateData ( params object[] lstParams )
        {

            ChangeStatus( ABCScreenStatus.LoadedData );
        }

        #region Refresh
        public virtual void RefreshMainObject ( )
        {
            foreach ( ABCDataObject obj in DataManager.DataObjectsList.Values )
            {
                if ( obj.Config.IsMainObject )
                    obj.Refresh();
                RefreshUI(obj.Config.Name);
            }
           
        }

        public virtual void RefreshDataObject ( String strObjectName )
        {
            if ( DataManager.DataObjectsList.ContainsKey( strObjectName ) )
                DataManager.DataObjectsList[strObjectName].Refresh();
        }
        public virtual void RefreshUI ( String strObjectName )
        {
            if ( DataManager.DataObjectsList.ContainsKey( strObjectName ) )
                DataManager.DataObjectsList[strObjectName].RefreshUI();
        }
        #endregion

        #endregion

        #region Events

        #region Load Screen Events
        public delegate void ABCScreenLoadingEventHandler ( ABCStandardEventArg e );
        public delegate void ABCScreenLoadedEventHandler ();

        public delegate void ABCScreenUILoadingEventHandler ( ABCStandardEventArg e );
        public delegate void ABCScreenUILoadedEventHandler ( );
        public delegate void ABCScreenDataLoadingEventHandler ( ABCStandardEventArg e );
        public delegate void ABCScreenDataLoadedEventHandler ( );

        public delegate void ABCScreenClosingEventHandler ( ABCStandardEventArg e );
        public delegate void ABCScreenClosedEventHandler ( );

        public event ABCScreenLoadingEventHandler ScreenLoadingEvent;
        public event ABCScreenLoadedEventHandler ScreenLoadedEvent;

        public event ABCScreenUILoadingEventHandler UILoadingEvent;
        public event ABCScreenUILoadedEventHandler UILoadedEvent;
        public event ABCScreenDataLoadingEventHandler DataLoadingEvent;
        public event ABCScreenDataLoadedEventHandler DataLoadedEvent;

        public event ABCScreenClosingEventHandler ScreenClosingEvent;
        public event ABCScreenClosedEventHandler ScreenClosedEvent;

        public virtual void OnScreenLoading ( ABCStandardEventArg e )
        {
            if ( this.ScreenLoadingEvent!=null )
                this.ScreenLoadingEvent( e );
        }
        public virtual void OnScreenLoaded ( )
        {
            if ( this.ScreenLoadedEvent!=null )
                this.ScreenLoadedEvent();
        }

        public virtual void OnUILoading ( ABCStandardEventArg e )
        {
            if ( this.UILoadingEvent!=null )
                this.UILoadingEvent( e );
        }
        public virtual void OnUILoaded ( )
        {
            if ( this.UILoadedEvent!=null )
                this.UILoadedEvent();
        }
        public virtual void OnDataLoading ( ABCStandardEventArg e )
        {
            if ( this.DataLoadingEvent!=null )
                this.DataLoadingEvent( e );
        }
        public virtual void OnDataLoaded ( )
        {
            if ( this.DataLoadedEvent!=null )
                this.DataLoadedEvent();
        }

        public virtual void OnScreenClosing ( ABCStandardEventArg e )
        {
            if ( this.ScreenClosingEvent!=null )
                this.ScreenClosingEvent( e );
        }
        public virtual void OnScreenClosed ( )
        {
            if ( this.ScreenClosedEvent!=null )
                this.ScreenClosedEvent();
        }
        #endregion

        #region Action Events
        public delegate void ABCActionEventHandler ( ABCScreenAction action , ABCStandardEventArg e );

        public event ABCActionEventHandler BeforeActionEvent;
        public event ABCActionEventHandler AfterActionEvent;

        public virtual void OnBeforeAction ( ABCScreenAction action , ABCStandardEventArg e )
        {
            if ( this.BeforeActionEvent!=null )
                this.BeforeActionEvent( action , e );
        }
        public virtual void OnAfterAction ( ABCScreenAction action , ABCStandardEventArg e )
        {
            if ( this.AfterActionEvent!=null )
                this.AfterActionEvent( action , e );
        }
        
        #endregion

        #region DataObject Events

        public virtual void OnGeneratingFilterQuery ( ABCDataObject data , ABCHelper.ConditionBuilder strBuilder )
        { 
        }

        public virtual void OnSearch ( String strDataSource , String strQuery )
        {
            this.DataManager.DataObjectsList[strDataSource].ReloadObject( strQuery );
        }
        public virtual void OnDataObjectInvalidating ( ABCDataObject data , DataManager.ABCCancelEventArg arg )
        {

        }
        public virtual void OnDataObjectInvalidated ( ABCDataObject data )
        {

        }
        public virtual void OnDataObjectChangingFromUI ( DataManager.ABCDataChangingStructer arg )
        {
            //if ( arg.Control is IABCBindableControl )
            //{
            //    String strDataSource=( arg.Control as IABCBindableControl ).DataSource;
            //    if ( this.DataManager.DataObjectsList.ContainsKey( strDataSource ) )
            //    {
            //        if ( this.DataManager.DataObjectsList[strDataSource].InRelationWithMainObject()
            //            &&this.ScreenStatus!=ABCScreenStatus.New&&this.ScreenStatus!=ABCScreenStatus.Edit )
            //        {
            //            arg.Cancel=true;
            //            //if ( ABCApp.ABCDataGlobal.Language=="VN" )
            //            //    arg.Error="Chỉ có trạng thái 'Thêm Mới' hoặc 'Sửa' mới được hiệu chỉnh thông tin. . .!";
            //            //else
            //            //    arg.Error="Please Edit before modify data . . .!";

            //        }
            //    }
            //}
        }
        public virtual void OnDataObjectChangedFromUI ( DataManager.ABCDataChangedStructer arg )
        {
            if ( arg.Control is IABCBindableControl )
            {
                PropertyInfo property=arg.Control.GetType().GetProperty( "DataSource" );
                if ( property==null )
                    return;

                object objDataSource=property.GetValue( arg.Control , null );
                if ( objDataSource==null||String.IsNullOrWhiteSpace( objDataSource.ToString() ) )
                    return;

                String strDataSource=objDataSource.ToString();
                if ( this.DataManager.DataObjectsList.ContainsKey( strDataSource ) )
                {
                    ABCDataObject obj=this.DataManager.DataObjectsList[strDataSource];
                    obj.IsModified=true;

                    if ( !obj.InRelationWithMainObject()&&obj.Config.AutoSave&&obj.Config.DisplayOnly==false )
                        obj.Save( true , false );

                    if ( RecalculateObject( obj , arg.DataFieldName ) )
                    {
                        if ( !obj.InRelationWithMainObject()&&obj.Config.AutoSave&&obj.Config.DisplayOnly==false )
                            obj.Save( true , false );
                    }
                }
            }
        }

        private bool RecalculateObject ( ABCDataObject obj ,String strAfterValidateFieldName)
        {
            bool isReCalculated=false;

            #region Current
            Guid mainID=( (BusinessObject)obj.Binding.Current ).GetID();
            BaseVoucher voucher=null;
            if ( mainID!=Guid.Empty )
                voucher=VoucherProvider.GetVoucher( obj.TableName , mainID );
            else
                voucher=VoucherProvider.GetVoucher( obj.TableName );

            Dictionary<String , IEnumerable<BusinessObject>> lstObjectItems=new Dictionary<string , IEnumerable<BusinessObject>>();

            if ( voucher!=null )
            {
                foreach ( ABCBindingConfig childConfig in obj.Config.Children.Values.Where( t => t.DisplayOnly==false ).ToList() )
                {
                    if ( voucher.ConfigItems.Count( t => t.ItemTableName==childConfig.TableName )>0&&
                        this.DataManager.DataObjectsList[childConfig.Name].DataObject is IABCList&&lstObjectItems.ContainsKey( childConfig.TableName )==false )
                        lstObjectItems.Add( childConfig.TableName , (IEnumerable<BusinessObject>)this.DataManager.DataObjectsList[childConfig.Name].DataObject );
                }
                isReCalculated=voucher.ReCalculate( (BusinessObject)obj.Binding.Current , lstObjectItems , false , strAfterValidateFieldName,false )||isReCalculated;
            }
            else
            {
                foreach ( ABCBindingConfig childConfig in obj.Config.Children.Values.Where( t => t.DisplayOnly==false ).ToList() )
                {
                    if ( this.DataManager.DataObjectsList[childConfig.Name].DataObject is IABCList&&lstObjectItems.ContainsKey( childConfig.TableName )==false)
                        lstObjectItems.Add( childConfig.TableName , (IEnumerable<BusinessObject>)this.DataManager.DataObjectsList[childConfig.Name].DataObject );
                }
                isReCalculated=FormulaProvider.CalculateMainOnly( (BusinessObject)obj.Binding.Current , lstObjectItems , strAfterValidateFieldName , false )||isReCalculated;
            }
            #endregion

            if ( obj.Config.Parent!=null&&this.DataManager.DataObjectsList.ContainsKey( obj.Config.Parent.Name ) )
            {
                ABCDataObject objParent=this.DataManager.DataObjectsList[obj.Config.Parent.Name];
                isReCalculated=RecalculateObject( objParent , strAfterValidateFieldName )||isReCalculated;
            }
            return isReCalculated;
        }

        public virtual void OnDataObjectChangedFromCode ( object obj , String strProperty )
        {
            if ( obj is ABCDataObject )
                ( obj as ABCDataObject ).IsModified=true;
        }
        #endregion

        #endregion


        #region Actions

        #region Standard Actions

        public void ChangeStatus ( ABCScreenStatus status )
        {
           
            BusinessObject objMain=null;
            String strTableName=String.Empty;

            if ( this.DataManager.MainObject!=null )
            {
                if ( this.DataManager.MainObject.DataObject!=null )
                    objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                strTableName=this.DataManager.MainObject.Config.TableName;
            }

            if ( status==ABCScreenStatus.LoadedData )
            {
                #region AllowView
                if ( this.DataManager.MainObject!=null )
                {
                    if ( ( this.DataManager.MainObject.DataObject!=null&&!VoucherProvider.CanView( objMain ) )
                       ||( this.DataManager.MainObject.DataObject==null&&!VoucherProvider.CanView( strTableName ) ) )
                    {
                        this.DataManager.MainObject.ReloadObject( Guid.Empty );
                        UIManager.HiddenAllToolBarButtons();
                        UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Search , this.DataManager.MainObject!=null );
                        UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.New , this.DataManager.MainObject!=null );
                        return;
                    }
                }
                #endregion
            }
            
            this.ScreenStatus=status;

            switch ( status )
            {
                case ABCScreenStatus.New:
                    UIManager.HiddenAllToolBarButtons();
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Save , true );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Cancel , true );
                    break;

                case ABCScreenStatus.Edit:
                    UIManager.HiddenAllToolBarButtons();
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Save , true );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Cancel , true );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Refresh , true );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Info , true );

                    break;

                case ABCScreenStatus.LoadedData:

                    VoucherProvider.CheckApprovalStatus( objMain );

                    UIManager.HiddenAllToolBarButtons();
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Search , true );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Refresh , objMain!=null&&objMain.GetID()!=Guid.Empty );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Info , objMain!=null&&objMain.GetID()!=Guid.Empty );

                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.New , this.UIManager.BackupToolbarStatus.ShowNewItem&&VoucherProvider.CanNew( strTableName ) );                
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Edit , this.UIManager.BackupToolbarStatus.ShowEditItem&&VoucherProvider.CanEdit( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Duplicate , objMain!=null&&objMain.GetID()!=Guid.Empty&&this.UIManager.BackupToolbarStatus.ShowDuplicateItem&&VoucherProvider.CanNew( strTableName ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Delete , this.UIManager.BackupToolbarStatus.ShowDeleteItem&&VoucherProvider.CanDelete( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Approve , VoucherProvider.CanApprove( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Reject , VoucherProvider.CanReject( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Lock , VoucherProvider.CanLock( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.UnLock , VoucherProvider.CanUnLock( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Post , VoucherProvider.CanPost( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.UnPost , VoucherProvider.CanUnPost( objMain ) );
                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Print , VoucherProvider.CanPrint( objMain ) );

                    UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Utilities , false );
                    if ( this.DataManager.MainObject!=null )
                    {
                        if ( this.UIManager.BackupToolbarStatus.ShowNewItem&&VoucherProvider.CanNew( strTableName ) )
                        {
                            foreach ( GERelationConfigsInfo config in VoucherProvider.GetRelationConfigs( this.DataManager.MainObject.TableName ) )
                            {
                                UIManager.SetToolBarButtonVisibility( ABCView.ScreenBarButton.Utilities , true );
                                UIManager.SetToolBarButtonVisibility( config.GetID() , true );
                            }
                        }
                    }

                    break;
            }
        }

        #region DoActions
        Object BackupMainObject=null;
        public virtual void DoAction ( ABCScreenAction action , bool confirm )
        {
            ABCStandardEventArg arg=new ABCStandardEventArg();
            OnBeforeAction( action , arg );
            if ( arg.Cancel )
                return;

            ABCScreenStatus status=ABCScreenStatus.None;
            switch ( action )
            {
                case ABCScreenAction.New:
                    BackupMainObject=(this.DataManager.MainObject.DataObject as BusinessObject).Clone();
                    ChangeStatus( ABCScreenStatus.New );
                    ActionNew( arg );
                    break;

                case ABCScreenAction.Edit:
                    BackupMainObject=( this.DataManager.MainObject.DataObject as BusinessObject ).Clone();
                    ActionEdit( arg );
                    if ( arg.Cancel )
                        return;

                    ChangeStatus( ABCScreenStatus.Edit );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Duplicate:
                    BackupMainObject=( this.DataManager.MainObject.DataObject as BusinessObject ).Clone();
                    ActionDuplicate( arg );
                    break;

                case ABCScreenAction.Delete:
                    ActionDelete( arg );
                    break;

                case ABCScreenAction.Save:

                    ActionSave( arg , confirm );
                    BackupMainObject=null;
                    if ( arg.Cancel )
                        return;

                    ChangeStatus( ABCScreenStatus.LoadedData );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Cancel:
                    ActionCancel( arg );
                    if ( BackupMainObject!=null )
                    {
                        this.DataManager.MainObject.DataObject=( BackupMainObject as BusinessObject ).Clone();
                        BackupMainObject=null;
                    }
                    ActionRefresh( new ABCStandardEventArg() );
                    ChangeStatus( ABCScreenStatus.LoadedData );
                    break;

                case ABCScreenAction.Refresh:
                    ActionRefresh( arg );
                    break;

                case ABCScreenAction.Post:
                    ActionPost( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.UnPost:
                    ActionUnPost( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Approve:
                    ActionApprove( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Reject:
                    ActionReject( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Lock:
                    ActionLock( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.UnLock:
                    ActionUnLock( arg );
                    ActionRefresh( new ABCStandardEventArg() );
                    break;

                case ABCScreenAction.Search:
                    ActionSearch( arg );
                    break;

                case ABCScreenAction.Custom:
                    ActionCustom( arg );
                    break;

                case ABCScreenAction.Print:
                    ActionPrint( arg );
                    ChangeStatus( ABCScreenStatus.LoadedData );
                    break;

                case ABCScreenAction.Info:
                    ActionInfo( arg );
                    break;
            }

            if ( arg.Cancel )
                return;

            if (SearchView!=null &&(action==ABCScreenAction.Save||action==ABCScreenAction.Delete||action==ABCScreenAction.Approve||action==ABCScreenAction.Reject
                ||action==ABCScreenAction.Lock||action==ABCScreenAction.UnLock||action==ABCScreenAction.Post||action==ABCScreenAction.UnPost))
            {
                SearchView.BindingObject=this.DataManager.MainObject;
                SearchView.Search();
            }

            OnAfterAction( action , new ABCStandardEventArg() );
        }
    
        public virtual void DoActionNew ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
                this.DataManager.MainObject.New();

            if ( this.DataManager.MainObject.DataObject!=null&&this.DataManager.MainObject.DataObject is BusinessObject )
            {
                BusinessObjectHelper.SetDefaultValue( this.DataManager.MainObject.DataObject as BusinessObject );

                if ( String.IsNullOrWhiteSpace( this.UIManager.View.MainValue )==false&&
                    DataStructureProvider.IsTableColumn( this.UIManager.View.MainTableName , this.UIManager.View.MainFieldName ) )
                {
                    try
                    {
                        ABCBusinessEntities.ABCDynamicInvoker.SetValue( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.MainFieldName , this.UIManager.View.MainValue );
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            }
        }
        public virtual void DoActionEdit ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
                this.DataManager.MainObject.Refresh();
        }
        public virtual void DoActionDuplicate ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null||this.DataManager.MainObject.DataObject is BusinessObject==false )
            {
                arg.Cancel=true;
                return;
            }

            ChangeStatus( ABCScreenStatus.New );

            foreach ( ABCDataObject objData in this.DataManager.DataObjectsList.Values )
            {
                if ( objData.Config.IsMainObject )
                {
                    BusinessObjectHelper.SetIDValue( objData.DataObject as BusinessObject , Guid.Empty );
                    BusinessObjectHelper.SetDefaultValue( objData.DataObject as BusinessObject );
                }
                else if ( objData.Config.IsList )
                {
                    foreach ( object objItem in ( objData.DataObject as IABCList ) )
                    {
                        BusinessObjectHelper.SetIDValue( objItem as BusinessObject , Guid.Empty );
                        BusinessObjectHelper.SetDefaultValue( objItem as BusinessObject );
                    }
                }
            }

        }

        public virtual void DoActionDelete ( ABCStandardEventArg arg )
        {
            BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
            String strTableName=objMain.AATableName;
            Guid ID=objMain.GetID();

            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                VoucherProvider.BeforeDeleteVoucher( strTableName , ID );

                this.DataManager.MainObject.Delete();

                VoucherProvider.AfterDeletedVoucher( strTableName , ID );

                VoucherProvider.ReCalculateRelation( strTableName , ID );
                scope.Complete();
            }
        }
        public virtual void DoActionSave ( ABCStandardEventArg arg )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                if ( this.DataManager.MainObject!=null )
                {
                    BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                   
                    if ( String.IsNullOrWhiteSpace( this.UIManager.View.MainValue )==false&&
                        DataStructureProvider.IsTableColumn( this.UIManager.View.MainTableName , this.UIManager.View.MainFieldName ) )
                    {
                        try
                        {
                            ABCBusinessEntities.ABCDynamicInvoker.SetValue( objMain , this.UIManager.View.MainFieldName , this.UIManager.View.MainValue );
                        }
                        catch ( Exception ex )
                        {
                        }
                    }


                    VoucherProvider.BeforeSaveVoucher( objMain.AATableName , objMain.GetID() );

                    this.DataManager.MainObject.Save(true,true);

                    VoucherProvider.AfterSavedVoucher( objMain.AATableName , objMain.GetID() );

                    VoucherProvider.ReCalculateVoucher( objMain.AATableName , objMain.GetID() );

                    DataCachingProvider.RefreshLookupTable( this.DataManager.MainObject.TableName );

                    if ( this.ScreenStatus==ABCScreenStatus.Edit&&this.UIManager.View.DataField!=null )
                        LoggingProvider.LogAction( objMain , this.UIManager.View.DataField.STViewDesc , "Edit" );

                    if ( this.ScreenStatus==ABCScreenStatus.New&&this.UIManager.View.DataField!=null )
                        LoggingProvider.LogAction( objMain , this.UIManager.View.DataField.STViewDesc , "New" );
                }
                else
                {
                    foreach ( ABCDataObject data in this.DataManager.DataObjectsList.Values )
                    {
                        if ( data.Config.Parent==null&&data.Config.IsList==false&&data.Config.IsMainObject==false )
                        {
                            data.Save(true,true);
                            DataCachingProvider.RefreshLookupTable( data.TableName );
                        }
                    }
                }
                scope.Complete();
            }
        }
        public virtual void DoActionCancel ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
                this.DataManager.Invalidate( this.DataManager.MainObject );
        }
        public virtual void DoActionPost ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.PostVoucher( objMain.AATableName , objMain.GetID() );
            }
        }
        public virtual void DoActionUnPost( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.UnPostVoucher( objMain.AATableName , objMain.GetID() );
            }
        }
        public virtual void DoActionApprove ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.ApproveVoucher( objMain.AATableName , objMain.GetID() );
            }
        }
        public virtual void DoActionReject ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.RejectVoucher( objMain.AATableName , objMain.GetID() );
            }
        }
        public virtual void DoActionLock ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.LockVoucher( objMain.AATableName , objMain.GetID() );
            }
        }
        public virtual void DoActionUnLock ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                VoucherProvider.UnLockVoucher( objMain.AATableName , objMain.GetID() );
            }
        }

        public virtual void DoActionPrint ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject objMain=this.DataManager.MainObject.DataObject as BusinessObject;
                //VoucherProvider.UnLockVoucher( objMain.AATableName , objMain.GetID() );
            }
        }


        public virtual void DoActionNewFromRelation ( ABCStandardEventArg arg )
        {
            if ( arg.Tag!=null&&arg.Tag is Guid&&( (Guid)arg.Tag )!=Guid.Empty )
            {
                ABCRelationChooser chooser=new ABCRelationChooser( (Guid)arg.Tag );
                ABCScreenManager.Instance.ShowForm( chooser ,false );
                if ( chooser.DialogResult==System.Windows.Forms.DialogResult.OK&&chooser.DestinyResult !=null)
                    ABCScreenManager.Instance.RunLink( chooser.RelationConfig.DestinyTableName , ViewMode.Runtime , false , chooser.DestinyResult.GetID() , ABCScreenAction.None );
            }
        }

        #endregion

        public virtual void ActionNew ( ABCStandardEventArg arg )
        {
            DoActionNew( arg );

        }
      
        public virtual void ActionEdit ( ABCStandardEventArg arg )
        {
            DoActionEdit( arg );
        }

        public virtual void ActionDuplicate ( ABCStandardEventArg arg )
        {
            DoActionDuplicate( arg );
        }

        public virtual void ActionDelete ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                String strMessage=String.Empty;
                String strCaption=String.Empty;

                if ( ABCApp.ABCDataGlobal.Language=="EN" )
                {
                    strMessage="Do you realy want to use this Delete function ?";
                    strCaption="Deleting Comfirmation";
                }
                else
                {
                    strMessage="Bạn có thực sự muốn xóa dữ liệu trong hệ thống ?";
                    strCaption="Xác nhận";
                }

                if ( DialogResult.Yes==ABCHelper.ABCMessageBox.Show( strMessage , strCaption , MessageBoxButtons.YesNo , MessageBoxIcon.Warning ) )
                {
                    DoActionDelete( arg );
                    if ( arg.Cancel )
                        return;

                    DataCachingProvider.RefreshLookupTable( this.DataManager.MainObject.TableName );
                    this.DataManager.MainObject.ReloadObject();

                    foreach ( ABCDataObject data in this.DataManager.DataObjectsList.Values )
                    {
                        if ( data.Config.Parent==null&&data.Config.IsList )
                        {
                            if ( this.DataManager.MainObject!=null&&this.DataManager.MainObject.TableName==data.TableName )
                                data.Refresh();
                        }
                    }
                    if ( this.UIManager.View.DataField!=null )
                        LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "Delete" );
                }
            }
        }
       
        public virtual void ActionSave ( ABCStandardEventArg arg , bool confirm )
        {
            if ( this.DataManager.MainObject!=null )
            {
                if ( confirm )
                {
                    String strMessage=String.Empty;
                    String strCaption=String.Empty;

                    if ( ABCApp.ABCDataGlobal.Language=="EN" )
                    {
                        strMessage="Do you  want to Save ?";
                        strCaption="Saving Comfirmation";
                    }
                    else
                    {
                        strMessage="Bạn có muốn lưu dữ liệu vào hệ thống ?";
                        strCaption="Xác nhận";
                    }
                    DialogResult result=ABCHelper.ABCMessageBox.Show( strMessage , strCaption , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                    if ( result==DialogResult.Yes )
                        DoActionSave( arg );

                    if ( result==DialogResult.Cancel )
                        arg.Cancel=true;
                }
                else
                {
                    DoActionSave( arg );
                }
            }

            if ( arg.Cancel==false )
            {
                foreach ( ABCDataObject data in this.DataManager.DataObjectsList.Values )
                {
                    if ( data.Config.Parent==null&&data.Config.IsList )
                    {
                        if ( this.DataManager.MainObject!=null&&this.DataManager.MainObject.TableName==data.TableName )
                            data.Refresh();
                    }
                }
            }

        }

        public virtual void ActionCancel ( ABCStandardEventArg arg )
        {
            DoActionCancel( arg );
        }

        public virtual void ActionRefresh ( ABCStandardEventArg arg )
        {
            RefreshMainObject();
            if ( ScreenStatus==ABCScreenStatus.New||ScreenStatus==ABCScreenStatus.Edit )
                return;
            ChangeStatus( ABCScreenStatus.LoadedData );
        }

        public virtual void ActionPost ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document to Post!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu trước khi Ghi Sổ";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before Posting!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi Ghi Sổ";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionPost( arg );

     //       DataCachingProvider.RefreshLookupTable( "GLJournalVouchers" );
        }
   
        public virtual void ActionUnPost ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document to UnPost!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu trước khi Hủy Ghi Sổ";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before UnPosting!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi Hủy Ghi Sổ";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionUnPost( arg );

            //       DataCachingProvider.RefreshLookupTable( "GLJournalVouchers" );
        }

        public virtual void ActionApprove ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document for Approval!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu trước khi Phê Duyệt";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before Approval!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi Phê Duyệt";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionApprove( arg );
            if ( arg.Cancel )
                return;
            if ( this.UIManager.View.DataField!=null )
                LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "Approve" );
        }
    
        public virtual void ActionReject ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document for Reject!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu trước khi Giữ Lại";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before Reject!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi Giữ Lại";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionReject( arg );
            if ( arg.Cancel )
                return;
            if ( this.UIManager.View.DataField!=null )
                LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "Reject" );
        }

        public virtual void ActionLock ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document to Lock!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu!";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before Lock!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi thực hiện Khóa Phiếu";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionLock( arg );
            if ( arg.Cancel )
                return;

            if ( this.UIManager.View.DataField!=null )
                LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "Lock" );
        }
    
        public virtual void ActionUnLock ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document to UnLock!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu!";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before UnLock!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi thực hiện Mở Khóa";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionUnLock( arg );
            if ( arg.Cancel )
                return;

            if ( this.UIManager.View.DataField!=null )
                LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "UnLock" );
        }

        #region ActionSearch
        ABCSearchView SearchView;
        public virtual void ActionSearch ( ABCStandardEventArg arg )
        {
            if ( SearchView!=null )
            {
                SearchView.BindingObject=this.DataManager.MainObject;
                SearchView.Search();
                ABCScreenManager.Instance.ShowForm( SearchView , false );
            }
            else
            {
                if ( this.DataManager.MainObject!=null )
                {
                    SearchView=new ABCSearchView( this.DataManager.MainObject );
                    SearchView.Search();

                    STViewsInfo viewInfo=null;
                    if ( this.UIManager.View.DataField!=null )
                        viewInfo=(STViewsInfo)new STViewsController().GetObjectByNo( this.UIManager.View.DataField.STViewNo );
                    if ( viewInfo==null )
                    {
                        if ( ABCStudio.ABCStudioHelper.Instance!=null )
                            SearchView.Show( ABCStudio.ABCStudioHelper.Instance.MainStudio );
                        else
                            SearchView.Show( ABCApp.ABCAppHelper.Instance.MainForm );
                    }
                    else
                    {
                        //    ABCBaseScreen SearchScreen=ABCScreen.ABCScreenFactory.GetABCScreen( viewInfo , UIManager.View.Mode );
                        foreach ( ABCDataObject obj in this.DataManager.DataObjectsList.Values )
                        {
                            if ( obj.TableName==this.DataManager.MainObject.TableName )
                            {
                                if ( obj.Binding.Current!=null )
                                {
                                    obj.Binding.PositionChanged+=new EventHandler( SearchBindingSource_PositionChanged );
                                    if ( obj.DataObject is IABCList )
                                        ( obj.DataObject as IABCList ).GridControl.GridDefaultView.DoubleClick+=new EventHandler( GridDefaultView_DoubleClick );
                                }
                                break;
                            }
                        }

                        ABCScreenManager.Instance.ShowForm( SearchView , false );
                    }
                }
            }
        }

        void GridDefaultView_DoubleClick ( object sender , EventArgs e )
        {
            ABCGridView gridView=sender as ABCGridView;
            try
            {
                this.InvalidateData( ( ( gridView.ABCGridControl.GridDataSource as BindingSource ).Current as BusinessObject ).GetID() );
             //   this.DataManager.MainObject.ReloadObject( (gridView.ABCGridControl.GridDataSource as BindingSource).Current );
            }
            catch ( Exception ex )
            {
            }
            Form frm=( gridView.GridControl ).FindForm();
            frm.Close();

        }

        void SearchBindingSource_PositionChanged ( object sender , EventArgs e )
        {
            BindingSource bindingSource=sender as BindingSource;
            this.InvalidateData( ( bindingSource.Current as BusinessObject ).GetID() );
          //  this.DataManager.MainObject.ReloadObject( bindingSource.Current );
        }
        
        #endregion

        public virtual void ActionCustom ( ABCStandardEventArg arg )
        {
            if ( this.UIManager.View.Mode!=ViewMode.Runtime||this.UIManager.View.DataField==null )
            {
                ABCHelper.ABCMessageBox.Show( "Can not customize this form! It's alreadys in design mode" , "Customize Form" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                return;
            }

            if ( this.UIManager.View.DataField!=null )
            {
                Guid iBackupMainID=Guid.Empty;
                if ( this.DataManager.MainObject!=null&&this.DataManager.MainObject.DataObject!=null&&this.DataManager.MainObject.DataObject is BusinessObject )
                {
                    iBackupMainID=BusinessObjectHelper.GetIDValue( this.DataManager.MainObject.DataObject as BusinessObject );
                }

                String strBackupXML=this.UIManager.View.DataField.STViewXML;
                String strBackupSource=this.UIManager.View.DataField.STViewCode;
                bool isBackupUserCode=this.UIManager.View.DataField.STViewUseCode;

               
                ABCApp.ABCAppHelper.Instance.CustomizeView( this.UIManager.View.DataField );
                STViewsInfo viewInfo=(STViewsInfo)new STViewsController().GetObjectByID( this.UIManager.View.DataField.STViewID );
                if ( viewInfo.STViewUseCode!=isBackupUserCode||viewInfo.STViewXML!=strBackupXML
                    ||( viewInfo.STViewUseCode==isBackupUserCode&&isBackupUserCode&&viewInfo.STViewCode!=strBackupSource ) )
                {
                    LoadScreen( viewInfo , this.UIManager.View.Mode );
                    if ( iBackupMainID!=Guid.Empty )
                        InvalidateData( iBackupMainID );
                }
            }
        }

        public virtual void ActionPrint ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject==null||this.DataManager.MainObject.DataObject==null )
            {
                String strAlert="Please choose 1 document for Print!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng chọn 1 phiếu trước khi In Phiếu";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }

            if ( this.ScreenStatus==ABCScreenStatus.New||this.ScreenStatus==ABCScreenStatus.Edit )
            {
                String strAlert="Please Save this document before Print!";
                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    strAlert="Vui lòng lưu phiếu trước khi In Phiếu";
                ABCHelper.ABCMessageBox.Show( strAlert );
                arg.Cancel=true;
                return;
            }
            DoActionPrint( arg );
            if ( arg.Cancel )
                return;
            if ( this.UIManager.View.DataField!=null )
                LoggingProvider.LogAction( this.DataManager.MainObject.DataObject as BusinessObject , this.UIManager.View.DataField.STViewDesc , "Print" );
        }
        public virtual void ActionInfo ( ABCStandardEventArg arg )
        {
            if ( this.DataManager.MainObject!=null )
            {
                BusinessObject obj=( this.DataManager.MainObject.DataObject as BusinessObject );         
                if ( BusinessObjectHelper.GetIDValue( obj )!=Guid.Empty )
                    ABCObjectInformation.ShowObjectInfo( obj );

            }
        }

        #endregion

        #region Extends Actions

        public virtual void DoActionEx ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ABCStandardEventArg arg=new ABCStandardEventArg( e.Item );
            OnBeforeActionEx( sender , arg );
            if ( arg.Cancel )
                return;

            OnAfterActionEx( e.Item.Tag , new ABCStandardEventArg( e.Item ) );
        }

        public delegate void ABCActionExEventHandler ( object tag , ABCStandardEventArg e );
        public event ABCActionExEventHandler BeforeActionExEvent;
        public event ABCActionExEventHandler AfterActionExEvent;
        public virtual void OnBeforeActionEx ( object tag , ABCStandardEventArg e )
        {
            if ( this.BeforeActionExEvent!=null )
                this.BeforeActionExEvent( tag , e );
        }
        public virtual void OnAfterActionEx ( object tag , ABCStandardEventArg e )
        {
            if ( this.AfterActionExEvent!=null )
                this.AfterActionExEvent( tag , e );
        }

        #endregion

        #endregion

        #region Error Provider
        public DXErrorProvider ErrorProvider=new DXErrorProvider();
        #endregion

        public void Close ( )
        {
            if ( ViewForm!=null )
                ViewForm.Close();

            GC.Collect();
        }
    }
}
