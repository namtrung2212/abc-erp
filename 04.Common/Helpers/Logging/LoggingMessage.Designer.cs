namespace ABCDataLib.Utilities
{
    partial class LoggingMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggingMessage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkRecentMessage = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.gridCtrlMessage = new DevExpress.XtraGrid.GridControl();
            this.gridViewMessages = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colMdl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAction = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lnkRecentMessage);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 197);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 33);
            this.panel1.TabIndex = 1;
            // 
            // lnkRecentMessage
            // 
            this.lnkRecentMessage.AutoSize = true;
            this.lnkRecentMessage.Location = new System.Drawing.Point(12, 11);
            this.lnkRecentMessage.Name = "lnkRecentMessage";
            this.lnkRecentMessage.Size = new System.Drawing.Size(108, 13);
            this.lnkRecentMessage.TabIndex = 1;
            this.lnkRecentMessage.TabStop = true;
            this.lnkRecentMessage.Text = ">> Recent Messages";
            this.lnkRecentMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RecentMessage_LinkClicked);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(578, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridCtrlMessage
            // 
            this.gridCtrlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrlMessage.Location = new System.Drawing.Point(0, 0);
            this.gridCtrlMessage.MainView = this.gridViewMessages;
            this.gridCtrlMessage.Name = "gridCtrlMessage";
            this.gridCtrlMessage.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.gridCtrlMessage.Size = new System.Drawing.Size(656, 197);
            this.gridCtrlMessage.TabIndex = 2;
            this.gridCtrlMessage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMessages});
            // 
            // gridViewMessages
            // 
            this.gridViewMessages.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.Empty.BackColor2 = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.Empty.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.EvenRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.EvenRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(133)))), ((int)(((byte)(195)))));
            this.gridViewMessages.Appearance.FixedLine.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(109)))), ((int)(((byte)(189)))));
            this.gridViewMessages.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(139)))), ((int)(((byte)(206)))));
            this.gridViewMessages.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.GroupButton.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(216)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.GroupRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.GroupRow.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gridViewMessages.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(170)))), ((int)(((byte)(225)))));
            this.gridViewMessages.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(155)))), ((int)(((byte)(215)))));
            this.gridViewMessages.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.OddRow.Options.UseBorderColor = true;
            this.gridViewMessages.Appearance.OddRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gridViewMessages.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(155)))), ((int)(((byte)(215)))));
            this.gridViewMessages.Appearance.Preview.Options.UseFont = true;
            this.gridViewMessages.Appearance.Preview.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gridViewMessages.Appearance.Row.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.Row.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gridViewMessages.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(155)))), ((int)(((byte)(215)))));
            this.gridViewMessages.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gridViewMessages.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gridViewMessages.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewMessages.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gridViewMessages.Appearance.VertLine.Options.UseBackColor = true;
            this.gridViewMessages.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDate,
            this.colMdl,
            this.colAction,
            this.colDesc,
            this.colStatus,
            this.colUser});
            this.gridViewMessages.GridControl = this.gridCtrlMessage;
            this.gridViewMessages.Name = "gridViewMessages";
            this.gridViewMessages.OptionsBehavior.Editable = false;
            this.gridViewMessages.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewMessages.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewMessages.OptionsView.ShowAutoFilterRow = true;
            this.gridViewMessages.OptionsView.ShowGroupPanel = false;
            // 
            // colDate
            // 
            this.colDate.Caption = "Date";
            this.colDate.ColumnEdit = this.repositoryItemTextEdit1;
            this.colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colDate.FieldName = "GELogMsgDate";
            this.colDate.Name = "colDate";
            this.colDate.Visible = true;
            this.colDate.VisibleIndex = 0;
            this.colDate.Width = 157;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // colMdl
            // 
            this.colMdl.Caption = "Module";
            this.colMdl.FieldName = "GELogMsgMdl";
            this.colMdl.Name = "colMdl";
            // 
            // colAction
            // 
            this.colAction.Caption = "Action";
            this.colAction.FieldName = "GELogMsgAction";
            this.colAction.Name = "colAction";
            this.colAction.Visible = true;
            this.colAction.VisibleIndex = 1;
            this.colAction.Width = 126;
            // 
            // colDesc
            // 
            this.colDesc.Caption = "Desc";
            this.colDesc.FieldName = "GELogMsgDesc";
            this.colDesc.Name = "colDesc";
            this.colDesc.Visible = true;
            this.colDesc.VisibleIndex = 2;
            this.colDesc.Width = 352;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "GELogMsgStatus";
            this.colStatus.Name = "colStatus";
            // 
            // colUser
            // 
            this.colUser.Caption = "User";
            this.colUser.FieldName = "GELogMsgUser";
            this.colUser.Name = "colUser";
            // 
            // LoggingMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 230);
            this.Controls.Add(this.gridCtrlMessage);
            this.Controls.Add(this.panel1);
            this.Name = "LoggingMessage";
            this.Text = "Logging";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel lnkRecentMessage;
        private DevExpress.XtraGrid.GridControl gridCtrlMessage;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMessages;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colMdl;
        private DevExpress.XtraGrid.Columns.GridColumn colAction;
        private DevExpress.XtraGrid.Columns.GridColumn colDesc;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colUser;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;

    }
}