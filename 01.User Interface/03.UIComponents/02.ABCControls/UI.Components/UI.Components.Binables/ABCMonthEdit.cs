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

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( ABCComboBoxEdit ) )]
    [Designer( typeof( ABCMonthEditDesigner ) )]
    public class ABCMonthEdit : ABCComboBoxEdit , DataFormatProvider.IDontNeedFormatControl
    {

        public ABCMonthEdit ( )
        {
        }

        public override object EditValue
        {
            get
            {
                if ( base.EditValue!=null&&base.EditValue!=DBNull.Value )
                    return Convert.ToInt32( base.EditValue );
                return base.EditValue;
            }
            set
            {
                base.EditValue=value;
            }
        }
        #region IABCControl

        public override void InitRunTime ( )
        {
            for ( int i=1; i<=12; i++ )
                this.Properties.Items.Add( i );

            this.EditValue=DateTime.Now.Month;
        }
        
        #endregion

    }

    public class ABCMonthEditDesigner : ControlDesigner
    {
        public ABCMonthEditDesigner ( )
        {
        }
    }
}
