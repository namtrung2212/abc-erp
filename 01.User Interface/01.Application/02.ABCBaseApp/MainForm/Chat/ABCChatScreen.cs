using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCScreen;

using ABCControls;
using ABCProvider;
using ABCProvider;
using System.Reflection;
using System.Reflection.Emit;
using DevExpress.XtraBars;
using ABCBusinessEntities;

namespace ABCApp
{
    public partial class ABCChatScreen : DevExpress.XtraEditors.XtraForm
    {
        public bool SoundOn=true;

        public Dictionary<String , ABCChatBox> ChatList=new Dictionary<string , ABCChatBox>();

        public ABCChatScreen ( )
        {
            InitializeComponent();
            this.StartPosition=FormStartPosition.CenterScreen;
            this.FormClosing+=new FormClosingEventHandler( ABCChatScreen_FormClosing );
            this.xtraTabControl1.SelectedPageChanged+=new DevExpress.XtraTab.TabPageChangedEventHandler( xtraTabControl1_SelectedPageChanged );
        }

        void xtraTabControl1_SelectedPageChanged ( object sender , DevExpress.XtraTab.TabPageChangedEventArgs e )
        {
            if ( e.Page!=null && e.Page.Controls.Count>0&&e.Page.Controls[0] is ABCChatBox )
            {
                ( e.Page.Controls[0] as ABCChatBox ).ChatArea.SetViewAll();
                ( e.Page.Controls[0] as ABCChatBox ).ChatArea.RefreshDataSource();
            }
        }

        void ABCChatScreen_FormClosing ( object sender , FormClosingEventArgs e )
        {
            this.Hide();
            e.Cancel=true;
        }

        private void ABCChatScreen_Load ( object sender , EventArgs e )
        {

        }

        public void AutoOpenChatBox ( )
        {
            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT FromUser FROM GEChatContents WHERE ToUser ='{0}' AND Viewed=0 GROUP BY FromUser" , ABCUserProvider.CurrentUserName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                    OpenChatBox( dr[0].ToString() );
            }

        }
        public void OpenChatBox ( String strFiendUser )
        {
            if ( new ADUsersController().GetObjectByNo( strFiendUser )==null )
                return;

            if ( ChatList.ContainsKey( strFiendUser )==false )
            {
                ABCChatBox chatbox=new ABCChatBox( this , strFiendUser );
                ChatList.Add( strFiendUser , chatbox );

                DevExpress.XtraTab.XtraTabPage page=new DevExpress.XtraTab.XtraTabPage();
                page.Text=chatbox.ChatArea.EmployeeName2;
                chatbox.Dock=DockStyle.Fill;
                page.Controls.Add( chatbox );
                xtraTabControl1.TabPages.Add( page );
           
                ActiveChatPanel( strFiendUser );
            }

        }
        public void CloseChatBox ( String strFiendUser )
        {
            if ( ChatList.ContainsKey( strFiendUser ) )
            {
                xtraTabControl1.TabPages.Remove( ChatList[strFiendUser].Parent as DevExpress.XtraTab.XtraTabPage );
                ChatList[strFiendUser].Dispose();
                ChatList.Remove( strFiendUser );

                if ( ChatList.Count<=0 )
                    Close();
            }
        }
        public void ActiveChatPanel ( String strFiendUser )
        {
            xtraTabControl1.SelectedTabPage=ChatList[strFiendUser].Parent as DevExpress.XtraTab.XtraTabPage;
            ChatList[strFiendUser].ChatArea.ChatAreaContent.Focus();
            ChatList[strFiendUser].ChatArea.RefreshDataSource();
            if ( ABCApp.ABCAppManager.Instance.MainForm!=null )
            {
                ABCScreenManager.Instance.ShowForm( this , false );
                Application.DoEvents();
                ChatList[strFiendUser].ChatArea.RefreshDataSource();
            }
        }
      
        System.Windows.Forms.Timer timer;
        public void StartTimer ( )
        {
            if ( timer==null )
            {
                timer=new Timer();
                timer.Interval=2000;
                timer.Tick+=new EventHandler( timer_Tick );
                timer.Start();
            }

        }

        void timer_Tick ( object sender , EventArgs e )
        {
            AutoOpenChatBox();
        }

    }
}