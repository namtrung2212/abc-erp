using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Diagnostics;
using System.Data.SQLite;
using EntLibContrib.Data.SQLite;
using ABCProvider;

namespace ABCBusinessEntities
{
    public class BusinessObjectController
    {
        public String TableName=String.Empty;

        ABCDatabaseHelper dataHelper;
        public ABCDatabaseHelper DatabaseHelper
        {
            get
            {
                if ( dataHelper!=null||String.IsNullOrWhiteSpace( TableName ) )
                    return dataHelper;

                if ( DataStructureProvider.IsSystemTable( TableName ) )
                    dataHelper=DataQueryProvider.SystemDatabaseHelper;
                else
                    dataHelper=DataQueryProvider.CompanyDatabaseHelper;

                return dataHelper;
            }
        }

        #region Constructor
        public BusinessObjectController ( )
        {

        }
        public BusinessObjectController ( String strTableName )
        {
            TableName=strTableName;
        }

        #endregion

        #region Utility
        public int GetNextNoIndex ( )
        {
            return DatabaseHelper.GetMaxNoIndex( TableName )+1;
        }
        #endregion

        public Guid SaveObject ( BusinessObject obj )
        {
            if ( obj.GetID()==Guid.Empty )
                return CreateObject( obj );
            else
            {
                 UpdateObject( obj );
                 return obj.GetID();
            }
        }
        public virtual Guid CreateObject ( BusinessObject obj )
        {

            #region Default Value
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colSelected ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colSelected , false );

            if ( String.IsNullOrWhiteSpace( ABCBaseUserProvider.CurrentUserName )==false&&
                DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateUser ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colCreateUser , ABCBaseUserProvider.CurrentUserName );

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateTime ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colCreateTime , ABCApp.ABCDataGlobal.WorkingDate );

            #endregion

            Guid iID=Guid.Empty;
            if ( DatabaseHelper.IsSQLConnection() )
            {
                iID=ABCHelper.DataConverter.ConvertToGuid( ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.Insert , TableName ) , obj ) );
            }
            else
            {
                String strQuery=QueryTemplateGenerator.GenInsert( TableName );
                iID=Guid.NewGuid();
                strQuery=strQuery.Replace( String.Format( "@{0}" , DataStructureProvider.GetPrimaryKeyColumn( TableName ) ) , "'"+iID.ToString()+"'" );
                DatabaseHelper.RunScript( strQuery , obj );
            }

            ABCDynamicInvoker.SetValue( obj , DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName ) , iID );

            if ( BusinessObjectHelper.GenerateNoColumn( obj , true ) )
                UpdateObject( obj );

            return iID;
        }

      
        public void UpdateObject ( BusinessObject obj )
        {
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colSelected ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colSelected , false );

            if ( String.IsNullOrWhiteSpace( ABCBaseUserProvider.CurrentUserName )==false&&
             DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colUpdateUser ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colUpdateUser , ABCBaseUserProvider.CurrentUserName );

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colUpdateTime ) )
                ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colUpdateTime , ABCApp.ABCDataGlobal.WorkingDate );

            BusinessObjectHelper.GenerateNoColumn( obj , false );

            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.Update , TableName ) , obj );
            else
                DatabaseHelper.RunScript( QueryTemplateGenerator.GenUpdate( TableName ) , obj );
        }

        public void DeleteObject ( Guid iObjectID )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.Delete , TableName ) , iObjectID );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenDeleteByID( TableName , iObjectID ) );
        }
        public void DeleteObject ( BusinessObject obj )
        {
            object objID=(object)ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetPrimaryKeyColumn( obj.AATableName ) );
            Guid id=Guid.Empty;
            if ( objID is Guid )
                id=(Guid)objID;
            else if ( objID is Nullable<Guid>&&( (Nullable<Guid>)objID ).HasValue )
                id=( (Nullable<Guid>)objID ).Value;
            else
                return;

            DeleteObject( id );
        }
        public void DeleteAllObjects ( )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.DeleteAll , TableName ) );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenDeleteAll( TableName ) );
        }
        public void DeleteObjectsByFK ( String strForeignColumn , Guid objValue )
        {
            if ( String.IsNullOrWhiteSpace( strForeignColumn )||objValue ==Guid.Empty )
                return;

            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.DeleteByFK , TableName , strForeignColumn ) , objValue );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenDeleteByColumn( TableName , strForeignColumn , objValue ) );
        }

        public void RealDeleteObject ( Guid iObjectID )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.RealDelete , TableName ) , iObjectID );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenRealDeleteByID( TableName , iObjectID ) );

        }
        public void RealDeleteAllObjects ( )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.RealDeleteAll , TableName ) );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenRealDeleteAll( TableName ) );

        }
        public void RealDeleteObjectsByFK ( String strForeignColumn , object objValue )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.RealDeleteByFK , TableName , strForeignColumn ) , objValue );
            else
                DatabaseHelper.RunScript( QueryGenerator.GenRealDeleteByColumn( TableName , strForeignColumn , objValue ) );
        }

        #region Select

        #region Get Single Object
        public BusinessObject GetObject ( String strQuery )
        {
            DataSet ds=GetDataSet( strQuery );
            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        public BusinessObject GetObjectByCondition ( String strConditionQuery )
        {
            DataSet ds=GetDataSetByCondition( strConditionQuery );
            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        public BusinessObject GetObjectByID ( Guid? iObjectID )
        {
            if ( iObjectID.HasValue )
                return GetObjectByID( iObjectID.Value );

            return null;
        }
        public BusinessObject GetObjectByID ( Guid iObjectID )
        {
            try
            {
                DataSet ds=null;
                if ( DatabaseHelper.IsSQLConnection() )
                    ds=( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.Select , TableName ) , iObjectID );
                else
                    ds=( DatabaseHelper as SQLiteDatabaseHelper ).RunScript( QueryGenerator.GenSelectByID ( TableName , iObjectID ) );

                return BusinessObjectHelper.GetBusinessObject( ds , TableName );
            }
            catch ( Exception ex )
            {
                return null;
            }
        }
        public BusinessObject GetObjectByNo ( String strObjectNo )
        {
            DataSet ds=null;
            if ( DatabaseHelper.IsSQLConnection() )
                ds=( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.SelectByNo , TableName ) , strObjectNo );
            else
                ds=( DatabaseHelper as SQLiteDatabaseHelper ).RunScript( QueryGenerator.GenSelectByNo( TableName , strObjectNo,false,true ) );

            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        public BusinessObject GetObjectByName ( String strObjectName )
        {
            DataSet ds=null;
            if ( DatabaseHelper.IsSQLConnection() )
                ds=( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.SelectByName , TableName ) , strObjectName );
            else
                ds=( DatabaseHelper as SQLiteDatabaseHelper ).RunScript( QueryGenerator.GenSelectByName( TableName , strObjectName , false , true ) );

            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        public BusinessObject GetObjectByColumn ( String strFieldName , object objValue )
        {
            if ( DataStructureProvider.IsTableColumn( TableName , strFieldName )==false )
                return null;

            String strTypeName=DataStructureProvider.GetCodingType( TableName , strFieldName );
            if ( strTypeName=="bool"||strTypeName=="String"||strTypeName=="DateTime"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )
                return GetObject( String.Format( "SELECT * FROM {0} WHERE {1} = '{2}' " , TableName , strFieldName , objValue ) );

            return GetObject( String.Format( "SELECT * FROM {0} WHERE {1} = {2} " , TableName , strFieldName , objValue ) );
        }
        public BusinessObject GetObjectBySqlSP ( string spName , params object[] paramValues )
        {
            DataSet ds=GetDataSetBySqlSP( spName , paramValues );
            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }

        public BusinessObject GetFirstObject ( )
        {
            StringBuilder strBuilder=new StringBuilder();
            strBuilder.Append( String.Format( "Select Top(1) * From [{0}] " , TableName ) );

            if ( DataStructureProvider.IsExistABCStatus( TableName ) )
            {
                strBuilder.Append( " WHERE " );
                strBuilder.Append( String.Format( "[{0}]='{1}'" , ABCCommon.ABCConstString.colABCStatus , ABCCommon.ABCConstString.ABCStatusAlive ) );
            }

            String strQuery=strBuilder.ToString();
            if ( DatabaseHelper.IsSQLConnection()==false )
                strQuery=SQLiteDatabaseHelper.RepairSelectSQLite( strQuery );

            DataSet ds=DatabaseHelper.RunQuery( strQuery );
            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        public BusinessObject GetObjectFromDataRow ( DataRow row )
        {
            return BusinessObjectHelper.GetBusinessObject( row , TableName );
        }

        #endregion

        #region Get DataSet

        public DataSet GetDataSet ( String strQuery )
        {
            return DatabaseHelper.RunQuery( strQuery );
        }
        public DataSet GetDataSetByCondition ( String strConditionQuery )
        {
            String strQuery=QueryGenerator.GenSelectAll( TableName ,false , true );
            strQuery=QueryGenerator.AddCondition( strQuery , strConditionQuery );
            return DatabaseHelper.RunQuery( strQuery );
        }
        public DataSet GetDataSetByQuery ( string strQuery , params object[] paramValues )
        {
            DbCommand cmd=DatabaseHelper.CurrentDatabase.GetSqlStringCommand( strQuery );
            for ( int i=0; i<paramValues.Length; i++ )
                cmd.Parameters.Add( paramValues[i] );

            return (DataSet)DatabaseHelper.CurrentDatabase.ExecuteDataSet( cmd );
        }
        public DataSet GetDataSetBySqlSP ( string spName , params object[] paramValues )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                return (DataSet)( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( spName , paramValues );

            ABCHelper.ABCMessageBox.Show( "Not support Stored Procedure!" );
            return null;
        }

        public DataSet GetDataSetByForeignKey ( String strForeignColumnName , Guid? iObjectID )
        {
            if ( iObjectID.HasValue )
                return GetDataSetByForeignKey( strForeignColumnName , iObjectID.Value );
            return new DataSet();
        }
        public DataSet GetDataSetByForeignKey ( String strForeignColumnName , Guid iObjectID )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                return ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.SelectByFK , TableName , strForeignColumnName ) , iObjectID );
            else
                return DatabaseHelper.RunScript( QueryGenerator.GenSelectByColumn( TableName , strForeignColumnName , iObjectID , false , true ) );
        }
        public DataSet GetDataSetByColumn ( String strFieldName , object objValue )
        {
            if ( DataStructureProvider.IsTableColumn( TableName , strFieldName )==false )
                return null;

            String strTypeName=DataStructureProvider.GetCodingType( TableName , strFieldName );
            if ( strTypeName=="String"||strTypeName=="DateTime" ||strTypeName=="Guid"||strTypeName=="Nullable<Guid>")
                return GetDataSet( String.Format( "SELECT * FROM {0} WHERE {1} = '{2}' " , TableName , strFieldName , objValue ) );

            return GetDataSet( String.Format( "SELECT * FROM {0} WHERE {1} = {2} " , TableName , strFieldName , objValue ) );
        }
        public DataSet GetDataSetAllObjects ( )
        {
            if ( DatabaseHelper.IsSQLConnection() )
                return ( DatabaseHelper as SqlDatabaseHelper ).RunStoredProcedure( StoredProcedureGenerator.GetSPName( StoredProcedureGenerator.SPType.SelectAll , TableName ) );
            else
                return DatabaseHelper.RunScript( QueryGenerator.GenSelectAll( TableName,false,true ) );
        }
     
        #endregion

        #region Get List
        public List<BusinessObject> GetList ( String strQuery )
        {
            DataSet ds=GetDataSet( strQuery );
            return GetListFromDataset( ds );
        }
        public List<BusinessObject> GetListByCondition ( String strConditionQuery )
        {
            DataSet ds=GetDataSetByCondition( strConditionQuery );
            return GetListFromDataset( ds );
        }
        public List<BusinessObject> GetListByQuery ( string strQuery , params object[] paramValues )
        {
            DataSet ds=GetDataSetByQuery( strQuery , paramValues );
            return GetListFromDataset( ds );
        }
        public List<BusinessObject> GetListBySqlSP ( string spName , params object[] paramValues )
        {
            DataSet ds=GetDataSetBySqlSP( spName , paramValues );
            return GetListFromDataset( ds );
        }

        public List<BusinessObject> GetListByForeignKey ( String strForeignColumnName , Guid iObjectID )
        {
            DataSet ds=GetDataSetByForeignKey( strForeignColumnName , iObjectID );
            return GetListFromDataset( ds );
        }
        public List<BusinessObject> GetListByForeignKey ( String strForeignColumnName , Guid? iObjectID )
        {
            DataSet ds=GetDataSetByForeignKey( strForeignColumnName , iObjectID );
            return GetListFromDataset( ds );
        }
        public List<BusinessObject> GetListByColumn ( String strFieldName , object objValue )
        {
            if ( DataStructureProvider.IsTableColumn( TableName , strFieldName )==false )
                return new List<BusinessObject>();

            String strQuery=QueryGenerator.GenSelectByColumn( TableName , strFieldName , objValue , false );
            return GetList( strQuery );
        }
        public List<BusinessObject> GetListAllObjects ( )
        {
            DataSet ds=GetDataSetAllObjects();
            return GetListFromDataset( ds );
        }

        public List<BusinessObject> GetListFromDataset ( DataSet ds )
        {
            List<BusinessObject> lstResult=new List<BusinessObject>();

            if ( ds!=null&&ds.Tables[0]!=null&&ds.Tables[0].Rows.Count>0 )
            {
                foreach ( DataRow drow in ds.Tables[0].Rows )
                {
                    BusinessObject bus=(BusinessObject)this.GetObjectFromDataRow( drow );
                    lstResult.Add( bus );
                }

            }
            return lstResult;
        }
        #endregion

        #region Get Field

        public String GetNameByID ( Guid iObjectID )
        {
            BusinessObject obj=GetObjectByID( iObjectID );
            return (String)ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetNAMEColumn( TableName ) );
        }
        public String GetNoByID ( Guid iObjectID )
        {
            BusinessObject obj=GetObjectByID( iObjectID );
            return (String)ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetNOColumn( TableName ) );
        }
        public Guid GetIDByName ( String strObjectName )
        {
            BusinessObject obj=GetObjectByName( strObjectName );
            return (Guid)ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetPrimaryKeyColumn( TableName ) );
        }
        public Nullable<Guid> GetIDByNo ( String strObjectNo )
        {
            BusinessObject obj=GetObjectByNo( strObjectNo );
            if ( obj==null )
                return null;

            object objID=ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetPrimaryKeyColumn( TableName ) );
            if ( objID==null )
                return null;

            return (Nullable<Guid>)objID;
        }
       
        public static object GetData ( String strQuery )
        {
            DataSet ds=DataQueryProvider.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0&&ds.Tables[0].Columns.Count>0 )
                return ds.Tables[0].Rows[0][0];

            return null;
        }

        public static List<object> GetListObjects ( String strQuery )
        {
            List<object> lstResults=new List<object>();
            DataSet ds=DataQueryProvider.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0&&ds.Tables[0].Columns.Count>0 )
                foreach ( DataRow dr in ds.Tables[0].Rows )
                    if ( dr[0]!=null&&dr[0]!=DBNull.Value )
                        lstResults.Add( dr[0] );

            return lstResults;
        }

        #endregion

        #region Others

        public DataSet GetAllTemplateObjects ( )
        {
            String strSQL=String.Format( "Select * From [{0}] Where [{1}]='{2}'" , TableName , ABCCommon.ABCConstString.colABCStatus , ABCCommon.ABCConstString.ABCStatusTemplate );
            return DatabaseHelper.RunQuery( strSQL );
        }
        public BusinessObject GetTemplateObject ( )
        {
            String strSQL=String.Format( "Select * From [{0}] Where [{1}]='{2}'" , TableName , ABCCommon.ABCConstString.colABCStatus , ABCCommon.ABCConstString.ABCStatusTemplate );
            DataSet ds=DatabaseHelper.RunQuery( strSQL );
            return BusinessObjectHelper.GetBusinessObject( ds , TableName );
        }
        #endregion

        #endregion

        public static DataSet RunQuery ( String strQuery )
        {
            return DataQueryProvider.RunQuery( strQuery );
        }
        public static DataSet RunQuery ( String strQuery , String strTableName )
        {
            if ( DataStructureProvider.IsSystemTable( strTableName ) )
                return DataQueryProvider.SystemDatabaseHelper.RunQuery( strQuery );
            else
                return DataQueryProvider.CompanyDatabaseHelper.RunQuery( strQuery );
        }
    }
}
