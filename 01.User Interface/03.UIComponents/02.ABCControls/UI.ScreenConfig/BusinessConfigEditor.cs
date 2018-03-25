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
    public class ABCBusinessConfigEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue ( ITypeDescriptorContext context , System.IServiceProvider provider , object value )
        {

            if ( context.Instance!=( HostSurfaceManager.CurrentManager.ActiveDesignSurface as HostSurface ).DesignerHost.RootComponent )
            {
                ABCHelper.ABCMessageBox.Show( "Can not modify the Binding Configurations of child View ! " , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return null;
            }

            IWindowsFormsEditorService svc=null;
            if ( provider!=null )
                svc=(IWindowsFormsEditorService)provider.GetService( typeof( IWindowsFormsEditorService ) );

            if ( svc!=null )
            {
                ABCView view=(ABCView)context.Instance;
                using ( ABCBusinessConfigEditorForm form=new ABCBusinessConfigEditorForm( view ) )
                {

                    form.DataConfig=view.DataConfig;
                    if ( form.DataConfig==null )
                        form.DataConfig=new ABCScreen.ABCScreenConfig( view );
                    if ( svc.ShowDialog( form )==DialogResult.OK )
                        value=form.NewDataConfig;
                }
            }

            return value;
        }
    }
}