using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace ABCApp
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            ABCApp.ABCAppManager.Start();

        }
    }
}
