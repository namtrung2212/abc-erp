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


namespace ABCControls
{

    [ToolboxBitmapAttribute( typeof( DevExpress.XtraCharts.ChartControl ) )]
    [Designer( typeof( ABCChartTwoSeriesControlDesigner ) )]
    public class ABCChartTwoSeriesControl : ABCChartBaseControl
    {
        public ABCChartBaseSeries MainSeries1;
        public ABCChartBaseSeries MainSeries2;

        public ABCChartTwoSeriesControl ( )
        {
        
        }

        public override void BeginInitialize ( )
        {
            if ( MainSeries1==null )
            {
                MainSeries1=new ABCChartBaseSeries( this );
                MainSeries1.BeginInitialize();
            }
            if ( MainSeries2==null )
            {
                MainSeries2=new ABCChartBaseSeries( this );
                MainSeries2.BeginInitialize();
            }
            
            base.BeginInitialize();
            
        }
        public override void EndInitialize ( )
        {
            MainSeries1.EndInitialize();
            MainSeries2.EndInitialize();
     
            base.EndInitialize();
        }

        public override void InitSeries ( )
        {
            MainSeries1.InitSeries();
            MainSeries2.InitSeries();

            //DevExpress.XtraCharts.SeriesPoint seriesPoint1=new DevExpress.XtraCharts.SeriesPoint( "Tham số 1" , new object[] { ( (object)( 49D ) ) } , 0 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint2=new DevExpress.XtraCharts.SeriesPoint( "Tham số 2" , new object[] { ( (object)( 34D ) ) } , 1 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint3=new DevExpress.XtraCharts.SeriesPoint( "Tham số 3" , new object[] { ( (object)( 27D ) ) } , 2 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint4=new DevExpress.XtraCharts.SeriesPoint( "Tham số 1" , new object[] { ( (object)( 20D ) ) } , 0 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint5=new DevExpress.XtraCharts.SeriesPoint( "Tham số 2" , new object[] { ( (object)( 75D ) ) } , 1 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint6=new DevExpress.XtraCharts.SeriesPoint( "Tham số 3" , new object[] { ( (object)( 14D ) ) } , 2 );

            //MainSeries1.Points.AddRange( new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint1 , seriesPoint2 , seriesPoint3 } );
            //MainSeries2.Points.AddRange( new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint4 , seriesPoint5 , seriesPoint6 } );

            MainSeries1.ColorEach=false;
            MainSeries2.ColorEach=false;


        }

      
    }

    public class ABCChartTwoSeriesControlDesigner : ControlDesigner
    {
        public ABCChartTwoSeriesControlDesigner ( )
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
