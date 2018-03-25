using ScintillaNET;

namespace ABCControls
{
    partial class SQLScriptEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing&&( components!=null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( SQLScriptEditorForm ) );
            this.panelControl2=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnNext=new DevExpress.XtraEditors.SimpleButton();
            this.SourceSQLEditor=new Scintilla();
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl2 ) ).BeginInit();
            this.panelControl2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.SourceSQLEditor ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add( this.btnCancel );
            this.panelControl2.Controls.Add( this.btnNext );
            this.panelControl2.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location=new System.Drawing.Point( 0 , 354 );
            this.panelControl2.Name="panelControl2";
            this.panelControl2.Size=new System.Drawing.Size( 598 , 33 );
            this.panelControl2.TabIndex=3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 519 , 3 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=1;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnNext
            // 
            this.btnNext.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnNext.Image=( (System.Drawing.Image)( resources.GetObject( "btnNext.Image" ) ) );
            this.btnNext.Location=new System.Drawing.Point( 441 , 3 );
            this.btnNext.Name="btnNext";
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=0;
            this.btnNext.Text="&Next";
            this.btnNext.Click+=new System.EventHandler( this.btnNext_Click );
            // 
            // SourceSQLEditor
            // 
            this.SourceSQLEditor.ConfigurationManager.Language="mssql";
            this.SourceSQLEditor.Dock=System.Windows.Forms.DockStyle.Fill;
            this.SourceSQLEditor.Indentation.ShowGuides=true;
            this.SourceSQLEditor.Indentation.SmartIndentType=SmartIndent.Simple;
            this.SourceSQLEditor.Location=new System.Drawing.Point( 0 , 0 );
            this.SourceSQLEditor.Margins.Margin0.Width=40;
            this.SourceSQLEditor.Margins.Margin2.Width=20;
            this.SourceSQLEditor.Name="SourceSQLEditor";
            this.SourceSQLEditor.Size=new System.Drawing.Size( 598 , 221 );
            this.SourceSQLEditor.Styles.BraceBad.FontName="Verdana";
            this.SourceSQLEditor.Styles.BraceLight.FontName="Verdana";
            this.SourceSQLEditor.Styles.ControlChar.FontName="Verdana";
            this.SourceSQLEditor.Styles.Default.FontName="Verdana";
            this.SourceSQLEditor.Styles.IndentGuide.FontName="Verdana";
            this.SourceSQLEditor.Styles.LastPredefined.FontName="Verdana";
            this.SourceSQLEditor.Styles.LineNumber.FontName="Verdana";
            this.SourceSQLEditor.Styles.Max.FontName="Verdana";
            this.SourceSQLEditor.TabIndex=1;
            this.SourceSQLEditor.Whitespace.Mode=WhitespaceMode.VisibleAfterIndent;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal=false;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.SourceSQLEditor );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.gridControl1 );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 598 , 354 );
            this.splitContainerControl1.SplitterPosition=128;
            this.splitContainerControl1.TabIndex=4;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 598 , 128 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowViewCaption=true;
            this.gridView1.ViewCaption="Query Result";
            // 
            // SQLScriptEditorForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 598 , 387 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl2 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="SQLScriptEditorForm";
            this.Text="SQL Script Editor";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl2 ) ).EndInit();
            this.panelControl2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.SourceSQLEditor ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private Scintilla SourceSQLEditor;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}