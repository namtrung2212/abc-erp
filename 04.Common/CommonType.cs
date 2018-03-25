using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABCCommon
{
    public enum LoginType
    {
        Studio ,
        ERP
    }

    public enum ABCColumnType
    {
        ID=0 ,
        NO=1 ,
        NAME=2 ,
        ABCStatus=3 ,
        CreateUser=4 ,
        CreateDate=5 ,
        UpdateUser=6 ,
        UpdateDate=7 ,
        NoIndex=8 ,
        Remark=9
    }


    public class ABCConstString
    {
      
        public const String colABCStatus="ABCStatus";
        public const String colCreateUser="CreateUser";
        public const String colCreateTime="CreateTime";
        public const String colUpdateUser="UpdateUser";
        public const String colUpdateTime="UpdateTime";
        public const String colEditCount="EditCount";
        public const String colSelected="Selected";
        public const String colNoIndex="NoIndex";

        public const String colExchangeRate="ExchangeRate";

        public const String colDocumentDate="DocumentDate";
        public const String colVoucher="Voucher";
        public const String colVoucherDate="VoucherDate";
        public const String colApprovalStatus="ApprovalStatus";
        public const String colApprovedDate="ApprovedDate";
        public const String colLockStatus="LockStatus";
        public const String colJournalStatus="JournalStatus";
        public const String colJournalDate="JournalDate";
        public const String colRemark="Remark";


        public const String ABCStatusAlive="Alive";
        public const String ABCStatusDeleted="Deleted";
        public const String ABCStatusTemplate="Template";

        public const String ApprovalTypeApproved="Approved";
        public const String ApprovalTypeRejected="Rejected";
        public const String ApprovalTypeNew="New";

        public const String LockStatusNew="New";
        public const String LockStatusLocked="Locked";
        public const String LockStatusUnlocked="Unlocked";

        public const String PostStatusNew="New";
        public const String PostStatusPosted="Posted";
        public const String PostStatusUnPost="UnPost";


        public const String TimingStatusNotStart="NotStart";
        public const String TimingStatusWIP="WIP";
        public const String TimingStatusPause="Pause";
        public const String TimingStatusCompleted="Completed";

        public const String EntryTypeNormal="Normal";
        public const String EntryTypeERP="ERP";
        public const String EntryTypeERPModified="ERPModified";
        public const String EntryTypePeriodEnding="PeriodEnding";
    }

    public enum VoucherPermission
    {
        AllowView=0 ,
        AllowNew=1 ,
        AllowEdit=2 ,
        AllowDelete=3 ,
        AllowApproval=4 ,
        AllowLock=5 ,
        AllowPost=6,
        AllowPrint=7
    }

    public enum ViewPermission
    {
        AllowView=0
    }
    public enum TablePermission
    {
        AllowView=0 ,
        AllowNew=1 ,
        AllowEdit=2 ,
        AllowDelete=3
    }
    public enum FieldPermission
    {
        AllowView=0 ,
        AllowEdit=1
    }


    public enum ViewMode
    {
        Design ,
        Runtime ,
        Test
    }

    public enum ABCScreenAction
    {
        Disable ,
        None ,
        New ,
        Edit ,
        Duplicate ,
        Delete ,
        Save ,
        Cancel ,
        Search ,
        Refresh ,
        Approve ,
        Reject ,
        Lock ,
        UnLock ,
        Post ,
        UnPost ,
        Custom ,
        Print ,
        Info
    }
    public enum ABCScreenStatus
    {
        None ,
        LoadedData ,
        New ,
        Edit
    }

    public enum ABCRepositoryType : int
    {
        None=0 ,
        TextEdit=1 ,
        MemoEdit=2 ,
        ProgressBar=3 ,
        DateEdit=4 ,
        TimeEdit=5
    }
    public enum ABCSummaryType
    {
        None=0 ,
        SUM=1 ,
        MAX=2 ,
        MIN=3 ,
        AVG=4
    }
    public class ABCStandardEventArg
    {
        public object Tag;
        public bool Cancel;

        public ABCStandardEventArg ( )
        {
            Cancel=false;
        }
        public ABCStandardEventArg ( object obj )
        {
            Tag=obj;
            Cancel=false;
        }
    }
   
}
