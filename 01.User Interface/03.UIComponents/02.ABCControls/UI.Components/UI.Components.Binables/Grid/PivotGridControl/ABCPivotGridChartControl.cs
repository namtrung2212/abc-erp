using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ABCControls
{
    public partial class ABCPivotGridChartControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ABCPivotGridChartControl ( )
        {
            InitializeComponent();
            InitChart();
       
        }

      
        public  DevExpress.XtraCharts.ChartControl Chart;

        public void SetPivotGridControl ( ABCPivotGridControl pivotGrid )
        {
            this.Chart.DataSource=pivotGrid.Grid;

            this.Chart.PivotGridDataSourceOptions.RetrieveEmptyCells=false;
            //this.Chart.PivotGridDataSourceOptions.RetrieveColumnGrandTotals=false;
            //this.Chart.PivotGridDataSourceOptions.RetrieveRowGrandTotals=false;
        }
        public void InitChart ( )
        {
            Chart=new DevExpress.XtraCharts.ChartControl();
       

            DevExpress.XtraCharts.XYDiagram xyDiagram1=new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1=new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1=new DevExpress.XtraCharts.LineSeriesView();

            this.SuspendLayout();

            ( (System.ComponentModel.ISupportInitialize)( this.Chart ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( xyDiagram1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( pointSeriesLabel1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( lineSeriesView1 ) ).BeginInit();

            xyDiagram1.AxisX.Label.Staggered=true;
            xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled=true;
            xyDiagram1.AxisX.Range.SideMarginsEnabled=true;
            xyDiagram1.AxisX.VisibleInPanesSerializable="-1";
            xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled=true;
            xyDiagram1.AxisY.Range.SideMarginsEnabled=true;
            xyDiagram1.AxisY.VisibleInPanesSerializable="-1";

            this.Chart.Diagram=xyDiagram1;
            this.Chart.Dock=System.Windows.Forms.DockStyle.Fill;
            this.Chart.Legend.MaxHorizontalPercentage=30;
            this.Chart.Location=new System.Drawing.Point( 0 , 0 );
            this.Chart.Name="chartControl";
            this.Chart.RuntimeHitTesting=false;
            this.Chart.SeriesDataMember="Series";
            this.Chart.SeriesSerializable=new DevExpress.XtraCharts.Series[0];
            this.Chart.SeriesTemplate.ArgumentDataMember="Arguments";
            pointSeriesLabel1.LineVisible=true;
            pointSeriesLabel1.Visible=false;
            this.Chart.SeriesTemplate.Label=pointSeriesLabel1;
            this.Chart.SeriesTemplate.ValueDataMembersSerializable="Values";
            this.Chart.SeriesTemplate.View=lineSeriesView1;
            this.Chart.Size=new System.Drawing.Size( 690 , 176 );
            this.Chart.TabIndex=3;
            this.chartBarController1.ChartControl=this.Chart;
            this.Controls.Add( Chart );

            ( (System.ComponentModel.ISupportInitialize)( xyDiagram1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( pointSeriesLabel1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( lineSeriesView1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.Chart ) ).EndInit();

            this.ResumeLayout( false );
        }

        public void ChangeChartType ( DevExpress.XtraCharts.ViewType type )
        {
            try
            {
                Chart.SeriesTemplate.ChangeView( type );

                if ( ( Chart.SeriesTemplate.View as DevExpress.XtraCharts.SimpleDiagramSeriesViewBase )==null )
                    Chart.Legend.Visible=true;

                if ( Chart.Diagram is DevExpress.XtraCharts.Diagram3D )
                {
                    DevExpress.XtraCharts.Diagram3D diagram=(DevExpress.XtraCharts.Diagram3D)Chart.Diagram;
                    diagram.RuntimeRotation=true;
                    diagram.RuntimeZooming=true;
                    diagram.RuntimeScrolling=true;
                }
                foreach ( DevExpress.XtraCharts.Series series in Chart.Series )
                {
                    DevExpress.XtraCharts.ISupportTransparency supportTransparency=series.View as DevExpress.XtraCharts.ISupportTransparency;
                    if ( supportTransparency!=null )
                    {
                        if ( ( series.View is DevExpress.XtraCharts.AreaSeriesView )||( series.View is DevExpress.XtraCharts.Area3DSeriesView )
                            ||( series.View is DevExpress.XtraCharts.RadarAreaSeriesView )||( series.View is DevExpress.XtraCharts.Bar3DSeriesView ) )
                            supportTransparency.Transparency=135;
                        else
                            supportTransparency.Transparency=0;
                    }
                }
            }
            catch ( Exception ex )
            {
            }
        }
     
    }
}
