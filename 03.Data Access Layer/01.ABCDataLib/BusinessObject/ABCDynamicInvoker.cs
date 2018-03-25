using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using ABCBusinessEntities;

namespace ABCBusinessEntities
{
    public delegate object GetHandler(object source);
    public delegate void SetHandler(object source, object value);
    public delegate object InstantiateObjectHandler();

    public sealed class ABCDynamicMethodCompiler
    {
        // DynamicMethodCompiler
        private ABCDynamicMethodCompiler() { }

        // CreateInstantiateObjectDelegate
        internal static InstantiateObjectHandler CreateInstantiateObjectHandler(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            if (constructorInfo == null)
            {
                throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", type));
            }

            DynamicMethod dynamicMethod = new DynamicMethod("InstantiateObject", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(object), null, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);
            return (InstantiateObjectHandler)dynamicMethod.CreateDelegate(typeof(InstantiateObjectHandler));
        }
        
        // CreateGetDelegate
        internal static GetHandler CreateGetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Call, getMethodInfo);
            BoxIfNeeded(getMethodInfo.ReturnType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        // CreateGetDelegate
        internal static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            BoxIfNeeded(fieldInfo.FieldType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        // CreateSetDelegate
        internal static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(setMethodInfo.GetParameters()[0].ParameterType, setGenerator);
            setGenerator.Emit(OpCodes.Call, setMethodInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }

        // CreateSetDelegate
        internal static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(fieldInfo.FieldType, setGenerator);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }

        // CreateGetDynamicMethod
        private static DynamicMethod CreateGetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
        }

        // CreateSetDynamicMethod
        private static DynamicMethod CreateSetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicSet", typeof(void), new Type[] { typeof(object), typeof(object) }, type, true);
        }

        // BoxIfNeeded
        private static void BoxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        // UnboxIfNeeded
        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }
    }

    public sealed class ABCDynamicInvoker
    {
        public static Dictionary<String , GetHandler> lstGetHandler=new Dictionary<String , GetHandler>();
        public static Dictionary<String , SetHandler> lstSetHandler=new Dictionary<String , SetHandler>();
        public static Dictionary<String , InstantiateObjectHandler> lstInitHandler=new Dictionary<String , InstantiateObjectHandler>();

        public static object GetValue ( BusinessObject obj , String strColName )
        {
            if ( obj==null )
                return null;

            string key=obj.AATableName+strColName;

            try
            {
                GetHandler getHandler=null;
                if ( lstGetHandler.TryGetValue( key , out getHandler )==false )
                {
                    Type type=obj.GetType();

                    PropertyInfo proInfo=BusinessObjectHelper.GetProperty(obj.AATableName, strColName );
                    if ( proInfo==null )
                        proInfo=type.GetProperty( strColName );

                    getHandler=ABCDynamicMethodCompiler.CreateGetHandler( type , proInfo );
                    lstGetHandler.Add( key , getHandler );
                }

                return getHandler( obj );
            }
            catch ( System.Exception ex )
            {
                PropertyInfo proInfo=obj.GetType().GetProperty( strColName );
                if ( proInfo==null )
                {
                //    Utilities.ABCLogging.LogNewMessage( "ABCDataLib" , "" , "GetValue" , obj.GetType().Name+" not contain "+strColName , "FAILE" );
                    return null;
                }

                return proInfo.GetValue( obj , null );
            }

        }
        public static object GetValue ( BusinessObject obj , PropertyInfo proInfo )
        {
            string key=obj.AATableName+proInfo.Name;

            try
            {
                GetHandler getHandler=null;
                if ( lstGetHandler.TryGetValue( key , out getHandler )==false )
                {
                    getHandler=ABCDynamicMethodCompiler.CreateGetHandler( obj.GetType() , proInfo );
                    lstGetHandler.Add( key , getHandler );
                }
                return getHandler( obj );
            }
            catch ( System.Exception ex )
            {
                return proInfo.GetValue( obj , null );
            }

        }

        public static void SetValue ( BusinessObject obj , String strColName , object value )
        {
            string key=obj.AATableName+strColName;

            try
            {
                SetHandler setHandler=null;
                if ( lstSetHandler.TryGetValue( key , out setHandler )==false )
                {
                    Type type=obj.GetType();

                    PropertyInfo proInfo=BusinessObjectHelper.GetProperty( obj.AATableName , strColName );
                    if ( proInfo==null )
                        proInfo=type.GetProperty( strColName );

                    setHandler=ABCDynamicMethodCompiler.CreateSetHandler( type , proInfo );
                    lstSetHandler.Add( key , setHandler );

                }

                if ( value is String&&value.ToString().Replace("'","").ToUpper()=="TRUE" )
                    value=true;
                else if ( value is String&&value.ToString().Replace("'","").ToUpper()=="FALSE" )
                    value=false;
                setHandler( obj , value );
            }
            catch ( System.Exception ex )
            {
                PropertyInfo proInfo=obj.GetType().GetProperty( strColName );
                if ( proInfo==null )
                {
                    //    Utilities.ABCLogging.LogNewMessage( "ABCDataLib" , "" , "SetValue" , obj.GetType().Name+" not contain "+strColName , "FAILE" );
                    return;
                }
                proInfo.SetValue( obj , value , null );
            }

        }
        public static void SetValue ( BusinessObject obj , PropertyInfo proInfo , object value )
        {
            string key=obj.AATableName+proInfo.Name;

            try
            {
                SetHandler setHandler=null;
                if (  lstSetHandler.TryGetValue( key , out setHandler) ==false )
                {
                    setHandler=ABCDynamicMethodCompiler.CreateSetHandler( obj.GetType() , proInfo );
                    lstSetHandler.Add( key , setHandler );
                }
                setHandler( obj , value );
            }
            catch ( System.Exception ex )
            {
                proInfo.SetValue( obj , value , null );
            }


        }

        public static object CreateInstanceObject ( Type typeObj )
        {
            if ( typeObj==null )
                return null;
            try
            {
                InstantiateObjectHandler instantiateObjectHandler=null;
                lstInitHandler.TryGetValue( typeObj.FullName , out instantiateObjectHandler );
                if ( instantiateObjectHandler==null )
                {
                    instantiateObjectHandler=ABCDynamicMethodCompiler.CreateInstantiateObjectHandler( typeObj );
                    lstInitHandler.Add( typeObj.FullName , instantiateObjectHandler );
                }

                return instantiateObjectHandler();
            }
            catch ( System.Exception ex )
            {
                try
                {
                    return typeObj.InvokeMember( "" , BindingFlags.CreateInstance , null , null , null );
                }
                catch ( System.Exception ex2 )
                {
                }
            }
            return null;

        }
    }

}