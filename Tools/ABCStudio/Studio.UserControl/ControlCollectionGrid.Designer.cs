namespace ABCStudio
{
    partial class ControlCollectionGrid
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
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 481 , 276 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colType} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel=false;
            // 
            // colName
            // 
            this.colName.Caption="Name";
            this.colName.FieldName="Name";
            this.colName.Name="colName";
            this.colName.Visible=true;
            this.colName.VisibleIndex=0;
            // 
            // colType
            // 
            this.colType.Caption="Type";
            this.colType.FieldName="Type";
            this.colType.Name="colType";
            this.colType.Visible=true;
            this.colType.VisibleIndex=1;
            // 
            // ControlCollectionGrid
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Name="ControlCollectionGrid";
            this.Size=new System.Drawing.Size( 481 , 276 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
    }
}
