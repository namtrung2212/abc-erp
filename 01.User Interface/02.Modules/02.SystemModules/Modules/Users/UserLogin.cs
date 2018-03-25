using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCScreen
{
 
    public partial class UserLogin : DevExpress.XtraEditors.XtraForm
    {
        LoginType loginType;
        public UserLogin (LoginType type )
        {
            loginType=type;

            InitializeComponent();
      
            this.StartPosition=FormStartPosition.CenterScreen;
            this.ShowInTaskbar=true;
            this.TopMost=true;
            this.btnChangePass.Click+=new EventHandler( btnChangePass_Click );
            this.btnLogin.Click+=new EventHandler( btnLogin_Click );
            this.btnCancelNewPass.Click+=new EventHandler( btnCancelNewPass_Click );
            this.btnSaveNewPass.Click+=new EventHandler( btnSaveNewPass_Click );

            this.cmbDatabase.Properties.Items.AddRange( DataQueryProvider.CompanyCollection.Keys );
            foreach ( DBConnectionController connection in DataQueryProvider.CompanyCollection.Values )
                if ( connection.Connection.IsDefault )
                    this.cmbDatabase.Text=connection.CompanyName;
            this.Activated+=new EventHandler( UserLogin_Activated );

            this.KeyPreview=true;
            this.KeyUp+=new KeyEventHandler( UserLogin_KeyUp );
            this.Shown+=new EventHandler( UserLogin_Shown );
       
        }

        void UserLogin_Shown ( object sender , EventArgs e )
        {
            this.txtUserName.Focus();
        }

        void UserLogin_KeyUp ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Enter )
            {
                SendKeys.Send( "{TAB}" );
                Application.DoEvents();
                if ( btnLogin.Focused )
                    ABCUserManager.Login( loginType , this.cmbDatabase.Text , txtUserName.Text , txtPassword.Text );
            }
        }

        void UserLogin_Activated ( object sender , EventArgs e )
        {
            this.txtUserName.Focus();
        }

        void btnSaveNewPass_Click ( object sender , EventArgs e )
        {
            if ( this.txtNewPass.Text!=this.txtNewPass2.Text )
            {
                ABCHelper.ABCMessageBox.Show( "Mật khẩu mới không khớp" , "Đổi mật khẩu" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return;
            }
            if ( ABCUserManager.ChangePassword( this.cmbDatabase.Text , txtUserName.Text , txtOldPass.Text , txtNewPass.Text ) )
            {
                this.panelLogin.Visible=true;
                this.panelChangePass.Visible=false;
                this.txtOldPass.Text=String.Empty;
                this.txtNewPass.Text=String.Empty;
                this.txtNewPass2.Text=String.Empty;
            }
        }

        void btnCancelNewPass_Click ( object sender , EventArgs e )
        {
            this.panelLogin.Visible=true;
            this.panelChangePass.Visible=false;
            this.txtOldPass.Text=String.Empty;
            this.txtNewPass.Text=String.Empty;
            this.txtNewPass2.Text=String.Empty;
        }

        void btnChangePass_Click ( object sender , EventArgs e )
        {
            this.panelChangePass.Visible=true;
            this.panelLogin.Visible=false;
        }

        void btnLogin_Click ( object sender , EventArgs e )
        {
            ABCUserManager.Login( loginType , this.cmbDatabase.Text , txtUserName.Text ,txtPassword.Text);
        }
    }
}