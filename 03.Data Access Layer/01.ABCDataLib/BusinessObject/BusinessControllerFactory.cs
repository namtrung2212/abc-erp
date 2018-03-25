using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;


namespace ABCBusinessEntities
{
    public class BusinessControllerFactory
    {

        public static Dictionary<String,BusinessObjectController> BusControllersList = new Dictionary<String,BusinessObjectController>( );

        public static BusinessObjectController GetBusinessController ( String strTableName )
        {
            if ( BusControllersList.Count<=0 )
            {
                AppDomain domain=AppDomain.CreateDomain( "ABCBusinessObject" );
                GetAllController( domain , Application.StartupPath+"\\BaseObjects.dll" );
                GetAllController( domain , Application.StartupPath+"\\BusinessObjects.dll" );
                AppDomain.Unload( domain );
            }

            BusinessObjectController businessCtrl=null;
            BusControllersList.TryGetValue( strTableName+"Controller" , out businessCtrl );
            if ( businessCtrl!=null )
                return businessCtrl;

            return null;
        }

        public static void GetAllController (AppDomain domain,String strAssFileName )
        {
          
            try
            {
                Assembly assEntities=domain.Load( AssemblyName.GetAssemblyName( strAssFileName ) );
                if ( assEntities==null )
                    return;

                foreach ( Type type in assEntities.GetTypes() )
                {
                    if ( typeof( BusinessObjectController ).IsAssignableFrom( type ) )
                    {
                        BusinessObjectController Ctrl=(BusinessObjectController)ABCDynamicInvoker.CreateInstanceObject( type );
                        if ( Ctrl!=null )
                            BusControllersList.Add( type.Name , Ctrl );
                    }
                }
            }
            catch ( Exception ex )
            {
            }
            
        }
    
    }
    
}
