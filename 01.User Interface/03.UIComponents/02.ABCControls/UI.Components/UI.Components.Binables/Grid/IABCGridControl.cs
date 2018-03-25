using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABCControls
{
    public class ABCDefineEvents
    {
        public delegate void ABCBarItemClickEventHandler ( object sender , String strTag );
    }
    public interface IABCGridControl : IABCBindableControl
    {
        ABCView OwnerView { get; set; }
        object GridDataSource { get; set; }
        DevExpress.XtraGrid.Views.Grid.GridView GridDefaultView { get; }
        string ViewCaption { get; set; }
        String Header { get; set; }
        String Footer { get; set; }
        String ReportName { get; set; }

        event ABCDefineEvents.ABCBarItemClickEventHandler BarItemClick;
      
    }

    public interface IABCGridView
    {
        IABCGridControl ABCGridControl { get; set; }
        String TableName { get; set; }
    }
}
