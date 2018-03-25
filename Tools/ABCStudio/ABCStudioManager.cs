using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using ABCCommon;
using ABCStudio;
using ABCControls;
using ABCScreen;
using Microsoft.Win32;
using System.Runtime.InteropServices;

using ABCProvider;
using ABCBusinessEntities;

namespace ABCStudio
{
    public class ABCStudioManager : IABCStudioManager
    {
        static IABCStudioManager Instance
        {
            get { return ABCStudioHelper.Instance; }
        }

        public Form MainStudio { get; set; }
        public static STViewsInfo CustomizeView;

        public static void Start ( )
        {
            Start( null );
        }
      
        public static void Start ( STViewsInfo viewInfo )
        {
            SystemProvider.Initialize();

            if ( ABCStudioHelper.Instance==null )
                ABCStudioHelper.Instance=new ABCStudioManager();

            CustomizeView=viewInfo;
            if ( CustomizeView!=null )
                ABCStudioManager.Instance.StartSection();
            else
                ABCUserManager.ShowLogIn( LoginType.Studio );
        }

        static System.Timers.Timer CloseTrialTimer;
        public void StartSection ( )
        {
            SystemProvider.StartSection();
            if ( SystemProvider.SystemConfig.IsRelease )
            {
                ABCUserProvider.SynchronizePermission();
           //     InventoryProvider.PeriodEndingProcessings();
            }

            ABCScreen.SplashUtils.ShowSplash( LoginType.Studio );

            CloseTrialTimer=new System.Timers.Timer();
            CloseTrialTimer.Interval=10;
            CloseTrialTimer.Elapsed+=new System.Timers.ElapsedEventHandler( CloseDevexpressTrialForm );
            CloseTrialTimer.Start();

            MainStudio=new Studio( false );

            if ( CustomizeView!=null )
            {
                ( MainStudio as Studio ).Worker.OpenFromDatabase( CustomizeView );
                ( MainStudio as Studio ).RefreshFieldBindingTree( true );
            }

            ABCScreen.SplashUtils.CloseSplash();

            MainStudio.ShowDialog();

        }


        static void CloseDevexpressTrialForm ( object sender , System.Timers.ElapsedEventArgs e )
        {
            // retrieve the handler of the window  
            int iHandle=FindWindow( null , "Information" );
            if ( iHandle>0 )
            {
                // close the window using API        
                SendMessage( iHandle , WM_SYSCOMMAND , SC_CLOSE , 0 );
                CloseTrialTimer.Close();

            }
            if ( ABCScreen.SplashUtils.IsShowing()==false )
                CloseTrialTimer.Close();
        }

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName,string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
            
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

    }
}
