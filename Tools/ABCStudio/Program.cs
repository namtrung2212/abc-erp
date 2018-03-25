using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using ABCControls;
using ABCApp;
namespace ABCStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main ( )
        {

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();

            Application.Idle+=new EventHandler( Application_Idle );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

      
            ABCStudio.ABCStudioManager.Start();

        }

        static void Application_Idle ( object sender , EventArgs e )
        {
            ABCControls.HostSurfaceManager.DestroySurfaceList();
        }

    }
}
