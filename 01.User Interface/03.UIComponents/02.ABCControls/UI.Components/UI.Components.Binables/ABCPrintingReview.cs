using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;


using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraPrinting.Control.PrintControl) )]
    [Designer( typeof( ABCPrintingPreviewDesigner ) )]
    public partial class ABCPrintingPreview :  DevExpress.XtraEditors.XtraUserControl ,IABCControl 
    {
        public ABCView OwnerView { get; set; }
        public ABCPrintingPreview ( )
        {
            InitializeComponent();
        }

        public String ReportName { get; set; }
        public String GridDataControl { get; set; }

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

        bool isVisible=true;
        [Category( "External" )]
        public Boolean IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible=value;
                if ( OwnerView!=null &&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

    
        #endregion

        #region IABCControl

        public void InitControl ( )
        {
            
        }
        #endregion

    }

    public class ABCPrintingPreviewDesigner : ControlDesigner
    {
        public ABCPrintingPreviewDesigner ( )
        {
        }
    }
}
