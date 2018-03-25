namespace ABCControls
{
    partial class FieldChooserEx
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
            this.components=new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( FieldChooserEx ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnNext=new DevExpress.XtraEditors.SimpleButton();
            this.treeList1=new DevExpress.XtraTreeList.TreeList();
            this.barManager1=new DevExpress.XtraBars.BarManager( this.components );
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.treeList1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnNext );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 247 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 452 , 33 );
            this.panelControl1.TabIndex=2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=ABCControls.ABCImageList.GetImage16x16( "Cancel" );
            this.btnCancel.Location=new System.Drawing.Point( 373 , 3 );
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
            this.btnNext.Image=ABCControls.ABCImageList.GetImage16x16( "Save" );
            this.btnNext.Location=new System.Drawing.Point( 295 , 3 );
            this.btnNext.Name="btnNext";
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=0;
            this.btnNext.Text="&Next";
            this.btnNext.Click+=new System.EventHandler( this.btnNext_Click );
            // 
            // treeList1
            // 
            this.treeList1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location=new System.Drawing.Point( 0 , 0 );
            this.treeList1.Name="treeList1";
            this.treeList1.Size=new System.Drawing.Size( 452 , 247 );
            this.treeList1.TabIndex=3;
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add( this.barDockControlTop );
            this.barManager1.DockControls.Add( this.barDockControlBottom );
            this.barManager1.DockControls.Add( this.barDockControlLeft );
            this.barManager1.DockControls.Add( this.barDockControlRight );
            this.barManager1.Form=this;
            this.barManager1.MaxItemId=0;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation=false;
            this.barDockControlTop.Dock=System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlTop.Size=new System.Drawing.Size( 452 , 0 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 280 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 452 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 280 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 452 , 0 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 280 );
            // 
            // FieldChooserEx
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 352, 280 );
            this.Controls.Add( this.treeList1 );
            this.Controls.Add( this.panelControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="FieldChooserEx";
            this.Text="Column Chooser";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.treeList1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}