using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCProvider
{
    public class FinancialProvider
    {
        #region Benifit Calculation

        #region Calculation

        public static void BenifitCalculate ( )
        {
            object objTime=BusinessObjectController.GetData( String.Format( @"SELECT MAX(JournalDate) FROM GLJournalEntrys WHERE ABCStatus ='Alive'  AND (EntryType IS NULL OR  EntryType != '{0}')" , ABCCommon.ABCConstString.EntryTypePeriodEnding ) );
            DateTime currentPeriod=Convert.ToDateTime( objTime.ToString() );
            if ( currentPeriod==null||currentPeriod.Year<=1000 )
                return;

            AccountForReCalcList.Clear();

            GLBenifitCalcsController benifitCalcCtrl=new GLBenifitCalcsController();

            List<BusinessObject> lstPeriods=new GEPeriodsController().GetListAllObjects();
            foreach ( GEPeriodsInfo period in lstPeriods )
            {
                if ( period.Period.HasValue==false||period.Closed )
                    continue;

                GLBenifitCalcsInfo benifitInfo=benifitCalcCtrl.GetObjectByColumn( "FK_GEPeriodID" , period.GEPeriodID ) as GLBenifitCalcsInfo;
                if ( benifitInfo==null )
                {
                    DateTime dtStart=new DateTime( SystemProvider.AppConfig.StartDate.Value.Year , SystemProvider.AppConfig.StartDate.Value.Month , 1 );
                    if ( dtStart<=period.Period.Value&&period.Period.Value<=currentPeriod )
                    {
                        benifitInfo=new GLBenifitCalcsInfo();
                        benifitInfo.FK_GEPeriodID=period.GEPeriodID;
                        benifitInfo.Month=period.Month;
                        benifitInfo.Year=period.Year;
                        benifitInfo.Period=period.Period;
                        benifitInfo.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeNew;
                        benifitCalcCtrl.CreateObject( benifitInfo );

                        if ( BenifitCalculationOfPeriod( benifitInfo ) )
                        {
                            if ( period.Period.Value.Year!=currentPeriod.Year||period.Period.Value.Month!=currentPeriod.Month )
                                PostBenifitCalculation( benifitInfo );
                        }
                    }
                }
                else
                {
                    if ( period.Period.Value.Year==currentPeriod.Year&&period.Period.Value.Month==currentPeriod.Month )
                        BenifitCalculationOfPeriod( benifitInfo );
                    else
                    {
                        if ( period.Period.Value.AddMonths( 1 ).Year==currentPeriod.Year&&period.Period.Value.AddMonths( 1 ).Month==currentPeriod.Month )
                            if ( BenifitCalculationOfPeriod( benifitInfo )||benifitInfo.ApprovalStatus!=ABCCommon.ABCConstString.ApprovalTypeApproved )
                                PostBenifitCalculation( benifitInfo );
                    }
                }
            }

            foreach ( Guid iAccountID in AccountForReCalcList )
                AccountingProvider.CalculateAccount( iAccountID );

            DataCachingProvider.RefreshLookupTable( "GLJournalVouchers" );
        }
        public static bool BenifitCalculationOfPeriod ( GLBenifitCalcsInfo benifitInfo )
        {
            if ( benifitInfo.FK_GEPeriodID.HasValue==false||benifitInfo.GLBenifitCalcID==Guid.Empty )
                return false;

            GEPeriodsInfo period=new GEPeriodsController().GetObjectByID( benifitInfo.FK_GEPeriodID.Value ) as GEPeriodsInfo;
            if ( period==null )
                return false;

            DateTime startDate=new DateTime( period.Year.Value , period.Month.Value , 1 );
            DateTime endDate=startDate.AddMonths( 1 ).AddSeconds( -10 );

            BenifitCalculationOfRange( benifitInfo , startDate , endDate );

            if ( BusinessObjectHelper.IsModifiedObject( benifitInfo ) )
            {
                new GLBenifitCalcsController().UpdateObject( benifitInfo );
                return true;
            }
            return false;
        }
        public static void BenifitCalculationOfRange ( GLBenifitCalcsInfo benifitInfo , DateTime? startDate , DateTime? endDate )
        {
            #region BenifitGrossSale

            #region RevenueGrossSale
            benifitInfo.RevenueGrossSale=0;
            if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
            {
                benifitInfo.RevenueGrossSale+=AccountingProvider.GetCreditAmount( AccountingProvider.GetAccountID( "511" ) , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                benifitInfo.RevenueGrossSale-=AccountingProvider.GetDebitAmount( AccountingProvider.GetAccountID( "511" ) , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
            }
            if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
            {
                benifitInfo.RevenueGrossSale+=AccountingProvider.GetCreditAmount( AccountingProvider.GetAccountID( "512" ) , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                benifitInfo.RevenueGrossSale-=AccountingProvider.GetDebitAmount( AccountingProvider.GetAccountID( "512" ) , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
            }
            #endregion

            #region RevenueDecrease
            benifitInfo.RevenueDecreaseTax=0;
            benifitInfo.RevenueDecreaseCommercialDiscount=0;
            benifitInfo.RevenueDecreaseSaleDiscount=0;
            benifitInfo.RevenueDecreaseReturnGood=0;

            #region RevenueDecreaseTax
            if ( AccountingProvider.GetAccountID( "3332" )!=Guid.Empty )
            {
                if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseTax+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "511" ) ,
                                                                                                                               AccountingProvider.GetAccountID( "3332" )
                                                                                                                                 , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }

                if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseTax+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "512" ) ,
                                                                                                                               AccountingProvider.GetAccountID( "3332" )
                                                                                                                                , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
            }

            if ( AccountingProvider.GetAccountID( "3333" )!=Guid.Empty )
            {
                if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseTax+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "511" ) ,
                                                                                                                             AccountingProvider.GetAccountID( "3333" )
                                                                                                                             , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
                if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseTax+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "512" ) ,
                                                                                                                           AccountingProvider.GetAccountID( "3333" )
                                                                                                                           , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
            }
            #endregion

            #region RevenueDecreaseCommercialDiscount
            if ( AccountingProvider.GetAccountID( "521" )!=Guid.Empty )
            {
                if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseCommercialDiscount+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "511" ) ,
                                                                                                                                    AccountingProvider.GetAccountID( "521" )
                                                                                                                                     , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
                if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseCommercialDiscount+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "512" ) ,
                                                                                                                          AccountingProvider.GetAccountID( "521" )
                                                                                                                           , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
            }
            #endregion

            #region RevenueDecreaseSaleDiscount
            if ( AccountingProvider.GetAccountID( "532" )!=Guid.Empty )
            {
                if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseSaleDiscount+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "511" ) ,
                                                                                                                                         AccountingProvider.GetAccountID( "532" )
                                                                                                                                         , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
                if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseSaleDiscount+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "512" ) ,
                                                                                                                           AccountingProvider.GetAccountID( "532" )
                                                                                                                           , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
            }
            #endregion

            #region RevenueDecreaseReturnGood
            if ( AccountingProvider.GetAccountID( "531" )!=Guid.Empty )
            {
                if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseReturnGood+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "511" ) ,
                                                                                                                                           AccountingProvider.GetAccountID( "531" )
                                                                                                                                            , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
                if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                {
                    benifitInfo.RevenueDecreaseReturnGood+=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "512" ) ,
                                                                                                                          AccountingProvider.GetAccountID( "531" )
                                                                                                                           , startDate , endDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true );
                }
            }
            #endregion

            benifitInfo.RevenueDecrease=benifitInfo.RevenueDecreaseTax+benifitInfo.RevenueDecreaseCommercialDiscount
                                        +benifitInfo.RevenueDecreaseSaleDiscount+benifitInfo.RevenueDecreaseReturnGood;

            #endregion

            benifitInfo.RevenueNetSale=benifitInfo.RevenueGrossSale-benifitInfo.RevenueDecrease;

            #region CostOfGoodSolved
            benifitInfo.CostOfGoodSolved=0;
            if ( AccountingProvider.GetAccountID( "632" )!=Guid.Empty )
                benifitInfo.CostOfGoodSolved=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "632" ) , startDate , endDate , "" , true );
            #endregion

            benifitInfo.BenifitGrossSale=benifitInfo.RevenueNetSale-benifitInfo.CostOfGoodSolved;

            #endregion

            #region BenifitComercial

            #region RevenueCommercial
            benifitInfo.RevenueCommercial=0;
            if ( AccountingProvider.GetAccountID( "515" )!=Guid.Empty )
                benifitInfo.RevenueCommercial=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "515" ) , AccountingProvider.GetAccountID( "911" ) , startDate , endDate , "" , true );

            #endregion

            #region CostCommercial
            benifitInfo.CostCommercial=0;
            if ( AccountingProvider.GetAccountID( "635" )!=Guid.Empty )
                benifitInfo.CostCommercial=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "635" ) , startDate , endDate , "" , true );

            #endregion

            benifitInfo.CostLoan=0;

            benifitInfo.BenifitComercial=benifitInfo.RevenueCommercial-benifitInfo.CostCommercial-benifitInfo.CostLoan;

            #endregion

            #region CostSale
            benifitInfo.CostSale=0;
            if ( AccountingProvider.GetAccountID( "641" )!=Guid.Empty )
                benifitInfo.CostSale=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "641" ) , startDate , endDate , "" , true );

            #endregion

            #region CostManage
            benifitInfo.CostManage=0;
            if ( AccountingProvider.GetAccountID( "642" )!=Guid.Empty )
                benifitInfo.CostManage=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "642" ) , startDate , endDate , "" , true );

            #endregion

            benifitInfo.BenifitNet=benifitInfo.BenifitGrossSale+( benifitInfo.RevenueCommercial-benifitInfo.CostCommercial )-( benifitInfo.CostSale+benifitInfo.CostManage );

            #region BenifitOthers

            #region RevenueOthers
            benifitInfo.RevenueOthers=0;
            if ( AccountingProvider.GetAccountID( "711" )!=Guid.Empty )
                benifitInfo.RevenueOthers=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "711" ) , AccountingProvider.GetAccountID( "911" ) , startDate , endDate , "" , true );

            #endregion

            #region CostOthers
            benifitInfo.CostOthers=0;
            if ( AccountingProvider.GetAccountID( "811" )!=Guid.Empty )
                benifitInfo.CostOthers=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "811" ) , startDate , endDate , "" , true );

            #endregion

            benifitInfo.BenifitOthers=benifitInfo.RevenueOthers-benifitInfo.CostOthers;

            #endregion

            benifitInfo.BenifitBeforeTax=benifitInfo.BenifitNet+benifitInfo.BenifitOthers;

            #region Income Tax

            #region CostIncomeTax
            benifitInfo.CostIncomeTax=0;
            if ( AccountingProvider.GetAccountID( "8211" )!=Guid.Empty )
                benifitInfo.CostIncomeTax=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "8211" ) , startDate , endDate , "" , true );

            #endregion

            #region CostIncomeTaxDelay
            benifitInfo.CostIncomeTaxDelay=0;
            if ( AccountingProvider.GetAccountID( "8212" )!=Guid.Empty )
                benifitInfo.CostIncomeTaxDelay=AccountingProvider.GetJournalAmount( AccountingProvider.GetAccountID( "911" ) , AccountingProvider.GetAccountID( "8212" ) , startDate , endDate , "" , true );

            #endregion

            #endregion

            benifitInfo.Benifit=benifitInfo.BenifitBeforeTax-benifitInfo.CostIncomeTax-benifitInfo.CostIncomeTaxDelay;

        }
        public static GLBenifitCalcsInfo BenifitCalculationOfRange ( DateTime? startDate , DateTime? endDate )
        {
            GLBenifitCalcsInfo benifitInfo=new GLBenifitCalcsInfo();
            BenifitCalculationOfRange( benifitInfo , startDate , endDate );
            return benifitInfo;
        }

        #endregion

        #region Post To GL

        static List<Guid> AccountForReCalcList=new List<Guid>();
        public static void PostBenifitCalculation ( GLBenifitCalcsInfo mainObject )
        {
            if ( mainObject.FK_GEPeriodID.HasValue==false||mainObject.GLBenifitCalcID==Guid.Empty )
                return;

            if ( AccountingProvider.GetAccountID( "911" )==Guid.Empty )
                return;

            if ( mainObject.FK_GLJournalVoucherID!=Guid.Empty )
            {
                new GLJournalEntrysController().RealDeleteObjectsByFK( "FK_GLJournalVoucherID" , mainObject.FK_GLJournalVoucherID.Value );
                Guid iID=mainObject.FK_GLJournalVoucherID.Value;
                mainObject.FK_GLJournalVoucherID=null;
                new GLBenifitCalcsController().UpdateObject( mainObject );
                new GLJournalVouchersController().RealDeleteObject( iID );
            }

            DateTime startDate=PeriodProvider.GetFirstDay( mainObject.FK_GEPeriodID.Value );
            DateTime endDate=PeriodProvider.GetLastDay( mainObject.FK_GEPeriodID.Value );

            #region Voucher
            GLJournalVouchersController voucherCtrl=new GLJournalVouchersController();
            GLJournalVouchersInfo voucher=new GLJournalVouchersInfo();
            BusinessObjectHelper.CopyObject( mainObject , voucher , false );

            //voucher.FK_GLVoucherTypeID="BenifitCalc";
            voucher.ERPVoucherID=mainObject.GLBenifitCalcID;
            voucher.ERPVoucherTableName="GLBenifitCalcs";
            voucher.JournalDate=endDate;

            voucher.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;

            voucher.Remark=String.Format( "Xác định kết quả kinh doanh tháng {0}/{1}" , startDate.Month , startDate.Year );

            voucherCtrl.CreateObject( voucher );

            mainObject.FK_GLJournalVoucherID=voucher.GLJournalVoucherID;
            mainObject.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;
            new GLBenifitCalcsController().UpdateObject( mainObject );
            #endregion

            Guid Acc911ID=AccountingProvider.GetAccountID( "911" );

            GLJournalEntrysController entryCtrl=new GLJournalEntrysController();
            List<GLJournalEntrysInfo> lstEntrys=new List<GLJournalEntrysInfo>();

            String strRemark=String.Format( " tháng {0}/{1}" , startDate.Month , startDate.Year );


            if ( AccountingProvider.GetAccountID( "511" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "511" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển doanh thu thuần bán hàng"+strRemark ) );
            if ( AccountingProvider.GetAccountID( "512" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "512" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển doanh thu thuần bán hàng nội bộ"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "632" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "632" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển giá vốn hàng bán"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "515" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "515" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển doanh thu hoạt động tài chính"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "635" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "635" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí tài chính"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "641" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "641" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí bán hàng"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "642" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "642" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí quản lý doanh nghiệp"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "711" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "711" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển thu nhập khác"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "811" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "811" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí khác"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "8211" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "8211" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí thuế TNDN phải nộp"+strRemark ) );

            if ( AccountingProvider.GetAccountID( "8212" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( AccountingProvider.GetAccountID( "8212" ) , Acc911ID , true , startDate , endDate , "" , "Kết chuyển chi phí thuế TNDN hoãn lại"+strRemark ) );

            foreach ( GLJournalEntrysInfo entry in lstEntrys )
            {
                if ( AccountForReCalcList.Contains( entry.FK_GLAccountID_Debit )==false )
                    AccountForReCalcList.Add( entry.FK_GLAccountID_Debit );
                if ( AccountForReCalcList.Contains( entry.FK_GLAccountID_Credit )==false )
                    AccountForReCalcList.Add( entry.FK_GLAccountID_Credit );

                entry.FK_GLJournalVoucherID=voucher.GLJournalVoucherID;
                entry.JournalDate=endDate;
                entry.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;
                entryCtrl.CreateObject( entry );
            }

            lstEntrys.Clear();

            if ( AccountingProvider.GetAccountID( "4212" )!=Guid.Empty )
                lstEntrys.AddRange( GenerateCloseEntrys( Acc911ID , AccountingProvider.GetAccountID( "4212" ) , true , startDate , endDate , "" , "Kết chuyển Lợi Nhuận"+strRemark ) );
            foreach ( GLJournalEntrysInfo entry in lstEntrys )
            {
                if ( AccountForReCalcList.Contains( entry.FK_GLAccountID_Debit )==false )
                    AccountForReCalcList.Add( entry.FK_GLAccountID_Debit );
                if ( AccountForReCalcList.Contains( entry.FK_GLAccountID_Credit )==false )
                    AccountForReCalcList.Add( entry.FK_GLAccountID_Credit );

                entry.FK_GLJournalVoucherID=voucher.GLJournalVoucherID;
                entry.JournalDate=endDate;
                entry.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;
                entryCtrl.CreateObject( entry );
            }

            BenifitCalculationOfPeriod( mainObject );
        }
        public static List<GLJournalEntrysInfo> GenerateCloseEntrys ( Guid fromAccID , Guid toAccID , bool isIncludeChildren , DateTime? startDate , DateTime? endDate , String strConditionQuery , String strRemark )
        {
            List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();

            GLAccountsController accCtrl=new GLAccountsController();
            GLAccountsInfo fromAccount=accCtrl.GetObjectByID( fromAccID ) as GLAccountsInfo;
            GLAccountsInfo toAccount=accCtrl.GetObjectByID( toAccID ) as GLAccountsInfo;
            if ( fromAccount==null||toAccount==null )
                return lstResults;

            #region Current
            double dbDebitAmt=AccountingProvider.GetDebitAmount( fromAccID , startDate , endDate , strConditionQuery , false );
            double dbCreditAmt=AccountingProvider.GetCreditAmount( fromAccID , startDate , endDate , strConditionQuery , false );
            double dbDiff=dbDebitAmt-dbCreditAmt;
            if ( dbDiff!=0 )
            {
                GLJournalEntrysInfo entry=new GLJournalEntrysInfo();
                entry.Remark=strRemark+String.Format( @" : TK {0} sang TK {1}" , fromAccount.No , toAccount.No );
                entry.EntryType=ABCCommon.ABCConstString.EntryTypePeriodEnding;
                if ( dbDiff>0 )
                {
                    entry.AmtTot=dbDiff;
                    entry.FK_GLAccountID_Debit=toAccID;
                    entry.FK_GLAccountID_Credit=fromAccID;
                }
                else
                {
                    entry.AmtTot=-dbDiff;
                    entry.FK_GLAccountID_Debit=fromAccID;
                    entry.FK_GLAccountID_Credit=toAccID;
                }
                entry.RaiseAmtTot=entry.AmtTot;
                entry.FK_GLAccountID_RaiseDebit=entry.FK_GLAccountID_Debit;
                entry.FK_GLAccountID_RaiseCredit=entry.FK_GLAccountID_Credit;

                lstResults.Add( entry );
            }
            #endregion

            if ( isIncludeChildren )
            {
                List<BusinessObject> lstChildren=accCtrl.GetListByForeignKey( "FK_GLParentAccountID" , fromAccount.GLAccountID );
                foreach ( GLAccountsInfo child in lstChildren )
                    lstResults.AddRange( GenerateCloseEntrys( child.GLAccountID , toAccID , true , startDate , endDate , strConditionQuery , strRemark ) );
            }
            return lstResults;
        }

        #endregion

        #endregion


    }

}
