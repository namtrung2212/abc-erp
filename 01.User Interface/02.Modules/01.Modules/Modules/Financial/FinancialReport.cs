using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCModules
{
    
    public class FinanceStatisticTime
    {
        public FinanceStatisticType StatisticType=FinanceStatisticType.Year;
        public DateTime StartDate=DateTime.MinValue;
        public DateTime EndDate=DateTime.MaxValue;

        public FinanceStatisticTime ( DateTime endDay )
        {
            EndDate=endDay.Date.AddDays( 1 ).AddSeconds( -1 );
            StatisticType=FinanceStatisticType.RangeDate;
        }
        public FinanceStatisticTime ( DateTime? start , DateTime? end )
        {
            if ( start.HasValue )
                StartDate=start.Value.Date;
            if ( end.HasValue )
                EndDate=end.Value.Date.AddDays( 1 ).AddSeconds( -1 );
            StatisticType=FinanceStatisticType.RangeDate;
        }
        public FinanceStatisticTime ( int year )
        {
            StatisticType=FinanceStatisticType.Year;
            StartDate=new DateTime( year , 1 , 1 );
            EndDate=StartDate.AddYears( 1 ).AddSeconds( -1 );
        }
        public FinanceStatisticTime ( int year , int quater )
        {
            if ( quater<1 )
                quater=1;
            if ( quater>4 )
                quater=4;

            StatisticType=FinanceStatisticType.Quater;
            StartDate=new DateTime( year , ( quater-1 )*3+1 , 1 );
            EndDate=StartDate.AddMonths( 3 ).AddSeconds( -1 );
        }

        public void SetTime ( DateTime endDay )
        {
            EndDate=endDay.Date.AddDays( 1 ).AddSeconds( -1 );
            StatisticType=FinanceStatisticType.RangeDate;
        }
        public void SetTime ( DateTime? start , DateTime? end )
        {
            if ( start.HasValue )
                StartDate=start.Value.Date;
            if ( end.HasValue )
                EndDate=end.Value.Date.AddDays( 1 ).AddSeconds( -1 );
            StatisticType=FinanceStatisticType.RangeDate;
        }
        public void SetTime ( int year )
        {
            StatisticType=FinanceStatisticType.Year;
            StartDate=new DateTime( year , 1 , 1 );
            EndDate=StartDate.AddYears( 1 ).AddSeconds( -1 );
        }
        public void SetTime ( int year , int quater )
        {
            if ( quater<1 )
                quater=1;
            if ( quater>4 )
                quater=4;

            StatisticType=FinanceStatisticType.Quater;
            StartDate=new DateTime( year , ( quater-1 )*3+1 , 1 );
            EndDate=StartDate.AddMonths( 3 ).AddSeconds( -1 );
        }
    }
    public enum FinanceStatisticType
    {
        Year ,
        Quater ,
        RangeDate
    }

    public class ProfitAndLossInfo
    {
        public String Index { get; set; }
        public String Factor { get; set; }
        public String No { get; set; }
        public String Desc { get; set; }
        public double Current { get; set; }
        public double Past { get; set; }

        public ProfitAndLossInfo ( String strIndex , String strFactor , String strNo , String strDesc , double dbCurrent , double dbPast )
        {
            Factor=strFactor;
            No=strNo;
            Desc=strDesc;
            Current=dbCurrent;
            Past=dbPast;
            Index=strIndex;
        }
    }

    public class BalanceSheetInfo
    {
        public String Index { get; set; }
        public String Factor { get; set; }
        public String No { get; set; }
        public String Desc { get; set; }
        public double Current { get; set; }
        public double Past { get; set; }
        public bool Bold { get; set; }
        public BalanceSheetInfo ( bool isBold , String strIndex , String strFactor , String strNo , String strDesc , double dbCurrent , double dbPast )
        {
            Factor=strFactor;
            No=strNo;
            Desc=strDesc;
            Current=dbCurrent;
            Past=dbPast;
            Index=strIndex;

            Bold=isBold;
        }
    }

    public class CashFlowDirectInfo
    {
        public String Index { get; set; }
        public String Factor { get; set; }
        public String No { get; set; }
        public String Desc { get; set; }
        public double Current { get; set; }
        public double Past { get; set; }
        public bool Bold { get; set; }
        public CashFlowDirectInfo ( bool isBold , String strIndex , String strFactor , String strNo , String strDesc , double dbCurrent , double dbPast )
        {
            Factor=strFactor;
            No=strNo;
            Desc=strDesc;
            Current=dbCurrent;
            Past=dbPast;
            Index=strIndex;

            Bold=isBold;
        }
    }

    public class CashFlowInDirectInfo
    {
        public String Index { get; set; }
        public String Factor { get; set; }
        public String No { get; set; }
        public String Desc { get; set; }
        public double Current { get; set; }
        public double Past { get; set; }
        public bool Bold { get; set; }
        public CashFlowInDirectInfo ( bool isBold , String strIndex , String strFactor , String strNo , String strDesc , double dbCurrent , double dbPast )
        {
            Factor=strFactor;
            No=strNo;
            Desc=strDesc;
            Current=dbCurrent;
            Past=dbPast;
            Index=strIndex;

            Bold=isBold;
        }
    }


}
