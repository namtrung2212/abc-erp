namespace ABCScreen
{
    partial class UserLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLogin));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.cmbDatabase = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnChangePass = new DevExpress.XtraEditors.SimpleButton();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.panelLogin = new DevExpress.XtraEditors.PanelControl();
            this.panelChangePass = new DevExpress.XtraEditors.PanelControl();
            this.txtNewPass2 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancelNewPass = new DevExpress.XtraEditors.SimpleButton();
            this.txtNewPass = new DevExpress.XtraEditors.TextEdit();
            this.btnSaveNewPass = new DevExpress.XtraEditors.SimpleButton();
            this.txtOldPass = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogin)).BeginInit();
            this.panelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChangePass)).BeginInit();
            this.panelChangePass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPass2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPass.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(133, 97);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(226, 37);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(201, 20);
            this.txtUserName.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(73, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(201, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.Location = new System.Drawing.Point(226, 11);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDatabase.Size = new System.Drawing.Size(201, 20);
            this.cmbDatabase.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(157, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(62, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Cơ sở dữ liệu";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(157, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 13);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Người dùng";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 7);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(44, 13);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Mật khẩu";
            // 
            // btnChangePass
            // 
            this.btnChangePass.Location = new System.Drawing.Point(199, 27);
            this.btnChangePass.Name = "btnChangePass";
            this.btnChangePass.Size = new System.Drawing.Size(75, 25);
            this.btnChangePass.TabIndex = 5;
            this.btnChangePass.Text = "&Đổi mật khẩu";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(118, 27);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "&Đăng nhập";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(9, 156);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(136, 13);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "Phát triển bởi ABC Solutions ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(2, 172);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(151, 14);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.abcsolutions.com.vn";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(279, 173);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(153, 14);
            this.linkLabel2.TabIndex = 0;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "namtrung2212@gmail.com";
            // 
            // panelLogin
            // 
            this.panelLogin.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.txtPassword);
            this.panelLogin.Controls.Add(this.btnChangePass);
            this.panelLogin.Controls.Add(this.labelControl3);
            this.panelLogin.Location = new System.Drawing.Point(153, 60);
            this.panelLogin.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(280, 66);
            this.panelLogin.TabIndex = 0;
            // 
            // panelChangePass
            // 
            this.panelChangePass.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelChangePass.Controls.Add(this.txtNewPass2);
            this.panelChangePass.Controls.Add(this.labelControl6);
            this.panelChangePass.Controls.Add(this.labelControl8);
            this.panelChangePass.Controls.Add(this.btnCancelNewPass);
            this.panelChangePass.Controls.Add(this.txtNewPass);
            this.panelChangePass.Controls.Add(this.btnSaveNewPass);
            this.panelChangePass.Controls.Add(this.txtOldPass);
            this.panelChangePass.Controls.Add(this.labelControl7);
            this.panelChangePass.Location = new System.Drawing.Point(153, 60);
            this.panelChangePass.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelChangePass.Name = "panelChangePass";
            this.panelChangePass.Size = new System.Drawing.Size(279, 110);
            this.panelChangePass.TabIndex = 13;
            this.panelChangePass.Visible = false;
            // 
            // txtNewPass2
            // 
            this.txtNewPass2.Location = new System.Drawing.Point(73, 51);
            this.txtNewPass2.Name = "txtNewPass2";
            this.txtNewPass2.Properties.PasswordChar = '*';
            this.txtNewPass2.Size = new System.Drawing.Size(201, 20);
            this.txtNewPass2.TabIndex = 9;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(4, 7);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(58, 13);
            this.labelControl6.TabIndex = 7;
            this.labelControl6.Text = "Mật khẩu cũ";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(4, 54);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(38, 13);
            this.labelControl8.TabIndex = 12;
            this.labelControl8.Text = "Nhập lại";
            // 
            // btnCancelNewPass
            // 
            this.btnCancelNewPass.Location = new System.Drawing.Point(199, 75);
            this.btnCancelNewPass.Name = "btnCancelNewPass";
            this.btnCancelNewPass.Size = new System.Drawing.Size(75, 25);
            this.btnCancelNewPass.TabIndex = 11;
            this.btnCancelNewPass.Text = "&Bỏ qua";
            // 
            // txtNewPass
            // 
            this.txtNewPass.Location = new System.Drawing.Point(73, 27);
            this.txtNewPass.Name = "txtNewPass";
            this.txtNewPass.Properties.PasswordChar = '*';
            this.txtNewPass.Size = new System.Drawing.Size(201, 20);
            this.txtNewPass.TabIndex = 8;
            // 
            // btnSaveNewPass
            // 
            this.btnSaveNewPass.Location = new System.Drawing.Point(114, 75);
            this.btnSaveNewPass.Name = "btnSaveNewPass";
            this.btnSaveNewPass.Size = new System.Drawing.Size(79, 25);
            this.btnSaveNewPass.TabIndex = 10;
            this.btnSaveNewPass.Text = "&Đổi mật khẩu";
            // 
            // txtOldPass
            // 
            this.txtOldPass.Location = new System.Drawing.Point(73, 3);
            this.txtOldPass.Name = "txtOldPass";
            this.txtOldPass.Properties.PasswordChar = '*';
            this.txtOldPass.Size = new System.Drawing.Size(201, 20);
            this.txtOldPass.TabIndex = 6;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(4, 31);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(63, 13);
            this.labelControl7.TabIndex = 0;
            this.labelControl7.Text = "Mật khẩu mới";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl4.Location = new System.Drawing.Point(7, 134);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(152, 14);
            this.labelControl4.TabIndex = 28;
            this.labelControl4.Text = "Every Business Processes";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl9.Location = new System.Drawing.Point(7, 114);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(70, 19);
            this.labelControl9.TabIndex = 27;
            this.labelControl9.Text = "Simplify ";
            // 
            // UserLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 190);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.panelChangePass);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserLogin";
            this.Opacity = 0.95D;
            this.Text = "Đăng nhập vào Hệ Thống ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogin)).EndInit();
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChangePass)).EndInit();
            this.panelChangePass.ResumeLayout(false);
            this.panelChangePass.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPass2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPass.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDatabase;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnChangePass;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private DevExpress.XtraEditors.PanelControl panelLogin;
        private DevExpress.XtraEditors.PanelControl panelChangePass;
        private DevExpress.XtraEditors.TextEdit txtNewPass2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SimpleButton btnCancelNewPass;
        private DevExpress.XtraEditors.TextEdit txtNewPass;
        private DevExpress.XtraEditors.SimpleButton btnSaveNewPass;
        private DevExpress.XtraEditors.TextEdit txtOldPass;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl9;
    }
}