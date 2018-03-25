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
    [Designer( typeof( ABCChartBarControlDesigner ) )]
    public class ABCChartBarControl : ABCChartOneSeriesControl
    {

        public ABCChartBarControl ( )
        {
        
        }

        public override void BeginInitialize ( )
        {
            MainSeries=new ABCChartBarSeries( this );
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

       
    }

    public class ABCChartBarControlDesigner : ControlDesigner
    {
        public ABCChartBarControlDesigner ( )
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
