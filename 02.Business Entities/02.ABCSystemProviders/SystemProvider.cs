using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

using System.Reflection;
using System.Reflection.Emit;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class SystemProvider
    {
      
        public static GEAppConfigsInfo AppConfig;
        public static STSystemConfigsInfo SystemConfig;

        public static void Initialize ( )
        {

            if ( ABCApp.ABCDataGlobal.Language=="VN" )
            {
                System.Threading.Thread.CurrentThread.CurrentCulture=new System.Globalization.CultureInfo( "vi-VN" );
                System.Threading.Thread.CurrentThread.CurrentUICulture=new System.Globalization.CultureInfo( "vi-VN" );
            }

            #region Complier

            #region Complier Runtime
            AppDomainSetup setup=AppDomain.CurrentDomain.SetupInformation;
            setup.ShadowCopyFiles="true";
            AppDomain.CurrentDomain.SetShadowCopyFiles();
            #endregion

            if ( System.IO.Directory.Exists( @"Temp" )==false )
                System.IO.Directory.CreateDirectory( @"Temp" );

            foreach ( String strFileName in System.IO.Directory.GetFiles( AppDomain.CurrentDomain.BaseDirectory ) )
            {
                try
                {
                    if ( strFileName.Contains( "TempScreen" ) )
                        System.IO.File.Delete( strFileName );
                }
                catch ( Exception ex )
                {

                }
            }
            #endregion

            ABCScreen.IABCScreenManager globalBase=(ABCScreen.IABCScreenManager)AppDomain.CurrentDomain.CreateInstanceAndUnwrap( "ABCBaseScreen" , "ABCScreen.ABCScreenManager" );
            globalBase.CallInitialize();
        }

        public static void StartSection ( )
        {
            Initialize();

            AppConfig=new GEAppConfigsController().GetFirstObject() as GEAppConfigsInfo;
            if ( AppConfig==null )
                AppConfig=new GEAppConfigsInfo();

            SystemConfig=new STSystemConfigsController().GetFirstObject() as STSystemConfigsInfo;
            if ( SystemConfig==null )
                SystemConfig=new STSystemConfigsInfo();

  
        }
     }
}
