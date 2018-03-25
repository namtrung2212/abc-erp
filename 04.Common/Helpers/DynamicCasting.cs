using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;

namespace ABCHelper
{

    public static class DynamicCasting
    {
        private static T Cast<T> ( dynamic o )
        { return (T)o; }

        /// <exception cref="System.InvalidCastException"></exception>
        public static dynamic DynamicCast ( this Type T , dynamic o )
        {
            return typeof( DynamicCasting ).GetMethod( "Cast" , BindingFlags.Static|BindingFlags.NonPublic )
                .MakeGenericMethod( T ).Invoke( null , new object[] { o } );
        }
    }
}
