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
    public class ABCChartBarSeries : ABCChartBaseSeries
    {

        public ABCChartBarSeries (ABCChartBaseControl parentChart ):base(parentChart)
        {
        }

        public override void BeginInitialize ( )
        {
            base.BeginInitialize();

        
        }

        public override void InitSeries ( )
        {
            ( this.seriesView as SideBySideBarSeriesView ).Shadow.Visible=true;
            ( this.seriesView as SideBySideBarSeriesView ).Shadow.Size=3;
  //        ( this.seriesView as SideBySideBarSeriesView ).Shadow.Color=Color.Gray;

            base.InitSeries();
        }
    }
}
