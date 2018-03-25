using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using ABCBusinessEntities;
using ABCProvider;
using Security;
using ABCProvider;
using System.Xml;

namespace ABCProvider
{
   public class LoggingProvider
    {
       public static void LogAction ( BusinessObject obj , String ViewDesc )
       {
           Guid iID=BusinessObjectHelper.GetIDValue( obj );
           if ( iID==Guid.Empty )
               return;


       }

        public static void LogAction ( BusinessObject obj , String ViewDesc , String Action )
        {
            Guid iID=BusinessObjectHelper.GetIDValue( obj );
            if ( iID==Guid.Empty )
                return;

            String strNo=BusinessObjectHelper.GetNoValue( obj );
            String strRemark=BusinessObjectHelper.GetRemarkValue( obj );
            String strTableDesc=DataConfigProvider.GetTableCaption( obj.AATableName );
            LogAction( obj.AATableName , iID , strNo , strRemark , strTableDesc , ViewDesc , Action );
        }
        public static void LogAction ( string TableName , Guid ID , String ObjNo , String ObjRemark , String TableDesc , String ViewDesc , String Action )
        {

            int iCount=0;
            DataSet ds=DataQueryProvider.CompanyDatabaseHelper.RunQuery( String.Format( @"SELECT COUNT(*) FROM GEActionLogs WHERE TableName='{0}' AND ID ='{1}' " , TableName , ID ) );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                iCount=Convert.ToInt32( ds.Tables[0].Rows[0][0] );
            iCount++;

            String strQuery=QueryTemplateGenerator.GenInsert( "GEActionLogs" );
            strQuery=strQuery.Replace( "@GEActionLogID" , "'"+Guid.NewGuid()+"'" );
            strQuery=strQuery.Replace( "@TableName" , "'"+TableName+"'" );
            strQuery=strQuery.Replace( "@ID" , "'"+ID.ToString()+"'" );
            strQuery=strQuery.Replace( "@ActionIndex" , iCount.ToString() );
            strQuery=strQuery.Replace( "@Time" , "GetDate()" );// Generation.TimeProvider.GenDateTimeString(ABCApp.ABCDataGlobal.WorkingDate) );


            if ( DataQueryProvider.IsCompanySQLConnection )
            {
                strQuery=strQuery.Replace( "@TableDesc" , "N'"+TableDesc.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ViewDesc" , "N'"+ViewDesc.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ObjNo" , "N'"+ObjNo.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ObjRemark" , "N'"+ObjRemark.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ActionUser" , "N'"+ABCBaseUserProvider.CurrentUserName.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ActionEmployee" , "N'"+ABCBaseUserProvider.CurrentEmployeeName.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@Action" , "N'"+Action.Replace( "'" , "''" )+"'" );
            }
            else
            {
                strQuery=strQuery.Replace( "@TableDesc" , "'"+TableDesc.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ViewDesc" , "'"+ViewDesc.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ObjNo" , "'"+ObjNo.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ObjRemark" , "'"+ObjRemark.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ActionUser" , "'"+ABCBaseUserProvider.CurrentUserName.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@ActionEmployee" , "'"+ABCBaseUserProvider.CurrentEmployeeName.Replace( "'" , "''" )+"'" );
                strQuery=strQuery.Replace( "@Action" , "'"+Action.Replace( "'" , "''" )+"'" );
            }
            DataQueryProvider.CompanyDatabaseHelper.RunScript( strQuery );
        }

    }

}
