using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using ABCProvider;

namespace ABCBusinessEntities
{
    public class BusinessObjectFactory
    {

        public static Dictionary<String , Type> CachingBusinessObjectType=new Dictionary<string , Type>();
        public static BusinessObject GetBusinessObject ( String strTableName )
        {
            Type type=GetBusinessObjectType( strTableName );
            if ( type!=null )
                return (BusinessObject)ABCDynamicInvoker.CreateInstanceObject( type );

            return null;
        }

        public static Type GetBusinessObjectType ( String strTableName )
        {
            String strBusName=strTableName+"Info";

            Type type=null;

            if ( CachingBusinessObjectType.TryGetValue( strBusName , out type )==false )
            {
                try
                {
                    if ( DataStructureProvider.IsSystemTable( strTableName ) )
                    {
                        Assembly assEntities=Assembly.LoadFrom( Application.StartupPath+"\\BaseObjects.dll" );
                        type=assEntities.GetType( "ABCBusinessEntities."+strBusName );
                    }
                    else
                    {
                        Assembly assEntities=Assembly.LoadFrom( Application.StartupPath+"\\BusinessObjects.dll" );
                        type=assEntities.GetType( "ABCBusinessEntities."+strBusName );
                    }
                    if ( type!=null )
                        CachingBusinessObjectType.Add( strBusName , type );

                }
                catch ( Exception )
                {

                }
            }
            return type;
        }

    }
}
