namespace ABCControls
{
    partial class ABCDateTimeSearch
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
            this.dateEdit1=new DevExpress.XtraEditors.DateEdit();
            this.layoutControlItem1=new DevExpress.XtraLayout.LayoutControlItem();
            this.dateEdit2=new DevExpress.XtraEditors.DateEdit();
            this.layoutControlItem2=new DevExpress.XtraLayout.LayoutControlItem();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControl1 ) ).BeginInit();
            this.layoutControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlGroup1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit1.Properties.VistaTimeProperties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit1.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlItem1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit2.Properties.VistaTimeProperties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit2.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlItem2 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add( this.dateEdit2 );
            this.layoutControl1.Controls.Add( this.dateEdit1 );
            this.layoutControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.layoutControl1.Margin=new System.Windows.Forms.Padding( 0 );
            this.layoutControl1.Name="layoutControl1";
            this.layoutControl1.Root=this.layoutControlGroup1;
            this.layoutControl1.Size=new System.Drawing.Size( 200 , 20 );
            this.layoutControl1.TabIndex=0;
            this.layoutControl1.Text="layoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText="layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders=DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible=false;
            this.layoutControlGroup1.Items.AddRange( new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2} );
            this.layoutControlGroup1.Location=new System.Drawing.Point( 0 , 0 );
            this.layoutControlGroup1.Name="layoutControlGroup1";
            this.layoutControlGroup1.Padding=new DevExpress.XtraLayout.Utils.Padding( 0 , 0 , 0 , 0 );
            this.layoutControlGroup1.Size=new System.Drawing.Size( 200 , 20 );
            this.layoutControlGroup1.Text="layoutControlGroup1";
            this.layoutControlGroup1.TextVisible=false;
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue=null;
            this.dateEdit1.Location=new System.Drawing.Point( 26 , 0 );
            this.dateEdit1.Margin=new System.Windows.Forms.Padding( 0 );
            this.dateEdit1.Name="dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.dateEdit1.Properties.VistaTimeProperties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.dateEdit1.Size=new System.Drawing.Size( 74 , 20 );
            this.dateEdit1.StyleController=this.layoutControl1;
            this.dateEdit1.TabIndex=4;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control=this.dateEdit1;
            this.layoutControlItem1.CustomizationFormText=" từ ";
            this.layoutControlItem1.Location=new System.Drawing.Point( 0 , 0 );
            this.layoutControlItem1.Name="layoutControlItem1";
            this.layoutControlItem1.Padding=new DevExpress.XtraLayout.Utils.Padding( 0 , 0 , 0 , 0 );
            this.layoutControlItem1.Size=new System.Drawing.Size( 93 , 20 );
            this.layoutControlItem1.Text=" từ ";
            this.layoutControlItem1.TextVisible=false;
            this.layoutControlItem1.TextSize=new System.Drawing.Size( 22 , 13 );
            // 
            // dateEdit2
            // 
            this.dateEdit2.EditValue=null;
            this.dateEdit2.Location=new System.Drawing.Point( 126 , 0 );
            this.dateEdit2.Name="dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.dateEdit2.Properties.VistaTimeProperties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.dateEdit2.Size=new System.Drawing.Size( 74 , 20 );
            this.dateEdit2.StyleController=this.layoutControl1;
            this.dateEdit2.TabIndex=5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control=this.dateEdit2;
            this.layoutControlItem2.CustomizationFormText=" đến ";
            this.layoutControlItem2.Location=new System.Drawing.Point( 100 , 0 );
            this.layoutControlItem2.Name="layoutControlItem2";
            this.layoutControlItem2.Padding=new DevExpress.XtraLayout.Utils.Padding( 0 , 0 , 0 , 0 );
            this.layoutControlItem2.Size=new System.Drawing.Size( 108 , 20 );
            this.layoutControlItem2.Text=" đến ";
            this.layoutControlItem2.TextSize=new System.Drawing.Size( 22 , 13 );
            // 
            // ABCDateTimeSearch
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.layoutControl1 );
            this.Name="ABCDateTimeSearch";
            this.Size=new System.Drawing.Size( 200 , 20 );
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControl1 ) ).EndInit();
            this.layoutControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlGroup1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit1.Properties.VistaTimeProperties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit1.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlItem1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit2.Properties.VistaTimeProperties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dateEdit2.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.layoutControlItem2 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit2;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;

    }
}
