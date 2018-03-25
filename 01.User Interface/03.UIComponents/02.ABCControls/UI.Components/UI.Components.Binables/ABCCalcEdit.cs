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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.CalcEdit ))]
    [Designer(typeof(ABCCalcEditDesigner))]
    public class ABCCalcEdit : DevExpress.XtraEditors.CalcEdit , IABCControl , IABCBindableControl
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

        [Browsable(false)]
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
        public String EditMask
        {
            get
            {
                return this.Properties.Mask.EditMask;
            }
            set
            {
                this.Properties.Mask.EditMask=value;
            }
        }
        [Category( "External" )]
        public DevExpress.XtraEditors.Mask.MaskType MaskType
        {
            get
            {
                return this.Properties.Mask.MaskType;
            }
            set
            {
                this.Properties.Mask.MaskType=value;
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

        [Category( "External" )]
        public String DummyText
        {
            get
            {
                return this.Properties.NullValuePrompt;
            }
            set
            {
                this.Properties.NullValuePrompt=value;
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
       
        public ABCCalcEdit ()
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
            this.Properties.NullValuePrompt=DummyText;

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;
         
            

            if ( !String.IsNullOrWhiteSpace( EditMask.ToString() )&&!String.IsNullOrWhiteSpace( MaskType.ToString() ) )
                this.Properties.Mask.UseMaskAsDisplayFormat=true;

            this.Properties.NullValuePromptShowForEmptyValue=true;

            if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
                this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
        }
        public void InitRunTime ( )
        {
            this.Properties.Appearance.ForeColor=Color.Black;
            this.Properties.Appearance.Options.UseForeColor=true;
        }
        public void InitDesignTime ( )
        {
        }
        
        #endregion

    }

    public class ABCCalcEditDesigner : ControlDesigner
    {
        public ABCCalcEditDesigner ( )
        {
        }
    }
}
