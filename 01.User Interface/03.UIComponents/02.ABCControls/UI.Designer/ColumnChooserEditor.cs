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
    public class ColumnChooserEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue ( ITypeDescriptorContext context , System.IServiceProvider provider , object value )
        {
            IWindowsFormsEditorService svc=null;
            if ( provider!=null )
                svc=(IWindowsFormsEditorService)provider.GetService( typeof( IWindowsFormsEditorService ) );

            String strContextFieldName=String.Empty;
            if ( context is DevExpress.XtraVerticalGrid.Data.DescriptorContext )
                strContextFieldName=( context as DevExpress.XtraVerticalGrid.Data.DescriptorContext ).FieldName;
          //else if ( context is System.Windows.Forms.PropertyGridInternal.GridEntry)
          //      strContextFieldName=( context as System.Windows.Forms.PropertyGrid.ControlAccessibleObject.).;

            if ( svc!=null ) 
            {
                String strTableName=String.Empty;
                if ( context.Instance is ABCScreen.ABCBindingConfig )
                {
                    if ( strContextFieldName=="ChildField" )
                        strTableName=( (ABCScreen.ABCBindingConfig)context.Instance ).TableName;
                    else if ( strContextFieldName=="ParentField" )
                    {
                        String strDataSource=( (ABCScreen.ABCBindingConfig)context.Instance ).ParentName;
                        if ( String.IsNullOrWhiteSpace( strDataSource ) )
                            return value;

                        ABCView view=(ABCView)( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).DesignerHost.RootComponent;
                        if ( view.DataConfig.BindingList.ContainsKey( strDataSource ) )
                            strTableName=view.DataConfig.BindingList[strDataSource].TableName;
              
                    }
                }
                else if ( context.Instance is IABCControl)
                {
                    if ( String.IsNullOrWhiteSpace(strContextFieldName)==false&&strContextFieldName.EndsWith( "DataMember" ) )
                    {
                        String strDataSource=( (IABCControl)context.Instance ).GetType().GetProperty( "DataSource" ).GetValue( context.Instance , null ).ToString();
                        ABCView view=(ABCView)( (HostSurface)HostSurfaceManager.CurrentManager.ActiveDesignSurface ).DesignerHost.RootComponent;
                        if ( view.DataConfig.BindingList.ContainsKey( strDataSource ) )
                            strTableName=view.DataConfig.BindingList[strDataSource].TableName;
                    }
                }
                
                if ( context.Instance is ABCLookUpEdit||context.Instance is ABCGridLookUpEdit )
                {
                    if ( strContextFieldName.EndsWith( "Member" ) )
                    {
                        if ( context.Instance is ABCLookUpEdit )
                            strTableName=( context.Instance as ABCLookUpEdit ).TableName;
                        if ( context.Instance is ABCGridLookUpEdit )
                            strTableName=( context.Instance as ABCGridLookUpEdit ).TableName;
                    }
                }

                if(( context.Instance is ABCSearchPanel))
                {
                    strTableName=( context.Instance as ABCSearchPanel ).TableName;
                    using ( ColumnChooserForm form=new ColumnChooserForm( strTableName ) )
                    {
                        List<String> oldArr=null;
                        if ( value!=null )
                            oldArr=(List<String>)value;
                        List<String> arrResult=form.ShowChoose( oldArr.ToArray() );
                        if ( form.DialogResult==DialogResult.OK )
                            value=arrResult;
                    }
                }
                if ( context.Instance is ABCControls.TreeConfigData  )
                {
                    if ( strContextFieldName=="ChildField" )
                        strTableName=( ( ABCControls.TreeConfigData)context.Instance ).TableName;
                    else if ( strContextFieldName=="ParentField" )
                        strTableName=( (ABCControls.TreeConfigData)context.Instance ).ParentTableName;
                }

                if ( string.IsNullOrWhiteSpace( strTableName ) )
                    return value;

                using ( ColumnChooserForm form=new ColumnChooserForm( strTableName ) )
                {
                    String strOld=String.Empty;
                    if ( value!=null )
                        strOld=value.ToString();
                    String strResult=form.ShowChooseOne( strOld );
                    if ( form.DialogResult==DialogResult.OK )
                        value=strResult;
                }
            }

            if ( context.Instance is ABCGridControl )
                ( (ABCGridControl)context.Instance ).DefaultView.InitColumns();
            return value;
        }
    }
}