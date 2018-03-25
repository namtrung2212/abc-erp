namespace ABCCommonForms
{
    partial class FilterBuilderForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( FilterBuilderForm ) );
            this.panelControl2=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnNext=new DevExpress.XtraEditors.SimpleButton();
            this.filterEditorControl1=new DevExpress.XtraFilterEditor.FilterEditorControl();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl2 ) ).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add( this.btnCancel );
            this.panelControl2.Controls.Add( this.btnNext );
            this.panelControl2.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location=new System.Drawing.Point( 0 , 207 );
            this.panelControl2.Name="panelControl2";
            this.panelControl2.Size=new System.Drawing.Size( 397 , 33 );
            this.panelControl2.TabIndex=2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 318 , 3 );
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
            this.btnNext.Location=new System.Drawing.Point( 240 , 3 );
            this.btnNext.Name="btnNext";
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=0;
            this.btnNext.Text="&Next";
            this.btnNext.Click+=new System.EventHandler( this.btnNext_Click );
            // 
            // filterEditorControl1
            // 
            this.filterEditorControl1.AppearanceEmptyValueColor=System.Drawing.Color.Empty;
            this.filterEditorControl1.AppearanceFieldNameColor=System.Drawing.Color.Empty;
            this.filterEditorControl1.AppearanceGroupOperatorColor=System.Drawing.Color.Empty;
            this.filterEditorControl1.AppearanceOperatorColor=System.Drawing.Color.Empty;
            this.filterEditorControl1.AppearanceValueColor=System.Drawing.Color.Empty;
            this.filterEditorControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.filterEditorControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.filterEditorControl1.Name="filterEditorControl1";
            this.filterEditorControl1.Size=new System.Drawing.Size( 397 , 207 );
            this.filterEditorControl1.TabIndex=3;
            this.filterEditorControl1.Text="filterEditorControl1";
            this.filterEditorControl1.UseMenuForOperandsAndOperators=false;
            this.filterEditorControl1.Click+=new System.EventHandler( this.filterEditorControl1_Click );
            // 
            // FilterBuilderForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 397 , 240 );
            this.Controls.Add( this.filterEditorControl1 );
            this.Controls.Add( this.panelControl2 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="FilterBuilderForm";
            this.Text="Filter Builder";
            this.Load+=new System.EventHandler( this.FilterBuilderForm_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl2 ) ).EndInit();
            this.panelControl2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private DevExpress.XtraFilterEditor.FilterEditorControl filterEditorControl1;
    }
}