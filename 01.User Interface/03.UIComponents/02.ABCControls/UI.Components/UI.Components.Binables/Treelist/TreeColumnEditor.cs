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
    public class TreeColumnsEditor : UITypeEditor
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
                if ( context.Instance is ABCTreeList )
                {
                    ABCTreeList Tree=context.Instance as ABCTreeList;
                    using ( TreeColumnConfigForm form=new TreeColumnConfigForm( Tree.ColumnConfigs ,Tree.Manager.RootConfig) )
                    {
                        form.TableName=Tree.TableName;
                        form.Script=Tree.Script;
                        if ( svc.ShowDialog( form )==DialogResult.OK )
                        {
                            Tree.ColumnConfigs=form.ColumnList;
                            Tree.Manager=form.Manager;
                            Tree.Manager.TreeList=Tree;
                            Tree.InitColumns();
                        }
                    }
                }
            }

         
            
            return value;
        }
    }
}