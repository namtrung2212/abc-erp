namespace ABCApp
{
    partial class BeAssignedTaskList
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
            this.gridViewTasks = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCompleted = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridViewTasks
            // 
            this.gridViewTasks.Appearance.Empty.BackColor=ABCControls.ABCPresentHelper.GetSkinBackColor();
            this.gridViewTasks.Appearance.Empty.Options.UseBackColor = true;
            this.gridViewTasks.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridViewTasks.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridViewTasks.Appearance.Row.BackColor=ABCControls.ABCPresentHelper.GetSkinBackColor();
            this.gridViewTasks.Appearance.Row.Options.UseBackColor=true;
            this.gridViewTasks.Appearance.SelectedRow.BackColor=ABCControls.ABCPresentHelper.GetSkinBackColor();
            this.gridViewTasks.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.gridViewTasks.Appearance.SelectedRow.Options.UseFont=true;
            this.gridViewTasks.Appearance.SelectedRow.Options.UseBackColor=true;
            this.gridViewTasks.OptionsSelection.EnableAppearanceFocusedRow=false;
            this.gridViewTasks.Appearance.VertLine.BackColor = System.Drawing.Color.Transparent;
            this.gridViewTasks.Appearance.VertLine.Options.UseBackColor = true;
            this.gridViewTasks.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTitle,
            this.colEstStartTime,
            this.colEstEndTime,
            this.colCompleted});
            this.gridViewTasks.GridControl = this.gridControl1;
            this.gridViewTasks.Name = "gridViewTasks";
            this.gridViewTasks.OptionsBehavior.Editable = false;
            this.gridViewTasks.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTasks.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewTasks.OptionsView.ShowGroupPanel = false;
            this.gridViewTasks.OptionsView.ShowIndicator = false;
            // 
            // colTitle
            // 
            this.colTitle.Caption = "Nhiệm vụ";
            this.colTitle.FieldName = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.Visible = true;
            this.colTitle.VisibleIndex = 0;
            this.colTitle.Width = 267;
            // 
            // colEstStartTime
            // 
            this.colEstStartTime.Caption = "Bắt đầu";
            this.colEstStartTime.FieldName = "EstStartTime";
            this.colEstStartTime.MaxWidth = 50;
            this.colEstStartTime.MinWidth = 50;
            this.colEstStartTime.Name = "colEstStartTime";
            this.colEstStartTime.Width = 50;
            // 
            // colEstEndTime
            // 
            this.colEstEndTime.Caption = "Thời hạn";
            this.colEstEndTime.FieldName = "EstEndTime";
            this.colEstEndTime.MaxWidth = 50;
            this.colEstEndTime.MinWidth = 50;
            this.colEstEndTime.Name = "colEstEndTime";
            this.colEstEndTime.Visible = true;
            this.colEstEndTime.VisibleIndex = 1;
            this.colEstEndTime.Width = 50;
            // 
            // colCompleted
            // 
            this.colCompleted.Caption = "%";
            this.colCompleted.ColumnEdit = this.repositoryItemProgressBar1;
            this.colCompleted.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCompleted.FieldName = "PercentCompleted";
            this.colCompleted.MaxWidth = 35;
            this.colCompleted.MinWidth = 35;
            this.colCompleted.Name = "colCompleted";
            this.colCompleted.Visible = true;
            this.colCompleted.VisibleIndex = 2;
            this.colCompleted.Width = 35;
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            this.repositoryItemProgressBar1.ShowTitle = true;
            this.repositoryItemProgressBar1.Step = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridViewTasks;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemProgressBar1});
            this.gridControl1.Size = new System.Drawing.Size(772, 221);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTasks});
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.MaxItemId = 7;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(772, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 221);
            this.barDockControlBottom.Size = new System.Drawing.Size(772, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 221);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(772, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 221);
            // 
            // BeAssignedTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "BeAssignedTaskList";
            this.Size = new System.Drawing.Size(772, 221);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTasks;
        private DevExpress.XtraGrid.Columns.GridColumn colTitle;
        private DevExpress.XtraGrid.Columns.GridColumn colEstStartTime;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.Columns.GridColumn colEstEndTime;
        private DevExpress.XtraGrid.Columns.GridColumn colCompleted;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;

    }
}
