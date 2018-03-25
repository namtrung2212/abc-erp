using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ABCDataLib.Utilities
{



    public partial class LoggingMessage : DevExpress.XtraEditors.XtraForm
    {

        public LoggingMessage ( )
        {
            InitializeComponent();
            this.gridCtrlMessage.DataSource=ABCLogging.ContentList;
            this.gridViewMessages.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( GridViewMessages_CustomDrawCell );
            this.Activated+=new EventHandler( LoggingMessage_Activated );
        }

        void LoggingMessage_Activated ( object sender , EventArgs e )
        {
            this.TopMost=true;
        }

        void GridViewMessages_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.RowHandle>=0 )
            {
                GELogMsgsInfo msgInfo=( this.gridViewMessages as DevExpress.XtraGrid.Views.Grid.GridView ).GetRow( e.RowHandle ) as GELogMsgsInfo;
                if ( msgInfo!=null )
                {
                    if ( msgInfo.GELogMsgStatus.ToUpper().Contains("FAIL"))
                        e.Appearance.BackColor=Color.Salmon;
                }
            }
        }

        private void BtnClose_Click ( object sender , EventArgs e )
        {
            ABCLogging.CloseDiaglog();
        }

        public void RefreshMessageList()
        {
              if (ABCLogging.ContentList.Count>5000)
              {
                    ABCLogging.ContentList.Clear();
              }
            this.gridCtrlMessage.DataSource=ABCLogging.ContentList;
            this.gridCtrlMessage.RefreshDataSource();
            this.gridViewMessages.FocusedRowHandle=this.gridViewMessages.RowCount-1;
        }

        private void RecentMessage_LinkClicked ( object sender , LinkLabelLinkClickedEventArgs e )
        {
            ABCLogging.ShowRecentMessages();
        }

    }

}