using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.Utils.Paint;

using ABCProvider;
using ABCProvider;
using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;
using ABCControls;
namespace ABCApp
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraRichEdit.RichEditControl ) )]
    [Designer( typeof( ABCChatAreaDesigner ) )]
    public partial class ABCChatArea : DevExpress.XtraEditors.XtraUserControl
    {
        ABCChatBox ChatBox;
        public bool IsLoadAllMessages=false;
        public bool IsViewed=true;

        ADUsersController userCtrl=new ADUsersController();
        HREmployeesController employeeCtrl=new HREmployeesController();
        public String User1=String.Empty;
        public String User2=String.Empty;
        public String EmployeeName1=String.Empty;
        public String EmployeeName2=String.Empty;

        public ABCChatArea ( ABCChatBox form , String strUser2 )
        {
            ChatBox=form;
            InitializeComponent();

            this.GotFocus+=new EventHandler( ABCChatArea_GotFocus );
            
            this.gridContentView.OptionsView.RowAutoHeight=true;
            this.gridContentView.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridContentView.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridContentView.OptionsSelection.EnableAppearanceFocusedRow=false;
            this.gridContentView.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridContentView_CustomDrawCell );
            this.gridContentView.GotFocus+=new EventHandler( gridContentView_GotFocus );

            this.ChatAreaContent.Properties.LinesCount=0;
            this.ChatAreaContent.Properties.NullValuePrompt="nhập nội dung ....";
            this.ChatAreaContent.PreviewKeyDown+=new PreviewKeyDownEventHandler( memoEdit1_PreviewKeyDown );
            this.ChatAreaContent.KeyUp+=new KeyEventHandler( memoEdit1_KeyUp );
            this.ChatAreaContent.GotFocus+=new EventHandler( ChatAreaContent_GotFocus );
            this.colEmployee.OptionsColumn.AllowEdit=false;
            this.colChatContent.OptionsColumn.AllowEdit=true;
            this.colChatContent.OptionsColumn.ReadOnly=true;
            
            this.colEmployee.AppearanceCell.TextOptions.VAlignment=DevExpress.Utils.VertAlignment.Top;
            this.colChatContent.AppearanceCell.TextOptions.VAlignment=DevExpress.Utils.VertAlignment.Top;

            Initialize( ABCUserProvider.CurrentUserName , strUser2 );

            this.txtUser.Text=EmployeeName2;

            InitDrag();
        }

        public void Initialize ( String strUser1 , String strUser2 )
        {
            User1=strUser1;
            User2=strUser2;
            EmployeeName1=strUser1;
            EmployeeName2=strUser2;

            ADUsersInfo user1Info=userCtrl.GetObjectByNo( User1 ) as ADUsersInfo;
            ADUsersInfo user2Info=userCtrl.GetObjectByNo( User2 ) as ADUsersInfo;
            if ( user1Info!=null&&user1Info.FK_HREmployeeID.HasValue )
            {
                HREmployeesInfo emp1Info=employeeCtrl.GetObjectByID( user1Info.FK_HREmployeeID.Value ) as HREmployeesInfo;
                if ( emp1Info!=null )
                    EmployeeName1=emp1Info.Name;
            }
            if ( user2Info!=null&&user2Info.FK_HREmployeeID.HasValue )
            {
                HREmployeesInfo emp2Info=employeeCtrl.GetObjectByID( user2Info.FK_HREmployeeID.Value ) as HREmployeesInfo;
                if ( emp2Info!=null )
                    EmployeeName2=emp2Info.Name;
            }
            CheckPlaySound();
            LoadChatContents(true);

        }

        #region LoadChatContents
        DataTable ContentTable=null;
        DateTime lastUpdate=DateTime.MinValue;
        public void LoadChatContents ( bool isFirstLoad )
        {
            if ( isFirstLoad||ContentTable==null )
            {
                if ( IsLoadAllMessages )
                {
                    DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM GEChatContents WHERE (FromUser =N'{0}' AND ToUser =N'{1}') OR  (ToUser =N'{0}' AND FromUser =N'{1}') ORDER BY CreateTime" , User1 , User2 ) );
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        if ( ContentTable!=null )
                            ContentTable.Dispose();
                        ContentTable=ds.Tables[0];
                    }
                }
                else
                {
                    String strQuery=String.Format( @"SELECT * FROM (SELECT TOP 50 * FROM GEChatContents WHERE (FromUser =N'{0}' AND ToUser =N'{1}') OR  (ToUser =N'{0}' AND FromUser =N'{1}') ORDER BY CreateTime DESC ) as A ORDER BY A.CreateTime" , User1 , User2 );
                    if ( DataQueryProvider.IsCompanySQLConnection ==false)
                        strQuery=String.Format( @"SELECT * FROM (SELECT * FROM GEChatContents WHERE (FromUser =N'{0}' AND ToUser =N'{1}') OR  (ToUser =N'{0}' AND FromUser =N'{1}') ORDER BY CreateTime DESC LIMIT 50 ) as A ORDER BY A.CreateTime" , User1 , User2 );

                    DataSet ds=BusinessObjectController.RunQuery( strQuery );
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        if ( ContentTable!=null )
                            ContentTable.Dispose();
                        ContentTable=ds.Tables[0];
                    }
                }
            }
            else
            {
                String strQuery=String.Format( @"SELECT * FROM (SELECT TOP 50 * FROM GEChatContents WHERE ((FromUser =N'{0}' AND ToUser =N'{1}') OR  (ToUser =N'{0}' AND FromUser =N'{1}'))  AND {2} ORDER BY CreateTime DESC ) as A ORDER BY A.CreateTime" , User1 , User2 , TimeProvider.GenCompareDateTime( "CreateTime" , ">" , lastUpdate ) );
                if ( DataQueryProvider.IsCompanySQLConnection==false )
                    strQuery=String.Format( @"SELECT * FROM (SELECT * FROM GEChatContents WHERE ((FromUser =N'{0}' AND ToUser =N'{1}') OR  (ToUser =N'{0}' AND FromUser =N'{1}'))  AND {2} ORDER BY CreateTime DESC LIMIT 50 ) as A ORDER BY A.CreateTime" , User1 , User2 , TimeProvider.GenCompareDateTime( "CreateTime" , ">" , lastUpdate ) );
                DataSet ds=BusinessObjectController.RunQuery( strQuery );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        ContentTable.ImportRow( dr );
                }
            }

            if ( ContentTable!=null&&ContentTable.Rows.Count>0 )
            {
                object obj=ContentTable.Rows[ContentTable.Rows.Count-1]["CreateTime"];
                if ( obj!=null&&obj!=DBNull.Value )
                    lastUpdate=Convert.ToDateTime( obj );
            }

            RefreshDataSource();

            this.colEmployee.MinWidth=0;
            this.colEmployee.BestFit();
            this.colEmployee.MinWidth=this.colEmployee.Width;
            this.colEmployee.MaxWidth=this.colEmployee.Width;
            StartTimer();

            if ( ABCUserProvider.IsOnline( User2 ) )
                picOnOff.Image=ABCControls.ABCImageList.GetImage16x16( "Online" );
            else
                picOnOff.Image=ABCControls.ABCImageList.GetImage16x16( "Offline" );
        }

        public void RefreshDataSource ( )
        {
            this.gridContent.DataSource=ContentTable;
            this.gridContent.RefreshDataSource();
            MoveLast();
        }
        public void MoveLast ( )
        {
            this.gridContentView.MoveLast();
            gridContentView.TopRowIndex=gridContentView.FocusedRowHandle;
        }
        private void CheckPlaySound ( )
        {
            String strQuery=String.Format( @"SELECT COUNT(*) FROM GEChatContents WHERE  (ToUser =N'{0}' AND FromUser =N'{1}') AND Viewed =0 AND {2}"
                      , User1 , User2 , TimeProvider.GenCompareDateTime( "CreateTime" , ">" , lastUpdate ) );
            object objQty=BusinessObjectController.GetData( strQuery );
            if ( objQty!=null&&objQty!=DBNull.Value&&Convert.ToInt32( objQty )>0 )
            {
                IsViewed=false;
                if ( ( this.ChatBox.ChatScreen.Visible==false||this.ChatBox.ChatScreen.WindowState==FormWindowState.Minimized )&&this.ChatBox.ChatScreen.SoundOn )
                    new System.Media.SoundPlayer( @"SoundChat.wav" ).Play();
            }
        }

      
        #endregion
      
        public void AddChatContent ( String strChatContent )
        {
            if ( String.IsNullOrWhiteSpace( strChatContent ) )
                return;

            GEChatContentsInfo content=new GEChatContentsInfo();
            content.FromUser=User1;
            content.FromEmployee=EmployeeName1;
            content.ToUser=User2;
            content.ToEmployee=EmployeeName2;
            content.ChatContent=strChatContent;
            content.Viewed=false;
            new GEChatContentsController().CreateObject( content );
          
            SetViewAll();
          
            LoadChatContents(false);

            this.ChatAreaContent.Text="";
            this.panelChatContent.Height=28;
        }

        #region  Timer
        System.Windows.Forms.Timer AutoLoadTimer;
        System.Windows.Forms.Timer SplashTimer;
        public void StartTimer ( )
        {
            if ( AutoLoadTimer==null )
            {
                AutoLoadTimer=new Timer();
                AutoLoadTimer.Interval=5000;
                AutoLoadTimer.Tick+=new EventHandler( AutoLoadTimer_Tick );
                AutoLoadTimer.Start();
            }
            if ( SplashTimer==null )
            {
                SplashTimer=new Timer();
                SplashTimer.Interval=700;
                SplashTimer.Tick+=new EventHandler( SplashTimer_Tick );
                SplashTimer.Start();
            }

        }
        public void StopTimer ( )
        {
            if ( AutoLoadTimer==null )
                AutoLoadTimer.Stop();
            if ( SplashTimer==null )
                SplashTimer.Stop();
        }

        void AutoLoadTimer_Tick ( object sender , EventArgs e )
        {
            CheckPlaySound();
            LoadChatContents( false );
        }
        void SplashTimer_Tick ( object sender , EventArgs e )
        {
            if ( this.ChatBox.Parent!=null&&this.ChatBox.Parent is DevExpress.XtraTab.XtraTabPage )
            {
                ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.Options.UseImage=!IsViewed;
                ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.BackColor=Color.SteelBlue;

                if ( IsViewed==false )
                {
                    if (  ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).TabControl.SelectedTabPage== ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage )
                        ||( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.BackColor!=Color.SteelBlue )
                        ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.BackColor=Color.SteelBlue;
                    else
                        ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.BackColor=Color.Orange;

                    ( this.ChatBox.Parent as DevExpress.XtraTab.XtraTabPage ).Appearance.Header.Image=ABCControls.ABCImageList.GetImage16x16( "Chating" );
                }
            }
        }
        #endregion
        

        #region SetViewAll

        public void SetViewAll ( )
        {
            BusinessObjectController.RunQuery( String.Format( @"UPDATE GEChatContents SET Viewed =1 WHERE (ToUser ='{0}' AND FromUser ='{1}') " , User1 , User2 ) );
            IsViewed=true;
        }
        void ABCChatArea_GotFocus ( object sender , EventArgs e )
        {
            SetViewAll();
        }

        void ChatAreaContent_GotFocus ( object sender , EventArgs e )
        {
            SetViewAll();
        }

        void gridContentView_GotFocus ( object sender , EventArgs e )
        {
            SetViewAll();
        }

     
    
        #endregion
   
        #region Events

        private void btnSend_Click ( object sender , EventArgs e )
        {
            AddChatContent( this.ChatAreaContent.Text );
        }

        void memoEdit1_KeyUp ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Enter&&e.Control==false )
            {
                e.SuppressKeyPress=true;
                ChatAreaContent.Text="";
            }
        }
        void memoEdit1_PreviewKeyDown ( object sender , PreviewKeyDownEventArgs e )
        {
            if ( e.KeyCode==Keys.Enter&&e.Control==false )
            {
                AddChatContent( ChatAreaContent.Text );
                e.IsInputKey=false;
            }
        }

        void gridContentView_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.Column.FieldName=="FromEmployee" )
            {
                if ( e.DisplayText==this.EmployeeName1 )
                    e.Appearance.ForeColor=Color.Gray;
                else
                    e.Appearance.ForeColor=Color.SteelBlue;
                e.Appearance.Options.UseForeColor=true;
            }
        }

        private void btnClose_Click ( object sender , EventArgs e )
        {
            if ( this.ChatBox!=null&&this.ChatBox.ChatScreen!=null)
                this.ChatBox.ChatScreen.CloseChatBox( this.User2 );
        }

        #endregion

        #region Drag Drop

        //Check radius for begin drag n drop
        public bool AllowDrag { get; set; }
        private bool _isDragging=false;
        private int _DDradius=40;
        private int startX=0;
        private int startY=0;

        void InitDrag ( )
        {
            AllowDrag=true;
            this.titleArea.MouseDown+=new MouseEventHandler( titleArea_MouseDown );
            this.titleArea.MouseMove+=new MouseEventHandler( titleArea_MouseMove );
            this.titleArea.MouseUp+=new MouseEventHandler( titleArea_MouseUp );
        }

        void titleArea_MouseDown ( object sender , MouseEventArgs e )
        {
            this.Focus();
            if ( e.Y<18 )
            {
                startX=e.X;
                startY=e.Y;
                this._isDragging=false;
            }
        }

        void titleArea_MouseMove ( object sender , MouseEventArgs e )
        {
            if ( !_isDragging )
            {
                // This is a check to see if the mouse is moving while pressed.
                // Without this, the DragDrop is fired directly when the control is clicked, now you have to drag a few pixels first.
                if ( e.Button==MouseButtons.Left&&_DDradius>0&&this.AllowDrag )
                {
                    int num1=startX-e.X;
                    int num2=startY-e.Y;
                    if ( ( ( num1*num1 )+( num2*num2 ) )>_DDradius )
                    {
                        DoDragDrop( this.Parent , DragDropEffects.All );
                        _isDragging=true;
                        return;
                    }
                }
            }
        }

        void titleArea_MouseUp ( object sender , MouseEventArgs e )
        {
            _isDragging=false;
        }

        private void gridContent_Click ( object sender , EventArgs e )
        {

        }
        
        #endregion
        
        #region Show All Messages

        private void lnkAllMessages_LinkClicked ( object sender , LinkLabelLinkClickedEventArgs e )
        {
            IsLoadAllMessages=!IsLoadAllMessages;
            LoadChatContents( true );
            if ( IsLoadAllMessages )
                lnkAllMessages.Text="Tin nhắn gần đây";
            else
                lnkAllMessages.Text="Tất cả tin nhắn";
        }
        
        #endregion

        #region Minimize
        int iNormalHeight=177;
        bool isMinimize=true;
        private void titleArea_DoubleClick ( object sender , EventArgs e )
        {
            if ( this.Parent.Size.Height==20 )
                isMinimize=true;
            else
                isMinimize=false;

            isMinimize=!isMinimize;
            if ( isMinimize )
            {
                iNormalHeight=this.Parent.Size.Height;
                this.Parent.Size=new Size( this.Parent.Size.Width , 20 );
            }
            else
            {
                this.Parent.Size=new Size( this.Parent.Size.Width , iNormalHeight );
            }
        }
        #endregion
    }

    public class ABCChatAreaDesigner : ControlDesigner
    {
        public ABCChatAreaDesigner ( )
        {
        }
    }
}
