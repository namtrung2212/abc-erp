using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace ABCPresent
{

    public enum ViewPermission
    {
        AllowView=0 ,
        AllowNew=1 ,
        AllowEdit=2 ,
        AllowDelete=3 ,
        AllowApprove=4 ,
        AllowLock=5 ,
        AllowPost=6
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
        Disable,
        None ,
        New ,
        Edit ,
        Duplicate,
        Delete ,
        Save ,
        Cancel ,
        Search ,
        Refresh ,
        Post ,
        Approve ,
        Lock ,
        Custom ,
        Print,
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

        public ABCStandardEventArg ()
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
