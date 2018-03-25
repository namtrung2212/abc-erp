namespace ABCControls
{
    partial class ABCBindingBaseEdit
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
            this.layoutControl1=new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1=new DevExpress.XtraLayout.LayoutControlGroup();
            this.LayoutItem=new DevExpress.XtraLayout.LayoutControlItem();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControl1 ) ).BeginInit();
            this.layoutControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlGroup1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.LayoutItem ) ).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AutoScroll=false;
            this.layoutControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.layoutControl1.Name="layoutControl1";
            this.layoutControl1.Root=this.layoutControlGroup1;
            this.layoutControl1.Size=new System.Drawing.Size( 229 , 20 );
            this.layoutControl1.TabIndex=0;
            this.layoutControl1.Text="layoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText="layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders=DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible=false;
            this.layoutControlGroup1.Items.AddRange( new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.LayoutItem} );
            this.layoutControlGroup1.Location=new System.Drawing.Point( 0 , 0 );
            this.layoutControlGroup1.Name="layoutControlGroup1";
            this.layoutControlGroup1.Padding=new DevExpress.XtraLayout.Utils.Padding( 0 , 0 , 0 , 0 );
            this.layoutControlGroup1.Size=new System.Drawing.Size( 229 , 20 );
            this.layoutControlGroup1.Text="layoutControlGroup1";
            this.layoutControlGroup1.TextVisible=false;
          
            // 
            // layoutControlItem1
            // 
            this.LayoutItem.CustomizationFormText="Sample Text String";
          //  this.layoutControlItem1.Image=global::ABCControls.Properties.Resources.InfoIcon;
            this.LayoutItem.ImageAlignment=System.Drawing.ContentAlignment.MiddleRight;
            this.LayoutItem.Padding=new DevExpress.XtraLayout.Utils.Padding( 0 );
            this.LayoutItem.Location=new System.Drawing.Point( 0 , 0 );
            this.LayoutItem.Name="layoutControlItem1";
            this.LayoutItem.Size=new System.Drawing.Size( 227 , 20 );
            this.LayoutItem.SizeConstraintsType=DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
            this.LayoutItem.Text="Sample Text String";
            this.LayoutItem.ControlAlignment=System.Drawing.ContentAlignment.MiddleRight;
          //  this.LayoutItem.TextAlignMode=DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.LayoutItem.TextAlignMode=DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LayoutItem.TextSize=new System.Drawing.Size( 100 , 16 );
            this.LayoutItem.TextToControlDistance=25;
            // 
            // ABCBindingControl
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.layoutControl1 );
            this.Name="ABCBindingControl";
            this.Size=new System.Drawing.Size( 229 , 20 );
            this.Margin=new System.Windows.Forms.Padding( 3 , 1 , 3 , 1);
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControl1 ) ).EndInit();
            this.layoutControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlGroup1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.LayoutItem ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        public DevExpress.XtraLayout.LayoutControlItem LayoutItem;
    }
}
