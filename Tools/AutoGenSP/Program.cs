using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ABCProvider;
namespace AutoGenSP
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
            DevExpress.Utils.WaitDialogForm waiting=new DevExpress.Utils.WaitDialogForm();
            waiting.SetCaption( "Connecting . . .!" );
            waiting.Text="";
            waiting.Show();

            if ( DataQueryProvider.Connect()==false )
            {
                waiting.Close();
                DevExpress.XtraEditors.XtraMessageBox.Show( "Please check connection" , "Auto generator" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return;
            }
            DataQueryProvider.InitDataTables();

            Form1 form=    new Form1();
            waiting.Close();
      
            Application.Run( form );
        }
    }
}
