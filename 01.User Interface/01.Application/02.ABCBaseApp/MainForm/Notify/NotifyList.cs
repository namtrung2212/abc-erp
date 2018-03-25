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
using System.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace ABCApp
{
    
    public partial class NotifyList : DevExpress.XtraEditors.XtraUserControl
    {
        ABCApp.MainForm mainForm;
        public bool SoundOn=true;

        public NotifyList ( MainForm form )
        {
            mainForm=form;

            InitializeComponent();

            this.gridViewNotifies.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridViewNotifies_CustomDrawCell );
            this.gridViewNotifies.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewNotifies.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridViewNotifies.Click+=new EventHandler( gridViewNotifies_Click );
            this.gridViewNotifies.MouseMove+=new MouseEventHandler( gridViewNotifies_MouseMove );
            this.gridViewNotifies.RowCellStyle+=new RowCellStyleEventHandler( gridViewNotifies_RowCellStyle );
            this.gridViewNotifies.ExpandAllGroups();

            chkShowNewMailOnly.Checked=true;
            ChangeView( false );

            chkSound.Checked=false;
            ChangeSoundOnOff( true );
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
            ReloadNotifies( false );
        } 
        #endregion

        #region Events

        void gridViewNotifies_Click ( object sender , EventArgs e )
        {
            Point pt=gridControl1.PointToClient( Control.MousePosition );

            GridHitInfo info=gridViewNotifies.CalcHitInfo( pt );
            if ( info!=null&&info.InRow||info.InRowCell )
            {
                DataRow dr=gridViewNotifies.GetDataRow( info.RowHandle );
                if ( dr==null )
                    return;
                dr["Viewed"]=1;
                NotifyProvider.SetNotifyToViewed( ABCHelper.DataConverter.ConvertToGuid( dr["GENotifyID"] ) );
                if ( dr["TableName"].ToString()=="GEMeetings" )
                    ABCScreen.ABCScreenManager.Instance.RunLink( "GEMeetings" , ABCCommon.ViewMode.Runtime , false , ABCHelper.DataConverter.ConvertToGuid( dr["ID"] ) , ABCCommon.ABCScreenAction.None );
                else
                    ABCControls.ABCObjectInformation.ShowObjectInfo( dr["TableName"].ToString() , ABCHelper.DataConverter.ConvertToGuid( dr["ID"] ) );
            }

        }

        void gridViewNotifies_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            DataRow dr=gridViewNotifies.GetDataRow( e.RowHandle );
            if ( dr==null )
                return;

            if ( Convert.ToBoolean( dr["Viewed"] ) )
            {
                e.Appearance.ForeColor=Color.Gray;
                e.Appearance.Options.UseForeColor=true;
            }
            else
            {
                e.Appearance.Font=new Font( e.Appearance.Font , e.Appearance.Font.Style|FontStyle.Bold );
            }

            if ( e.Column.FieldName=="NotifyTitle" )
            {
                SolidBrush brush=new SolidBrush( e.Appearance.BackColor );
                e.Graphics.FillRectangle( brush , ( e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo ).Bounds );

                if ( String.IsNullOrWhiteSpace( dr["PriorityLevel"].ToString() )==false )
                    e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( dr["PriorityLevel"].ToString() ) , e.Bounds.Location );

                Rectangle r=e.Bounds;
                r.Width-=18;
                r.X+=18;
                e.Appearance.DrawString( e.Cache , e.DisplayText , r );

                e.Handled=true;
            }
            else if ( e.Column.FieldName=="LastTime" )
            {
                if ( e.CellValue!=null )
                {
                    DateTime lastTime=Convert.ToDateTime( e.CellValue );
                    if ( lastTime.Date==ABCApp.ABCDataGlobal.WorkingDate.Date )
                        e.DisplayText=String.Format( @"{0}:{1}" , lastTime.Hour.ToString( "00" ) , lastTime.Minute.ToString( "00" ) );
                    else
                        e.DisplayText=String.Format( @"{0,2}/{1,-2}" , lastTime.Day , lastTime.Month );
                }
            }
        }

        #region Toolbar
        void ChangeView ( bool isShowUnreadOnly )
        {
            if ( isShowUnreadOnly )
            {
                gridViewNotifies.ActiveFilterString="[Viewed] = 0";
                chkShowNewMailOnly.Glyph=ABCControls.ABCImageList.GetImage16x16( "NewMail" );
            }
            else
            {
                gridViewNotifies.ActiveFilterString="";
                chkShowNewMailOnly.Glyph=ABCControls.ABCImageList.GetImage16x16( "AllMail" );
            }
        }
        private void chkShowUnreadOnly_CheckedChanged ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ChangeView( !chkShowNewMailOnly.Checked );
        }

        void ChangeSoundOnOff ( bool isSoundOn )
        {
            SoundOn=isSoundOn;
            if ( SoundOn )
                chkSound.Glyph=ABCControls.ABCImageList.GetImage16x16( "SoundOn" );
            else
                chkSound.Glyph=ABCControls.ABCImageList.GetImage16x16( "SoundOff" );
        }
        private void chkSoundOnOff_CheckedChanged ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ChangeSoundOnOff( !chkSound.Checked );
        }

        
        #endregion


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

                    gridViewNotifies.RefreshRow( prevHotTrackRow );
                    gridViewNotifies.RefreshRow( hotTrackRow );

                    if ( hotTrackRow>=0 )
                        gridControl1.Cursor=Cursors.Hand;
                    else
                        gridControl1.Cursor=Cursors.Default;
                }
            }
        }

        private void gridViewNotifies_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo info=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( info.InRowCell )
                HotTrackRow=info.RowHandle;
            else
                HotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void gridViewNotifies_RowCellStyle ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e )
        {
            if ( e.RowHandle==HotTrackRow )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Underline );
        }
        
        #endregion

        #endregion

        DataTable NotifiesTable=null;
        DateTime lastUpdate=DateTime.MinValue;
        public void ReloadNotifies ( bool isFirstLoad )
        {
            bool isHasNew=false;
            String strQuery=String.Format( @"SELECT COUNT(*) FROM GENotifys WHERE  ToUser ='{0}' AND Viewed =0 AND {1}" , ABCUserProvider.CurrentUserName , TimeProvider.GenCompareDateTime( "LastTime" , ">" , lastUpdate ) );
            object objQty=BusinessObjectController.GetData( strQuery );
            if ( objQty!=null&&objQty!=DBNull.Value&&Convert.ToInt32( objQty )>0 )
            {
                isHasNew=true;
                if ( this.SoundOn )
                    new System.Media.SoundPlayer( @"SoundMail.wav" ).Play();
            }

            if ( isFirstLoad||NotifiesTable==null||isHasNew )
            {
                DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM GENotifys WHERE ToUser ='{0}' ORDER BY LastTime DESC" , ABCUserProvider.CurrentUserName ) );
                if ( ds!=null&&ds.Tables.Count>0 )
                {
                    if ( NotifiesTable!=null )
                        NotifiesTable.Dispose();
                    NotifiesTable=ds.Tables[0];
                }
            }
         
            if ( NotifiesTable!=null&&NotifiesTable.Rows.Count>0 )
            {
                object obj=NotifiesTable.Rows[0]["LastTime"];
                if ( obj!=null&&obj!=DBNull.Value )
                    lastUpdate=Convert.ToDateTime( obj );
            }

            RefreshDataSource();

            StartTimer();

        }

        public void RefreshDataSource ( )
        {
            this.gridControl1.DataSource=NotifiesTable;
            this.gridControl1.RefreshDataSource();
            this.gridViewNotifies.MoveFirst();
        }
     
    }
}
