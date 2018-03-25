using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;

namespace ABCProvider
{
    public  class TimeProvider
    {

        public static DateTime GetFirstTimeOfMonth ( DateTime date )
        {
            return new DateTime( date.Year , date.Month , 1 , 0 , 0 , 0 );
        }
        public static DateTime GetLastTimeOfMonth ( DateTime date )
        {
            return new DateTime( date.Year , date.Month , 1 , 0 , 0 , 0 ).AddMonths(1).AddSeconds(-1);
        }
        
        public static DateTime GetServerDateTime ( )
        {

            DataSet ds=DataQueryProvider.RunQuery( @"SELECT GETDATE()" );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
               return Convert.ToDateTime( ds.Tables[0].Rows[0][0].ToString() );

            return DateTime.MinValue;
        }

        public static int GetRecordCountOfTable (String strTableName )
        {

            DataSet ds=DataQueryProvider.RunQuery( String.Format( @"SELECT COUNT(*) FROM {0}" , strTableName ) );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                return Convert.ToInt32( ds.Tables[0].Rows[0][0].ToString() );

            return -1;
        }

        public static DateTime GetTableLastUpdateTime ( String strTableName )
        {
            String strQuery=String.Format( @"SELECT MAX({0}) FROM {1} WHERE YEAR({0}) < 5000" , ABCCommon.ABCConstString.colUpdateTime , strTableName );
            if ( DataQueryProvider.IsCompanySQLConnection==false )
                strQuery=String.Format( @"SELECT MAX({0}) FROM {1} WHERE strftime('%Y', {0}) < 5000" , ABCCommon.ABCConstString.colUpdateTime , strTableName );

            DataSet ds=DataQueryProvider.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 &&String.IsNullOrWhiteSpace(ds.Tables[0].Rows[0][0].ToString() )==false)
            {
                DateTime dt=Convert.ToDateTime( ds.Tables[0].Rows[0][0].ToString() );
                if ( dt!=null&&dt.Year>1000 )
                    return dt;
            }

            return DateTime.MinValue;
        }


        //public static String GenDateTimeNow ( )
        //{
        //    return GenDateTime( ABCApp.ABCDataGlobal.WorkingDate );
        //}
        public static String GenDateTimeString ( DateTime dt )
        {
            if ( dt.Year<1753 )
                dt=new DateTime( 1800 , 1 , 1 );

            //     return String.Format( " CONVERT(VARCHAR(19), '{0}', 120) " , dt.ToString( "yyyy-MM-dd HH:mm:ss" ) );
            return "'"+dt.ToString( "yyyy-MM-dd HH:mm:ss" )+"'";
        }

        public static String GenDateTimeConvertString ( String strCol )
        {
            if ( DataQueryProvider.IsCompanySQLConnection )
                return String.Format( " CONVERT(VARCHAR(19), [{0}], 120) " , strCol );
            else
                return String.Format( @" strftime('%Y-%m-%d %H:%M:%S',[{0}]) " , strCol );
        }

        public static String GenCompareDateTime ( String strCol , String strOperator , DateTime dt )
        {
            return String.Format( @"{0} {1} {2}" , GenDateTimeConvertString( strCol ) , strOperator , GenDateTimeString( dt ) );
        }

        public static DateTime ConvertToDateTime ( object obj )
        {
            if ( obj==null||obj==DBNull.Value )
                return DateTime.MinValue;
            return Convert.ToDateTime( obj.ToString() );
        }
    }
}
