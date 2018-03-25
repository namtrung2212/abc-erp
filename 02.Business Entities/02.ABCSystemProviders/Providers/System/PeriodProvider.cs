using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ABCBusinessEntities;

namespace ABCProvider
{
    public  class PeriodProvider
    {
        public static void GeneratePeriods ( )
        {
            GEPeriodsController periodCtrl=new GEPeriodsController();

            for ( int year=ABCApp.ABCDataGlobal.WorkingDate.Year; year<=ABCApp.ABCDataGlobal.WorkingDate.Year+1; year++ )
            {
                String strQuery=String.Format( @"SELECT COUNT(*) FROM GEPeriods WHERE Year = {0}" , year );
                object objAmt=BusinessObjectController.GetData( strQuery );
                if ( objAmt==null||objAmt==DBNull.Value||Convert.ToInt32( objAmt )!=12 )
                {
                    strQuery=String.Format( @"DELETE  FROM GEPeriods WHERE Year = {0}" , year );
                    BusinessObjectController.RunQuery( strQuery );

                    for ( int i=1; i<=12; i++ )
                    {
                        GEPeriodsInfo period=new GEPeriodsInfo();
                        period.Month=i;
                        period.Year=year;
                        period.Period=new DateTime( period.Year.Value , period.Month.Value , 1 );
                        if ( i>=10 )
                            period.No=String.Format( "Tháng {0}/{1}" , period.Month.Value , period.Year.Value );
                        else
                            period.No=String.Format( "Tháng  0{0}/{1}" , period.Month.Value , period.Year.Value );

                        period.Closed=false;
                        periodCtrl.CreateObject( period );
                    }
                }
            }
        }

        public static DateTime GetFirstDay ( Guid periodID )
        {
            GEPeriodsInfo period=new GEPeriodsController().GetObjectByID( periodID ) as GEPeriodsInfo;
            if ( period!=null )
                return new DateTime( period.Year.Value , period.Month.Value , 1 );

            return DateTime.MinValue;
        }
        public static DateTime GetLastDay ( Guid periodID )
        {
            return GetFirstDay( periodID ).AddMonths( 1 ).AddSeconds( -1 );
        }

        public static Guid GetCurrentPeriod ( )
        {
            return GetPeriod( ABCApp.ABCDataGlobal.WorkingDate.Year , ABCApp.ABCDataGlobal.WorkingDate.Month );
        }

        public static Guid GetPeriod ( int year , int month )
        {
            if ( year<2000||month<=0 )
                return Guid.Empty;

            String strQuery=String.Format( @"SELECT * FROM GEPeriods WHERE Year = {0} AND Month = {1} " , year , month );
            GEPeriodsInfo period=new GEPeriodsController().GetObject( strQuery ) as GEPeriodsInfo;
            if ( period!=null )
                return period.GEPeriodID;
            else
            {
                period=new GEPeriodsInfo();
                period.Month=month;
                period.Year=year;
                period.Period=new DateTime( period.Year.Value , period.Month.Value , 1 );
                if ( month>=10 )
                    period.No=String.Format( "Tháng {0}/{1}" , period.Month.Value , period.Year.Value );
                else
                    period.No=String.Format( "Tháng  0{0}/{1}" , period.Month.Value , period.Year.Value );

                period.Closed=false;
                new GEPeriodsController().CreateObject( period );

                return period.GetID();
            }

            return Guid.Empty;
        }

        public static Guid GetPeriod ( Guid periodID  )
        {
            return GetPeriod( periodID , 0 );
        }
        public static int GetMonth ( Guid periodID )
        {
            GEPeriodsInfo currentPeriod=new GEPeriodsController().GetObjectByID( periodID ) as GEPeriodsInfo;
            if ( currentPeriod==null&&currentPeriod.Month.HasValue)
                return 0;

            return currentPeriod.Month.Value;
        }

        public static int GetYear( Guid periodID )
        {
            GEPeriodsInfo currentPeriod=new GEPeriodsController().GetObjectByID( periodID ) as GEPeriodsInfo;
            if ( currentPeriod==null&&currentPeriod.Year.HasValue )
                return 0;

            return currentPeriod.Year.Value;
        }

        public static Guid GetPeriod ( Guid currentPeriodID , int index )
        {
            GEPeriodsInfo currentPeriod=new GEPeriodsController().GetObjectByID( currentPeriodID ) as GEPeriodsInfo;
            if ( currentPeriod==null )
                return Guid.Empty;

            DateTime dtNext=new DateTime( currentPeriod.Year.Value , currentPeriod.Month.Value , 1 ).AddMonths( index );
            return GetPeriod( dtNext.Year , dtNext.Month );
        }

        public static Guid GetPeriod ( DateTime fromDate , DateTime toDate )
        {
            if ( fromDate.Year==toDate.Year&&fromDate.Month==toDate.Month&&fromDate.Day==1&&toDate.Day==fromDate.AddMonths( 1 ).AddDays( -1 ).Day )
                return GetPeriod( fromDate.Year , fromDate.Month );
            return Guid.Empty;
        }

    }
}
