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
    public class ABCRelationChooser : DevExpress.XtraEditors.XtraForm
    {
        public GERelationConfigsInfo RelationConfig;
        public List<BusinessObject> SelectedObjects;
        public BusinessObject DestinyResult;

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ABCControls.ABCSimpleButton btnCancel;
        private ABCControls.ABCSimpleButton btnSelect;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage mainTab;
        private DevExpress.XtraTab.XtraTabPage itemTab;
        ABCGridControl MainGridCtrl;
        ABCGridControl ItemGridCtrl;
        BindingSource bindingMain;
        BindingSource bindingItem;
        public ABCRelationChooser (Guid configID )
        {
            RelationConfig=new GERelationConfigsController().GetObjectByID( configID ) as GERelationConfigsInfo;
            if ( RelationConfig==null )
                return;
         
            SelectedObjects=new List<BusinessObject>();

            InitializeComponent();
            InitializeControls();
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;

            ReloadDatas();
        }
        
        #region UI

        public void InitializeControls ( )
        {
            if ( RelationConfig==null )
                return;

            if ( RelationConfig.IsFromSource )
            {
                #region MainObject
                MainGridCtrl=new ABCGridControl();
                MainGridCtrl.Initialize( RelationConfig.SourceTableName );
                bindingMain=new BindingSource();
                MainGridCtrl.GridDataSource=bindingMain;
                MainGridCtrl.RefreshDataSource();

                ABCGridColumn col=new ABCGridColumn();
                col.FieldName=ABCCommon.ABCConstString.colSelected;
                col.Caption="Chọn";
                col.TableName=RelationConfig.SourceTableName;
                col.VisibleIndex=0;
                col.Visible=true;
                col.Width=20;
                col.OptionsColumn.AllowEdit=true;
                col.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
                col.OptionsFilter.AllowAutoFilter=true;
                col.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                MainGridCtrl.GridDefaultView.Columns.Insert(0, col );

                MainGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
                MainGridCtrl.Parent=mainTab;
                MainGridCtrl.ShowSaveButton=false;
                MainGridCtrl.ShowDeleteButton=false;
                MainGridCtrl.ShowRefreshButton=false;
                MainGridCtrl.EnableFocusedCell=false;
                MainGridCtrl.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                MainGridCtrl.BringToFront();

                mainTab.Text=" Chọn "+DataConfigProvider.GetTableCaption( RelationConfig.SourceTableName );

                #endregion
            }
            if ( RelationConfig.IsFromSourceItem )
            {
                #region ItemObject
                ItemGridCtrl=new ABCGridControl();
                ItemGridCtrl.Initialize( RelationConfig.SourceItemTableName );
                bindingItem=new BindingSource();
                ItemGridCtrl.GridDataSource=bindingItem;
                ItemGridCtrl.RefreshDataSource();

                ABCGridColumn col=new ABCGridColumn();
                col.FieldName=ABCCommon.ABCConstString.colSelected;
                col.Caption="Chọn";
                col.TableName=RelationConfig.SourceItemTableName;
                col.VisibleIndex=0;
                col.Visible=true;
                col.Width=20;
                col.OptionsColumn.AllowEdit=true;
                col.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
                col.OptionsFilter.AllowAutoFilter=true;
                col.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                ItemGridCtrl.GridDefaultView.Columns.Insert( 0 , col );

                ItemGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
                ItemGridCtrl.Parent=itemTab;
                ItemGridCtrl.ShowSaveButton=false;
                ItemGridCtrl.ShowDeleteButton=false;
                ItemGridCtrl.ShowRefreshButton=false;
                ItemGridCtrl.EnableFocusedCell=false;
                ItemGridCtrl.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                ItemGridCtrl.BringToFront();

                itemTab.Text=" Chọn "+DataConfigProvider.GetTableCaption( RelationConfig.SourceItemTableName );

           
                #endregion
            }

            this.Shown+=new EventHandler( ABCRelationChooser_Shown );
            this.ShowInTaskbar=false;
            this.StartPosition=FormStartPosition.CenterParent;

            this.Text=String.Format( "Tạo {0} từ {1}" , DataConfigProvider.GetTableCaption( RelationConfig.DestinyTableName ) , DataConfigProvider.GetTableCaption( RelationConfig.SourceTableName ) );
        }

        void ABCRelationChooser_Shown ( object sender , EventArgs e )
        {
          
        }

        private void InitializeComponent ( )
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ABCRelationChooser));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new ABCControls.ABCSimpleButton();
            this.btnSelect = new ABCControls.ABCSimpleButton();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.mainTab = new DevExpress.XtraTab.XtraTabPage();
            this.itemTab = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnSelect);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 351);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(699, 37);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ButtonType = ABCControls.ABCSimpleButton.ABCButtonType.None;
            this.btnCancel.Comment = null;
            this.btnCancel.DataSource = null;
            this.btnCancel.FieldGroup = null;
            this.btnCancel.IconType = ABCControls.ABCSimpleButton.ABCIconType.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageIndex = 0;
            this.btnCancel.IsVisible = true;
            this.btnCancel.Location = new System.Drawing.Point(503, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.OwnerView = null;
            this.btnCancel.Size = new System.Drawing.Size(92, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.ButtonType = ABCControls.ABCSimpleButton.ABCButtonType.None;
            this.btnSelect.Comment = null;
            this.btnSelect.DataSource = null;
            this.btnSelect.FieldGroup = null;
            this.btnSelect.IconType = ABCControls.ABCSimpleButton.ABCIconType.Approve;
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.ImageIndex = 0;
            this.btnSelect.IsVisible = true;
            this.btnSelect.Location = new System.Drawing.Point(601, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.OwnerView = null;
            this.btnSelect.Size = new System.Drawing.Size(92, 27);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "&Chọn";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // xtraTabControl1
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "xtraTabControl1";
            this.tabControl.SelectedTabPage = this.mainTab;
            this.tabControl.Size = new System.Drawing.Size(699, 351);
            this.tabControl.TabIndex = 1;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.mainTab,
            this.itemTab});
            // 
            // xtraTabPage1
            // 
            this.mainTab.Name = "xtraTabPage1";
            this.mainTab.Size = new System.Drawing.Size(693, 323);
            this.mainTab.Text = "xtraTabPage1";
            // 
            // xtraTabPage2
            // 
            this.itemTab.Name = "xtraTabPage2";
            this.itemTab.Size = new System.Drawing.Size(0, 0);
            this.itemTab.Text = "xtraTabPage2";
            // 
            // ABCRelationChooser
            // 
            this.ClientSize = new System.Drawing.Size(699, 388);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelControl1);
            this.MinimizeBox = false;
            this.Name = "ABCRelationChooser";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        public void ReloadDatas ( )
        {
            if ( RelationConfig==null )
                return;
       
            VoucherProvider.ReCalculateRelation( RelationConfig.GERelationConfigID );

            if ( MainGridCtrl!=null&&RelationConfig.IsFromSource )
            {
                object datasource=null;
                Type objType=BusinessObjectFactory.GetBusinessObjectType( RelationConfig.SourceTableName );
                if ( objType!=null )
                {
                    Type typeABCList=typeof( ABCList<> ).MakeGenericType( objType );
                    MethodInfo method;
                    if ( typeABCList!=null )
                    {
                        datasource=ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeABCList );
                        method=typeABCList.GetMethod( "LoadData" , new Type[] { typeof( List<BusinessObject> ) } );
                        method.Invoke( datasource , new object[] { VoucherProvider.GetOpenningRelationObjects( RelationConfig.GERelationConfigID , false ) } );
                    }
                }
                bindingMain.DataSource=datasource;
                bindingMain.ResetBindings( false );

                this.MainGridCtrl.GridDefaultView.BestFitColumns();
            }

            if ( ItemGridCtrl!=null&&RelationConfig.IsFromSourceItem )
            {
                object datasource=null;
                Type objType=BusinessObjectFactory.GetBusinessObjectType( RelationConfig.SourceItemTableName );
                if ( objType!=null )
                {
                    Type typeABCList=typeof( ABCList<> ).MakeGenericType( objType );
                    MethodInfo method;
                    if ( typeABCList!=null )
                    {
                        datasource=ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeABCList );
                        method=typeABCList.GetMethod( "LoadData" , new Type[] { typeof( List<BusinessObject> ) } );
                        method.Invoke( datasource , new object[] { VoucherProvider.GetOpenningRelationObjects( RelationConfig.GERelationConfigID , true ) } );
                    }
                }
                bindingItem.DataSource=datasource;
                bindingItem.ResetBindings( false );

                this.ItemGridCtrl.GridDefaultView.BestFitColumns();
            }
        }

        private void btnSelect_Click ( object sender , EventArgs e )
        {
            SelectedObjects.Clear();

            if ( RelationConfig==null )
            {
                this.Close();
                return;
            }

            DestinyResult=null;
            if ( RelationConfig.IsFromSource&&tabControl.SelectedTabPage==mainTab )
            {
                #region From Main
                foreach ( BusinessObject obj in ( (BindingSource)MainGridCtrl.GridDataSource ).DataSource as System.Collections.IList )
                {
                    if ( obj.Selected )
                        SelectedObjects.Add( obj );
                }
                if ( SelectedObjects.Count<=0 )
                {
                    ABCMessageBox.Show( "Vui lòng chọn dữ liệu trước khi tạo phiếu !" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                    this.Close();
                    return;
                }
                if ( !RelationConfig.IsMultiSource&&SelectedObjects.Count>1 )
                {
                    ABCMessageBox.Show( "Chỉ được chọn 1 dòng !" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                    this.Close();
                    return;
                }
                #endregion

                DestinyResult=VoucherProvider.CreateVoucherFromRelation( RelationConfig.GERelationConfigID , false , SelectedObjects );
            }
            else if ( RelationConfig.IsFromSourceItem&&tabControl.SelectedTabPage==itemTab )
            {
                #region From Item
                foreach ( BusinessObject obj in ( (BindingSource)ItemGridCtrl.GridDataSource ).DataSource as System.Collections.IList )
                {
                    if ( obj.Selected )
                        SelectedObjects.Add( obj );
                }
                if ( SelectedObjects.Count<=0 )
                {
                    ABCMessageBox.Show( "Vui lòng chọn dữ liệu trước khi tạo phiếu !" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                    this.Close();
                    return;
                }
                if ( !RelationConfig.IsMultiSource&&SelectedObjects.Count>1 )
                {
                    ABCMessageBox.Show( "Chỉ được chọn 1 dòng !" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                    this.Close();
                    return;
                }
                #endregion

                DestinyResult=VoucherProvider.CreateVoucherFromRelation( RelationConfig.GERelationConfigID , true , SelectedObjects );
            }
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
            if ( DestinyResult!=null&&DestinyResult.GetID()!=Guid.Empty )
                ABCScreenManager.Instance.RunLink( RelationConfig.DestinyTableName , ABCCommon.ViewMode.Runtime , false , DestinyResult.GetID() , ABCCommon.ABCScreenAction.None );
            this.Close();
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            SelectedObjects.Clear();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

    }

}
