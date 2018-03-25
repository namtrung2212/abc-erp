using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;


using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.SplitterControl ) )]
    [Designer( typeof( ABCSplitterControlDesigner ) )]
    public class ABCSplitterControl : DevExpress.XtraEditors.SplitterControl, IABCControl
    {
        public ABCView OwnerView { get; set; }

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
        public ABCSplitterControl ( )
        {


        }

        public void InitControl ( )
        {
        }

        public String GetPropertyBindingName ( )
        {
            return "";
        }
    }

    public class ABCSplitterControlDesigner : ControlDesigner
    {
        public ABCSplitterControlDesigner ( )
        {
        }
    }
}
