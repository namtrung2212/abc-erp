namespace ABCControls
{
    partial class ABCPivotGridControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.InnerGrid=new DevExpress.XtraPivotGrid.PivotGridControl();
          
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.InnerGrid ) ).BeginInit();
            this.SuspendLayout();
            // 
            // gridCtrl
            // 
            this.InnerGrid.Dock=System.Windows.Forms.DockStyle.Fill;
            this.InnerGrid.Location=new System.Drawing.Point( 0 , 0 );
            this.InnerGrid.Name="gridCtrl";
            this.InnerGrid.Size=new System.Drawing.Size( 293 , 172 );
            this.InnerGrid.TabIndex=0;

            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal=false;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 75 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.InnerGrid );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.PanelVisibility=DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitContainerControl1.Size=new System.Drawing.Size( 690 , 395 );
            this.splitContainerControl1.SplitterPosition=150;
            this.splitContainerControl1.TabIndex=5;
            this.splitContainerControl1.Text="splitContainerControl1";
         
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainerControl1 );
            this.Name="DefaultView";
            this.Size=new System.Drawing.Size( 293 , 172 );

            ( (System.ComponentModel.ISupportInitialize)( this.InnerGrid ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraPivotGrid.PivotGridControl InnerGrid;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}
