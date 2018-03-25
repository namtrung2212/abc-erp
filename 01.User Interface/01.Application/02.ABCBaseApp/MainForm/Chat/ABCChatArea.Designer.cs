using ABCControls;
namespace ABCApp
{
    partial class ABCChatArea
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ABCChatArea));
            this.btnClose = new ABCControls.ABCSimpleButton();
            this.titleArea = new DevExpress.XtraEditors.PanelControl();
            this.lnkAllMessages = new System.Windows.Forms.LinkLabel();
            this.txtUser = new DevExpress.XtraEditors.LabelControl();
            this.picOnOff = new System.Windows.Forms.PictureBox();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelChatContent = new DevExpress.XtraEditors.PanelControl();
            this.ChatAreaContent = new DevExpress.XtraEditors.MemoEdit();
            this.gridContent = new DevExpress.XtraGrid.GridControl();
            this.gridContentView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEmployee = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colChatContent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.titleArea)).BeginInit();
            this.titleArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOnOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelChatContent)).BeginInit();
            this.panelChatContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChatAreaContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridContentView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseBorderColor = true;
            this.btnClose.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnClose.ButtonType = ABCControls.ABCSimpleButton.ABCButtonType.None;
            this.btnClose.Comment = null;
            this.btnClose.DataSource = null;
            this.btnClose.FieldGroup = null;
            this.btnClose.IconType = ABCControls.ABCSimpleButton.ABCIconType.None;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageIndex = 0;
            this.btnClose.IsVisible = true;
            this.btnClose.Location = new System.Drawing.Point(378, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.OwnerView = null;
            this.btnClose.Size = new System.Drawing.Size(25, 18);
            this.btnClose.TabIndex = 3;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // titleArea
            // 
            this.titleArea.Appearance.BackColor = System.Drawing.Color.SteelBlue;
            this.titleArea.Appearance.Options.UseBackColor = true;
            this.titleArea.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.titleArea.Controls.Add(this.lnkAllMessages);
            this.titleArea.Controls.Add(this.txtUser);
            this.titleArea.Controls.Add(this.picOnOff);
            this.titleArea.Controls.Add(this.btnClose);
            this.titleArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleArea.Location = new System.Drawing.Point(0, 0);
            this.titleArea.Name = "titleArea";
            this.titleArea.Size = new System.Drawing.Size(399, 18);
            this.titleArea.TabIndex = 5;
            this.titleArea.DoubleClick += new System.EventHandler(this.titleArea_DoubleClick);
            // 
            // lnkAllMessages
            // 
            this.lnkAllMessages.ActiveLinkColor = System.Drawing.Color.NavajoWhite;
            this.lnkAllMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAllMessages.AutoSize = true;
            this.lnkAllMessages.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lnkAllMessages.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lnkAllMessages.Location = new System.Drawing.Point(291, 3);
            this.lnkAllMessages.Name = "lnkAllMessages";
            this.lnkAllMessages.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lnkAllMessages.Size = new System.Drawing.Size(79, 13);
            this.lnkAllMessages.TabIndex = 5;
            this.lnkAllMessages.TabStop = true;
            this.lnkAllMessages.Text = "Tất cả tin nhắn";
            this.lnkAllMessages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkAllMessages.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAllMessages_LinkClicked);
            // 
            // txtUser
            // 
            this.txtUser.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtUser.Appearance.ForeColor = System.Drawing.Color.White;
            this.txtUser.Location = new System.Drawing.Point(22, 2);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(70, 13);
            this.txtUser.TabIndex = 4;
            this.txtUser.Text = "Default User";
            // 
            // picOnOff
            // 
            this.picOnOff.BackColor = System.Drawing.Color.Transparent;
            this.picOnOff.Location = new System.Drawing.Point(3, 1);
            this.picOnOff.Name = "picOnOff";
            this.picOnOff.Size = new System.Drawing.Size(23, 18);
            this.picOnOff.TabIndex = 1;
            this.picOnOff.TabStop = false;
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.MaxItemId = 3;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(399, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 177);
            this.barDockControlBottom.Size = new System.Drawing.Size(399, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 177);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(399, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 177);
            // 
            // panelChatContent
            // 
            this.panelChatContent.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelChatContent.Controls.Add(this.ChatAreaContent);
            this.panelChatContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChatContent.Location = new System.Drawing.Point(0, 0);
            this.panelChatContent.Name = "panelChatContent";
            this.panelChatContent.Size = new System.Drawing.Size(399, 30);
            this.panelChatContent.TabIndex = 2;
            // 
            // ChatAreaContent
            // 
            this.ChatAreaContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatAreaContent.EditValue = "";
            this.ChatAreaContent.Location = new System.Drawing.Point(0, 0);
            this.ChatAreaContent.Name = "ChatAreaContent";
            this.ChatAreaContent.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Moccasin;
            this.ChatAreaContent.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ChatAreaContent.Size = new System.Drawing.Size(399, 30);
            this.ChatAreaContent.TabIndex = 1;
            // 
            // gridContent
            // 
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.MainView = this.gridContentView;
            this.gridContent.Name = "gridContent";
            this.gridContent.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.gridContent.Size = new System.Drawing.Size(399, 123);
            this.gridContent.TabIndex = 0;
            this.gridContent.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridContentView,
            this.gridView1});
            this.gridContent.Click += new System.EventHandler(this.gridContent_Click);
            // 
            // gridContentView
            // 
            this.gridContentView.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(244)))));
            this.gridContentView.Appearance.Empty.Options.UseBackColor = true;
            this.gridContentView.Appearance.HorzLine.BackColor = System.Drawing.Color.Transparent;
            this.gridContentView.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridContentView.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(244)))));
            this.gridContentView.Appearance.Row.Options.UseBackColor = true;
            this.gridContentView.Appearance.VertLine.BackColor = System.Drawing.Color.Transparent;
            this.gridContentView.Appearance.VertLine.Options.UseBackColor = true;
            this.gridContentView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEmployee,
            this.colChatContent});
            this.gridContentView.GridControl = this.gridContent;
            this.gridContentView.Name = "gridContentView";
            this.gridContentView.OptionsView.ShowColumnHeaders = false;
            this.gridContentView.OptionsView.ShowGroupPanel = false;
            this.gridContentView.OptionsView.ShowIndicator = false;
            // 
            // colEmployee
            // 
            this.colEmployee.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold);
            this.colEmployee.AppearanceCell.Options.UseFont = true;
            this.colEmployee.FieldName = "FromEmployee";
            this.colEmployee.Name = "colEmployee";
            this.colEmployee.Visible = true;
            this.colEmployee.VisibleIndex = 0;
            this.colEmployee.Width = 100;
            // 
            // colChatContent
            // 
            this.colChatContent.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colChatContent.FieldName = "ChatContent";
            this.colChatContent.Name = "colChatContent";
            this.colChatContent.Visible = true;
            this.colChatContent.VisibleIndex = 1;
            this.colChatContent.Width = 293;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridContent;
            this.gridView1.Name = "gridView1";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 18);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridContent);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelChatContent);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(399, 159);
            this.splitContainerControl1.SplitterPosition = 30;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // ABCChatArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.titleArea);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ABCChatArea";
            this.Size = new System.Drawing.Size(399, 177);
            ((System.ComponentModel.ISupportInitialize)(this.titleArea)).EndInit();
            this.titleArea.ResumeLayout(false);
            this.titleArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOnOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelChatContent)).EndInit();
            this.panelChatContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChatAreaContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridContentView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ABCSimpleButton btnClose;
        private DevExpress.XtraEditors.PanelControl titleArea;
        private DevExpress.XtraEditors.LabelControl txtUser;
        private System.Windows.Forms.PictureBox picOnOff;
        private System.Windows.Forms.LinkLabel lnkAllMessages;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridContent;
        private DevExpress.XtraGrid.Views.Grid.GridView gridContentView;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn colChatContent;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.PanelControl panelChatContent;
        public DevExpress.XtraEditors.MemoEdit ChatAreaContent;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}
