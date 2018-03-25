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
using ABCDataLib;


namespace ABCPresentLib
{
    public class GridColumnsEditor : UITypeEditor
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
                if ( context.Instance is ABCGridControl )
                {
                    ABCGridControl grid=context.Instance as ABCGridControl;
                    using ( GridColumnConfigForm form=new GridColumnConfigForm( grid.DefaultView.ColumnConfigs ) )
                    {
                        form.TableName=grid.TableName;
                        form.Script=grid.Script;
                        if ( svc.ShowDialog( form )==DialogResult.OK )
                        {
                            grid.DefaultView.ColumnConfigs=form.ColumnList;
                            grid.DefaultView.InitColumns();
                        }
                    }
             

                }
            }

         
            
            return value;
        }
    }
}