using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace ABCControls
{
    /// <summary>
    /// This service resolved the types and is required when using the
    /// CodeDomHostLoader
    /// </summary>
    public class TypeResolutionService : ITypeResolutionService
    {
        public static TypeResolutionService CurrentService=new TypeResolutionService();

        public static Hashtable ht;

        public TypeResolutionService ( )
        {
            if(ht==null)
                 ht=new Hashtable();
            if ( CurrentService==null )
                CurrentService=this;
            else
                return;

            InitTypesFromAssembly( "ABCControls.dll" );

           AddType( typeof( System.Drawing.Size ) );
           AddType( typeof( System.Drawing.Point ) );
           AddType( typeof( System.Drawing.Color ) );
           AddType( typeof( System.Drawing.Bitmap ) );
           AddType( typeof( System.Drawing.Image ) );
           AddType( typeof( System.Drawing.Icon ) );
           AddType( typeof( System.Drawing.Font ) );
           AddType( typeof( System.Drawing.FontFamily ) );

           AddType( typeof( DevExpress.XtraEditors.XtraForm ) );
           AddType( typeof( DevExpress.XtraEditors.XtraForm ) );

           AddType( typeof( DevExpress.XtraEditors.GroupControl ) );
           AddType( typeof( DevExpress.XtraTab.XtraTabControl ) );
           AddType( typeof( System.Windows.Forms.FlowLayoutPanel ) );
           AddType( typeof( DevExpress.XtraEditors.PanelControl ) );
           AddType( typeof( DevExpress.XtraEditors.RadioGroup ) );

           AddType( typeof( DevExpress.XtraEditors.Controls.EditorButton ) );
           AddType( typeof( DevExpress.XtraEditors.Controls.EditorButtonCollection ) );
           AddType( typeof( DevExpress.XtraEditors.ButtonEdit ) );
           AddType( typeof( DevExpress.XtraEditors.DateControl ) );
           AddType( typeof( DevExpress.XtraEditors.DateEdit ) );
           AddType( typeof( DevExpress.XtraEditors.CalcEdit ) );
           AddType( typeof( DevExpress.XtraEditors.CheckEdit ) );
           AddType( typeof( DevExpress.XtraEditors.LabelControl ) );
           AddType( typeof( DevExpress.XtraEditors.TextEdit ) );
           AddType( typeof( DevExpress.XtraEditors.MemoEdit ) );
           AddType( typeof( DevExpress.XtraEditors.MemoExEdit ) );
           AddType( typeof( DevExpress.XtraEditors.RichTextEdit ) );

           AddType( typeof( DevExpress.XtraRichEdit.RichEditControl ) );

           AddType( typeof( DevExpress.XtraEditors.ComboBox ) );
           AddType( typeof( DevExpress.XtraEditors.ComboBoxEdit ) );
           AddType( typeof( DevExpress.XtraEditors.LookUpEdit ) );
           //  AddType( typeof( DevExpress.XtraEditors.GridLookUpEdit ) );

           AddType( typeof( DevExpress.XtraGrid.GridControl ) );
           AddType( typeof( DevExpress.XtraGrid.Views.Grid.GridView ) );
           AddType( typeof( DevExpress.XtraGrid.Views.Base.BaseView ) );

            InitTypesFromAssembly( typeof( System.Windows.Forms.Button ) );//System.Windows.Form
            InitTypesFromAssembly( typeof( System.Drawing.Image ) );//System.Drawing

            InitTypesFromAssembly( "DevExpress.Utils.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.Data.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.XtraEditors.v12.1.dll" );
      //      InitTypesFromAssembly( "DevExpress.XtraEditors.v12.1.Design.dll" );
            InitTypesFromAssembly( "DevExpress.XtraBars.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.XtraGrid.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.XtraNavbar.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.Printing.v12.1.Core.dll" );
            InitTypesFromAssembly( "DevExpress.RichEdit.v12.1.Core.dll" );
            InitTypesFromAssembly( "DevExpress.XtraTreeList.v12.1.dll" );
            InitTypesFromAssembly( "DevExpress.XtraLayout.v12.1.dll" );

            InitTypesFromAssembly( typeof( System.ComponentModel.Design.DesignSurface ) );//System.Design
            InitTypesFromAssembly( typeof( System.Enum ) );//System
            InitTypesFromAssembly( typeof( System.Uri ) );//System
            InitTypesFromAssembly( typeof( System.Drawing.Design.ToolboxItem ) );//System.Drawing.Design
            InitTypesFromAssembly( typeof( System.Data.DataRow ) );//System.Data
            InitTypesFromAssembly( typeof( System.TimeZoneInfo ) );//System.Core

        }

        public static void AddType ( Type type )
        {
            if ( ht.ContainsKey( type.FullName )==false )
                ht.Add( type.FullName , type );
            if ( ht.ContainsKey( type.Name )==false )
                ht.Add( type.Name , type );
        }
        public void InitTypesFromAssembly ( String strFileName )
        {
            try
            {
                Assembly winForms=Assembly.LoadFrom( strFileName );
                Type[] types=winForms.GetTypes();
                foreach ( Type type in types )
                    AddType( type );
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message , strFileName );
            }
        }
        public void InitTypesFromAssembly ( Type typeInAssembly )
        {
            Assembly winForms=Assembly.GetAssembly(typeInAssembly);
            Type[] types=winForms.GetTypes();
            foreach ( Type type in types )
                AddType( type );
        }

        #region ITypeResolutionService
        public System.Reflection.Assembly GetAssembly ( System.Reflection.AssemblyName name )
        {
            return GetAssembly( name , true );
        }
        public System.Reflection.Assembly GetAssembly ( System.Reflection.AssemblyName name , bool throwOnErrors )
        {
            return Assembly.GetAssembly( typeof( DevExpress.XtraEditors.XtraForm ) );
        }
        public string GetPathOfAssembly ( System.Reflection.AssemblyName name )
        {
            return null;
        }
        public Type GetType ( string name )
        {
            return this.GetType( name , true );
        }
        public Type GetType ( string name , bool throwOnError )
        {
            return this.GetType( name , throwOnError , false );
        }
        public Type GetType ( string name , bool throwOnError , bool ignoreCase )
        {

            if ( ht.ContainsKey( name ) )
                return (Type)ht[name];

            return Type.GetType( name , false , ignoreCase );
        }
        public void ReferenceAssembly ( System.Reflection.AssemblyName name )
        {
        }

        #endregion


    }
}
