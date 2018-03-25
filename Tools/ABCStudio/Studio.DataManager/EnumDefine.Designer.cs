namespace ABCStudio
{
    partial class EnumDefineScreen
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( EnumDefineScreen ) );
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7=new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 661 , 259 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn6,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn7} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowViewCaption=true;
            this.gridView1.ViewCaption="Enum Configuration";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption="Enum Name";
            this.gridColumn1.FieldName="EnumName";
            this.gridColumn1.Name="gridColumn1";
            this.gridColumn1.Visible=true;
            this.gridColumn1.VisibleIndex=0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption="Enum Display (VN)";
            this.gridColumn2.FieldName="EnumDisplayVN";
            this.gridColumn2.Name="gridColumn2";
            this.gridColumn2.Visible=true;
            this.gridColumn2.VisibleIndex=1;
            this.gridColumn2.Width=96;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption="Enum Display (EN)";
            this.gridColumn6.FieldName="EnumDisplayEN";
            this.gridColumn6.Name="gridColumn6";
            this.gridColumn6.Visible=true;
            this.gridColumn6.VisibleIndex=2;
            this.gridColumn6.Width=96;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption="Item Name";
            this.gridColumn3.FieldName="ItemName";
            this.gridColumn3.Name="gridColumn3";
            this.gridColumn3.Visible=true;
            this.gridColumn3.VisibleIndex=3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption="Item Display (VN)";
            this.gridColumn5.FieldName="ItemDisplayVN";
            this.gridColumn5.Name="gridColumn5";
            this.gridColumn5.Visible=true;
            this.gridColumn5.VisibleIndex=4;
            this.gridColumn5.Width=92;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption="Item Display (EN)";
            this.gridColumn7.FieldName="ItemDisplayEN";
            this.gridColumn7.Name="gridColumn7";
            this.gridColumn7.Visible=true;
            this.gridColumn7.VisibleIndex=5;
            this.gridColumn7.Width=92;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 259 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 661 , 33 );
            this.panelControl1.TabIndex=2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 581 , 3 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=3;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnSave
            // 
            this.btnSave.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnSave.Image=( (System.Drawing.Image)( resources.GetObject( "btnSave.Image" ) ) );
            this.btnSave.Location=new System.Drawing.Point( 503 , 3 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // EnumDefineScreen
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="EnumDefineScreen";
            this.Size=new System.Drawing.Size( 661 , 292 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    }
}
