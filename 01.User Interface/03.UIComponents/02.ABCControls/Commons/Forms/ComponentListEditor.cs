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




namespace ABCControls.TypeEditor
{
    public class ComponentEditor<T> : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue ( ITypeDescriptorContext context , IServiceProvider provider , object value )
        {

            IWindowsFormsEditorService editorService=null;

            if ( context!=null&&context.Instance!=null&&provider!=null )
            {

                editorService=provider.GetService( typeof( IWindowsFormsEditorService ) ) as IWindowsFormsEditorService;
                if ( editorService!=null )
                {

                    ListBox boxEdit=new ListBox();
                    foreach ( Component component in context.Container.Components )
                    {
                        if ( component is T )
                            boxEdit.Items.Add((component  as Control ).Name);
                    }

                    editorService.DropDownControl( boxEdit );

                    if ( boxEdit.SelectedItem!=null )
                    {
                        value=boxEdit.SelectedItem;
                    }
                    else
                    {
                        value=null;
                    }

                }

            }

            return value;

        }
    }
}