namespace ABCControls
{
    partial class ColumnChooserForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( ColumnChooserForm ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnNext=new DevExpress.XtraEditors.SimpleButton();
            this.ColumnListCtrl=new DevExpress.XtraEditors.CheckedListBoxControl();
            this.chkAllColumns=new DevExpress.XtraEditors.CheckEdit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnListCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chkAllColumns.Properties ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.chkAllColumns );
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnNext );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 253 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 417 , 33 );
            this.panelControl1.TabIndex=1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 338 , 3 );
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
            this.btnNext.Location=new System.Drawing.Point( 260 , 3 );
            this.btnNext.Name="btnNext";
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=0;
            this.btnNext.Text="&Next";
            this.btnNext.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // ColumnListCtrl
            // 
            this.ColumnListCtrl.CheckOnClick=true;
            this.ColumnListCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ColumnListCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.ColumnListCtrl.Margin=new System.Windows.Forms.Padding( 10 );
            this.ColumnListCtrl.MultiColumn=true;
            this.ColumnListCtrl.Name="ColumnListCtrl";
            this.ColumnListCtrl.SelectionMode=System.Windows.Forms.SelectionMode.MultiExtended;
            this.ColumnListCtrl.Size=new System.Drawing.Size( 417 , 253 );
            this.ColumnListCtrl.TabIndex=2;
            // 
            // chkAllColumns
            // 
            this.chkAllColumns.Location=new System.Drawing.Point( 9 , 7 );
            this.chkAllColumns.Name="chkAllColumns";
            this.chkAllColumns.Properties.Caption="All Columns";
            this.chkAllColumns.Size=new System.Drawing.Size( 75 , 19 );
            this.chkAllColumns.TabIndex=3;
            // 
            // ColumnChooserForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 417 , 286 );
            this.Controls.Add( this.ColumnListCtrl );
            this.Controls.Add( this.panelControl1 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="ColumnChooserForm";
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text="Choose ColumnName from ListBox below";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnListCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chkAllColumns.Properties ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private DevExpress.XtraEditors.CheckedListBoxControl ColumnListCtrl;
        private DevExpress.XtraEditors.CheckEdit chkAllColumns;
    }
}