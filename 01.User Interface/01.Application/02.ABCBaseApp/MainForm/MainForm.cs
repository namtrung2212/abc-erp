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
using System.Reflection;
using System.Reflection.Emit;
using DevExpress.XtraBars;
using ABCBusinessEntities;

using ABCProvider;

namespace ABCApp
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm ( )
        {
            InitializeComponent();

            ModulesExplorerTree.Initialize( this );
            
            InitMenu();

         //   InitializeSkins();

            InitChat();

            InitNotifiesList();

            InitMeetingsList();

            InitTasksList();

            InitHomepages();

            this.Shown+=new EventHandler( MainForm_Shown );
            this.Text="ABCERP - Hệ thống quản trị doanh nghiệp toàn diện - ABC Solutions - namtrung2212@gmail.com";
        }

        void MainForm_Shown ( object sender , EventArgs e )
        {
            ABCScreen.SplashUtils.CloseSplash();
            this.Activate();
            this.ChatScreen.StartTimer();
            this.NotifiesList.ReloadNotifies( true );

            AlertProvider.ShowAlerts();
        }
        
        #region Menu

        void InitMenu ( )
        {
            barManager1.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( barManager1_ItemClick );
        }
        void barManager1_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag==null )
                return;


            if ( e.Item.Tag.ToString().Contains( "Skin" ) )
                SetSkin( e.Item as DevExpress.XtraBars.BarCheckItem );
        }
        
        #endregion

        #region Skin
        private void InitializeSkins ( )
        {

            int i=100;
            foreach ( DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins )
            {
                i++;
                DevExpress.XtraBars.BarCheckItem menuItem=new DevExpress.XtraBars.BarCheckItem();
                menuItem.Caption=skin.SkinName;
                menuItem.Name="Skin:"+skin.SkinName;
                menuItem.Tag="Skin:"+skin.SkinName;
                menuItem.Id=i;
                menuItem.Checked=false;

                barManager1.Items.Add( menuItem );
                barMenuSkin.LinksPersistInfo.Add( new DevExpress.XtraBars.LinkPersistInfo( menuItem ) );
                //         barMenuSkins.AddItem( menuItem );
            }
        }

        private void SetSkin ( DevExpress.XtraBars.BarCheckItem menuItem )
        {
            foreach ( DevExpress.XtraBars.BarItemLink link in barMenuSkin.ItemLinks )
                ( link.Item as DevExpress.XtraBars.BarCheckItem ).Checked=false;
            menuItem.Checked=true;

            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName=menuItem.Tag.ToString().Split( ':' )[1];
        }
        #endregion

        #region Notify
        public NotifyList NotifiesList;
        public void InitNotifiesList ( )
        {
            NotifiesList=new NotifyList( this );
            NotifiesList.Dock=DockStyle.Fill;
            this.dockPanelNotify_Container.Controls.Add( NotifiesList );
        }
        #endregion

        #region Chat

        public UserOnlineList UserList;
        public ABCChatScreen ChatScreen;
        public void InitChat ( )
        {
            ChatScreen=new ABCChatScreen( );

            UserList=new UserOnlineList( ChatScreen );
            UserList.Dock=DockStyle.Fill;
            this.dockPanelUser_Container.Controls.Add( UserList );

        }

       

        #endregion

        #region Meeting
        public MeetingList MeetingsList;
        public void InitMeetingsList ( )
        {
            MeetingsList=new MeetingList( this );
            MeetingsList.Dock=DockStyle.Fill;

            this.dockPanelMeeting_Container.Controls.Add( MeetingsList );
        }
        #endregion

        #region Task
        public AssignedTaskList AssignedTasksList;
        public BeAssignedTaskList BeAssignedTasksList;
        public void InitTasksList ( )
        {
            BeAssignedTasksList=new BeAssignedTaskList( this );
            BeAssignedTasksList.Dock=DockStyle.Fill;
            this.dockPanelMyTask_Container.Controls.Add( BeAssignedTasksList );

            AssignedTasksList=new AssignedTaskList( this );
            AssignedTasksList.Dock=DockStyle.Fill;
            this.dockPanelAssignedTask_Container.Controls.Add( AssignedTasksList );
        }
        #endregion

        #region Homepages
        public HomePagesArea homePagesArea;
        public void InitHomepages ( )
        {
            homePagesArea=new HomePagesArea(this);
            homePagesArea.LoadHomePages();
            homePagesArea.Dock=DockStyle.Fill;

            this.Controls.Add( homePagesArea );
        }
        #endregion
    
        private void dockPanelNotify_Click ( object sender , EventArgs e )
        {

        }

        private void dockPanel4_Click ( object sender , EventArgs e )
        {

        }

        private void dockPanelUser_Click ( object sender , EventArgs e )
        {

        }

    }
}