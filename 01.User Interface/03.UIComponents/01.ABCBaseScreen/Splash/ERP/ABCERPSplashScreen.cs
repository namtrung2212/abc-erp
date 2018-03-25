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
    public partial class ABCAppSplashScreen : SplashScreen
    {
        public ABCAppSplashScreen ( )
        {
            InitializeComponent();
            this.TopMost=true;
            this.StartPosition=FormStartPosition.CenterScreen;
            this.Activated+=new EventHandler( ABCAppSplashScreen_Activated );
        }

        void ABCAppSplashScreen_Activated ( object sender , EventArgs e )
        {
            this.TopMost=true;
        //    this.Location=FormStartPosition.CenterScreen;
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

        private void pictureEdit1_EditValueChanged ( object sender , EventArgs e )
        {

        }
    }
}