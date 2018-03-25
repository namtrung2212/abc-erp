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

using ABCProvider;


namespace ABCControls.TypeEditor
{
    public class FilterConditionEditor : UITypeEditor
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

                String strTableName=String.Empty;

                if ( ( context as DevExpress.XtraVerticalGrid.Data.DescriptorContext ).FieldName=="FilterCondition"&&context.Instance.GetType().FullName==typeof(ABCScreen.ABCBindingConfig).FullName )
                {
                    //if ( Convert.ToBoolean( context.Instance.GetType().GetProperty( "IsList" ).GetValue( context.Instance , null ) )==false )
                    //{
                    //    ABCHelper.ABCMessageBox.Show( "Please set IsList = true before use this function." );
                    //    return value;
                    //}
                    strTableName=context.Instance.GetType().GetProperty( "TableName" ).GetValue( context.Instance , null ).ToString();
                }
                if ( ( context as DevExpress.XtraVerticalGrid.Data.DescriptorContext ).FieldName.EndsWith("Editor.FilterStringEx")&&context.Instance.GetType().FullName.EndsWith("LookUpEdit" ))
                {
                    strTableName=context.Instance.GetType().GetProperty( "TableName" ).GetValue( context.Instance , null ).ToString();
                    String strFieldName=context.Instance.GetType().GetProperty( "DataMember" ).GetValue( context.Instance , null ).ToString();
                    if(DataStructureProvider.IsForeignKey(strTableName,strFieldName)==false)
                        return value;

                    strTableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , strFieldName );
                }

                if ( String.IsNullOrWhiteSpace( strTableName )==false )
                {
                    using ( ABCCommonForms.FilterBuilderForm form=new ABCCommonForms.FilterBuilderForm( strTableName ) )
                    {
                        String strOld=String.Empty;
                        if ( value!=null )
                            strOld=value.ToString();

                        form.SetFilterString( strOld );
                        if ( form.ShowDialog()==DialogResult.OK )
                            value=form.FilterString;
                    }

                }
            }

         
            return value;
        }
    }
}