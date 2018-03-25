using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABCScreen
{
    public class SplashUtils
    {
        public static void ShowSplash ( ABCCommon.LoginType type )
        {
            Cursor.Current=Cursors.WaitCursor;
            if ( IsShowing()==false )
            {
                if ( type==ABCCommon.LoginType.Studio )
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm( typeof( ABCStudioSplashScreen ) );
                if ( type==ABCCommon.LoginType.ERP )
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm( typeof( ABCAppSplashScreen ) );
            }
        }
        public static void CloseSplash ( )
        {
            Cursor.Current=Cursors.Default;

            if ( DevExpress.XtraSplashScreen.SplashScreenManager.Default!=null )
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }
        public static bool IsShowing ( )
        {
            if ( DevExpress.XtraSplashScreen.SplashScreenManager.Default!=null )
                return DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible;

            return false;
        }
    }
}
