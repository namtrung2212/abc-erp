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

    public partial class AssignedTaskList : DevExpress.XtraEditors.XtraUserControl
    {
        ABCApp.MainForm mainForm;
        public bool SoundOn=true;

        public AssignedTaskList ( MainForm form )
        {
            mainForm=form;

            InitializeComponent();

            this.gridViewTasks.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridView1_CustomDrawCell );
            this.gridViewTasks.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewTasks.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridViewTasks.Click+=new EventHandler( gridViewTasks_Click );
            this.gridViewTasks.MouseMove+=new MouseEventHandler( gridViewTasks_MouseMove );
            this.gridViewTasks.RowCellStyle+=new RowCellStyleEventHandler( gridViewTasks_RowCellStyle );
            this.gridViewTasks.ExpandAllGroups();

            ReloadTasks();
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
            ReloadTasks();
        }
        #endregion

        #region Events

        void gridViewTasks_Click ( object sender , EventArgs e )
        {
            Point pt=gridControl1.PointToClient( Control.MousePosition );

            GridHitInfo info=gridViewTasks.CalcHitInfo( pt );
            if ( info!=null&&info.InRow||info.InRowCell )
            {
                DataRow dr=gridViewTasks.GetDataRow( info.RowHandle );
                if ( dr==null )
                    return;
                ABCScreenManager.Instance.RunLink( "ADUserTasks" , ABCCommon.ViewMode.Runtime , false , ABCHelper.DataConverter.ConvertToGuid( dr["ADUserTaskID"] ) , ABCCommon.ABCScreenAction.None );
            }

        }

        void gridView1_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            DataRow dr=gridViewTasks.GetDataRow( e.RowHandle );
            if ( dr==null )
                return;

            if ( dr["TaskStatus"].ToString()==ABCCommon.ABCConstString.TimingStatusCompleted )
            {
                e.Appearance.ForeColor=Color.Gray;
                e.Appearance.Options.UseForeColor=true;
            }
            else
            {
                e.Appearance.Font = new Font(e.Appearance.Font,e.Appearance.Font.Style|FontStyle.Bold);
            }

            if ( e.Column.FieldName=="Title" )
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
            else if ( e.Column.FieldName=="EstStartTime"||e.Column.FieldName=="EstEndTime" )
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

        DataTable TasksTable=null;
        public void ReloadTasks ( )
        {

            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM ADUserTasks WHERE ADUserTasks.CreateUser = '{0}'  ORDER BY CreateTime DESC" , ABCUserProvider.CurrentUserName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                if ( TasksTable!=null )
                    TasksTable.Dispose();
                TasksTable=ds.Tables[0];
            }
            RefreshDataSource();

            StartTimer();

        }

        public void RefreshDataSource ( )
        {
            this.gridControl1.DataSource=TasksTable;
            this.gridControl1.RefreshDataSource();
            this.gridViewTasks.MoveFirst();
        }

        private void btnAdd_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ABCScreenManager.Instance.OpenScreenForNew( "ADUserTasks" , ABCCommon.ViewMode.Runtime , true );
            ReloadTasks();
        }

        private void btnEdit_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( this.gridViewTasks.FocusedRowHandle>=0 )
            {
                DataRow dr=gridViewTasks.GetDataRow( this.gridViewTasks.FocusedRowHandle );
                if ( dr==null )
                    return;
                if ( dr["CreateUser"].ToString()==ABCUserProvider.CurrentUserName )
                {
                    ABCScreenManager.Instance.RunLink( "ADUserTasks" , ABCCommon.ViewMode.Runtime , true , ABCHelper.DataConverter.ConvertToGuid( dr["ADUserTaskID"] ) , ABCCommon.ABCScreenAction.Edit );
                    ReloadTasks();
                }
                else
                {
                    ABCHelper.ABCMessageBox.Show( "Chỉ được sửa Nhiệm Vụ do bạn tạo ra!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                }
            }
        }

        private void btnDelete_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( this.gridViewTasks.FocusedRowHandle>=0 )
            {
                DataRow dr=gridViewTasks.GetDataRow( this.gridViewTasks.FocusedRowHandle );
                if ( dr==null )
                    return;

                Guid iID=ABCHelper.DataConverter.ConvertToGuid( dr["ADUserTaskID"] );

                ADUserTasksController TaskCtrl=new ADUserTasksController();
                ADUserTasksInfo Task=TaskCtrl.GetObjectByID( iID ) as ADUserTasksInfo;
                if ( Task!=null )
                {
                    if ( Task.CreateUser==ABCUserProvider.CurrentUserName )
                    {
                        if ( DialogResult.Yes==ABCHelper.ABCMessageBox.Show( "Bạn có thực sự muốn xóa Nhiệm Vụ này không?" , Task.Title , MessageBoxButtons.YesNo , MessageBoxIcon.Question ) )
                        {
                            new ADUserTaskMembersController().DeleteObjectsByFK( "FK_ADUserTaskID" , iID );
                            TaskCtrl.DeleteObject( Task );
                            ReloadTasks();
                        }
                    }
                    else
                    {
                        ABCHelper.ABCMessageBox.Show( "Chỉ được sửa Nhiệm Vụ do bạn tạo ra!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    }
                }

            }
        }

        private void barCheckItem1_CheckedChanged ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {

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

                    gridViewTasks.RefreshRow( prevHotTrackRow );
                    gridViewTasks.RefreshRow( hotTrackRow );

                    if ( hotTrackRow>=0 )
                        gridControl1.Cursor=Cursors.Hand;
                    else
                        gridControl1.Cursor=Cursors.Default;
                }
            }
        }

        private void gridViewTasks_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo info=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( info.InRowCell )
                HotTrackRow=info.RowHandle;
            else
                HotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void gridViewTasks_RowCellStyle ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e )
        {
            if ( e.RowHandle==HotTrackRow )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Underline );
        }

        #endregion
    }
}
