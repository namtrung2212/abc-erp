using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ABCProvider;
namespace AutoGenDLL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main ( )
        {

            if ( DataQueryProvider.Connect()==false )
            {
                MessageBox.Show( "Please check connection" , "Auto generator" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return;
            }

            DataQueryProvider.InitDataTables();

            GenerationProvider.DetectModifyDatabase();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            MessageBox.Show( "Finished" , "Auto generator" , MessageBoxButtons.OK , MessageBoxIcon.Information );
        }
    }
}
