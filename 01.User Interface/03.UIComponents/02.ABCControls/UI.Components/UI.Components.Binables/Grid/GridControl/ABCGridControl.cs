using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;

using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;
using DevExpress.XtraEditors.Repository;

namespace ABCControls
{

    [ToolboxBitmapAttribute( typeof( DevExpress.XtraGrid.GridControl ) )]
    [Designer( typeof( ABCGridControlDesigner ) )]
    public partial class ABCGridControl : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl , IABCGridControl , IABCCustomControl
    {


        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [Browsable( false )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [ReadOnly( true )]
        public String LookupTableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "GridDataSource";
            }
        }
        #endregion

        #region IABCGridControl

        public ABCView OwnerView { get; set; }

        public DevExpress.XtraGrid.Views.Grid.GridView GridDefaultView
        {
            get
            {
                return this.DefaultView;
            }
        }

        [Browsable( false )]
        public object GridDataSource
        {
            get
            {
                return this.InnerGrid.DataSource;
            }
            set
            {
                this.InnerGrid.DataSource=value;
            }
        }

        [Category( "Options" )]
        public string ViewCaption { get { return DefaultView.ViewCaption; } set { DefaultView.ViewCaption=value; } }

        [Category( "Printing" )]
        public string ReportName { get; set; }
        
        [Editor( typeof( RichTextEditor ) , typeof( UITypeEditor ) )]
        [Category( "Printing" )]
        public String Header { get; set; }

        [Editor( typeof( RichTextEditor ) , typeof( UITypeEditor ) )]
        [Category( "Printing" )]
        public String Footer { get; set; }

        #endregion

        #region GridView- Properties

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
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )
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
                    DefaultView.Appearance.Empty.BackColor=System.Drawing.Color.FromArgb( 181 , 200 , 223 );
                    DefaultView.Appearance.Empty.Options.UseBackColor=true;
                }
                else
                    DefaultView.Appearance.Empty.Options.UseBackColor=false;
            }
        }


        #region Options
        [Category( "Options" )]
        public bool AllowCellMerge { get { return DefaultView.OptionsView.AllowCellMerge; } set { DefaultView.OptionsView.AllowCellMerge=value; } }

        [Category( "Options" )]
        public DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle FocusRectStyle { get { return DefaultView.FocusRectStyle; } set { DefaultView.FocusRectStyle=value; } }

        [Category( "Options" )]
        public bool ShowViewCaption { get { return DefaultView.OptionsView.ShowViewCaption; } set { DefaultView.OptionsView.ShowViewCaption=value; } }

        [Category( "Options" )]
        public DevExpress.Utils.DefaultBoolean AllowAddRows { get { return DefaultView.OptionsBehavior.AllowAddRows; } set { DefaultView.OptionsBehavior.AllowAddRows=value; } }

        [Category( "Options" )]
        public DevExpress.XtraGrid.Views.Grid.NewItemRowPosition NewItemRowPosition { get { return DefaultView.OptionsView.NewItemRowPosition; } set { DefaultView.OptionsView.NewItemRowPosition=value; } }

        [Category( "Options" )]
        public DevExpress.Utils.DefaultBoolean AllowDeleteRows { get { return DefaultView.OptionsBehavior.AllowDeleteRows; } set { DefaultView.OptionsBehavior.AllowDeleteRows=value; } }
        [Category( "Options" )]
        public bool Editable { get { return DefaultView.OptionsBehavior.Editable; } set { DefaultView.OptionsBehavior.Editable=value; } }
        [Category( "Options" )]
        public bool ReadOnly { get { return DefaultView.OptionsBehavior.ReadOnly; } set { DefaultView.OptionsBehavior.ReadOnly=value; } }
        [Category( "Options" )]
        public bool ShowFind { get { return DefaultView.OptionsFind.AlwaysVisible; } set { DefaultView.OptionsFind.AlwaysVisible=value; } }
        [Category( "Options" )]
        public bool MultiSelect { get { return DefaultView.OptionsSelection.MultiSelect; } set { DefaultView.OptionsSelection.MultiSelect=value; } }
        [Category( "Options" )]
        public DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode MultiSelectMode { get { return DefaultView.OptionsSelection.MultiSelectMode; } set { DefaultView.OptionsSelection.MultiSelectMode=value; } }
        [Category( "Options" )]
        public bool DrawColorEvenRow { get { return DefaultView.OptionsView.EnableAppearanceEvenRow; } set { DefaultView.OptionsView.EnableAppearanceEvenRow=value; } }
        [Category( "Options" )]
        public bool DrawColorOddRow { get { return DefaultView.OptionsView.EnableAppearanceOddRow; } set { DefaultView.OptionsView.EnableAppearanceOddRow=value; } }
        [Category( "Options" )]
        public bool DrawNotFocusSelection { get { return DefaultView.OptionsSelection.EnableAppearanceHideSelection; } set { DefaultView.OptionsSelection.EnableAppearanceHideSelection=value; } }

        [Category( "Options" )]
        public bool EnableFocusedCell { get { return DefaultView.OptionsSelection.EnableAppearanceFocusedCell; } set { DefaultView.OptionsSelection.EnableAppearanceFocusedCell=value; } }
        [Category( "Options" )]
        public bool EnableFocusedRow { get { return DefaultView.OptionsSelection.EnableAppearanceFocusedRow; } set { DefaultView.OptionsSelection.EnableAppearanceFocusedRow=value; } }

        [Category( "Options" )]
        public bool ShowAutoFilterRow { get { return DefaultView.OptionsView.ShowAutoFilterRow; } set { DefaultView.OptionsView.ShowAutoFilterRow=value; } }
        [Category( "Options" )]
        public bool ShowColumnHeaders { get { return DefaultView.OptionsView.ShowColumnHeaders; } set { DefaultView.OptionsView.ShowColumnHeaders=value; } }
        [Category( "Options" )]
        public DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode ShowFilterPanelMode { get { return DefaultView.OptionsView.ShowFilterPanelMode; } set { DefaultView.OptionsView.ShowFilterPanelMode=value; } }
        [Category( "Options" )]
        public bool ShowFooter { get { return DefaultView.OptionsView.ShowFooter; } set { DefaultView.OptionsView.ShowFooter=value; } }
        [Category( "Options" )]
        public bool ShowGroupPanel { get { return DefaultView.OptionsView.ShowGroupPanel; } set { DefaultView.OptionsView.ShowGroupPanel=value; } }
        [Category( "Options" )]
        public bool ShowGroupExpandCollapseButtons { get { return DefaultView.OptionsView.ShowGroupExpandCollapseButtons; } set { DefaultView.OptionsView.ShowGroupExpandCollapseButtons=value; } }
        [Category( "Options" )]
        public bool ShowHorzLines { get { return DefaultView.OptionsView.ShowHorzLines; } set { DefaultView.OptionsView.ShowHorzLines=value; } }
        [Category( "Options" )]
        public bool ShowVertLines { get { return DefaultView.OptionsView.ShowVertLines; } set { DefaultView.OptionsView.ShowVertLines=value; } }
        [Category( "Options" )]
        public bool ShowIndicator { get { return DefaultView.OptionsView.ShowIndicator; } set { DefaultView.OptionsView.ShowIndicator=value; } }
        [Category( "Options" )]
        public bool ShowPreview { get { return DefaultView.OptionsView.ShowPreview; } set { DefaultView.OptionsView.ShowPreview=value; } }
        [Category( "Options" )]
        public string PreviewFieldName { get { return DefaultView.PreviewFieldName; } set { DefaultView.PreviewFieldName=value; } }
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
        [Editor( typeof( GridColumnsEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "Columns" ) , Description( "Which type to use..." )]
        public BindingList<ABCGridColumn.ColumnConfig> ColumnConfigs
        {
            get
            {
                return this.DefaultView.ColumnConfigs;
            }
            set
            {
                this.DefaultView.ColumnConfigs=value;
                this.DefaultView.InitColumns();
            }
        }

        #endregion


        BusinessObjectController Controller;
        public ABCGridControl ( )
        {
            InitializeComponent();
            InitMenuBar();

            this.DefaultView.KeyDown+=new KeyEventHandler( gridView_KeyDown );
            this.DefaultView.MouseUp+=new MouseEventHandler( GridControl_MouseUp );
            this.Disposed+=new EventHandler( ABCGridControl_Disposed );

            if ( String.IsNullOrWhiteSpace( TableName )==false )
                Controller=BusinessControllerFactory.GetBusinessController( TableName );


            this.DrawNotFocusSelection=false;

            DrawBlueEmptyArea=true;
        }

        void ABCGridControl_Disposed ( object sender , EventArgs e )
        {
            if ( RowDetail!=null )
            {
                if ( RowDetail.Visible )
                    RowDetail.Close();

                RowDetail.Dispose();
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
            mainBar.BarName="Grid menu";
            mainBar.DockCol=0;
            mainBar.DockRow=0;
            mainBar.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            mainBar.OptionsBar.AllowQuickCustomization=false;
            mainBar.OptionsBar.DrawDragBorder=false;
            mainBar.OptionsBar.MultiLine=true;
            mainBar.OptionsBar.UseWholeRow=true;
            mainBar.Text="Grid menu";

            barManager=new DevExpress.XtraBars.BarManager();
            barManager.Bars.AddRange( new DevExpress.XtraBars.Bar[] { mainBar } );
            barManager.Form=this;
            barManager.Items.AddRange( new DevExpress.XtraBars.BarItem[] { SaveItem , DeleteItem , RefreshItem , RowDetailItem , FilterItem , ColumnChooserItem , ExportItem , PrintItem } );
            barManager.MainMenu=mainBar;
            barManager.MaxItemId=5;
            barManager.Images=ABCImageList.List16x16;
            barManager.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( barManager_ItemClick );
            mainBar.AddItems( new DevExpress.XtraBars.BarItem[] { SaveItem , DeleteItem , RefreshItem , RowDetailItem , FilterItem , ColumnChooserItem , ExportItem , PrintItem } );
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
            String strTag=e.Item.Tag.ToString();

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
        public virtual void OnBarItemClick ( String strTag )
        {
            if ( this.BarItemClick!=null )
                this.BarItemClick( this , strTag );
        }

        public virtual void OnRefresh ( )
        {
            if ( String.IsNullOrWhiteSpace( this.DataSource )&&String.IsNullOrWhiteSpace( this.Script )==false )
                LoadDataSourceFromScript();

        }
        public virtual void OnFilter ( )
        {
            this.DefaultView.ShowFilterEditor( null );
        }
        public virtual void OnColumnChooser ( )
        {
            this.DefaultView.ShowCustomization();
        }
        public virtual void OnShowRowDetail ( )
        {
            ShowRowDetail();
        }
        public virtual void OnExport ( )
        {
            SaveFileDialog dlg=new SaveFileDialog();
            dlg.Filter="xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            dlg.FilterIndex=2;
            dlg.RestoreDirectory=true;
            if ( dlg.ShowDialog()==DialogResult.OK )
            {
                //foreach ( DevExpress.XtraGrid.Columns.GridColumn col in DefaultView.Columns )
                //    col.MinWidth=DefaultView.CalcColumnBestWidth( col );


                //if ( dlg.FileName.EndsWith( "xlsx" ) )
                //{
                //    DevExpress.XtraPrinting.XlsxExportOptions xlsOptions=new DevExpress.XtraPrinting.XlsxExportOptions();
                //    xlsOptions.TextExportMode=DevExpress.XtraPrinting.TextExportMode.Text;
                //    xlsOptions.ShowGridLines=true;
                //    this.DefaultView.ExportToXlsx( dlg.FileName , xlsOptions );
                //}
                //else
                //{
                //    DevExpress.XtraPrinting.XlsExportOptions xlsOptions=new DevExpress.XtraPrinting.XlsExportOptions();
                //    xlsOptions.TextExportMode=DevExpress.XtraPrinting.TextExportMode.Text;
                //    xlsOptions.ShowGridLines=true;
                //    this.DefaultView.ExportToXls( dlg.FileName , xlsOptions );
                //}
                if ( dlg.FileName.EndsWith( "xlsx" ) )
                {
                    DevExpress.XtraExport.ExportXlsxProvider provider=new DevExpress.XtraExport.ExportXlsxProvider( dlg.FileName );
                    DevExpress.XtraGrid.Export.GridViewExportLink link=DefaultView.CreateExportLink( provider ) as DevExpress.XtraGrid.Export.GridViewExportLink;
                    //    link.ExportCellsAsDisplayText=false;
                    link.ExportTo( true );

                }
                else
                {
                    DevExpress.XtraPrinting.XlsExportOptions xlsOptions=new DevExpress.XtraPrinting.XlsExportOptions();
                    xlsOptions.TextExportMode=DevExpress.XtraPrinting.TextExportMode.Text;
                    xlsOptions.ShowGridLines=true;
                    this.DefaultView.ExportToXls( dlg.FileName , xlsOptions );
                }
             
                System.Diagnostics.Process.Start( dlg.FileName );
            }




        }

        #region OnPrint
        public virtual void OnPrint ( )
        {
            #region Grid
            //if ( composLink.Landscape )
            //    this.GridDefaultView.OptionsPrint.AutoWidth=( dbWidth<1000 );
            //else
            //    this.GridDefaultView.OptionsPrint.AutoWidth=( dbWidth<700 );

            this.GridDefaultView.OptionsPrint.UsePrintStyles=true;
            this.GridDefaultView.OptionsPrint.AutoWidth=true;

            this.GridDefaultView.AppearancePrint.Row.BorderColor=Color.Black;
            this.GridDefaultView.AppearancePrint.Lines.BorderColor=Color.Black;
            this.GridDefaultView.AppearancePrint.Lines.BackColor=Color.Black;

            this.GridDefaultView.AppearancePrint.FooterPanel.BackColor=Color.White;
            this.GridDefaultView.AppearancePrint.FooterPanel.ForeColor=Color.Black;
            this.GridDefaultView.AppearancePrint.FooterPanel.BorderColor=Color.Black;

            this.GridDefaultView.AppearancePrint.HeaderPanel.BackColor=Color.Gray;
            this.GridDefaultView.AppearancePrint.HeaderPanel.ForeColor=Color.Black;
            this.GridDefaultView.AppearancePrint.HeaderPanel.BorderColor=Color.Black;
            this.GridDefaultView.AppearancePrint.HeaderPanel.TextOptions.HAlignment=HorzAlignment.Center;
            #endregion

            double dbWidth=0;
            foreach ( ABCGridColumn col in this.GridDefaultView.VisibleColumns )
                dbWidth+=col.VisibleWidth;

            PrintingSystem printingSystem1=new PrintingSystem();
            DevExpress.XtraPrintingLinks.CompositeLink composLink=new DevExpress.XtraPrintingLinks.CompositeLink( printingSystem1 );
            composLink.Margins=new System.Drawing.Printing.Margins( 50 , 50 , 40 , 50 );
            composLink.PaperKind=System.Drawing.Printing.PaperKind.A4;
            composLink.VerticalContentSplitting=VerticalContentSplitting.Smart;
            composLink.CreateMarginalHeaderArea+=new CreateAreaEventHandler( printableGridLink_CreateMarginalHeaderArea );
            composLink.CreateMarginalFooterArea+=new CreateAreaEventHandler( printableGridLink_CreateMarginalFooterArea );
            composLink.Landscape=!( dbWidth<1000 );

            #region Header - Footer
            if ( String.IsNullOrWhiteSpace( this.Header ) )
            {
                Link linkHeader=new Link();
                linkHeader.CreateDetailArea+=new CreateAreaEventHandler( linkHeader_CreateDetailArea );
                composLink.Links.Add( linkHeader );
            }
            else
                composLink.RtfReportHeader=this.Header;

            composLink.RtfReportFooter=this.Footer;
            #endregion

            Link linkSeparate=new Link();
            linkSeparate.CreateDetailArea+=new CreateAreaEventHandler( linkSeparate_CreateDetailArea );


            bool isShowViewCaption=this.ShowViewCaption;
            this.ShowViewCaption=false;

            PrintableComponentLink printableGridLink=new PrintableComponentLink();
            printableGridLink.Component=this.InnerGrid;
            composLink.Links.Add( linkSeparate );
            composLink.Links.Add( printableGridLink );
            composLink.Links.Add( linkSeparate );

            composLink.CreateDocument( false );
            // if ( ( composLink.Landscape&&dbWidth>=1000 )||( composLink.Landscape==false&&dbWidth>=700 ) )
            printableGridLink.PrintingSystem.Document.AutoFitToPagesWidth=1;

            composLink.PrintingSystem.PreviewFormEx.WindowState=FormWindowState.Normal;
            composLink.PrintingSystem.PreviewFormEx.Width=500;
            composLink.PrintingSystem.PreviewFormEx.Height=700;
            composLink.PrintingSystem.PreviewFormEx.StartPosition=FormStartPosition.CenterParent;
            composLink.PrintingSystem.PreviewFormEx.Shown+=new EventHandler( PreviewFormEx_Shown );
            composLink.PrintingSystem.PreviewFormEx.Show( ABCApp.ABCAppHelper.Instance.MainForm , this.LookAndFeel );
            this.ShowViewCaption=isShowViewCaption;
        }

        void PreviewFormEx_Shown ( object sender , EventArgs e )
        {
            Form form=sender as Form;
            form.WindowState=FormWindowState.Normal;
        }

        void linkSeparate_CreateDetailArea ( object sender , CreateAreaEventArgs e )
        {

            TextBrick tb=new TextBrick();
            tb.Rect=new RectangleF( 0 , 0 , e.Graph.ClientPageSize.Width , 20 );
            tb.BackColor=Color.Transparent;
            tb.BorderColor=Color.Transparent;
            e.Graph.DrawBrick( tb );
        }

        void printableGridLink_CreateMarginalHeaderArea ( object sender , CreateAreaEventArgs e )
        {
            e.Graph.Modifier=BrickModifier.MarginalHeader;
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) , 10 );
            string format="In ngày : {0:dd/M/yyyy}";
            PageInfoBrick brick=e.Graph.DrawPageInfo( PageInfo.DateTime , format , Color.FromArgb( 255 , 119 , 149 , 203 ) ,
            new RectangleF( 0 , 0 , 0 , 20 ) , BorderSide.None );
            brick.Alignment=BrickAlignment.Near;
            brick.AutoWidth=true;
        }
        void printableGridLink_CreateMarginalFooterArea ( object sender , CreateAreaEventArgs e )
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
        void linkHeader_CreateDetailArea ( object sender , CreateAreaEventArgs e )
        {
            string reportHeader=this.ViewCaption;

            e.Graph.StringFormat=new BrickStringFormat(StringAlignment.Center, StringAlignment.Center );
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) , 17 , FontStyle.Bold|FontStyle.Underline );
            e.Graph.BackColor=Color.White;
            RectangleF rec=new RectangleF( 50 , 20 , e.Graph.PrintingSystem.PageSettings.UsablePageRect.Width , 50 );
            e.Graph.DrawString( reportHeader , Color.FromArgb( 255 , 119 , 149 , 203 ) , rec , BorderSide.None );

        }
        #endregion
        #endregion

        #endregion

        #region IABCControl
        public void InitControl ( )
        {

        }
        #endregion

        #region GridView

        #region Layout

        #region Save
        public void GetChildrenXMLLayout ( XmlElement gridElement )
        {
            XmlNode nodeCol=ABCHelper.DOMXMLUtil.GetFirstNode( gridElement , "P" , "Collection(" );
            if ( nodeCol!=null )
                gridElement.RemoveChild( nodeCol );

            foreach ( ABCGridColumn col in DefaultView.Columns )
            {
                XmlElement ele=ABCPresentHelper.Serialization( gridElement.OwnerDocument , col.Config , "CL" );
                gridElement.AppendChild( ele );
            }
        }
        #endregion

        #region Load
        public void InitLayout ( ABCView view , XmlNode gridNode )
        {
            OwnerView=view;
            this.DefaultView.ABCGridControl=this;
            this.DefaultView.TableName=this.TableName;
            this.DefaultView.ColumnConfigs=new BindingList<ABCGridColumn.ColumnConfig>();

            foreach ( XmlNode nodeCol in gridNode.SelectNodes( "CL" ) )
            {
                ABCGridColumn.ColumnConfig colInfo=new ABCGridColumn.ColumnConfig();
                ABCPresentHelper.DeSerialization( colInfo , nodeCol );
                this.DefaultView.ColumnConfigs.Add( colInfo );
            }

            this.DefaultView.InitColumns();
            LoadDataSourceFromScript();

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

            #region InitColumns

            #region BindingList<ABCGridColumn.ColumnInfo>
            this.DefaultView.ColumnConfigs=new BindingList<ABCGridColumn.ColumnConfig>();

            int iIndex=-1;
            List<DataConfigProvider.FieldConfig> lstConfigs=DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values.OrderBy( x => x.SortOrder ).ToList();
            foreach ( DataConfigProvider.FieldConfig colAlias in lstConfigs )
            {
                if ( colAlias.InUse==false )
                    continue;

                if ( DataStructureProvider.IsPrimaryKey( strTableName , colAlias.FieldName ))
                    continue;

                if ( colAlias.FieldName=="ABCStatus"||colAlias.FieldName=="CreateTime"||colAlias.FieldName=="UpdateTime"
                    ||colAlias.FieldName=="CreateUser"||colAlias.FieldName=="UpdateUser" )
                    continue;
                iIndex++;

                ABCGridColumn.ColumnConfig colInfo=new ABCGridColumn.ColumnConfig();
                colInfo.FieldName=colAlias.FieldName;
                colInfo.TableName=this.TableName;
                colInfo.Visible=colAlias.IsDefault;
                colInfo.VisibleIndex=iIndex;
                colAlias.SortOrder=iIndex;

                colInfo.GroupBy=colAlias.IsGrouping;
                colInfo.IsUseAlias=true;

                String strColumnDataType=DataStructureProvider.GetColumnDataType( this.TableName , colInfo.FieldName ).ToLower();
                if ( strColumnDataType.Contains( "float" )||
                    ( strColumnDataType.Contains( "Guid" )
                    &&DataStructureProvider.IsForeignKey( this.TableName , colInfo.FieldName )==false
                    &&DataStructureProvider.IsPrimaryKey( this.TableName , colInfo.FieldName )==false ) )
                    colInfo.SumType=ABCSummaryType.SUM;

                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    colInfo.Caption=colAlias.CaptionVN;
                else
                    colInfo.Caption=colAlias.CaptionEN;

                this.DefaultView.ColumnConfigs.Add( colInfo );
            }
         
            #endregion

            this.DefaultView.InitColumns();

            #endregion

            this.DefaultView.BestFitColumns();
            this.ShowViewCaption=true;
            this.ViewCaption=DataConfigProvider.GetTableCaption( strTableName );
            this.ShowAutoFilterRow=true;

        }

        public void Initialize ( String strTableName )
        {
            ABCBindingInfo bindIfo=new ABCBindingInfo();
            bindIfo.TableName=strTableName;
            this.DefaultView.TableName=strTableName;
            Initialize( null , bindIfo );
        }
        #endregion

        void gridView_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete&&this.ShowDeleteButton )
            {
                if ( this.DefaultView.SelectedRowsCount>0 )
                    DoMenuClick( "Delete" );
            }
        }

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
                        this.GridDataSource=ds.Tables[0];
                        this.RefreshDataSource();

                    }
                }
            }
        }
        #endregion

        #region Row Detail
        ABCGridRowDetail RowDetail;
        public void ShowRowDetail ( )
        {
            if ( RowDetail==null||RowDetail.IsDisposed )
                RowDetail=new ABCGridRowDetail( this );
            RowDetail.Show();
        }
        #endregion

        #region RunLink

        public delegate void ABCGridRunLinkEventHandler ( object sender , ABCGridColumn column , int iRowHandle );
        public event ABCGridRunLinkEventHandler RunLinkEvent;
        public virtual void OnRunLinkEvent ( ABCGridColumn column , int iRowHandle )
        {
            if ( this.RunLinkEvent!=null )
                this.RunLinkEvent( this , column , iRowHandle );
        }

        #endregion

        #region Context Menu

        #region Init

        public class ABCGridViewMenuInfo
        {
            public ABCGridViewMenuInfo ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hit , object _objectCell , object _objectRow )
            {
                this.HitInfo=hit;
                this.CelllData=_objectCell;
                this.RowData=_objectRow;
            }
            public DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo HitInfo;
            public object CelllData;
            public object RowData;
        }
        public class ABCGridViewColumnButtonMenu : GridViewMenu
        {
            public ABCGridViewColumnButtonMenu ( DevExpress.XtraGrid.Views.Grid.GridView view ) : base( view ) { }

            public void AddItem ( DXMenuItem MenuItem )
            {
                Items.Add( MenuItem );
            }
            public DXMenuItem AddItem ( String strCaption , object tag , Image img )
            {
                Items.Add( CreateMenuItem( strCaption , img , tag , true ) );
                return Items[Items.Count-1];
            }
            public DXMenuItem AddItem ( DXSubMenuItem parent , String strCaption , object tag , Image img )
            {
                parent.Items.Add( CreateMenuItem( strCaption , img , tag , true ) );
                return parent.Items[parent.Items.Count-1];
            }

            public DXSubMenuItem AddSubMenu ( String strCaption , object tag , Image img )
            {
                Items.Add( CreateSubMenuItem( strCaption , img , tag , true , false ) );
                return Items[Items.Count-1] as DXSubMenuItem;
            }
            public DXSubMenuItem AddSubSubMenu ( DXSubMenuItem parent , String strCaption , object tag , Image img )
            {
                parent.Items.Add( CreateSubMenuItem( strCaption , img , tag , true , false ) );
                return parent.Items[parent.Items.Count-1] as DXSubMenuItem;
            }
        }

        public ABCGridViewColumnButtonMenu PopupMenu=null;

        public virtual DXMenuItem PopupMenuAddItem ( String strCaption , object tag , Image img , bool isBeginGroup )
        {
            DXMenuItem menuItem=PopupMenu.AddItem( strCaption , tag , img );
            menuItem.BeginGroup=isBeginGroup;
            menuItem.Click+=new EventHandler( PopupMenu_Click );
            return menuItem;
        }
        public virtual DXMenuItem PopupMenuAddItem ( DXSubMenuItem parent , String strCaption , object tag , Image img , bool isBeginGroup )
        {
            DXMenuItem menuItem=PopupMenu.AddItem( parent , strCaption , tag , img );
            menuItem.BeginGroup=isBeginGroup;
            menuItem.Click+=new EventHandler( PopupMenu_Click );
            return menuItem;
        }

        public virtual void InitCellPopupMenu ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=(DevExpress.XtraGrid.Views.Grid.GridView)this.DefaultView;
            if ( PopupMenu==null )
                PopupMenu=new ABCGridViewColumnButtonMenu( view );
            PopupMenu.Items.Clear();

            InitRowPopupMenu( hi );

        }
        public virtual void InitRowPopupMenu ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=(DevExpress.XtraGrid.Views.Grid.GridView)this.DefaultView;
            if ( PopupMenu==null )
                PopupMenu=new ABCGridViewColumnButtonMenu( view );
            PopupMenu.Items.Clear();

            OnCustomContextMenu( hi );

            if ( PopupMenu.Tag is ABCGridViewMenuInfo )
            {
                BusinessObject objBus=null;
                if ( ( PopupMenu.Tag as ABCGridViewMenuInfo ).RowData is DataRowView )
                {
                    BusinessObjectController ctrller=BusinessControllerFactory.GetBusinessController( this.TableName );
                    if ( ctrller!=null )
                        objBus=ctrller.GetObjectFromDataRow( ( ( PopupMenu.Tag as ABCGridViewMenuInfo ).RowData as DataRowView ).Row );
                }
                else
                    objBus=( PopupMenu.Tag as ABCGridViewMenuInfo ).RowData as BusinessObject;

                if ( objBus!=null&&DataStructureProvider.IsExistedTable( objBus.AATableName ) )
                {
                    if ( ABCPresentHelper.IsMainObject( objBus.AATableName ) )
                        PopupMenuAddItem( "Chi tiết" , "DETAIL" , ABCImageList.GetImage16x16( "DocLink" ) , false );

                    #region Related Information
                    List<String> lstFKs=new List<string>();
                    foreach ( String strFK in DataStructureProvider.DataTablesList[objBus.AATableName].ForeignColumnsList.Keys )
                    {
                        String strTableName=DataStructureProvider.GetTableNameOfForeignKey( objBus.AATableName , strFK );
                        if ( ABCPresentHelper.IsMainObject( strTableName )&&( this.OwnerView==null||strTableName!=this.OwnerView.MainTableName ) )
                        {

                            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objBus , strFK );
                            Guid iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                            if ( iLinkID!=Guid.Empty )
                                lstFKs.Add( strFK );
                        }
                    }

                    if ( lstFKs.Count>0 )
                    {
                        DXSubMenuItem subMenu=PopupMenu.AddSubMenu( "Thông tin liên quan" , null , ABCImageList.GetImage16x16( "Info" ) );
                        if ( subMenu!=null )
                        {
                            foreach ( String strFK in lstFKs )
                            {
                                String strCaption=DataConfigProvider.GetFieldCaption( objBus.AATableName , strFK );
                                String strTableName=DataStructureProvider.GetTableNameOfForeignKey( objBus.AATableName , strFK );
                                if ( String.IsNullOrWhiteSpace( strCaption )||strCaption==strFK )
                                    strCaption=DataConfigProvider.GetTableCaption( strTableName );

                                if ( String.IsNullOrWhiteSpace( strCaption )==false&&strCaption!=strFK )
                                {
                                    object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objBus , strFK );
                                    if ( objValue!=null )
                                    {
                                        objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( objBus.AATableName , ABCHelper.DataConverter.ConvertToGuid( objValue ) , strFK );
                                        if ( objValue!=null&&objValue!=DBNull.Value )
                                            strCaption=strCaption+" : "+objValue.ToString();

                                        PopupMenuAddItem( subMenu , strCaption , "INFO-"+strFK , ABCImageList.GetImage16x16( "View" ) , false );
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                }
                if ( ShowRefreshButton&&ShowMenuBar )
                    PopupMenuAddItem( "Làm mới" , "REFRESH" , ABCImageList.GetImage16x16( "Refresh" ) , true );

                if ( ShowDeleteButton&&ShowMenuBar )
                    PopupMenuAddItem( "Xóa" , "DELETE" , ABCImageList.GetImage16x16( "Delete" ) , false );

            }


            PopupMenuAddItem( "Xuất ra Excel" , "EXPORT" , ABCImageList.GetImage16x16( "Export" ) , false );

            PopupMenuAddItem( "In" , "PRINT" , ABCImageList.GetImage16x16( "Print" ) , false );

          
        }

        #region Events
        public delegate void ABCCustomContextMenuEventHandler ( ABCGridViewColumnButtonMenu popupMenu , DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi );
        public delegate void ABCMenuItemClickEventHandler (DXMenuItem itemMenu, BusinessObject obj );
        public event ABCCustomContextMenuEventHandler ABCCustomContextMenu;
        public event ABCMenuItemClickEventHandler ABCMenuItemClick;

        public virtual void OnCustomContextMenu ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi )
        {
            if ( this.ABCCustomContextMenu!=null )
                this.ABCCustomContextMenu( PopupMenu , hi );
        }
        public virtual void OnMenuItemClick ( DXMenuItem itemMenu , BusinessObject obj )
        {
            if ( this.ABCMenuItemClick!=null )
                this.ABCMenuItemClick( itemMenu , obj );
        }

        #endregion

        #endregion

        #region Show Context Menu
        protected void GridControl_MouseUp ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right )
                DoShowPopupMenu( ( (DevExpress.XtraGrid.Views.Grid.GridView)this.DefaultView ).CalcHitInfo( new Point( e.X , e.Y ) ) );
        }

        public void DoShowPopupMenu ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=(DevExpress.XtraGrid.Views.Grid.GridView)this.DefaultView;

            #region CellPopupMenu
            if ( hi.HitTest==DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell )
            {
                object cellData=view.GetRowCellValue( hi.RowHandle , hi.Column );
                object rowData=view.GetRow( hi.RowHandle );

                if ( PopupMenu==null )
                    PopupMenu=new ABCGridViewColumnButtonMenu( view );

                ABCGridViewMenuInfo info=new ABCGridViewMenuInfo( hi , cellData , rowData );
                PopupMenu.Tag=info;

                InitCellPopupMenu( hi );

                PopupMenu.Show( hi.HitPoint );

            }
            #endregion

            #region ABCPopupMenu
            if ( hi.HitTest==DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.Row&&PopupMenu!=null )
            {
                object rowData=view.GetRow( hi.RowHandle );

                if ( PopupMenu==null )
                    PopupMenu=new ABCGridViewColumnButtonMenu( view );

                ABCGridViewMenuInfo info=new ABCGridViewMenuInfo( hi , null , rowData );
                PopupMenu.Tag=info;

                InitRowPopupMenu( hi );

                PopupMenu.Show( hi.HitPoint );

            }
            #endregion
        }

        #endregion

        #region Default Events
        public virtual void PopupMenu_Click ( object sender , EventArgs e )
        {

            if ( PopupMenu.Tag is ABCGridViewMenuInfo==false )
                return;

            DevExpress.XtraGrid.Views.Grid.GridView gridView=(DevExpress.XtraGrid.Views.Grid.GridView)this.DefaultView;
            DXMenuItem item=sender as DXMenuItem;
            ABCGridViewMenuInfo menuInfo=(ABCGridViewMenuInfo)PopupMenu.Tag;

            BusinessObject objBus=null;
            if ( menuInfo.RowData is DataRowView )
            {
                BusinessObjectController ctrller=BusinessControllerFactory.GetBusinessController( this.TableName );
                if ( ctrller!=null )
                    objBus=ctrller.GetObjectFromDataRow( ( menuInfo.RowData as DataRowView ).Row );
            }
            else
                objBus=menuInfo.RowData as BusinessObject;

            OnMenuItemClick( item , objBus );

            if ( item.Tag.ToString().Equals( "DETAIL" )&&menuInfo.RowData!=null )
                ActionViewDetail( objBus );

            if ( item.Tag.ToString().StartsWith( "INFO-" ) )
            {
                String strFK=item.Tag.ToString().Substring( 5 , item.Tag.ToString().Length-5 );
                ActionViewInfo( objBus , strFK );
            }

            if ( item.Tag.ToString().Equals( "SAVE" ) )
                DoMenuClick( "Save" );

            if ( item.Tag.ToString().Equals( "DELETE" ) )
                DoMenuClick( "Delete" );

            if ( item.Tag.ToString().Equals( "REFRESH" ) )
                DoMenuClick( "Refresh" );

            if ( item.Tag.ToString().Equals( "EXPORT" ) )
                DoMenuClick( "Export" );

            if ( item.Tag.ToString().Equals( "PRINT" ) )
                DoMenuClick( "Print" );
        }


        public virtual void ActionViewDetail ( BusinessObject objBus )
        {
            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objBus , DataStructureProvider.GetPrimaryKeyColumn( this.TableName ) );
            if ( objValue==null||( objValue.GetType()!=typeof( Guid )&&objValue.GetType()!=typeof( Nullable<Guid> ) ) )
                return;

            Guid iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
            if ( iLinkID!=Guid.Empty)
            {
                if ( this.OwnerView!=null )
                     ABCScreen.ABCScreenHelper.Instance.RunLink( this.TableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                else
                     ABCScreen.ABCScreenHelper.Instance.RunLink( this.TableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );
            }
        }
        public virtual void ActionViewInfo ( BusinessObject objBus , String strFK )
        {
            if ( objBus==null )
                return;

            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objBus , strFK );
            Guid iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
            if ( iLinkID!=Guid.Empty )
            {
                String strTableName=DataStructureProvider.GetTableNameOfForeignKey( objBus.AATableName , strFK );
                if ( this.OwnerView!=null )
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strTableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                else
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );
            }

        }

        #endregion


        #endregion

        public void RefreshDataSource ( )
        {
            if ( this.InnerGrid!=null )
                this.InnerGrid.RefreshDataSource();
        }

    }

    public class ABCGridControlDesigner : DevExpress.XtraEditors.Design.BaseEditDesigner
    {
        public ABCGridControlDesigner ( )
        {
        }

        public override IList SnapLines
        {
            get
            {

                ArrayList snapLines=base.SnapLines as ArrayList;
                snapLines.Add( new SnapLine( SnapLineType.Baseline , 0 , SnapLinePriority.Medium ) );
                snapLines.Add( new SnapLine( SnapLineType.Bottom , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Horizontal , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Left , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Right , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Top , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Vertical , 0 , SnapLinePriority.High ) );
                return snapLines;
            }
        }
        public override bool ParticipatesWithSnapLines
        {
            get
            {
                return true;
            }
        }
    }

    public class ABCBaseGridControl : GridControl
    {

        protected override BaseView CreateDefaultView ( )
        {
            return CreateView( "ABCGridView" );
        }
        protected override void RegisterAvailableViewsCore ( InfoCollection collection )
        {
            base.RegisterAvailableViewsCore( collection );
            collection.Add( new ABCGridViewInfoRegistrator() );
        }
    }

    #region ABCGridView
    public class ABCGridViewInfoRegistrator : GridInfoRegistrator
    {
        public override string ViewName { get { return "ABCGridView"; } }
        public override BaseView CreateView ( GridControl grid ) { return new ABCGridView( grid as ABCBaseGridControl ); }
        public override BaseViewInfo CreateViewInfo ( BaseView view )
        {
            return new ABCGridViewInfo( view as ABCGridView );
        }
        public override BaseViewHandler CreateHandler ( BaseView view )
        {
            return new ABCGridHandler( view as ABCGridView );
        }

    }
    public class ABCGridHandler : DevExpress.XtraGrid.Views.Grid.Handler.GridHandler
    {
        public ABCGridHandler ( ABCGridView gridView ) : base( gridView ) { }
    }
    public class ABCGridViewInfo : GridViewInfo
    {
        ABCGridView GridView;
        public ABCGridViewInfo ( ABCGridView gridView )
            : base( gridView )
        {
            GridView=gridView;
        }
        public override int CalcColumnBestWidth ( GridColumn column )
        {
            int iWidth=base.CalcColumnBestWidth( column );
            if ( ( column.View as ABCGridView ).IsNeedDrawLink( column ) )
                iWidth+=15;
            return iWidth;
        }
    }

    public class ABCGridView : DevExpress.XtraGrid.Views.Grid.GridView , IABCGridView
    {
        public IABCGridControl ABCGridControl { get; set; }
        public String TableName { get; set; }
        public BindingList<ABCGridColumn.ColumnConfig> ColumnConfigs=new BindingList<ABCGridColumn.ColumnConfig>();

        protected override string ViewName { get { return "ABCGridView"; } }
        protected override GridColumnCollection CreateColumnCollection ( )
        {
            return new ABCGridColumnCollection( this );
        }
        public ABCGridView ( )
        {
            Initialize();
        }

        public ABCGridView ( ABCBaseGridControl grid )
            : base( grid )
        {
            Initialize();

        }

        public ABCGridView ( ABCGridControl grid )
        {
            ABCGridControl=grid;
            TableName=grid.TableName;
            Initialize();

        }
        public ABCGridView ( String strTableName )
        {
            TableName=strTableName;
            Initialize();

        }

        private void Initialize ( )
        {
            this.OptionsView.ShowGroupPanel=false;
            //      this.OptionsFilter.AllowFilterEditor=false;

            this.OptionsView.ColumnAutoWidth=false;
            this.OptionsView.ShowAutoFilterRow=false;
            this.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.ActiveFilterEnabled=false;


            this.GroupFooterShowMode=GroupFooterShowMode.VisibleIfExpanded;
            this.GroupFormat="[#image]{1} {2}";
            this.Appearance.GroupRow.BackColor=Color.FromArgb( 70 , 90 , 125 );
            this.Appearance.GroupRow.ForeColor=Color.White;
            this.Appearance.GroupRow.Font=new Font( this.Appearance.GroupRow.Font , FontStyle.Bold );
            this.Appearance.GroupButton.BackColor=Color.White;

            this.Appearance.Empty.BackColor=System.Drawing.Color.FromArgb( 181 , 200 , 223 );
            this.Appearance.Empty.Options.UseBackColor=true;
            this.Appearance.FocusedRow.BackColor=Color.FromArgb( 112 , 146 , 190 );
            this.Appearance.FocusedRow.Options.UseBackColor=true;
            this.Appearance.FocusedCell.BackColor=Color.FromArgb( 255 , 209 , 120 );
            this.Appearance.FocusedCell.Options.UseBackColor=true;

            this.CustomColumnDisplayText+=new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler( ABCGridView_CustomColumnDisplayText );
            this.CustomUnboundColumnData+=new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler( ABCGridView_CustomUnboundColumnData );
            //     this.CustomColumnGroup+=new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler( ABCGridView_CustomColumnGroup );
            //   this.CustomColumnSort+=new CustomColumnSortEventHandler( ABCGridView_CustomColumnSort );
            this.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( GridView_CustomDrawCell );
            this.RowCellClick+=new RowCellClickEventHandler( GridView_RowCellClick );
            this.MouseMove+=new MouseEventHandler( GridView_MouseMove );
            
        }

     
        void ABCGridView_CustomColumnSort ( object sender , CustomColumnSortEventArgs e )
        {
            if ( e.Value1!=e.Value2 )
            {
                if ( e.Column.FieldName.Contains( ":" ) )
                {
                    ABCGridColumn gridCol=e.Column as ABCGridColumn;
                    String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                    if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField ) )
                    {
                        object objValue1=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value1 ) , gridCol.Config.FieldName );
                        object objValue2=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value2 ) , gridCol.Config.FieldName );
                        if ( objValue1==objValue2||( objValue1!=null&&objValue2!=null&&objValue1.ToString()==objValue2.ToString() ) )
                        {
                            e.Handled=true;
                            e.Result=0;
                        }
                    }
                }
            }
        }

        void ABCGridView_CustomColumnGroup ( object sender , DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e )
        {
            if ( e.Value1!=e.Value2 )
            {
                if ( e.Column.FieldName.Contains( ":" ) )
                {
                    ABCGridColumn gridCol=e.Column as ABCGridColumn;
                    String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                    if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField ) )
                    {
                        object objValue1=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value1 ) , gridCol.Config.FieldName );
                        object objValue2=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value2 ) , gridCol.Config.FieldName );
                        if ( objValue1==objValue2||( objValue1!=null&&objValue2!=null&&objValue1.ToString()==objValue2.ToString() ) )
                        {
                            e.Handled=true;
                            e.Result=0;
                        }
                    }
                }
            }
        }


        protected override DevExpress.Data.Filtering.CriteriaOperator CreateAutoFilterCriterion ( GridColumn column , AutoFilterCondition condition , object _value , string strVal )
        {
            if ( condition==AutoFilterCondition.Like )
                condition=AutoFilterCondition.Contains;
            return base.CreateAutoFilterCriterion( column , condition , _value , strVal );
        }

        #region Cell

        #region Accross Table DisplayText
        void ABCGridView_CustomUnboundColumnData ( object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e )
        {
            if ( e.Column==null||e.Column is ABCGridColumn==false||this.ABCGridControl==null )
                return;

            //if ( this.ABCGridControl.OwnerView==null||this.ABCGridControl.OwnerView.Mode==ViewMode.Design )
            //    return;

            if ( e.RowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle )
                return;

            ABCGridColumn gridCol=e.Column as ABCGridColumn;
            if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false&&gridCol.Config.FieldName.Contains( ":" ) )
            {
                String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField )||strBaseField=="ID" )
                {
                    if ( e.IsGetData )
                    {
                        object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( e.Row as BusinessObject , strBaseField );
                        e.Value=objValue;
                    }
                    else
                    {
                        ABCBusinessEntities.ABCDynamicInvoker.SetValue( e.Row as BusinessObject , strBaseField , ABCHelper.DataConverter.ConvertToGuid( e.Value ) );
                    }
                }
            }
        }

        void ABCGridView_CustomColumnDisplayText ( object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e )
        {
            if ( e.Column==null||e.Column is ABCGridColumn==false||this.ABCGridControl==null )
                return;

            //if ( this.ABCGridControl.OwnerView==null||this.ABCGridControl.OwnerView.Mode==ViewMode.Design )
            //    return;

            if ( e.RowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle||e.Value==null||e.Value==DBNull.Value )
                return;

            ABCGridColumn gridCol=e.Column as ABCGridColumn;
            if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false )
            {
                String strBaseField=gridCol.Config.FieldName;
                if ( gridCol.Config.FieldName.Contains( ":" ) )
                    strBaseField=gridCol.Config.FieldName.Split( ':' )[0];

                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField )||strBaseField=="ID" )
                {
                    String strDisplayField=gridCol.Config.FieldName;
                    if ( strDisplayField.Contains( ":" )==false )
                    {
                        String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( gridCol.Config.TableName , strBaseField );
                        strDisplayField=gridCol.Config.FieldName+":"+DataStructureProvider.GetDisplayColumn( strPKTableName );
                    }
                    object objValue=null;
                    if ( strBaseField=="ID" )
                    {
                        object objIDTableName=this.ABCGridControl.GridDefaultView.GetRowCellValue( e.RowHandle , "TableName" );
                        if ( objIDTableName!=null )
                            objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value ) , strDisplayField , objIDTableName.ToString() );
                    }

                    if ( objValue==null )
                        objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value ) , strDisplayField );

                    e.DisplayText=DataFormatProvider.DoFormat( objValue , gridCol.Config.TableName , strDisplayField );

                }


                if ( gridCol.Config.FieldName.ToLower()=="tablename"&&!gridCol.Config.TableName.StartsWith( "ST" ) )
                    e.DisplayText=DataConfigProvider.GetTableCaption( e.Value.ToString() );
                if ( gridCol.Config.FieldName.ToLower()=="fieldname"&&!gridCol.Config.TableName.StartsWith( "ST" ) )
                {
                    object objIDTableName=this.ABCGridControl.GridDefaultView.GetRowCellValue( e.RowHandle , "TableName" );
                    if ( objIDTableName!=null )
                        e.DisplayText=DataConfigProvider.GetFieldCaption( objIDTableName.ToString() , e.Value.ToString() );
                }

                if ( String.IsNullOrWhiteSpace( e.DisplayText ) )
                    e.DisplayText=e.Value.ToString();
            }

            if ( gridCol.AllowView==false )
                e.DisplayText="<Dữ liệu ẩn>";

            if ( e.Value is bool )
            {
                if ( Convert.ToBoolean( e.Value )==false )
                    e.DisplayText="";
                else
                    e.DisplayText="x";
            }

            if ( e.Value is int||e.Value is Nullable<int>||e.Value is double||e.Value is Nullable<double>||e.Value is decimal||e.Value is Nullable<decimal> )
            {
                try
                {
                    if ( Convert.ToDouble( e.Value )==0 )
                        e.DisplayText="";
                }
                catch ( Exception ex )
                {
                }
            }


            Guid id=Guid.Empty;
            if ( Guid.TryParse( e.DisplayText , out id ) )
                e.DisplayText=String.Empty;

        }

        #endregion

        public void RunFocusedLink ( )
        {
            if ( this.FocusedRowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle )
                return;

            ABCGridColumn gridCol=this.FocusedColumn as ABCGridColumn;
            if ( gridCol==null||this.ABCGridControl==null )
                return;

            ( this.ABCGridControl as ABCGridControl ).OnRunLinkEvent( gridCol , FocusedRowHandle );

            object objValue=this.GetFocusedRowCellValue( this.FocusedColumn );
            if ( objValue==null||objValue==DBNull.Value )
                return;

            String strTableName=this.TableName;
            if ( gridCol.Config!=null )
                strTableName=gridCol.Config.TableName;

            String strLinkTableName=String.Empty;
            Guid iLinkID=Guid.Empty;
            if ( gridCol.Config!=null&&gridCol.Config.FieldName=="ID" )
            {
                #region ID
                iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                if ( iLinkID==Guid.Empty )
                    return;

                object objDataRow=this.GetRow( this.FocusedRowHandle );
                if ( objDataRow==null )
                    return;

                PropertyInfo proInfo=objDataRow.GetType().GetProperty( "TableName" );
                if ( proInfo==null )
                    return;
                objValue=proInfo.GetValue( objDataRow , null );
                if ( objValue==null||objValue.GetType()!=typeof( String ) )
                    return;
                strLinkTableName=objValue.ToString();
                #endregion
            }
            else
            {
                if ( gridCol.Config!=null&&gridCol.Config.FieldName.Contains( ":" ) )
                {
                    #region Contain ":"
                    object objDataRow=this.GetRow( this.FocusedRowHandle );
                    if ( objDataRow==null )
                        return;

                    String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                    PropertyInfo proInfo=objDataRow.GetType().GetProperty( strBaseField );
                    if ( proInfo==null )
                        return;
                    objValue=proInfo.GetValue( objDataRow , null );
                    if ( objValue==null||objValue.GetType()!=typeof( Guid ) )
                        return;

                    DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( objDataRow as BusinessObject , ABCHelper.DataConverter.ConvertToGuid( objValue ) , gridCol.Config.FieldName );
                    if ( acrrosInfo!=null&&( DataStructureProvider.IsPrimaryKey( acrrosInfo.TableName , acrrosInfo.FieldName )
                                                  ||DataStructureProvider.IsNOColumn( acrrosInfo.TableName , acrrosInfo.FieldName )
                                                  ||DataStructureProvider.IsNAMEColumn( acrrosInfo.TableName , acrrosInfo.FieldName ) ) )
                    {
                        if ( this.ABCGridControl!=null&&this.ABCGridControl.OwnerView!=null&&this.ABCGridControl.OwnerView.DataField!=null
                              &&this.ABCGridControl.OwnerView.DataField.MainTableName!=gridCol.Config.TableName )
                        {
                            strLinkTableName=acrrosInfo.TableName;
                            iLinkID=acrrosInfo.TableID;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Without ":"
                    if ( DataStructureProvider.IsForeignKey( strTableName , gridCol.FieldName ) )
                    {
                        strLinkTableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , gridCol.FieldName );
                        iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                    }
                    else if ( DataStructureProvider.IsPrimaryKey( strTableName , gridCol.FieldName )
                             ||DataStructureProvider.IsNOColumn( strTableName , gridCol.FieldName )
                             ||DataStructureProvider.IsNAMEColumn( strTableName , gridCol.FieldName ) )
                    {
                        strLinkTableName=strTableName;
                        object objDataRow=this.GetRow( this.FocusedRowHandle );
                        if ( objDataRow==null )
                            return;

                        if ( this.ABCGridControl.OwnerView!=null&&this.ABCGridControl.OwnerView.DataField!=null
                           &&this.ABCGridControl.OwnerView.DataField.MainTableName==gridCol.Config.TableName )
                            return;

                        PropertyInfo proInfo=objDataRow.GetType().GetProperty( DataStructureProvider.GetPrimaryKeyColumn( strTableName ) );
                        if ( proInfo==null )
                            return;

                        objValue=proInfo.GetValue( objDataRow , null );
                        if ( objValue==null||objValue.GetType()!=typeof( Guid ) )
                            return;

                        iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                    }
                    #endregion

                }
            }
            if ( iLinkID!=Guid.Empty )
            {
                if ( this.ABCGridControl!=null&&this.ABCGridControl.OwnerView!=null )
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , this.ABCGridControl.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                else
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );
            }

        }

        #region Cell Click
        void GridView_MouseMove ( object sender , MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo hitInfo=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( hitInfo==null||hitInfo.InRowCell==false||hitInfo.Column==null||hitInfo.RowHandle<0||hitInfo.RowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle )
                return;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo=(DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)view.GetViewInfo();
            if ( viewInfo==null )
                return;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cell=viewInfo.GetGridCellInfo( hitInfo.RowHandle , hitInfo.Column );
            if ( cell==null )
                return;

            if ( 0<e.Location.X-cell.Bounds.Left&&e.Location.X-cell.Bounds.Left<18 )
            {
                Boolean isDraw=false;
                if ( ForeignColumnList.TryGetValue( hitInfo.Column , out isDraw ) )
                {
                    if ( !isDraw )
                    {
                        ToolTipController.DefaultController.HideHint();
                        return;
                    }
                }

                String strDesc=String.Empty;

                ABCGridColumn gridCol=hitInfo.Column as ABCGridColumn;
                if (gridCol!=null&& gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false )
                {
                    String strBaseField=gridCol.Config.FieldName;
                    if ( gridCol.Config.FieldName.Contains( ":" ) )
                        strBaseField=gridCol.Config.FieldName.Split( ':' )[0];

                    if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField ) )
                    {
                        String strDisplayField=gridCol.Config.FieldName;
                        if ( strDisplayField.Contains( ":" )==false )
                        {
                            String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( gridCol.Config.TableName , strBaseField );
                            String strNameCol=DataStructureProvider.GetNAMEColumn( strPKTableName );
                            if ( String.IsNullOrWhiteSpace( strNameCol )==false )
                                strDisplayField=gridCol.Config.FieldName+":"+strNameCol;
                            else
                            {
                                if ( DataStructureProvider.IsTableColumn( strPKTableName , "Remark" ) )
                                    strDisplayField=gridCol.Config.FieldName+":Remark";
                            }
                        }
                        if ( String.IsNullOrWhiteSpace( strDisplayField )==false&&cell.CellValue!=DBNull.Value )
                        {
                            object objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( cell.CellValue ) , strDisplayField );
                            strDesc=DataFormatProvider.DoFormat( objValue , gridCol.Config.TableName , strDisplayField );
                        }
                    }
                }

                if ( String.IsNullOrWhiteSpace( strDesc )==false&&strDesc!=view.GetDisplayTextByColumnValue( hitInfo.Column , cell.CellValue ) )
                    ToolTipController.DefaultController.ShowHint( strDesc , this.GridControl.PointToScreen( new Point( e.X+15 , e.Y ) ) );
                else
                    ToolTipController.DefaultController.HideHint();
            }
            else
                ToolTipController.DefaultController.HideHint();
        }

        void GridView_RowCellClick ( object sender , RowCellClickEventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo info=(DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)this.GetViewInfo();
            if ( info==null )
                return;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cell=info.GetGridCellInfo( e.RowHandle , e.Column );
            if ( cell==null )
                return;

            if ( e.Button==System.Windows.Forms.MouseButtons.Left&&0<e.Location.X-cell.Bounds.Left&&e.Location.X-cell.Bounds.Left<18 )
            {
                Boolean isDraw=false;
                if ( ForeignColumnList.TryGetValue( e.Column , out isDraw ) )
                {
                    if ( !isDraw )
                        return;
                }
                RunFocusedLink();
                e.Handled=true;
            }
        }

        Dictionary<GridColumn , bool> ForeignColumnList=new Dictionary<GridColumn , bool>();
        
        public bool IsNeedDrawLink ( GridColumn col )
        {
            Boolean isDraw=false;
            // ForeignColumnList.Clear();
            if ( ForeignColumnList.TryGetValue( col , out isDraw ) )
                return isDraw;

        
            if ( ( col is ABCGridColumn==false )||( col as ABCGridColumn ).Config==null||String.IsNullOrWhiteSpace( ( col as ABCGridColumn ).Config.FieldName ) )
            {
                if ( col.FieldName=="ID" )
                {
                    if ( DataStructureProvider.IsTableColumn( this.TableName , "TableName" ) )
                        isDraw=true;
                }
       
                #region In GridLookupEdit
                if ( DataStructureProvider.IsForeignKey( this.TableName , col.FieldName ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , col.FieldName );
                    if ( ABCPresentHelper.IsMainObject( strPKTableName ) )
                        isDraw=true;
                }
                #endregion
            }
            else
            {
                ABCGridColumn gridCol=col as ABCGridColumn;
                if ( col.FieldName=="ID" )
                {
                    if ( DataStructureProvider.IsTableColumn( gridCol.Config.TableName , "TableName" ) )
                        isDraw=true;
                }
                else
                {
                    if ( gridCol.Config.FieldName.Contains( ":" )==false )
                    {
                        #region Without ":"
                        String strTableName=String.Empty;
                        if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , col.FieldName ) )
                        {
                            strTableName=DataStructureProvider.GetTableNameOfForeignKey( gridCol.Config.TableName , col.FieldName );
                        }
                        else if ( DataStructureProvider.IsPrimaryKey( gridCol.Config.TableName , col.FieldName )
                            ||DataStructureProvider.IsNOColumn( gridCol.Config.TableName , col.FieldName )
                            ||DataStructureProvider.IsNAMEColumn( gridCol.Config.TableName , col.FieldName ) )
                        {
                            if ( this.ABCGridControl!=null&&this.ABCGridControl.OwnerView!=null&&this.ABCGridControl.OwnerView.DataField!=null
                              &&this.ABCGridControl.OwnerView.DataField.MainTableName!=gridCol.Config.TableName )
                            {
                                strTableName=gridCol.Config.TableName;
                            }
                        }
                        if ( String.IsNullOrWhiteSpace( strTableName )==false )
                        {
                            if ( ABCPresentHelper.IsMainObject( strTableName ) )
                                isDraw=true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Contains ":"

                        String[] strArray=gridCol.Config.FieldName.Split(':');                        
                        DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( gridCol.Config.TableName , gridCol.Config.FieldName );
                        if ( acrrosInfo!=null )
                        {
                            if ( acrrosInfo.FieldName=="ID"&&strArray[strArray.Length-1].StartsWith("FK_"))
                                isDraw=true;
                            else if ( DataStructureProvider.IsPrimaryKey( acrrosInfo.TableName , acrrosInfo.FieldName )
                                                              ||DataStructureProvider.IsNOColumn( acrrosInfo.TableName , acrrosInfo.FieldName )
                                                     ||DataStructureProvider.IsNAMEColumn( acrrosInfo.TableName , acrrosInfo.FieldName )
                                                       ||DataStructureProvider.IsDisplayColumn( acrrosInfo.TableName , acrrosInfo.FieldName ) )
                            {
                                if ( ABCPresentHelper.IsMainObject( acrrosInfo.TableName ) )
                                    isDraw=true;
                            }
                        }
                        #endregion
                    }
                }
            }

            ForeignColumnList.Add( col , isDraw );
            return isDraw;
        }

        private void DrawButton ( DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {

            if ( e.Column.OptionsColumn.AllowEdit==false )
            {
                SolidBrush brush=new SolidBrush( e.Appearance.BackColor );
                e.Graphics.FillRectangle( brush , ( e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo ).Bounds );
            }

            e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "DocLink" ) , e.Bounds.Location );

            Rectangle r=e.Bounds;
            r.Width-=18;
            r.X+=18;
            e.Appearance.DrawString( e.Cache , e.DisplayText , r );

            e.Handled=true;
        }

        void GridView_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            //if ( this.ABCGridControl!=null&&this.ABCGridControl.OwnerView!=null&&this.ABCGridControl.OwnerView.Mode==ViewMode.Design )
            //    return;

            if ( e.RowHandle<0||e.RowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle )
                return;

            if ( e.Column.OptionsColumn.AllowEdit==false ||this.OptionsBehavior.Editable==false)
            {
                e.Appearance.BackColor=Color.FromArgb( 181 , 200 , 223 );
                e.Appearance.Options.UseBackColor=true;
            }

            if ( e.CellValue==DBNull.Value||e.CellValue==null )
                return;

            Boolean isDraw=false;
            if ( ForeignColumnList.TryGetValue( e.Column , out isDraw ) )
            {
                if ( isDraw )
                    DrawButton( e );
            }
            else if ( IsNeedDrawLink( e.Column ) )
                DrawButton( e );

            if ( e.Column.OptionsColumn.AllowEdit==false )
            {
                if ( e.CellValue==null||e.CellValue==DBNull.Value||e.CellValue.ToString()=="0")
                    e.DisplayText="";
            }
        }


        #endregion

        #endregion

        public void InitColumns ( )
        {

            this.Columns.Clear();
            if ( this.ColumnConfigs==null )
                return;

        //    bool isAllNotUseAlias=true;

            SortedList<String , ABCGridColumn.ColumnConfig> lstSort=new SortedList<string , ABCGridColumn.ColumnConfig>();
            foreach ( ABCGridColumn.ColumnConfig colInfo in this.ColumnConfigs )
            {
                //if ( colInfo.IsUseAlias )
                //    isAllNotUseAlias=false;

                lstSort.Add( colInfo.VisibleIndex+colInfo.Caption+colInfo.FieldName , colInfo );
            }
       
            foreach ( ABCGridColumn.ColumnConfig colInfo in lstSort.Values )
            {
                if (this.ABCGridControl!=null &&( this.ABCGridControl.OwnerView==null||( this.ABCGridControl.OwnerView!=null&&this.ABCGridControl.OwnerView.Mode!=ViewMode.Design ) ))
                {
                    String strRealCol=colInfo.FieldName.Split( ':' )[0];
                    if ( !DataStructureProvider.IsTableColumn( this.TableName , strRealCol ) )
                        continue;

                    if ( !DataConfigProvider.TableConfigList.ContainsKey( this.TableName )||!DataConfigProvider.TableConfigList[this.TableName].FieldConfigList.ContainsKey( strRealCol ) )
                        continue;

                    if ( !DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strRealCol].InUse )
                        continue;
                }

                //if ( isAllNotUseAlias )
                //    colInfo.IsUseAlias=true;

                ABCGridColumn col=new ABCGridColumn( colInfo , this.TableName );
             //   if ( ABCGridControl!=null)
                    col.Initialize();

                if ( col.SummaryItem.SummaryType!=DevExpress.Data.SummaryItemType.None )
                {
                    this.OptionsView.ShowFooter=true;

                    DevExpress.XtraGrid.GridGroupSummaryItem item1=new DevExpress.XtraGrid.GridGroupSummaryItem();
                    item1.FieldName=col.SummaryItem.FieldName;
                    item1.SummaryType=col.SummaryItem.SummaryType;
                    item1.DisplayFormat=col.SummaryItem.DisplayFormat;
                    item1.ShowInGroupColumnFooter=col;

                    this.GroupSummary.Add( item1 );
                }
                this.Columns.Add( col );

            }
        }
    }

    #endregion

    public class ABCGridColumn : DevExpress.XtraGrid.Columns.GridColumn
    {
        public override string ColumnEditName
        {
            get
            {
                return base.ColumnEditName;
            }
            set
            {
                base.ColumnEditName=value;
            }
        }
        [Serializable]
        public class ColumnConfig
        {
            public String FieldName { get; set; }
            public String TableName { get; set; }
            public String Caption { get; set; }
            public bool IsUseAlias { get; set; }
            public int Width { get; set; }
            public bool Visible { get; set; }

      //      public ABCRepositoryType RepoType { get; set; }
            public int VisibleIndex { get; set; }
            public DevExpress.XtraGrid.Columns.FixedStyle Fixed { get; set; }
            public ABCSummaryType SumType { get; set; }
            public int ImageIndex { get; set; }
            public bool AllowEdit { get; set; }
            public bool GroupBy { get; set; }

            public String FilterString { get; set; }
            public String RelationParentField { get; set; }
            public String RelationParentControl { get; set; }
            public String RelationChildField { get; set; }

            public ColumnConfig ( )
            {
                //RepoType=ABCRepositoryType.None;
                ImageIndex=-1;
                Width=50;
                Visible=true;
                VisibleIndex=0;
                Fixed=DevExpress.XtraGrid.Columns.FixedStyle.None;
                SumType=ABCSummaryType.None;
                AllowEdit=false;
                GroupBy=false;
            }


            public object Clone ( )
            {
                return (ColumnConfig)this.MemberwiseClone();
            }
        }

        public ColumnConfig Config;
        public String TableName;

        public bool AllowView=true;

        public ABCGridColumn ( )
        {
        }
        public ABCGridColumn ( ABCGridColumn.ColumnConfig colInfo , String strTableName )
        {
            Config=colInfo;
            this.Caption=colInfo.Caption;
            this.AppearanceHeader.TextOptions.HAlignment=HorzAlignment.Center;
            this.AppearanceHeader.Options.UseTextOptions=true;

            if ( String.IsNullOrWhiteSpace( colInfo.FieldName )==false )
            {
                this.FieldName=colInfo.FieldName.Split( ':' )[0].Trim();
                if ( colInfo.FieldName.Contains( ":" ) )
                {
                    this.UnboundType=DevExpress.Data.UnboundColumnType.Decimal;
                    this.FieldName=colInfo.FieldName;
                }
            }

            this.TableName=strTableName;
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                this.TableName=colInfo.TableName;

            this.VisibleIndex=colInfo.VisibleIndex;
            this.Visible=colInfo.Visible;
            this.Width=colInfo.Width;
            this.ImageIndex=colInfo.ImageIndex;
            this.Fixed=colInfo.Fixed;

            if ( colInfo.FieldName!=null&&colInfo.FieldName.Contains( ":" ) )
            {
                this.SortMode=DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                this.GroupInterval=ColumnGroupInterval.DisplayText;
            }
          
            DataCachingProvider.AccrossStructInfo accross=DataCachingProvider.GetAccrossStructInfo( this.TableName , this.FieldName );
            if (  ABCScreen.ABCScreenHelper.Instance.CheckFieldPermission( accross.TableName , accross.FieldName , FieldPermission.AllowView )==false )
            {
                AllowView=false;
                colInfo.AllowEdit=false;
            }
            else if ( colInfo.AllowEdit )
            {
                colInfo.AllowEdit= ABCScreen.ABCScreenHelper.Instance.CheckFieldPermission( accross.TableName , accross.FieldName , FieldPermission.AllowEdit );
            }

            this.OptionsColumn.AllowEdit=colInfo.AllowEdit;
            
            this.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.OptionsFilter.AllowAutoFilter=true;

            if ( colInfo.GroupBy )
                this.Group();
        }

        public void Initialize ( )
        {
            #region Caption

            //    colInfo.IsUseAlias=true;
            if ( this.Config.IsUseAlias&&this.Config.FieldName!=null )
            {
                String strCaption=String.Empty;
                if ( this.Config.FieldName.Contains( ":" ) )
                {
                    DataCachingProvider.AccrossStructInfo structInfo=DataCachingProvider.GetAccrossStructInfo( this.TableName , this.Config.FieldName );
                    if ( structInfo!=null )
                        this.Caption=DataConfigProvider.GetFieldCaption( structInfo.TableName , structInfo.FieldName );
                }
                else
                {
                    strCaption=DataConfigProvider.GetFieldCaption( this.TableName , this.Config.FieldName );
                }
                if ( string.IsNullOrWhiteSpace( strCaption )==false&&strCaption!=this.Config.FieldName )
                {
                    this.Caption=strCaption;
                    this.Config.Caption=strCaption;
                }
            }

            #endregion

            try
            {
                #region Repository

                String strFieldName=this.FieldName;
                if ( this.FieldName.Contains( ":" ) )
                    strFieldName=this.FieldName.Split( ':' )[0].Trim();

                #region RepositoryGridLookupEdit
                if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , strFieldName );

                    if ( String.IsNullOrWhiteSpace( strPKTableName )==false )
                    {

                        #region DataSource
                        if ( String.IsNullOrWhiteSpace( this.Config.FilterString )&&
                            String.IsNullOrWhiteSpace( this.Config.RelationParentField )&&String.IsNullOrWhiteSpace( this.Config.RelationParentControl ) )
                        {
                            this.ColumnEdit=ABCControls.UICaching.GetDefaultRepository( strPKTableName , false );
                            ( this.ColumnEdit as RepositoryItemLookUpEditBase ).DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );
                        }
                        else
                        {
                            this.ColumnEdit=ABCControls.UICaching.GetDefaultRepository( strPKTableName , true );
                            DataView newView=DataCachingProvider.TryToGetDataView( strPKTableName , true );
                            newView.RowFilter=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( this.Config.FilterString ) );
                            ( this.ColumnEdit as RepositoryItemLookUpEditBase ).DataSource=newView;
                              
                        }
                        #endregion

                        if ( this.ColumnEdit is RepositoryItemLookUpEditBase )
                            ( this.ColumnEdit as RepositoryItemLookUpEditBase ).QueryPopUp+=new CancelEventHandler( ABCRepositoryGridLookUpEdit_QueryPopUp );

                        ( this.ColumnEdit as RepositoryItemLookUpEditBase ).Buttons[1].Visible=ABCPresentHelper.IsMainObject( strPKTableName );

                    }
                }
                #endregion

                #region Enum
                else 
                {
                    UICaching.AssignEnums( this , this.TableName , strFieldName , this.Config.FilterString );

                }
                #endregion

                #endregion
            }
            catch ( Exception ex )
            {
            }

            InitFormat();

            #region Summary
            switch ( Config.SumType )
            {
                case ABCSummaryType.SUM:
                    this.SummaryItem.SummaryType=DevExpress.Data.SummaryItemType.Sum;
                    break;
                case ABCSummaryType.AVG:
                    this.SummaryItem.SummaryType=DevExpress.Data.SummaryItemType.Average;
                    break;
                case ABCSummaryType.MAX:
                    this.SummaryItem.SummaryType=DevExpress.Data.SummaryItemType.Max;
                    break;
                case ABCSummaryType.MIN:
                    this.SummaryItem.SummaryType=DevExpress.Data.SummaryItemType.Min;
                    break;
            }

            this.SummaryItem.Format=this.DisplayFormat.Format;
            this.SummaryItem.DisplayFormat="{0:"+this.DisplayFormat.FormatString+"}";
            #endregion

            this.OptionsFilter.ImmediateUpdateAutoFilter=false;

        }

        public void InitFormat ( )
        {
            String strFieldName=String.Empty;
            if ( this.Config!=null )
                strFieldName=this.Config.FieldName;
            else
                strFieldName=this.FieldName;

            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( this.TableName , strFieldName );
            if ( formatInfo!=null )
            {
                if ( formatInfo.FormatInfo.FormatType==FormatType.Numeric )
                {
                    this.AppearanceCell.TextOptions.HAlignment=HorzAlignment.Far;
                    this.AppearanceCell.Options.UseTextOptions=true;
                }
           
                if ( this.ColumnEdit==null )
                {
                    if ( formatInfo.FormatInfo.FormatType==FormatType.Numeric )
                    {
                        //if ( formatInfo.ABCFormat==DataFormatProvider.FieldFormat.Percent&&this.OptionsColumn.AllowEdit==false )
                        //{
                        //    this.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
                        //    ( this.ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ).PercentView=true;
                        //    ( this.ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ).ShowTitle=true;
                        //}
                        //else
                            this.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                    }
                    if ( formatInfo.FormatInfo.FormatType==FormatType.DateTime&&this.OptionsColumn.AllowEdit )
                    {
                        if ( formatInfo.ABCFormat==DataFormatProvider.FieldFormat.Time )
                            this.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
                        else
                            this.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
                    }
                }

           
                if ( this.ColumnEdit!=null )
                {
                    this.ColumnEdit.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
                    this.ColumnEdit.DisplayFormat.FormatType=FormatType.None;
                    this.ColumnEdit.DisplayFormat.FormatString=String.Empty;
                    this.ColumnEdit.EditFormat.FormatType=FormatType.None;
                    this.ColumnEdit.EditFormat.FormatString=String.Empty;
                    this.DisplayFormat.FormatString=String.Empty;
                    this.DisplayFormat.FormatType=FormatType.None;

                    if ( this.ColumnEdit is DevExpress.XtraEditors.Repository.RepositoryItemTextEdit )
                    {
                        ( this.ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.UseMaskAsDisplayFormat=true;
                        ( this.ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.EditMask=formatInfo.EditMask;
                        ( this.ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.MaskType=formatInfo.MaskType;
                    }
                    else
                    {
                        this.DisplayFormat.FormatString=formatInfo.FormatInfo.FormatString;
                        this.DisplayFormat.FormatType=formatInfo.FormatInfo.FormatType;

                        this.ColumnEdit.DisplayFormat.FormatString=formatInfo.FormatInfo.FormatString;
                        this.ColumnEdit.DisplayFormat.FormatType=formatInfo.FormatInfo.FormatType;
                        this.ColumnEdit.EditFormat.FormatString=formatInfo.FormatInfo.FormatString;
                        this.ColumnEdit.EditFormat.FormatType=formatInfo.FormatInfo.FormatType;
                    }
                }
             
            }
        }

        void ABCRepositoryGridLookUpEdit_QueryPopUp ( object sender , CancelEventArgs e )
        {
     
            if ( ( String.IsNullOrWhiteSpace( this.Config.RelationParentField )==false||String.IsNullOrWhiteSpace( this.Config.RelationParentControl )==false )
                &&String.IsNullOrWhiteSpace( this.Config.RelationChildField )==false )
            {
                String strFieldName=this.FieldName;
                if ( this.FieldName.Contains( ":" ) )
                    strFieldName=this.FieldName.Split( ':' )[0].Trim();

                if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , strFieldName );

                    if ( DataStructureProvider.IsTableColumn( strPKTableName , this.Config.RelationChildField ) )
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView gridView=this.View as DevExpress.XtraGrid.Views.Grid.GridView;

                        #region parent Value
                        object objValue=null;
                        object objRow=gridView.GetFocusedRow();
                        if ( objRow!=null&&String.IsNullOrWhiteSpace( this.Config.RelationParentField )==false )
                        {
                            objValue=objRow.GetType().GetProperty( this.Config.RelationParentField ).GetValue( objRow , null );
                        }
                        else if ( String.IsNullOrWhiteSpace( this.Config.RelationParentControl )==false )
                        {
                            if ( ( gridView as ABCGridView ).ABCGridControl.OwnerView!=null )
                            {
                                Control ctrl=( gridView as ABCGridView ).ABCGridControl.OwnerView.GetControl( this.Config.RelationParentControl );
                                if ( ctrl!=null )
                                {
                                    if ( ctrl is ABCBindingBaseEdit )
                                        objValue=( ctrl as ABCBindingBaseEdit ).EditValue;
                                    else if ( ctrl is IABCBindableControl )
                                    {
                                        if ( String.IsNullOrWhiteSpace( ( ctrl as IABCBindableControl ).BindingProperty )==false )
                                            objValue=ctrl.GetType().GetProperty( ( ctrl as IABCBindableControl ).BindingProperty ).GetValue( ctrl , null );
                                    }
                                }
                            }
                        }
                        #endregion

                        if ( objValue!=null&&objValue!=DBNull.Value )
                        {
                            DataView newView=DataCachingProvider.TryToGetDataView( strPKTableName , true );
                            newView.RowFilter=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( this.Config.FilterString ) );

                            String strFilter=String.Empty;
                            if ( objValue.GetType()==typeof( int )||objValue.GetType()==typeof( double )
                                ||objValue.GetType()==typeof( Nullable<int> )||objValue.GetType()==typeof( Nullable<double> )
                                ||objValue.GetType()==typeof( Boolean )||objValue.GetType()==typeof( Nullable<Boolean> ) )
                                strFilter=String.Format( @" {0} = {1} " , this.Config.RelationChildField , objValue );
                            if ( objValue.GetType()==typeof( String )||objValue.GetType()==typeof( Guid )||objValue.GetType()==typeof( Nullable<Guid> ) )
                                strFilter=String.Format( @" {0} = '{1}' " , this.Config.RelationChildField , objValue.ToString() );
                            if ( String.IsNullOrWhiteSpace( newView.RowFilter )==false )
                                strFilter=" AND "+strFilter;
                            newView.RowFilter+=strFilter;

                            ( sender as LookUpEditBase ).Properties.DataSource=newView;
                        }
                    }


                }
            }
        }


    }
    public class ABCGridColumnCollection : GridColumnCollection
    {
        public ABCGridColumnCollection ( ColumnView view )
            : base( view )
        {
        }

        protected override GridColumn CreateColumn ( )
        {
            return new ABCGridColumn();
        }
    }
}
