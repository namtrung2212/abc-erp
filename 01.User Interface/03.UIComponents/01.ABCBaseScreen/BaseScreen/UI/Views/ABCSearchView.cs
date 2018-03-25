using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using ABCControls;
using ABCHelper;
using ABCProvider;
using ABCProvider;

using ABCBusinessEntities;
using ABCScreen.Data;
namespace ABCScreen.UI
{
    public class ABCSearchView : DevExpress.XtraEditors.XtraForm
    {
        public ABCDataObject BindingObject;

        ABCAutoSearchPanel SearchPanel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit chkLocked;
        private DevExpress.XtraEditors.CheckEdit chkNotYetPosted;
        private DevExpress.XtraEditors.CheckEdit chkNotYetApproved;
        ABCGridControl GridCtrl;
        BindingSource binding;

        public ABCSearchView ( ABCDataObject bindObject )
        {
            BindingObject=bindObject;
            InitializeComponent();
            InitializeControls();
          //  this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;

        }


        public void InitializeControls ( )
        {
            //SearchPanel=new ABCAutoSearchPanel();
            //SearchPanel.PopulateControls( BindingObject.TableName );
            //SearchPanel.Dock=System.Windows.Forms.DockStyle.Top;
            //this.Width=SearchPanel.Width+30;

            //SearchPanel.Parent=this;
            //SearchPanel.Search+=new ABCSearchPanel.ABCSearchEventHandler( SearchPanel_Search );

            GridCtrl=new ABCGridControl();
            GridCtrl.Initialize( BindingObject.TableName );

            binding=new BindingSource();
            GridCtrl.GridDataSource=binding;
            GridCtrl.RefreshDataSource();

            if ( DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colEditCount ) )
            {
                if ( GridCtrl.GridDefaultView.Columns.ColumnByFieldName( ABCCommon.ABCConstString.colEditCount )==null )
                {
                    ABCGridColumn col=new ABCGridColumn();
                    col.FieldName=ABCCommon.ABCConstString.colEditCount;
                    col.Caption="Hiệu chỉnh";
                    col.TableName=BindingObject.TableName;
                    col.VisibleIndex=GridCtrl.GridDefaultView.Columns.Count-1;
                    col.Visible=true;
                    col.Width=20;
                    col.OptionsColumn.AllowEdit=false;
                    col.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
                    col.OptionsFilter.AllowAutoFilter=true;
                    GridCtrl.GridDefaultView.Columns.Insert(GridCtrl.GridDefaultView.Columns.Count-1, col );
                }
                //if ( GridCtrl.GridDefaultView.Columns.ColumnByFieldName( ABCCommon.ABCConstString.colSelected )==null )
                //{
                //    ABCGridColumn col=new ABCGridColumn();
                //    col.FieldName=ABCCommon.ABCConstString.colSelected;
                //    col.Caption="Chọn";
                //    col.TableName=BindingObject.TableName;
                //    col.VisibleIndex=0;
                //    col.Visible=true;
                //    col.Width=20;
                //    col.OptionsColumn.AllowEdit=true;
                //    col.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
                //    col.OptionsFilter.AllowAutoFilter=true;
                //    col.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                //    GridCtrl.GridDefaultView.Columns.Add( col );
                //}
            }


            GridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            GridCtrl.Parent=this;
            GridCtrl.ShowSaveButton=false;
            GridCtrl.ShowDeleteButton=false;
            GridCtrl.ShowRefreshButton=false;
            GridCtrl.EnableFocusedCell=false;
            GridCtrl.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            GridCtrl.GridDefaultView.DoubleClick+=new EventHandler( GridDefaultView_DoubleClick );
            GridCtrl.GridDefaultView.MouseUp+=new System.Windows.Forms.MouseEventHandler( GridDefaultView_MouseUp );
            GridCtrl.BringToFront();

            this.Height=400;
            this.Width=700;
            if ( BindingObject.DataManager.Screen.ViewForm!=null )
            {
                this.Location=new Point( BindingObject.DataManager.Screen.ViewForm.Left-this.Width , BindingObject.DataManager.Screen.ViewForm.Top );
                this.Height=BindingObject.DataManager.Screen.ViewForm.Height;
                BindingObject.DataManager.Screen.ViewForm.LocationChanged+=new EventHandler( ABCSearchView_LocationChanged );
                BindingObject.DataManager.Screen.ViewForm.SizeChanged+=new EventHandler( ABCSearchView_SizeChanged );
                this.Width=300;
            }
            this.Shown+=new EventHandler( ABCSearchView_Shown );
            this.FormClosing+=new System.Windows.Forms.FormClosingEventHandler( ABCSearchView_FormClosing );

            this.ShowInTaskbar=false;

            this.Text="Danh sách "+DataConfigProvider.GetTableCaption( BindingObject.TableName );

            chkNotYetApproved.Visible=DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colApprovalStatus );
            chkLocked.Visible=DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colLockStatus );
            chkNotYetPosted.Visible=DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colJournalStatus );
            chkNotYetApproved.Checked=false;
            chkLocked.Checked=false;
            chkNotYetPosted.Checked=false;
            chkNotYetApproved.CheckedChanged+=new EventHandler( chk_CheckedChanged );
            chkLocked.CheckedChanged+=new EventHandler( chk_CheckedChanged );
            chkNotYetPosted.CheckedChanged+=new EventHandler( chk_CheckedChanged );
            this.StartPosition=FormStartPosition.CenterParent;

            //   SearchPanel_Search( null );
        }

        void chk_CheckedChanged ( object sender , EventArgs e )
        {
            Search();
        }

        void ABCSearchView_Shown ( object sender , EventArgs e )
        {
            Form form=BindingObject.DataManager.Screen.ViewForm as Form;
            if ( form!=null&&form.WindowState==FormWindowState.Normal )
            {
                if ( ABCStudio.ABCStudioHelper.Instance!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
                    this.Location=new Point( form.Left-this.Width , form.Top );
                else
                    this.Location=new Point( form.Left-this.Width , form.Top-22 );
                this.Height=form.Height;
                form.TopMost=true;
            }
            this.TopMost=true;
            if ( this.TopLevel==false )
                this.BringToFront();
        }

        void ABCSearchView_SizeChanged ( object sender , EventArgs e )
        {
            Form form=sender as Form;
            if ( form!=null&&form.WindowState==FormWindowState.Normal )
            {
                if ( ABCStudio.ABCStudioHelper.Instance!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
                    this.Location=new Point( form.Left-this.Width , form.Top );
                else
                    this.Location=new Point( form.Left-this.Width , form.Top-22 );
                this.Height=form.Height;
                form.TopMost=true;
            }
            this.TopMost=true;
            if ( this.TopLevel==false )
                this.BringToFront();
        }

        void ABCSearchView_LocationChanged ( object sender , EventArgs e )
        {
            Form form=sender as Form;
            if ( form!=null&&form.WindowState==FormWindowState.Normal )
            {
                if ( ABCStudio.ABCStudioHelper.Instance!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
                    this.Location=new Point( form.Left-this.Width , form.Top );
                else
                    this.Location=new Point( form.Left-this.Width , form.Top-22 );

                this.Height=form.Height;
                form.TopMost=true;
            }
            this.TopMost=true;
            if ( this.TopLevel==false )
                this.BringToFront();
        }

        void GridDefaultView_MouseUp ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
          
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitinfo= GridCtrl.GridDefaultView.CalcHitInfo( e.Location );
            if(hitinfo!=null && hitinfo.InRow)
                InvalidateMainObject();
        }

        void ABCSearchView_FormClosing ( object sender , System.Windows.Forms.FormClosingEventArgs e )
        {
            e.Cancel=true;
            this.Hide();

            GC.Collect();

            //if ( ABCStudio.ABCStudioHelper.Instance!=null&&
            //    ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&
            //    ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
            //{
            //    ABCStudio.ABCStudioHelper.Instance.MainStudio.Activate();
            //    return;
            //}
            //if ( ABCApp.ABCAppProvider.AppInstance!=null&&
            //   ABCApp.ABCAppProvider.AppInstance.MainForm!=null&&
            //   ABCApp.ABCAppProvider.AppInstance.MainForm.Visible )
            //{
            //    ABCApp.ABCAppProvider.AppInstance.MainForm.Activate();
            //    return;
            //}
        }
 
        void GridDefaultView_DoubleClick ( object sender , EventArgs e )
        {
         //   InvalidateMainObject();

            this.Hide();
        }
        void SearchPanel_Search ( object sender )
        {
            Search();
        }

        private void InvalidateMainObject ( )
        {
            BusinessObjectController controller=BusinessControllerFactory.GetBusinessController( BindingObject.TableName );
            BusinessObject obj=GridCtrl.GridDefaultView.GetRow( GridCtrl.GridDefaultView.FocusedRowHandle ) as BusinessObject;
            if ( obj!=null )
                BindingObject.DataManager.Invalidate( BindingObject.Config.Name , obj.GetID() );
        }
        public void Search ( )
        {
            if ( BindingObject!=null&&GridCtrl!=null )
            {
                if ( BindingObject.DataManager.MainObject==null )
                    return;

                BusinessObject mainObj=BindingObject.DataManager.MainObject.DataObject as BusinessObject;
                BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( mainObj.AATableName );

                ConditionBuilder strBuilder=BindingObject.GenerateQuery( true );

                if ( strBuilder==null||String.IsNullOrWhiteSpace( strBuilder.ToString() ) )
                    return;

                if ( SearchPanel!=null )
                    SearchPanel.GetSearchQuery( strBuilder , SearchPanel );

                if ( BindingObject.DataManager.Screen.UIManager.View.DataField!=null )
                {
                    GEVouchersInfo config=VoucherProvider.GetConfig( BindingObject.TableName , BindingObject.DataManager.Screen.UIManager.View.DataField.STViewNo );
                    if ( config!=null&&!String.IsNullOrWhiteSpace( config.ConditionString ) )
                        strBuilder.AddCondition( config.ConditionString );
                }

                if ( DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                    if ( chkNotYetApproved.Checked )
                        strBuilder.AddCondition( String.Format( "{0}<>'{1}'" , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeApproved ) );

                if ( DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colLockStatus ) )
                    if ( chkLocked.Checked )
                        strBuilder.AddCondition( String.Format( "{0}='{1}'" , ABCCommon.ABCConstString.colLockStatus , ABCCommon.ABCConstString.LockStatusLocked ) );

                if ( DataStructureProvider.IsTableColumn( BindingObject.TableName , ABCCommon.ABCConstString.colJournalStatus ) )
                    if ( chkNotYetPosted.Checked )
                        strBuilder.AddCondition( String.Format( "{0}<>'{1}'" , ABCCommon.ABCConstString.colJournalStatus , ABCCommon.ABCConstString.PostStatusPosted ) );

                object datasource=null;
                Type objType=BusinessObjectFactory.GetBusinessObjectType( GridCtrl.TableName );
                if ( objType!=null )
                {
                    Type typeABCList=typeof( ABCList<> ).MakeGenericType( objType );
                    MethodInfo method;
                    if ( typeABCList!=null )
                    {
                        datasource=ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeABCList );
                        method=typeABCList.GetMethod( "LoadData" , new Type[] { typeof( List<BusinessObject> ) } );
                        method.Invoke( datasource , new object[] { ctrl.GetListByQuery( strBuilder.ToString() ) } );
                    }
                }
                binding.DataSource=datasource;
                binding.ResetBindings( false );

                this.GridCtrl.GridDefaultView.BestFitColumns();

                for ( int i=0; i<this.GridCtrl.GridDefaultView.DataRowCount; i++ )
                {
                    BusinessObject obj=this.GridCtrl.GridDefaultView.GetRow( i ) as BusinessObject;
                    if ( obj!=null&&obj.GetID()==mainObj.GetID() )
                    {
                        this.GridCtrl.GridDefaultView.FocusedRowHandle=i;
                        return;
                    }
                }
            }
        }

        private void InitializeComponent ( )
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.chkLocked = new DevExpress.XtraEditors.CheckEdit();
            this.chkNotYetPosted = new DevExpress.XtraEditors.CheckEdit();
            this.chkNotYetApproved = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkLocked.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNotYetPosted.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNotYetApproved.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.chkLocked);
            this.panelControl1.Controls.Add(this.chkNotYetPosted);
            this.panelControl1.Controls.Add(this.chkNotYetApproved);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 241);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(317, 28);
            this.panelControl1.TabIndex = 0;
            // 
            // chkLocked
            // 
            this.chkLocked.Location = new System.Drawing.Point(118, 5);
            this.chkLocked.Name = "chkLocked";
            this.chkLocked.Properties.Caption = "Bị khóa";
            this.chkLocked.Size = new System.Drawing.Size(75, 19);
            this.chkLocked.TabIndex = 0;
            this.chkLocked.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // chkNotYetPosted
            // 
            this.chkNotYetPosted.Location = new System.Drawing.Point(206, 5);
            this.chkNotYetPosted.Name = "chkNotYetPosted";
            this.chkNotYetPosted.Properties.Caption = "Chưa ghi sổ";
            this.chkNotYetPosted.Size = new System.Drawing.Size(104, 19);
            this.chkNotYetPosted.TabIndex = 4;
            // 
            // chkNotYetApproved
            // 
            this.chkNotYetApproved.Location = new System.Drawing.Point(5, 5);
            this.chkNotYetApproved.Name = "chkNotYetApproved";
            this.chkNotYetApproved.Properties.Caption = "Chưa phê duyệt";
            this.chkNotYetApproved.Size = new System.Drawing.Size(109, 19);
            this.chkNotYetApproved.TabIndex = 2;
            // 
            // ABCSearchView
            // 
            this.ClientSize = new System.Drawing.Size(317, 269);
            this.Controls.Add(this.panelControl1);
            this.MinimizeBox = false;
            this.Name = "ABCSearchView";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkLocked.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNotYetPosted.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNotYetApproved.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private void checkEdit1_CheckedChanged ( object sender , EventArgs e )
        {

        }
      
    }

}
