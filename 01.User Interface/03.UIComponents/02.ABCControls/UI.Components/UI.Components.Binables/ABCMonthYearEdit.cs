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

using ABCProvider;
using ABCCommon;

namespace ABCControls
{
  
    [ToolboxBitmapAttribute( typeof( ABCComboBoxEdit ) )]
    [Designer( typeof( ABCMonthYearEditDesigner ) )]
    public partial class ABCMonthYearEdit : DevExpress.XtraEditors.XtraUserControl , DataFormatProvider.IDontNeedFormatControl , IABCControl , IABCBindableControl
    {

        public int Month
        {
            get { return Convert.ToInt32( cmbMonth.EditValue ); }
            set { cmbMonth.EditValue=value; }
        }
        public int Year
        {
            get { return Convert.ToInt32( cmbYear.EditValue ); }
            set { cmbYear.EditValue=value; }
        }

        public object EditValue
        {
            get
            {
                return new DateTime( this.Year , this.Month , 1 );
            }
            set
            {
                if ( value!=null&&value is DateTime )
                {
                    this.Year=( (DateTime)value ).Year;
                    this.Month=( (DateTime)value ).Month;
                }
            }
        }

        public ABCMonthYearEdit ( )
        {
            InitializeComponent();

            for ( int i=1; i<=12; i++ )
                cmbMonth.Properties.Items.Add( i );
            cmbMonth.Properties.AllowNullInput=DevExpress.Utils.DefaultBoolean.False;

            for ( int i=DateTime.Now.Year+3; i>DateTime.Now.Year-3; i-- )
                cmbYear.Properties.Items.Add( i );
            cmbYear.Properties.AllowNullInput=DevExpress.Utils.DefaultBoolean.False;

            this.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
        }
        
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
                 if(OwnerView!=null && OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }

        #endregion

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
        }
        public virtual void InitRunTime ( )
        {
            this.Appearance.ForeColor=Color.Black;
            this.Appearance.Options.UseForeColor=true;

          
        }
        public void InitDesignTime ( )
        {
        }

        #endregion

    }

    public class ABCMonthYearEditDesigner : ControlDesigner
    {
        public ABCMonthYearEditDesigner ( )
        {
        }
    }
}
