using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;

using ABCCommon;
using ABCBusinessEntities;


namespace ABCControls
{


    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.XtraUserControl ) )]
    [Designer( typeof( ABCViewDesigner ) )]
    [Designer( typeof( System.Windows.Forms.Design.ScrollableControlDesigner ) )]
    public class ABCView : DevExpress.XtraEditors.XtraUserControl , IABCControl
    {
        public ABCView OwnerView { get; set; }

        [Category( "ABC" )]
        public bool IsUseSourceCode
        {
            get
            {
                return isUseSourceCode;
            }
            set
            {
                isUseSourceCode=value;
                if ( this.DataField!=null )
                    this.DataField.STViewUseCode=value;
            }
        }
        bool isUseSourceCode=false;

        #region ABC.Main
        [Category( "ABC.Main" )]
        public String MainTableName
        {
            get
            {
                return strMainTableName;
            }
            set
            {
                strMainTableName=value;
                if ( this.DataField!=null )
                    this.DataField.MainTableName=value;
            }
        }
        String strMainTableName=String.Empty;

        [Category( "ABC" )]
        public String MainFieldName
        {
            get
            {
                return strMainFieldName;
            }
            set
            {
                strMainFieldName=value;
                if ( this.DataField!=null )
                    this.DataField.MainFieldName=value;
            }
        }
        String strMainFieldName=String.Empty;

        [Category( "ABC" )]
        public String MainValue
        {
            get
            {
                return strMainValue;
            }
            set
            {
                strMainValue=value;
            //    if ( this.DataField!=null )
                //    this.DataField.MainValue=value;
            }
        }
        String strMainValue=String.Empty;

        #endregion

        [Category( "Search" )]
        public String SearchForm { get; set; }

        #region Binding


        [Category( "BindConfig" )]
        [Editor( typeof( ABCBusinessConfigEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "BindConfig" ) , Description( "Which type to use..." )]
        public new ABCScreen.ABCScreenConfig DataConfig { get; set; }

        public void RefreshBindingControl ( )
        {
            foreach ( IComponent comp in this.Surface.DesignerHost.Container.Components )
            {
                if ( comp is ABCBindingBaseEdit )
                    ( (ABCBindingBaseEdit)comp ).InvalidateControl();
            }
        }
        #endregion

        public List<ABCView> ChildrenView=new List<ABCView>();
        public List<ABCSimpleButton> ButtonList=new List<ABCSimpleButton>();
        public List<ABCComment> CommentList=new List<ABCComment>();
        public ABCView ( )
        {

            InitializeComponent();

            InitMenuBar();

            //IsJoinBindingWithParent=false;
            //IsUseSourceCode=false;
            //ShowToolbar=false;


            this.ControlAdded+=new ControlEventHandler( ABCView_ControlAdded );

        }

        #region UI

        public STViewsInfo DataField;
        public String XMLFileName;

        public ViewMode Mode;
        public HostSurface Surface;
        public bool IsRoot=false;

        #region External Properties

        [Category( "ABC" )]
        [ReadOnly( true )]
        public Guid ViewID
        {
            get
            {
                if ( DataField!=null )
                    return DataField.STViewID;
                else
                    return Guid.Empty;
            }
        }


        [Category( "ABC" )]
        public String Name { get; set; }

        [Category( "ABC" )]
        public String Caption { get; set; }

        [Category( "ABC" )]
        public String ScreenName { get; set; }

        [Category( "ABC" )]
        public bool DisplayInTree { get; set; }

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

        [Category( "ABC.Layout" )]
        public FormWindowState WindowState { get; set; }

        [Category( "ABC.Layout" )]
        public FormStartPosition StartPosition { get; set; }

        [Category( "ABC.Layout" )]
        public FormBorderStyle FormBorderStyle { get; set; }

        [Category( "ABC.Layout" )]
        public Boolean ControlBox { get; set; }
        [Category( "ABC.Layout" )]
        public Boolean MinimizelBox { get; set; }
        [Category( "ABC.Layout" )]
        public Boolean MaximizeBox { get; set; }


        bool isVisible=true;
        [Category( "External" )]
        public Boolean IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible=value;
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }
        #endregion


        #region Default UI

        public void InitControl ( )
        {
            if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                InitDesignTime();
            else
                InitRunTime();

            InitBothMode();
        }

        public void InitBothMode ( )
        {

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;
            if ( String.IsNullOrWhiteSpace( this.Caption ) )
            {
                if ( this.DataField!=null )
                    this.Caption=this.DataField.STViewNo;
                else
                    this.Caption=this.Name;
            }
        }

        public void InitRunTime ( )
        {
            this.AutoScroll=false;
        }
        public void InitDesignTime ( )
        {

            this.AutoScroll=true;
        }

        private void InitializeComponent ( )
        {

            this.SuspendLayout();

            // 
            // ABCView
            // 
            this.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
            this.Name="ABCView";
            this.Size=new System.Drawing.Size( 464 , 398 );
            this.ResumeLayout( false );

            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.WindowState=FormWindowState.Normal;
            this.StartPosition=FormStartPosition.CenterScreen;
            this.ControlBox=true;
            this.MaximizeBox=true;
            this.MinimizelBox=true;

        }
        protected override void OnPaint ( PaintEventArgs pe )
        {


            if ( this.OwnerView==null||OwnerView.Mode==ViewMode.Design&&this.Controls.Count<=0 )
            {
                float padx=( (float)this.Size.Width )*( 0.5F );
                float pady=( (float)this.Size.Height )*( 0.5F );

                Font font=new Font( Font.FontFamily.Name , 12.0f );
                pe.Graphics.DrawString( "[- ABC Studio -]" , font , new SolidBrush( Color.Gray ) , 10 , ( (float)this.Size.Height )-25 );

                Font font2=new Font( Font.FontFamily.Name , 10.0f );
                pe.Graphics.DrawString( "Drag and Drop components here." , font2 , new SolidBrush( Color.Gray ) , padx-100 , pady-12 );
            }

        }

        #region CurrentDockManager

        public ABCDockManager CurrentDockManager;

        void ABCView_ControlAdded ( object sender , ControlEventArgs e )
        {
            //if ( e.Control is DevExpress.XtraBars.Docking.DockPanel&&Surface!=HostSurfaceManager.TemplateView.HostSurface )
            //{
            //    DevExpress.XtraBars.Docking.DockPanel panel=(DevExpress.XtraBars.Docking.DockPanel)e.Control;
            //    panel.Register( GetDockManager() );
            //    panel.DockTo( panel.Dock );
            //}
        }

        #endregion

        #endregion

        #region Load

        public void Load ( XmlDocument doc , ViewMode mode )
        {
            Mode=mode;

            this.SuspendLayout();
            try
            {

                DataConfig=new ABCScreen.ABCScreenConfig( this );
                DataConfig.DeSerialization( doc );

                XmlNodeList nodeList=doc.GetElementsByTagName( "C" );
                ABCPresentHelper.LoadComponent( this , nodeList[0] );

                ABCDockManager.InitDockManager( this , doc );
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message+" : "+ex.StackTrace );
            }

            this.InitControl();
            this.ResumeLayout( false );

            if ( this.Surface!=null )
                this.Surface.UndoEngine.CleanEngine();
        }
        public void Load ( STViewsInfo viewInfo , ViewMode mode )
        {
            DataField=viewInfo;

            XmlDocument doc=new XmlDocument();
            string strDecompress=ABCHelper.StringCompressor.DecompressString( viewInfo.STViewXML );
            doc.LoadXml( strDecompress );

            Load( doc , mode );

            this.Caption=viewInfo.STViewDesc;
            this.ScreenName=viewInfo.ScreenName;
            this.DisplayInTree=viewInfo.DisplayInTree;
            this.MainTableName=viewInfo.MainTableName;
            this.MainFieldName=viewInfo.MainFieldName;
            this.MainValue=viewInfo.MainValue;
        }
        public void Load ( String strFileName , ViewMode mode )
        {
            XMLFileName=strFileName;

            XmlDocument doc=new XmlDocument();
            doc.Load( strFileName );

            Load( doc , mode );
        }

        #endregion

        #region Save

        public void Save ( )
        {
            if ( DataField!=null )
            {
           
                ABCHelper.ABCWaitingDialog.Show( "" ,  "Saving . . .!" );

                DataField.STViewUseCode=this.IsUseSourceCode;
                DataField.MainTableName=this.MainTableName;
                DataField.MainFieldName=this.MainFieldName;
                DataField.MainValue=this.MainValue;

                if ( String.IsNullOrWhiteSpace( DataField.MainTableName )&&this.DataConfig!=null )
                {
                    foreach ( ABCScreen.ABCBindingConfig config in this.DataConfig.BindingList.Values )
                    {
                        if ( config.IsMainObject )
                        {
                            DataField.MainTableName=config.TableName;
                            this.MainTableName=config.TableName;
                            break;
                        }
                    }
                }

                XmlDocument doc=GetXMLDocument();
                DataField.STViewXML=ABCHelper.StringCompressor.CompressString( doc.InnerXml );
                DataField.STViewDesc=this.Caption;
                DataField.ScreenName=this.ScreenName;
                DataField.DisplayInTree=this.DisplayInTree;
                DataField.MainTableName=this.MainTableName;
                DataField.MainFieldName=this.MainFieldName;
                DataField.MainValue=this.MainValue;
                new STViewsController().UpdateObject( DataField );

                HostSurfaceManager.CurrentManager.RefreshSurface( DataField.STViewID );

                ABCHelper.ABCWaitingDialog.Close();
            }
            else
            {
                if ( String.IsNullOrWhiteSpace( this.XMLFileName )||File.Exists( this.XMLFileName )==false )
                {
                    SaveFileDialog dlg=new SaveFileDialog();
                    dlg.Filter="xml files (*.xml)|*.xml|All files (*.*)|*.*";
                    dlg.FilterIndex=2;
                    dlg.RestoreDirectory=true;
                    dlg.Title="Save Layout to XML file";
                    if ( dlg.ShowDialog()==DialogResult.OK )
                        this.XMLFileName=dlg.FileName;

                }

                if ( String.IsNullOrWhiteSpace( this.XMLFileName )==false )
                    SaveToXML( this.XMLFileName );
            }

            if ( this.OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                this.Surface.UndoEngine.CanUndo=false;

        }

        public void SaveToXML ( String strFileName )
        {
            XmlDocument doc=GetXMLDocument();
            doc.Save( strFileName );
        }

        #region GetXMLLayout

        List<String> CompNameList=new List<string>();
        public XmlDocument GetXMLDocument ( )
        {
            XmlDocument doc=new XmlDocument();
            XmlDeclaration dec=doc.CreateXmlDeclaration( "1.0" , "utf-16" , null );
            doc.AppendChild( dec );

            XmlElement root=doc.CreateElement( "root" );

            if ( DataConfig!=null )
                DataConfig.Serialization( root );

            #region layout

            CompNameList.Clear();
            ICollection ControlCollection;
            if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
            {
                foreach ( IComponent comp in this.Surface.DesignerHost.Container.Components )
                {
                    if ( CompNameList.Contains( comp.Site.Name )==false )
                    {
                        if ( comp is DevExpress.XtraBars.Docking.ControlContainer )
                            continue;

                        XmlElement ele=null;
                        if ( comp is ABCDockManager )
                            ele=( (ABCDockManager)comp ).GetXMLLayout( doc );
                        else
                            ele=ComponentSerialization( doc , comp );

                        if ( ele==null )
                            continue;

                        root.AppendChild( ele );

                    }
                }
            }
            else
            {
                foreach ( IComponent comp in this.Controls )
                {
                    if ( CompNameList.Contains( ( comp as Control ).Name )==false )
                    {
                        if ( comp is DevExpress.XtraBars.Docking.ControlContainer )
                            continue;

                        XmlElement ele=null;
                        if ( comp is ABCDockManager )
                            ele=( (ABCDockManager)comp ).GetXMLLayout( doc );
                        else
                            ele=ComponentSerialization( doc , comp );

                        if ( ele==null )
                            continue;

                        root.AppendChild( ele );

                    }
                }
            }

            #endregion

            doc.AppendChild( root );

            return doc;

        }
        public XmlElement ComponentSerialization ( XmlDocument doc , IComponent currentCom )
        {

            if ( currentCom is ABCView&&currentCom==this.Surface.DesignerHost.RootComponent )
                ( (ABCView)currentCom ).Location=new Point( 0 , 0 );

            Type type=currentCom.GetType();

            XmlElement ele=ABCPresentHelper.Serialization( doc , currentCom , "C" );

            if ( currentCom is ABCView )
            {
                if ( currentCom==this.Surface.DesignerHost.RootComponent )
                    ele.SetAttribute( "isRoot" , "true" );
                else
                {
                    ele.SetAttribute( "ID" , ( (ABCView)currentCom ).ViewID.ToString() );
                    CompNameList.Add( currentCom.Site.Name );
                    return ele;
                }
            }

            #region Get ChildrenNode

            if ( currentCom is ABCGridControl )
            {
                ( (ABCGridControl)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is ABCGridBandedControl )
            {
                ( (ABCGridBandedControl)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is ABCPivotGridControl )
            {
                ( (ABCPivotGridControl)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is ABCTreeList )
            {
                ( (ABCTreeList)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is ABCChartBaseControl )
            {
                ( (ABCChartBaseControl)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is DevExpress.XtraBars.Docking.DockPanel )
            {
                ABCDockPanel.GetChildrenXMLLayout( this , (DevExpress.XtraBars.Docking.DockPanel)currentCom , ele );
            }
            else if ( currentCom is ABCBindingBaseEdit )
            {
                ( (ABCBindingBaseEdit)currentCom ).GetChildrenXMLLayout( ele );
            }
            else if ( currentCom is ABCSearchPanel )
            {
                ( (ABCSearchPanel)currentCom ).GetChildrenXMLLayout( this , ele );
            }
            //else if ( currentCom is ABCDataPanel )
            //{
            //    ( (ABCDataPanel)currentCom ).GetChildrenXMLLayout( this , ele );
            //}
            else
            {
                #region Default Component
                if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                {
                    ComponentDesigner designer=(ComponentDesigner)this.Surface.DesignerHost.GetDesigner( currentCom );
                    if ( designer!=null&&designer.AssociatedComponents!=null )
                    {
                        List<XmlElement> lstTemp=new List<XmlElement>();
                        foreach ( object associatedComponent in designer.AssociatedComponents )
                        {
                            XmlElement eleChild=ComponentSerialization( doc , (IComponent)associatedComponent );
                            if ( eleChild!=null )
                                ele.AppendChild( eleChild );
                        }
                    }
                }
                else
                {
                    List<XmlElement> lstTemp=new List<XmlElement>();
                    foreach ( object associatedComponent in ( currentCom as Control ).Controls )
                    {
                        XmlElement eleChild=ComponentSerialization( doc , (IComponent)associatedComponent );
                        if ( eleChild!=null )
                            ele.AppendChild( eleChild );
                    }
                }
                #endregion
            }

            #endregion

            if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                CompNameList.Add( currentCom.Site.Name );
            else
                CompNameList.Add( ( currentCom as Control ).Name );
            return ele;
        }
        #endregion

        public static XmlDocument GetEmptyXMLLayout ( String strViewName )
        {
            XmlDocument doc=new XmlDocument();
            XmlDeclaration dec=doc.CreateXmlDeclaration( "1.0" , "utf-16" , null );
            doc.AppendChild( dec );

            XmlElement root=doc.CreateElement( "root" );

            XmlElement ele=doc.CreateElement( "C" );
            ele.SetAttribute( "name" , strViewName );
            ele.SetAttribute( "type" , typeof( ABCControls.ABCView ).Name );
            ele.SetAttribute( "isRoot" , "true" );
            root.AppendChild( ele );

            doc.AppendChild( root );
            return doc;
        }

        #endregion

        #region Utils

        public String GetPropertyBindingName ( )
        {
            return "";
        }

        public void RefreshLayout ( )
        {
            if ( this.Surface==null )
                return;

            ABCHelper.ABCWaitingDialog.Show( "" , "Refresh . . .!" );

            this.Surface.OwnerHostControl.Visible=false;
            this.Surface.OwnerHostControl.SuspendLayout();

            if ( DataField!=null )
            {
                if ( ( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).DesignerHost.RootComponent==this )
                    ( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).CleanSurface();

                this.CleanView();
                DataField=(STViewsInfo)new STViewsController().GetObjectByID( DataField.STViewID );
                if ( DataField!=null )
                    Load( DataField , Mode );

                this.Invalidate();
            }
            else if ( String.IsNullOrWhiteSpace( XMLFileName )==false )
            {
                if ( ( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).DesignerHost.RootComponent==this )
                    ( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).CleanSurface();
                this.CleanView();
                Load( XMLFileName , Mode );
                this.Invalidate();
            }

            this.Surface.OwnerHostControl.ResumeLayout( false );
            Application.DoEvents();
            this.Surface.OwnerHostControl.Visible=true;

            ABCHelper.ABCWaitingDialog.Close();
        }
        public void CleanView ( )
        {
            this.Controls.Clear();
        }

        public bool CheckLoopSurface ( Guid iSourceViewID )
        {
            if ( this.ViewID==null || this.ViewID==Guid.Empty )
                return true;

            if ( this.ViewID==iSourceViewID )
                return false;

            STViewsInfo objInfo=(STViewsInfo)new STViewsController().GetObjectByID( iSourceViewID );
            if ( objInfo==null )
                return false;

            XmlDocument doc=new XmlDocument();
            string strDecompress=ABCHelper.StringCompressor.DecompressString( objInfo.STViewXML );
            if ( String.IsNullOrWhiteSpace( strDecompress ) )
                return false;

            doc.LoadXml( strDecompress );

            XmlNodeList node=doc.SelectNodes( String.Format( "//C[@ID='{0}']" , this.ViewID.ToString() ) );
            if ( node!=null&&node.Count>0 )
                return false;

            return true;
        }

        #endregion

        #endregion

        #region MenuBar

        public enum ScreenBarButton
        {
            New ,
            Edit ,
            Delete ,
            Duplicate ,
            Save ,
            Cancel ,
            Post ,
            UnPost,
            Approve ,
            Reject,
            Lock ,
            UnLock,
            Refresh ,
            Search ,
            Studio ,
            Print,
            Info,
            Utilities,
            NewFromRelation
        }


        #region Properties

        [Category( "ABC.Toolbar" )]
        public Boolean ShowToolbar
        {
            get { return mainBar.Visible; }
            set
            {
                mainBar.Visible=value;
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowCustomItem
        {
            get { return StudioItem.Visibility==BarItemVisibility.Always; }
            set
            {
                if ( value )
                    StudioItem.Visibility=BarItemVisibility.Always;
                else
                    StudioItem.Visibility=BarItemVisibility.Never;
            }
        }

        public Boolean ShowSaveItem
        {
            get { return SaveItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Save , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowNewItem
        {
            get { return NewItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.New , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowEditItem
        {
            get { return EditItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Edit , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowDuplicateItem
        {
            get { return DuplicateItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Duplicate , value );
            }
        }

        [Category( "ABC.Toolbar" )]
        public Boolean ShowDeleteItem
        {
            get { return DeleteItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Delete , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowRefreshItem
        {
            get { return RefreshItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Refresh , value );
            }
        }
     
        [Category( "ABC.Toolbar" )]
        public Boolean ShowPostItem
        {
            get { return PostItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Post , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowApproveItem
        {
            get { return ApproveItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Approve , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowLockItem
        {
            get { return LockItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Lock , value );
            }
        }

        public Boolean ShowCancelItem
        {
            get { return CancelItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Cancel , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowSearchItem
        {
            get { return SearchItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Search , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowPrintItem
        {
            get { return PrintItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Print , value );
            }
        }
        [Category( "ABC.Toolbar" )]
        public Boolean ShowInfoItem
        {
            get { return InfoItem.Visibility==BarItemVisibility.Always; }
            set
            {
                SetToolBarButtonVisibility( ScreenBarButton.Info , value );
            }
        }

        #endregion

        DevExpress.XtraBars.BarManager ToolbarManager;
        DevExpress.XtraBars.Bar mainBar;
        DevExpress.XtraBars.BarButtonItem SaveItem;
        DevExpress.XtraBars.BarButtonItem CancelItem;
        DevExpress.XtraBars.BarButtonItem NewItem;
        DevExpress.XtraBars.BarButtonItem EditItem;
        DevExpress.XtraBars.BarButtonItem DuplicateItem;
        DevExpress.XtraBars.BarButtonItem DeleteItem;
        DevExpress.XtraBars.BarButtonItem RefreshItem;
        DevExpress.XtraBars.BarButtonItem PostItem;
        DevExpress.XtraBars.BarButtonItem UnPostItem;
        DevExpress.XtraBars.BarButtonItem ApproveItem;
        DevExpress.XtraBars.BarButtonItem RejectItem;
        DevExpress.XtraBars.BarButtonItem LockItem;
        DevExpress.XtraBars.BarButtonItem UnLockItem;
        DevExpress.XtraBars.BarButtonItem SearchItem;
        DevExpress.XtraBars.BarButtonItem StudioItem;
        DevExpress.XtraBars.BarButtonItem PrintItem;
        DevExpress.XtraBars.BarButtonItem InfoItem;
        DevExpress.XtraBars.BarSubItem UtilitiesSubItem;

        private void InitMenuBar ( )
        {
            mainBar=new DevExpress.XtraBars.Bar();
            mainBar.BarName="Main menu";
            mainBar.DockCol=0;
            mainBar.DockRow=0;
            mainBar.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            mainBar.OptionsBar.AllowQuickCustomization=false;
            mainBar.OptionsBar.DrawDragBorder=false;
            mainBar.OptionsBar.MultiLine=true;
            mainBar.OptionsBar.UseWholeRow=true;
            mainBar.Text="Main menu";

            ToolbarManager=new DevExpress.XtraBars.BarManager();
            ToolbarManager.Bars.AddRange( new DevExpress.XtraBars.Bar[] { mainBar } );
            ToolbarManager.Form=this;
            ToolbarManager.MainMenu=mainBar;
            ToolbarManager.MaxItemId=5;
            ToolbarManager.Images=ABCImageList.List24x24;
            ToolbarManager.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Toolbar_ItemClick );

            String strCaption="Save";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Lưu";
            SaveItem=AddNewToolbarButton( strCaption , ScreenBarButton.Save , "SaveItem" , 51 );

            strCaption="New";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Thêm";
            NewItem=AddNewToolbarButton( strCaption , ScreenBarButton.New , "NewItem" , 46 );
     
            strCaption="Edit";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Sửa";
            EditItem=AddNewToolbarButton( strCaption , ScreenBarButton.Edit , "EditItem" , 47 );

          
            strCaption="Delete";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Xóa";
            DeleteItem=AddNewToolbarButton( strCaption , ScreenBarButton.Delete , "DeleteItem" , 48 );
        
            strCaption="Cancel";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Tạm ngưng";
            CancelItem=AddNewToolbarButton( strCaption , ScreenBarButton.Cancel , "CancelItem" , 53 );

            strCaption="Duplicate";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="x2";
            DuplicateItem=AddNewToolbarButton( strCaption , ScreenBarButton.Duplicate , "DuplicateItem" , 56 );

         
            strCaption="Approve";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Duyệt";
            ApproveItem=AddNewToolbarButton( strCaption , ScreenBarButton.Approve , "ApproveItem" , 55 );

            strCaption="Reject";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Giữ lại";
            RejectItem=AddNewToolbarButton( strCaption , ScreenBarButton.Reject , "RejectItem" , 58 );

            strCaption="Lock";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="";
            LockItem=AddNewToolbarButton( strCaption , ScreenBarButton.Lock , "LockItem" , 59 );

            strCaption="UnLock";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="";
            UnLockItem=AddNewToolbarButton( strCaption , ScreenBarButton.UnLock , "UnLockItem" , 60 );

            strCaption="Post";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Ghi Sổ";
            PostItem=AddNewToolbarButton( strCaption , ScreenBarButton.Post , "PostItem" , 16 );
          
            strCaption="UnPost";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="Hủy Ghi Sổ";
            UnPostItem=AddNewToolbarButton( strCaption , ScreenBarButton.UnPost , "UnPostItem" , 10 );
           
            strCaption="Refresh";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="";
            RefreshItem=AddNewToolbarButton( strCaption , ScreenBarButton.Refresh , "RefreshItem" , 54 );

            strCaption="Search";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="";
            SearchItem=AddNewToolbarButton( strCaption , ScreenBarButton.Search , "SearchItem" , 25 );

            strCaption="Print";
            if ( ABCApp.ABCDataGlobal.Language=="VN" )
                strCaption="";
            PrintItem=AddNewToolbarButton( strCaption , ScreenBarButton.Print , "PrintItem" , 64 );
            PrintItem.Visibility=BarItemVisibility.Never;

            InfoItem=AddNewToolbarButton( "" , ScreenBarButton.Info , "InfoItem" , 61 );
            InfoItem.Visibility=BarItemVisibility.Never;

           UtilitiesSubItem= AddNewParentToolbarButton( "Tiện ích" , ScreenBarButton.Utilities , "UtilitiesItem" , 26 );
           UtilitiesSubItem.Visibility=BarItemVisibility.Never;

           strCaption="Studio";
           if ( ABCApp.ABCDataGlobal.Language=="VN" )
               strCaption="Studio";
           StudioItem=AddNewToolbarButton( "UtilitiesItem" , strCaption , ScreenBarButton.Studio , "StudioItem" , 26 );

        }

        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag )
        {
            return AddNewToolbarButton( strCaption , tag , "" , 0 );
        }
        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            BarButtonItem barItem=AddNewToolbarButton( strCaption , tag , strName , -1 );
            barItem.Glyph=img;
            return barItem;
        }
        public BarButtonItem AddNewToolbarButton ( String strCaption , object tag , String strName , int iImageIndex )
        {
            BarButtonItem barItem=new BarButtonItem();

            barItem.Caption=strCaption;
            barItem.Name=strName;
            barItem.Tag=tag;
            barItem.ImageIndex=iImageIndex;
            barItem.PaintStyle=BarItemPaintStyle.CaptionGlyph;

            ToolbarManager.Items.Add( barItem );
            mainBar.AddItem( barItem );
            mainBar.LinksPersistInfo.Add( new LinkPersistInfo( DevExpress.XtraBars.BarLinkUserDefines.PaintStyle , barItem , DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph ) );

            return barItem;
        }

        #region WithParent

        #region ParentToolbarButton
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag )
        {
            return AddNewParentToolbarButton( strCaption , tag , "" , 0 );
        }
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            BarSubItem barItem=AddNewParentToolbarButton( strCaption , tag , strName , -1 );
            barItem.Glyph=img;
            return barItem;
        }
        public BarSubItem AddNewParentToolbarButton ( String strCaption , object tag , String strName , int iImageIndex )
        {
            BarSubItem parentItem=new BarSubItem();
            parentItem.Caption=strCaption;
            parentItem.Name=strName;
            parentItem.Tag=tag;
            parentItem.ImageIndex=iImageIndex;
            parentItem.PaintStyle=BarItemPaintStyle.CaptionGlyph;

            ToolbarManager.Items.Add( parentItem );
            mainBar.AddItem( parentItem );
            mainBar.LinksPersistInfo.Add( new LinkPersistInfo( DevExpress.XtraBars.BarLinkUserDefines.PaintStyle , parentItem , DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph ) );

            return parentItem;
        }
        #endregion

        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag )
        {
            return AddNewToolbarButton( strParentName , strCaption , tag , "" , 0 );
        }
        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag , String strName , int iImageIndex )
        {
            if ( ToolbarManager.Items[strParentName]!=null&&ToolbarManager.Items[strParentName] is BarSubItem )
            {
                BarSubItem parentItem=ToolbarManager.Items[strParentName] as BarSubItem;
                return AddNewToolbarButton( parentItem , strCaption , tag , strName , iImageIndex );
            }

            return null;
        }
        public BarButtonItem AddNewToolbarButton ( String strParentName , String strCaption , object tag , String strName , System.Drawing.Image img )
        {
            if ( ToolbarManager.Items[strParentName]!=null&&ToolbarManager.Items[strParentName] is BarSubItem )
            {
                BarButtonItem barItem=AddNewToolbarButton( strParentName , strCaption , tag , strName , -1 );
                barItem.Glyph=img;
                return barItem;
            }
            return null;
        }
        public BarButtonItem AddNewToolbarButton ( BarSubItem parentItem , String strCaption , object tag , String strName , int iImageIndex )
        {
            BarButtonItem barItem=new BarButtonItem();

            barItem.Caption=strCaption;
            barItem.Name=strName;
            barItem.Tag=tag;
            barItem.ImageIndex=iImageIndex;
            barItem.PaintStyle=BarItemPaintStyle.CaptionGlyph;

            ToolbarManager.Items.Add( barItem );
            parentItem.AddItem( barItem );
            parentItem.LinksPersistInfo.Add( new LinkPersistInfo( DevExpress.XtraBars.BarLinkUserDefines.PaintStyle , barItem , DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph ) );

            return barItem;
        }
        #endregion

        public bool IsToolBarButtonVisibility ( object tag )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
            {
                if ( barItem.Tag!=null&&( barItem.Tag==tag||barItem.Tag.ToString()==tag.ToString() ) )
                      return  barItem.Visibility==BarItemVisibility.Always;
            }

            return false;
        }
        public void HiddenAllToolBarButtons ( )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
                barItem.Visibility=BarItemVisibility.Never;
        }
        public void SetToolBarButtonVisibility ( object tag , bool isVisible )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
            {
                if ( barItem.Tag!=null&&( barItem.Tag==tag||barItem.Tag.ToString()==tag.ToString() ) )
                {
                    if ( isVisible )
                    {
                        barItem.Visibility=BarItemVisibility.Always;
                    }
                    else
                        barItem.Visibility=BarItemVisibility.Never;

                    return;
                }
            }
        }
        public void SetToolBarButtonEnable ( object tag , bool isEnable )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
            {
                if ( barItem.Tag==tag )
                {
                    barItem.Enabled=isEnable;
                    return;
                }
            }
        }

        public void SetToolBarButtonCaption ( object tag , String strCaption )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
            {
                if ( barItem.Tag==tag )
                {
                    barItem.Caption=strCaption;
                    return;
                }
            }
        }
        public void SetToolBarButtonImageIndex ( object tag , int iImageIndex )
        {
            foreach ( BarItem barItem in ToolbarManager.Items )
            {
                if ( barItem.Tag==tag )
                {
                    barItem.ImageIndex=iImageIndex;
                    return;
                }
            }
        }
        void Toolbar_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            OnToolbarClick( sender , e );
        }

        #region Events
        public delegate void ABCToobarClickEventHandler ( object sender , DevExpress.XtraBars.ItemClickEventArgs e );
        public event ABCToobarClickEventHandler ToolbarClickEvent;

        public virtual void OnToolbarClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( this.ToolbarClickEvent!=null )
                this.ToolbarClickEvent( sender , e );
        }

        #endregion

        #endregion

        public Control this[String strControlName]
        {
            get { return GetControl( strControlName ); }
        }
        public Control GetControl ( String strControlName )
        {
            Control[] controls=this.Controls.Find( strControlName , true );
            if ( controls.Length>0 )
                return controls[0];
            return null;
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                foreach ( ABCView child in ChildrenView )
                    child.Dispose();
            }
            base.Dispose( disposing );
        }
    }

    public class ABCViewDesigner : System.Windows.Forms.Design.ScrollableControlDesigner 
    {
        
        public ABCViewDesigner ( )
        {
        }
    }
}
