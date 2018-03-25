using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
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
using ABCProvider;

namespace ABCBusinessEntities
{
    public static class BusinessObjectHelper
    {

        #region Util

        public static Dictionary<String , PropertyInfo> BasePropertyList=new Dictionary<String , PropertyInfo>();
        public static Dictionary<String , Dictionary<String , PropertyInfo>> PropertyList=new Dictionary<String , Dictionary<String , PropertyInfo>>();
        public static Dictionary<String , BusinessObject> TemplateObjectList=new Dictionary<string , BusinessObject>();

        public static void InitBasePropertyList ( )
        {
            if ( BasePropertyList.Count<=0 )
            {
                PropertyInfo[] properties=typeof( BusinessObject ).GetProperties();
                foreach ( PropertyInfo prop in properties )
                    BasePropertyList.Add( prop.Name , prop );
            }
        }
        public static bool IsBaseProperty ( String strPropertyName )
        {
            InitBasePropertyList();
            return BasePropertyList.ContainsKey( strPropertyName );
        }

        public static void InitPropertyList ( String strTableName )
        {
            InitBasePropertyList();

            if ( PropertyList.ContainsKey( strTableName )==false )
            {
                Dictionary<String , PropertyInfo> innerList=new Dictionary<string , PropertyInfo>();
                BusinessObject obj=BusinessObjectFactory.GetBusinessObject( strTableName );
                if ( obj==null )
                    return;

                PropertyInfo[] properties=obj.GetType().GetProperties();
                foreach ( PropertyInfo prop in properties )
                {
                    if ( IsBaseProperty( prop.Name )==false )
                        innerList.Add( prop.Name , prop );
                }
                if ( innerList.Count>0 )
                    PropertyList.Add( strTableName , innerList );
            }
        }
        public static bool IsProperty ( String strTableName , String strPropertyName )
        {
            InitPropertyList( strTableName );
            if ( PropertyList.ContainsKey( strTableName )==false )
                return false;

            return PropertyList[strTableName].ContainsKey( strPropertyName );
        }

        public static PropertyInfo GetProperty ( String strTableName , String strPropertyName )
        {
            Dictionary<String , PropertyInfo> lstInner=null;
            if ( PropertyList.TryGetValue( strTableName , out lstInner )==false )
            {
                InitPropertyList( strTableName );
                if ( PropertyList.TryGetValue( strTableName , out lstInner )==false )
                    return null;
            }

            PropertyInfo property=null;
            lstInner.TryGetValue( strPropertyName , out property );
            return property;
        }
        #endregion

        public static BusinessObject GetBusinessObject ( DataSet ds , Type type )
        {
            return GetBusinessObject( ds , type.Name.Substring( 0 , type.Name.Length-4 ) );
        }
        public static BusinessObject GetBusinessObject ( DataSet ds , String strTableName )
        {
            try
            {
                if ( ds==null||ds.Tables.Count<=0||ds.Tables[0].Rows.Count<=0 )
                    return null;

                return GetBusinessObject( ds.Tables[0].Rows[0] , strTableName );
            }
            catch ( Exception )
            {
                return null;
            }
        }
        public static BusinessObject GetBusinessObject ( DataRow row , String strTableName )
        {
            if ( row==null )
                return null;

            InitPropertyList( strTableName );
            Dictionary<String , PropertyInfo> lstInner=null;
            if ( PropertyList.TryGetValue( strTableName , out lstInner )==false )
                return null;

            #region ABCDynamicInvoker
            BusinessObject obj=BusinessObjectFactory.GetBusinessObject( strTableName );
            foreach ( DataColumn column in row.Table.Columns )
            {
                object objValue=row[column];
                if ( objValue==System.DBNull.Value )
                    continue;
                if ( objValue.GetType()==typeof( Int64 ) )
                    objValue=Convert.ToInt32( objValue );

                PropertyInfo property=null;
                lstInner.TryGetValue( column.ColumnName , out property );
                if ( property!=null )
                    ABCDynamicInvoker.SetValue( obj , property , objValue );

            }
            return obj;
            #endregion

        }
        public static BusinessObject GetBusinessObject ( DataRow row , Type type )
        {
            return GetBusinessObject( row , type.Name.Substring( 0 , type.Name.Length-4 ) );
        }
        public static BusinessObject GetBusinessObject ( BusinessObject sourceObj , String strTableName )
        {
            BusinessObject obj=BusinessObjectFactory.GetBusinessObject( strTableName );
            obj.GetFromBusinessObject( sourceObj );
            return obj;
        }

        public static BusinessObject GetBusinessObject ( String strTableName , Guid iID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
                return ctrl.GetObjectByID( iID );
            return null;
        }
        public static BusinessObject GetBusinessObject ( String strTableName , String strNoValue )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
                return ctrl.GetObjectByNo( strNoValue );
            return null;
        }

        public static void SetIDValue ( BusinessObject obj , Guid iID )
        {
            string strPrimaryKeyColumn=DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName );
            ABCDynamicInvoker.SetValue( obj , strPrimaryKeyColumn , iID );

        }
        public static Guid GetIDValue ( BusinessObject obj )
        {
            if ( obj==null )
                return Guid.Empty;

            string strPrimaryKeyColumn=DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName );
            return ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , strPrimaryKeyColumn ) );
        }
        public static void SetNOValue ( BusinessObject obj , String strNo )
        {
            string strNOCol=DataStructureProvider.GetNOColumn( obj.AATableName );
            ABCDynamicInvoker.SetValue( obj , strNOCol , strNo );
        }

        public static bool GenerateNoColumn ( BusinessObject obj , bool isAlwayGenerate )
        {
            String strNoCol=DataStructureProvider.GetNOColumn( obj.AATableName );
            if ( !String.IsNullOrWhiteSpace( strNoCol ) )
            {
                if ( isAlwayGenerate||String.IsNullOrWhiteSpace( ABCDynamicInvoker.GetValue( obj , strNoCol ).ToString() ) )
                {
                    String strNo=NumberingProvider.GenerateNo( obj );
                    if ( !String.IsNullOrWhiteSpace( strNo ) )
                    {
                        ABCDynamicInvoker.SetValue( obj , strNoCol , strNo );
                        return true;
                    }
                }
            }
            return false;
        }

        public static void SetDefaultValue ( BusinessObject obj )
        {
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colDocumentDate ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colDocumentDate , ABCApp.ABCDataGlobal.WorkingDate );
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , "DocumentExpireDate" ) )
                ABCDynamicInvoker.SetValue( obj , "DocumentExpireDate" , ABCApp.ABCDataGlobal.WorkingDate.AddMonths( 1 ) );
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colVoucherDate ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colVoucherDate , ABCApp.ABCDataGlobal.WorkingDate );

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colApprovedDate ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colApprovedDate , null );

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colJournalStatus ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colJournalStatus , ABCCommon.ABCConstString.PostStatusNew );
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colJournalDate ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colJournalDate , ABCApp.ABCDataGlobal.WorkingDate );

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colLockStatus ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colLockStatus , ABCCommon.ABCConstString.LockStatusNew );

            obj.SetNoValue( String.Empty );

            CurrencyProvider.GenerateCurrencyValue( obj );
        }
        public static bool SetAutoValue ( BusinessObject obj )
        {
            bool isModified=false;

            #region Edit Count
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colEditCount ) )
            {
                int iCount=-1;
                object objCount=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colEditCount );
                if ( objCount!=null&&objCount!=DBNull.Value )
                    iCount=Convert.ToInt32( objCount );
                iCount++;

                object objOldValue=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colEditCount );
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colEditCount , iCount );
                if ( Convert.ToInt32( objOldValue )!=iCount )
                    isModified=true;
            }
            #endregion

            isModified=isModified||CurrencyProvider.GenerateCurrencyValue( obj );
            return isModified;
        }

        public static String GetNoValue ( String strTableName , Guid? iID )
        {
            if ( iID.HasValue )
                return GetNoValue( strTableName , iID.Value );
            return String.Empty;
        }
        public static String GetNoValue ( String strTableName , Guid iID )
        {
            BusinessObject obj=GetBusinessObject( strTableName , iID );
            return GetNoValue( obj );
        }
        public static String GetNoValue ( BusinessObject obj )
        {
            if ( obj==null )
                return String.Empty;

            string strNo=DataStructureProvider.GetNOColumn( obj.AATableName );
            if ( String.IsNullOrWhiteSpace( strNo ) )
                strNo=DataStructureProvider.GetNAMEColumn( obj.AATableName );

            object objReturn=ABCDynamicInvoker.GetValue( obj , strNo );
            if ( objReturn!=null )
                return objReturn.ToString();

            return String.Empty;
        }
        public static String GetNameValue ( String strTableName , Guid? iID )
        {
            if ( iID.HasValue )
                return GetNameValue( strTableName , iID.Value );
            return String.Empty;
        }
        public static String GetNameValue ( String strTableName , Guid iID )
        {
            BusinessObject obj=GetBusinessObject( strTableName , iID );
            return GetNameValue( obj );
        }
        public static String GetNameValue ( BusinessObject obj )
        {
            if ( obj==null )
                return String.Empty;

            string strName=DataStructureProvider.GetNAMEColumn( obj.AATableName );
            if ( String.IsNullOrWhiteSpace( strName ) )
                strName=DataStructureProvider.GetNOColumn( obj.AATableName );

            object objReturn=ABCDynamicInvoker.GetValue( obj , strName );
            if ( objReturn!=null )
                return objReturn.ToString();

            return String.Empty;
        }
        public static String GetDisplayValue ( BusinessObject obj )
        {
            if ( obj==null )
                return null;

            string strCol=DataStructureProvider.GetDisplayColumn( obj.AATableName );
            object objReturn=ABCDynamicInvoker.GetValue( obj , strCol );
            if ( objReturn!=null )
                return objReturn.ToString();

            return "";
        }
        public static String GetRemarkValue ( BusinessObject obj )
        {
            if ( obj==null )
                return null;

            object objReturn=ABCDynamicInvoker.GetValue( obj , "Remark" );
            if ( objReturn!=null )
                return objReturn.ToString();

            return "";
        }
        public static List<object> GetListDataByColumn ( List<BusinessObject> lstObjects , String strFieldName )
        {
            List<object> lstReturns=new List<object>();
            foreach ( BusinessObject obj in lstObjects )
            {
                object objValue=ABCDynamicInvoker.GetValue( obj , strFieldName );
                if ( objValue!=null&&lstReturns.Contains( objValue )==false )
                    lstReturns.Add( objValue );
            }
            return lstReturns;
        }


        public static bool CopyObject ( BusinessObject objFrom , BusinessObject objTo , Boolean isCleanFieldOnly )
        {
            if ( objFrom==null||objTo==null )
                return false;

            bool isCopied=false;
            String strFromName=objFrom.AATableName;
            String strToName=objTo.AATableName;

            BusinessObjectHelper.InitPropertyList( strToName );
            foreach ( PropertyInfo propTo in BusinessObjectHelper.PropertyList[strToName].Values )
            {
                if ( DataStructureProvider.IsPrimaryKey( strToName , propTo.Name )
                    ||DataStructureProvider.IsNOColumn( strToName , propTo.Name ) )
                    continue;

                PropertyInfo propFrom=BusinessObjectHelper.GetProperty( strFromName , propTo.Name );
                if ( propFrom!=null )
                {
                    if ( isCleanFieldOnly==false||( isCleanFieldOnly&&IsCleanField( objTo , propTo.Name ) ) )
                    {
                        object objValue=ABCDynamicInvoker.GetValue( objFrom , propFrom );
                        object objOldValue=ABCDynamicInvoker.GetValue( objTo , propTo );
                        ABCDynamicInvoker.SetValue( objTo , propTo , objValue );
                        if ( objOldValue!=objValue )
                            isCopied=true;
                    }
                }
            }
            return isCopied;
        }
        public static bool CopyField ( BusinessObject objFrom , BusinessObject objTo , String strField , Boolean isCleanFieldOnly )
        {
            if ( objFrom==null||objTo==null )
                return false;

            if ( isCleanFieldOnly==false||( isCleanFieldOnly&&IsCleanField( objTo , strField ) ) )
            {
                object objValue=ABCDynamicInvoker.GetValue( objFrom , strField );
                if ( objValue!=null )
                {
                    object objOldValue=ABCDynamicInvoker.GetValue( objTo , strField );
                    ABCDynamicInvoker.SetValue( objTo , strField , objValue );
                    if ( objOldValue!=objValue )
                        return true;
                }
            }
            return false;
        }
        public static bool CopyField ( BusinessObject objFrom , BusinessObject objTo , String strFromField , String strToField , Boolean isCleanFieldOnly )
        {
            if ( objFrom==null||objTo==null )
                return false;

            if ( isCleanFieldOnly==false||( isCleanFieldOnly&&IsCleanField( objTo , strToField ) ) )
            {
                object objValue=ABCDynamicInvoker.GetValue( objFrom , strFromField );
                if ( objValue!=null )
                {
                    object objOldValue=ABCDynamicInvoker.GetValue( objTo , strToField );
                    ABCDynamicInvoker.SetValue( objTo , strToField , objValue );
                    if ( objOldValue!=objValue )
                        return true;
                }
            }
            return false;
        }

        public static void CopyFKFields ( BusinessObject sourceObj , String strItemTableName )
        {
            if ( sourceObj==null )
                return;

            String strFK=DataStructureProvider.GetForeignKeyOfTableName(strItemTableName,sourceObj.AATableName);
            if ( String.IsNullOrWhiteSpace( strFK ) )
                return;


            String strQuery=String.Format( "UPDATE {0} SET " , strItemTableName );

            Dictionary<String , object> lstCols=new Dictionary<string , object>();
            foreach ( PropertyInfo pro in BusinessObjectHelper.PropertyList[sourceObj.AATableName].Values )
            {
                if ( DataStructureProvider.IsTableColumn( strItemTableName , pro.Name )==false )
                    continue;

                if ( DataStructureProvider.IsForeignKey( sourceObj.AATableName , pro.Name ) )
                    lstCols.Add( pro.Name , ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( sourceObj , pro ) ) );

                if ( pro.Name==ABCCommon.ABCConstString.colApprovalStatus
                     ||pro.Name==ABCCommon.ABCConstString.colApprovedDate
                     ||pro.Name==ABCCommon.ABCConstString.colDocumentDate
                     ||pro.Name==ABCCommon.ABCConstString.colLockStatus
                     ||pro.Name==ABCCommon.ABCConstString.colVoucher
                     ||pro.Name==ABCCommon.ABCConstString.colVoucherDate
                     ||pro.Name==ABCCommon.ABCConstString.colJournalStatus
                     ||pro.Name==ABCCommon.ABCConstString.colJournalDate
                     ||pro.Name==ABCCommon.ABCConstString.colUpdateTime
                     ||pro.Name==ABCCommon.ABCConstString.colUpdateUser )
                    lstCols.Add( pro.Name , ABCDynamicInvoker.GetValue( sourceObj , pro ) );

            }
 
            int i=-1;
            foreach ( String strKey in lstCols.Keys )
            {
                i++;
                if ( lstCols[strKey]!=null&&lstCols[strKey]!=DBNull.Value )
                {
                    String strValue=lstCols[strKey].ToString();
                    if ( lstCols[strKey] is DateTime )
                        strValue=( (DateTime)lstCols[strKey] ).ToString( "yyyy-MM-dd HH:mm:ss" );
                    if ( lstCols[strKey] is Nullable<DateTime> )
                        strValue=( (Nullable<DateTime>)lstCols[strKey] ).Value.ToString( "yyyy-MM-dd HH:mm:ss" );

                    if ( i<lstCols.Count-1 )
                        strQuery=strQuery+String.Format( " [{0}] = '{1}'," , strKey , strValue );
                    else
                        strQuery=strQuery+String.Format( " [{0}] = '{1}'" , strKey , strValue );
                }
                else
                {
                    if ( i<lstCols.Count-1 )
                        strQuery=strQuery+String.Format( " [{0}] = NULL," , strKey );
                    else
                        strQuery=strQuery+String.Format( " [{0}] =NULL" , strKey );
                }
            }
            strQuery=strQuery+String.Format( " WHERE [{0}] ='{1}' " , strFK , sourceObj.GetID() );
            BusinessObjectController.RunQuery( strQuery );
        }
        public static bool CopyFKFields ( BusinessObject objFrom , BusinessObject objTo , Boolean isCleanFieldOnly )
        {
            if ( objFrom==null||objTo==null )
                return false;

            bool isCopied=false;

            String strFromName=objFrom.AATableName;
            String strToName=objTo.AATableName;

            BusinessObjectHelper.InitPropertyList( strToName );
            foreach ( PropertyInfo propTo in BusinessObjectHelper.PropertyList[strToName].Values )
            {
                if ( DataStructureProvider.IsForeignKey( strToName , propTo.Name )==false )
                    continue;

                PropertyInfo propFrom=BusinessObjectHelper.GetProperty( strFromName , propTo.Name );
                if ( propFrom!=null )
                {
                    if ( isCleanFieldOnly==false||( isCleanFieldOnly&&IsCleanField( objTo , propTo.Name ) ) )
                    {
                        object objValue=ABCDynamicInvoker.GetValue( objFrom , propFrom );
                        object objOldValue=ABCDynamicInvoker.GetValue( objTo , propTo );
                        ABCDynamicInvoker.SetValue( objTo , propTo , objValue );
                        if ( objOldValue!=objValue )
                            isCopied=true;
                    }
                }
            }
            return isCopied;
        }

        public static bool IsModifiedObject ( BusinessObject obj )
        {
            String strPKCol=DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName );
            object objID=ABCDynamicInvoker.GetValue( obj , strPKCol );
            if ( objID==null||objID==DBNull.Value )
                return true;

            Guid id=Guid.Empty;
            if ( objID is Guid )
                id=(Guid)objID;
            else if ( objID is Nullable<Guid>&&( (Nullable<Guid>)objID ).HasValue )
                id=( (Nullable<Guid>)objID ).Value;
            else
                return true;


            BusinessObject obj2=BusinessControllerFactory.GetBusinessController( obj.AATableName ).GetObjectByID( id );

            InitPropertyList( obj.AATableName );
            foreach ( PropertyInfo proInfo in PropertyList[obj.AATableName].Values )
            {
                object pro1=proInfo.GetValue( obj , null );
                object pro2=proInfo.GetValue( obj2 , null );
                if ( pro1==pro2||( pro1==null&&pro2==null )||( pro1!=null&&pro2!=null&&pro1.ToString()==pro2.ToString() ) )
                    continue;

                return true;
            }

            return false;
        }

        public static bool IsCleanObject ( BusinessObject obj )
        {
            if ( TemplateObjectList.ContainsKey( obj.AATableName )==false )
                TemplateObjectList.Add( obj.AATableName , (BusinessObject)ABCDynamicInvoker.CreateInstanceObject( obj.GetType() ) );

            InitPropertyList( obj.AATableName );
            BusinessObject objTemplate=TemplateObjectList[obj.AATableName];
            if ( objTemplate!=null )
            {
                foreach ( PropertyInfo proInfo in PropertyList[obj.AATableName].Values )
                {
                    object obj1=proInfo.GetValue( obj , null );
                    object obj2=proInfo.GetValue( objTemplate , null );
                    if ( obj1==obj2||( obj1==null&&obj2==null )||( obj1!=null&&obj2!=null&&obj1.ToString()==obj2.ToString() ) )
                        continue;

                    return false;
                }
            }

            return true;
        }
        public static bool IsCleanField ( BusinessObject obj , String strFieldName )
        {
            if ( TemplateObjectList.ContainsKey( obj.AATableName )==false )
                TemplateObjectList.Add( obj.AATableName , (BusinessObject)ABCDynamicInvoker.CreateInstanceObject( obj.GetType() ) );

            InitPropertyList( obj.AATableName );
            BusinessObject objTemplate=TemplateObjectList[obj.AATableName];
            if ( objTemplate!=null&&PropertyList[obj.AATableName].ContainsKey( strFieldName ) )
            {
                PropertyInfo proInfo=PropertyList[obj.AATableName][strFieldName];

                object obj1=proInfo.GetValue( obj , null );
                object obj2=proInfo.GetValue( objTemplate , null );
                if ( obj1==obj2||( obj1==null&&obj2==null )||( obj1!=null&&obj2!=null&&obj1.ToString()==obj2.ToString() ) )
                    return true;
            }
            return false;
        }

        public static bool IsExistObject ( BusinessObject obj )
        {
            return IsExistObject( obj.AATableName , obj.GetID() );
        }
        public static bool IsExistObject ( String strTableName , Guid iID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl==null )
                return false;

            if ( ctrl.GetObjectByID( iID )==null )
                return false;
            return true;
        }

        public static bool IsApprovedObject ( BusinessObject obj )
        {
            return IsApprovedObject( obj.AATableName , obj.GetID() );
        }
        public static bool IsApprovedObject ( String strTableName , Guid iID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl==null )
                return true;

            BusinessObject obj=ctrl.GetObjectByID( iID );
            if ( obj==null )
                return true;

            object objTemp=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colApprovalStatus );
            return ( objTemp!=null&&objTemp.ToString()==ABCCommon.ABCConstString.ApprovalTypeApproved );
        }

        public static bool IsLockedObject ( BusinessObject obj )
        {
            return IsLockedObject( obj.AATableName , obj.GetID() );
        }
        public static bool IsLockedObject ( String strTableName , Guid iID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl==null )
                return true;

            BusinessObject obj=ctrl.GetObjectByID( iID );
            if ( obj==null )
                return true;

            object objTemp=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colLockStatus );
            return ( objTemp!=null&&objTemp.ToString()==ABCCommon.ABCConstString.LockStatusLocked );
        }
        public static bool IsPostedObject ( BusinessObject obj )
        {
            return IsPostedObject( obj.AATableName , obj.GetID() );
        }
        public static bool IsPostedObject ( String strTableName , Guid iID )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl==null )
                return true;

            BusinessObject obj=ctrl.GetObjectByID( iID );
            if ( obj==null )
                return true;

            object objTemp=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colJournalStatus );
            return ( objTemp!=null&&objTemp.ToString()==ABCCommon.ABCConstString.PostStatusPosted );
        }

        public static Dictionary<String , object> GetSameColumnValues ( List<Guid> lstObjects ,String strTableName)
        {
            String strQuery=QueryGenerator.GenSelect( strTableName , "*" , false );
            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( @"{0} IN ({1})" , DataStructureProvider.GetPrimaryKeyColumn(strTableName), string.Format( "'{0}'" , String.Join( "','" , lstObjects ) ) ) );
            return GetSameColumnValues( BusinessControllerFactory.GetBusinessController( strTableName ).GetList( strQuery ) );
        }
        public static Dictionary<String , object> GetSameColumnValues ( List<BusinessObject> lstObjects )
        {
            Dictionary<String , object> lstResults=new Dictionary<string , object>();
            foreach ( String strProName in GetSameColumnNames( lstObjects ) )
            {
                bool isSameValue=true;
                object objFieldValue=null;
                foreach ( BusinessObject obj in lstObjects )
                {
                    if ( objFieldValue==null )
                    {
                        objFieldValue=ABCDynamicInvoker.GetValue( obj , strProName );
                        continue;
                    }
                    if ( objFieldValue!=ABCDynamicInvoker.GetValue( obj , strProName ) )
                    {
                        isSameValue=false;
                        break;
                    }
                }
                if ( isSameValue )
                    if ( lstResults.ContainsKey( strProName )==false )
                        lstResults.Add( strProName , objFieldValue );
            }

            return lstResults;
        }

        public static List<String> GetSameColumnNames ( List<BusinessObject> lstObjects )
        {
            List<String> lstProperties=new List<string>();
            foreach ( String strTableName in lstObjects.Select( a => a.AATableName ).Distinct() )
                lstProperties.AddRange( BusinessObjectHelper.PropertyList[strTableName].Keys.Where( t =>!IsBaseProperty(t) && !lstProperties.Contains( t ) ).ToList() );
            return lstProperties.Except( new List<string>(){ABCCommon.ABCConstString.colABCStatus,
                                                                    ABCCommon.ABCConstString.colCreateTime,
                                                                    ABCCommon.ABCConstString.colCreateUser,
                                                                    ABCCommon.ABCConstString.colUpdateTime,
                                                                    ABCCommon.ABCConstString.colUpdateUser,                                                                    
                                                                    ABCCommon.ABCConstString.colNoIndex,
                                                                    ABCCommon.ABCConstString.colEditCount} ).ToList();
        }
    }
}
