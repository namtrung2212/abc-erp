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
    [Designer( typeof( ABCChartPieControlDesigner ) )]
    public class ABCChartPieControl : ABCChartOneSeriesControl
    {

        public ABCChartPieControl ( )
        {
            this.InnerChart.CustomDrawSeriesPoint+=new CustomDrawSeriesPointEventHandler( InnerChart_CustomDrawSeriesPoint );
        }

        void InnerChart_CustomDrawSeriesPoint ( object sender , CustomDrawSeriesPointEventArgs e )
        {
        }

        public override void BeginInitialize ( )
        {
            MainSeries=new ABCChartPieSeries( this );
            MainSeries.BeginInitialize();

            ( (System.ComponentModel.ISupportInitialize)( this.InnerChart ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( InnerDiagram ) ).BeginInit();
            this.SuspendLayout();
        }
        public override void EndInitialize ( )
        {
            MainSeries.EndInitialize();

            ( (System.ComponentModel.ISupportInitialize)( InnerDiagram ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.InnerChart ) ).EndInit();
            this.ResumeLayout( false );
        }

        [Category( "SeriesLabel" )]
        public PieSeriesLabelPosition Position
        {
            get
            {
                if ( Series1!=null && Series1.Label is PieSeriesLabel )
                    return ( Series1.Label as PieSeriesLabel ).Position;
                else
                    return PieSeriesLabelPosition.Inside;
            }
            set
            {
                if ( Series1!=null&&Series1.Label is PieSeriesLabel )
                    ( Series1.Label as PieSeriesLabel ).Position=value;
            }
        }


    }

    public class ABCChartPieControlDesigner : ControlDesigner
    {
        public ABCChartPieControlDesigner ( )
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
