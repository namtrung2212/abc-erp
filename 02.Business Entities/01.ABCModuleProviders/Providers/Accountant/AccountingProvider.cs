using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.Text;
using NCalc;
using ABCBusinessEntities;
using ABCProvider;
namespace ABCProvider
{
    public partial class AccountingProvider 
    {
        #region Account

        public enum AccountType
        {
            Debit ,
            Credit ,
            Multi
        }

        static Dictionary<String , GLAccountsInfo> AccountList=new Dictionary<string , GLAccountsInfo>();
      
        public static Guid GetAccountID ( String strAccountNo )
        {
            GLAccountsInfo account=null;
            if ( AccountList.TryGetValue( strAccountNo , out account )==false )
            {
                account=new GLAccountsController().GetObjectByNo( strAccountNo ) as GLAccountsInfo;
                AccountList.Add( strAccountNo , account );
            }
            if ( account!=null )
                return account.GLAccountID;
            return Guid.Empty;
        }
        public static GLAccountsInfo GetAccount ( String strAccountNo )
        {
            GLAccountsInfo account=null;
            if ( AccountList.TryGetValue( strAccountNo , out account )==false )
            {
                account=new GLAccountsController().GetObjectByNo( strAccountNo ) as GLAccountsInfo;
                AccountList.Add( strAccountNo , account );
            }

            return account;
        }
        public static List<GLAccountsInfo> GetAccounts ( String strAccountNo , bool isIncludeChildren )
        {
            List<GLAccountsInfo> lstResults=new List<GLAccountsInfo>();
            GLAccountsInfo account=GetAccount( strAccountNo );
            if ( account!=null )
            {
                lstResults.Add( account );
                if ( isIncludeChildren )
                {
                    List<BusinessObject> lstChildren=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , account.GLAccountID );
                    foreach ( GLAccountsInfo child in lstChildren )
                        lstResults.AddRange( GetAccounts( child.No , false ) );
                }
            }
            return lstResults;
        }
        public static List<GLAccountsInfo> GetAccounts ( List<String> lstAccounts , bool isIncludeChildren )
        {
            List<GLAccountsInfo> lstResults=new List<GLAccountsInfo>();
            foreach ( String strAccNo in lstAccounts )
                lstResults.AddRange( GetAccounts( strAccNo , isIncludeChildren ) );
            return lstResults;
        }
        public static List<GLAccountsInfo> GetAccountList ( String strFilterNoString )
        {
            return new GLAccountsController().GetList( String.Format( @"SELECT * FROM GLAccounts WHERE No like '{0}' " , strFilterNoString ) ).Cast<GLAccountsInfo>().ToList<GLAccountsInfo>();
        }
        public static Guid? GetCashAccountFromPaymentMethod ( GLJournalEntrysInfo entry , Guid methodID )
        {
            GEPaymentMethodsInfo methodInfo=new GEPaymentMethodsController().GetObjectByID( methodID ) as GEPaymentMethodsInfo;
            if ( methodInfo==null||String.IsNullOrWhiteSpace( methodInfo.PaymentMethod ) )
                return null;

            entry.FK_GEBankAccountID=methodInfo.FK_GEBankAccountID;
            if ( methodInfo.PaymentMethod=="CashOnHand" )
                return AccountingProvider.GetAccountID( "1111" );
            else if ( methodInfo.PaymentMethod=="Advance" )
                return AccountingProvider.GetAccountID( "141" );
            else if ( methodInfo.PaymentMethod=="CashInBank" )
            {
                if ( methodInfo.FK_GLAccountID.HasValue )
                    return methodInfo.FK_GLAccountID.Value;
                return AccountingProvider.GetAccountID( "1121" );
            }

            return AccountingProvider.GetAccountID( "1111" );
        }

        #region CalculateAccount
        public static void CalculateAccount ( Guid  iAccountID )
        {
            GLAccountsController accCtrl=new GLAccountsController();
            GLAccountsInfo accInfo=accCtrl.GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return;

            accInfo=CalculateAccount( accInfo , null , null , "" , true );

            accCtrl.UpdateObject( accInfo );

            if ( accInfo.FK_GLAccountID.HasValue )
                CalculateAccount( accInfo.FK_GLAccountID.Value );
        }

        public static GLAccountsInfo CalculateAccount ( GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsController accCtrl=new GLAccountsController();
            accInfo=accCtrl.GetObjectByID( accInfo.GLAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return null;

            #region BeginBalance

            if ( startDate.HasValue&&startDate.Value>SystemProvider.AppConfig.StartDate.Value )
            {
                accInfo.DebitBeginBalance=GetDebitAmount( accInfo.GLAccountID , null , startDate.Value.AddSeconds( -10 ) , strConditionQuery , isIncludeChildren );
                accInfo.CreditBeginBalance=GetCreditAmount( accInfo.GLAccountID , null , startDate.Value.AddSeconds( -10 ) , strConditionQuery , isIncludeChildren );
            }
            else
            {
                accInfo.DebitBeginBalance=GetDebitBeginBalance( accInfo );
                accInfo.CreditBeginBalance=GetCreditBeginBalance( accInfo );
            }

            if ( accInfo.DebitBeginBalance>accInfo.CreditBeginBalance )
            {
                accInfo.DebitBeginBalance-=accInfo.CreditBeginBalance;
                accInfo.CreditBeginBalance=0;
            }
            else
            {
                accInfo.CreditBeginBalance-=accInfo.DebitBeginBalance;
                accInfo.DebitBeginBalance=0;
            }
            #endregion

            accInfo.CurrentDebit=GetDebitAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
            accInfo.CurrentCredit=GetCreditAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );

            if ( accInfo.CurrentDebit>accInfo.CurrentCredit )
            {
                accInfo.CurrentDebit-=accInfo.CurrentCredit;
                accInfo.CurrentCredit=0;
            }
            else
            {
                accInfo.CurrentCredit-=accInfo.CurrentDebit;
                accInfo.CurrentDebit=0;
            }

            return accInfo;

        }
        #endregion

        public static double GetDebitBeginBalance ( GLAccountsInfo accInfo )
        {
            double dbResult=0;

            List<BusinessObject> lstAccounts=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
            foreach ( GLAccountsInfo accChildInfo in lstAccounts )
                dbResult+=GetDebitBeginBalance( accChildInfo );

            if ( dbResult<=0 )
                return accInfo.DebitBeginBalance;

            return dbResult;
        }
        public static double GetCreditBeginBalance ( GLAccountsInfo accInfo )
        {
            double dbResult=0;

            List<BusinessObject> lstAccounts=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
            foreach ( GLAccountsInfo accChildInfo in lstAccounts )
                dbResult+=GetCreditBeginBalance( accChildInfo );

            if ( dbResult<=0 )
                return accInfo.CreditBeginBalance;

            return dbResult;
        }

        public static double GetDebitAmount ( GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            double dbResult=0;

            String strQuery=String.Format( @"SELECT SUM(AmtTot) FROM GLJournalEntrys WHERE  ApprovalStatus='{0}' AND FK_GLAccountID_Debit={1} " , ABCCommon.ABCConstString.ApprovalTypeApproved , accInfo.GLAccountID );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );

            
            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            object objAmt=BusinessObjectController.GetData( strQuery );
            if ( objAmt!=null&&objAmt!=DBNull.Value )
                dbResult+=Convert.ToDouble( objAmt );

            if ( isIncludeChildren )
            {
                List<BusinessObject> lstChildren=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
                foreach ( GLAccountsInfo accChildInfo in lstChildren )
                    dbResult+=GetDebitAmount( accChildInfo , startDate , endDate , strConditionQuery , true );
                if ( lstChildren.Count<=0 )
                {
                    if ( startDate.HasValue==false||( startDate.HasValue&&startDate.Value<=SystemProvider.AppConfig.StartDate.Value ) )
                        dbResult+=accInfo.DebitBeginBalance;
                }
            }
            else
            {
                if ( startDate.HasValue==false||( startDate.HasValue&&startDate.Value<=SystemProvider.AppConfig.StartDate.Value ) )
                    dbResult+=accInfo.DebitBeginBalance;
            }

            return dbResult;
        }
        public static double GetDebitAmount ( Guid iAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsInfo accInfo=new GLAccountsController().GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return 0;

            return GetDebitAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
        }

        public static double GetCreditAmount ( GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            double dbResult=0;

            String strQuery=String.Format( @"SELECT SUM(AmtTot) FROM GLJournalEntrys WHERE ApprovalStatus='{0}' AND FK_GLAccountID_Credit='{1}' " , ABCCommon.ABCConstString.ApprovalTypeApproved , accInfo.GLAccountID );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );

            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            object objAmt=BusinessObjectController.GetData( strQuery );
            if ( objAmt!=null&&objAmt!=DBNull.Value )
                dbResult+=Convert.ToDouble( objAmt );

            if ( isIncludeChildren )
            {
                List<BusinessObject> lstChildren=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
                foreach ( GLAccountsInfo accChildInfo in lstChildren )
                    dbResult+=GetCreditAmount( accChildInfo , startDate , endDate , strConditionQuery , true );
                if ( lstChildren.Count<=0 )
                {
                    if ( startDate.HasValue==false||( startDate.HasValue&&startDate.Value<=SystemProvider.AppConfig.StartDate.Value ) )
                        dbResult+=accInfo.CreditBeginBalance;
                }
            }
            else
            {
                if ( startDate.HasValue==false||( startDate.HasValue&&startDate.Value<=SystemProvider.AppConfig.StartDate.Value ) )
                    dbResult+=accInfo.CreditBeginBalance;
            }

            return dbResult;
        }
        public static double GetCreditAmount ( Guid iAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {

            GLAccountsInfo accInfo=new GLAccountsController().GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return 0;

            return GetCreditAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
        }

        public static double GetAccountAmount ( AccountType type , GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            double debit=GetDebitAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
            double credit=GetCreditAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );

            if ( type==AccountType.Debit )
                return debit-credit;
            else if ( type==AccountType.Credit )
                return credit-debit;
            else
            {
                if ( debit>credit )
                {
                    debit-=credit;
                    return debit-credit;
                }
                else
                {
                    credit-=debit;
                    return credit-debit;
                }
            }
            return 0;
        }
        public static double GetAccountAmount ( AccountType type , Guid iAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsInfo accInfo=new GLAccountsController().GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return 0;

            return GetAccountAmount( type , accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );

        }
        public static double GetAccountAmount ( AccountType type , List<String> lstAccounts , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            double debitTot=0;
            double creditTot=0;

            GLAccountsController accountCtrl=new GLAccountsController();
            foreach ( String strAccNo in lstAccounts )
            {
                GLAccountsInfo accInfo=accountCtrl.GetObjectByNo( strAccNo ) as GLAccountsInfo;
                if ( accInfo!=null )
                {
                    debitTot+=GetDebitAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
                    creditTot+=GetCreditAmount( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
                }
            }

            if ( type==AccountType.Debit )
                return debitTot-creditTot;
            else if ( type==AccountType.Credit )
                return creditTot-debitTot;
            else
            {
                if ( debitTot>creditTot )
                {
                    debitTot-=creditTot;
                    return debitTot-creditTot;
                }
                else
                {
                    creditTot-=debitTot;
                    return creditTot-debitTot;
                }
            }
        }

        public static double GetJournalAmount ( Guid debitAccountID , Guid creditAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsInfo debitAccount=new GLAccountsController().GetObjectByID( debitAccountID ) as GLAccountsInfo;
            GLAccountsInfo creditAccount=new GLAccountsController().GetObjectByID( creditAccountID ) as GLAccountsInfo;
            if ( debitAccount==null||creditAccount==null )
                return 0;

            return GetJournalAmount( debitAccount , creditAccount , startDate , endDate , strConditionQuery , isIncludeChildren );
        }
        public static double GetJournalAmount ( GLAccountsInfo debitAccount , GLAccountsInfo creditAccount , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            double dbResult=0;

            String strQuery=String.Format( @"SELECT SUM(AmtTot) FROM GLJournalEntrys WHERE ApprovalStatus='{0}' AND FK_GLAccountID_Debit='{1}' AND FK_GLAccountID_Credit='{2}' " , ABCCommon.ABCConstString.ApprovalTypeApproved , debitAccount.GLAccountID , creditAccount.GLAccountID );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );

            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            object objAmt=BusinessObjectController.GetData( strQuery );
            if ( objAmt!=null&&objAmt!=DBNull.Value )
                dbResult+=Convert.ToDouble( objAmt );


            if ( isIncludeChildren )
            {
                List<BusinessObject> lstChildren=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , creditAccount.GLAccountID );
                foreach ( GLAccountsInfo creditChild in lstChildren )
                    dbResult+=GetJournalAmount( debitAccount , creditChild , startDate , endDate , strConditionQuery , true );

                lstChildren=new GLAccountsController().GetListByForeignKey( "FK_GLAccountID" , debitAccount.GLAccountID );
                foreach ( GLAccountsInfo debitChild in lstChildren )
                    dbResult+=GetJournalAmount( debitChild , creditAccount , startDate , endDate , strConditionQuery , true );
            }

            return dbResult;
        }
        public static double GetJournalAmount ( List<String> lstDebitAccounts , List<String> lstCreditAccounts , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            List<GLAccountsInfo> lstDebits=GetAccounts( lstDebitAccounts , isIncludeChildren );
            List<GLAccountsInfo> lstCredits=GetAccounts( lstCreditAccounts , isIncludeChildren );

            String strDebit=String.Empty;
            foreach ( GLAccountsInfo acc in lstDebits )
            {
                if ( String.IsNullOrEmpty( strDebit )==false )
                    strDebit+=" OR ";
                strDebit+=String.Format( @" FK_GLAccountID_Debit='{0}' " , acc.GLAccountID );
            }
            strDebit="("+strDebit+")";

            String stCredit=String.Empty;
            foreach ( GLAccountsInfo acc in lstCredits )
            {
                if ( String.IsNullOrEmpty( stCredit )==false )
                    stCredit+=" OR ";
                stCredit+=String.Format( @" FK_GLAccountID_Credit='{0}' " , acc.GLAccountID );
            }
            stCredit="("+stCredit+")";


            double dbResult=0;


            String strQuery=String.Format( @"SELECT SUM(AmtTot) FROM GLJournalEntrys WHERE ApprovalStatus='{0}' AND {1} AND {2}  " , ABCCommon.ABCConstString.ApprovalTypeApproved , strDebit , stCredit );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );

            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            object objAmt=BusinessObjectController.GetData( strQuery );
            if ( objAmt!=null&&objAmt!=DBNull.Value )
                dbResult+=Convert.ToDouble( objAmt );

            return dbResult;
        }


        public static List<GLJournalEntrysInfo> GetDebitEntrys ( GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            String strQuery=String.Format( @"SELECT * FROM GLJournalEntrys WHERE ApprovalStatus='{0}' AND FK_GLAccountID_Debit='{1}' " , ABCCommon.ABCConstString.ApprovalTypeApproved , accInfo.GLAccountID );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );
            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            strQuery+=String.Format( @" ORDER BY JournalDate " );

            List<BusinessObject> lstResults=new GLJournalEntrysController().GetList( strQuery );

            if ( isIncludeChildren )
            {
                GLAccountsController accCtrl=new GLAccountsController();
                DataSet ds=accCtrl.GetDataSetByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                    {
                        GLAccountsInfo accChildInfo=accCtrl.GetObjectFromDataRow( dr ) as GLAccountsInfo;
                        if ( accChildInfo!=null )
                            lstResults.AddRange( GetDebitEntrys( accChildInfo , startDate , endDate , strConditionQuery , true ) );
                    }
                }
            }

            return lstResults.ConvertAll<GLJournalEntrysInfo>( delegate( BusinessObject item ) { return (GLJournalEntrysInfo)item; } );
        }
        public static List<GLJournalEntrysInfo> GetDebitEntrys ( Guid iAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsInfo accInfo=new GLAccountsController().GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return new List<GLJournalEntrysInfo>();

            return GetDebitEntrys( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
        }
        public static List<GLJournalEntrysInfo> GetDebitEntrys ( List<String> lstAccounts , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();
            GLAccountsController accountCtrl=new GLAccountsController();

            foreach ( String strAccNo in lstAccounts )
            {
                GLAccountsInfo accInfo=accountCtrl.GetObjectByNo( strAccNo ) as GLAccountsInfo;
                if ( accInfo!=null )
                    lstResults.AddRange( GetDebitEntrys( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren ) );
            }
            return lstResults;
        }

        public static List<GLJournalEntrysInfo> GetCreditEntrys ( GLAccountsInfo accInfo , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            String strQuery=String.Format( @"SELECT * FROM GLJournalEntrys WHERE ApprovalStatus='{0}' AND FK_GLAccountID_Credit='{1}' " , ABCCommon.ABCConstString.ApprovalTypeApproved , accInfo.GLAccountID );
            if ( startDate.HasValue )
                strQuery+=String.Format( @" AND {0}" , TimeProvider.GenCompareDateTime( "JournalDate" , ">=" , startDate.Value ) );
            if ( endDate.HasValue )
                strQuery+=String.Format( @" AND {0} " , TimeProvider.GenCompareDateTime( "JournalDate" , "<=" , endDate.Value ) );
            if ( String.IsNullOrWhiteSpace( strConditionQuery )==false )
                strQuery+=String.Format( @" AND {0} " , strConditionQuery );

            strQuery+=String.Format( @" ORDER BY JournalDate " );

            List<BusinessObject> lstResults=new GLJournalEntrysController().GetList( strQuery );

            if ( isIncludeChildren )
            {
                GLAccountsController accCtrl=new GLAccountsController();
                DataSet ds=accCtrl.GetDataSetByForeignKey( "FK_GLAccountID" , accInfo.GLAccountID );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                    {
                        GLAccountsInfo accChildInfo=accCtrl.GetObjectFromDataRow( dr ) as GLAccountsInfo;
                        if ( accChildInfo!=null )
                            lstResults.AddRange( GetCreditEntrys( accChildInfo , startDate , endDate , strConditionQuery , true ) );
                    }
                }
            }

            return lstResults.ConvertAll<GLJournalEntrysInfo>( delegate( BusinessObject item ) { return (GLJournalEntrysInfo)item; } );
        }
        public static List<GLJournalEntrysInfo> GetCreditEntrys ( Guid iAccountID , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            GLAccountsInfo accInfo=new GLAccountsController().GetObjectByID( iAccountID ) as GLAccountsInfo;
            if ( accInfo==null )
                return new List<GLJournalEntrysInfo>();

            return GetCreditEntrys( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren );
        }
        public static List<GLJournalEntrysInfo> GetCreditEntrys ( List<String> lstAccounts , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();
            GLAccountsController accountCtrl=new GLAccountsController();

            foreach ( String strAccNo in lstAccounts )
            {
                GLAccountsInfo accInfo=accountCtrl.GetObjectByNo( strAccNo ) as GLAccountsInfo;
                if ( accInfo!=null )
                    lstResults.AddRange( GetCreditEntrys( accInfo , startDate , endDate , strConditionQuery , isIncludeChildren ) );
            }
            return lstResults;
        }

        public static List<GLJournalEntrysInfo> GetEntrys ( List<String> lstAccounts , DateTime? startDate , DateTime? endDate , String strConditionQuery , bool isIncludeChildren )
        {
            List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();
            lstResults.AddRange( GetDebitEntrys( lstAccounts , startDate , endDate , strConditionQuery , isIncludeChildren ) );
            lstResults.AddRange( GetCreditEntrys( lstAccounts , startDate , endDate , strConditionQuery , isIncludeChildren ) );

            return lstResults;
        }

        #endregion

        #region Entry
       
        public static void InvalidateJournalEntry ( GLJournalEntrysInfo entry , GLJournalVouchersInfo voucher , BusinessObject objSourceObj )
        {
            BusinessObjectHelper.CopyFKFields( objSourceObj , entry , true );
            BusinessObjectHelper.CopyFKFields( voucher , entry , true );
            BusinessObjectHelper.CopyField( objSourceObj , entry , "JournalDate" , false );

            BusinessObjectHelper.CopyField( objSourceObj , entry , "Voucher" , false );
            BusinessObjectHelper.CopyField( objSourceObj , entry , "VoucherContent" , false );
            BusinessObjectHelper.CopyField( objSourceObj , entry , "VoucherDate" , false );

            if ( String.IsNullOrWhiteSpace( entry.EntryType ) )
                entry.EntryType=ABCCommon.ABCConstString.EntryTypeERP;
            entry.RaiseAmtTot=entry.AmtTot;

            entry.FK_GLAccountID_RaiseCredit=entry.FK_GLAccountID_Credit;
            entry.FK_GLAccountID_RaiseDebit=entry.FK_GLAccountID_Debit;

            InvalidateJournalEntry( entry , voucher );
        }
        public static void InvalidateJournalEntry ( GLJournalEntrysInfo entry , GLJournalVouchersInfo voucher )
        {
            if ( entry==null )
                return;

            if ( voucher!=null )
            {
                BusinessObjectHelper.CopyFKFields( voucher , entry , true );

                entry.FK_GLJournalVoucherID=voucher.GLJournalVoucherID;
                entry.ApprovalStatus=voucher.ApprovalStatus;
                entry.JournalDate=voucher.JournalDate;

                if ( String.IsNullOrWhiteSpace( entry.Voucher ) )
                    entry.Voucher=voucher.Voucher;

                if ( String.IsNullOrWhiteSpace( entry.VoucherContent ) )
                    entry.VoucherContent=voucher.VoucherContent;

                if ( entry.VoucherDate.HasValue==false )
                    entry.VoucherDate=voucher.VoucherDate;
            }

            if ( entry.EntryType==ABCCommon.ABCConstString.EntryTypeERP )
            {
                if ( entry.RaiseAmtTot!=entry.AmtTot||entry.FK_GLAccountID_RaiseDebit.HasValue==false||entry.FK_GLAccountID_RaiseDebit.Value!=entry.FK_GLAccountID_Debit
                                                                    ||entry.FK_GLAccountID_RaiseCredit.HasValue==false||entry.FK_GLAccountID_RaiseCredit.Value!=entry.FK_GLAccountID_Credit )
                    entry.EntryType=ABCCommon.ABCConstString.EntryTypeERPModified;
            }
            if ( String.IsNullOrWhiteSpace( entry.EntryType ) )
                entry.EntryType=ABCCommon.ABCConstString.EntryTypeNormal;
            
            #region ObjectDesc
            entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_ASFixedAssetID" , entry.FK_ASFixedAssetID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_HREmployeeID" , entry.FK_HREmployeeID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "MAPartners" , entry.FK_MAPartnerID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_GECompanyUnitID" , entry.FK_GECompanyUnitID );
        
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_GLMonitorObjectID" , entry.FK_GLMonitorObjectID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_PMProjectID" , entry.FK_PMProjectID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_COCostGroupID" , entry.FK_COCostGroupID );
            if ( String.IsNullOrWhiteSpace( entry.ObjectDesc ) )
                entry.ObjectDesc=BusinessObjectHelper.GetNameValue( "FK_GEBankAccountID" , entry.FK_GEBankAccountID );
            #endregion
        }

        public static GLJournalEntrysInfo GenerateJournalEntry ( BusinessObject objItem , String strDebitNo , String strCreditNo , double dbAmt , String strDesc )
        {
            GLAccountsController accCtrl=new GLAccountsController();

            GLJournalEntrysInfo entry=new GLJournalEntrysInfo();
            BusinessObjectHelper.CopyObject( objItem , entry , false );
            InvalidateJournalEntry( entry , null );
            entry.Remark=strDesc;

            entry.FK_GLAccountID_Debit=ABCHelper.DataConverter.ConvertToGuid( accCtrl.GetIDByNo( strDebitNo ));
            entry.FK_GLAccountID_RaiseDebit=entry.FK_GLAccountID_Debit;

            entry.FK_GLAccountID_Credit=ABCHelper.DataConverter.ConvertToGuid( accCtrl.GetIDByNo( strCreditNo ) );
            entry.FK_GLAccountID_RaiseCredit=entry.FK_GLAccountID_Credit;

            entry.AmtTot=dbAmt;
            return entry;

        }
        public static GLJournalEntrysInfo GenerateJournalEntry ( BusinessObject objItem , Guid iDebitID , Guid iCreditID , double dbAmt , String strDesc )
        {
            GLJournalEntrysInfo entry=new GLJournalEntrysInfo();
            BusinessObjectHelper.CopyObject( objItem , entry , false );
            InvalidateJournalEntry( entry , null );
            entry.Remark=strDesc;

            entry.FK_GLAccountID_Debit=iDebitID;
            entry.FK_GLAccountID_RaiseDebit=entry.FK_GLAccountID_Debit;

            entry.FK_GLAccountID_Credit=iCreditID;
            entry.FK_GLAccountID_RaiseCredit=entry.FK_GLAccountID_Credit;

            entry.AmtTot=dbAmt;
            return entry;

        }
        public static List<GLJournalEntrysInfo> GenerateJournalEntrys ( GLBatchEntryConfigsInfo batchConfig )
        {
            List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();

            GLBatchEntryConfigItemsController itemConfigCtrl=new GLBatchEntryConfigItemsController();
            List<BusinessObject> lstItemConfigs=itemConfigCtrl.GetListFromDataset( itemConfigCtrl.GetDataSetByForeignKey( "FK_GLBatchEntryConfigID" , batchConfig.GLBatchEntryConfigID ) );
            foreach ( GLBatchEntryConfigItemsInfo item in lstItemConfigs )
            {
                if ( String.IsNullOrWhiteSpace( item.Expression ) )
                    continue;

                GLJournalEntrysInfo entry=new GLJournalEntrysInfo();
                entry.FK_GLAccountID_Debit=item.FK_GLAccountID_Debit;
                entry.FK_GLAccountID_Credit=item.FK_GLAccountID_Credit;
                entry.Remark=item.Remark;
                entry.FK_COCostGroupID=item.FK_COCostGroupID;
                entry.CostType=item.CostType;
                if ( batchConfig.JournalDate.HasValue )
                    entry.JournalDate=batchConfig.JournalDate.Value;
                else if ( batchConfig.VoucherDate.HasValue )
                    entry.JournalDate=batchConfig.VoucherDate.Value;
                else
                    entry.JournalDate=new DateTime();

                entry.VoucherDate=batchConfig.VoucherDate;
                entry.Voucher=batchConfig.Voucher;
                entry.VoucherContent=batchConfig.VoucherContent;

                #region AmtTot
                String strExpression=item.Expression;
                for ( int i=1; i<=20; i++ )
                    strExpression=strExpression.Replace( "{"+i+"}" , "["+i+"]" );

                try
                {
                    Expression e=new Expression( strExpression );

                    for ( int i=1; i<=20; i++ )
                    {
                        if ( strExpression.Contains( "["+i+"]" ) )
                        {
                            object objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( batchConfig , "Param"+i );
                            e.Parameters[i.ToString()]=Convert.ToDouble( objValue );
                        }
                    }

                    object objAmt=e.Evaluate();
                    entry.AmtTot=Math.Round( Convert.ToDouble( objAmt ) , 3 );
                }
                catch ( Exception ex )
                {
                    ABCHelper.ABCMessageBox.Show( String.Format( "Công thức cho nghiệp vụ '{0}' không đúng !" , item.Remark ) , "Thông báo" , System.Windows.Forms.MessageBoxButtons.OK , System.Windows.Forms.MessageBoxIcon.Error );
                    continue;
                }
                #endregion

                if ( entry.AmtTot>0 )
                    lstResults.Add( entry );
            }

            return lstResults;

        }
     
        #endregion

        #region Vouchers

        public static bool IsAutoGenEntry ( )
        {
            return true;
        }

        public static void ApproveJournalVoucher ( GLJournalVouchersInfo voucher )
        {
            if ( voucher.GLJournalVoucherID==Guid.Empty )
                return;

            if ( voucher.ApprovalStatus==ABCCommon.ABCConstString.ApprovalTypeApproved )
            {
                if ( DialogResult.No==ABCHelper.ABCMessageBox.Show( "Phiếu kế toán đã được ghi sổ trước đó. Bạn có muốn ghi sổ lại không?" , "Ghi sổ" , MessageBoxButtons.YesNo , MessageBoxIcon.Question ) )
                    return;
            }

            voucher.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;
            new GLJournalVouchersController().UpdateObject( voucher );

            GLJournalEntrysController entryCtrl=new GLJournalEntrysController();
            List<Guid> lstAccounts=new List<Guid>();
            foreach ( GLJournalEntrysInfo entry in entryCtrl.GetListByForeignKey( "FK_GLJournalVoucherID" , voucher.GLJournalVoucherID ) )
            {
                InvalidateJournalEntry( entry , voucher );

                entry.ApprovalStatus=ABCCommon.ABCConstString.ApprovalTypeApproved;
                entry.JournalDate=voucher.JournalDate;
                entryCtrl.UpdateObject( entry );

                if ( lstAccounts.Contains( entry.FK_GLAccountID_Debit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Debit );
                if ( lstAccounts.Contains( entry.FK_GLAccountID_Credit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Credit );
            }
            foreach ( Guid iAccountID in lstAccounts )
                AccountingProvider.CalculateAccount( iAccountID );
        }
        public static void ApproveJournalVoucher ( Guid iVoucherID )
        {
            GLJournalVouchersInfo voucher=new GLJournalVouchersController().GetObjectByID( iVoucherID ) as GLJournalVouchersInfo;
            if ( voucher!=null )
                ApproveJournalVoucher( voucher );
        }
        public static void ApproveJournalVoucher ( Guid? iVoucherID )
        {
            if ( iVoucherID.HasValue )
                ApproveJournalVoucher( iVoucherID.Value );
        }

        public static void LockJournalVoucher ( GLJournalVouchersInfo voucher )
        {
            if ( voucher.GLJournalVoucherID==Guid.Empty )
                return;

            voucher.LockStatus=ABCCommon.ABCConstString.LockStatusLocked;
            new GLJournalVouchersController().UpdateObject( voucher );

            GLJournalEntrysController entryCtrl=new GLJournalEntrysController();
            List<Guid> lstAccounts=new List<Guid>();
            foreach ( GLJournalEntrysInfo entry in new GLJournalEntrysController().GetListByForeignKey( "FK_GLJournalVoucherID" , voucher.GLJournalVoucherID ) )
            {
                InvalidateJournalEntry( entry , voucher );

                entry.LockStatus=ABCCommon.ABCConstString.LockStatusLocked;
                entry.JournalDate=voucher.JournalDate;
                entryCtrl.UpdateObject( entry );

                if ( lstAccounts.Contains( entry.FK_GLAccountID_Debit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Debit );
                if ( lstAccounts.Contains( entry.FK_GLAccountID_Credit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Credit );
            }
            foreach ( Guid iAccountID in lstAccounts )
                AccountingProvider.CalculateAccount( iAccountID );
        }
        public static void LockJournalVoucher ( Guid iVoucherID )
        {
            GLJournalVouchersInfo voucher=new GLJournalVouchersController().GetObjectByID( iVoucherID ) as GLJournalVouchersInfo;
            if ( voucher!=null )
                LockJournalVoucher( voucher );
        }
        public static void LockJournalVoucher ( Guid? iVoucherID )
        {
            if ( iVoucherID.HasValue )
                LockJournalVoucher( iVoucherID.Value );
        }

        public static void DeleteJournalVoucher ( GLJournalVouchersInfo voucher )
        {
            if ( voucher.GLJournalVoucherID==Guid.Empty )
                return;

            DeleteJournalVoucher( voucher.GLJournalVoucherID );
        }
        public static void DeleteJournalVoucher ( Guid iVoucherID )
        {
            List<Guid> lstAccounts=new List<Guid>();
            foreach ( GLJournalEntrysInfo entry in new GLJournalEntrysController().GetListByForeignKey( "FK_GLJournalVoucherID" , iVoucherID ) )
            {
                if ( lstAccounts.Contains( entry.FK_GLAccountID_Debit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Debit );
                if ( lstAccounts.Contains( entry.FK_GLAccountID_Credit )==false )
                    lstAccounts.Add( entry.FK_GLAccountID_Credit );
            }

            new GLJournalEntrysController().DeleteObjectsByFK( "FK_GLJournalVoucherID" , iVoucherID );
            new GLJournalVouchersController().DeleteObject( iVoucherID );

            foreach ( Guid iAccountID in lstAccounts )
                AccountingProvider.CalculateAccount( iAccountID );
        }
        public static void DeleteJournalVoucher ( Guid? iVoucherID )
        {
            if ( iVoucherID.HasValue )
                DeleteJournalVoucher( iVoucherID.Value );
        }
        
        public static void InvalidateJournalVoucher ( IList<GLJournalEntrysInfo> lstEntrys , GLJournalVouchersInfo voucher )
        {

            List<String> lstVouchers=new List<string>();
            foreach ( GLJournalEntrysInfo entry in lstEntrys )
            {
                InvalidateJournalEntry( entry , voucher );
                if ( lstVouchers.Contains( entry.Voucher )==false )
                    lstVouchers.Add( entry.Voucher );
            }

            #region voucher.Voucher
            voucher.Voucher=String.Empty;
            if ( String.IsNullOrWhiteSpace( voucher.ERPVoucherNo )==false )
            {
                voucher.Voucher=voucher.ERPVoucherNo;
            }
            else
            {
                foreach ( string strVoucherNo in lstVouchers )
                {
                    if ( String.IsNullOrWhiteSpace( voucher.Voucher ) )
                        voucher.Voucher=strVoucherNo;
                    else
                        voucher.Voucher+=( ";"+strVoucherNo );
                }
            }
            #endregion
        }

        #region Relation

        public static Dictionary<String , List<BusinessObject>> GetSourceVouchers ( Guid iGLVoucherID )
        {
            return new Dictionary<string , List<BusinessObject>>();
        }
        public static void UpdateVoucherRelations ( Guid iGLVoucherID , List<BusinessObject> lstSourceVouchers )
        {
        }

        #endregion

        #endregion
    }
}
