using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using ABCControls;
using CompileProvider;

using ABCBusinessEntities;
using ABCCommon;
namespace ABCScreen
{
    public class ABCScreenFactory 
    {

        public static Dictionary<String , Type> CachingABCScreenType=new Dictionary<string , Type>();

        public static ABCBaseScreen GetABCScreen ( Guid iViewID )
        {
            return GetABCScreen( iViewID , ViewMode.Runtime );
        }
        public static ABCBaseScreen GetABCScreen ( Guid iViewID , ViewMode mode )
        {
            STViewsInfo info=new STViewsController().GetObjectByID( iViewID ) as STViewsInfo;
            if ( info!=null )
                return GetABCScreen( info , mode );

            return null;
        }
        public static ABCBaseScreen GetABCScreen ( String strScreenName )
        {
            return GetABCScreen( strScreenName , ViewMode.Runtime );
        }
        public static ABCBaseScreen GetABCScreen ( String strScreenName , ViewMode mode )
        {
            STViewsInfo info=new STViewsController().GetObjectByNo( strScreenName ) as STViewsInfo;
            if ( info!=null )
                return GetABCScreen( info , mode );

            return null;
        }
        public static ABCBaseScreen GetABCScreen ( STViewsInfo info , ViewMode mode )
        {
            if ( info.STViewID==null || info.STViewID==Guid.Empty )
                return null;

            info=(STViewsInfo)new STViewsController().GetObjectByID( info.STViewID );
            if ( info==null )
                return null;


            if ( ABCScreenManager.Instance.CheckViewPermission( info.STViewID , ViewPermission.AllowView )==false )
            {
                ABCHelper.ABCMessageBox.Show( "Bạn không đủ quyền hạn sử dụng tính năng này!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return null;
            }
            ABCBaseScreen screen=null;

            #region Get Type
            Type type=GetABCScreenType( info.STViewID );
            if ( type==null )
            {
                if ( info.STViewUseCode&&String.IsNullOrWhiteSpace( info.STViewCode )==false )
                {
                    Assembly ass=ABCScreenFactory.CompileScreenSouceCode( info );
                    if ( ass!=null )
                    {
                        if(!String.IsNullOrWhiteSpace(info.ScreenName))
                            type=ass.GetType( "ABCApp.Screens."+info.ScreenName+"Screen" );

                        if(type==null && !String.IsNullOrWhiteSpace(info.STViewNo))
                            type=ass.GetType( "ABCApp.Screens."+info.STViewNo+"Screen" );

                        if ( type!=null )
                            screen=(ABCBaseScreen)ass.CreateInstance( type.FullName );
                        //   screen=(ABCBaseScreen)AppDomain.CurrentDomain.CreateInstanceAndUnwrap( type.Assembly.FullName ,type.FullName );
                        if ( screen!=null )
                        {
                            screen.LoadScreen( info , mode );
                            return screen;
                        }
                    }
                }
            }

            if ( type==null )
                type=typeof( ABCBaseScreen ); 
            #endregion

            if ( type!=null )
                screen=(ABCBaseScreen)ABCDynamicInvoker.CreateInstanceObject( type );

            if ( screen!=null )
                screen.LoadScreen( info , mode );

            return screen;
        }
        public static ABCBaseScreen GetABCScreen ( String strLayoutXMLPath ,STViewsInfo viewInfo , ViewMode mode )
        {

            //if ( ABCBaseProvider.BaseInstance.CheckViewPermission( viewInfo.STViewNo , ViewPermission.AllowView )==false )
            //{
            //    ABCHelper.ABCMessageBox.Show( "Bạn không đủ quyền hạn sử dụng tính năng này!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
            //    return null;
            //}

            ABCBaseScreen screen=null;
            Type type=null;
            if ( viewInfo!=null )
                type=GetABCScreenType( viewInfo.STViewID );

            if ( type==null )
                type=typeof( ABCBaseScreen );

            screen=(ABCBaseScreen)ABCDynamicInvoker.CreateInstanceObject( type );
            if ( screen!=null )
                screen.LoadScreen( strLayoutXMLPath , mode );

            return screen;

        }

        public static void RunScreen ( Guid iViewID )
        {
            RunScreen( iViewID , ViewMode.Runtime );
        }
        public static void RunScreen ( Guid iViewID , ViewMode mode )
        {
            ABCBaseScreen screen=GetABCScreen( iViewID , mode );
            if ( screen!=null )
                screen.ShowDialog();
        }
        public static void RunScreen ( STViewsInfo info , ViewMode mode )
        {
            ABCBaseScreen screen=GetABCScreen( info , mode );
            if ( screen!=null )
                screen.ShowDialog();
        }
        public static void RunScreen ( String strLayoutXMLPath , STViewsInfo info , ViewMode mode )
        {
            ABCBaseScreen screen=GetABCScreen( strLayoutXMLPath , info , mode );
            if ( screen!=null )
            {
                Form frm=screen.GetDialog();
                if ( info.STViewDesc!=null )
                    frm.Text=info.STViewDesc;
                frm.ShowDialog();
            }
        }
        public static void DebugScreen ( String strLayoutXMLPath , String strSourceCode , String strViewNo , ViewMode mode )
        {
            ABCHelper.ABCWaitingDialog.Show( "" , ". . ." );

            try
            {

                ABCBaseScreen screen=null;
                Type type=null;
                Assembly ass=null;

                #region Build
                if ( String.IsNullOrWhiteSpace( strSourceCode )==false )
                {
                    ass=ABCScreenFactory.CompileScreenSouceCode( strSourceCode , strViewNo );
                    if ( ass!=null )
                        type=ass.GetType( "ABCApp.Screens."+strViewNo+"Screen" );
                }
                if ( type==null )
                    type=typeof( ABCBaseScreen );
                #endregion

                Assembly asBase=null;
                ABCScreenManager globalBase=null;

                AppDomain domain=AppDomain.CreateDomain( "DebugABCScreen" , AppDomain.CurrentDomain.Evidence , AppDomain.CurrentDomain.SetupInformation );
                try
                {
                    asBase=domain.Load( typeof( ABCScreenManager ).Assembly.FullName );
                    globalBase=(ABCScreenManager)domain.CreateInstanceAndUnwrap( "ABCBaseScreen" , "ABCScreen.ABCScreenManager" );
                    globalBase.CallInitialize();

                    if ( ass!=null )
                        domain.Load(ass.GetName());

                    screen=(ABCBaseScreen)domain.CreateInstanceAndUnwrap( type.Assembly.FullName , type.FullName );
                    if ( screen!=null )
                    {
                        screen.LoadScreen( strLayoutXMLPath , mode );
                        ABCHelper.ABCWaitingDialog.Close();
                        screen.ShowDialog();
                    }

                }
                catch ( Exception ex )
                {
                }

                screen=null;
                globalBase=null;
                asBase=null;

                AppDomain.Unload( domain );
                domain=null;
            }
            catch ( Exception exx )
            { }

            GC.Collect(); // collects all unused memory
            GC.WaitForPendingFinalizers(); // wait until GC has finished its work
            GC.Collect();
            ABCHelper.ABCWaitingDialog.Close();
        }

        #region Load ScreenType
        public static void LoadAllScreenTypes ( )
        {
            LoadAllScreenTypes( Application.StartupPath+"\\ABCBaseApp.exe" );
            LoadAllScreenTypes( Application.StartupPath+"\\ABCBaseModules.exe" );
            LoadAllScreenTypes( Application.StartupPath+"\\ABCApp.dll" );
            LoadAllScreenTypes( Application.StartupPath+"\\ABCAppModules.dll" );

            if ( System.IO.Directory.Exists( Application.StartupPath+"\\Modules" ) )
            {
                foreach ( string strFileName in System.IO.Directory.GetFiles( Application.StartupPath+"\\Modules" ) )
                    LoadAllScreenTypes( strFileName );
            }
        }
        public static void LoadAllScreenTypes ( String strFileName )
        {
            if ( System.IO.File.Exists( strFileName )==false )
                return;

            Assembly assembly=Assembly.LoadFrom( strFileName );
            foreach ( Type type in assembly.GetTypes() )
            {
                if ( typeof( ABCBaseScreen ).IsAssignableFrom( type )&&CachingABCScreenType.ContainsKey(type.Name )==false )
                    CachingABCScreenType.Add( type.Name , type );
            }
        }

        public static Type GetABCScreenType ( Guid iViewID )
        {
            STViewsInfo info=new STViewsController().GetObjectByID( iViewID ) as STViewsInfo;
            if ( info==null )
                return null;

            Type type=null;

            String strScreenName=String.Empty;
            if ( String.IsNullOrEmpty( info.ScreenName )==false )
            {
                strScreenName=info.ScreenName+"Screen";
                type=GetABCScreenType( strScreenName );
            }

            if ( type==null )
            {
                strScreenName=info.STViewNo+"Screen";
                type=GetABCScreenType( strScreenName );
            }

            return type;
        }

        private static Type GetABCScreenType ( String strScreenName )
        {

            Type type=null;

            if ( CachingABCScreenType.TryGetValue( strScreenName , out type )==false )
            {
                try
                {
                    Assembly assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCAppModules.dll" );
                    type=assbly.GetType( "ABCApp.Screens."+strScreenName );
                    if ( type==null )
                    {
                        assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCSystemModules.dll" );
                        type=assbly.GetType( "ABCApp.Screens."+strScreenName );
                    }
                    if ( type==null )
                    {
                        assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCApp.dll" );
                        type=assbly.GetType( "ABCApp.Screens."+strScreenName );
                    }
                    if ( type==null )
                    {
                        assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCBaseApp.dll" );
                        type=assbly.GetType( "ABCApp.Screens."+strScreenName );
                    }
                    if ( type==null )
                    {
                        assbly=Assembly.LoadFrom( Application.StartupPath+"\\ABCAppReports.dll" );
                        type=assbly.GetType( "ABCApp.Screens."+strScreenName );
                    }

                    if ( type!=null )
                        CachingABCScreenType.Add( strScreenName , type );

                }
                catch ( Exception )
                {

                }
            }
            return type;
        }
        #endregion

        public static Assembly CompileScreenSouceCode ( STViewsInfo info )
        {
            if ( String.IsNullOrWhiteSpace( info.STViewCode ) )
                return null;

            return CompileScreenSouceCode( info.STViewCode , info.STViewNo );
        }

        static int iCount=0;
        public static Assembly CompileScreenSouceCode ( String strSourceCode,String strViewNo )
        {
            if ( System.IO.Directory.Exists( @"Temp" )==false )
                System.IO.Directory.CreateDirectory( @"Temp" );
            
          iCount++;

           CompiledAssembly ass=(CompiledAssembly)Compiler.CompileAssembly( strSourceCode , CodeType.CSharp , String.Format( @"TempScreen{0}_{1}.dll" , strViewNo ,iCount) ,
                                                new string[] { "System.dll" ,"System.Data.dll", "System.Core.dll","Microsoft.CSharp.dll","System.Drawing.dll","System.Windows.Forms.dll","ABCBaseScreen.dll","dll","ABCControls.dll","ABCStudio.exe","ABCApp.exe","BaseObjects.dll","BusinessObjects.dll",
                                                    "DevExpress.Data.v12.1.dll",   "DevExpress.Utils.v12.1.dll",   "DevExpress.XtraBars.v12.1.dll",    "DevExpress.XtraEditors.v12.1.dll"} );
            if ( ass==null||ass.Errors.Errors.Length>0 )
                return null;

            return ass.Assembly;
        }
        public Assembly CallComplie ( String strSourceCode , String strViewNo )
        {
            return CompileScreenSouceCode( strSourceCode , strViewNo );
        }
    }
}
