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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.RadioGroup ) )]
    [Designer( typeof( ABCRadioGroupDesigner ) )]
    public class ABCRadioGroup : DevExpress.XtraEditors.RadioGroup , IABCControl , IABCBindableControl
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
                return "EditValue";
            }
        }
        #endregion

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }
        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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

        public ABCRadioGroup ( )
        {
        }

        #region IABCControl
        public void InitControl ( )
        {
            InitBothMode();

              if ( OwnerView!=null &&OwnerView.Mode==ViewMode.Design)
                InitDesignTime();
            else
                InitRunTime();
        }

        public void InitBothMode ( )
        {

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;

            

            if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
                this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
        }
        public void InitRunTime ( )
        {

        }
        public void InitDesignTime ( )
        {
        }

        #endregion

        public void Initialize ( ABCBindingInfo bindingInfo )
        {
            this.DataSource=bindingInfo.BusName;
            this.DataMember=bindingInfo.FieldName;
            this.TableName=bindingInfo.TableName;
        }
    }

    public class ABCRadioGroupDesigner : ControlDesigner
    {
        public ABCRadioGroupDesigner ( )
        {
        }
    }
}
