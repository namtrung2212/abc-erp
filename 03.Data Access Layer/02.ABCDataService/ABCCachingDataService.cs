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

    [ServiceContract]
    public interface ICachingData
    {
        [OperationContract]
        double Add ( double n1 , double n2 );
    }


    public class ABCCachingDataService : ICachingData
    {
        public double Add ( double n1 , double n2 )
        {
            double result=n1+n2;
            return result;
        }
    }

}
