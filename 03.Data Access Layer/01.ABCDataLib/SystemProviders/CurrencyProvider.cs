using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class CurrencyProvider
    {
        public static Guid GetDefaultCurrency ( )
        {
            return Guid.Empty;
        }
        public static void CalculateCurrencyRate ( Guid currencyID )
        {
        }
        public static Dictionary<Guid , SortedList<DateTime , double>> ExchangeRateLists;
     
        [ABCRefreshTable( "GEExchangeRates" )]
        public static void InitExchangeRates ( )
        {
            ExchangeRateLists=new Dictionary<Guid , SortedList<DateTime , double>>();
            String strQuery=String.Format( @"SELECT FK_GECurrencyID,RateDate, EndExchangeRate FROM GEExchangeRates ORDER BY RateDate ASC" );
            DataSet ds=BusinessObjectController.RunQuery( strQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    Guid currencyID=ABCHelper.DataConverter.ConvertToGuid( dr["FK_GECurrencyID"] );
                    DateTime rateDate=TimeProvider.ConvertToDateTime( dr["RateDate"] );
                    double exchangeRate=Convert.ToDouble( dr["EndExchangeRate"] );

                    if ( currencyID==Guid.Empty||rateDate==null || rateDate==DateTime.MinValue )
                        continue;

                    if ( !ExchangeRateLists.ContainsKey( currencyID ) )
                        ExchangeRateLists.Add( currencyID , new SortedList<DateTime , double>() );
                  
                    ExchangeRateLists[currencyID].Add( rateDate , exchangeRate );
                }
            }
        }
        public static double GetExchangeRate ( Guid currencyID , DateTime date )
        {

            if ( ExchangeRateLists==null )
                InitExchangeRates();

            if ( !ExchangeRateLists.ContainsKey( currencyID ) )
                return 0;

            foreach ( DateTime dt in ExchangeRateLists[currencyID].Keys )
            {
                int iIndex=ExchangeRateLists[currencyID].IndexOfKey( dt );
                if ( dt<=date )
                {
                    if ( ( iIndex==ExchangeRateLists[currencyID].Count-1 )
                        ||date<ExchangeRateLists[currencyID].Keys[iIndex+1] )
                    {
                        return ExchangeRateLists[currencyID][dt];
                    }
                }
            }

            return 0;
        }
        public static double GetExchangeRate ( String strCurrencyNo , DateTime date )
        {
            return 0;
        }

        public static Guid AppCurrencyID=Guid.Empty;

        public static bool GenerateCurrencyValue ( BusinessObject obj )
        {
            bool isModified=false;

            String strFK_GECurrencyID=DataStructureProvider.GetForeignKeyOfTableName( obj.AATableName , "GECurrencys" );
            if ( !String.IsNullOrWhiteSpace( strFK_GECurrencyID ) )
            {
                if ( AppCurrencyID==Guid.Empty )
                {
                    String strQuery=@"SELECT FK_GECurrencyID FROM GEAppConfigs";
                    AppCurrencyID=ABCHelper.DataConverter.ConvertToGuid( BusinessObjectController.GetData( strQuery ) );
                }
                ABCDynamicInvoker.SetValue( obj , strFK_GECurrencyID , AppCurrencyID );
                if ( AppCurrencyID!=Guid.Empty&&DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colExchangeRate ) )
                {
                    object objDate=DateTime.MinValue;
                    String strDateCol=String.Empty;
                    if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colCreateTime ) )
                        strDateCol=ABCCommon.ABCConstString.colCreateTime;
                    if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colDocumentDate ) )
                        strDateCol=ABCCommon.ABCConstString.colDocumentDate;

                    if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colVoucherDate ) )
                        strDateCol=ABCCommon.ABCConstString.colVoucherDate;

                    if ( !String.IsNullOrWhiteSpace( strDateCol ) )
                    {
                        objDate=ABCDynamicInvoker.GetValue( obj , strDateCol );

                        object objOldValue=ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colExchangeRate );

                        if ( objDate!=null&&objDate is DateTime )
                        {
                            object objNewValue=CurrencyProvider.GetExchangeRate( AppCurrencyID , Convert.ToDateTime( objDate ) );
                            ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colExchangeRate , objNewValue );
                            isModified=isModified||( objOldValue!=objNewValue );
                        }
                        else if ( objDate!=null&&objDate is Nullable<DateTime> )
                        {
                            object objNewValue=CurrencyProvider.GetExchangeRate( AppCurrencyID , ( objDate as Nullable<DateTime> ).Value );
                            ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colExchangeRate , objNewValue );
                            isModified=isModified||( objOldValue!=objNewValue );
                        }
                    }
                }
            }

            return isModified;
        }

        public static double ConvertCurrency ( Guid sourceCurrencyID , Guid destinyCurrencyID , double amount )
        {
            return ConvertCurrency( sourceCurrencyID , destinyCurrencyID , amount , DateTime.Now );
        }
        public static double ConvertCurrency ( Guid sourceCurrencyID , Guid destinyCurrencyID , double amount , DateTime date )
        {
            return 0;
        }
        public static double ConvertCurrency ( String strSourceCurrencyNo , String strDestinyCurrencyNo , double amount )
        {
            return ConvertCurrency( strSourceCurrencyNo , strDestinyCurrencyNo , amount , DateTime.Now );
        }
        public static double ConvertCurrency ( String strSourceCurrencyNo , String strDestinyCurrencyNo , double amount , DateTime date )
        {
            return 0;
        }

        public static double DiffValue ( Guid currencyID ,double amount, DateTime sourceDate , DateTime destinyDate )
        {
            return 0;
        }
        public static double DiffValue ( String strCurrencyNo , double amount , DateTime sourceDate , DateTime destinyDate )
        {
            return 0;
        }
    }
}
