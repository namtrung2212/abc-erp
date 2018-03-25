using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using CompileProvider;
using ABCHelper;
using ABCControls;

using ABCBusinessEntities;
namespace ABCScreen
{
    public class ABCReportFactory 
    {

        public static Dictionary<String , Type> CachingABCReportType=new Dictionary<string , Type>();

        #region Load ReportType
        public static void LoadAllReportTypes ( )
        {
            LoadAllReportTypes( Application.StartupPath+"\\ABCAppReports.dll" );
       
            if ( System.IO.Directory.Exists( Application.StartupPath+"\\Reports" ) )
            {
                foreach ( string strFileName in System.IO.Directory.GetFiles( Application.StartupPath+"\\Reports" ) )
                    LoadAllReportTypes( strFileName );
            }
        }
        public static void LoadAllReportTypes ( String strFileName )
        {
            if ( System.IO.File.Exists( strFileName )==false )
                return;

            Assembly assembly=Assembly.LoadFrom( strFileName );
            foreach ( Type type in assembly.GetTypes() )
            {
                if ( typeof( ABCBaseReport ).IsAssignableFrom( type )&&CachingABCReportType.ContainsKey(type.Name )==false )
                    CachingABCReportType.Add( type.Name , type );
            }
        }

        public static Type GetABCReportType ( String strReportName )
        {
          
            Type type=null;

            if ( CachingABCReportType.TryGetValue( strReportName , out type )==false )
            {
                try
                {
                    Assembly assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCAppReports.dll" );
                    type=assbly.GetType( "ABCApp.Reports."+strReportName );
                   
                    if ( type!=null )
                        CachingABCReportType.Add( strReportName , type );

                }
                catch ( Exception )
                {

                }
            }
            return type;
        }
        #endregion

        public static Assembly CompileReportSouceCode ( STViewsInfo info )
        {
            if ( String.IsNullOrWhiteSpace( info.STViewCode ) )
                return null;

            return CompileReportSouceCode( info.STViewCode , info.STViewNo );
        }

        static int iCount=0;
        public static Assembly CompileReportSouceCode ( String strSourceCode,String strViewNo )
        {
            if ( System.IO.Directory.Exists( @"Temp" )==false )
                System.IO.Directory.CreateDirectory( @"Temp" );
            
          iCount++;

           CompiledAssembly ass=(CompiledAssembly)Compiler.CompileAssembly( strSourceCode , CodeType.CSharp , String.Format( @"TempReport{0}_{1}.dll" , strViewNo ,iCount) ,
                                                new string[] { "System.dll" ,"System.Data.dll", "System.Core.dll","Microsoft.CSharp.dll","System.Drawing.dll","System.Windows.Forms.dll","ABCBaseScreen.dll","dll","ABCControls.dll","ABCStudio.exe","ABCApp.exe","BaseObjects.dll","BusinessObjects.dll",
                                                    "DevExpress.Data.v12.1.dll",   "DevExpress.Utils.v12.1.dll",   "DevExpress.XtraBars.v12.1.dll",    "DevExpress.XtraEditors.v12.1.dll"} );
            if ( ass==null||ass.Errors.Errors.Length>0 )
                return null;

            return ass.Assembly;
        }
        public Assembly CallComplie ( String strSourceCode , String strViewNo )
        {
            return CompileReportSouceCode( strSourceCode , strViewNo );
        }
    }
}
