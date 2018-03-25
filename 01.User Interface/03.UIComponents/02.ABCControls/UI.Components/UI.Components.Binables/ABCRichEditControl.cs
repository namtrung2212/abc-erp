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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraRichEdit.RichEditControl ) )]
    [Designer( typeof( ABCRichEditControlDesigner ) )]
    public partial class ABCRichEditControl :  DevExpress.XtraEditors.XtraUserControl ,IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }
        public ABCRichEditControl ( )
        {
            InitializeComponent();
        }
      
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
                return "RtfText";
            }
        }
        #endregion

        public String RtfText
        {
            get { return richEditControl1.RtfText; }
            set { richEditControl1.RtfText=value; }
        }


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

        bool isReadOnly=false;
        [Category( "External" )]
        public Boolean ReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly=value;
                this.ribbonControl1.Visible=!value;
                this.richEditControl1.ReadOnly=value;
                if ( value )
                {
                    this.richEditControl1.Options.HorizontalRuler.Visibility=DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
                    //             this.richEditControl1.Options.VerticalRuler.Visibility=DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
                }
                else
                {
                    this.richEditControl1.Options.HorizontalRuler.Visibility=DevExpress.XtraRichEdit.RichEditRulerVisibility.Visible;
                    //             this.richEditControl1.Options.VerticalRuler.Visibility=DevExpress.XtraRichEdit.RichEditRulerVisibility.Visible;
                }
            }
            
        }
        #endregion

        #region IABCControl

        public void InitControl ( )
        {
        }
        #endregion

    }

    public class ABCRichEditControlDesigner : ControlDesigner
    {
        public ABCRichEditControlDesigner ( )
        {
        }
    }
}
