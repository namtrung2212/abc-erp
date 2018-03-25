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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.MemoExEdit ) )]
    [Designer( typeof( ABCMemoExEditDesigner ) )]
    public class ABCMemoExEdit : DevExpress.XtraEditors.MemoExEdit , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "Text";
            }
        }
        #endregion

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
                 if(OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }
        [Category( "External" )]
        public Boolean ReadOnly
        {
            get
            {
                return this.Properties.ReadOnly;
            }
            set
            {
                this.Properties.ReadOnly=value;
            }
        }
        #endregion

        public ABCMemoExEdit ( )
        {


        }

        #region IABCControl
        public void InitControl ( )
        {
            this.Properties.Appearance.ForeColor=Color.Black;
            this.Properties.Appearance.Options.UseForeColor=true;
        }
        
        #endregion
    }

    public class ABCMemoExEditDesigner : ControlDesigner
    {
        public ABCMemoExEditDesigner ( )
        {
        }
    }
}
