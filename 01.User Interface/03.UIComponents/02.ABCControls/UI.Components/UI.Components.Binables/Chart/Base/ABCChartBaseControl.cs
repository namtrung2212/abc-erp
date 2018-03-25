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
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraEditors;
using  DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;

using ABCProvider;
using ABCCommon;

namespace ABCControls
{

    public class ABCAppearanceTypeConverter : DevExpress.XtraCharts.Design.AppearanceTypeConverter
    {
        public override StandardValuesCollection GetStandardValues ( ITypeDescriptorContext context )
        {
            ABCChartBaseControl chart=context.Instance as ABCChartBaseControl;
            String[] strNames=chart.InnerChart.GetAppearanceNames();
            return new StandardValuesCollection( strNames );
        }
    }
    public class ABCPaletteTypeConverter : DevExpress.XtraCharts.Design.PaletteTypeConverter
    {
        public override StandardValuesCollection GetStandardValues ( ITypeDescriptorContext context )
        {
            ABCChartBaseControl chart=context.Instance as ABCChartBaseControl;
            String[] strNames=chart.InnerChart.GetPaletteNames();
            return new StandardValuesCollection( strNames );
        }
    }

    [ToolboxBitmapAttribute( typeof( DevExpress.XtraCharts.ChartControl) )]
    [Designer( typeof( ABCChartBaseControlDesigner ) )]
    public class ABCChartBaseControl : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl , IABCCustomControl
    {
        #region IBindingableControl
        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }


        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [Browsable( false )]
        public String DataMember { get; set; }

        [Browsable( false )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "";
            }
        }

    
        #endregion

        public ABCChartBaseControl ( )
        {
            InitializeComponent();

           // ShowLegend=true;
            ShowMenuBar=false;
        }

        public DevExpress.XtraCharts.ChartControl InnerChart;
        public DevExpress.XtraCharts.XYDiagram InnerDiagram;

        #region InitializeComponent
        public void InitializeComponent ( )
        {
            this.InnerDiagram=new DevExpress.XtraCharts.XYDiagram();
            this.InnerChart=new DevExpress.XtraCharts.ChartControl();

            #region Bar - BeginInit
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DColumn chartControlCommandGalleryItemGroup2DColumn2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DColumn();
            DevExpress.XtraCharts.UI.CreateBarChartItem createBarChartItem2=new DevExpress.XtraCharts.UI.CreateBarChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedBarChartItem createFullStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideFullStackedBarChartItem createSideBySideFullStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideFullStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideStackedBarChartItem createSideBySideStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateStackedBarChartItem createStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateStackedBarChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DColumn chartControlCommandGalleryItemGroup3DColumn2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DColumn();
            DevExpress.XtraCharts.UI.CreateBar3DChartItem createBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedBar3DChartItem createFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateManhattanBarChartItem createManhattanBarChartItem2=new DevExpress.XtraCharts.UI.CreateManhattanBarChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideFullStackedBar3DChartItem createSideBySideFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideStackedBar3DChartItem createSideBySideStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateStackedBar3DChartItem createStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupCylinderColumn chartControlCommandGalleryItemGroupCylinderColumn2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupCylinderColumn();
            DevExpress.XtraCharts.UI.CreateCylinderBar3DChartItem createCylinderBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateCylinderFullStackedBar3DChartItem createCylinderFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateCylinderManhattanBarChartItem createCylinderManhattanBarChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderManhattanBarChartItem();
            DevExpress.XtraCharts.UI.CreateCylinderSideBySideFullStackedBar3DChartItem createCylinderSideBySideFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderSideBySideFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateCylinderSideBySideStackedBar3DChartItem createCylinderSideBySideStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderSideBySideStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateCylinderStackedBar3DChartItem createCylinderStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateCylinderStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupConeColumn chartControlCommandGalleryItemGroupConeColumn2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupConeColumn();
            DevExpress.XtraCharts.UI.CreateConeBar3DChartItem createConeBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateConeBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateConeFullStackedBar3DChartItem createConeFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateConeFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateConeManhattanBarChartItem createConeManhattanBarChartItem2=new DevExpress.XtraCharts.UI.CreateConeManhattanBarChartItem();
            DevExpress.XtraCharts.UI.CreateConeSideBySideFullStackedBar3DChartItem createConeSideBySideFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateConeSideBySideFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateConeSideBySideStackedBar3DChartItem createConeSideBySideStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateConeSideBySideStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreateConeStackedBar3DChartItem createConeStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreateConeStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPyramidColumn chartControlCommandGalleryItemGroupPyramidColumn2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPyramidColumn();
            DevExpress.XtraCharts.UI.CreatePyramidBar3DChartItem createPyramidBar3DChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidBar3DChartItem();
            DevExpress.XtraCharts.UI.CreatePyramidFullStackedBar3DChartItem createPyramidFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreatePyramidManhattanBarChartItem createPyramidManhattanBarChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidManhattanBarChartItem();
            DevExpress.XtraCharts.UI.CreatePyramidSideBySideFullStackedBar3DChartItem createPyramidSideBySideFullStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidSideBySideFullStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreatePyramidSideBySideStackedBar3DChartItem createPyramidSideBySideStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidSideBySideStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.CreatePyramidStackedBar3DChartItem createPyramidStackedBar3DChartItem2=new DevExpress.XtraCharts.UI.CreatePyramidStackedBar3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DLine chartControlCommandGalleryItemGroup2DLine2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DLine();
            DevExpress.XtraCharts.UI.CreateLineChartItem createLineChartItem2=new DevExpress.XtraCharts.UI.CreateLineChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedLineChartItem createFullStackedLineChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedLineChartItem();
            DevExpress.XtraCharts.UI.CreateScatterLineChartItem createScatterLineChartItem2=new DevExpress.XtraCharts.UI.CreateScatterLineChartItem();
            DevExpress.XtraCharts.UI.CreateSplineChartItem createSplineChartItem2=new DevExpress.XtraCharts.UI.CreateSplineChartItem();
            DevExpress.XtraCharts.UI.CreateStackedLineChartItem createStackedLineChartItem2=new DevExpress.XtraCharts.UI.CreateStackedLineChartItem();
            DevExpress.XtraCharts.UI.CreateStepLineChartItem createStepLineChartItem2=new DevExpress.XtraCharts.UI.CreateStepLineChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DLine chartControlCommandGalleryItemGroup3DLine2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DLine();
            DevExpress.XtraCharts.UI.CreateLine3DChartItem createLine3DChartItem2=new DevExpress.XtraCharts.UI.CreateLine3DChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedLine3DChartItem createFullStackedLine3DChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedLine3DChartItem();
            DevExpress.XtraCharts.UI.CreateSpline3DChartItem createSpline3DChartItem2=new DevExpress.XtraCharts.UI.CreateSpline3DChartItem();
            DevExpress.XtraCharts.UI.CreateStackedLine3DChartItem createStackedLine3DChartItem2=new DevExpress.XtraCharts.UI.CreateStackedLine3DChartItem();
            DevExpress.XtraCharts.UI.CreateStepLine3DChartItem createStepLine3DChartItem2=new DevExpress.XtraCharts.UI.CreateStepLine3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DPie chartControlCommandGalleryItemGroup2DPie2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DPie();
            DevExpress.XtraCharts.UI.CreatePieChartItem createPieChartItem2=new DevExpress.XtraCharts.UI.CreatePieChartItem();
            DevExpress.XtraCharts.UI.CreateDoughnutChartItem createDoughnutChartItem2=new DevExpress.XtraCharts.UI.CreateDoughnutChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DPie chartControlCommandGalleryItemGroup3DPie2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DPie();
            DevExpress.XtraCharts.UI.CreatePie3DChartItem createPie3DChartItem2=new DevExpress.XtraCharts.UI.CreatePie3DChartItem();
            DevExpress.XtraCharts.UI.CreateDoughnut3DChartItem createDoughnut3DChartItem2=new DevExpress.XtraCharts.UI.CreateDoughnut3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DBar chartControlCommandGalleryItemGroup2DBar2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DBar();
            DevExpress.XtraCharts.UI.CreateRotatedBarChartItem createRotatedBarChartItem2=new DevExpress.XtraCharts.UI.CreateRotatedBarChartItem();
            DevExpress.XtraCharts.UI.CreateRotatedFullStackedBarChartItem createRotatedFullStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateRotatedFullStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateRotatedSideBySideFullStackedBarChartItem createRotatedSideBySideFullStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateRotatedSideBySideFullStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateRotatedSideBySideStackedBarChartItem createRotatedSideBySideStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateRotatedSideBySideStackedBarChartItem();
            DevExpress.XtraCharts.UI.CreateRotatedStackedBarChartItem createRotatedStackedBarChartItem2=new DevExpress.XtraCharts.UI.CreateRotatedStackedBarChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DArea chartControlCommandGalleryItemGroup2DArea2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup2DArea();
            DevExpress.XtraCharts.UI.CreateAreaChartItem createAreaChartItem2=new DevExpress.XtraCharts.UI.CreateAreaChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedAreaChartItem createFullStackedAreaChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedAreaChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedSplineAreaChartItem createFullStackedSplineAreaChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedSplineAreaChartItem();
            DevExpress.XtraCharts.UI.CreateSplineAreaChartItem createSplineAreaChartItem2=new DevExpress.XtraCharts.UI.CreateSplineAreaChartItem();
            DevExpress.XtraCharts.UI.CreateStackedAreaChartItem createStackedAreaChartItem2=new DevExpress.XtraCharts.UI.CreateStackedAreaChartItem();
            DevExpress.XtraCharts.UI.CreateStackedSplineAreaChartItem createStackedSplineAreaChartItem2=new DevExpress.XtraCharts.UI.CreateStackedSplineAreaChartItem();
            DevExpress.XtraCharts.UI.CreateStepAreaChartItem createStepAreaChartItem2=new DevExpress.XtraCharts.UI.CreateStepAreaChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DArea chartControlCommandGalleryItemGroup3DArea2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroup3DArea();
            DevExpress.XtraCharts.UI.CreateArea3DChartItem createArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedArea3DChartItem createFullStackedArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateFullStackedSplineArea3DChartItem createFullStackedSplineArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateFullStackedSplineArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateSplineArea3DChartItem createSplineArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateSplineArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateStackedArea3DChartItem createStackedArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateStackedArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateStackedSplineArea3DChartItem createStackedSplineArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateStackedSplineArea3DChartItem();
            DevExpress.XtraCharts.UI.CreateStepArea3DChartItem createStepArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateStepArea3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPoint chartControlCommandGalleryItemGroupPoint2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPoint();
            DevExpress.XtraCharts.UI.CreatePointChartItem createPointChartItem2=new DevExpress.XtraCharts.UI.CreatePointChartItem();
            DevExpress.XtraCharts.UI.CreateBubbleChartItem createBubbleChartItem2=new DevExpress.XtraCharts.UI.CreateBubbleChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupFunnel chartControlCommandGalleryItemGroupFunnel2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupFunnel();
            DevExpress.XtraCharts.UI.CreateFunnelChartItem createFunnelChartItem2=new DevExpress.XtraCharts.UI.CreateFunnelChartItem();
            DevExpress.XtraCharts.UI.CreateFunnel3DChartItem createFunnel3DChartItem2=new DevExpress.XtraCharts.UI.CreateFunnel3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupFinancial chartControlCommandGalleryItemGroupFinancial2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupFinancial();
            DevExpress.XtraCharts.UI.CreateStockChartItem createStockChartItem2=new DevExpress.XtraCharts.UI.CreateStockChartItem();
            DevExpress.XtraCharts.UI.CreateCandleStickChartItem createCandleStickChartItem2=new DevExpress.XtraCharts.UI.CreateCandleStickChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupRadar chartControlCommandGalleryItemGroupRadar2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupRadar();
            DevExpress.XtraCharts.UI.CreateRadarPointChartItem createRadarPointChartItem2=new DevExpress.XtraCharts.UI.CreateRadarPointChartItem();
            DevExpress.XtraCharts.UI.CreateRadarLineChartItem createRadarLineChartItem2=new DevExpress.XtraCharts.UI.CreateRadarLineChartItem();
            DevExpress.XtraCharts.UI.CreateRadarAreaChartItem createRadarAreaChartItem2=new DevExpress.XtraCharts.UI.CreateRadarAreaChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPolar chartControlCommandGalleryItemGroupPolar2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupPolar();
            DevExpress.XtraCharts.UI.CreatePolarPointChartItem createPolarPointChartItem2=new DevExpress.XtraCharts.UI.CreatePolarPointChartItem();
            DevExpress.XtraCharts.UI.CreatePolarLineChartItem createPolarLineChartItem2=new DevExpress.XtraCharts.UI.CreatePolarLineChartItem();
            DevExpress.XtraCharts.UI.CreatePolarAreaChartItem createPolarAreaChartItem2=new DevExpress.XtraCharts.UI.CreatePolarAreaChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupRange chartControlCommandGalleryItemGroupRange2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupRange();
            DevExpress.XtraCharts.UI.CreateRangeBarChartItem createRangeBarChartItem2=new DevExpress.XtraCharts.UI.CreateRangeBarChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideRangeBarChartItem createSideBySideRangeBarChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideRangeBarChartItem();
            DevExpress.XtraCharts.UI.CreateRangeAreaChartItem createRangeAreaChartItem2=new DevExpress.XtraCharts.UI.CreateRangeAreaChartItem();
            DevExpress.XtraCharts.UI.CreateRangeArea3DChartItem createRangeArea3DChartItem2=new DevExpress.XtraCharts.UI.CreateRangeArea3DChartItem();
            DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupGantt chartControlCommandGalleryItemGroupGantt2=new DevExpress.XtraCharts.UI.ChartControlCommandGalleryItemGroupGantt();
            DevExpress.XtraCharts.UI.CreateGanttChartItem createGanttChartItem2=new DevExpress.XtraCharts.UI.CreateGanttChartItem();
            DevExpress.XtraCharts.UI.CreateSideBySideGanttChartItem createSideBySideGanttChartItem2=new DevExpress.XtraCharts.UI.CreateSideBySideGanttChartItem();
            DevExpress.Skins.SkinPaddingEdges skinPaddingEdges3=new DevExpress.Skins.SkinPaddingEdges();
            DevExpress.Skins.SkinPaddingEdges skinPaddingEdges4=new DevExpress.Skins.SkinPaddingEdges();
            this.barManager1=new DevExpress.XtraBars.BarManager();
            this.menuBar=new DevExpress.XtraCharts.UI.ChartTypeBar();
            this.createBarBaseItem1=new DevExpress.XtraCharts.UI.CreateBarBaseItem();
            this.galleryDropDown1=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.createLineBaseItem1=new DevExpress.XtraCharts.UI.CreateLineBaseItem();
            this.galleryDropDown2=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.createPieBaseItem1=new DevExpress.XtraCharts.UI.CreatePieBaseItem();
            this.galleryDropDown3=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.createRotatedBarBaseItem1=new DevExpress.XtraCharts.UI.CreateRotatedBarBaseItem();
            this.galleryDropDown4=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.createAreaBaseItem1=new DevExpress.XtraCharts.UI.CreateAreaBaseItem();
            this.galleryDropDown5=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.createOtherSeriesTypesBaseItem1=new DevExpress.XtraCharts.UI.CreateOtherSeriesTypesBaseItem();
            this.galleryDropDown6=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.changePaletteGalleryBaseItem1=new DevExpress.XtraCharts.UI.ChangePaletteGalleryBaseItem();
            this.galleryDropDown7=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.changeAppearanceGalleryBaseBarManagerItem1=new DevExpress.XtraCharts.UI.ChangeAppearanceGalleryBaseBarManagerItem();
            this.galleryDropDown8=new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.printChartItem1=new DevExpress.XtraCharts.UI.PrintChartItem();
            this.createExportBaseItem1=new DevExpress.XtraCharts.UI.CreateExportBaseItem();
            this.exportToPDFChartItem1=new DevExpress.XtraCharts.UI.ExportToPDFChartItem();
            this.exportToHTMLChartItem1=new DevExpress.XtraCharts.UI.ExportToHTMLChartItem();
            this.exportToMHTChartItem1=new DevExpress.XtraCharts.UI.ExportToMHTChartItem();
            this.exportToXLSChartItem1=new DevExpress.XtraCharts.UI.ExportToXLSChartItem();
            this.exportToXLSXChartItem1=new DevExpress.XtraCharts.UI.ExportToXLSXChartItem();
            this.exportToRTFChartItem1=new DevExpress.XtraCharts.UI.ExportToRTFChartItem();
            this.createExportToImageBaseItem1=new DevExpress.XtraCharts.UI.CreateExportToImageBaseItem();
            this.exportToBMPChartItem1=new DevExpress.XtraCharts.UI.ExportToBMPChartItem();
            this.exportToGIFChartItem1=new DevExpress.XtraCharts.UI.ExportToGIFChartItem();
            this.exportToJPEGChartItem1=new DevExpress.XtraCharts.UI.ExportToJPEGChartItem();
            this.exportToPNGChartItem1=new DevExpress.XtraCharts.UI.ExportToPNGChartItem();
            this.exportToTIFFChartItem1=new DevExpress.XtraCharts.UI.ExportToTIFFChartItem();
            this.printPreviewChartItem1=new DevExpress.XtraCharts.UI.PrintPreviewChartItem();

            this.chartBarController1=new DevExpress.XtraCharts.UI.ChartBarController();

            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown2 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown3 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown4 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown5 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown6 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown7 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown8 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chartBarController1 ) ).BeginInit(); 
          
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange( new DevExpress.XtraBars.Bar[] {
            this.menuBar} );
            this.barManager1.Form=this;
            this.barManager1.Items.AddRange( new DevExpress.XtraBars.BarItem[] {
            this.createBarBaseItem1,
            this.createLineBaseItem1,
            this.createPieBaseItem1,
            this.createRotatedBarBaseItem1,
            this.createAreaBaseItem1,
            this.createOtherSeriesTypesBaseItem1,
            this.changePaletteGalleryBaseItem1,
            this.changeAppearanceGalleryBaseBarManagerItem1,
            this.printPreviewChartItem1,
            this.printChartItem1,
            this.createExportBaseItem1,
            this.exportToPDFChartItem1,
            this.exportToHTMLChartItem1,
            this.exportToMHTChartItem1,
            this.exportToXLSChartItem1,
            this.exportToXLSXChartItem1,
            this.exportToRTFChartItem1,
            this.exportToBMPChartItem1,
            this.exportToGIFChartItem1,
            this.exportToJPEGChartItem1,
            this.exportToPNGChartItem1,
            this.exportToTIFFChartItem1,
            this.createExportToImageBaseItem1} );
            this.barManager1.MaxItemId=27;
            // 
            // chartTypeBar1
            // 
            this.menuBar.DockCol=0;
            this.menuBar.DockRow=0;
            this.menuBar.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            this.menuBar.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.createBarBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createLineBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createPieBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createRotatedBarBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createAreaBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createOtherSeriesTypesBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changePaletteGalleryBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeAppearanceGalleryBaseBarManagerItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.printChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createExportBaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.printPreviewChartItem1)} );
            this.menuBar.OptionsBar.AllowQuickCustomization=false;
            this.menuBar.OptionsBar.DisableCustomization=true;
            this.menuBar.OptionsBar.DrawDragBorder=false;
            this.menuBar.OptionsBar.UseWholeRow=true;
            // 
            // createBarBaseItem1
            // 
            this.createBarBaseItem1.DropDownControl=this.galleryDropDown1;
            this.createBarBaseItem1.Id=0;
            this.createBarBaseItem1.Name="createBarBaseItem1";
            // 
            // galleryDropDown1
            // 
            // 
            // galleryDropDown1
            // 
            this.galleryDropDown1.Gallery.AllowFilter=false;
            this.galleryDropDown1.Gallery.ColumnCount=4;
            chartControlCommandGalleryItemGroup2DColumn2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createBarChartItem2,
            createFullStackedBarChartItem2,
            createSideBySideFullStackedBarChartItem2,
            createSideBySideStackedBarChartItem2,
            createStackedBarChartItem2} );
            chartControlCommandGalleryItemGroup3DColumn2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createBar3DChartItem2,
            createFullStackedBar3DChartItem2,
            createManhattanBarChartItem2,
            createSideBySideFullStackedBar3DChartItem2,
            createSideBySideStackedBar3DChartItem2,
            createStackedBar3DChartItem2} );
            chartControlCommandGalleryItemGroupCylinderColumn2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createCylinderBar3DChartItem2,
            createCylinderFullStackedBar3DChartItem2,
            createCylinderManhattanBarChartItem2,
            createCylinderSideBySideFullStackedBar3DChartItem2,
            createCylinderSideBySideStackedBar3DChartItem2,
            createCylinderStackedBar3DChartItem2} );
            chartControlCommandGalleryItemGroupConeColumn2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createConeBar3DChartItem2,
            createConeFullStackedBar3DChartItem2,
            createConeManhattanBarChartItem2,
            createConeSideBySideFullStackedBar3DChartItem2,
            createConeSideBySideStackedBar3DChartItem2,
            createConeStackedBar3DChartItem2} );
            chartControlCommandGalleryItemGroupPyramidColumn2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createPyramidBar3DChartItem2,
            createPyramidFullStackedBar3DChartItem2,
            createPyramidManhattanBarChartItem2,
            createPyramidSideBySideFullStackedBar3DChartItem2,
            createPyramidSideBySideStackedBar3DChartItem2,
            createPyramidStackedBar3DChartItem2} );
            this.galleryDropDown1.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroup2DColumn2,
            chartControlCommandGalleryItemGroup3DColumn2,
            chartControlCommandGalleryItemGroupCylinderColumn2,
            chartControlCommandGalleryItemGroupConeColumn2,
            chartControlCommandGalleryItemGroupPyramidColumn2} );
            this.galleryDropDown1.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown1.Gallery.RowCount=10;
            this.galleryDropDown1.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown1.Manager=this.barManager1;
            this.galleryDropDown1.Name="galleryDropDown1";
            // 
            // createLineBaseItem1
            // 
            this.createLineBaseItem1.DropDownControl=this.galleryDropDown2;
            this.createLineBaseItem1.Id=1;
            this.createLineBaseItem1.Name="createLineBaseItem1";
            // 
            // galleryDropDown2
            // 
            // 
            // galleryDropDown2
            // 
            this.galleryDropDown2.Gallery.AllowFilter=false;
            this.galleryDropDown2.Gallery.ColumnCount=3;
            chartControlCommandGalleryItemGroup2DLine2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createLineChartItem2,
            createFullStackedLineChartItem2,
            createScatterLineChartItem2,
            createSplineChartItem2,
            createStackedLineChartItem2,
            createStepLineChartItem2} );
            chartControlCommandGalleryItemGroup3DLine2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createLine3DChartItem2,
            createFullStackedLine3DChartItem2,
            createSpline3DChartItem2,
            createStackedLine3DChartItem2,
            createStepLine3DChartItem2} );
            this.galleryDropDown2.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroup2DLine2,
            chartControlCommandGalleryItemGroup3DLine2} );
            this.galleryDropDown2.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown2.Gallery.RowCount=4;
            this.galleryDropDown2.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown2.Manager=this.barManager1;
            this.galleryDropDown2.Name="galleryDropDown2";
            // 
            // createPieBaseItem1
            // 
            this.createPieBaseItem1.DropDownControl=this.galleryDropDown3;
            this.createPieBaseItem1.Id=2;
            this.createPieBaseItem1.Name="createPieBaseItem1";
            // 
            // galleryDropDown3
            // 
            // 
            // galleryDropDown3
            // 
            this.galleryDropDown3.Gallery.AllowFilter=false;
            this.galleryDropDown3.Gallery.ColumnCount=2;
            chartControlCommandGalleryItemGroup2DPie2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createPieChartItem2,
            createDoughnutChartItem2} );
            chartControlCommandGalleryItemGroup3DPie2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createPie3DChartItem2,
            createDoughnut3DChartItem2} );
            this.galleryDropDown3.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroup2DPie2,
            chartControlCommandGalleryItemGroup3DPie2} );
            this.galleryDropDown3.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown3.Gallery.RowCount=2;
            this.galleryDropDown3.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown3.Manager=this.barManager1;
            this.galleryDropDown3.Name="galleryDropDown3";
            // 
            // createRotatedBarBaseItem1
            // 
            this.createRotatedBarBaseItem1.DropDownControl=this.galleryDropDown4;
            this.createRotatedBarBaseItem1.Id=3;
            this.createRotatedBarBaseItem1.Name="createRotatedBarBaseItem1";
            // 
            // galleryDropDown4
            // 
            // 
            // galleryDropDown4
            // 
            this.galleryDropDown4.Gallery.AllowFilter=false;
            this.galleryDropDown4.Gallery.ColumnCount=3;
            chartControlCommandGalleryItemGroup2DBar2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createRotatedBarChartItem2,
            createRotatedFullStackedBarChartItem2,
            createRotatedSideBySideFullStackedBarChartItem2,
            createRotatedSideBySideStackedBarChartItem2,
            createRotatedStackedBarChartItem2} );
            this.galleryDropDown4.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroup2DBar2} );
            this.galleryDropDown4.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown4.Gallery.RowCount=2;
            this.galleryDropDown4.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown4.Manager=this.barManager1;
            this.galleryDropDown4.Name="galleryDropDown4";
            // 
            // createAreaBaseItem1
            // 
            this.createAreaBaseItem1.DropDownControl=this.galleryDropDown5;
            this.createAreaBaseItem1.Id=4;
            this.createAreaBaseItem1.Name="createAreaBaseItem1";
            // 
            // galleryDropDown5
            // 
            // 
            // galleryDropDown5
            // 
            this.galleryDropDown5.Gallery.AllowFilter=false;
            this.galleryDropDown5.Gallery.ColumnCount=4;
            chartControlCommandGalleryItemGroup2DArea2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createAreaChartItem2,
            createFullStackedAreaChartItem2,
            createFullStackedSplineAreaChartItem2,
            createSplineAreaChartItem2,
            createStackedAreaChartItem2,
            createStackedSplineAreaChartItem2,
            createStepAreaChartItem2} );
            chartControlCommandGalleryItemGroup3DArea2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createArea3DChartItem2,
            createFullStackedArea3DChartItem2,
            createFullStackedSplineArea3DChartItem2,
            createSplineArea3DChartItem2,
            createStackedArea3DChartItem2,
            createStackedSplineArea3DChartItem2,
            createStepArea3DChartItem2} );
            this.galleryDropDown5.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroup2DArea2,
            chartControlCommandGalleryItemGroup3DArea2} );
            this.galleryDropDown5.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown5.Gallery.RowCount=4;
            this.galleryDropDown5.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown5.Manager=this.barManager1;
            this.galleryDropDown5.Name="galleryDropDown5";
            // 
            // createOtherSeriesTypesBaseItem1
            // 
            this.createOtherSeriesTypesBaseItem1.DropDownControl=this.galleryDropDown6;
            this.createOtherSeriesTypesBaseItem1.Id=5;
            this.createOtherSeriesTypesBaseItem1.Name="createOtherSeriesTypesBaseItem1";
            // 
            // galleryDropDown6
            // 
            // 
            // galleryDropDown6
            // 
            this.galleryDropDown6.Gallery.AllowFilter=false;
            this.galleryDropDown6.Gallery.ColumnCount=4;
            chartControlCommandGalleryItemGroupPoint2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createPointChartItem2,
            createBubbleChartItem2} );
            chartControlCommandGalleryItemGroupFunnel2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createFunnelChartItem2,
            createFunnel3DChartItem2} );
            chartControlCommandGalleryItemGroupFinancial2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createStockChartItem2,
            createCandleStickChartItem2} );
            chartControlCommandGalleryItemGroupRadar2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createRadarPointChartItem2,
            createRadarLineChartItem2,
            createRadarAreaChartItem2} );
            chartControlCommandGalleryItemGroupPolar2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createPolarPointChartItem2,
            createPolarLineChartItem2,
            createPolarAreaChartItem2} );
            chartControlCommandGalleryItemGroupRange2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createRangeBarChartItem2,
            createSideBySideRangeBarChartItem2,
            createRangeAreaChartItem2,
            createRangeArea3DChartItem2} );
            chartControlCommandGalleryItemGroupGantt2.Items.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            createGanttChartItem2,
            createSideBySideGanttChartItem2} );
            this.galleryDropDown6.Gallery.Groups.AddRange( new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            chartControlCommandGalleryItemGroupPoint2,
            chartControlCommandGalleryItemGroupFunnel2,
            chartControlCommandGalleryItemGroupFinancial2,
            chartControlCommandGalleryItemGroupRadar2,
            chartControlCommandGalleryItemGroupPolar2,
            chartControlCommandGalleryItemGroupRange2,
            chartControlCommandGalleryItemGroupGantt2} );
            this.galleryDropDown6.Gallery.ImageSize=new System.Drawing.Size( 32 , 32 );
            this.galleryDropDown6.Gallery.RowCount=7;
            this.galleryDropDown6.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown6.Manager=this.barManager1;
            this.galleryDropDown6.Name="galleryDropDown6";
            // 
            // changePaletteGalleryBaseItem1
            // 
            this.changePaletteGalleryBaseItem1.DropDownControl=this.galleryDropDown7;
            this.changePaletteGalleryBaseItem1.Id=6;
            this.changePaletteGalleryBaseItem1.Name="changePaletteGalleryBaseItem1";
            // 
            // galleryDropDown7
            // 
            // 
            // galleryDropDown7
            // 
            this.galleryDropDown7.Gallery.AllowFilter=false;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Hovered.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Hovered.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Hovered.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Normal.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Pressed.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Pressed.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemCaptionAppearance.Pressed.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Hovered.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Hovered.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Hovered.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Normal.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Normal.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Normal.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Pressed.Options.UseFont=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Pressed.Options.UseTextOptions=true;
            this.galleryDropDown7.Gallery.Appearance.ItemDescriptionAppearance.Pressed.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Near;
            this.galleryDropDown7.Gallery.ColumnCount=1;
            this.galleryDropDown7.Gallery.ImageSize=new System.Drawing.Size( 160 , 10 );
            this.galleryDropDown7.Gallery.ItemImageLayout=DevExpress.Utils.Drawing.ImageLayoutMode.MiddleLeft;
            this.galleryDropDown7.Gallery.ItemImageLocation=DevExpress.Utils.Locations.Right;
            skinPaddingEdges3.Bottom=-3;
            skinPaddingEdges3.Top=-3;
            this.galleryDropDown7.Gallery.ItemImagePadding=skinPaddingEdges3;
            skinPaddingEdges4.Bottom=-3;
            skinPaddingEdges4.Top=-3;
            this.galleryDropDown7.Gallery.ItemTextPadding=skinPaddingEdges4;
            this.galleryDropDown7.Gallery.RowCount=10;
            this.galleryDropDown7.Gallery.ShowGroupCaption=false;
            this.galleryDropDown7.Gallery.ShowItemText=true;
            this.galleryDropDown7.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown7.Manager=this.barManager1;
            this.galleryDropDown7.Name="galleryDropDown7";
            // 
            // changeAppearanceGalleryBaseBarManagerItem1
            // 
            this.changeAppearanceGalleryBaseBarManagerItem1.DropDownControl=this.galleryDropDown8;
            this.changeAppearanceGalleryBaseBarManagerItem1.Id=7;
            this.changeAppearanceGalleryBaseBarManagerItem1.Name="changeAppearanceGalleryBaseBarManagerItem1";
            // 
            // galleryDropDown8
            // 
            // 
            // galleryDropDown8
            // 
            this.galleryDropDown8.Gallery.AllowFilter=false;
            this.galleryDropDown8.Gallery.ColumnCount=7;
            this.galleryDropDown8.Gallery.ImageSize=new System.Drawing.Size( 80 , 50 );
            this.galleryDropDown8.Gallery.RowCount=4;
            this.galleryDropDown8.Gallery.ShowGroupCaption=false;
            this.galleryDropDown8.Gallery.ShowScrollBar=DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Auto;
            this.galleryDropDown8.Manager=this.barManager1;
            this.galleryDropDown8.Name="galleryDropDown8";
            // 
            // printChartItem1
            // 
            this.printChartItem1.Id=12;
            this.printChartItem1.Name="printChartItem1";
            // 
            // createExportBaseItem1
            // 
            this.createExportBaseItem1.Id=13;
            this.createExportBaseItem1.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToPDFChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToHTMLChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToMHTChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToXLSChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToXLSXChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToRTFChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.createExportToImageBaseItem1)} );
            this.createExportBaseItem1.MenuDrawMode=DevExpress.XtraBars.MenuDrawMode.SmallImagesText;
            this.createExportBaseItem1.Name="createExportBaseItem1";
            // 
            // exportToPDFChartItem1
            // 
            this.exportToPDFChartItem1.Id=14;
            this.exportToPDFChartItem1.Name="exportToPDFChartItem1";
            // 
            // exportToHTMLChartItem1
            // 
            this.exportToHTMLChartItem1.Id=15;
            this.exportToHTMLChartItem1.Name="exportToHTMLChartItem1";
            // 
            // exportToMHTChartItem1
            // 
            this.exportToMHTChartItem1.Id=16;
            this.exportToMHTChartItem1.Name="exportToMHTChartItem1";
            // 
            // exportToXLSChartItem1
            // 
            this.exportToXLSChartItem1.Id=17;
            this.exportToXLSChartItem1.Name="exportToXLSChartItem1";
            // 
            // exportToXLSXChartItem1
            // 
            this.exportToXLSXChartItem1.Id=18;
            this.exportToXLSXChartItem1.Name="exportToXLSXChartItem1";
            // 
            // exportToRTFChartItem1
            // 
            this.exportToRTFChartItem1.Id=19;
            this.exportToRTFChartItem1.Name="exportToRTFChartItem1";
            // 
            // createExportToImageBaseItem1
            // 
            this.createExportToImageBaseItem1.Id=20;
            this.createExportToImageBaseItem1.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToBMPChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToGIFChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToJPEGChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToPNGChartItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.exportToTIFFChartItem1)} );
            this.createExportToImageBaseItem1.MenuDrawMode=DevExpress.XtraBars.MenuDrawMode.SmallImagesText;
            this.createExportToImageBaseItem1.Name="createExportToImageBaseItem1";
            // 
            // exportToBMPChartItem1
            // 
            this.exportToBMPChartItem1.Id=21;
            this.exportToBMPChartItem1.Name="exportToBMPChartItem1";
            // 
            // exportToGIFChartItem1
            // 
            this.exportToGIFChartItem1.Id=22;
            this.exportToGIFChartItem1.Name="exportToGIFChartItem1";
            // 
            // exportToJPEGChartItem1
            // 
            this.exportToJPEGChartItem1.Id=23;
            this.exportToJPEGChartItem1.Name="exportToJPEGChartItem1";
            // 
            // exportToPNGChartItem1
            // 
            this.exportToPNGChartItem1.Id=24;
            this.exportToPNGChartItem1.Name="exportToPNGChartItem1";
            // 
            // exportToTIFFChartItem1
            // 
            this.exportToTIFFChartItem1.Id=25;
            this.exportToTIFFChartItem1.Name="exportToTIFFChartItem1";
            // 
            // printPreviewChartItem1
            // 
            this.printPreviewChartItem1.Id=11;
            this.printPreviewChartItem1.Name="printPreviewChartItem1";
         
            // 
            // chartBarController1
            // 
            this.chartBarController1.BarItems.Add( this.createBarBaseItem1 );
            this.chartBarController1.BarItems.Add( this.createLineBaseItem1 );
            this.chartBarController1.BarItems.Add( this.createPieBaseItem1 );
            this.chartBarController1.BarItems.Add( this.createRotatedBarBaseItem1 );
            this.chartBarController1.BarItems.Add( this.createAreaBaseItem1 );
            this.chartBarController1.BarItems.Add( this.createOtherSeriesTypesBaseItem1 );
            this.chartBarController1.BarItems.Add( this.changePaletteGalleryBaseItem1 );
            this.chartBarController1.BarItems.Add( this.changeAppearanceGalleryBaseBarManagerItem1 );
            this.chartBarController1.BarItems.Add( this.printPreviewChartItem1 );
            this.chartBarController1.BarItems.Add( this.printChartItem1 );
            this.chartBarController1.BarItems.Add( this.createExportBaseItem1 );
            this.chartBarController1.BarItems.Add( this.exportToPDFChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToHTMLChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToMHTChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToXLSChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToXLSXChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToRTFChartItem1 );
            this.chartBarController1.BarItems.Add( this.createExportToImageBaseItem1 );
            this.chartBarController1.BarItems.Add( this.exportToBMPChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToGIFChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToJPEGChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToPNGChartItem1 );
            this.chartBarController1.BarItems.Add( this.exportToTIFFChartItem1 ); 
            #endregion

            BeginInitialize();

            #region Init Chart

            // 
            // InnerChart
            // 
            //this.InnerChart.AppearanceNameSerializable="Pastel Kit";
            this.InnerChart.BackColor=ABCPresentHelper.GetSkinBackColor();

            this.InnerChart.BorderOptions.Visible=false;
            //   InnerDiagram.LayoutDirection=DevExpress.XtraCharts.LayoutDirection.Vertical;
            //   InnerDiagram.PerspectiveAngle=3;
            //   InnerDiagram.RotationMatrixSerializable="0.712152295966167;0.499431057763122;-0.493363685217889;0;0.612064690171736;-0.785"+
            //     "904806285531;0.0879229805131639;0;-0.343825424300091;-0.364585043575504;-0.86536"+
            //      "8027839002;0;0;0;0;1";
            InnerDiagram.AxisX.Label.ResolveOverlappingMode=AxisLabelResolveOverlappingMode.HideOverlapped;
            this.InnerChart.Diagram=InnerDiagram;

            this.InnerChart.Dock=System.Windows.Forms.DockStyle.Fill;
            this.InnerChart.EmptyChartText.Font=new System.Drawing.Font( "Tahoma" , 14F );
            this.InnerChart.EmptyChartText.Text="Không có dữ liệu.";
            this.InnerChart.EmptyChartText.TextColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 128 ) ) ) ) , ( (int)( ( (byte)( 128 ) ) ) ) , ( (int)( ( (byte)( 128 ) ) ) ) );
            this.InnerChart.Legend.BackColor=System.Drawing.Color.Transparent;
            this.InnerChart.Legend.Border.Visible=false;
            this.InnerChart.Legend.Margins.Bottom=1;
            this.InnerChart.Legend.Margins.Left=1;
            this.InnerChart.Legend.Margins.Right=1;
            this.InnerChart.Legend.Margins.Top=1;
            this.InnerChart.Legend.Padding.Bottom=0;
            this.InnerChart.Legend.Padding.Left=0;
            this.InnerChart.Legend.Padding.Right=0;
            this.InnerChart.Legend.Padding.Top=0;
            this.InnerChart.Legend.TextColor=ABCPresentHelper.GetSkinForeColor();
            this.InnerChart.Legend.Visible=false;
            this.InnerChart.Location=new System.Drawing.Point( 0 , 0 );
            this.InnerChart.Name="InnerChart";
            this.InnerChart.Padding.Bottom=1;
            this.InnerChart.Padding.Left=1;
            this.InnerChart.Padding.Right=1;
            this.InnerChart.Padding.Top=1;
            this.InnerChart.RuntimeSelection=true;
            this.InnerChart.RuntimeSeriesSelectionMode=DevExpress.XtraCharts.SeriesSelectionMode.Point;
            this.InnerChart.Size=new System.Drawing.Size( 360 , 283 );
            this.InnerChart.SmallChartText.Text="Mở rộng biểu đồ để thấy toàn bộ.";
            this.InnerChart.TabIndex=0;

            InitSeries();
            
            #endregion

            this.Controls.Add( this.InnerChart );
            this.Name="ABCChartControl";
            this.Size=new System.Drawing.Size( 360 , 283 );

            #region  Bar - EndInit
            this.chartBarController1.Control=this.InnerChart;
            // 
            // UserControl2
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown2 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown3 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown4 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown5 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown6 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown7 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.galleryDropDown8 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chartBarController1 ) ).EndInit();

            #endregion
       
            EndInitialize();

            if ( this.InnerChart.Diagram is XYDiagram )
            {
                ( (XYDiagram)this.InnerChart.Diagram ).EnableAxisXZooming=true;
                ( (XYDiagram)this.InnerChart.Diagram ).EnableAxisYZooming=true;
                ( (XYDiagram)this.InnerChart.Diagram ).EnableAxisXScrolling=true;
                ( (XYDiagram)this.InnerChart.Diagram ).EnableAxisYScrolling=true;
            }
            if ( ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )||OwnerView==null&&this.InnerChart.Diagram is Diagram3D )
            {
                ( this.InnerChart.Diagram as Diagram3D ).RuntimeRotation=true;
                ( this.InnerChart.Diagram as Diagram3D ).RuntimeScrolling=true;
                ( this.InnerChart.Diagram as Diagram3D ).RuntimeZooming=true;
            }
            this.InnerChart.ObjectHotTracked+=new HotTrackEventHandler( InnerChart_ObjectHotTracked );
            this.InnerChart.ObjectSelected+=new HotTrackEventHandler( InnerChart_ObjectSelected );
    
            this.InnerChart.CustomDrawAxisLabel+=new CustomDrawAxisLabelEventHandler( InnerChart_CustomDrawAxisLabel );
            this.InnerChart.CustomDrawSeriesPoint+=new CustomDrawSeriesPointEventHandler( InnerChart_CustomDrawSeriesPoint );
            this.InnerChart.MouseUp+=new MouseEventHandler( InnerChart_MouseUp );
        }

        void InnerChart_MouseUp ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right )
                DoShowPopupMenu( new Point( e.X , e.Y ) );
        }

        #region Bar
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraCharts.UI.ChartTypeBar menuBar;
        private DevExpress.XtraCharts.UI.CreateBarBaseItem createBarBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown1;
        private DevExpress.XtraCharts.UI.CreateLineBaseItem createLineBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown2;
        private DevExpress.XtraCharts.UI.CreatePieBaseItem createPieBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown3;
        private DevExpress.XtraCharts.UI.CreateRotatedBarBaseItem createRotatedBarBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown4;
        private DevExpress.XtraCharts.UI.CreateAreaBaseItem createAreaBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown5;
        private DevExpress.XtraCharts.UI.CreateOtherSeriesTypesBaseItem createOtherSeriesTypesBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown6;
        private DevExpress.XtraCharts.UI.ChangePaletteGalleryBaseItem changePaletteGalleryBaseItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown7;
        private DevExpress.XtraCharts.UI.ChangeAppearanceGalleryBaseBarManagerItem changeAppearanceGalleryBaseBarManagerItem1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown8;
        private DevExpress.XtraCharts.UI.PrintPreviewChartItem printPreviewChartItem1;
        private DevExpress.XtraCharts.UI.PrintChartItem printChartItem1;
        private DevExpress.XtraCharts.UI.CreateExportBaseItem createExportBaseItem1;
        private DevExpress.XtraCharts.UI.ExportToPDFChartItem exportToPDFChartItem1;
        private DevExpress.XtraCharts.UI.ExportToHTMLChartItem exportToHTMLChartItem1;
        private DevExpress.XtraCharts.UI.ExportToMHTChartItem exportToMHTChartItem1;
        private DevExpress.XtraCharts.UI.ExportToXLSChartItem exportToXLSChartItem1;
        private DevExpress.XtraCharts.UI.ExportToXLSXChartItem exportToXLSXChartItem1;
        private DevExpress.XtraCharts.UI.ExportToRTFChartItem exportToRTFChartItem1;
        private DevExpress.XtraCharts.UI.CreateExportToImageBaseItem createExportToImageBaseItem1;
        private DevExpress.XtraCharts.UI.ExportToBMPChartItem exportToBMPChartItem1;
        private DevExpress.XtraCharts.UI.ExportToGIFChartItem exportToGIFChartItem1;
        private DevExpress.XtraCharts.UI.ExportToJPEGChartItem exportToJPEGChartItem1;
        private DevExpress.XtraCharts.UI.ExportToPNGChartItem exportToPNGChartItem1;
        private DevExpress.XtraCharts.UI.ExportToTIFFChartItem exportToTIFFChartItem1;

        private DevExpress.XtraCharts.UI.ChartBarController chartBarController1;
        #endregion

        public virtual void BeginInitialize ( )
        {
            ( (System.ComponentModel.ISupportInitialize)( this.InnerChart ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( InnerDiagram ) ).BeginInit();
            this.SuspendLayout();

        }
        public virtual void EndInitialize ( )
        {
            ( (System.ComponentModel.ISupportInitialize)( InnerDiagram ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.InnerChart ) ).EndInit();
            this.ResumeLayout( false );
        }
        public virtual void InitSeries ( )
        {
        }
        
        #endregion


        void InnerChart_CustomDrawSeriesPoint ( object sender , CustomDrawSeriesPointEventArgs e )
        {
            if ( e.Series==this.Series1&&DataStructureProvider.IsForeignKey( this.Series1.TableName , this.Series1.ArgumentDataMember ) )
            {
                try
                {
                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( e.SeriesPoint.Argument );
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( this.Series1.TableName , this.Series1.ArgumentDataMember );
                    String strDisplayField=this.Series1.ArgumentDataMember;
                    if ( string.IsNullOrWhiteSpace( this.Series1.ArgumentDisplayCol )==false )
                        strDisplayField=strDisplayField+":"+this.Series1.ArgumentDisplayCol;
                    else
                        strDisplayField=strDisplayField+":"+DataStructureProvider.GetDisplayColumn( strPKTableName );

                    object objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( this.Series1.TableName , iID , strDisplayField );
                    e.LegendText=DataFormatProvider.DoFormat( objValue , this.Series1.TableName , strDisplayField );
                }
                catch ( Exception ex )
                { }
            }
        }
     
        void InnerChart_CustomDrawAxisLabel ( object sender , CustomDrawAxisLabelEventArgs e )
        {
            if ( e.Item.Axis is DevExpress.XtraCharts.AxisX )
            {
                if ( ( this.Series1.ArgumentDisplayCol!=null&&this.Series1.ArgumentDisplayCol.Split( ':' )[0]==this.Series1.ArgumentDataMember )&&
                     DataStructureProvider.IsForeignKey( this.Series1.TableName , this.Series1.ArgumentDataMember ) )
                {
                    try
                    {
                        Guid iID=ABCHelper.DataConverter.ConvertToGuid( e.Item.AxisValue );         
                        object objValue=DataCachingProvider.GetCachingObjectDisplayAccrossTable( this.Series1.TableName , iID , this.Series1.ArgumentDisplayCol );
                        e.Item.Text=DataFormatProvider.DoFormat( objValue , this.Series1.TableName , this.Series1.ArgumentDisplayCol );
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            }
        }

        #region IABCControl
      
        public ABCView OwnerView { get; set; }
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
        public void InitControl ( )
        {
        
        }

        void InnerChart_ObjectSelected ( object sender , HotTrackEventArgs e )
        {
            if ( e.Object is ChartControl||e.Object is Diagram||e.Object is AxisBase||e.Object is SeriesLabelBase||e.Object is Legend||e.Object is TitleBase)
                e.Cancel=true;
        }

        void InnerChart_ObjectHotTracked ( object sender , HotTrackEventArgs e )
        {
            if ( e.Object is ChartControl||e.Object is Diagram||e.Object is AxisBase||e.Object is SeriesLabelBase||e.Object is Legend||e.Object is TitleBase )
                e.Cancel=true;
        }

        #endregion

        #region Properties

        #region ABCChart
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series1
        {
            get
            {
                if ( this.InnerChart.Series.Count>0 )
                    return this.InnerChart.Series[0] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series2
        {
            get
            {
                if ( this.InnerChart.Series.Count>1 )
                    return this.InnerChart.Series[1] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series3
        {
            get
            {
                if ( this.InnerChart.Series.Count>2 )
                    return this.InnerChart.Series[2] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series4
        {
            get
            {
                if ( this.InnerChart.Series.Count>3 )
                    return this.InnerChart.Series[3] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series5
        {
            get
            {
                if ( this.InnerChart.Series.Count>4 )
                    return this.InnerChart.Series[4] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series6
        {
            get
            {
                if ( this.InnerChart.Series.Count>5 )
                    return this.InnerChart.Series[5] as ABCChartBaseSeries;
                else
                    return null;
            }
        }
        [Category( "ABCChart" )]
        public ABCChartBaseSeries Series7
        {
            get
            {
                if ( this.InnerChart.Series.Count>6 )
                    return this.InnerChart.Series[6] as ABCChartBaseSeries;
                else
                    return null;
            }
        }

        [Category( "ABCChart" )]
        public String EmptyChartText
        {
            get { return this.InnerChart.EmptyChartText.Text; }
            set { this.InnerChart.EmptyChartText.Text=value; }
        }

        String strCaption=String.Empty;
        [Category( "ABCChart" )]
        public String Caption
        {
            get
            {
                return strCaption;
                if ( this.InnerChart.Titles.Count>0 )
                    return this.InnerChart.Titles[0].Text;
                else
                    return String.Empty;
            }
            set
            {
                strCaption=value;
                if ( this.InnerChart.Titles.Count<=0 )
                {
                    ChartTitle title=new ChartTitle();
                    title.Font=new System.Drawing.Font( "Tahoma" , 14F );
                    title.Indent=0;
                    title.Text=strCaption;
                    title.TextColor=System.Drawing.Color.Gray;
                    this.InnerChart.Titles.AddRange( new DevExpress.XtraCharts.ChartTitle[] { title } );
                }
                else
                    this.InnerChart.Titles[0].Text=strCaption;

                if ( String.IsNullOrWhiteSpace( strCaption ) )
                    this.InnerChart.Titles[0].Visible=false;
                else
                    this.InnerChart.Titles[0].Visible=true;
            }
        }

        [Category( "ABCChart" )]
        public Boolean ShowMenuBar
        {
            get { return this.menuBar.Visible; }
            set { this.menuBar.Visible=value; }
        }
        #endregion

        #region Appearance
        [TypeConverter( typeof( ABCAppearanceTypeConverter ) )]
        [Category( "Appearance" )]
        public String AppearanceName
        {
            get { return this.InnerChart.AppearanceName; }
            set { this.InnerChart.AppearanceName=value; }
        }

        [TypeConverter( typeof( ABCPaletteTypeConverter ) )]
    //    [Editor( typeof( DevExpress.XtraCharts.Design.PaletteTypeEditor ) , typeof( UITypeEditor ) )]
        [Category( "Appearance" )]
        public String PaletteName
        {
            get { return this.InnerChart.PaletteName; }
            set { this.InnerChart.PaletteName=value; }
        }

        [Category( "Appearance" )]
        public int PaletteBaseColorNumber
        {
            get { return this.InnerChart.PaletteBaseColorNumber; }
            set { this.InnerChart.PaletteBaseColorNumber=value; }
        }
        
        #endregion

        #region Legend
        [Category( "Legend" )]
        public bool ShowLegend
        {
            get { return this.InnerChart.Legend.Visible; }
            set { this.InnerChart.Legend.Visible=value; }
        }
        
        #endregion

        #region Diagram

        [Category( "Diagram" )]
        public AxisBase AxisX
        {
            get { return this.InnerDiagram.AxisX as AxisBase; }
        }
        [Category( "Diagram" )]
        public AxisBase AxisY
        {
            get { return this.InnerDiagram.AxisY as AxisBase; }
        }
        #endregion

        #endregion

        #region Layout
        public void GetChildrenXMLLayout ( XmlElement chartElement )
        {
         

            if ( Series1!=null )
                SerializeSeries( Series1 as ABCChartBaseSeries , chartElement , "Series1" );
         
            if ( Series2!=null )
                SerializeSeries( Series2 as ABCChartBaseSeries  , chartElement , "Series2" );

            if ( Series3!=null )
                SerializeSeries( Series3 as ABCChartBaseSeries , chartElement , "Series3" );

            if ( Series4!=null )
                SerializeSeries( Series4 as ABCChartBaseSeries , chartElement , "Series4" );

            if ( Series5!=null )
                SerializeSeries( Series5 as ABCChartBaseSeries , chartElement , "Series5" );
         
            if ( Series6!=null )
                SerializeSeries( Series6 as ABCChartBaseSeries , chartElement , "Series6" );
         
            if ( Series7!=null )
                SerializeSeries( Series7 as ABCChartBaseSeries , chartElement , "Series7" );


            if ( AxisX!=null )
                SerializeAxis( AxisX as Axis , chartElement , "AxisX" );

            if ( AxisY!=null )
                SerializeAxis( AxisY as Axis , chartElement , "AxisY" );

        }
        private void SerializeAxis ( Axis axis , XmlElement chartElement, String strTagName )
        {
            XmlElement ele=ABCPresentHelper.Serialization( chartElement.OwnerDocument , axis , strTagName );
            chartElement.AppendChild( ele );

            ABCPresentHelper.Serialization( axis.AutoScaleBreaks , ele , "AutoScaleBreaks" );
            ABCPresentHelper.Serialization( axis.ConstantLines , ele , "ConstantLines" );
            ABCPresentHelper.Serialization( axis.CustomLabels , ele , "CustomLabels" );
            ABCPresentHelper.Serialization( axis.DateTimeOptions , ele , "DateTimeOptions" );
           
            XmlElement ele1=ABCPresentHelper.Serialization( axis.GridLines , ele , "GridLines" );
            ABCPresentHelper.Serialization( axis.GridLines.LineStyle , ele1 , "LineStyle" );
            ABCPresentHelper.Serialization( axis.GridLines.MinorLineStyle , ele1 , "MinorLineStyle" );
            ABCPresentHelper.Serialization( axis.InterlacedFillStyle , ele , "InterlacedFillStyle" );
            ABCPresentHelper.Serialization( axis.Label , ele , "Label" );
            ABCPresentHelper.Serialization( axis.NumericOptions , ele , "NumericOptions" );

            //ele1=ABCPresentUtils.Serialization( axis.Range , ele , "Range" );
            //ABCPresentUtils.Serialization( axis.Range.ScrollingRange , ele1 , "ScrollingRange" );

            ABCPresentHelper.Serialization( axis.ScaleBreakOptions , ele , "ScaleBreakOptions" );
            ABCPresentHelper.Serialization( axis.ScaleBreaks , ele , "ScaleBreaks" );
            ABCPresentHelper.Serialization( axis.Strips , ele , "Strips" );
            ABCPresentHelper.Serialization( axis.Tickmarks , ele , "Tickmarks" );
            ABCPresentHelper.Serialization( axis.Title , ele , "Title" );

            ele1=ABCPresentHelper.Serialization( axis.WorkdaysOptions , ele , "WorkdaysOptions" );
            ABCPresentHelper.Serialization( axis.WorkdaysOptions.ExactWorkdays , ele1 , "ExactWorkdays" );
            ABCPresentHelper.Serialization( axis.WorkdaysOptions.Holidays , ele1 , "Holidays" );

        }

        private void SerializeSeries ( ABCChartBaseSeries series , XmlElement chartElement , String strTagName )
        {
            XmlElement ele=ABCPresentHelper.Serialization( chartElement.OwnerDocument , series , strTagName );
            chartElement.AppendChild( ele );

            ABCPresentHelper.Serialization( series.DataFilters , ele , "DataFilters" );

            XmlElement ele1=ABCPresentHelper.Serialization( series.Label , ele , "Label" );
            ABCPresentHelper.Serialization( series.Label.LineStyle , ele1 , "LineStyle" );
            XmlElement ele2=ABCPresentHelper.Serialization( series.Label.PointOptions , ele1 , "PointOptions" );
            ABCPresentHelper.Serialization( series.Label.PointOptions.ArgumentNumericOptions , ele2 , "ArgumentNumericOptions" );
            ABCPresentHelper.Serialization( series.Label.PointOptions.ArgumentDateTimeOptions , ele2 , "ArgumentDateTimeOptions" );
            ABCPresentHelper.Serialization( series.Label.PointOptions.ValueNumericOptions , ele2 , "ValueNumericOptions" );
            ABCPresentHelper.Serialization( series.Label.PointOptions.ValueDateTimeOptions , ele2 , "ValueDateTimeOptions" );
            ABCPresentHelper.Serialization( series.Label.Shadow , ele1 , "Shadow" );
            ABCPresentHelper.Serialization( series.Label.Border , ele1 , "Border" );
            ABCPresentHelper.Serialization( series.Label.FillStyle , ele1 , "FillStyle" );
            ABCPresentHelper.Serialization( series.Label.Font , ele1 , "Font" );


            ele1=ABCPresentHelper.Serialization( series.LegendPointOptions , ele , "LegendPointOptions" );
            ABCPresentHelper.Serialization( series.LegendPointOptions.ArgumentNumericOptions , ele1 , "ArgumentNumericOptions" );
            ABCPresentHelper.Serialization( series.LegendPointOptions.ArgumentDateTimeOptions , ele1 , "ArgumentDateTimeOptions" );
            ABCPresentHelper.Serialization( series.LegendPointOptions.ValueNumericOptions , ele1 , "ValueNumericOptions" );
            ABCPresentHelper.Serialization( series.LegendPointOptions.ValueDateTimeOptions , ele1 , "ValueDateTimeOptions" );

            ABCPresentHelper.Serialization( series.TopNOptions , ele , "TopNOptions" );

            ele1=ABCPresentHelper.Serialization( series.View , ele , "View" );
            //ABCPresentUtils.Serialization( series.View.Shadow , ele1 , "Shadow" );
            //ABCPresentUtils.Serialization( series.View.Border , ele1 , "Border" );
            //ABCPresentUtils.Serialization( series.View.FillStyle , ele1 , "FillStyle" );
        }

        public void InitLayout ( ABCView view , XmlNode chartNode )
        {
            OwnerView=view;

            if ( Series1!=null )
                DeSerializeSeries( Series1 , chartNode , "Series1" );

            if ( Series2!=null )
                DeSerializeSeries( Series2 , chartNode , "Series2" );

            if ( Series3!=null )
                DeSerializeSeries( Series3 , chartNode , "Series3" );

            if ( Series4!=null )
                DeSerializeSeries( Series4 , chartNode , "Series4" );

            if ( Series5!=null )
                DeSerializeSeries( Series5 , chartNode , "Series5" );

            if ( Series6!=null )
                DeSerializeSeries( Series6 , chartNode , "Series6" );

            if ( Series7!=null )
                DeSerializeSeries( Series7 , chartNode , "Series7" );

            if ( AxisX!=null )
                DeSerializeAxis( AxisX as Axis , chartNode , "AxisX" );

            if ( AxisY!=null )
                DeSerializeAxis( AxisY as Axis , chartNode , "AxisY" );


            //    ABCPresentUtils.DeSerialization( this , chartNode );

            InnerDiagram.AxisY.GridSpacingAuto=true;
            InnerDiagram.AxisY.Interlaced=true;
            InnerDiagram.AxisY.GridLines.Visible=true;
            InnerDiagram.AxisY.GridLines.MinorVisible=true;

            InnerDiagram.AxisX.GridSpacingAuto=true;
            InnerDiagram.AxisX.Interlaced=true;
            InnerDiagram.AxisX.GridLines.Visible=true;
            InnerDiagram.AxisX.GridLines.MinorVisible=true;
            InnerDiagram.AxisX.Label.ResolveOverlappingOptions.MinIndent=5;

            this.InnerChart.CrosshairOptions.CrosshairLabelMode=CrosshairLabelMode.ShowCommonForAllSeries;
            this.InnerChart.CrosshairOptions.ShowCrosshairLabels=false;
            //this.InnerChart.ToolTipEnabled=true;
            //this.InnerChart.ToolTipOptions.ShowForPoints=true;

        }
        private void DeSerializeAxis ( Axis axis , XmlNode chartNode , String strTagName )
        {
            XmlNode node=chartNode.SelectSingleNode( strTagName );
            if ( node!=null )
            {
                ABCPresentHelper.DeSerialization( axis , node );

                ABCPresentHelper.DeSerialization( axis.AutoScaleBreaks , typeof( AutoScaleBreaks ) , node.SelectSingleNode( "AutoScaleBreaks" ) );

                XmlNode nodeParent=node.SelectSingleNode( "ConstantLines" );
                if ( nodeParent!=null )
                {
                    foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                        axis.ConstantLines.Add( ABCPresentHelper.DeSerialization( null , typeof( ConstantLine ) , nodeChild ) as ConstantLine );
                }

                nodeParent=node.SelectSingleNode( "CustomLabels" );
                if ( nodeParent!=null )
                {
                    foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                        axis.CustomLabels.Add( ABCPresentHelper.DeSerialization( null , typeof( CustomAxisLabel ) , nodeChild ) as CustomAxisLabel );
                }

                ABCPresentHelper.DeSerialization( axis.DateTimeOptions , typeof( AutoScaleBreaks ) , node.SelectSingleNode( "DateTimeOptions" ) );

                nodeParent=node.SelectSingleNode( "GridLines" );
                if ( nodeParent!=null )
                {
                    ABCPresentHelper.DeSerialization( axis.GridLines , typeof( GridLines ) , nodeParent );
                    ABCPresentHelper.DeSerialization( axis.GridLines.LineStyle , typeof( LineStyle ) , nodeParent.SelectSingleNode( "LineStyle" ) );
                    ABCPresentHelper.DeSerialization( axis.GridLines.MinorLineStyle , typeof( LineStyle ) , nodeParent.SelectSingleNode( "MinorLineStyle" ) );
                }

                ABCPresentHelper.DeSerialization( axis.InterlacedFillStyle , typeof( RectangleFillStyle ) , node.SelectSingleNode( "InterlacedFillStyle" ) );
                ABCPresentHelper.DeSerialization( axis.Label , typeof( AxisLabel ) , node.SelectSingleNode( "Label" ) );
                ABCPresentHelper.DeSerialization( axis.NumericOptions , typeof( NumericOptions ) , node.SelectSingleNode( "NumericOptions" ) );

                //ABCPresentUtils.DeSerialization( axis.Range , typeof( AxisRange ) , node.SelectSingleNode( "Range" ) );
                //ABCPresentUtils.DeSerialization( axis.Range.ScrollingRange , typeof( ScrollingRange ) , node.SelectSingleNode( "ScrollingRange" ) );

                ABCPresentHelper.DeSerialization( axis.ScaleBreakOptions , typeof( ScaleBreakOptions ) , node.SelectSingleNode( "ScaleBreakOptions" ) );

                nodeParent=node.SelectSingleNode( "ScaleBreaks" );
                if ( nodeParent!=null )
                {
                    foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                        axis.ScaleBreaks.Add( ABCPresentHelper.DeSerialization( null , typeof( ScaleBreak ) , nodeChild ) as ScaleBreak );
                }

                nodeParent=node.SelectSingleNode( "Strips" );
                if ( nodeParent!=null )
                {
                    foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                        axis.Strips.Add( ABCPresentHelper.DeSerialization( null , typeof( Strip ) , nodeChild ) as Strip );
                }

                ABCPresentHelper.DeSerialization( axis.Tickmarks , typeof( Tickmarks ) , node.SelectSingleNode( "Tickmarks" ) );
                ABCPresentHelper.DeSerialization( axis.Title , typeof( Title ) , node.SelectSingleNode( "Title" ) );

                ABCPresentHelper.DeSerialization( axis.WorkdaysOptions , typeof( WorkdaysOptions ) , node.SelectSingleNode( "WorkdaysOptions" ) );

                nodeParent=node.SelectSingleNode( "WorkdaysOptions" );
                if ( nodeParent!=null )
                {
                    nodeParent=nodeParent.SelectSingleNode( "ExactWorkdays" );
                    if ( nodeParent!=null )
                    {
                        foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                            axis.WorkdaysOptions.ExactWorkdays.Add( ABCPresentHelper.DeSerialization( null , typeof( KnownDate ) , nodeChild ) as KnownDate );
                   
                        nodeParent=nodeParent.SelectSingleNode( "Holidays" );
                        if ( nodeParent!=null )
                        {
                            foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                                axis.WorkdaysOptions.Holidays.Add( ABCPresentHelper.DeSerialization( null , typeof( KnownDate ) , nodeChild ) as KnownDate );
                        }
                    }
                }
            }
        }
        private void DeSerializeSeries ( ABCChartBaseSeries series , XmlNode chartNode , String strTagName )
        {
            XmlNode node=chartNode.SelectSingleNode( strTagName );
            if ( node!=null )
            {
                ABCPresentHelper.DeSerialization( series , node );


                XmlNode nodeParent=node.SelectSingleNode( "DataFilters" );
                if ( nodeParent!=null )
                {
                    foreach ( XmlNode nodeChild in nodeParent.SelectNodes( "Item" ) )
                        series.DataFilters.Add( ABCPresentHelper.DeSerialization( null , typeof( DataFilter ) , nodeChild ) as DataFilter );
                }

                XmlNode nodeLabel=node.SelectSingleNode( "Label" );
                if ( nodeLabel!=null )
                {
                    ABCPresentHelper.DeSerialization( series.Label , typeof( AxisLabel ) , nodeLabel );
                    ABCPresentHelper.DeSerialization( series.Label.LineStyle , typeof( LineStyle ) , nodeLabel.SelectSingleNode( "LineStyle" ) );
                    ABCPresentHelper.DeSerialization( series.Label.Shadow , typeof( Shadow ) , nodeLabel.SelectSingleNode( "Shadow" ) );
                    ABCPresentHelper.DeSerialization( series.Label.Border , typeof( RectangularBorder ) , nodeLabel.SelectSingleNode( "Border" ) );
                    ABCPresentHelper.DeSerialization( series.Label.FillStyle , typeof( RectangleFillStyle ) , nodeLabel.SelectSingleNode( "FillStyle" ) );
                    ABCPresentHelper.DeSerialization( series.Label.Font , typeof( Font ) , nodeLabel.SelectSingleNode( "Font" ) );

                    XmlNode nodePointOptions=nodeLabel.SelectSingleNode( "PointOptions" );
                    if ( nodePointOptions!=null )
                    {
                        ABCPresentHelper.DeSerialization( series.Label.PointOptions , typeof( PointOptions ) , nodePointOptions );

                        ABCPresentHelper.DeSerialization( series.Label.PointOptions.ArgumentNumericOptions , typeof( NumericOptions ) , nodePointOptions.SelectSingleNode( "ArgumentNumericOptions" ) );
                        ABCPresentHelper.DeSerialization( series.Label.PointOptions.ArgumentDateTimeOptions , typeof( DateTimeOptions ) , nodePointOptions.SelectSingleNode( "ArgumentDateTimeOptions" ) );
                        ABCPresentHelper.DeSerialization( series.Label.PointOptions.ValueNumericOptions , typeof( NumericOptions ) , nodePointOptions.SelectSingleNode( "ValueNumericOptions" ) );
                        ABCPresentHelper.DeSerialization( series.Label.PointOptions.ValueDateTimeOptions , typeof( DateTimeOptions ) , nodePointOptions.SelectSingleNode( "ValueDateTimeOptions" ) );
                    }
                }

                XmlNode nodeLegend=node.SelectSingleNode( "LegendPointOptions" );
                if ( nodeLegend!=null )
                {
                    ABCPresentHelper.DeSerialization( series.LegendPointOptions , typeof( PointOptions ) , nodeLegend );
                    ABCPresentHelper.DeSerialization( series.LegendPointOptions.ArgumentNumericOptions , typeof( NumericOptions ) , nodeLegend.SelectSingleNode( "ArgumentNumericOptions" ) );
                    ABCPresentHelper.DeSerialization( series.LegendPointOptions.ArgumentDateTimeOptions , typeof( DateTimeOptions ) , nodeLegend.SelectSingleNode( "ArgumentDateTimeOptions" ) );
                    ABCPresentHelper.DeSerialization( series.LegendPointOptions.ValueNumericOptions , typeof( NumericOptions ) , nodeLegend.SelectSingleNode( "ValueNumericOptions" ) );
                    ABCPresentHelper.DeSerialization( series.LegendPointOptions.ValueDateTimeOptions , typeof( DateTimeOptions ) , nodeLegend.SelectSingleNode( "ValueDateTimeOptions" ) );

                }
                ABCPresentHelper.DeSerialization( series.TopNOptions , typeof( TopNOptions ) , node.SelectSingleNode( "TopNOptions" ) );
                ABCPresentHelper.DeSerialization( series.View , typeof( SeriesViewBase ) , node.SelectSingleNode( "View" ) );

                if ( series.View is SideBySideBarSeriesView )
                {
                    ( series.View as SideBySideBarSeriesView ).Shadow.Visible=true;
                    ( series.View as SideBySideBarSeriesView ).Shadow.Size=2;
                    ( (BarSeriesLabel)series.Label ).ShowForZeroValues=false;

                }
                if ( series.View is LineSeriesView )
                {
                    ( series.View as LineSeriesView ).Shadow.Visible=false;
                    ( series.View as LineSeriesView ).LineMarkerOptions.Visible=false;

                }
                series.CrosshairLabelPattern=series.Label.PointOptions.Pattern;
                if ( DataStructureProvider.IsForeignKey( series.TableName , series.ArgumentDataMember ) )
                {
                    if ( series.ArgumentDisplayCol==null )
                        series.ArgumentDisplayCol=series.ArgumentDataMember;

                    series.ArgumentScaleType=ScaleType.Qualitative;
                }
            }
        }
        
        #endregion

        #region Menu

        #region Show menu
        public void DoShowPopupMenu ( Point hi )
        {
            InitPopupMenu();

            ABCChartMenuInfo info=new ABCChartMenuInfo( hi , null );
            ABCPopupMenu.Tag=info;

            object[] hitObjects=this.InnerChart.HitTest( hi.X , hi.Y );
            foreach ( object hitObject in hitObjects )
            {
                if ( hitObject is Axis||hitObject is Legend||hitObject is ChartControl )
                {
                    MenuManagerHelper.ShowMenu( ABCPopupMenu , null , null , this , hi );
                    break;
                }
            }
        }
        #endregion

        #region Init Menu
        public class ABCChartMenuInfo
        {
            public ABCChartMenuInfo ( Point hit , object objData )
            {
                this.Point=hit;
                this.objData=objData;
            }
            public Point Point;
            public object objData;
        }
        public class ABCChartButtonMenu : DXPopupMenu
        {
            public object Tag;
            public ABCChartButtonMenu ( ) { }

            public DXMenuItem AddItem ( DXSubMenuItem parent , String strCaption , object tag , Boolean isCheckItem )
            {
                DXMenuItem item;
                if ( isCheckItem )
                {
                    item=new DXMenuCheckItem();
                    ( (DXMenuCheckItem)item ).Checked=true;
                }
                else
                    item=new DXMenuItem();
                item.Caption=strCaption;
                item.Tag=tag;
                parent.Items.Add( item );
                return item;
            }
            public DXMenuItem AddItem ( String strCaption , object tag , Boolean isCheckItem )
            {
                DXMenuItem item;
                if ( isCheckItem )
                {
                    item=new DXMenuCheckItem();
                    ( (DXMenuCheckItem)item ).Checked=true;
                }
                else
                    item=new DXMenuItem();

                item.Caption=strCaption;
                item.Tag=tag;
                this.Items.Add( item );

                return item;
            }
            public DXSubMenuItem AddSubItem ( String strCaption , object tag )
            {
                DXSubMenuItem item=new DXSubMenuItem();
                item.Caption=strCaption;
                item.Tag=tag;
                this.Items.Add( item );
                return item;
            }
        }
        public ABCChartButtonMenu ABCPopupMenu=null;
        public virtual void InitPopupMenu ( )
        {
            if ( ABCPopupMenu==null )
                ABCPopupMenu=new ABCChartButtonMenu();
            ABCPopupMenu.Items.Clear();

            DXMenuCheckItem showBarMenu=(DXMenuCheckItem)ABCPopupMenu.AddItem( "Thanh công cụ" , "BAR" , true );
            showBarMenu.Click+=new EventHandler( PopupMenu_Click );
            showBarMenu.Checked=this.ShowMenuBar;

            #region DateTime 
            if ( this.Series1.ActualArgumentScaleType==ScaleType.DateTime )
            {
                DXSubMenuItem timeType=ABCPopupMenu.AddSubItem( "Định dạng thời gian" , "TIMETYPE" );
                ABCPopupMenu.AddItem( timeType , "Năm" , DateTimeMeasurementUnit.Year , true );
                ABCPopupMenu.AddItem( timeType , "Quý" , DateTimeMeasurementUnit.Quarter , true );
                ABCPopupMenu.AddItem( timeType , "Tháng" , DateTimeMeasurementUnit.Month , true );
                ABCPopupMenu.AddItem( timeType , "Tuần" , DateTimeMeasurementUnit.Week , true );
                ABCPopupMenu.AddItem( timeType , "Ngày" , DateTimeMeasurementUnit.Day , true );
                ABCPopupMenu.AddItem( timeType , "Giờ" , DateTimeMeasurementUnit.Hour , true );
       
                foreach ( DXMenuItem item in timeType.Items )
                {
                    item.Click+=new EventHandler( PopupMenu_Click );
                    if ( item.Tag is DateTimeMeasurementUnit )
                        ( (DXMenuCheckItem)item ).Checked=( (DateTimeMeasurementUnit)item.Tag==this.AxisX.DateTimeMeasureUnit );
                }
            }
            
            #endregion

            #region ViewType
            DXSubMenuItem changeType=ABCPopupMenu.AddSubItem( "Loại biểu đồ" , "CHANGETYPE" );
            ABCPopupMenu.AddItem( changeType , "Lines" , ViewType.Line , true );
            ABCPopupMenu.AddItem( changeType , "Bar" , ViewType.Bar , true );
            ABCPopupMenu.AddItem( changeType , "Pie" , ViewType.Pie , true );
            //       ABCPopupMenu.AddItem( changeType,"Pie3D" , ViewType.Pie3D ,true );
            ABCPopupMenu.AddItem( changeType , "StackedBar" , ViewType.StackedBar , true );
            ABCPopupMenu.AddItem( changeType , "Step Lines" , ViewType.StepLine , true );
            ABCPopupMenu.AddItem( changeType , "Areas" , ViewType.Area , true );
            ABCPopupMenu.AddItem( changeType , "Stacked Areas" , ViewType.StackedArea , true );
            //      ABCPopupMenu.AddItem( changeType,"3D Lines" , ViewType.Line3D,true  );
            //       ABCPopupMenu.AddItem( changeType,"3D Step Lines" , ViewType.StepLine3D ,true );
            //        ABCPopupMenu.AddItem( changeType,"3D Areas" , ViewType.Area3D ,true );
            //          ABCPopupMenu.AddItem( changeType,"3D Stacked Areas" , ViewType.StackedArea3D ,true );
            ABCPopupMenu.AddItem( changeType , "Spline" , ViewType.Spline , true );
            ABCPopupMenu.AddItem( changeType , "Spline Area" , ViewType.SplineArea , true );
            ABCPopupMenu.AddItem( changeType , "Stacked Spline" , ViewType.StackedSplineArea , true );
            ABCPopupMenu.AddItem( changeType , "Full-Stacked Spline" , ViewType.FullStackedSplineArea , true );
            //      ABCPopupMenu.AddItem( changeType,"Spline 3D" , ViewType.Spline3D,true  );
            //       ABCPopupMenu.AddItem( changeType,"Spline Area 3D" , ViewType.SplineArea3D,true  );
            //        ABCPopupMenu.AddItem( changeType,"Full-Stacked Spline 3D" , ViewType.FullStackedSplineArea3D,true  );

            foreach ( DXMenuItem item in changeType.Items )
            {
                item.Click+=new EventHandler( PopupMenu_Click );

                if ( item.Tag is ViewType )
                {
                    ( (DXMenuCheckItem)item ).Checked=false;
                    if ( (ViewType)item.Tag==this.Series1.SeriesViewType )
                        ( (DXMenuCheckItem)item ).Checked=true;
                }
            } 
            #endregion

            #region LABEL

            #region Position
            foreach ( Series series in this.InnerChart.Series )
                if ( series.Label!=null&&series.Label is PieSeriesLabel )
                {
                    DXSubMenuItem labelViewType=ABCPopupMenu.AddSubItem( "Label Type" , "LABELTYPE" );
                    DXMenuCheckItem LABELTYPE_INSIDE=(DXMenuCheckItem)ABCPopupMenu.AddItem( labelViewType , "Inside" , "LABELTYPE_INSIDE" , true );
                    LABELTYPE_INSIDE.Click+=new EventHandler( PopupMenu_Click );
                    LABELTYPE_INSIDE.Checked=( ( (PieSeriesLabel)series.Label ).Position==PieSeriesLabelPosition.Inside );

                    DXMenuCheckItem LABELTYPE_OUTSIDE=(DXMenuCheckItem)ABCPopupMenu.AddItem( labelViewType , "Outside" , "LABELTYPE_OUTSIDE" , true );
                    LABELTYPE_OUTSIDE.Click+=new EventHandler( PopupMenu_Click );
                    LABELTYPE_OUTSIDE.Checked=( ( (PieSeriesLabel)series.Label ).Position==PieSeriesLabelPosition.Outside );

                    DXMenuCheckItem LABELTYPE_RADIAL=(DXMenuCheckItem)ABCPopupMenu.AddItem( labelViewType , "Radial" , "LABELTYPE_RADIAL" , true );
                    LABELTYPE_RADIAL.Click+=new EventHandler( PopupMenu_Click );
                    LABELTYPE_RADIAL.Checked=( ( (PieSeriesLabel)series.Label ).Position==PieSeriesLabelPosition.Radial );

                    DXMenuCheckItem LABELTYPE_TWOCOLUMNS=(DXMenuCheckItem)ABCPopupMenu.AddItem( labelViewType , "Two Columns" , "LABELTYPE_TWOCOLUMNS" , true );
                    LABELTYPE_TWOCOLUMNS.Click+=new EventHandler( PopupMenu_Click );
                    LABELTYPE_TWOCOLUMNS.Checked=( ( (PieSeriesLabel)series.Label ).Position==PieSeriesLabelPosition.TwoColumns );

                    DXMenuCheckItem LABELTYPE_TANGENT=(DXMenuCheckItem)ABCPopupMenu.AddItem( labelViewType , "Tangent" , "LABELTYPE_TANGENT" , true );
                    LABELTYPE_TANGENT.Click+=new EventHandler( PopupMenu_Click );
                    LABELTYPE_TANGENT.Checked=( ( (PieSeriesLabel)series.Label ).Position==PieSeriesLabelPosition.Tangent );
                    break;
                }
            #endregion

            #region Visible
            foreach ( Series series in this.InnerChart.Series )
                if ( series.Label!=null )
                {
                    DXMenuCheckItem labelMenu=(DXMenuCheckItem)ABCPopupMenu.AddItem( "Show Label" , "LABEL" , true );
                    labelMenu.Click+=new EventHandler( PopupMenu_Click );
                    labelMenu.Checked=series.Label.Visible;
                    break;
                }
            #endregion

            #endregion

            #region LEGEND
            DXMenuCheckItem legendMenu=(DXMenuCheckItem)ABCPopupMenu.AddItem( "Show Legend" , "LEGEND" , true );
            legendMenu.Click+=new EventHandler( PopupMenu_Click );
            legendMenu.Checked=this.InnerChart.Legend.Visible;
            #endregion

            #region MARKER

            foreach ( Series series in this.InnerChart.Series )
            {
                if ( series.View is DevExpress.XtraCharts.LineSeriesView )
                {
                    DevExpress.XtraCharts.LineSeriesView view=series.View as DevExpress.XtraCharts.LineSeriesView;
                    if ( view!=null&&view.LineMarkerOptions!=null )
                    {
                        DXMenuCheckItem markerMenu=(DXMenuCheckItem)ABCPopupMenu.AddItem( "Show Marker" , "MARKER" , true );
                        markerMenu.Click+=new EventHandler( PopupMenu_Click );
                        markerMenu.Checked=view.LineMarkerOptions.Visible;
                        break;
                    }
                }
                else if ( series.View is DevExpress.XtraCharts.Native.IAreaSeriesView )
                {
                    DevExpress.XtraCharts.Native.IAreaSeriesView view=series.View as DevExpress.XtraCharts.Native.IAreaSeriesView;
                    if ( view!=null&&view.MarkerOptions!=null )
                    {
                        DXMenuCheckItem markerMenu=(DXMenuCheckItem)ABCPopupMenu.AddItem( "Show Marker" , "MARKER" , true );
                        markerMenu.Click+=new EventHandler( PopupMenu_Click );
                        markerMenu.Checked=view.MarkerOptions.Visible;
                        break;
                    }
                }

            }
            #endregion
        }
        #endregion

        public virtual void PopupMenu_Click ( object sender , EventArgs e )
        {
            try
            {
                DXMenuItem item=sender as DXMenuItem;
                if ( item.Tag is ViewType )
                {
                    ChangeViewType( (ViewType)item.Tag );
                }
                else if ( item.Tag is DateTimeMeasurementUnit )
                {
                    ChangeDateTimeType( (DateTimeMeasurementUnit)item.Tag );
                }
                else if ( item.Tag.ToString().Equals( "BAR" ) )
                {
                    this.ShowMenuBar=( (DXMenuCheckItem)item ).Checked;
                }
                else if ( item.Tag.ToString().Equals( "LEGEND" ) )
                {
                    this.InnerChart.Legend.Visible=( (DXMenuCheckItem)item ).Checked;
                }
                else if ( item.Tag.ToString().Equals( "LABEL" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null )
                            series.Label.Visible=( (DXMenuCheckItem)item ).Checked;
                }
                else if ( item.Tag.ToString().Equals( "MARKER" ) )
                {
                    #region MARKER
                    foreach ( Series series in this.InnerChart.Series )
                    {
                        if ( series.View is DevExpress.XtraCharts.LineSeriesView )
                        {
                            DevExpress.XtraCharts.LineSeriesView view=series.View as DevExpress.XtraCharts.LineSeriesView;
                            if ( view!=null&&view.LineMarkerOptions!=null )
                                view.LineMarkerOptions.Visible=( (DXMenuCheckItem)item ).Checked;
                        }
                        else if ( series.View is DevExpress.XtraCharts.Native.IAreaSeriesView )
                        {
                            DevExpress.XtraCharts.Native.IAreaSeriesView view=series.View as DevExpress.XtraCharts.Native.IAreaSeriesView;
                            if ( view!=null&&view.MarkerOptions!=null )
                                view.MarkerOptions.Visible=( (DXMenuCheckItem)item ).Checked;
                        }

                    }
                    #endregion
                }

                #region LABEL - POSITION
                else if ( item.Tag.ToString().Equals( "LABELTYPE_INSIDE" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null&&series.Label is PieSeriesLabel )
                        {
                            ( (PieSeriesLabel)series.Label ).Position=PieSeriesLabelPosition.Inside;
                            break;
                        }
                }
                else if ( item.Tag.ToString().Equals( "LABELTYPE_OUTSIDE" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null&&series.Label is PieSeriesLabel )
                        {
                            ( (PieSeriesLabel)series.Label ).Position=PieSeriesLabelPosition.Outside;
                            break;
                        }
                }
                else if ( item.Tag.ToString().Equals( "LABELTYPE_RADIAL" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null&&series.Label is PieSeriesLabel )
                        {
                            ( (PieSeriesLabel)series.Label ).Position=PieSeriesLabelPosition.Radial;
                            break;
                        }
                }
                else if ( item.Tag.ToString().Equals( "LABELTYPE_TWOCOLUMNS" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null&&series.Label is PieSeriesLabel )
                        {
                            ( (PieSeriesLabel)series.Label ).Position=PieSeriesLabelPosition.TwoColumns;
                            break;
                        }
                }
                else if ( item.Tag.ToString().Equals( "LABELTYPE_TANGENT" ) )
                {
                    foreach ( Series series in this.InnerChart.Series )
                        if ( series.Label!=null&&series.Label is PieSeriesLabel )
                        {
                            ( (PieSeriesLabel)series.Label ).Position=PieSeriesLabelPosition.Tangent;
                            break;
                        }
                }
                #endregion
            }
            catch ( Exception ex )
            {
            }
        }
      
        #endregion
     
        public virtual void ChangeViewType ( ViewType viewType )
        {
            foreach ( Series series in this.InnerChart.Series )
            {
                series.ChangeView( viewType );
                byte transparency=0;
                if ( series.View is Area3DSeriesView||series.View is AreaSeriesView||series.View is SplineAreaSeriesView )
                    transparency=135;
                ITransparableView transparableView=series.View as ITransparableView;
                if ( transparableView!=null )
                    transparableView.Transparency=transparency;
            //    UpdateSeriesFormat( series );
            }

            Diagram3D diagram=this.InnerChart.Diagram as Diagram3D;
            if ( diagram!=null )
            {
                diagram.ZoomPercent=120;
                diagram.RuntimeRotation=true;
                diagram.RuntimeScrolling=true;
                diagram.RuntimeZooming=true;
                XYDiagram3D xyDiagram3D=diagram as XYDiagram3D;
            //    if ( xyDiagram3D!=null )
            //        UpdateAxesFormat( xyDiagram3D.AxisX , xyDiagram3D.AxisY , this.InnerChart.Series[0].View );
            }
            else
            {
                XYDiagram xyDiagram=this.InnerChart.Diagram as XYDiagram;
                //if ( xyDiagram!=null )
                //    UpdateAxesFormat( xyDiagram.AxisX , xyDiagram.AxisY , this.InnerChart.Series[0].View );
            }


        }
        public virtual void ChangeDateTimeType ( DateTimeMeasurementUnit type )
        {
            this.AxisX.DateTimeMeasureUnit=type;
            this.AxisX.DateTimeGridAlignment=type;

            switch ( type )
            {
                case DateTimeMeasurementUnit.Year:
                    this.AxisX.DateTimeOptions.Format=DateTimeFormat.Custom;
                    this.AxisX.DateTimeOptions.FormatString="yyyy";
                    break;
                case DateTimeMeasurementUnit.Quarter:
                    this.AxisX.DateTimeOptions.Format=DateTimeFormat.QuarterAndYear;
                    break;
                case DateTimeMeasurementUnit.Month:
                    this.AxisX.DateTimeOptions.Format=DateTimeFormat.Custom;   
                    this.AxisX.DateTimeOptions.FormatString="MM/yyyy";
                    break;
                case DateTimeMeasurementUnit.Week:
                case DateTimeMeasurementUnit.Day:
                    this.AxisX.DateTimeOptions.Format=DateTimeFormat.ShortDate;
                    break;
                case DateTimeMeasurementUnit.Hour:
                    this.AxisX.DateTimeOptions.Format=DateTimeFormat.ShortTime;  
                    break;
            }
        }
      
        public void InitBinding ( String strObjectName , BindingSource binding )
        {
            foreach ( Series series in this.InnerChart.Series )
            {
                if ( ( series as ABCChartBaseSeries ).DataSourceName==strObjectName )
                    ( series as ABCChartBaseSeries ).InitBinding( strObjectName , binding );
              
            }
        }
    }

    public class ABCChartBaseControlDesigner : ControlDesigner
    {
        public ABCChartBaseControlDesigner ( )
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
}
