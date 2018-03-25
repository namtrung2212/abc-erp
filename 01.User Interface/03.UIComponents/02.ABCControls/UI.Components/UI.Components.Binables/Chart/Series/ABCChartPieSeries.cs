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
    public class ABCChartPieSeries : ABCChartBaseSeries
    {

        public ABCChartPieSeries (ABCChartBaseControl parentChart ):base(parentChart)
        {
        }

        public override void BeginInitialize ( )
        {
            seriesLabel=new DevExpress.XtraCharts.PieSeriesLabel();
            seriesView=new DevExpress.XtraCharts.PieSeriesView();
            pointOptions=new DevExpress.XtraCharts.PiePointOptions();

            base.BeginInitialize();
        }

        public override void InitSeries ( )
        {
            if ( pointOptions is PiePointOptions )
            {
                ( pointOptions as PiePointOptions ).PercentOptions.PercentageAccuracy=4;
                ( pointOptions as PiePointOptions ).ValueNumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Percent;
                ( pointOptions as PiePointOptions ).PercentOptions.ValueAsPercent=true;
            }

       
            if ( seriesLabel is PieSeriesLabel )
                ( seriesLabel as PieSeriesLabel ).Position=DevExpress.XtraCharts.PieSeriesLabelPosition.Inside;

            base.InitSeries();
        }
      
        #region Properties
    
        [Category( "SeriesLabel" )]
        public PieSeriesLabelPosition Position
        {
            get
            {
                if ( this.Label is PieSeriesLabel )
                    return ( this.Label as PieSeriesLabel ).Position;
                else
                    return PieSeriesLabelPosition.Inside;
            }
            set
            {
                if ( this.Label is PieSeriesLabel )
                    ( this.Label as PieSeriesLabel ).Position=value;
            }
        }

        #endregion
    }
}
