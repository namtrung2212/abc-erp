using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.IO;

using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{
	public enum LoaderType
	{
		BasicDesignerLoader = 1,
		CodeDomDesignerLoader = 2,
		NoLoader = 3
	}
  
	public class HostSurfaceManager : DesignSurfaceManager
	{

        public static HostSurfaceManager CurrentManager;
   
        public HostSurfaceManager ( )
            : base()
        {
            CurrentManager=this;
        }

        #region Template Surface

        public static HostControl TemplateView;
        public static void CreateTemplateComponent ( Type type )
        {
            if ( TemplateView==null )
            {
                TemplateView=CurrentManager.GetNewHost( typeof( ABCControls.ABCView ) , LoaderType.CodeDomDesignerLoader );
                TemplateView.HostSurface.IsTemplate=true;
            }
            try
            {
                Component comp=(Component)TemplateView.DesignerHost.CreateComponent( type );
                if ( comp is Control )
                    ( (Control)comp ).Parent=(ABCControls.ABCView)TemplateView.DesignerHost.RootComponent;
            }
            catch ( Exception ex )
            {
            }
        }

        public static Dictionary<String , Component> TemplateComList=new Dictionary<string , Component>();
        public static Component GetTemplateComponent ( Type type )
        {
            Component obj=null;

            if ( TemplateComList.TryGetValue( type.FullName , out obj ) )
                return obj;

            foreach ( Component comp in TemplateView.DesignerHost.Container.Components )
            {
                obj=GetTemplateComponent( type , comp );
                if ( obj!=null )
                {
                    TemplateComList.Add( type.FullName , obj );
                    return obj;
                }
            }
            return obj;
        }
        public static Component GetTemplateComponent ( Type type , IComponent currentCom )
      {
          if ( currentCom.GetType()==type )
              return (Component)currentCom;

          ComponentDesigner designer=(ComponentDesigner)TemplateView.DesignerHost.GetDesigner( currentCom );
          if ( designer!=null&&designer.AssociatedComponents!=null )
          {
              foreach ( object associatedComponent in designer.AssociatedComponents )
              {
                  Component comp=GetTemplateComponent( type , (IComponent)associatedComponent );
                  if ( comp!=null )
                      return comp;
              }
          }

          return null;
      }
      
        #endregion

		protected override DesignSurface CreateDesignSurfaceCore(IServiceProvider parentProvider)
		{
			return new HostSurface(parentProvider);
		}
        public void AddService ( Type type , object serviceInstance )
        {
            this.ServiceContainer.AddService( type , serviceInstance );
        }
        
        #region GetNewHost

        private HostControl GetNewHost ( Type rootComponentType )
        {
            HostSurface hostSurface=(HostSurface)this.CreateDesignSurface( this.ServiceContainer );

            if ( rootComponentType==typeof( DevExpress.XtraEditors.XtraForm ) )
                hostSurface.BeginLoad( typeof( DevExpress.XtraEditors.XtraForm ) );
            else if ( rootComponentType==typeof( ABCControls.ABCView ) )
                hostSurface.BeginLoad( typeof( ABCControls.ABCView ) );
            else if ( rootComponentType==typeof( UserControl ) )
                hostSurface.BeginLoad( typeof( UserControl ) );
            else if ( rootComponentType==typeof( Component ) )
                hostSurface.BeginLoad( typeof( Component ) );
            else
                throw new Exception( "Undefined Host Type: "+rootComponentType.ToString() );

            hostSurface.Initialize();
            this.ActiveDesignSurface=hostSurface;
            return new HostControl( hostSurface );
        }
        public HostControl GetNewHost ( Type rootComponentType , LoaderType loaderType )
        {
            if ( loaderType==LoaderType.NoLoader )
                return GetNewHost( rootComponentType );

            HostSurface hostSurface=(HostSurface)this.CreateDesignSurface( this.ServiceContainer );
            IDesignerHost host=(IDesignerHost)hostSurface.GetService( typeof( IDesignerHost ) );

            switch ( loaderType )
            {
                case LoaderType.BasicDesignerLoader:
                    BasicHostLoader basicHostLoader=new BasicHostLoader( rootComponentType );
                    hostSurface.BeginLoad( basicHostLoader );
                    hostSurface.Loader=basicHostLoader;
                    break;

                case LoaderType.CodeDomDesignerLoader:
                    CodeDomHostLoader codeDomHostLoader=new CodeDomHostLoader();
                    hostSurface.BeginLoad( codeDomHostLoader );
                    hostSurface.Loader=codeDomHostLoader;
                    break;

                default:
                    throw new Exception( "Loader is not defined: "+loaderType.ToString() );
            }

            hostSurface.Initialize();
            return new HostControl( hostSurface );
        }
        public HostControl GetHostFromSourceCode ( String strSourceCode )
        {
            HostSurface hostSurface=(HostSurface)this.CreateDesignSurface( this.ServiceContainer );
            IDesignerHost host=(IDesignerHost)hostSurface.GetService( typeof( IDesignerHost ) );

            CodeDomHostLoader codeDomHostLoader=new CodeDomHostLoader();
            codeDomHostLoader.SourceCodeToPasrse=strSourceCode;

            hostSurface.BeginLoad( codeDomHostLoader );
            hostSurface.Loader=codeDomHostLoader;
            hostSurface.Initialize();

            return new HostControl( hostSurface );
        }
        public HostControl GetNewHostFromFile ( string fileName )
        {
            if ( fileName==null||!File.Exists( fileName ) )
                MessageBox.Show( "FileName is incorrect: "+fileName );


            if ( fileName.EndsWith( "xml" ) )
            {
                HostSurface hostSurface=(HostSurface)this.CreateDesignSurface( this.ServiceContainer );
                IDesignerHost host=(IDesignerHost)hostSurface.GetService( typeof( IDesignerHost ) );

                BasicHostLoader basicHostLoader=new BasicHostLoader( fileName );
                hostSurface.BeginLoad( basicHostLoader );
                hostSurface.Loader=basicHostLoader;
                hostSurface.Initialize();
                return new HostControl( hostSurface );
            }
            else
            {
                StreamReader sr=new StreamReader( fileName );
                string strSourceCode=sr.ReadToEnd();
                return GetHostFromSourceCode( strSourceCode );
            }



        }

        public HostControl AddNewForm ( )
        {
            return AddNewForm( "UnSaved View" );
        }
        public HostControl AddNewForm ( String strTextForm )
        {

            HostControl hc=this.GetNewHost( typeof( ABCControls.ABCView ) , LoaderType.CodeDomDesignerLoader );
            //   HostControl hc=this.GetNewHostFromFile( @"D:\ccc.cs" );
            //  ( (ABCControls.ABCView)hc.HostSurface.DesignerHost.RootComponent ).Size=new System.Drawing.Size( 466 , 400 );
            ( (ABCControls.ABCView)hc.HostSurface.DesignerHost.RootComponent ).Text=strTextForm;

            return hc;
        }
        public HostControl OpenNewForm ( String strFileName )
        {

            HostControl hc=this.GetNewHost( typeof( ABCControls.ABCView ) , LoaderType.CodeDomDesignerLoader );
         
            DesignerTransaction transaction=hc.HostSurface.DesignerHost.CreateTransaction( "Open XML" );
         
            ((ABCView)hc.HostSurface.DesignerHost.RootComponent ).Load( strFileName,ViewMode.Design );

            transaction.Commit();
            ( (IDisposable)transaction ).Dispose();

            return hc;
        }

        public HostControl OpenNewForm ( STViewsInfo viewInfo )
        {
            try
            {
                HostControl hc=this.GetNewHost( typeof( ABCControls.ABCView ) , LoaderType.CodeDomDesignerLoader );

                DesignerTransaction transaction=hc.HostSurface.DesignerHost.CreateTransaction( "Open XML" );

                ( (ABCView)hc.HostSurface.DesignerHost.RootComponent ).Load( viewInfo , ViewMode.Design );

                transaction.Commit();
                ( (IDisposable)transaction ).Dispose();

                return hc;
            }
            catch ( Exception ex )
            {
            }
            return null;
        }

        
        #endregion

        static List<DesignSurface> NeedDisposeSurfaceList=new List<DesignSurface>();
        public static void DestroySurfaceList ( )
        {
            foreach(DesignSurface surface in NeedDisposeSurfaceList)
                surface.Dispose();

            NeedDisposeSurfaceList.Clear();
        }

        public void CloseSurface ( DesignSurface surface )
        {
            int iFound=-1;
            for ( int i=0; i<this.DesignSurfaces.Count; i++ )
            {
                if ( this.DesignSurfaces[i]==surface )
                {
                    iFound=i;
                    break;
                }
            }

            if ( iFound>=0 )
                NeedDisposeSurfaceList.Add( this.DesignSurfaces[iFound] );
              

        }

        public void RefreshSurface ( Guid iViewID )
        {
            foreach ( HostSurface surface in this.DesignSurfaces )
            {
                foreach ( Component comp in surface.DesignerHost.Container.Components )
                {
                    if (  comp is ABCView  && surface.DesignerHost.RootComponent!=comp )
                    {
                        if ( ( (ABCView)comp ).ViewID==iViewID )
                            ( (ABCView)comp ).RefreshLayout();
                    }
                }
            }

        }

	}
}
