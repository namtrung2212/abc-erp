using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCBusinessEntities;
using System.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ABCProvider;

namespace ABCApp
{
    public partial class UserOnlineList : DevExpress.XtraEditors.XtraUserControl
    {
        BindingList<ABCUserInfo> lstAllUsers;

        ABCChatScreen ChatScreen;

        public UserOnlineList ( ABCChatScreen form )
        {
            ChatScreen=form;

            InitializeComponent();

            lstAllUsers=new BindingList<ABCUserInfo>( ABCUserProvider.GetAllUsers(true,true) );

            this.gridControl1.DataSource=lstAllUsers;
            this.gridControl1.RefreshDataSource();
            this.gridView1.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridView1_CustomDrawCell );
            this.gridView1.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedRow=false;
            this.gridView1.Click+=new EventHandler( gridView1_Click );
            this.gridView1.MouseMove+=new MouseEventHandler( gridView1_MouseMove );
            this.gridView1.RowCellStyle+=new RowCellStyleEventHandler( gridView1_RowCellStyle );
            this.gridView1.ExpandAllGroups();

            btnShowOnlineOnly.Checked=false;
            ChangeView( false );

            btnSoundOn.Checked=true;
            ChangeSoundOnOff( true );

            Timer timer=new Timer();
            timer.Interval=15000;
            timer.Tick+=new EventHandler( timer_Tick );
            timer.Start();
        }

        void gridView1_Click ( object sender , EventArgs e )
        {
            Point pt=gridControl1.PointToClient( Control.MousePosition );

            GridHitInfo info=gridView1.CalcHitInfo( pt );
            if ( info!=null&&info.Column!=null&&info.InRow||info.InRowCell )
            {
                if ( info.Column.FieldName=="Employee" )
                {
                    ABCUserInfo user=gridView1.GetRow( info.RowHandle ) as ABCUserInfo;
                    if ( user==null )
                        return;

                    if ( ChatScreen!=null )
                    {
                        ChatScreen.OpenChatBox( user.User );
                        ChatScreen.ActiveChatPanel( user.User );
                    }
                }
            }
        }

        void gridView1_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.Column.FieldName=="Employee" )
            {
                ABCUserInfo user=gridView1.GetRow( e.RowHandle ) as ABCUserInfo;
                if ( user==null )
                    return;

                SolidBrush brush=new SolidBrush( e.Appearance.BackColor );
                e.Graphics.FillRectangle( brush , ( e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo ).Bounds );
             
                if ( user.IsOnline )
                {
                    e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "Online" ) , e.Bounds.Location );

                }else
                {
                    e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "Offline" ) , e.Bounds.Location );

                    e.Appearance.ForeColor=Color.Gray;
                    e.Appearance.Options.UseForeColor=true;
                }

              
                Rectangle r=e.Bounds;
                r.Width-=18;
                r.X+=18;
                e.Appearance.DrawString( e.Cache , e.DisplayText , r );

                e.Handled=true;
            }
        }

        void timer_Tick ( object sender , EventArgs e )
        {
            CheckOnline(); 
        }

        void CheckOnline ( )
        {
            IEnumerable<ADUserStatussInfo> lstTemp=new ADUserStatussController().GetListAllObjects().Cast<ADUserStatussInfo>().ToList<ADUserStatussInfo>();
            foreach ( ABCUserInfo user in lstAllUsers )
            {
                if ( user.User!=ABCUserProvider.CurrentUserName )
                {
                    foreach ( ADUserStatussInfo status in lstTemp )
                    {
                        if ( status.FK_ADUserID.HasValue&&status.FK_ADUserID.Value==user.UserID )
                        {
                            user.IsOnline=status.IsOnline;
                            break;
                        }
                    }
                }
            }

            this.gridControl1.DataSource=lstAllUsers;
            this.gridControl1.RefreshDataSource();
        }

        void ChangeView ( bool isShowOnlineOnly )
        {
            if ( isShowOnlineOnly )
            {
                gridView1.ActiveFilterString="[IsOnline] = 1";
            }
            else
            {
                gridView1.ActiveFilterString="";
            }
        }
        void ChangeSoundOnOff ( bool isSoundOn )
        {
            this.ChatScreen.SoundOn=isSoundOn;
            if ( isSoundOn )
                btnSoundOn.Glyph=ABCControls.ABCImageList.GetImage16x16( "SoundOn" );
            else
                btnSoundOn.Glyph=ABCControls.ABCImageList.GetImage16x16( "SoundOff" );
        }

        private void btnShowOnlineOnly_CheckedChanged ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ChangeView( btnShowOnlineOnly.Checked );
        }

        private void btnSoundOn_CheckedChanged ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            ChangeSoundOnOff( btnSoundOn.Checked );
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

                    gridView1.RefreshRow( prevHotTrackRow );
                    gridView1.RefreshRow( hotTrackRow );

                    if ( hotTrackRow>=0 )
                        gridControl1.Cursor=Cursors.Hand;
                    else
                        gridControl1.Cursor=Cursors.Default;
                }
            }
        }

        private void gridView1_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo info=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( info.InRowCell )
                HotTrackRow=info.RowHandle;
            else
                HotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void gridView1_RowCellStyle ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e )
        {
            if ( e.RowHandle==HotTrackRow )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Underline );
        }

        #endregion
    }
}
