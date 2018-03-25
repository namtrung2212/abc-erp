using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCProvider;
using ABCBusinessEntities;
using ABCScreen;
using System.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace ABCApp
{

    public partial class MeetingList : DevExpress.XtraEditors.XtraUserControl
    {
        ABCApp.MainForm mainForm;
        public bool SoundOn=true;

        public MeetingList ( MainForm form )
        {
            mainForm=form;

            InitializeComponent();

            this.gridViewMeetings.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridViewMeetings_CustomDrawCell );
            this.gridViewMeetings.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewMeetings.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridViewMeetings.Click+=new EventHandler( gridViewMeetings_Click );
            this.gridViewMeetings.MouseMove+=new MouseEventHandler( gridViewMeetings_MouseMove );
            this.gridViewMeetings.RowCellStyle+=new RowCellStyleEventHandler( gridViewMeetings_RowCellStyle );
            this.gridViewMeetings.ExpandAllGroups();

            ReloadMeetings();
        }


        #region AutoLoadTimer

        System.Windows.Forms.Timer AutoLoadTimer;
        public void StartTimer ( )
        {
            if ( AutoLoadTimer==null )
            {
                AutoLoadTimer=new Timer();
                AutoLoadTimer.Interval=15000;
                AutoLoadTimer.Tick+=new EventHandler( AutoLoadTimer_Tick );
                AutoLoadTimer.Start();
            }

        }
        void AutoLoadTimer_Tick ( object sender , EventArgs e )
        {
            ReloadMeetings();
        }
        #endregion

        #region Events

        void gridViewMeetings_Click ( object sender , EventArgs e )
        {
            Point pt=gridControl1.PointToClient( Control.MousePosition );

            GridHitInfo info=gridViewMeetings.CalcHitInfo( pt );
            if ( info!=null&&info.InRow||info.InRowCell )
            {
                DataRow dr=gridViewMeetings.GetDataRow( info.RowHandle );
                if ( dr==null )
                    return;
                ABCScreenManager.Instance.RunLink( "GEMeetings" , ABCCommon.ViewMode.Runtime , false , ABCHelper.DataConverter.ConvertToGuid( dr["GEMeetingID"] ) , ABCCommon.ABCScreenAction.None );
            }

        }

        void gridViewMeetings_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            DataRow dr=gridViewMeetings.GetDataRow( e.RowHandle );
            if ( dr==null )
                return;

            if ( e.Column.FieldName=="MeetingTitle" )
            {
                //SolidBrush brush=new SolidBrush( e.Appearance.BackColor );
                //e.Graphics.FillRectangle( brush , ( e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo ).Bounds );

                //if ( String.IsNullOrWhiteSpace( dr["PriorityLevel"].ToString() )==false )
                //    e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( dr["PriorityLevel"].ToString() ) , e.Bounds.Location );

                //Rectangle r=e.Bounds;
                //r.Width-=18;
                //r.X+=18;
                //e.Appearance.DrawString( e.Cache , e.DisplayText , r );

                //e.Handled=true;
            }
            else if ( e.Column.FieldName=="CreateTime" )
            {
                if ( e.CellValue!=null )
                {
                    DateTime time=Convert.ToDateTime( e.CellValue );
                    if ( time.Date==ABCApp.ABCDataGlobal.WorkingDate.Date )
                        e.DisplayText=String.Format( @"{0}:{1}" , time.Hour.ToString( "00" ) , time.Minute.ToString( "00" ) );
                    else
                        e.DisplayText=String.Format( @"{0,2}/{1,-2}" , time.Day , time.Month );
                }
            }
        }

        #endregion

        DataTable MeetingsTable=null;
        public void ReloadMeetings ( )
        {

            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM GEMeetings WHERE GEMeetings.CreateUser = '{0}' OR GEMeetingID IN ( SELECT FK_GEMeetingID FROM  GEMeetingMembers,ADUsers WHERE FK_ADUserID =ADUserID AND ADUsers.No='{0}' GROUP BY FK_GEMeetingID) ORDER BY CreateTime DESC" , ABCUserProvider.CurrentUserName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                if ( MeetingsTable!=null )
                    MeetingsTable.Dispose();
                MeetingsTable=ds.Tables[0];
            }
            RefreshDataSource();

            StartTimer();

        }

        public void RefreshDataSource ( )
        {
            this.gridControl1.DataSource=MeetingsTable;
            this.gridControl1.RefreshDataSource();
            this.gridViewMeetings.MoveFirst();
        }

        private void btnAdd_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ABCScreenManager.Instance.OpenScreenForNew( "GEMeetings" , ABCCommon.ViewMode.Runtime , true );
            ReloadMeetings();
        }

        private void btnEdit_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( this.gridViewMeetings.FocusedRowHandle>=0 )
            {
                DataRow dr=gridViewMeetings.GetDataRow( this.gridViewMeetings.FocusedRowHandle );
                if ( dr==null )
                    return;
                if ( dr["CreateUser"].ToString()==ABCUserProvider.CurrentUserName )
                {
                    ABCScreenManager.Instance.RunLink( "GEMeetings" , ABCCommon.ViewMode.Runtime , true , ABCHelper.DataConverter.ConvertToGuid( dr["GEMeetingID"] ) , ABCCommon.ABCScreenAction.Edit );
                    ReloadMeetings();
                }
                else
                {
                    ABCHelper.ABCMessageBox.Show( "Chỉ được sửa Hội Nghị do bạn tạo ra!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                }
            }
        }

        private void btnDelete_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( this.gridViewMeetings.FocusedRowHandle>=0 )
            {
                DataRow dr=gridViewMeetings.GetDataRow( this.gridViewMeetings.FocusedRowHandle );
                if ( dr==null )
                    return;

                Guid iID=ABCHelper.DataConverter.ConvertToGuid( dr["GEMeetingID"] );

                GEMeetingsController meetingCtrl=new GEMeetingsController();
                GEMeetingsInfo meeting=meetingCtrl.GetObjectByID( iID ) as GEMeetingsInfo;
                if ( meeting!=null )
                {
                    if ( meeting.CreateUser==ABCUserProvider.CurrentUserName )
                    {
                        if ( DialogResult.Yes==ABCHelper.ABCMessageBox.Show( "Bạn có thực sự muốn xóa Hội nghị này không?" , meeting.Title , MessageBoxButtons.YesNo , MessageBoxIcon.Question ) )
                        {
                            new GEMeetingMembersController().DeleteObjectsByFK( "FK_GEMeetingID" , iID );
                            meetingCtrl.DeleteObject( meeting );
                            ReloadMeetings();
                        }
                    }
                    else
                    {
                        ABCHelper.ABCMessageBox.Show( "Chỉ được sửa Hội Nghị do bạn tạo ra!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
            
                    }
                }

            }
        }


        #region Hot Track
        private int hotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;

        private int HotTrackRow
        {
            get
            {
                return hotTrackRow;
            }
            set
            {
                if ( hotTrackRow!=value )
                {
                    int prevHotTrackRow=hotTrackRow;
                    hotTrackRow=value;

                    gridViewMeetings.RefreshRow( prevHotTrackRow );
                    gridViewMeetings.RefreshRow( hotTrackRow );

                    if ( hotTrackRow>=0 )
                        gridControl1.Cursor=Cursors.Hand;
                    else
                        gridControl1.Cursor=Cursors.Default;
                }
            }
        }

        private void gridViewMeetings_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo info=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( info.InRowCell )
                HotTrackRow=info.RowHandle;
            else
                HotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void gridViewMeetings_RowCellStyle ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e )
        {
            if ( e.RowHandle==HotTrackRow )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Underline );
        }

        #endregion
    }
}
