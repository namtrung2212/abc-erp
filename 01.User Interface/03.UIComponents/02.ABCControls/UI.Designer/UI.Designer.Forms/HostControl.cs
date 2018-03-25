using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using ScintillaNET;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


using ABCCommon;

namespace ABCControls
{

    public class HostControl : DevExpress.XtraEditors.XtraUserControl
    {
        public HostSurface HostSurface;
        public IDesignerHost DesignerHost
        {
            get
            {
                return (IDesignerHost)HostSurface.GetService( typeof( IDesignerHost ) );
            }
        }

        public HostControl ( HostSurface hostSurface )
        {
            hostSurface.OwnerHostControl=this;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            InitializeHost( hostSurface );
            TabControl.SelectedPageChanging+=new DevExpress.XtraTab.TabPageChangingEventHandler( xtraTabControl1_SelectedPageChanging );
            SourceCSharpEditor.TextChanged+=new EventHandler(SourceCSharpEditor_TextChanged);
        }

        void SourceCSharpEditor_TextChanged ( object sender , EventArgs e )
        {
            if ( ( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField!=null )
                ( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField.STViewCode=SourceCSharpEditor.Text;
        }

        #region internal
        private DevExpress.XtraTab.XtraTabPage tabPageDesigner;
        private DevExpress.XtraTab.XtraTabPage tabPageCode;
        private DevExpress.XtraTab.XtraTabControl TabControl;
        public Scintilla SourceCSharpEditor;
        private DevExpress.XtraTab.XtraTabControl TabCtrlCode;
        private DevExpress.XtraTab.XtraTabPage tabCSharp;
        private DevExpress.XtraTab.XtraTabPage tabXML;
        private Scintilla SourceXMLEditor;
        private DevExpress.XtraTab.XtraTabPage tabPreview;



        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                if ( HostSurface!=null )
                    HostSurface.Dispose();

                if ( components!=null )
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( HostControl ) );
            this.tabPageDesigner=new DevExpress.XtraTab.XtraTabPage();
            this.tabPageCode=new DevExpress.XtraTab.XtraTabPage();
            this.TabCtrlCode=new DevExpress.XtraTab.XtraTabControl();
            this.tabCSharp=new DevExpress.XtraTab.XtraTabPage();
            this.SourceCSharpEditor=new Scintilla();
            this.tabXML=new DevExpress.XtraTab.XtraTabPage();
            this.SourceXMLEditor=new Scintilla();
            this.TabControl=new DevExpress.XtraTab.XtraTabControl();
            this.tabPreview=new DevExpress.XtraTab.XtraTabPage();
            this.tabPageCode.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.TabCtrlCode ) ).BeginInit();
            this.TabCtrlCode.SuspendLayout();
            this.tabCSharp.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.SourceCSharpEditor ) ).BeginInit();
            this.tabXML.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.SourceXMLEditor ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.TabControl ) ).BeginInit();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageDesigner
            // 
            this.tabPageDesigner.Name="tabPageDesigner";
            this.tabPageDesigner.Size=new System.Drawing.Size( 650 , 478 );
            this.tabPageDesigner.Text="       Designer      ";
            // 
            // tabPageCode
            // 
            this.tabPageCode.Controls.Add( this.TabCtrlCode );
            this.tabPageCode.Name="tabPageCode";
            this.tabPageCode.Size=new System.Drawing.Size( 650 , 478 );
            this.tabPageCode.Text="  Source Code  ";
            // 
            // TabCtrlCode
            // 
            this.TabCtrlCode.Dock=System.Windows.Forms.DockStyle.Fill;
            this.TabCtrlCode.Location=new System.Drawing.Point( 0 , 0 );
            this.TabCtrlCode.Name="TabCtrlCode";
            this.TabCtrlCode.SelectedTabPage=this.tabCSharp;
            this.TabCtrlCode.Size=new System.Drawing.Size( 650 , 478 );
            this.TabCtrlCode.TabIndex=2;
            this.TabCtrlCode.TabPages.AddRange( new DevExpress.XtraTab.XtraTabPage[] {
            this.tabCSharp,
            this.tabXML} );
            // 
            // tabCSharp
            // 
            this.tabCSharp.Controls.Add( this.SourceCSharpEditor );
            this.tabCSharp.Name="tabCSharp";
            this.tabCSharp.Size=new System.Drawing.Size( 644 , 452 );
            this.tabCSharp.Text="        C#        ";
            // 
            // SourceCSharpEditor
            // 
            this.SourceCSharpEditor.ConfigurationManager.Language="cs";
            this.SourceCSharpEditor.Dock=System.Windows.Forms.DockStyle.Fill;
            this.SourceCSharpEditor.Indentation.ShowGuides=true;
            this.SourceCSharpEditor.Indentation.SmartIndentType=SmartIndent.Simple;
            this.SourceCSharpEditor.Location=new System.Drawing.Point( 0 , 0 );
            this.SourceCSharpEditor.Margins.Margin0.Width=40;
            this.SourceCSharpEditor.Margins.Margin2.Width=20;
            this.SourceCSharpEditor.Name="SourceCSharpEditor";
            this.SourceCSharpEditor.Size=new System.Drawing.Size( 644 , 452 );
            this.SourceCSharpEditor.Styles.BraceBad.FontName="Verdana";
            this.SourceCSharpEditor.Styles.BraceLight.FontName="Verdana";
            this.SourceCSharpEditor.Styles.ControlChar.FontName="Verdana";
            this.SourceCSharpEditor.Styles.Default.FontName="Verdana";
            this.SourceCSharpEditor.Styles.IndentGuide.FontName="Verdana";
            this.SourceCSharpEditor.Styles.LastPredefined.FontName="Verdana";
            this.SourceCSharpEditor.Styles.LineNumber.FontName="Verdana";
            this.SourceCSharpEditor.Styles.Max.FontName="Verdana";
            this.SourceCSharpEditor.TabIndex=1;
            this.SourceCSharpEditor.Whitespace.Mode=WhitespaceMode.VisibleAfterIndent;
            // 
            // tabXML
            // 
            this.tabXML.Controls.Add( this.SourceXMLEditor );
            this.tabXML.Name="tabXML";
            this.tabXML.Size=new System.Drawing.Size( 644 , 452 );
            this.tabXML.Text="        XML        ";
            // 
            // SourceXMLEditor
            // 
            this.SourceXMLEditor.ConfigurationManager.Language="xml";
            this.SourceXMLEditor.Dock=System.Windows.Forms.DockStyle.Fill;
            this.SourceXMLEditor.Indentation.ShowGuides=true;
            this.SourceXMLEditor.Indentation.SmartIndentType=SmartIndent.Simple;
            this.SourceXMLEditor.Location=new System.Drawing.Point( 0 , 0 );
            this.SourceXMLEditor.Margins.Margin0.Width=40;
            this.SourceXMLEditor.Margins.Margin2.Width=20;
            this.SourceXMLEditor.Name="SourceXMLEditor";
            this.SourceXMLEditor.Size=new System.Drawing.Size( 644 , 452 );
            this.SourceXMLEditor.Styles.BraceBad.FontName="Verdana";
            this.SourceXMLEditor.Styles.BraceLight.FontName="Verdana";
            this.SourceXMLEditor.Styles.ControlChar.FontName="Verdana";
            this.SourceXMLEditor.Styles.Default.FontName="Verdana";
            this.SourceXMLEditor.Styles.IndentGuide.FontName="Verdana";
            this.SourceXMLEditor.Styles.LastPredefined.FontName="Verdana";
            this.SourceXMLEditor.Styles.LineNumber.FontName="Verdana";
            this.SourceXMLEditor.Styles.Max.FontName="Verdana";
            this.SourceXMLEditor.TabIndex=2;
            this.SourceXMLEditor.Text=resources.GetString( "SourceXMLEditor.Text" );
            this.SourceXMLEditor.Whitespace.Mode=WhitespaceMode.VisibleAfterIndent;
            // 
            // TabControl
            // 
            this.TabControl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.TabControl.HeaderLocation=DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.TabControl.Location=new System.Drawing.Point( 0 , 0 );
            this.TabControl.Name="TabControl";
            this.TabControl.SelectedTabPage=this.tabPageDesigner;
            this.TabControl.Size=new System.Drawing.Size( 656 , 504 );
            this.TabControl.TabIndex=1;
            this.TabControl.TabPages.AddRange( new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageDesigner,
            this.tabPageCode,
            this.tabPreview} );
            // 
            // tabPreview
            // 
            this.tabPreview.Name="tabPreview";
            this.tabPreview.Size=new System.Drawing.Size( 650 , 478 );
            this.tabPreview.Text="      Preview     ";
            // 
            // HostControl
            // 
            this.Controls.Add( this.TabControl );
            this.Name="HostControl";
            this.Size=new System.Drawing.Size( 656 , 504 );
            this.tabPageCode.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.TabCtrlCode ) ).EndInit();
            this.TabCtrlCode.ResumeLayout( false );
            this.tabCSharp.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.SourceCSharpEditor ) ).EndInit();
            this.tabXML.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.SourceXMLEditor ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.TabControl ) ).EndInit();
            this.TabControl.ResumeLayout( false );
            this.ResumeLayout( false );

        }
        #endregion



        internal void InitializeHost ( HostSurface hostSurface )
        {
            try
            {
                if ( hostSurface==null )
                    return;

                HostSurface=hostSurface;

                Control control=HostSurface.View as Control;

                control.Parent=this.tabPageDesigner;
                control.Dock=DockStyle.Fill;
                control.Visible=true;
                control.PreviewKeyDown+=new PreviewKeyDownEventHandler( control_PreviewKeyDown );
            }
            catch ( Exception ex )
            {
                Trace.WriteLine( ex.ToString() );
            }
        }

        void control_PreviewKeyDown ( object sender , PreviewKeyDownEventArgs e )
        {
            //if(e.KeyCode==Keys.Delete)
            //    this.HostSurface.PerformAction( System.ComponentModel.Design.StandardCommands.Delete );
        }
        #endregion



        void xtraTabControl1_SelectedPageChanging ( object sender , DevExpress.XtraTab.TabPageChangingEventArgs e )
        {
            if ( e.Page==tabPageCode )
            {
                #region Source Code
                //   String strSOurceCOde=( (CodeDomHostLoader)HostSurface.Loader ).GetCode( "C#" );
                SourceCSharpEditor.IsReadOnly=false;
                if ( ( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField!=null )
                {
                    if ( String.IsNullOrWhiteSpace( ( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField.STViewCode ) )
                        SourceCSharpEditor.Text=GenerateScreenSourceCode( ( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField.STViewNo );
                    else
                        SourceCSharpEditor.Text=( (ABCView)HostSurface.DesignerHost.RootComponent ).DataField.STViewCode;
                }
                else
                    SourceCSharpEditor.Text=GenerateScreenSourceCode( "ABCTemp" );

                ( (ABCView)HostSurface.DesignerHost.RootComponent ).SaveToXML(  @"temp.xml" );
                StreamReader reader=new StreamReader( @"temp.xml" );
          
                String strTemp=reader.ReadToEnd();
                reader.Close();
                SourceXMLEditor.IsReadOnly=false;
                SourceXMLEditor.Text=strTemp.Clone().ToString();
             
                //String strCodeDom=( (CodeDomHostLoader)this.HostSurface.Loader ).GenerateCodeDOM( strSOurceCOde );

                //StreamWriter writer=new StreamWriter( @"D:\bbb.cs" );
                //writer.Write( strCodeDom );
                //writer.Flush();
                //writer.Close();
                //     ( (CodeDomHostLoader)HostSurface.Loader ).SerializationCodeCompileUnit(); 
                #endregion


                SourceCSharpEditor.IsReadOnly=!( (ABCView)HostSurface.DesignerHost.RootComponent ).IsUseSourceCode;
                SourceXMLEditor.IsReadOnly=true;

            }
            else if ( e.Page==tabPreview )
            {
                ShowPreview();
            }
        }

        public String GenerateScreenSourceCode ( String strViewNo )
        {
            StreamReader reader=new StreamReader( @"Config\ScreenTemp.abc" );
            String strSource=reader.ReadToEnd();
            reader.Close();

            return strSource.Replace( "[ViewNo]" , strViewNo );
        }

        #region Preview

        public void ShowPreview ( )
        {
            if ( HostSurface==null||DesignerHost==null||DesignerHost.RootComponent==null||DesignerHost.RootComponent is ABCView==false )
                return;

            Cursor.Current=Cursors.WaitCursor;

            ABCView currentView=(ABCView)DesignerHost.RootComponent;
            currentView.SaveToXML( "temp.xml" );

            ABCControls.ABCView view=new ABCView();
            view.Load( "temp.xml" , ViewMode.Test );

            tabPreview.SuspendLayout();

            foreach ( Control ctrl in tabPreview.Controls )
                ctrl.Dispose();

            tabPreview.Controls.Clear();
            view.Parent=tabPreview;
            //   view.Dock=DockStyle.Fill;
            view.AutoScroll=true;

            tabPreview.ResumeLayout( false );

            Cursor.Current=Cursors.Default;

        }
        #endregion
    }
}
