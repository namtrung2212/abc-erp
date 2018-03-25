namespace ABCControls
{
    partial class ABCObjectInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ABCObjectInformation));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnRunLink = new System.Windows.Forms.PictureBox();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.txtObjectNo = new DevExpress.XtraEditors.LabelControl();
            this.txtObjectType = new DevExpress.XtraEditors.LabelControl();
            this.txtEditCount = new DevExpress.XtraEditors.LabelControl();
            this.txtUpdateUser = new DevExpress.XtraEditors.LabelControl();
            this.txtUpdateTime = new DevExpress.XtraEditors.LabelControl();
            this.txtCreateUser = new DevExpress.XtraEditors.LabelControl();
            this.txtCreateTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRunLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 27);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(667, 337);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Image = ((System.Drawing.Image)(resources.GetObject("xtraTabPage1.Image")));
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(661, 306);
            this.xtraTabPage1.Text = "      Trao đổi      ";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Image = ((System.Drawing.Image)(resources.GetObject("xtraTabPage2.Image")));
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(661, 306);
            this.xtraTabPage2.Text = "      Lịch sử      ";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnRunLink);
            this.panelControl1.Controls.Add(this.labelControl13);
            this.panelControl1.Controls.Add(this.txtObjectNo);
            this.panelControl1.Controls.Add(this.txtObjectType);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(667, 27);
            this.panelControl1.TabIndex = 1;
            // 
            // btnRunLink
            // 
            this.btnRunLink.Image = ((System.Drawing.Image)(resources.GetObject("btnRunLink.Image")));
            this.btnRunLink.Location = new System.Drawing.Point(5, 4);
            this.btnRunLink.Name = "btnRunLink";
            this.btnRunLink.Size = new System.Drawing.Size(19, 19);
            this.btnRunLink.TabIndex = 20;
            this.btnRunLink.TabStop = false;
            this.btnRunLink.Click += new System.EventHandler(this.btnRunLink_Click);
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(205, 6);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(4, 13);
            this.labelControl13.TabIndex = 16;
            this.labelControl13.Text = ":";
            // 
            // txtObjectNo
            // 
            this.txtObjectNo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtObjectNo.Location = new System.Drawing.Point(225, 6);
            this.txtObjectNo.Name = "txtObjectNo";
            this.txtObjectNo.Size = new System.Drawing.Size(113, 13);
            this.txtObjectNo.TabIndex = 11;
            this.txtObjectNo.Text = "________________";
            // 
            // txtObjectType
            // 
            this.txtObjectType.Location = new System.Drawing.Point(34, 6);
            this.txtObjectType.Name = "txtObjectType";
            this.txtObjectType.Size = new System.Drawing.Size(145, 13);
            this.txtObjectType.TabIndex = 10;
            this.txtObjectType.Text = "________________________";
            // 
            // txtEditCount
            // 
            this.txtEditCount.Location = new System.Drawing.Point(588, 26);
            this.txtEditCount.Name = "txtEditCount";
            this.txtEditCount.Size = new System.Drawing.Size(66, 13);
            this.txtEditCount.TabIndex = 15;
            this.txtEditCount.Text = "                      ";
            // 
            // txtUpdateUser
            // 
            this.txtUpdateUser.Location = new System.Drawing.Point(331, 26);
            this.txtUpdateUser.Name = "txtUpdateUser";
            this.txtUpdateUser.Size = new System.Drawing.Size(96, 13);
            this.txtUpdateUser.TabIndex = 14;
            this.txtUpdateUser.Text = "                                ";
            // 
            // txtUpdateTime
            // 
            this.txtUpdateTime.Location = new System.Drawing.Point(327, 5);
            this.txtUpdateTime.Name = "txtUpdateTime";
            this.txtUpdateTime.Size = new System.Drawing.Size(96, 13);
            this.txtUpdateTime.TabIndex = 13;
            this.txtUpdateTime.Text = "                                ";
            // 
            // txtCreateUser
            // 
            this.txtCreateUser.Location = new System.Drawing.Point(65, 26);
            this.txtCreateUser.Name = "txtCreateUser";
            this.txtCreateUser.Size = new System.Drawing.Size(96, 13);
            this.txtCreateUser.TabIndex = 12;
            this.txtCreateUser.Text = "                                ";
            // 
            // txtCreateTime
            // 
            this.txtCreateTime.Location = new System.Drawing.Point(65, 5);
            this.txtCreateTime.Name = "txtCreateTime";
            this.txtCreateTime.Size = new System.Drawing.Size(96, 13);
            this.txtCreateTime.TabIndex = 5;
            this.txtCreateTime.Text = "                                ";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(507, 26);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(81, 13);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "Số lần thay đổi : ";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(245, 26);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(86, 13);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Người cập nhật  : ";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(245, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(82, 13);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Cập nhật cuối    :";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 26);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Người tạo :";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Ngày tạo  :";
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.txtCreateTime);
            this.panelControl2.Controls.Add(this.txtCreateUser);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.txtEditCount);
            this.panelControl2.Controls.Add(this.txtUpdateTime);
            this.panelControl2.Controls.Add(this.txtUpdateUser);
            this.panelControl2.Controls.Add(this.labelControl5);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 364);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(667, 45);
            this.panelControl2.TabIndex = 2;
            // 
            // ABCObjectInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 409);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ABCObjectInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRunLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl txtObjectNo;
        private DevExpress.XtraEditors.LabelControl txtObjectType;
        private DevExpress.XtraEditors.LabelControl txtCreateTime;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl txtEditCount;
        private DevExpress.XtraEditors.LabelControl txtUpdateUser;
        private DevExpress.XtraEditors.LabelControl txtUpdateTime;
        private DevExpress.XtraEditors.LabelControl txtCreateUser;
        private System.Windows.Forms.PictureBox btnRunLink;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}