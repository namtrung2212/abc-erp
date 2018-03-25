using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using  System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;

using ABCBusinessEntities;
using ABCCommon;
using ABCProvider;
using DevExpress.XtraEditors.Repository;

namespace ABCControls
{

    public class ABCPivotGridField : DevExpress.XtraPivotGrid.PivotGridField
    {
        [Serializable]
        public class FieldConfig
        {
            public String FieldName { get; set; }
            public String TableName { get; set; }
            public String Caption { get; set; }
            public int Width { get; set; }
            public int Index { get; set; }
            public int AreaIndex { get; set; }
            public DevExpress.XtraPivotGrid.PivotArea Area { get; set; }
            public bool Visible { get; set; }

            public ABCRepositoryType RepoType { get; set; }
            public ABCSummaryType SumType { get; set; }
            public int ImageIndex { get; set; }
            public bool AllowEdit { get; set; }

            public FieldConfig ( )
            {
                RepoType=ABCRepositoryType.None;
                ImageIndex=-1;
                Width=50;
                Visible=true;
                SumType=ABCSummaryType.None;
                AllowEdit=false;
                AreaIndex=0;
                Index=0;
                Area=DevExpress.XtraPivotGrid.PivotArea.DataArea;
            }

            public object Clone ( )
            {
                return  (FieldConfig)this.MemberwiseClone();
            }
        }

        public FieldConfig Config;
        public String TableName;

        public ABCPivotGridField ( )
        {
        }
        public ABCPivotGridField ( ABCPivotGridField.FieldConfig colInfo,String strTableName )
        {
            Config=colInfo;

            this.Caption=colInfo.Caption;
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

            this.Visible=colInfo.Visible;
            this.Width=colInfo.Width;
            this.ImageIndex=colInfo.ImageIndex;
            this.Options.AllowEdit=colInfo.AllowEdit;
            this.AreaIndex=colInfo.AreaIndex;
            this.Index=colInfo.Index;
            this.Area=colInfo.Area;

        }

        public void Initialize ( )
        {
            #region Repository
            if ( Config.RepoType==ABCRepositoryType.None )
            {
                if ( String.IsNullOrWhiteSpace( this.Config.FieldName )==false&&this.Config.FieldName.Contains( ":" ) )
                {
                    this.Options.AllowEdit=false;
                }
                else
                {
                    String strPKTableName=String.Empty;
                    if ( DataStructureProvider.IsForeignKey( this.TableName , this.FieldName ) )
                        strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , this.FieldName );

                    if ( String.IsNullOrWhiteSpace( strPKTableName )==false )
                    {
                        this.FieldEdit=ABCControls.UICaching.GetDefaultRepository( strPKTableName , false );
                        ( this.FieldEdit as RepositoryItemLookUpEditBase ).DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );
                    }
                }

            } 
            #endregion

            #region Format

            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( this.TableName , this.Config.FieldName );
            if ( formatInfo!=null )
            {
                this.CellFormat.FormatString=formatInfo.FormatInfo.FormatString;
                this.CellFormat.FormatType=formatInfo.FormatInfo.FormatType;
            }
            #endregion
        }


        //public void InitFormat ( )
        //{
        //    String strPKTableName=String.Empty;
        //    if ( DataStructureProvider.IsForeignKey( this.TableName , this.FieldName ) )
        //        strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , this.FieldName );

        //    if ( String.IsNullOrWhiteSpace( strPKTableName )==false )
        //    {
        //        this.FieldEdit=ABCControls.UICaching.GetDefaultRepositoryGridLookupEdit( strPKTableName );
        //        ( this.FieldEdit as ABCRepositoryGridLookupEdit ).DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );
        //    }
        //}
    }

    [ToolboxBitmapAttribute( typeof( DevExpress.XtraPivotGrid.PivotGridControl ) )]
    [Designer( typeof( ABCPivotGridControlDesigner ) )]
    public partial class ABCPivotGridControl : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl , IABCCustomControl
    {

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [Browsable(false)]
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

        public ABCView OwnerView { get; set; }
        public BusinessObjectController Controller;

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
        [Browsable( false )]
        public DevExpress.XtraPivotGrid.PivotGridControl Grid
        {
            get
            {
                return this.InnerGrid;
            }
        }

        #region Properties

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
                 if(OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }

        #region Options
        [Category( "Options" )]
        public bool DrawFocusedCellRect { get { return InnerGrid.OptionsView.DrawFocusedCellRect; } set { InnerGrid.OptionsView.DrawFocusedCellRect=value; } }
        [Category( "Options" )]
        public bool GroupFieldsInCustomizationWindow { get { return InnerGrid.OptionsView.GroupFieldsInCustomizationWindow; } set { InnerGrid.OptionsView.GroupFieldsInCustomizationWindow=value; } }
        [Category( "Options" )]
        public bool ShowColumnGrandTotalHeader { get { return InnerGrid.OptionsView.ShowColumnGrandTotalHeader; } set { InnerGrid.OptionsView.ShowColumnGrandTotalHeader=value; } }
        [Category( "Options" )]
        public bool ShowColumnGrandTotals { get { return InnerGrid.OptionsView.ShowColumnGrandTotals; } set { InnerGrid.OptionsView.ShowColumnGrandTotals=value; } }
        [Category( "Options" )]
        public bool ShowColumnHeaders { get { return InnerGrid.OptionsView.ShowColumnHeaders; } set { InnerGrid.OptionsView.ShowColumnHeaders=value; } }
        [Category( "Options" )]
        public bool ShowColumnTotals { get { return InnerGrid.OptionsView.ShowColumnTotals; } set { InnerGrid.OptionsView.ShowColumnTotals=value; } }
        [Category( "Options" )]
        public bool ShowCustomTotalsForSingleValues { get { return InnerGrid.OptionsView.ShowCustomTotalsForSingleValues; } set { InnerGrid.OptionsView.ShowCustomTotalsForSingleValues=value; } }
        [Category( "Options" )]
        public bool ShowDataHeaders { get { return InnerGrid.OptionsView.ShowDataHeaders; } set { InnerGrid.OptionsView.ShowDataHeaders=value; } }
        [Category( "Options" )]
        public bool ShowFilterHeaders { get { return InnerGrid.OptionsView.ShowFilterHeaders; } set { InnerGrid.OptionsView.ShowFilterHeaders=value; } }
        [Category( "Options" )]
        public bool ShowFilterSeparatorBar { get { return InnerGrid.OptionsView.ShowFilterSeparatorBar; } set { InnerGrid.OptionsView.ShowFilterSeparatorBar=value; } }
        [Category( "Options" )]
        public bool ShowGrandTotalsForSingleValues { get { return InnerGrid.OptionsView.ShowGrandTotalsForSingleValues; } set { InnerGrid.OptionsView.ShowGrandTotalsForSingleValues=value; } }
        [Category( "Options" )]
        public bool ShowHorzLines { get { return InnerGrid.OptionsView.ShowHorzLines; } set { InnerGrid.OptionsView.ShowHorzLines=value; } }
        [Category( "Options" )]
        public bool ShowVertLines { get { return InnerGrid.OptionsView.ShowVertLines; } set { InnerGrid.OptionsView.ShowVertLines=value; } }
        [Category( "Options" )]
        public bool ShowRowGrandTotalHeader { get { return InnerGrid.OptionsView.ShowRowGrandTotalHeader; } set { InnerGrid.OptionsView.ShowRowGrandTotalHeader=value; } }
        [Category( "Options" )]
        public bool ShowRowGrandTotals { get { return InnerGrid.OptionsView.ShowRowGrandTotals; } set { InnerGrid.OptionsView.ShowRowGrandTotals=value; } }
        [Category( "Options" )]
        public bool ShowRowHeaders { get { return InnerGrid.OptionsView.ShowRowHeaders; } set { InnerGrid.OptionsView.ShowRowHeaders=value; } }
        [Category( "Options" )]
        public bool ShowRowTotals { get { return InnerGrid.OptionsView.ShowRowTotals; } set { InnerGrid.OptionsView.ShowRowTotals=value; } }
        [Category( "Options" )]
        public bool ShowTotalsForSingleValues { get { return InnerGrid.OptionsView.ShowTotalsForSingleValues; } set { InnerGrid.OptionsView.ShowTotalsForSingleValues=value; } }
       
        [Category( "Options" )]
        public int RowTreeWidth { get { return InnerGrid.OptionsView.RowTreeWidth; } set { InnerGrid.OptionsView.RowTreeWidth=value; } }
     
        [Category( "Printing" )]
        public string ViewCaption { get; set; }
        
        [Category( "Printing" )]
        public Margins PageMargins { get; set; }

        [Category( "Printing" )]
        public bool Landscape { get; set; }

        [Category( "Printing" )]
        public PaperKind PaperKind { get; set; }
        
        [Category( "Printing" )]
        public DevExpress.XtraCharts.Printing.PrintSizeMode ChartSizeMode { get; set; }
       
        #endregion

        #region MenuBar
        [Category( "MenuBar" )]
        public Boolean ShowMenuBar { get { return mainBar.Visible; } set { mainBar.Visible=value; } }
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

        [Category( "Fields" )]
        [Editor( typeof( PivotGridFieldsEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "Fields" ) , Description( "Which type to use..." )]
        public BindingList<ABCPivotGridField.FieldConfig> FieldConfigs{get;set;}

        #endregion
     
        public ABCPivotGridControl ( )
        {
            InitializeComponent();

            Initialize();

            this.Disposed+=new EventHandler( ABCPivotGridControl_Disposed );
        }
        void ABCPivotGridControl_Disposed ( object sender , EventArgs e )
        {
            //if ( RowDetail!=null )
            //{
            //    if ( RowDetail.Visible )
            //        RowDetail.Close();

            //    RowDetail.Dispose();
            //}
        }
      
        private void Initialize ( )
        {
           // this.gridCtrl.OptionsView.ShowGroupPanel=false;
            this.InnerGrid.Appearance.Empty.BackColor=System.Drawing.Color.FromArgb( 181 , 200 , 223 );
            this.InnerGrid.Appearance.Empty.Options.UseBackColor=true;
            this.InnerGrid.OptionsCustomization.CustomizationFormStyle=DevExpress.XtraPivotGrid.Customization.CustomizationFormStyle.Excel2007;
            this.InnerGrid.OptionsCustomization.AllowFilterInCustomizationForm=true;
            this.InnerGrid.OptionsCustomization.AllowSortInCustomizationForm=true;
            this.InnerGrid.OptionsChartDataSource.ProvideDataByColumns=false;
            this.InnerGrid.OptionsChartDataSource.FieldValuesProvideMode=DevExpress.XtraPivotGrid.PivotChartFieldValuesProvideMode.DisplayText;
            this.InnerGrid.OptionsView.RowTotalsLocation=DevExpress.XtraPivotGrid.PivotRowTotalsLocation.Tree;
            this.InnerGrid.OptionsBehavior.HorizontalScrolling=DevExpress.XtraPivotGrid.PivotGridScrolling.CellsArea;

            this.chartControl.Chart.OptionsPrint.SizeMode=DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;

            this.InnerGrid.CustomCellDisplayText+=new DevExpress.XtraPivotGrid.PivotCellDisplayTextEventHandler(Grid_CustomCellDisplayText);
            this.InnerGrid.CustomCellValue+=new EventHandler<DevExpress.XtraPivotGrid.PivotCellValueEventArgs>( Grid_CustomCellValue );
            this.InnerGrid.CustomDrawCell+=new DevExpress.XtraPivotGrid.PivotCustomDrawCellEventHandler( Grid_CustomDrawCell );
            this.InnerGrid.CellClick+=new DevExpress.XtraPivotGrid.PivotCellEventHandler( Grid_CellClick );

            this.InnerGrid.CustomUnboundFieldData+=new DevExpress.XtraPivotGrid.CustomFieldDataEventHandler( Grid_CustomUnboundFieldData );
            this.InnerGrid.FieldValueDisplayText+=new DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventHandler( Grid_FieldValueDisplayText );
      
            this.InnerGrid.CustomChartDataSourceData+=new DevExpress.XtraPivotGrid.PivotCustomChartDataSourceDataEventHandler( gridCtrl_CustomChartDataSourceData );
            this.chartControl.Chart.BoundDataChanged+=new DevExpress.XtraCharts.BoundDataChangedEventHandler( Chart_BoundDataChanged );
        
            InitMenuBar();
        }

      
   
        void gridCtrl_CustomChartDataSourceData ( object sender , DevExpress.XtraPivotGrid.PivotCustomChartDataSourceDataEventArgs e )
        {
            //if ( e.FieldValueInfo==null )
            //    return;

            //if ( e.ItemDataMember==DevExpress.XtraPivotGrid.PivotChartItemDataMember.Series&& e.FieldValueInfo.DataField!=null)
            //    e.Value=e.FieldValueInfo.DataField.ToString();

            //if ( e.ItemDataMember==DevExpress.XtraPivotGrid.PivotChartItemDataMember.Argument|| e.ItemDataMember==DevExpress.XtraPivotGrid.PivotChartItemDataMember.Value)
            //{
            //    if ( e.Value==DBNull.Value )
            //        return;

            //    if ( ( e.FieldValueInfo.Field is ABCPivotGridField==false )||( e.FieldValueInfo.Field as ABCPivotGridField ).Config==null||String.IsNullOrWhiteSpace( ( e.FieldValueInfo.Field as ABCPivotGridField ).Config.FieldName ) )
            //        return;

            //    ABCPivotGridField gridCol=e.FieldValueInfo.Field as ABCPivotGridField;
            //    if ( gridCol.Config.FieldName.Contains( ":" )==false )
            //    {

            //        if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , gridCol.FieldName ) )
            //            e.Value="ABCHide";
            //    }
            //    else
            //    {

            //        DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( gridCol.Config.TableName , gridCol.Config.FieldName );
            //        if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
            //                                            ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
            //            e.Value="ABCHide";
            //    }

            //}
        }
        void Chart_BoundDataChanged ( object sender , EventArgs e )
        {
       
            foreach ( DevExpress.XtraCharts.Series  s in this.chartControl.Chart.Series )
            {
                if ( s.Name=="ABCHide" )
                    s.Visible=false;
            }
            
        }

        #region Accross Table DisplayText
        void Grid_CustomUnboundFieldData ( object sender , DevExpress.XtraPivotGrid.CustomFieldDataEventArgs e )
        {
            try
            {
                if ( e.Field!=null )
                {
                    if ( ( e.Field.Area==DevExpress.XtraPivotGrid.PivotArea.ColumnArea )||( e.Field.Area==DevExpress.XtraPivotGrid.PivotArea.RowArea ) )
                    {
                        if ( e.Field.FieldEdit==null )
                        {
                            ABCPivotGridField gridCol=e.Field as ABCPivotGridField;
                            if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false )
                            {
                                String strBaseField=gridCol.Config.TableName.Split( ':' )[0];
                                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField )&&e.Value!=DBNull.Value )
                                    e.Value=DataCachingProvider.GetCachingObjectAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value ) , gridCol.Config.FieldName );
                            }
                        }

                    }
                }
            }
            catch ( Exception exp )
            {
            }
        }

        void Grid_FieldValueDisplayText ( object sender , DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventArgs e )
        {
            try
            {
                if ( e.Field!=null )
                {
                    if ( ( e.Field.Area==DevExpress.XtraPivotGrid.PivotArea.ColumnArea )||( e.Field.Area==DevExpress.XtraPivotGrid.PivotArea.RowArea ) )
                    {
                        if ( e.Field.FieldEdit!=null )
                        {
                            DevExpress.XtraEditors.Repository.RepositoryItem repItem=e.Field.FieldEdit;
                            e.DisplayText=repItem.GetDisplayText( e.Value );
                        }
                        else
                        {
                                ABCPivotGridField gridCol=e.Field as ABCPivotGridField;
                                if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false&&gridCol.Config.FieldName.Contains( ":" ) )
                                {
                                    String strBaseField=gridCol.Config.TableName.Split( ':' )[0];
                                    if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField )&&e.Value!=DBNull.Value )
                                        e.DisplayText=DataFormatProvider.DoFormat( e.Value , gridCol.Config.TableName , gridCol.Config.FieldName );
                                }
                        }

                    }
                }
            }
            catch ( Exception exp )
            {
            }
        }
        #endregion

        #region MenuBar
        DevExpress.XtraBars.BarManager barManager;
        DevExpress.XtraBars.Bar mainBar;
        DevExpress.XtraBars.BarButtonItem RefreshItem;
        DevExpress.XtraBars.BarButtonItem ExportItem;
        DevExpress.XtraBars.BarButtonItem PrintItem;
        DevExpress.XtraBars.BarButtonItem ColumnChooserItem;
        DevExpress.XtraBars.BarButtonItem ShowChartItem;

        private void InitMenuBar ( )
        {

            RefreshItem=new DevExpress.XtraBars.BarButtonItem();
            RefreshItem.Caption="Refresh";
            RefreshItem.Id=1;
            RefreshItem.ImageIndex=47;
            RefreshItem.Tag="Refresh";
            RefreshItem.Name="RefreshItem";

            ColumnChooserItem=new DevExpress.XtraBars.BarButtonItem();
            ColumnChooserItem.Caption="Columns";
            ColumnChooserItem.Id=3;
            ColumnChooserItem.ImageIndex=81;
            ColumnChooserItem.Tag="ColumnChooser";
            ColumnChooserItem.Name="ColumnChooserItem";

            ShowChartItem=new DevExpress.XtraBars.BarButtonItem();
            ShowChartItem.Caption="Show Chart";
            ShowChartItem.Id=3;
            ShowChartItem.ImageIndex=84;
            ShowChartItem.Tag="ShowChart";
            ShowChartItem.Name="ShowChartItem";

            
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
            barManager.Items.AddRange( new DevExpress.XtraBars.BarItem[] {  RefreshItem,ColumnChooserItem , ShowChartItem   , ExportItem , PrintItem } );
            barManager.MainMenu=mainBar;
            barManager.MaxItemId=5;
            barManager.Images=ABCImageList.List16x16;
            barManager.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( barManager_ItemClick );
            mainBar.AddItems( new DevExpress.XtraBars.BarItem[] { RefreshItem , ColumnChooserItem , ShowChartItem , ExportItem , PrintItem } );
            mainBar.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,RefreshItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
              new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,ShowChartItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,PrintItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
             new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,ExportItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
                new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle,ColumnChooserItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)} );
        }

        void barManager_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            String strTag=e.Item.Tag.ToString();

            OnBarItemClick( strTag );

            switch ( strTag )
            {
                case "Refresh":
                    OnRefresh();
                    break;
             
                case "ShowChart":
                    OnShowChart();
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
            else
            {
                this.Grid.RefreshData();
            }
        }
        public virtual void OnColumnChooser ( )
        {
            this.InnerGrid.FieldsCustomization();
        }
        public virtual void OnShowChart ( )
        {
            this.UseChartControl=!this.UseChartControl;
        }
        public virtual void OnExport ( )
        {
            SaveFileDialog dlg=new SaveFileDialog();
            dlg.Filter="xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            dlg.FilterIndex=2;
            dlg.RestoreDirectory=true;
            if ( dlg.ShowDialog()==DialogResult.OK )
            {
                //foreach ( DevExpress.XtraGrid.Columns.GridColumn col in gridView.Columns )
                //    col.MinWidth=gridView.CalcColumnBestWidth( col );

                //DevExpress.XtraPrinting.XlsExportOptions xlsOptions=new DevExpress.XtraPrinting.XlsExportOptions();
                //xlsOptions.TextExportMode=DevExpress.XtraPrinting.TextExportMode.Text;
                //xlsOptions.ShowGridLines=true;
                //gridView.ExportToXls( dlg.FileName , xlsOptions );

                //DevExpress.XtraExport.ExportXlsProvider provider=new DevExpress.XtraExport.ExportXlsProvider( dlg.FileName );
                //DevExpress.XtraGrid.Export.GridViewExportLink link=gridCtrl.CreateExportLink( provider ) as DevExpress.XtraGrid.Export.GridViewExportLink;
                //link.ExportCellsAsDisplayText=false;
                //link.ExportTo( true );
                if ( dlg.FileName.EndsWith( "xlsx" ) )
                    InnerGrid.ExportToXlsx( dlg.FileName );
                else
                    InnerGrid.ExportToXls( dlg.FileName );
                Application.DoEvents();

                System.Diagnostics.Process.Start( dlg.FileName );
            }




        }

        #region OnPrint
        public virtual void OnPrint ( )
        {
            this.InnerGrid.AppearancePrint.Cell.BorderColor=Color.Black;
            this.InnerGrid.AppearancePrint.Cell.Options.UseBorderColor=true;

            this.InnerGrid.AppearancePrint.FieldHeader.BackColor=Color.FromArgb( 255 , 119 , 149 , 203 );
            this.InnerGrid.AppearancePrint.FieldHeader.ForeColor=Color.White;
            this.InnerGrid.AppearancePrint.FieldHeader.Font=new Font( ABCFontProvider.GetFontFamily( "calibri" ) , 10 , FontStyle.Regular );
            this.InnerGrid.AppearancePrint.FieldHeader.BorderColor=Color.Black;
            this.InnerGrid.AppearancePrint.FieldHeader.Options.UseBackColor=true;
            this.InnerGrid.AppearancePrint.FieldHeader.Options.UseForeColor=true;
            this.InnerGrid.AppearancePrint.FieldHeader.Options.UseFont=true;
            this.InnerGrid.AppearancePrint.FieldHeader.Options.UseBorderColor=true;
            this.InnerGrid.AppearancePrint.FieldValue.BackColor=Color.FromArgb( 255 , 210 , 220 , 238 );
            this.InnerGrid.AppearancePrint.FieldValue.ForeColor=Color.Black;
            this.InnerGrid.AppearancePrint.FieldValue.Options.UseBackColor=true;
            this.InnerGrid.AppearancePrint.FieldValue.Options.UseForeColor=true;
            this.InnerGrid.OptionsPrint.UsePrintAppearance=false;
   
            PrintingSystem printingSystem1=new PrintingSystem();

            DevExpress.XtraPrintingLinks.CompositeLink composLink=new DevExpress.XtraPrintingLinks.CompositeLink( printingSystem1 );
            if ( this.PageMargins!=null )
                composLink.Margins=this.PageMargins;
            else
                composLink.Margins=new System.Drawing.Printing.Margins( 50 , 50 , 40 , 50 );

            composLink.Landscape=this.Landscape;

            if ( this.PaperKind!=null )
                composLink.PaperKind=this.PaperKind;
            else
                composLink.PaperKind=System.Drawing.Printing.PaperKind.A4;

            composLink.VerticalContentSplitting=VerticalContentSplitting.Smart;
            composLink.CreateMarginalHeaderArea+=new CreateAreaEventHandler( printableComponentLink1_CreateMarginalHeaderArea );
            composLink.CreateReportHeaderArea+=new CreateAreaEventHandler( printableComponentLink1_CreateReportHeaderArea );
            composLink.CreateMarginalFooterArea+=new CreateAreaEventHandler( printableComponentLink1_CreateMarginalFooterArea );

            //if ( this.InnerGrid.Size.Width>printingSystem1.PageSettings.PageSettings.PaperSize.Width-100 )
            //    composLink.Landscape=true;

            if ( this.UseChartControl )
            {
                if ( this.ChartSizeMode!=null )
                    this.chartControl.Chart.OptionsPrint.SizeMode=this.ChartSizeMode;
                else
                    this.chartControl.Chart.OptionsPrint.SizeMode=DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;
            
                PrintableComponentLink printableComponentLink2=new PrintableComponentLink();
                printableComponentLink2.Component=this.chartControl.Chart;
                composLink.Links.Add( printableComponentLink2 );
            }

            PrintableComponentLink printableComponentLink1=new PrintableComponentLink();
            printableComponentLink1.Component=this.InnerGrid;
            printableComponentLink1.PrintingSystem.Document.AutoFitToPagesWidth=1;
            composLink.Links.Add( printableComponentLink1 );


            printableComponentLink1.ShowRibbonPreviewDialog( this.LookAndFeel );

        }

        void printableComponentLink1_CreateMarginalHeaderArea ( object sender , CreateAreaEventArgs e )
        {
            e.Graph.Modifier=BrickModifier.MarginalHeader;
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) , 10 );
            string format="In ngày : {0:dd/mm/yyyy}";
            PageInfoBrick brick=e.Graph.DrawPageInfo( PageInfo.DateTime , format , Color.FromArgb( 255 , 119 , 149 , 203 ) ,
            new RectangleF( 0 , 0 , 0 , 20 ) , BorderSide.None );
            brick.Alignment=BrickAlignment.Near;
            brick.AutoWidth=true;
        }
        void printableComponentLink1_CreateReportHeaderArea ( object sender , CreateAreaEventArgs e )
        {
            string reportHeader=this.ViewCaption;

            e.Graph.StringFormat=new BrickStringFormat( StringAlignment.Center );
            e.Graph.Font=new System.Drawing.Font( ABCFontProvider.GetFontFamily( "calibri" ) , 17 , FontStyle.Bold|FontStyle.Underline );
            e.Graph.BackColor=Color.White;
            RectangleF rec=new RectangleF( 0 , 20 , e.Graph.ClientPageSize.Width , 50 );
            e.Graph.DrawString( reportHeader , Color.FromArgb( 255 , 119 , 149 , 203 ) , rec , BorderSide.None );
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
            if ( String.IsNullOrWhiteSpace( TableName )==false )
                Controller=BusinessControllerFactory.GetBusinessController( TableName );
        }
        #endregion

        #region PivotGrid
     
        #region Cell

        #region Cell Click
        public void RunFocusedLink ( ABCPivotGridField DataField , object objValue )
        {

            if ( DataField==null )
                return;

            if ( objValue==null||objValue==DBNull.Value||String.IsNullOrWhiteSpace(objValue.ToString()) )
                return;

            String strTableName=this.TableName;
            if ( DataField.Config!=null )
                strTableName=DataField.Config.TableName;

            String strLinkTableName=String.Empty;
            Guid iLinkID=Guid.Empty;

            if ( DataField.Config!=null&&DataField.Config.FieldName.Contains( ":" ) )
            {
                if ( ABCHelper.DataConverter.ConvertToGuid( objValue ).GetType()!=typeof( Guid ) )
                    return;

                DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( strTableName , ABCHelper.DataConverter.ConvertToGuid( objValue ) , DataField.Config.FieldName );
                if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                    ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                {
                    strLinkTableName=acrrosInfo.TableName;
                    iLinkID=acrrosInfo.TableID;
                }
            }
            else
            {
                if ( DataStructureProvider.IsForeignKey( strTableName , DataField.FieldName ) )
                {
                    strLinkTableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , DataField.FieldName );
                    iLinkID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                }
            }

            if ( iLinkID!=Guid.Empty )
            {
                if (this.OwnerView!=null )
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                else
                     ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );
            }
        }

        void Grid_CellClick ( object sender , DevExpress.XtraPivotGrid.PivotCellEventArgs e )
        {
            Point point1=MousePosition;
            Point point=splitContainerControl1.Panel2.PointToClient( point1 );
            if ( 0<point.X-e.Bounds.Left&&point.X-e.Bounds.Left<18 )
            {
                RunFocusedLink( e.DataField as ABCPivotGridField , e.Value );
            }
        }

        Dictionary<ABCPivotGridField , bool> ForeignColumnList=new Dictionary<ABCPivotGridField , bool>();
        private void DrawLinkButton ( DevExpress.XtraPivotGrid.PivotCustomDrawCellEventArgs e )
        {
            if (this.OwnerView!=null&&this.OwnerView.Mode==ViewMode.Design )
                return;

            if (e.Value==DBNull.Value )
                return;

            if ( ( e.DataField is ABCPivotGridField==false )||( e.DataField as ABCPivotGridField ).Config==null||String.IsNullOrWhiteSpace( ( e.DataField as ABCPivotGridField ).Config.FieldName ) )
                return;

            ABCPivotGridField gridCol=e.DataField as ABCPivotGridField;

            Boolean isDraw=false;
            if ( ForeignColumnList.TryGetValue( gridCol , out isDraw ) )
            {
                if ( isDraw )
                    DrawButton( e );
                return;
            }

            if ( gridCol.Config.FieldName.Contains( ":" )==false )
            {

                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , e.DataField.FieldName ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( gridCol.Config.TableName , e.DataField.FieldName );
                    STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , strPKTableName ) );
                    if ( viewIfo!=null )
                        isDraw=true;
                }
            }
            else
            {

                DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( gridCol.Config.TableName , gridCol.Config.FieldName );
                if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                    ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                {
                    STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , acrrosInfo.TableName ) );
                    if ( viewIfo!=null )
                        isDraw=true;
                }
            }

            ForeignColumnList.Add( gridCol , isDraw );

            if ( isDraw )
                DrawButton( e );
        }

        private void DrawButton ( DevExpress.XtraPivotGrid.PivotCustomDrawCellEventArgs e )
        {
            e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "DocLink" ) , e.Bounds.Location );

            Rectangle r=e.Bounds;
            r.Width-=18;
            r.X+=18;
            e.Appearance.DrawString( e.GraphicsCache , e.DisplayText , r );

            e.Handled=true;
        }
        void Grid_CustomDrawCell ( object sender , DevExpress.XtraPivotGrid.PivotCustomDrawCellEventArgs e )
        {
            DrawLinkButton( e );


            //e.Appearance.BackColor=Color.FromArgb( 181 , 200 , 223 );
            //e.Appearance.BackColor2=Color.Transparent;
            
            //e.Appearance.Options.UseBackColor=true;

        }

        #endregion

        #region  Accross Table DisplayText

        void Grid_CustomCellDisplayText ( object sender , DevExpress.XtraPivotGrid.PivotCellDisplayTextEventArgs e )
        {
            if ( e.DataField==null||e.DataField is ABCPivotGridField==false )
                return;

            if ( this.OwnerView==null||this.OwnerView.Mode==ViewMode.Design )
                return;

            if ( e.ColumnIndex<0||e.Value==null||e.Value==DBNull.Value )
                return;

            ABCPivotGridField gridCol=e.DataField as ABCPivotGridField;
            if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false&&gridCol.Config.FieldName.Contains( ":" ) )
            {
                String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField ) )
                    e.DisplayText=DataFormatProvider.DoFormat( e.Value , gridCol.Config.TableName , gridCol.Config.FieldName );
            }
        }


        void Grid_CustomCellValue ( object sender , DevExpress.XtraPivotGrid.PivotCellValueEventArgs e )
        {
            if ( e.DataField==null||e.DataField is ABCPivotGridField==false )
                return;

            if ( this.OwnerView==null||this.OwnerView.Mode==ViewMode.Design )
                return;

            if ( e.ColumnIndex<0||e.Value==null||e.Value==DBNull.Value||e.Value.GetType()!=typeof( int ) )
                return;

            ABCPivotGridField gridCol=e.DataField as ABCPivotGridField;
            if ( gridCol.Config!=null&&String.IsNullOrWhiteSpace( gridCol.Config.FieldName )==false&&gridCol.Config.FieldName.Contains( ":" ) )
            {
                String strBaseField=gridCol.Config.FieldName.Split( ':' )[0];
                if ( DataStructureProvider.IsForeignKey( gridCol.Config.TableName , strBaseField ) )
                    e.Value=DataCachingProvider.GetCachingObjectAccrossTable( gridCol.Config.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value ) , gridCol.Config.FieldName );
            }
        }

        #endregion

        #endregion

        #region Layout

        #region Save
        public void GetChildrenXMLLayout ( XmlElement gridElement )
        {
            XmlNode nodeCol=ABCHelper.DOMXMLUtil.GetFirstNode( gridElement , "P" , "Collection(" );
            if ( nodeCol!=null )
                gridElement.RemoveChild( nodeCol );

            foreach ( ABCPivotGridField field in InnerGrid.Fields )
            {
                XmlElement ele=ABCPresentHelper.Serialization( gridElement.OwnerDocument , field.Config , "F" );
                gridElement.AppendChild( ele );
            }
        }
        #endregion

        #region Load
        public void InitLayout ( ABCView view , XmlNode gridNode )
        {
            OwnerView=view;
            this.FieldConfigs=new BindingList<ABCPivotGridField.FieldConfig>();

            foreach ( XmlNode nodeCol in gridNode.SelectNodes( "F" ) )
            {
                ABCPivotGridField.FieldConfig colInfo=new ABCPivotGridField.FieldConfig();
                ABCPresentHelper.DeSerialization( colInfo , nodeCol );
                this.FieldConfigs.Add( colInfo );
            }

            InitFields();

            LoadDataSourceFromScript();
        }
        public void InitFields ( )
        {

            InnerGrid.Fields.Clear();
            if ( this.FieldConfigs==null )
                return;

            SortedList<String , ABCPivotGridField.FieldConfig> lstSort=new SortedList<string , ABCPivotGridField.FieldConfig>();
            foreach ( ABCPivotGridField.FieldConfig colInfo in this.FieldConfigs )
                lstSort.Add( colInfo.AreaIndex+colInfo.Caption+colInfo.FieldName , colInfo );

            foreach ( ABCPivotGridField.FieldConfig fieldInfo in lstSort.Values )
            {
                ABCPivotGridField field=new ABCPivotGridField( fieldInfo ,this.TableName);
                if ( this.OwnerView==null||this.OwnerView.Mode!=ViewMode.Design )
                    field.Initialize();

                InnerGrid.Fields.Add( field );
            }

        }
        #endregion

        //first time
        public void Initialize ( ABCView view , ABCBindingInfo bindingInfo )
        {
            String strTableName=bindingInfo.TableName;
            if ( DataConfigProvider.TableConfigList.ContainsKey( strTableName )==false )
                return;

            OwnerView=view;

            this.DataSource=bindingInfo.BusName;
            this.TableName=strTableName;

            #region InitColumns

            #region BindingList<ABCPivotGridColumn.ColumnInfo>
            this.FieldConfigs=new BindingList<ABCPivotGridField.FieldConfig>();

            int iIndex=0;
            foreach ( DataConfigProvider.FieldConfig colAlias in DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values )
            {
                if ( colAlias.InUse==false )
                    continue;

                if ( DataStructureProvider.IsPrimaryKey( strTableName , colAlias.FieldName )
                    ||DataStructureProvider.IsForeignKey( strTableName , colAlias.FieldName ) )
                    continue;

                iIndex++;

                ABCPivotGridField.FieldConfig colInfo=new ABCPivotGridField.FieldConfig();
                colInfo.FieldName=colAlias.FieldName;
                colInfo.TableName=this.TableName;
                colInfo.AreaIndex=iIndex;
                colInfo.Index=iIndex;
                if ( colAlias.IsDefault )
                {
                    colInfo.Visible=true;
                    colInfo.AreaIndex=iIndex;
                    colInfo.Index=iIndex;
                }
                else
                {
                    colInfo.Visible=false;
                    colInfo.AreaIndex=-1;
                    colInfo.Index=0;
                }

                if ( ABCApp.ABCDataGlobal.Language=="VN" )
                    colInfo.Caption=colAlias.CaptionVN;
                else
                    colInfo.Caption=colAlias.CaptionEN;

                if ( String.IsNullOrWhiteSpace( colInfo.Caption )==false )
                     this.FieldConfigs.Add( colInfo );

            }

            #endregion

            this.InitFields();

            #endregion

            this.ShowColumnGrandTotals=false;
            this.ChartSizeMode=DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;
            this.PageMargins=new Margins( 50 , 50 , 40 , 50 );
            this.PaperKind=System.Drawing.Printing.PaperKind.A4;
            this.Landscape=false;
            this.ShowColumnGrandTotalHeader=false;
            this.ShowRowGrandTotalHeader=false;
            this.InnerGrid.BestFit();
        }
        public void Initialize ( String strTableName )
        {
            ABCBindingInfo bindIfo=new ABCBindingInfo();
            bindIfo.TableName=strTableName;
            this.TableName=strTableName;
            Initialize( null , bindIfo );
        }
        #endregion


        #endregion

        #region Row Detail
        //ABCPivotGridRowDetail RowDetail;
        //public void ShowRowDetail ( )
        //{
        //    if ( RowDetail==null )
        //        RowDetail=new ABCPivotGridRowDetail( this );
        //    RowDetail.Show();
        //}
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

        #region Chart

        [Category( "Chart" )]
        public DevExpress.XtraCharts.ViewType ChartType
        {
            get { return DevExpress.XtraCharts.Native.SeriesViewFactory.GetViewType(chartControl.Chart.SeriesTemplate.View);  }
            set
            {
                chartControl.ChangeChartType( value );
            }
        }

        [Category( "Chart" )]
        public bool UseChartControl 
        { 
            get { return useChart; } 
            set 
            { 
                useChart=value;
                ShowHideChartControl();
            } 
        }
        [Category( "Chart" )]
        public bool ChartHorizontal
        {
            get { return splitContainerControl1.Horizontal; }
            set
            {
                splitContainerControl1.Horizontal=value;
            }
        }
        private bool useChart=false;
        ABCPivotGridChartControl chartControl=new ABCPivotGridChartControl();
        public void ShowHideChartControl ( )
        {
            if ( splitContainerControl1.Panel2.Controls.Contains( chartControl )==false )
            {
                chartControl.SetPivotGridControl( this );
                chartControl.Dock=DockStyle.Fill;
                splitContainerControl1.Panel2.Controls.Add( chartControl );
            }

            if ( useChart )
                splitContainerControl1.PanelVisibility=SplitPanelVisibility.Both;
            else
                splitContainerControl1.PanelVisibility=SplitPanelVisibility.Panel1;
        }
     
        #endregion

        public void RefreshDataSource ( )
        {
            if ( this.InnerGrid!=null )
                this.InnerGrid.RefreshData();
        }

    }

    public class ABCPivotGridControlDesigner : DevExpress.XtraEditors.Design.BaseEditDesigner
    {
        public ABCPivotGridControlDesigner ( )
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
