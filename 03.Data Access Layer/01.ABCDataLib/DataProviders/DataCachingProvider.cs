using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Runtime.Caching;
using System.Text;
using System.Linq;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;
using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace ABCProvider
{
    public class DataCachingProvider
    {

        public static void InitTableCachings ( )
        {
            if ( Application.ExecutablePath.Contains( "AutoGenDLL" ) )
                return;

            InitLookupTables();

            InitRefreshLookupTableMethods();
        }

        #region LookupTables

        static Dictionary<String , DateTime> LastUpdateTimes=new Dictionary<string , DateTime>();
        public static Dictionary<String , DataTable> LookupTables=new Dictionary<string , DataTable>();

        public static void InitLookupTables ( )
        {
            MemoryCache cache=MemoryCache.Default;
            LookupTables=cache["ABCLookupTables"] as Dictionary<String , DataTable>;
            if ( LookupTables==null )
            {
                LookupTables=new Dictionary<string , DataTable>();
                foreach ( String strTableName in DataConfigProvider.TableConfigList.Keys )
                {
                    if ( DataConfigProvider.TableConfigList[strTableName].IsCaching )
                        LoadLookupTable( strTableName );
                }
                GC.Collect();

                CacheItemPolicy policy=new CacheItemPolicy();
                policy.SlidingExpiration=new TimeSpan( 0 , 30 , 0 );

                cache.Add( "ABCLookupTables" , LookupTables , policy );
            }
            if ( RefreshLookupTableTimer==null )
            {
                RefreshLookupTableTimer=new System.Timers.Timer();
                RefreshLookupTableTimer.Interval=7000;
                RefreshLookupTableTimer.Elapsed+=new System.Timers.ElapsedEventHandler( AutoRefreshLookupTables );
                RefreshLookupTableTimer.Start();
            }
        }
        static System.Timers.Timer RefreshLookupTableTimer=null;
        static void AutoRefreshLookupTables ( object sender , System.Timers.ElapsedEventArgs e )
        {
            RefreshLookupTables();
        }

        private static void LoadLookupTable ( String strTableName )
        {
            if ( LookupTables.ContainsKey( strTableName )==false&&LastUpdateTimes.ContainsKey( strTableName )==false )
            {
                BusinessObjectController Ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
                if ( Ctrl!=null )
                {
                    DataSet ds=Ctrl.GetDataSetAllObjects();
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        LookupTables.Add( strTableName , ds.Tables[0] );

                        if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colUpdateTime ) )
                            LastUpdateTimes.Add( strTableName , TimeProvider.GetTableLastUpdateTime( strTableName ) );
                    }
                }
            }
        }

        public static void ReloadLookupTable ( String strTableName )
        {
            lock ( LookupTables )
            {
                LoadLookupTable( strTableName );
                DataTable lookupTable=LookupTables[strTableName];

                DataSet ds=BusinessControllerFactory.GetBusinessController( strTableName ).GetDataSetAllObjects();
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    lookupTable.Rows.Clear();
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        lookupTable.ImportRow( dr );
                }
            }
        }
        public static void RefreshLookupTables ( )
        {
            List<String> lstTables=new List<string>();
            foreach ( string strTableName in LookupTables.Keys )
                lstTables.Add( strTableName );

            foreach ( string strTableName in lstTables )
                RefreshLookupTable( strTableName );

            GC.Collect();
        }
        public static void RefreshLookupTable ( String strTableName )
        {
            bool isUpdate=false;
            lock ( LookupTables )
            {
                LoadLookupTable( strTableName );

                if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colUpdateTime )
                    &&LastUpdateTimes.ContainsKey( strTableName ) )
                {
                    #region Has 'UpdateTime'
                    DateTime lastTimeInDB=TimeProvider.GetTableLastUpdateTime( strTableName );
                    DataTable lookupTable=LookupTables[strTableName];
                    DateTime lastTimeInApp=LastUpdateTimes[strTableName];

                    if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colABCStatus ) )
                    {
                        #region Has 'ABCStatus'
                        if ( lastTimeInDB>lastTimeInApp )
                        {
                            #region Refresh Modified Items
                            String strQuery=String.Format( @"SELECT * FROM {0} WHERE ABCStatus ='Alive' AND {1}" , strTableName , TimeProvider.GenCompareDateTime( ABCCommon.ABCConstString.colUpdateTime , ">" , lastTimeInApp ) );
                            DataSet ds=DataQueryProvider.CompanyDatabaseHelper.RunQuery( strQuery );
                            if ( ds!=null&&ds.Tables.Count>0 &&ds.Tables[0].Rows.Count>0)
                            {
                                BusinessObjectController controller=BusinessControllerFactory.GetBusinessController( strTableName );
                                String strPK=DataStructureProvider.GetPrimaryKeyColumn( strTableName );
                                foreach ( DataRow row in ds.Tables[0].Rows )
                                {
                                    #region Row
                                    BusinessObject obj=(BusinessObject)controller.GetObjectFromDataRow( row );
                                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , strPK ) );

                                    DataRow[] rows=lookupTable.Select( String.Format( "{0}='{1}'" , strPK , iID ) );
                                    if ( rows.Length==0 ) // new
                                    {
                                        lookupTable.ImportRow( row );
                                    }
                                    else
                                    {
                                        DataRow dr=rows[0];
                                        int index=lookupTable.Rows.IndexOf( dr );
                                        if ( DataStructureProvider.IsExistABCStatus( obj.AATableName )&&ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colABCStatus ).ToString().Equals( ABCCommon.ABCConstString.ABCStatusDeleted ) )//delete
                                            lookupTable.Rows.RemoveAt( index );
                                        else //update
                                        {
                                            object[] lstArr=new object[row.ItemArray.Length];
                                            row.ItemArray.CopyTo( lstArr , 0 );
                                            lookupTable.Rows[index].ItemArray=lstArr;
                                        }
                                    }
                                    #endregion
                                }

                                isUpdate=true;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Without 'ABCStatus'
                        if ( lastTimeInDB>lastTimeInApp||TimeProvider.GetRecordCountOfTable( strTableName )!=lookupTable.Rows.Count )
                        {
                            DataSet ds=BusinessControllerFactory.GetBusinessController( strTableName ).GetDataSetAllObjects();
                            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                            {
                                lookupTable.Rows.Clear();
                                foreach ( DataRow dr in ds.Tables[0].Rows )
                                    lookupTable.ImportRow( dr );

                                isUpdate=true;
                            }
                        }
                        #endregion
                    }

                    LastUpdateTimes[strTableName]=lastTimeInDB;
                    #endregion
                }
                else
                {
                    #region Without 'UpdateTime'
                    DataTable lookupTable=LookupTables[strTableName];
                    if ( TimeProvider.GetRecordCountOfTable( strTableName )!=lookupTable.Rows.Count )
                    {
                        DataSet ds=BusinessControllerFactory.GetBusinessController( strTableName ).GetDataSetAllObjects();
                        if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                        {
                            lookupTable.Rows.Clear();
                            foreach ( DataRow dr in ds.Tables[0].Rows )
                                lookupTable.ImportRow( dr );

                            isUpdate=true;
                        }
                    }
                    #endregion
                }
            }

            if ( isUpdate&&RefreshTableMethods.ContainsKey( strTableName ) )
            {
                foreach ( MethodInfo method in RefreshTableMethods[strTableName] )
                    method.Invoke( null , new object[] {});
            }
        }

        public static Dictionary<String , List<MethodInfo>> RefreshTableMethods;

        public static void InitRefreshLookupTableMethods ( )
        {
            if ( RefreshTableMethods==null )
            {
                RefreshTableMethods=new Dictionary<string , List<MethodInfo>>();
          
                AppDomain domain=AppDomain.CreateDomain( "ABCRefreshTable" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCDataLib.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCProvider.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCAppProvider.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCSystemModules.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCAppModules.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCBaseApp.dll" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCApp.exe" );
                InitRefreshLookupTableAtributes( domain , Application.StartupPath+"\\ABCStudio.exe" );
                AppDomain.Unload( domain );
            }
        }
        public static void InitRefreshLookupTableAtributes ( AppDomain domain , String strAssFileName )
        {
            if ( RefreshTableMethods==null )
                RefreshTableMethods=new Dictionary<string , List<MethodInfo>>();

            try
            {
                Assembly assembly=domain.Load( AssemblyName.GetAssemblyName( strAssFileName ) );
                if ( assembly==null )
                    return;

                var methods=assembly.GetTypes()
                       .SelectMany( t => t.GetMethods() )
                       .Where( m => m.GetCustomAttributes( typeof( ABCRefreshTable ) ,false ).Length>0 )
                       .ToArray();

                foreach ( MethodInfo method in methods )
                {
                    foreach ( ABCRefreshTable attribute in method.GetCustomAttributes( typeof( ABCRefreshTable ) , false ) )
                    {
                        foreach ( String strTableName in attribute.TableNames )
                        {
                            if ( !RefreshTableMethods.ContainsKey( strTableName ) )
                                RefreshTableMethods.Add( strTableName , new List<MethodInfo>() );

                            RefreshTableMethods[strTableName].Add( method );
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
            }

        }

        #endregion

        public static DataView TryToGetDataView ( String strTableName , Boolean isCreateView )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return null;

            //  RefreshLookupTable( strTableName );
            LoadLookupTable( strTableName );

            DataTable table;

            if ( LookupTables.TryGetValue( strTableName , out table ) )
            {
                if ( isCreateView )
                    return new DataView( table );
                else
                    return table.DefaultView;
            }

            return null;
        }

        public static BusinessObject GetCachedBusinessObject ( String strTableName , Guid iID )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return null;

            if ( !DataStructureProvider.IsExistedTable( strTableName ) )
                return null;

            DataView view=TryToGetDataView( strTableName , false );
            DataRow[] rows=view.Table.Select( String.Format( "{0} = '{1}'" , DataStructureProvider.GetPrimaryKeyColumn( strTableName ) , iID ) );
            if ( rows.Length<=0 )
                return null;

            return BusinessControllerFactory.GetBusinessController( strTableName ).GetObjectFromDataRow( rows[0] );
        }

        #region AccrossStructInfo
        public class AccrossStructInfo
        {
            public String FieldName;
            public String TableName;
            public Guid TableID;
        }
        public static object GetCachingObjectAccrossTable ( BusinessObject baseObj , Guid iFieldValue , String strFieldString )
        {
            if ( strFieldString.Contains( ":" )&&strFieldString.Split( ':' )[0]=="ID"&&DataStructureProvider.IsTableColumn( baseObj.AATableName , "TableName" ) )
            {
                object strIDTableName=ABCDynamicInvoker.GetValue( baseObj , "TableName" );
                if ( strIDTableName!=null )
                    return GetCachingObjectAccrossTable( baseObj.AATableName , iFieldValue , strFieldString , strIDTableName.ToString() );
            }
            return GetCachingObjectAccrossTable( baseObj.AATableName , iFieldValue , strFieldString );
        }
        public static object GetCachingObjectAccrossTable ( String strTableName , Guid iFieldValue , String strFieldString )
        {
            return GetCachingObjectAccrossTable( strTableName , iFieldValue , strFieldString , String.Empty );
        }
        public static object GetCachingObjectAccrossTable ( String strTableName , Guid iFieldValue , String strFieldString , String strIDTableName )
        {
            AccrossStructInfo info=GetAccrossStructInfo( strTableName , iFieldValue , strFieldString , strIDTableName );

            if ( info!=null&&DataStructureProvider.IsTableColumn( info.TableName , info.FieldName ) )
            {
                BusinessObject objResult=GetCachedBusinessObject( info.TableName , info.TableID );
                return ABCBusinessEntities.ABCDynamicInvoker.GetValue( objResult , info.FieldName );
            }

            return null;
        }

        public static object GetCachingObjectDisplayAccrossTable ( BusinessObject baseObj , Guid iFieldValue , String strFieldString )
        {
            if ( strFieldString.Contains( ":" )&&strFieldString.Split( ':' )[0]=="ID"&&DataStructureProvider.IsTableColumn( baseObj.AATableName , "TableName" ) )
            {
                object strIDTableName=ABCDynamicInvoker.GetValue( baseObj , "TableName" );
                if ( strIDTableName!=null )
                    return GetCachingObjectDisplayAccrossTable( baseObj.AATableName , iFieldValue , strFieldString , strIDTableName.ToString() );
            }
            return GetCachingObjectDisplayAccrossTable( baseObj.AATableName , iFieldValue , strFieldString );
        }
        public static object GetCachingObjectDisplayAccrossTable ( String strTableName , Guid iFieldValue , String strFieldString )
        {
            return GetCachingObjectDisplayAccrossTable( strTableName , iFieldValue , strFieldString , String.Empty );
        }
        public static object GetCachingObjectDisplayAccrossTable ( String strTableName , Guid iFieldValue , String strFieldString , String strIDTableName )
        {
            AccrossStructInfo info=GetAccrossStructInfo( strTableName , iFieldValue , strFieldString , strIDTableName );

            if ( info!=null&&DataStructureProvider.IsTableColumn( info.TableName , info.FieldName ) )
            {
                BusinessObject objResult=GetCachedBusinessObject( info.TableName , info.TableID );
                object obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objResult , info.FieldName );

                String strEnum=DataConfigProvider.TableConfigList[info.TableName].FieldConfigList[info.FieldName].AssignedEnum;
                if ( EnumProvider.EnumList.ContainsKey( strEnum )
                    &&EnumProvider.EnumList[strEnum].Items.ContainsKey( obj.ToString() ) )
                {
                    return EnumProvider.EnumList[strEnum].Items[obj.ToString()].CaptionVN;
                }

                return obj;
            }
            return null;
        }

        public static AccrossStructInfo GetAccrossStructInfo ( BusinessObject baseObj , Guid iFieldValue , String strFieldString )
        {
            if ( strFieldString.Contains( ":" )&&strFieldString.Split( ':' )[0]=="ID"&&DataStructureProvider.IsTableColumn( baseObj.AATableName , "TableName" ) )
            {
                object strIDTableName=ABCDynamicInvoker.GetValue( baseObj , "TableName" );
                if ( strIDTableName!=null )
                    return GetAccrossStructInfo( baseObj.AATableName , iFieldValue , strFieldString , strIDTableName.ToString() );
            }
            return GetAccrossStructInfo( baseObj.AATableName , iFieldValue , strFieldString );
        }
        public static AccrossStructInfo GetAccrossStructInfo ( String strTableName , Guid iFieldValue , String strFieldString )
        {
            return GetAccrossStructInfo( strTableName , iFieldValue , strFieldString , String.Empty );
        }
        public static AccrossStructInfo GetAccrossStructInfo ( String strTableName , Guid iFieldValue , String strFieldString , String strIDTableName )
        {

            if ( String.IsNullOrWhiteSpace( strFieldString ) )
                return null;

            String[] strArr=strFieldString.Split( ':' );

            if ( DataStructureProvider.IsForeignKey( strTableName , strArr[0] )==false&&strArr[0]!="ID" )
                return null;

            AccrossStructInfo result=new AccrossStructInfo();
            if ( strArr[0]=="ID"&&!String.IsNullOrWhiteSpace( strIDTableName ) )
                result.TableName=strIDTableName;
            else
                result.TableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , strArr[0] );

            result.TableID=iFieldValue;
            result.FieldName=String.Empty;

            if ( !DataStructureProvider.IsExistedTable( result.TableName ) )
                return null;

            BusinessObject objTable=GetCachedBusinessObject( result.TableName , result.TableID );
            if ( objTable==null )
                return null;

            for ( int i=1; i<strArr.Length; i++ )
            {
                result.FieldName=strArr[i];
                if ( result.FieldName=="ID"&&DataStructureProvider.IsTableColumn( result.TableName , "TableName" ) )
                {
                    object objTemp=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objTable , "TableName" );
                    if ( objTemp==null )
                        break;
                    result.TableName=objTemp.ToString();

                    objTemp=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objTable , result.FieldName );
                    if ( objTemp==null )
                        break;
                    result.TableID=ABCHelper.DataConverter.ConvertToGuid( objTemp );
                }
                else
                {

                    if ( DataStructureProvider.IsForeignKey( result.TableName , result.FieldName )==false )
                        break;

                    result.TableName=DataStructureProvider.GetTableNameOfForeignKey( result.TableName , result.FieldName );
                    object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objTable , result.FieldName );
                    if ( objValue==null )
                        break;

                    result.TableID=ABCHelper.DataConverter.ConvertToGuid( objValue );
                    objTable=GetCachedBusinessObject( result.TableName , result.TableID );
                    if ( objTable==null )
                        break;
                }
                result.FieldName=String.Empty;
            }

            if ( DataStructureProvider.IsForeignKey( result.TableName , result.FieldName )||result.FieldName==String.Empty )
            {
                if ( DataStructureProvider.IsForeignKey( result.TableName , result.FieldName ) )
                    result.TableName=DataStructureProvider.GetTableNameOfForeignKey( result.TableName , result.FieldName );

                result.FieldName=DataStructureProvider.GetDisplayColumn( result.TableName );
            }

            if ( String.IsNullOrWhiteSpace( result.FieldName ) )
                result.FieldName=DataStructureProvider.GetDisplayColumn( result.TableName );
            if ( String.IsNullOrWhiteSpace( result.FieldName ) )
                result.FieldName=DataStructureProvider.GetPrimaryKeyColumn( result.TableName );

            return result;
        }
        public static AccrossStructInfo GetAccrossStructInfo ( String strTableName , String strFieldString )
        {
            AccrossStructInfo result=new AccrossStructInfo();
            result.FieldName=strFieldString;
            result.TableName=strTableName;

            String[] strArr=strFieldString.Split( ':' );
            if ( strArr.Length<=1 )
                return result;

            result.FieldName=strArr[0];
            result.TableName=strTableName;

            for ( int i=1; i<strArr.Length; i++ )
            {
                if ( DataStructureProvider.IsForeignKey( result.TableName , result.FieldName )==false )
                    break;

                result.TableName=DataStructureProvider.GetTableNameOfForeignKey( result.TableName , result.FieldName );
                result.FieldName=strArr[i];
            }

            if ( DataStructureProvider.IsForeignKey( result.TableName , result.FieldName ) )
            {
                result.TableName=DataStructureProvider.GetTableNameOfForeignKey( result.TableName , result.FieldName );
                result.FieldName=DataStructureProvider.GetDisplayColumn( result.TableName );
            }

            return result;
        }

        #endregion
    }

    [System.AttributeUsage( System.AttributeTargets.Method )]
    public class ABCRefreshTable : System.Attribute
    {
        public string[] TableNames;

        public ABCRefreshTable ( params string[] tableList )
        {
            this.TableNames=tableList;
        }
    }

}
