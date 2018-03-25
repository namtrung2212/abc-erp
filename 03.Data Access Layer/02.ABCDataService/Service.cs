using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using System.Configuration.Install;

namespace ABCClientDataService
{

    public class ABCDataWindowsService : ServiceBase
    {
        public ServiceHost serviceHost=null;
        public ABCDataWindowsService ( )
        {   
            ServiceName="ABCDataWindowsService";
        }

        public static void Main ( )
        {
            ServiceBase.Run( new ABCDataWindowsService() );
        }

        protected override void OnStart ( string[] args )
        {
            if ( serviceHost!=null )
                serviceHost.Close();

            Uri baseAddress=new Uri( "http://localhost/ABCCachingDataService" );

            serviceHost=new ServiceHost( typeof( ABCCachingDataService ) );
        
            WSHttpBinding binding=new WSHttpBinding();
            binding.OpenTimeout=new TimeSpan( 0 , 10 , 0 );
            binding.CloseTimeout=new TimeSpan( 0 , 10 , 0 );
            binding.SendTimeout=new TimeSpan( 0 , 10 , 0 );
            binding.ReceiveTimeout=new TimeSpan( 0 , 10 , 0 );

            serviceHost.AddServiceEndpoint( "ICachingData" , binding , baseAddress );
            serviceHost.Open();
        }

        protected override void OnStop ( )
        {
            if ( serviceHost!=null )
            {
                serviceHost.Close();
                serviceHost=null;
            }
        }
    }

    [RunInstaller( true )]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller ( )
        {
            process=new ServiceProcessInstaller();
            process.Account=ServiceAccount.LocalSystem;
      
            service=new ServiceInstaller();
            service.ServiceName="ABCDataWindowsService";
            Installers.Add( process );
            Installers.Add( service );
        }
    }
}