using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraTreeList.Columns;

using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;

using ABCBusinessEntities;
using ABCCommon;
using ABCProvider;

namespace ABCControls
{

  
    public class ABCTreeListColumn : DevExpress.XtraTreeList.Columns.TreeListColumn
    {
        [Serializable]
        public class ColumnConfig
        {
            public String Caption { get; set; }
            public int Width { get; set; }
            public bool Visible { get; set; }

            public ABCRepositoryType RepoType { get; set; }
            public int VisibleIndex { get; set; }
            public DevExpress.XtraTreeList.Columns.FixedStyle Fixed { get; set; }
            public ABCSummaryType SumType { get; set; }
            public int ImageIndex { get; set; }
            public bool AllowEdit { get; set; }

            public ColumnConfig ( )
            {
                RepoType=ABCRepositoryType.None;
                ImageIndex=-1;
                Width=50;
                Visible=true;
                VisibleIndex=0;
                Fixed=DevExpress.XtraTreeList.Columns.FixedStyle.None;
                SumType=ABCSummaryType.None;
                AllowEdit=false;
            }

            public object Clone ( )
            {
                return  (ColumnConfig)this.MemberwiseClone();
            }
        }

        public ColumnConfig Config;

        public ABCTreeListColumn ( )
        {
        }
        public ABCTreeListColumn ( ABCTreeListColumn.ColumnConfig colInfo )
        {
            Config=colInfo;

            this.Caption=colInfo.Caption;
            this.VisibleIndex=colInfo.VisibleIndex;
            this.Visible=colInfo.Visible;
            this.Width=colInfo.Width;
            this.ImageIndex=colInfo.ImageIndex;
            this.Fixed=colInfo.Fixed;
            this.OptionsColumn.AllowEdit=colInfo.AllowEdit;
        }

        public void InitRepository ( )
        {
            if ( Config.RepoType==ABCRepositoryType.None )
            {
                //if ( String.IsNullOrWhiteSpace( this.Config.FieldName )==false&&this.Config.FieldName.Contains( ":" ) )
                //{
                    this.OptionsColumn.AllowEdit=false;
                //}
                //else
                //{
                    //String strPKTableName=String.Empty;
                    //if ( DataStructureProvider.IsForeignKey( this.TableName , this.FieldName ) )
                    //    strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , this.FieldName );
                  
                    //if ( String.IsNullOrWhiteSpace( strPKTableName )==false )
                    //{
                    //    this.ColumnEdit=ABCControls.UICaching.GetDefaultRepositoryGridLookupEdit( strPKTableName );
                    //    ( this.ColumnEdit as ABCRepositoryGridLookupEdit ).DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );
                    //}
              //  }
            }
        }

    }

    [ToolboxBitmapAttribute( typeof( DevExpress.XtraTreeList.TreeList ) )]
    [Designer( typeof( ABCTreeListDesigner ) )]
    public partial class ABCTreeList : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl , IABCCustomControl
    {
        public ABCTreelistManager Manager;

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [Browsable(false)]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "TreeListDataSource";
            }
        }
        #endregion

        #region IABCTreeList

        public ABCView OwnerView { get; set; }

        [Browsable( false )]
        public BindingSource TreeListDataSource
        {
            get
            {
                return bindingSource;
            }
            set
            {
                bindingSource=value;
                if(bindingSource!=null)
                bindingSource.ListChanged+=new ListChangedEventHandler( bindingSource_ListChanged );
            }
        }

        void bindingSource_ListChanged ( object sender , ListChangedEventArgs e )
        {
            if ( e.ListChangedType==ListChangedType.Reset )
                this.Manager.Invalidate( bindingSource.List );
        }
        private BindingSource bindingSource;
        #endregion

        #region TreeList- Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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
                if ( OwnerView!=null && OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

        bool drawBlueEmptyArea=false;
        [Category( "Options" )]
        public bool DrawBlueEmptyArea
        {
            get { return drawBlueEmptyArea; }
            set
            {
                drawBlueEmptyArea=value;
                if ( drawBlueEmptyArea )
                {
                    this.InnerTreeList.Appearance.Empty.BackColor=System.Drawing.Color.FromArgb( 181 , 200 , 223 );
                    this.InnerTreeList.Appearance.Empty.Options.UseBackColor=true;
                }
                else
                    this.InnerTreeList.Appearance.Empty.Options.UseBackColor=false;
            }
        }

        #region Options

        [Category( "Options" )]
        public bool MultiSelect { get { return InnerTreeList.OptionsSelection.MultiSelect; } set { InnerTreeList.OptionsSelection.MultiSelect=value; } }
      
        [Category( "Options" )]
        public bool DrawColorEvenRow { get { return InnerTreeList.OptionsView.EnableAppearanceEvenRow; } set { InnerTreeList.OptionsView.EnableAppearanceEvenRow=value; } }
        [Category( "Options" )]
        public bool DrawColorOddRow { get { return InnerTreeList.OptionsView.EnableAppearanceOddRow; } set { InnerTreeList.OptionsView.EnableAppearanceOddRow=value; } }
   
        [Category( "Options" )]
        public bool EnableFocusedCell { get { return InnerTreeList.OptionsSelection.EnableAppearanceFocusedCell; } set { InnerTreeList.OptionsSelection.EnableAppearanceFocusedCell=value; } }
        [Category( "Options" )]
        public bool EnableFocusedRow { get { return InnerTreeList.OptionsSelection.EnableAppearanceFocusedRow; } set { InnerTreeList.OptionsSelection.EnableAppearanceFocusedRow=value; } }

        [Category( "Options" )]
        public bool ShowAutoFilterRow { get { return InnerTreeList.OptionsView.ShowAutoFilterRow; } set { InnerTreeList.OptionsView.ShowAutoFilterRow=value; } }
        [Category( "Options" )]
        public bool ShowColumnHeaders { get { return InnerTreeList.OptionsView.ShowColumns; } set { InnerTreeList.OptionsView.ShowColumns=value; } }
        [Category( "Options" )]
        public DevExpress.XtraTreeList.ShowFilterPanelMode ShowFilterPanelMode { get { return InnerTreeList.OptionsView.ShowFilterPanelMode; } set { InnerTreeList.OptionsView.ShowFilterPanelMode=value; } }
        [Category( "Options" )]
        public bool ShowSummaryFooter { get { return InnerTreeList.OptionsView.ShowSummaryFooter; } set { InnerTreeList.OptionsView.ShowSummaryFooter=value; } }
        [Category( "Options" )]
        public bool ShowRowFooterSummary { get { return InnerTreeList.OptionsView.ShowRowFooterSummary; } set { InnerTreeList.OptionsView.ShowRowFooterSummary=value; } }
        [Category( "Options" )]
        public bool ShowHorzLines { get { return InnerTreeList.OptionsView.ShowHorzLines; } set { InnerTreeList.OptionsView.ShowHorzLines=value; } }
        [Category( "Options" )]
        public bool ShowVertLines { get { return InnerTreeList.OptionsView.ShowVertLines; } set { InnerTreeList.OptionsView.ShowVertLines=value; } }
        [Category( "Options" )]
        public bool ShowIndicator { get { return InnerTreeList.OptionsView.ShowIndicator; } set { InnerTreeList.OptionsView.ShowIndicator=value; } }
        [Category( "Options" )]
        public bool ShowPreview { get { return InnerTreeList.OptionsView.ShowPreview; } set { InnerTreeList.OptionsView.ShowPreview=value; } }
        [Category( "Options" )]
        public string PreviewFieldName { get { return InnerTreeList.PreviewFieldName; } set { InnerTreeList.PreviewFieldName=value; } }
        #endregion

        #region MenuBar
        [Category( "MenuBar" )]
        public Boolean ShowMenuBar { get { return mainBar.Visible; } set { mainBar.Visible=value; } }
        [Category( "MenuBar" )]
        public Boolean ShowSaveButton
        {
            get { return SaveItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    SaveItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    SaveItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowDeleteButton
        {
            get { return DeleteItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    DeleteItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    DeleteItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowRefreshButton
        {
            get { return RefreshItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    RefreshItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    RefreshItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowFilterButton
        {
            get { return FilterItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    FilterItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    FilterItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowExportButton
        {
            get { return ExportItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    ExportItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    ExportItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowPrintButton
        {
            get { return PrintItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    PrintItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    PrintItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        [Category( "MenuBar" )]
        public Boolean ShowColumnChooser
        {
            get { return ColumnChooserItem.Visibility==DevExpress.XtraBars.BarItemVisibility.Always; }
            set
            {
                if ( value )
                    ColumnChooserItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                else
                    ColumnChooserItem.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        #endregion

        [Category( "Columns" )]
        [Editor( typeof( TreeColumnsEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "Columns" ) , Description( "Which type to use..." )]
        public BindingList<ABCTreeListColumn.ColumnConfig> ColumnConfigs
        {
            get
            {
                return this.columnConfigs;
            }
            set
            {
                this.columnConfigs=value;
           //     this.InitColumns();
            }
        }
        private BindingList<ABCTreeListColumn.ColumnConfig> columnConfigs;

        #endregion


        public ABCTreeList ( )
        {
            InitializeComponent();
            InitMenuBar();

            Manager=new ABCTreelistManager( this );

            DrawBlueEmptyArea=true;
            this.InnerTreeList.CustomDrawNodeCell+=new CustomDrawNodeCellEventHandler( InnerTreeList_CustomDrawNodeCell );
            this.InnerTreeList.OptionsView.AutoWidth=false;
         
            this.InnerTreeList.CustomNodeCellEdit+=new GetCustomNodeCellEditEventHandler( InnerTreeList_CustomNodeCellEdit );
            this.InnerTreeList.CustomDrawNodeCell+=new CustomDrawNodeCellEventHandler( InnerTreeList_CustomDrawNodeCell );
            this.InnerTreeList.MouseUp+=new MouseEventHandler( InnerTreeList_MouseUp );
        }
        void ABCTreeList_Disposed ( object sender , EventArgs e )
        {
            //if ( RowDetail!=null )
            //{
            //    if ( RowDetail.Visible )
            //        RowDetail.Close();

            //    RowDetail.Dispose();
            //}
        }

        #region Cell
        public void RunFocusedLink ( )
        {
            try
            {
                if ( this.OwnerView!=null&&this.OwnerView.Mode==ViewMode.Design )
                    return;

                if ( Manager==null )
                    return;

                ABCTreeListNode obj=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( InnerTreeList.FocusedNode );
                if ( obj==null||(BusinessObject)obj.InnerData==null )
                    return;


                TreeConfigNode config=null;
                if ( Manager.ConfigList.TryGetValue( obj.ObjectName , out config )==false )
                    return;
                if ( config==null )
                    return;

                String strField=String.Empty;
                if ( config.InnerData.ColumnFieldNames.TryGetValue( InnerTreeList.FocusedColumn.Caption , out strField )==false )
                    return;
                if ( String.IsNullOrWhiteSpace( strField ) )
                    return;

                object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( (BusinessObject)obj.InnerData , strField.Split( ':' )[0] );
                if ( objValue is int==false )
                    return;

                String strLinkTableName=String.Empty;
                Guid iLinkID=Guid.Empty;

                if ( strField.Contains( ":" ) )
                {

                    DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( config.InnerData.TableName , ABCHelper.DataConverter.ConvertToGuid( objValue ) , strField );
                    if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                        ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                    {
                        strLinkTableName=acrrosInfo.TableName;
                        iLinkID=acrrosInfo.TableID;
                    }
                }
                else
                {
                    if ( DataStructureProvider.IsForeignKey( config.InnerData.TableName , strField ) )
                    {
                        strLinkTableName=DataStructureProvider.GetTableNameOfForeignKey( config.InnerData.TableName , strField );
                        iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                    }
                }

                if ( iLinkID!=Guid.Empty)
                {
                    if ( this.OwnerView!=null )
                         ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                    else
                         ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );
                }
            }
            catch ( Exception ex )
            {
            }

        }

        public void RunFocusedLink_Click ( )
        {
            Point pt=InnerTreeList.PointToClient( MousePosition );
            TreeListHitInfo info=InnerTreeList.CalcHitInfo( pt );
            if ( info.Node==null||info.HitInfoType!=HitInfoType.Cell )
                return;

            if ( 0<info.MousePoint.X-info.Bounds.Left&&info.MousePoint.X-info.Bounds.Left<18 )
            {
                RunFocusedLink();
            }
        }

        #region DrawButton

        void InnerTreeList_CustomDrawNodeCell ( object sender , CustomDrawNodeCellEventArgs e )
        {
            e.Appearance.BackColor=System.Drawing.Color.FromArgb( 181 , 200 , 223 );
            e.Appearance.Options.UseBackColor=true;

            if ( e.CellValue==null||e.CellValue==DBNull.Value||String.IsNullOrWhiteSpace( e.CellText ) )
                return;

            if ( e.CellText=="0"||e.CellValue.ToString()=="0" )
            {
                e.CellText="";
                return;
            }

            if ( ( e.Column is ABCTreeListColumn==false )||e.EditViewInfo is DevExpress.XtraEditors.ViewInfo.ProgressBarViewInfo )
                return;

            ABCTreeListNode obj=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( e.Node );
            if ( obj==null||(BusinessObject)obj.InnerData==null )
                return;

            TreeConfigNode config=null;
            if ( Manager.ConfigList.TryGetValue( obj.ObjectName , out config )==false )
                return;

            if ( config.InnerData.Level==null || config.InnerData.Level<=0 )
            {
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
                e.Appearance.Options.UseFont=true;
            }
            String strField=String.Empty;
            if ( config.InnerData.ColumnFieldNames.TryGetValue( e.Column.Caption , out strField )==false )
                return;

            if ( String.IsNullOrWhiteSpace( strField ) )
                return;

            e.CellText=DataFormatProvider.DoFormat( e.CellValue , config.InnerData.TableName , strField );

            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( config.InnerData.TableName , strField );
            if ( formatInfo!=null )
            {
                if ( formatInfo.FormatInfo.FormatType==FormatType.Numeric )
                {
                    e.Appearance.TextOptions.HAlignment=HorzAlignment.Far;
                    e.Appearance.Options.UseTextOptions=true;
                }
            }
        }

        Dictionary<String , bool> ForeignColumnList=new Dictionary<string , bool>();
        private void DrawLinkButton ( CustomDrawNodeCellEventArgs e )
        {
            try
            {
                if ( this.OwnerView!=null&&this.OwnerView.Mode==ViewMode.Design )
                    return;

                if ( Manager==null )
                    return;

                ABCTreeListNode obj=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( e.Node );
                if ( obj==null||(BusinessObject)obj.InnerData==null )
                    return;

                TreeConfigNode config=null;
                if ( Manager.ConfigList.TryGetValue( obj.ObjectName , out config )==false )
                    return;


                Boolean isDraw=false;
                if ( ForeignColumnList.TryGetValue( e.Column.Caption+obj.ObjectName , out isDraw ) )
                {
                    if ( isDraw )
                        DrawButton( e );
                    return;
                }

                if ( ( e.Column is ABCTreeListColumn==false )||config==null )
                    return;

                String strField=String.Empty;
                if ( config.InnerData.ColumnFieldNames.TryGetValue( e.Column.Caption , out strField )==false )
                    return;

                if ( String.IsNullOrWhiteSpace( strField ) )
                    return;

                if ( strField.Contains( ":" )==false )
                {

                    if ( DataStructureProvider.IsForeignKey( obj.InnerData.AATableName , strField ) )
                    {
                        String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( obj.InnerData.AATableName , strField );
                        STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , strPKTableName ) );
                        if ( viewIfo!=null )
                            isDraw=true;
                    }
                }
                else
                {

                    DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( obj.InnerData.AATableName , strField );
                    if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                        ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                    {
                        STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , acrrosInfo.TableName ) );
                        if ( viewIfo!=null )
                            isDraw=true;
                    }
                }

                ForeignColumnList.Add( e.Column.Caption+obj.ObjectName , isDraw );

                if ( isDraw )
                    DrawButton( e );
            }
            catch ( Exception ex )
            {
            }
        }
        private void DrawButton ( CustomDrawNodeCellEventArgs e )
        {
            e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "DocLink" ) , e.Bounds.Location );

            Rectangle r=e.Bounds;
            r.Width-=18;
            r.X+=18;
            e.Appearance.DrawString( e.Cache , e.CellText , r );

            e.Handled=true;
        }

        #endregion

        DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar;
        void InnerTreeList_CustomNodeCellEdit ( object sender , GetCustomNodeCellEditEventArgs e )
        {
            try
            {
                ABCTreeListNode obj=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( e.Node );
                if ( obj!=null&&(BusinessObject)obj.InnerData!=null )
                {
                    if ( Manager==null )
                        return;

                    TreeConfigNode config=null;
                    if ( Manager.ConfigList.TryGetValue( obj.ObjectName , out config )==false )
                        return;

                    String strField=String.Empty;
                    if ( config.InnerData.ColumnFieldNames.TryGetValue( e.Column.Caption , out strField )==false )
                        return;

                    if ( String.IsNullOrWhiteSpace( strField ) )
                        return;

                    DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( config.InnerData.TableName , strField );
                    if ( formatInfo!=null )
                        if ( formatInfo.ABCFormat==DataFormatProvider.FieldFormat.Percent )
                        {
                            if ( ProgressBar==null )
                            {
                                ProgressBar=new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
                            //    ProgressBar.PercentView=true;
                                ProgressBar.ShowTitle=true;
                            }
                            e.RepositoryItem=ProgressBar;
                        }
                }
            }
            catch ( Exception ex )
            {
            }
        }
        #endregion

        void InnerTreeList_MouseUp ( object sender , MouseEventArgs e )
        {

            if ( e.Button==MouseButtons.Left&&ModifierKeys==Keys.None&&InnerTreeList.State==TreeListState.NodePressed )
                RunFocusedLink_Click();

            if ( e.Button==MouseButtons.Right )
            {
                DevExpress.XtraTreeList.TreeListHitInfo hitInfo=this.InnerTreeList.CalcHitInfo( e.Location );
                if ( hitInfo!=null )
                {
                    this.InnerTreeList.SetFocusedNode( hitInfo.Node );
                    DoShowPopupMenu( hitInfo );
                }
            }
        }

        #region MenuBar
        DevExpress.XtraBars.BarManager barManager;
        DevExpress.XtraBars.Bar mainBar;
        DevExpress.XtraBars.BarButtonItem SaveItem;
        DevExpress.XtraBars.BarButtonItem DeleteItem;
        DevExpress.XtraBars.BarButtonItem RefreshItem;
        DevExpress.XtraBars.BarButtonItem FilterItem;
        DevExpress.XtraBars.BarButtonItem ExportItem;
        DevExpress.XtraBars.BarButtonItem PrintItem;
        DevExpress.XtraBars.BarButtonItem ColumnChooserItem;
        DevExpress.XtraBars.BarButtonItem RowDetailItem;

        private void InitMenuBar ( )
        {

            SaveItem=new DevExpress.XtraBars.BarButtonItem();
            SaveItem.Caption="Save";
            SaveItem.Id=0;
            SaveItem.ImageIndex=2;
            SaveItem.Tag="Save";
            SaveItem.Name="SaveItem";

            RefreshItem=new DevExpress.XtraBars.BarButtonItem();
            RefreshItem.Caption="Refresh";
            RefreshItem.Id=1;
            RefreshItem.ImageIndex=47;
            RefreshItem.Tag="Refresh";
            RefreshItem.Name="RefreshItem";

            DeleteItem=new DevExpress.XtraBars.BarButtonItem();
            DeleteItem.Caption="Delete";
            DeleteItem.Id=2;
            DeleteItem.ImageIndex=53;
            DeleteItem.Tag="Delete";
            DeleteItem.Name="DeleteItem";

            FilterItem=new DevExpress.XtraBars.BarButtonItem();
            FilterItem.Caption="Filter";
            FilterItem.Id=3;
            FilterItem.ImageIndex=48;
            FilterItem.Tag="Filter";
            FilterItem.Name="FilterItem";

            RowDetailItem=new DevExpress.XtraBars.BarButtonItem();
            RowDetailItem.Caption="Row Detail";
            RowDetailItem.Id=3;
            RowDetailItem.ImageIndex=83;
            RowDetailItem.Tag="RowDetail";
            RowDetailItem.Name="RowDetailItem";

            
            ExportItem=new DevExpress.XtraBars.BarButtonItem();
            ExportItem.Caption="Export";
            ExportItem.Id=3;
            ExportItem.ImageIndex=79;
            ExportItem.Tag="Export";
            ExportItem.Name="ExportItem";

            PrintItem=new DevExpress.XtraBars.BarButtonItem();
            PrintItem.Caption="Print";
            PrintItem.Id=3;
            PrintItem.ImageIndex=80;
            PrintItem.Tag="Print";
            PrintItem.Name="PrintItem";

            ColumnChooserItem=new DevExpress.XtraBars.BarButtonItem();
            ColumnChooserItem.Caption="Columns";
            ColumnChooserItem.Id=3;
            ColumnChooserItem.ImageIndex=81;
            ColumnChooserItem.Tag="ColumnChooser";
            ColumnChooserItem.Name="ColumnChooserItem";


            mainBar=new DevExpress.XtraBars.Bar();
            mainBar.BarName="TreeList menu";
            mainBar.DockCol=0;
            mainBar.DockRow=0;
            mainBar.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            mainBar.OptionsBar.AllowQuickCustomization=false;
            mainBar.OptionsBar.DrawDragBorder=false;
            mainBar.OptionsBar.MultiLine=true;
            mainBar.OptionsBar.UseWholeRow=true;
            mainBar.Text="TreeList menu";

            barManager=new DevExpress.XtraBars.BarManager();
            barManager.Bars.AddRange( new DevExpress.XtraBars.Bar[] { mainBar } );
            barManager.Form=this;
            barManager.Items.AddRange( new DevExpress.XtraBars.BarItem[] { SaveItem , DeleteItem , RefreshItem , RowDetailItem,FilterItem , ColumnChooserItem , ExportItem , PrintItem } );
            barManager.MainMenu=mainBar;
            barManager.MaxItemId=5;
            barManager.Images=ABCImageList.List16x16;
            barManager.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( barManager_ItemClick );
            mainBar.AddItems( new DevExpress.XtraBars.BarItem[] { SaveItem , DeleteItem , RefreshItem ,RowDetailItem, FilterItem , ColumnChooserItem , ExportItem , PrintItem } );
            mainBar.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,SaveItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,RefreshItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
              new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,RowDetailItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,DeleteItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,PrintItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,FilterItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,ExportItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
                new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,ColumnChooserItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)} );
        }

        void barManager_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            String strTag= e.Item.Tag.ToString();

            DoMenuClick( strTag );
          
        }

        private void DoMenuClick ( String strTag )
        {
            OnBarItemClick( strTag );

            switch ( strTag )
            {
                case "Refresh":
                    OnRefresh();
                    break;

                case "Filter":
                    OnFilter();
                    break;

                case "RowDetail":
                    OnShowRowDetail();
                    break;

                case "ColumnChooser":
                    OnColumnChooser();
                    break;

                case "Export":
                    OnExport();
                    break;

                case "Print":
                    OnPrint();
                    break;
            }
        }

        #region Events

        public event ABCDefineEvents.ABCBarItemClickEventHandler BarItemClick;
        public virtual void OnBarItemClick ( String strTag)
        {
            if ( this.BarItemClick!=null )
                this.BarItemClick( this , strTag );
        }

        public virtual void OnRefresh ( )
        {
            //if ( String.IsNullOrWhiteSpace( this.DataSource )&&String.IsNullOrWhiteSpace( this.Script )==false )
            //    LoadDataSourceFromScript();
            ActionRefreshAllTree();
        }
        public virtual void OnFilter ( )
        {
            this.InnerTreeList.ShowFilterEditor( null );
        }
        public virtual void OnColumnChooser ( )
        {
            this.InnerTreeList.ColumnsCustomization();
        }
        public virtual void OnShowRowDetail ( )
        {
        //    ShowRowDetail();
        }
        public virtual void OnExport ( )
        {
            SaveFileDialog dlg=new SaveFileDialog();
            dlg.Filter="xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            dlg.FilterIndex=2;
            dlg.RestoreDirectory=true;
            if ( dlg.ShowDialog()==DialogResult.OK )
            {
                if ( dlg.FileName.EndsWith( "xlsx" ) )
                    InnerTreeList.ExportToXlsx( dlg.FileName );
                else
                    InnerTreeList.ExportToXls( dlg.FileName );
                Application.DoEvents();

                System.Diagnostics.Process.Start( dlg.FileName );
            }




        }

        #region OnPrint
        public virtual void OnPrint ( )
        {
            this.InnerTreeList.AppearancePrint.Row.BorderColor=Color.Black;
            this.InnerTreeList.AppearancePrint.HeaderPanel.BackColor=Color.FromArgb( 255 , 119 , 149 , 203 );
            this.InnerTreeList.AppearancePrint.HeaderPanel.ForeColor=Color.White;
            this.InnerTreeList.AppearancePrint.HeaderPanel.Font=new Font(ABCFontProvider.GetFontFamily( "calibri"),10 , FontStyle.Regular );
            this.InnerTreeList.AppearancePrint.Row.Options.UseBorderColor=true;
            this.InnerTreeList.AppearancePrint.HeaderPanel.Options.UseBackColor=true;
            this.InnerTreeList.AppearancePrint.HeaderPanel.Options.UseForeColor=true;
            this.InnerTreeList.AppearancePrint.HeaderPanel.Options.UseFont=true;
            this.InnerTreeList.AppearancePrint.Preview.ForeColor=Color.White;
            this.InnerTreeList.AppearancePrint.Preview.BackColor=Color.White;
            this.InnerTreeList.AppearancePrint.Preview.Options.UseBackColor=true;
            this.InnerTreeList.AppearancePrint.Preview.Options.UseForeColor=true;
            this.InnerTreeList.OptionsPrint.UsePrintStyles=false;
            this.InnerTreeList.OptionsPrint.AutoWidth=false;
   
            this.InnerTreeList.OptionsPrint.PrintPreview=false;
            this.InnerTreeList.OptionsPrint.UsePrintStyles=true;
        
            PrintingSystem printingSystem1=new PrintingSystem();
            PrintableComponentLink printableComponentLink1=new PrintableComponentLink();
            printingSystem1.Links.AddRange( new object[] { printableComponentLink1 } );
            printableComponentLink1.Margins=new System.Drawing.Printing.Margins( 50 , 50 , 40 , 50 );
            printableComponentLink1.Component=this.InnerTreeList;
            
            if ( this.InnerTreeList.Columns.Count>5 )
                printableComponentLink1.Landscape=true;
            else
                printableComponentLink1.Landscape=false;

            printableComponentLink1.PaperKind=System.Drawing.Printing.PaperKind.A4;
            printableComponentLink1.VerticalContentSplitting=VerticalContentSplitting.Smart;
   
            printableComponentLink1.CreateMarginalHeaderArea+=new CreateAreaEventHandler( printableComponentLink1_CreateMarginalHeaderArea );
            printableComponentLink1.CreateReportHeaderArea+=new CreateAreaEventHandler( printableComponentLink1_CreateReportHeaderArea );
            //      printableComponentLink1.CreateReportFooterArea+=new CreateAreaEventHandler( printableComponentLink1_CreateReportFooterArea );
            printableComponentLink1.CreateMarginalFooterArea+=new CreateAreaEventHandler( printableComponentLink1_CreateMarginalFooterArea );

            printableComponentLink1.CreateDocument();
            printableComponentLink1.PrintingSystem.Document.AutoFitToPagesWidth=1;
            printableComponentLink1.ShowRibbonPreviewDialog(this.LookAndFeel);

        }

        void printableComponentLink1_CreateMarginalHeaderArea ( object sender , CreateAreaEventArgs e )
        {
            e.Graph.Modifier=BrickModifier.MarginalHeader;
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) ,10);
            string format="In ngày : {0:dd/M/yyyy}";
            PageInfoBrick brick=e.Graph.DrawPageInfo( PageInfo.DateTime , format , Color.FromArgb( 255 , 119 , 149 , 203 ) ,
            new RectangleF( 0 , 0 , 0 , 20 ) , BorderSide.None );
            brick.Alignment=BrickAlignment.Near;
            brick.AutoWidth=true;
        }
        void printableComponentLink1_CreateReportHeaderArea ( object sender , CreateAreaEventArgs e )
        {
            //string reportHeader=this.ViewCaption;

            //e.Graph.StringFormat=new BrickStringFormat( StringAlignment.Center );
            //e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ),17 , FontStyle.Bold|FontStyle.Underline);
            //e.Graph.BackColor=Color.White;
            //RectangleF rec=new RectangleF( 0 ,20 , e.Graph.ClientPageSize.Width,50 );
            //e.Graph.DrawString( reportHeader , Color.FromArgb( 255 , 119 , 149 , 203 ) , rec , BorderSide.None );
        }

        //void printableComponentLink1_CreateReportFooterArea ( object sender , CreateAreaEventArgs e )
        //{
        //    e.Graph.Modifier=BrickModifier.ReportFooter;
        //    e.Graph.StringFormat=new BrickStringFormat( StringFormatFlags.NoWrap|StringFormatFlags.LineLimit );
        //    e.Graph.StringFormat=e.Graph.StringFormat.ChangeLineAlignment( StringAlignment.Far );
        //    e.Graph.DrawString( "Created by ABC Studio" , Color.Transparent , new Rectangle( 0 , 0 , 200 , 30 ) ,BorderSide.None );
        //}

        void printableComponentLink1_CreateMarginalFooterArea ( object sender , CreateAreaEventArgs e )
        {
            e.Graph.Modifier=BrickModifier.MarginalFooter;
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) , 10 , FontStyle.Regular );

            PageInfoBrick brick=e.Graph.DrawPageInfo( PageInfo.NumberOfTotal , "Trang {0} / {1}" , Color.FromArgb( 255 , 119 , 149 , 203 ) , new RectangleF( 0 , 0 , 0 , 20 ) , BorderSide.None );
            brick.Alignment=BrickAlignment.Far;
            brick.AutoWidth=true;

            e.Graph.StringFormat=new BrickStringFormat( StringFormatFlags.NoWrap|StringFormatFlags.LineLimit );
            e.Graph.StringFormat=e.Graph.StringFormat.ChangeLineAlignment( StringAlignment.Far );
            e.Graph.DrawString( "Created by ABC ERP" , Color.FromArgb( 255 , 119 , 149 , 203 ) , new Rectangle( 0 , 0 , 200 , 30 ) , BorderSide.None );
        }
        
        #endregion
  

        #endregion

        #endregion

        #region IABCControl
        public void InitControl ( )
        {

        }
        #endregion

        #region TreeList

        #region Layout

        #region Save
        public void GetChildrenXMLLayout ( XmlElement TreeListElement )
        {
            XmlNode nodeCol=ABCHelper.DOMXMLUtil.GetFirstNode( TreeListElement , "P" , "Collection(" );
            if ( nodeCol!=null )
                TreeListElement.RemoveChild( nodeCol );

            foreach ( ABCTreeListColumn col in this.InnerTreeList.Columns )
            {
                XmlElement ele=ABCPresentHelper.Serialization( TreeListElement.OwnerDocument , col.Config , "CL" );
                TreeListElement.AppendChild( ele );
            }

            foreach ( TreeConfigNode configNode in Manager.RootConfig.ChildrenNodes.Values )
            {
                XmlElement ele=GetConfigXMLNode( TreeListElement.OwnerDocument , configNode );
                if ( ele!=null )
                    TreeListElement.AppendChild( ele );
            }
        }

        private XmlElement GetConfigXMLNode ( XmlDocument document, TreeConfigNode currentNode )
        {
            if ( currentNode.InnerData==null )
                return null;

            XmlElement ele=ABCPresentHelper.Serialization( document , currentNode.InnerData , "DataConfig" );
            foreach ( String strKey in currentNode.InnerData.ColumnFieldNames.Keys )
            {
                XmlElement eleDisp=document.CreateElement( "DispField" );
                eleDisp.SetAttribute( "Column" , strKey );
                eleDisp.InnerText=currentNode.InnerData.ColumnFieldNames[strKey];
                ele.AppendChild( eleDisp );
            }


            foreach ( TreeConfigNode childNode in currentNode.ChildrenNodes.Values )
            {
                XmlElement eleChild=GetConfigXMLNode( document , childNode );
                if ( eleChild!=null )
                    ele.AppendChild( eleChild );
            }

            return ele;
        }
        #endregion

        #region Load
        public void InitLayout ( ABCView view , XmlNode TreeListNode )
        {
            OwnerView=view;
            this.ColumnConfigs=new BindingList<ABCTreeListColumn.ColumnConfig>();

            foreach ( XmlNode nodeCol in TreeListNode.SelectNodes( "CL" ) )
            {
                ABCTreeListColumn.ColumnConfig colInfo=new ABCTreeListColumn.ColumnConfig();
                ABCPresentHelper.DeSerialization( colInfo , nodeCol );
                this.ColumnConfigs.Add( colInfo );
            }

            this.Manager.ConfigList.Clear();
            this.Manager.RootConfig=new TreeConfigNode();

            foreach ( XmlNode configXmlNode in TreeListNode.SelectNodes( "DataConfig" ) )
                GetDataConfigFromXML( this.Manager.RootConfig , configXmlNode );

            this.InitColumns();
            LoadDataSourceFromScript();
        }
        private TreeConfigNode GetDataConfigFromXML( TreeConfigNode parent, XmlNode xmlNode )
        {
            TreeConfigData configData=new TreeConfigData();
            ABCPresentHelper.DeSerialization( configData , xmlNode );
            configData.ColumnFieldNames=new Dictionary<string , string>();

            foreach ( XmlNode nodeChild in xmlNode.SelectNodes( "DispField" ) )
            {
                String strKey=nodeChild.Attributes["Column"].Value.ToString();
                String strField=nodeChild.InnerText;
                if ( String.IsNullOrWhiteSpace( strKey )==false )
                    configData.ColumnFieldNames.Add( strKey , strField );
            }

            TreeConfigNode configNode=new TreeConfigNode( parent , configData );
            if ( this.Manager.ConfigList.ContainsKey( configData.Name )==false )
                this.Manager.ConfigList.Add( configData.Name , configNode );
            else
                this.Manager.ConfigList[configData.Name]=configNode;

            foreach ( XmlNode child in xmlNode.SelectNodes( "DataConfig" ) )
                GetDataConfigFromXML( configNode , child );

            return configNode;
        }
       
     
        #endregion

        //first time
        public void Initialize ( ABCView view , ABCBindingInfo bindingInfo )
        {
            //  String strTableName= view.DataConfig.BindingList[bindingInfo.BusName].TableName;
            String strTableName=bindingInfo.TableName;
            if ( DataConfigProvider.TableConfigList.ContainsKey( strTableName )==false )
                return;

            OwnerView=view;

            this.DataSource=bindingInfo.BusName;
            this.TableName=strTableName;

            //#region InitColumns

            //#region BindingList<ABCTreeListColumn.ColumnInfo>
            //this.DefaultView.ColumnConfigs=new BindingList<ABCTreeListColumn.ColumnConfig>();

            //int iIndex=0;
            //foreach ( DataConfigProvider.FieldConfig colAlias in DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values )
            //{
            //    iIndex++;

            //    ABCTreeListColumn.ColumnConfig colInfo=new ABCTreeListColumn.ColumnConfig();
            //    colInfo.FieldName=colAlias.FieldName;
            //    colInfo.TableName=this.TableName;
            //    colInfo.VisibleIndex=iIndex;

            //    if ( colAlias.IsDefault )
            //    {
            //        colInfo.Visible=true;
            //        colInfo.VisibleIndex=iIndex;
            //    }
            //    else
            //    {
            //        colInfo.Visible=false;
            //        colInfo.VisibleIndex=-1;
            //    }

            //    if ( ABCApp.ABCDataGlobal.Language=="VN" )
            //        colInfo.Caption=colAlias.CaptionVN;
            //    else
            //        colInfo.Caption=colAlias.CaptionEN;

            //    if ( String.IsNullOrWhiteSpace( colInfo.Caption )==false )
            //         this.DefaultView.ColumnConfigs.Add( colInfo );

            //}

            //#endregion

            //this.DefaultView.InitColumns();

            //#endregion
        

        }

        public void Initialize ( String strTableName )
        {
            ABCBindingInfo bindIfo=new ABCBindingInfo();
            bindIfo.TableName=strTableName;
         //   this.DefaultView.TableName=strTableName;
            Initialize( null , bindIfo );
        }

        public void InitColumns ( )
        {

            this.InnerTreeList.Columns.Clear();
            if ( this.ColumnConfigs==null )
                return;

            SortedList<String , ABCTreeListColumn.ColumnConfig> lstSort=new SortedList<string , ABCTreeListColumn.ColumnConfig>();
            foreach ( ABCTreeListColumn.ColumnConfig colInfo in this.ColumnConfigs )
                lstSort.Add( colInfo.VisibleIndex+colInfo.Caption , colInfo );

            ABCTreeListColumn[] lstCols=new ABCTreeListColumn[lstSort.Count];
            int iCount=-1;
            foreach ( ABCTreeListColumn.ColumnConfig colInfo in lstSort.Values )
            {
                ABCTreeListColumn col=new ABCTreeListColumn( colInfo);
                if ( OwnerView==null||OwnerView.Mode!=ViewMode.Design )
                    col.InitRepository();
                iCount++;
                lstCols[iCount]=col;
          
            }
            this.InnerTreeList.Columns.AddRange( lstCols );
        }
        #endregion

        #endregion

        #region Script
        [Category( "ABC.Script" )]
        [Editor( typeof( SQLScriptEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "Script" ) , Description( "Which type to use..." )]
        public String Script { get; set; }

        public void LoadDataSourceFromScript ( )
        {
            if ( String.IsNullOrWhiteSpace( this.DataSource ) )
            {
                this.ShowSaveButton=false;
                this.ShowDeleteButton=false;

                if ( String.IsNullOrWhiteSpace( Script )==false )
                {
                    DataSet ds=DataQueryProvider.RunQuery( Script );
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        this.Manager.Invalidate( ds );
                        this.RefreshDataSource();
                    }
                }
            }
        }
        #endregion

        #region Row Detail
        //ABCTreeListRowDetail RowDetail;
        //public void ShowRowDetail ( )
        //{
        //    if ( RowDetail==null||RowDetail.IsDisposed)
        //        RowDetail=new ABCTreeListRowDetail( this );
        //    RowDetail.Show();
        //}
        #endregion

        #region Context Menu

        #region Init

        public class ABCTreelistMenuInfo
        {
            public ABCTreelistMenuInfo ( DevExpress.XtraTreeList.TreeListHitInfo hit , String strObjectName , String strFieldName , object _objectRow )
            {
                this.HitInfo=hit;
                this.FieldName=strFieldName;
                this.RowData=_objectRow;
                this.ObjectName=strObjectName;
            }
            public DevExpress.XtraTreeList.TreeListHitInfo HitInfo;
            public String FieldName;
            public object RowData;
            public String ObjectName;
        }
        public class ABCTreelistColumnMenu : DevExpress.XtraBars.PopupMenu
        {
            public ABCTreelistMenuInfo MenuInfo;
            public ABCTreelistColumnMenu ( System.Windows.Forms.Form form )
            {
                this.Manager=new DevExpress.XtraBars.BarManager( new System.ComponentModel.Container() );
                this.Manager.Form=form;
                this.Manager.Images=ABCImageList.List16x16;
            }
         
            public DevExpress.XtraBars.BarButtonItem AddItem ( String strCaption , object tag , int iImageIndex )
            {
                DevExpress.XtraBars.BarButtonItem newItem=new DevExpress.XtraBars.BarButtonItem( this.Manager , strCaption );
                newItem.Tag=tag;
                newItem.ImageIndex=iImageIndex;
                 this.AddItem( newItem );

                 return newItem;
            }
            public DevExpress.XtraBars.BarButtonItem AddItem ( DevExpress.XtraBars.BarSubItem parent , String strCaption , object tag , int iImageIndex )
            {
                DevExpress.XtraBars.BarButtonItem newItem=new DevExpress.XtraBars.BarButtonItem( this.Manager , strCaption );
                newItem.Tag=tag;
                newItem.ImageIndex=iImageIndex;
                parent.AddItem( newItem );

                return newItem;
            }

            public DevExpress.XtraBars.BarSubItem AddSubMenu( String strCaption , object tag , int iImageIndex )
            {
                DevExpress.XtraBars.BarSubItem newItem=new DevExpress.XtraBars.BarSubItem( this.Manager , strCaption );
                newItem.Tag=tag;
                newItem.ImageIndex=iImageIndex;
                this.AddItem( newItem );

                return newItem;
            }
            public DevExpress.XtraBars.BarSubItem AddSubSubMenu ( DevExpress.XtraBars.BarSubItem parent , String strCaption , object tag , int iImageIndex )
            {
                DevExpress.XtraBars.BarSubItem newItem=new DevExpress.XtraBars.BarSubItem( this.Manager , strCaption );
                newItem.Tag=tag;
                newItem.ImageIndex=iImageIndex;
                parent.AddItem( newItem );

                return newItem;
            }
        }

        public ABCTreelistColumnMenu PopupMenu=null;

        public virtual DevExpress.XtraBars.BarButtonItem PopupMenuAddItem ( String strCaption , object tag , int iImageIndex , bool isBeginGroup )
        {
            DevExpress.XtraBars.BarButtonItem menuItem=PopupMenu.AddItem( strCaption , tag , iImageIndex );
            menuItem.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( PopupMenu_Click );
            menuItem.Links[0].BeginGroup=isBeginGroup;
            return menuItem;
        }
        public virtual DevExpress.XtraBars.BarButtonItem PopupMenuAddItem ( DevExpress.XtraBars.BarSubItem subMenu , String strCaption , object tag , int iImageIndex , bool isBeginGroup )
        {
            DevExpress.XtraBars.BarButtonItem menuItem=PopupMenu.AddItem(subMenu, strCaption , tag , iImageIndex );
            menuItem.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( PopupMenu_Click );
            menuItem.Links[0].BeginGroup=isBeginGroup;
            return menuItem;
        }

        public virtual void InitCellPopupMenu ( DevExpress.XtraTreeList.TreeListHitInfo hi )
        {
            if ( PopupMenu==null )
                PopupMenu=new ABCTreelistColumnMenu( this.FindForm() );
            PopupMenu.ClearLinks();

            InitRowPopupMenu( hi );

        }
        public virtual void InitRowPopupMenu ( DevExpress.XtraTreeList.TreeListHitInfo hi )
        {
            if ( PopupMenu==null )
                PopupMenu=new ABCTreelistColumnMenu( this.FindForm() );
            PopupMenu.ClearLinks();

            ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( hi.Node );
            if ( node!=null&&node.InnerData!=null )
            {

               
                TreeConfigNode config=null;
                if ( this.Manager.ConfigList.TryGetValue( node.ObjectName , out config ) )
                {
                    #region Add, Change,Remove

                    if (config.InnerData.AllowAdd&& config.InnerData.ParentTableName==config.InnerData.TableName
                        &&config.InnerData.ParentField==DataStructureProvider.GetPrimaryKeyColumn( config.InnerData.TableName ) )
                    {
                               String strTableCaption=DataConfigProvider.GetTableCaption( config.InnerData.TableName );
                               if ( String.IsNullOrWhiteSpace( strTableCaption )==false )
                                   PopupMenuAddItem( "Thêm "+strTableCaption , "ADD-"+config.InnerData.Name , ABCImageList.GetImageIndex16x16( "Add" ) , false );
                    
                    }

                    foreach ( TreeConfigNode childConfig in config.ChildrenNodes.Values )
                    {
                        if ( childConfig.InnerData.AllowAdd )
                        {
                            String strTableCaption=DataConfigProvider.GetTableCaption( childConfig.InnerData.TableName );
                            if ( String.IsNullOrWhiteSpace( strTableCaption )==false )
                                PopupMenuAddItem( "Thêm "+strTableCaption , "ADD-"+childConfig.InnerData.Name , ABCImageList.GetImageIndex16x16( "Add" ) , false );
                        }
                    }

                    if ( config.InnerData.AllowEdit )
                        PopupMenuAddItem( "Sửa" , "EDIT" , ABCImageList.GetImageIndex16x16( "Edit" ) , false );

                    if ( config.InnerData.AllowDelete )
                        PopupMenuAddItem( "Xóa" , "DELETE" , ABCImageList.GetImageIndex16x16( "Delete" ) , false );
                    #endregion

                    if ( ABCPresentHelper.IsMainObject( config.InnerData.TableName ) )
                    {
                        DevExpress.XtraBars.BarButtonItem item=PopupMenuAddItem( "Chi tiết" , "DETAIL" , ABCImageList.GetImageIndex16x16( "DocLink" ) , true );
                        item.Links[0].BeginGroup=true;
                    }

                    #region Related Information
                    List<String> lstFKs=new List<string>();
                    foreach ( String strFK in DataStructureProvider.DataTablesList[config.InnerData.TableName].ForeignColumnsList.Keys )
                    {
                        String strTableName=DataStructureProvider.GetTableNameOfForeignKey( config.InnerData.TableName , strFK );
                        if ( ABCPresentHelper.IsMainObject( strTableName ) )
                        {
                            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( node.InnerData , strFK );
                            int iLinkID=-1;
                            try
                            {
                                iLinkID=Convert.ToInt32( objValue );
                            }
                            catch ( Exception ex )
                            {
                                return;
                            }
                            if ( iLinkID>0 )
                                lstFKs.Add( strFK );
                        }
                    }

                    if ( lstFKs.Count>0 )
                    {
                        DevExpress.XtraBars.BarSubItem subMenu=PopupMenu.AddSubMenu( "Thông tin liên quan" , null , ABCImageList.GetImageIndex16x16( "Info" ) );
                        if ( subMenu!=null )
                        {
                            foreach ( String strFK in lstFKs )
                            {
                                String strCaption=DataConfigProvider.GetFieldCaption( config.InnerData.TableName , strFK );
                                if ( String.IsNullOrWhiteSpace( strCaption ) )
                                {
                                    String strTableName=DataStructureProvider.GetTableNameOfForeignKey( config.InnerData.TableName , strFK );
                                    strCaption=DataConfigProvider.GetTableCaption( strTableName );
                                }

                                if ( String.IsNullOrWhiteSpace( strCaption )==false )
                                    PopupMenuAddItem( subMenu , strCaption , "INFO-"+strFK , ABCImageList.GetImageIndex16x16( "View" ) , false );
                            }
                        }
                    }
                    #endregion

                }
           

                if ( ShowRefreshButton&&ShowMenuBar )
                    PopupMenuAddItem( "Làm mới" , "REFRESH" , ABCImageList.GetImageIndex16x16( "Refresh" ) , false );
            }

            PopupMenuAddItem( "Mở rộng" , "EXPAND" , ABCImageList.GetImageIndex16x16( "Right" ) , true );
            PopupMenuAddItem( "Gom lại" , "COLLAPSE" , ABCImageList.GetImageIndex16x16( "Left" ) , false );
            PopupMenuAddItem( "Mở rộng tất cả" , "EXPANDALL" , ABCImageList.GetImageIndex16x16( "Right" ) , false );
            PopupMenuAddItem( "Gom tất cả" , "COLLAPSEALL" , ABCImageList.GetImageIndex16x16( "Left" ) , false );

            OnCustomContextMenu( hi );
        }

        #region Events
        public delegate void ABCCustomContextMenuEventHandler ( ABCTreelistColumnMenu popupMenu , DevExpress.XtraTreeList.TreeListHitInfo hi );
        public event ABCCustomContextMenuEventHandler CustomContextMenu;

        public virtual void OnCustomContextMenu ( DevExpress.XtraTreeList.TreeListHitInfo hi )
        {
            if ( this.CustomContextMenu!=null )
                this.CustomContextMenu( PopupMenu , hi );
        }

        #endregion

        #endregion

        #region Show Context Menu

        public void DoShowPopupMenu ( DevExpress.XtraTreeList.TreeListHitInfo hi )
        {
            if ( hi.Column==null )
                return;

            ABCTreeListNode node=this.InnerTreeList.GetDataRecordByNode( hi.Node ) as ABCTreeListNode;

            #region CellPopupMenu
            if ( hi.HitInfoType==HitInfoType.Cell)
            {
                if ( PopupMenu==null )
                    PopupMenu=new ABCTreelistColumnMenu( this.FindForm() );

                ABCTreelistMenuInfo info=new ABCTreelistMenuInfo( hi , node.ObjectName , hi.Column.FieldName , node.InnerData );
                PopupMenu.MenuInfo=info;

                InitCellPopupMenu( hi );
               
                System.Drawing.Point pt=this.InnerTreeList.PointToScreen( hi.MousePoint );
                if ( pt!=null )
                    PopupMenu.ShowPopup( pt );

            }
            #endregion

            #region ABCPopupMenu
            if ( hi.HitInfoType==HitInfoType.Row&&PopupMenu!=null )
            {
                if ( PopupMenu==null )
                    PopupMenu=new ABCTreelistColumnMenu( this.FindForm() );

                ABCTreelistMenuInfo info=new ABCTreelistMenuInfo( hi , node.ObjectName , hi.Column.FieldName , node.InnerData );
                PopupMenu.MenuInfo=info;

                InitRowPopupMenu( hi );
               
                System.Drawing.Point pt=this.InnerTreeList.PointToScreen( hi.MousePoint );
                if ( pt!=null )
                    PopupMenu.ShowPopup( pt );

            }
            #endregion
        }

        #endregion

        #region Default Events
        public virtual void PopupMenu_Click ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {

            if ( PopupMenu.MenuInfo is ABCTreelistMenuInfo==false )
                return;

            DevExpress.XtraBars.BarButtonItem item=e.Item as DevExpress.XtraBars.BarButtonItem;
             BusinessObject objBus=PopupMenu.MenuInfo.RowData as BusinessObject;
             ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( this.InnerTreeList.FocusedNode );

             if ( item.Tag.ToString().Equals( "DETAIL" )&&PopupMenu.MenuInfo.RowData!=null )
             {
                 if ( node!=null&&this.Manager.ConfigList.ContainsKey( node.ObjectName )&&this.Manager.ConfigList[node.ObjectName].InnerData.AllowEdit==false )
                     ActionViewDetail( objBus , ABCScreenAction.Disable );
                 else
                     ActionViewDetail( objBus , ABCScreenAction.None );
             }
            if ( item.Tag.ToString().StartsWith( "ADD-" ) )
            {
                String strObjectName=item.Tag.ToString().Substring( 4 , item.Tag.ToString().Length-4 );
                ActionAddNew( strObjectName );
                ActionRefreshNodes();
            }

            if ( item.Tag.ToString().StartsWith( "INFO-" ) )
            {
                String strFK=item.Tag.ToString().Substring( 5 , item.Tag.ToString().Length-5 );
                ActionViewInfo( strFK );
                ActionRefreshNodes();
            }

            if ( item.Tag.ToString().Equals( "EDIT" ) )
            {
                ActionViewDetail( objBus , ABCScreenAction.Edit );
                ActionRefreshNodes();
            }
            if ( item.Tag.ToString().Equals( "DELETE" ) )
                ActionDeleteNodes();

       
            if ( item.Tag.ToString().Equals( "REFRESH" ) )
                ActionRefreshNodes();
            
            #region Expand - Collapse
            if ( item.Tag.ToString().Equals( "EXPANDALL" ) )
                ActionExpandAllTree();

            if ( item.Tag.ToString().Equals( "COLLAPSEALL" ) )
                ActionCollapseAllTree();

            if ( item.Tag.ToString().Equals( "EXPAND" ) )
                ActionExpandNodes();

            if ( item.Tag.ToString().Equals( "COLLAPSE" ) )
                ActionCollapseNodes(); 
            #endregion
        }

        public virtual void ActionViewDetail ( BusinessObject objBus,ABCScreenAction action)
        {
            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objBus , DataStructureProvider.GetPrimaryKeyColumn( objBus.AATableName ) );
            if ( objValue==null||ABCHelper.DataConverter.ConvertToGuid( objValue ).GetType()!=typeof( Guid ) )
                return;

            Guid  iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
            if ( iLinkID!=Guid.Empty )
            {
                if ( this.OwnerView!=null )
                    ABCScreen.ABCScreenHelper.Instance .RunLink( objBus.AATableName , this.OwnerView.Mode ,true , iLinkID , action );
                else
                     ABCScreen.ABCScreenHelper.Instance.RunLink( objBus.AATableName , ViewMode.Runtime , true , iLinkID , action );

                ActionRefreshNodes();

            }
        }
        public virtual void ActionViewInfo ( String strFK )
        {
            ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( this.InnerTreeList.FocusedNode );
            if ( node!=null&&node.InnerData!=null )
            {
                object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( node.InnerData , strFK );
                Guid iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                if ( iLinkID!=Guid.Empty )
                {
                    String strTableName=DataStructureProvider.GetTableNameOfForeignKey( node.InnerData.AATableName , strFK );
                    if ( this.OwnerView!=null )
                         ABCScreen.ABCScreenHelper.Instance.RunLink( strTableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                    else
                         ABCScreen.ABCScreenHelper.Instance.RunLink( strTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );

                    ActionRefreshNodes();

                }
            }
        }

        public virtual void ActionAddNew ( String strObjectName )
        {
            if ( this.Manager.ConfigList.ContainsKey( strObjectName )==false )
                return;

            String strTableName=this.Manager.ConfigList[strObjectName].InnerData.TableName;
             ABCScreen.ABCScreenHelper.Instance.OpenScreenForNew( strTableName , this.OwnerView.Mode , true );

            ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( this.InnerTreeList.FocusedNode );
            if ( node!=null )
            {
                this.Manager.RefreshCachingNodes( strObjectName );
                node.RefreshData( true , true , false );
            }
            this.InnerTreeList.DataSource=this.Manager.RootData;
            this.InnerTreeList.RefreshDataSource();

            ActionRefreshNodes();

            if ( this.InnerTreeList.Selection.Count>0 )
                this.InnerTreeList.SetFocusedNode( this.InnerTreeList.Selection[0] );
        }
        public virtual void ActionDeleteNodes ( )
        {
            DialogResult dlgResult=ABCHelper.ABCMessageBox.Show( "Bạn có thực sự muốn xóa dữ liệu ?" , "Thông báo" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
            if ( dlgResult!=DialogResult.Yes )
                return;

            Dictionary<Guid , ABCTreeListNode> parentNodes=new Dictionary<Guid , ABCTreeListNode>();
            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode treeNode in this.InnerTreeList.Selection )
            {
                ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( treeNode );
                if ( node!=null&&node.InnerData!=null )
                {
                    BusinessControllerFactory.GetBusinessController( node.InnerData.AATableName ).DeleteObject( node.InnerData );
                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( node.InnerData , DataStructureProvider.GetPrimaryKeyColumn( node.InnerData.AATableName ) ) );
                    if ( iID!=Guid.Empty )
                    {
                        if ( node.ParentNode!=null )
                        {
                            Guid iParentID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( node.ParentNode.InnerData , DataStructureProvider.GetPrimaryKeyColumn( node.ParentNode.InnerData.AATableName ) ) );
                            if ( parentNodes.ContainsKey( iParentID )==false )
                                parentNodes.Add( iParentID , node.ParentNode );

                            node.ParentNode.ChildrenNodes.Remove( iID );
                            node.ParentNode=null;
                        }
                        Dictionary<Guid , ABCTreeListNode> innerList=null;
                        if ( this.Manager.DataList.TryGetValue( node.ObjectName , out innerList ) )
                        {
                            if ( innerList.ContainsKey( iID ) )
                                innerList.Remove( iID );
                        }
                    }
                }
            }

            foreach ( ABCTreeListNode node in parentNodes.Values )
                node.RefreshData( true , true , true );

            this.InnerTreeList.DataSource=this.Manager.RootData;
            this.InnerTreeList.RefreshDataSource();
            if ( this.InnerTreeList.Selection.Count>0 )
                this.InnerTreeList.SetFocusedNode( this.InnerTreeList.Selection[0] );
        }

        public virtual void ActionRefreshAllTree()
        {
            this.Manager.RootData.RefreshData( false,true , true );
            this.InnerTreeList.DataSource=this.Manager.RootData;
            this.InnerTreeList.RefreshDataSource();
        }
        public virtual void ActionExpandAllTree ( )
        {
            this.Manager.RootData.RefreshData( false,true , false );
            this.InnerTreeList.DataSource=this.Manager.RootData;
            this.InnerTreeList.RefreshDataSource();

            this.InnerTreeList.ExpandAll();
        }
        public virtual void ActionCollapseAllTree ( )
        {
            this.InnerTreeList.CollapseAll();
        }

        public virtual void ActionRefreshNodes ( )
        {
            if ( this.InnerTreeList.FocusedNode!=null )
            {
                if ( this.InnerTreeList.Selection.Count<=0 )
                    this.InnerTreeList.Selection.Add( this.InnerTreeList.FocusedNode );
            }

            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode treeNode in this.InnerTreeList.Selection )
            {
                ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( treeNode );
                if ( node!=null )
                    node.RefreshData( true , false , false );
            }

            if ( this.InnerTreeList.Selection.Count>0 )
            {
                this.InnerTreeList.DataSource=this.Manager.RootData;
                this.InnerTreeList.RefreshDataSource();
                this.InnerTreeList.SetFocusedNode( this.InnerTreeList.Selection[0] );
            }
        }
        public virtual void ActionExpandNodes ( )
        {
            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode treeNode in this.InnerTreeList.Selection )
            {
                ABCTreeListNode node=(ABCTreeListNode)this.InnerTreeList.GetDataRecordByNode( treeNode );
                if ( node!=null )
                    node.ExpandData( false , false );
            }

            this.InnerTreeList.DataSource=this.Manager.RootData;
            this.InnerTreeList.RefreshDataSource();

            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode treeNode in this.InnerTreeList.Selection )
                treeNode.Expanded=true;

        }
        public virtual void ActionCollapseNodes ( )
        {
            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode treeNode in this.InnerTreeList.Selection )
                treeNode.Expanded=false;
        }


        #endregion


        #endregion

        public void RefreshDataSource ( )
        {
            if ( this.InnerTreeList!=null )
                this.InnerTreeList.RefreshDataSource();
        }

    }

    public class ABCTreeListDesigner : DevExpress.XtraEditors.Design.BaseEditDesigner
    {
        public ABCTreeListDesigner ( )
        {
        }

        public override IList SnapLines
        {
            get
            {

                ArrayList snapLines=base.SnapLines as ArrayList;
                snapLines.Add( new SnapLine( SnapLineType.Baseline , 0 , SnapLinePriority.Medium ) );
                snapLines.Add( new SnapLine( SnapLineType.Bottom ,0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Horizontal , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Left , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Right , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Top , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Vertical , 0 , SnapLinePriority.High ) );
                return snapLines;
            }
        }
        public override   bool ParticipatesWithSnapLines
        {
            get
            {
                return true;
            }
        }
    }


}
