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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.XtraScrollableControl ) )]
    [Designer( typeof( ABCScrollableControlDesigner ) )]
    public class ABCScrollableControl : DevExpress.XtraEditors.XtraScrollableControl , IABCControl
    {
        public ABCView OwnerView { get; set; }

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldScrollable { get; set; }

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
                 if(OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }
        #endregion

        public ABCScrollableControl ( )
        {


        }

        #region IABCControl
        public void InitControl ( )
        {
        } 
        #endregion

    }

    public class ABCScrollableControlDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public ABCScrollableControlDesigner ( )
        {
        }
    }
}
