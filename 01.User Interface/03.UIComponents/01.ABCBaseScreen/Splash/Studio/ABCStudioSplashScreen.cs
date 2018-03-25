using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace ABCScreen
{
    public partial class ABCStudioSplashScreen : SplashScreen
    {

       
        public ABCStudioSplashScreen ( )
        {
            InitializeComponent();
            this.TopMost=true;
            this.StartPosition=FormStartPosition.CenterScreen;
        }

        #region Overrides

        public override void ProcessCommand ( Enum cmd , object arg )
        {
            base.ProcessCommand( cmd , arg );
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void labelControl3_Click ( object sender , EventArgs e )
        {

        }
    }
}