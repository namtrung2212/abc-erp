namespace ABCControls
{
    partial class ABCLogging
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
            this.colTime=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmployee=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAction=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndex=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemMemoEdit1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1} );
            this.gridControl1.Size=new System.Drawing.Size( 496 , 266 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Empty.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 220 ) ) ) ) , ( (int)( ( (byte)( 230 ) ) ) ) , ( (int)( ( (byte)( 244 ) ) ) ) );
            this.gridView1.Appearance.Empty.Options.UseBackColor=true;
            this.gridView1.Appearance.HorzLine.BackColor=System.Drawing.Color.White;
            this.gridView1.Appearance.HorzLine.Options.UseBackColor=true;
            this.gridView1.Appearance.Row.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 220 ) ) ) ) , ( (int)( ( (byte)( 230 ) ) ) ) , ( (int)( ( (byte)( 244 ) ) ) ) );
            this.gridView1.Appearance.Row.Options.UseBackColor=true;
            this.gridView1.Appearance.VertLine.BackColor=System.Drawing.Color.Transparent;
            this.gridView1.Appearance.VertLine.Options.UseBackColor=true;
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTime,
            this.colEmployee,
            this.colAction,
            this.colIndex} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowIndicator=false;
            // 
            // colTime
            // 
            this.colTime.Caption="Thời gian";
            this.colTime.FieldName="Time";
            this.colTime.DisplayFormat.FormatString="{0:dd/MM/yyyy HH:mm}";
            this.colTime.DisplayFormat.FormatType=DevExpress.Utils.FormatType.DateTime;
            this.colTime.AppearanceCell.Font=new System.Drawing.Font( "Tahoma" , 7F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.colTime.AppearanceCell.ForeColor=System.Drawing.Color.Gray;
            this.colTime.AppearanceCell.Options.UseFont=true;
            this.colTime.AppearanceCell.Options.UseForeColor=true;
            this.colTime.MaxWidth=80;
            this.colTime.MinWidth=80;
            this.colTime.Name="colTime";
            this.colTime.Visible=true;
            this.colTime.VisibleIndex=0;
            this.colTime.Width=80;
            // 
            // colEmployee
            // 
            this.colEmployee.Caption="Nhân viên";
            this.colEmployee.FieldName="ActionEmployee";
            this.colEmployee.Name="colEmployee";
            this.colEmployee.Visible=true;
            this.colEmployee.VisibleIndex=1;
            this.colEmployee.Width=200;
            // 
            // colAction
            // 
            this.colAction.Caption="Hành vi";
            this.colAction.FieldName="Action";
            this.colAction.MaxWidth=100;
            this.colAction.MinWidth=100;
            this.colAction.Name="colAction";
            this.colAction.Visible=true;
            this.colAction.VisibleIndex=2;
            this.colAction.Width=100;
            // 
            // colIndex
            // 
            this.colIndex.Caption="Thứ tự";
            this.colIndex.FieldName="ActionIndex";
            this.colIndex.MaxWidth=70;
            this.colIndex.MinWidth=40;
            this.colIndex.Name="colIndex";
            this.colIndex.Visible=true;
            this.colIndex.VisibleIndex=3;
            this.colIndex.Width=70;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name="repositoryItemMemoEdit1";
            // 
            // ABCLogging
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Name="ABCLogging";
            this.Size=new System.Drawing.Size( 496 , 266 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemMemoEdit1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn colAction;
        private DevExpress.XtraGrid.Columns.GridColumn colIndex;
    }
}
