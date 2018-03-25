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
using ABCProvider;
using ABCProvider;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( ABCComboBoxEdit ) )]
    [Designer( typeof( ABCYearEditDesigner ) )]
    public class ABCYearEdit : ABCComboBoxEdit , DataFormatProvider.IDontNeedFormatControl
    {

        public ABCYearEdit ( )
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
            for ( int i=1900; i<DateTime.Now.Year+50; i++ )
                this.Properties.Items.Add( i );

            this.EditValue=DateTime.Now.Year;
        }
        
        #endregion

    }

    public class ABCYearEditDesigner : ControlDesigner
    {
        public ABCYearEditDesigner ( )
        {
        }
    }
}
