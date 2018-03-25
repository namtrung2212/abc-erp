using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABCHelper
{

    public class ABCMessageBox
    {
        public static DialogResult Show ( string text )
        {
            ABCWaitingDialog.Close();

            return DevExpress.XtraEditors.XtraMessageBox.Show( text );

        }
        public static DialogResult Show ( string text , string caption , MessageBoxButtons buttons , MessageBoxIcon icon )
        {
            ABCWaitingDialog.Close();
            return DevExpress.XtraEditors.XtraMessageBox.Show( text , caption , buttons , icon );

        }
        public static DialogResult Show ( string text , string caption , MessageBoxButtons buttons )
        {
            ABCWaitingDialog.Close();

            return DevExpress.XtraEditors.XtraMessageBox.Show( text , caption , buttons );

        }


        public static DialogResult Show ( IWin32Window owner , string text )
        {
            ABCWaitingDialog.Close();

            return DevExpress.XtraEditors.XtraMessageBox.Show( owner , text );

        }
        public static DialogResult Show ( IWin32Window owner , string text , string caption , MessageBoxButtons buttons , MessageBoxIcon icon )
        {
            ABCWaitingDialog.Close();
            return DevExpress.XtraEditors.XtraMessageBox.Show( owner , text , caption , buttons , icon );

        }
        public static DialogResult Show ( IWin32Window owner , string text , string caption , MessageBoxButtons buttons )
        {
            ABCWaitingDialog.Close();

            return DevExpress.XtraEditors.XtraMessageBox.Show( owner , text , caption , buttons );

        }
    }
}
