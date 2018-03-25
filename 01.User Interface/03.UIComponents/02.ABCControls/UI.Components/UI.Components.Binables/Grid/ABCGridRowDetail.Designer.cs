namespace ABCControls
{
    partial class ABCGridRowDetail
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
            this.VGrid=new DevExpress.XtraVerticalGrid.VGridControl();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            ( (System.ComponentModel.ISupportInitialize)( this.VGrid ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // vGridControl1
            // 
            this.VGrid.Dock=System.Windows.Forms.DockStyle.Fill;
            this.VGrid.Location=new System.Drawing.Point( 0 , 0 );
            this.VGrid.Name="vGridControl1";
            this.VGrid.Size=new System.Drawing.Size( 303 , 317 );
            this.VGrid.TabIndex=0;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 317 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 303 , 34 );
            this.panelControl1.TabIndex=1;
            // 
            // ABCGridRowDetail
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 303 , 351 );
            this.Controls.Add( this.VGrid );
            this.Controls.Add( this.panelControl1 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="ABCGridRowDetail";
            this.Text="ABCGridRowDetail";
            ( (System.ComponentModel.ISupportInitialize)( this.VGrid ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraVerticalGrid.VGridControl VGrid;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}