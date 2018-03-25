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
    [Designer( typeof( ABCChartSixSeriesControlDesigner ) )]
    public class ABCChartSixSeriesControl : ABCChartBaseControl
    {
        public ABCChartBaseSeries MainSeries1;
        public ABCChartBaseSeries MainSeries2;
        public ABCChartBaseSeries MainSeries3;
        public ABCChartBaseSeries MainSeries4;
        public ABCChartBaseSeries MainSeries5;
        public ABCChartBaseSeries MainSeries6;

        public ABCChartSixSeriesControl ( )
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
            if ( MainSeries3==null )
            {
                MainSeries3=new ABCChartBaseSeries( this );
                MainSeries3.BeginInitialize();
            }
            if ( MainSeries4==null )
            {
                MainSeries4=new ABCChartBaseSeries( this );
                MainSeries4.BeginInitialize();
            }
            if ( MainSeries5==null )
            {
                MainSeries5=new ABCChartBaseSeries( this );
                MainSeries5.BeginInitialize();
            }
            if ( MainSeries6==null )
            {
                MainSeries6=new ABCChartBaseSeries( this );
                MainSeries6.BeginInitialize();
            }
            base.BeginInitialize();
            
        }
        public override void EndInitialize ( )
        {
            MainSeries1.EndInitialize();
            MainSeries2.EndInitialize();
            MainSeries3.EndInitialize();
            MainSeries4.EndInitialize();
            MainSeries5.EndInitialize();
            MainSeries6.EndInitialize();

            base.EndInitialize();
        }

        public override void InitSeries ( )
        {
            MainSeries1.InitSeries();
            MainSeries2.InitSeries();
            MainSeries3.InitSeries();
            MainSeries4.InitSeries();
            MainSeries5.InitSeries();
            MainSeries6.InitSeries();

        }

      
    }

    public class ABCChartSixSeriesControlDesigner : ControlDesigner
    {
        public ABCChartSixSeriesControlDesigner ( )
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
