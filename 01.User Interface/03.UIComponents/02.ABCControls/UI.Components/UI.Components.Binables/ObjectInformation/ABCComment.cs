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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraRichEdit.RichEditControl ) )]
    [Designer( typeof( ABCCommentDesigner ) )]
    public partial class ABCComment : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }
        ABCTagObject tabPanel;

        public ABCComment ( )
        {
            InitializeComponent();

            this.gridView.Appearance.Empty.BackColor=ABCPresentHelper.GetSkinBackColor();
            this.gridView.Appearance.Empty.Options.UseBackColor=true;
            this.gridView.Appearance.Row.BackColor=ABCPresentHelper.GetSkinBackColor();
            this.gridView.Appearance.Row.Options.UseBackColor=true;

            this.gridView.OptionsView.RowAutoHeight=true;
            this.gridView.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridView.OptionsSelection.EnableAppearanceFocusedRow=false;
            this.gridView.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridContent_CustomDrawCell );
            this.gridView.Click+=new EventHandler( gridView_Click );
            this.gridView.MouseMove+=new MouseEventHandler( gridView_MouseMove );
            this.gridView.RowCellStyle+=new RowCellStyleEventHandler( gridView_RowCellStyle );
            this.gridView.OptionsBehavior.AllowPartialRedrawOnScrolling=true;
            this.colTime.OptionsColumn.AllowEdit=false;
            this.colEmployee.OptionsColumn.AllowEdit=false;
            this.colComment.OptionsColumn.AllowEdit=true;
            this.colComment.OptionsColumn.ReadOnly=true;

            this.colEmployee.AppearanceCell.TextOptions.VAlignment=DevExpress.Utils.VertAlignment.Top;
            this.colTime.AppearanceCell.TextOptions.VAlignment=DevExpress.Utils.VertAlignment.Top;
            this.colComment.AppearanceCell.TextOptions.VAlignment=DevExpress.Utils.VertAlignment.Top;


            tabPanel=new ABCTagObject();
            tabPanel.Dock=DockStyle.Fill;
            tagPanelContainer.Controls.Add( tabPanel );
        }

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "ObjectID";
            }
        }
        #endregion

        [Category( "ABC.BindingValue" )]
        public String TableName { get; set; }

        Guid _ObjectID=Guid.Empty;
        public Guid ObjectID
        {
            get
            {
                return _ObjectID;
            }
            set
            {
                _ObjectID=value;
                LoadComments();
                this.gridView.MoveLast();

            }
        }

        public String Tags { get; set; }
        public String TagsDisplay { get; set; }

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

        bool isVisible=true;
        [Category( "External" )]
        public Boolean IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible=value;
                if ( OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

        #endregion

        #region IABCControl

        public void InitControl ( )
        {
        }
        #endregion

        public void LoadComments ( )
        {
            if ( string.IsNullOrWhiteSpace( this.TableName )||ObjectID==Guid.Empty )
                return;

            LoadComments( this.TableName , ObjectID );
        }
        public void LoadComments ( String strTableName , Guid iID )
        {
            if ( iID==Guid.Empty )
                return;

            _ObjectID=iID;
            this.TableName=strTableName;

            if ( this.gridControl.DataSource!=null&&this.gridControl.DataSource is DataTable )
                ( this.gridControl.DataSource as DataTable ).Dispose();
        
            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM GEComments WHERE TableName ='{0}' AND ID ='{1}' AND ID IS NOT NULL ORDER BY CreateTime" , strTableName , iID ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    if ( String.IsNullOrWhiteSpace( dr["TagStringDisplay"].ToString() )==false )
                        dr["Comment"]=dr["Comment"].ToString()+" ( "+dr["TagStringDisplay"].ToString()+" )";

                }
                this.gridControl.DataSource=ds.Tables[0];
            }
            else
                this.gridControl.DataSource=null;

            this.gridControl.RefreshDataSource();
          
            this.colEmployee.MinWidth=0;
            this.colEmployee.MaxWidth=0;
            this.colEmployee.BestFit();
            this.colEmployee.MinWidth=this.colEmployee.Width;
            this.colEmployee.MaxWidth=this.colEmployee.Width;
            StartAutoLoadTimer();
        }

        public void AddComment ( String strComment )
        {
            if ( String.IsNullOrWhiteSpace( strComment ) )
                return;
            ABCStandardEventArg arg=new ABCStandardEventArg();
            OnAddComment( new CommentObject( this.TableName , ObjectID , strComment , Tags , TagsDisplay ) , arg );
            if ( arg.Cancel )
                return;
            AddComment( this.TableName , ObjectID , strComment );
        }
        public void AddComment ( String strTableName , Guid iID , String strComment )
        {
            if ( String.IsNullOrWhiteSpace( strComment ) )
                return;
            if ( DataStructureProvider.IsExistedTable( strTableName )==false )
                return;
            if ( BusinessControllerFactory.GetBusinessController( strTableName ).GetObjectByID( iID )==null )
                return;

            RefreshTagsString();

            String strCreateUser=ABCUserProvider.CurrentUserName.Replace( "'" , "''" );
            String strCreateEmployee=ABCUserProvider.CurrentEmployeeName.Replace( "'" , "''" );
        
            String strQuery=String.Format( @"INSERT INTO GEComments ( GECommentID,CreateTime , CreateUser , Employee , TableName , ID ,Comment,TagString,TagStringDisplay ) 
                                                              VALUES ('{0}',GetDate() ,N'{1}' ,N'{2}' ,'{3}','{4}',N'{5}',N'{6}',N'{7}')" , Guid.NewGuid() , strCreateUser.Replace( "'" , "''" ) , strCreateEmployee.Replace( "'" , "''" ) , strTableName , iID , strComment.Replace( "'" , "''" ) , Tags.Replace( "'" , "''" ) , TagsDisplay.Replace( "'" , "''" ) );
        
            BusinessObjectController.RunQuery( strQuery );
            NotifyProvider.CreateNewNotifyFromComment( strTableName , iID );
            LoadComments();
            this.gridView.MoveLast();

            this.richEditControl1.Text="";

            this.tabPanel.ClearTags();
        }

        private void RefreshTagsString ( )
        {

            Tags=String.Empty;

            TagsDisplay=String.Empty;
            foreach ( ABCUserInfo user in this.tabPanel.Users.Values )
            {
                if ( String.IsNullOrWhiteSpace( Tags ) )
                    Tags=user.User;
                else
                    Tags=Tags+"::"+user.User;

                if ( String.IsNullOrWhiteSpace( TagsDisplay ) )
                    TagsDisplay=user.Employee;
                else
                    TagsDisplay=TagsDisplay+" ; "+user.Employee;
            }
        }

        #region OnAddComment
        public class CommentObject
        {
            public String TableName=String.Empty;
            public Guid ID;
            public String Comment=String.Empty;
            public String Tags=String.Empty;
            public String TagsDisplay=String.Empty;
            public CommentObject ( String strTableName , Guid iID , String strComment , String strTags , String strTagsDisplay )
            {
                TableName=strTableName;
                ID=iID;
                Comment=strComment;
                Tags=strTags;
                TagsDisplay=strTagsDisplay;
            }
        }
        public delegate void ABCAddCommentEventHandler ( CommentObject comment , ABCStandardEventArg e );
        public event ABCAddCommentEventHandler AddCommentEvent;

        public virtual void OnAddComment ( CommentObject comment , ABCStandardEventArg e )
        {
            if ( this.AddCommentEvent!=null )
                this.AddCommentEvent( comment , e );
        }
        #endregion

        #region Events
        void gridView_Click ( object sender , EventArgs e )
        {
            Point pt=gridControl.PointToClient( Control.MousePosition );

            GridHitInfo info=gridView.CalcHitInfo( pt );
            if ( info!=null&&info.InRowCell&&info.Column.FieldName=="Employee" )
            {
                DataRow dr=gridView.GetDataRow( info.RowHandle );
                if ( dr==null )
                    return;
                if ( ABCApp.ABCAppHelper.Instance!=null )
                    ABCApp.ABCAppHelper.Instance.Chat( dr["CreateUser"].ToString() );

            }

        }

        void gridContent_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.RowHandle<0||e.RowHandle==DevExpress.XtraGrid.GridControl.AutoFilterRowHandle )
                return;

            DataRow dr=gridView.GetDataRow( e.RowHandle );
            if ( dr==null )
                return;

            if ( e.Column.FieldName=="Comment" )
            {
                String strTagDisplay=dr["TagStringDisplay"].ToString();
                if ( String.IsNullOrWhiteSpace( strTagDisplay )==false&&e.DisplayText.EndsWith( "( "+strTagDisplay+" )" )==false )
                {
                    String strOldText=e.DisplayText;

                    e.DisplayText=e.DisplayText+" ( "+strTagDisplay+" )";
                    //      XPaint.Graphics.DrawMultiColorString( e.Cache , e.Bounds , e.DisplayText ,strTagDisplay , e.Appearance , Color.Blue , e.Appearance.BackColor , false , strOldText.Length+3 );
                    //   e.Handled=true;
                }
            }
            else if ( e.Column.FieldName=="Employee" )
            {
                String strCreateUser=dr["CreateUser"].ToString();
                if ( strCreateUser==ABCUserProvider.CurrentUserName )
                    e.Appearance.ForeColor=Color.Gray;
                else
                    e.Appearance.ForeColor=Color.SteelBlue;

                e.Appearance.Options.UseForeColor=true;

            }

        }

        private void btnSend_Click ( object sender , EventArgs e )
        {
            AddComment( this.richEditControl1.RtfText);
        }

        //private void gridView1_CalcRowHeight ( object sender , DevExpress.XtraGrid.Views.Grid.RowHeightEventArgs e )
        //{
        //    GridViewInfo vi=gridView1.GetViewInfo() as GridViewInfo;

        //    Text=vi.ViewRects.Rows.ToString();
        //    if ( e.RowHeight>vi.ViewRects.Rows.Height-10 )
        //        e.RowHeight=vi.ViewRects.Rows.Height-10;
        //} 
        #endregion

        System.Windows.Forms.Timer AutoLoadTimer;
        public void StartAutoLoadTimer ( )
        {
            if ( AutoLoadTimer==null )
            {
                AutoLoadTimer=new Timer();
                AutoLoadTimer.Interval=5000;
                AutoLoadTimer.Tick+=new EventHandler( AutoLoadTimer_Tick );
                AutoLoadTimer.Start();
            }
        }

        void AutoLoadTimer_Tick ( object sender , EventArgs e )
        {
            LoadComments();
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

                    gridView.RefreshRow( prevHotTrackRow );
                    gridView.RefreshRow( hotTrackRow );

                    if ( hotTrackRow>=0 )
                        gridControl.Cursor=Cursors.Hand;
                    else
                        gridControl.Cursor=Cursors.Default;
                }
            }
        }

        private void gridView_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            GridView view=sender as GridView;
            GridHitInfo info=view.CalcHitInfo( new Point( e.X , e.Y ) );
            if ( info.InRowCell&&info.Column!=null && info.Column.FieldName=="Employee" )
                HotTrackRow=info.RowHandle;
            else
                HotTrackRow=DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void gridView_RowCellStyle ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e )
        {
            if ( e.RowHandle==HotTrackRow&&e.Column.FieldName=="Employee" )
                e.Appearance.Font=new Font( e.Appearance.Font ,e.Appearance.Font.Style| FontStyle.Underline );
        }

        #endregion
    }

    public class ABCCommentDesigner : ControlDesigner
    {
        public ABCCommentDesigner ( )
        {
        }
    }
}
