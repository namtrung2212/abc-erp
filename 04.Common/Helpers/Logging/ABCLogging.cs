using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace ABCDataLib.Utilities
{
    public static class ABCLogging
    {
        private static LoggingMessage LoggingDlg=new LoggingMessage();
        public static List<GELogMsgsInfo> ContentList=new List<GELogMsgsInfo>();
        public static Boolean IsLogToDatabase=false;
        public static GELogMsgsController msgCtrl=new GELogMsgsController();

        public static void LogNewMessage (string strModuleName,String strUserName,String strAction,String strDesc,String strStatus )
        {
            try
            {
                ABCLogging.LoggingDlg.Text=String.Format( "Logging History of {0} Module " , strModuleName );

                GELogMsgsInfo objGELogMsgsInfo=new GELogMsgsInfo();
                objGELogMsgsInfo.GELogMsgDate=DateTime.Now;
                objGELogMsgsInfo.GELogMsgMdl=strModuleName;
                objGELogMsgsInfo.GELogMsgUser=strUserName;
                objGELogMsgsInfo.GELogMsgAction=strAction;
                objGELogMsgsInfo.GELogMsgDesc=strDesc;
                objGELogMsgsInfo.GELogMsgStatus=strStatus;

                if ( ABCLogging.IsLogToDatabase )
                {
                    objGELogMsgsInfo.GELogMsgID=msgCtrl.CreateObject( objGELogMsgsInfo );
                }

                ContentList.Add( objGELogMsgsInfo );
                if ( ABCLogging.ContentList.Count>0 )
                {
                    if ( ABCLogging.LoggingDlg==null )
                        ABCLogging.LoggingDlg=new LoggingMessage();

                    ABCLogging.LoggingDlg.RefreshMessageList();
                    ABCLogging.LoggingDlg.TopMost=true;
                    ABCLogging.LoggingDlg.Location=new System.Drawing.Point( 200 , 200 );
                    ABCLogging.LoggingDlg.Show();

                }
            }
            catch ( Exception ex )
            {
                if ( ABCLogging.LoggingDlg!=null )
                    ABCLogging.LoggingDlg.Dispose();
            }
            Application.DoEvents();
        }

        public static void CloseDiaglog()
        {
            ABCLogging.LoggingDlg.Hide();

            foreach ( GELogMsgsInfo objGELogMsgsInfo in ABCLogging.ContentList )
           {
               if ( objGELogMsgsInfo.GELogMsgID<=0&&ABCLogging.IsLogToDatabase )
                   msgCtrl.CreateObject( objGELogMsgsInfo );
           }

           ABCLogging.ContentList.Clear();
        }

        public static void ShowRecentMessages()
        {
            if ( ABCLogging.IsLogToDatabase==false )
                return;

            DataSet  ds =msgCtrl.GetRecentMessage( 300 );
            ABCLogging.ContentList.Clear();
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow drow in ds.Tables[0].Rows )
                {
                    GELogMsgsInfo msgInfo=(GELogMsgsInfo)msgCtrl.GetObjectFromDataRow( drow );
                    if ( msgInfo!=null )
                        ABCLogging.ContentList.Insert(0, msgInfo );
                }
            }

            ABCLogging.LoggingDlg.RefreshMessageList();
        }

        public static void ShowLoggingMessageByModuleName(String strModuleName)
        {
            
                if ( ABCLogging.LoggingDlg==null )
                    ABCLogging.LoggingDlg=new LoggingMessage();

                DataSet ds=msgCtrl.GetAllMessageByModuleName( strModuleName );
                ABCLogging.ContentList.Clear();
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    foreach ( DataRow drow in ds.Tables[0].Rows )
                    {
                        GELogMsgsInfo msgInfo=(GELogMsgsInfo)msgCtrl.GetObjectFromDataRow( drow );
                        if ( msgInfo!=null )
                            ABCLogging.ContentList.Insert( 0 , msgInfo );
                    }
                }

                ABCLogging.LoggingDlg.RefreshMessageList();
                ABCLogging.LoggingDlg.Text=String.Format("Logging History of {0} Module ",strModuleName);
                ABCLogging.LoggingDlg.Show();

        }

    }
}
