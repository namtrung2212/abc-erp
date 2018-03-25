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
    public class GridBandedColumnsEditor : UITypeEditor
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
                if ( context.Instance is ABCGridBandedControl )
                {

                    ABCGridBandedControl grid=( (ABCGridBandedControl)context.Instance );
                    using ( GridBandedColumnConfigForm form=new GridBandedColumnConfigForm( grid.BandedView.ColumnConfigs , grid.BandedView.BandConfigs ) )
                    {
                        form.TableName=grid.TableName;
                        form.Script=grid.Script;
                        if ( svc.ShowDialog( form )==DialogResult.OK )
                        {
                            grid.BandedView.ColumnConfigs=form.ColumnList;
                            grid.BandedView.BandConfigs=form.BandsList;
                            grid.BandedView.LoadBands();
                        }
                    }
                }
            }
       
            return value;
        }
    }
}