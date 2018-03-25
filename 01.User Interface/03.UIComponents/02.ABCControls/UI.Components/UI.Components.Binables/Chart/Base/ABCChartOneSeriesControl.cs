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
    [Designer( typeof( ABCChartOneSeriesControlDesigner ) )]
    public class ABCChartOneSeriesControl : ABCChartBaseControl
    {
        public ABCChartBaseSeries MainSeries;

        public ABCChartOneSeriesControl ( )
        {
        
        }

        public override void BeginInitialize ( )
        {
            if ( MainSeries==null )
            {
                MainSeries=new ABCChartBaseSeries( this );
                MainSeries.BeginInitialize();
            }
            
            base.BeginInitialize();
            
        }
        public override void EndInitialize ( )
        {
            MainSeries.EndInitialize();

            base.EndInitialize();
        }

        public override void InitSeries ( )
        {
            MainSeries.InitSeries();

            //DevExpress.XtraCharts.SeriesPoint seriesPoint1=new DevExpress.XtraCharts.SeriesPoint( "Tham số 1" , new object[] { ( (object)( 49D ) ) } , 0 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint2=new DevExpress.XtraCharts.SeriesPoint( "Tham số 2" , new object[] { ( (object)( 34D ) ) } , 1 );
            //DevExpress.XtraCharts.SeriesPoint seriesPoint3=new DevExpress.XtraCharts.SeriesPoint( "Tham số 3" , new object[] { ( (object)( 27D ) ) } , 2 );
            //MainSeries.Points.AddRange( new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint1 , seriesPoint2 , seriesPoint3 } );
        }

    }

    public class ABCChartOneSeriesControlDesigner : ControlDesigner
    {
        public ABCChartOneSeriesControlDesigner ( )
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
