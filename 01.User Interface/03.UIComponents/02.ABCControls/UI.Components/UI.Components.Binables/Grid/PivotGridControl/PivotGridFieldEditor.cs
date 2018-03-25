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
    public class PivotGridFieldsEditor : UITypeEditor
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
                if ( context.Instance is ABCPivotGridControl )
                {
                    ABCPivotGridControl grid=context.Instance as ABCPivotGridControl;
                    using ( PivotGridFieldConfigForm form=new PivotGridFieldConfigForm( grid.FieldConfigs) )
                    {
                        form.TableName=grid.TableName;
                        form.RowTreeWidth=grid.Grid.OptionsView.RowTreeWidth;
                        form.UseChartControl=grid.UseChartControl;
                        form.Script=grid.Script;

                        if ( svc.ShowDialog( form )==DialogResult.OK )
                        {
                            grid.FieldConfigs=form.FieldsList;
                            grid.InitFields();
                            grid.Grid.OptionsView.RowTreeWidth=form.RowTreeWidth;
                            grid.UseChartControl=form.UseChartControl;
               //             grid.Script=form.Script;
                        }
                    }
             

                }
            }

         
            
            return value;
        }
    }
}