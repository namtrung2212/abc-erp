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
    public class ABCChartLineSeries : ABCChartBaseSeries
    {

        public ABCChartLineSeries (ABCChartBaseControl parentChart ):base(parentChart)
        {
        }

        public override void BeginInitialize ( )
        {
            seriesLabel=new DevExpress.XtraCharts.PointSeriesLabel();
            seriesView=new DevExpress.XtraCharts.LineSeriesView();
            pointOptions=new DevExpress.XtraCharts.PointOptions();

            base.BeginInitialize();
        }

        public override void InitSeries ( )
        {
           
            base.InitSeries();
        }
    }
}
