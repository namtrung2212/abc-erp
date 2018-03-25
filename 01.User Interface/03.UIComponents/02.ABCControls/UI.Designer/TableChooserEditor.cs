using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;



namespace ABCControls
{
    public class TableChooserEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue ( ITypeDescriptorContext context , System.IServiceProvider provider , object value )
        {
            IWindowsFormsEditorService svc=null;
            if ( provider!=null )
                svc=(IWindowsFormsEditorService)provider.GetService( typeof( IWindowsFormsEditorService ) );

            if ( svc!=null )
            {
                using ( TableChooserForm form=new TableChooserForm() )
                {
                    String strOld=String.Empty;
                    if ( value!=null )
                        strOld=value.ToString();
                    String strResult=form.ShowChooseOne( strOld );
                    if ( form.DialogResult==DialogResult.OK )
                        value=strResult;
                }
            }

         
            return value;
        }
    }
}