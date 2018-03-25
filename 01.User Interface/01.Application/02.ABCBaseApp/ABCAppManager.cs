using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using ABCBusinessEntities;

using ABCStudio;
using ABCControls;
using ABCScreen;
using ABCCommon;

using ABCProvider;
namespace ABCApp
{
    public class ABCAppManager : IABCAppManager
    {
        public static IABCAppManager Instance
        {
            get { return ABCAppHelper.Instance; }
        }
        public Form MainForm { get; set; }

        public static void Start ( )
        {
          
            SystemProvider.Initialize();
            
            if ( ABCAppHelper.Instance==null )
                ABCAppHelper.Instance=new ABCAppManager();

            ABCScreen.ABCUserManager.ShowLogIn( LoginType.ERP );

        }
        public void StartSection ( )
        {
            SystemProvider.StartSection();

            Application.ThreadExit+=new EventHandler( Application_ThreadExit );
            MainForm=new MainForm();
            MainForm.FormClosed+=new FormClosedEventHandler( MainForm_FormClosed );
           
            ABCScreen.SplashUtils.CloseSplash();
            MainForm.ShowDialog();
        }
        public void Chat ( String strWithUser )
        {
            if ( MainForm!=null && (MainForm as ABCApp.MainForm).ChatScreen!=null)
            {
                ( MainForm as ABCApp.MainForm ).ChatScreen.OpenChatBox( strWithUser );
                ( MainForm as ABCApp.MainForm ).ChatScreen.ActiveChatPanel( strWithUser );
            }
        }
        void MainForm_FormClosed ( object sender , FormClosedEventArgs e )
        {
            ABCUserManager.OnlineUpdate( false );
        }

        void Application_ThreadExit ( object sender , EventArgs e )
        {
            ABCUserManager.OnlineUpdate( false );
        }

        public void CustomizeView ( STViewsInfo viewInfo )
        {
            if ( viewInfo==null )
                return;

            ABCStudio.ABCStudioManager.Start( viewInfo );
        }

    }
}
