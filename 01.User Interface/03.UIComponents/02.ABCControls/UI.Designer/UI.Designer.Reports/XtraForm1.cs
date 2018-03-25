using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ABCControls.Designer.ReportDesigner
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1 ( )
        {
            InitializeComponent();
            this.FormClosing+=new FormClosingEventHandler( XtraForm1_FormClosing );
         //   xrDesignPanel1.OpenReport( new XtraReport1() );
        }

        void XtraForm1_FormClosing ( object sender , FormClosingEventArgs e )
        {
      //      xrDesignPanel1.SaveReportAs();
        }

        private void commandColorBarItem1_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {

        }

     
    }
}