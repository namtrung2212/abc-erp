using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using ABCControls;
using ABCHelper;
using ABCProvider;
using ABCProvider;

using ABCBusinessEntities;
using ABCScreen.Data;
namespace ABCScreen.UI
{
    public class ABCSelectionView : DevExpress.XtraEditors.XtraForm
    {
        public String TableName;
        public String ConditionString;
        public List<BusinessObject> SelectedObjects;

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ABCControls.ABCSimpleButton btnCancel;
        private ABCControls.ABCSimpleButton btnSelect;
        ABCGridControl GridCtrl;

        public ABCSelectionView (String strTableName,String strConditionString )
        {
            TableName=strTableName;
            ConditionString=strConditionString;
            SelectedObjects=new List<BusinessObject>();

            InitializeComponent();
            InitializeControls();
            this.StartPosition=FormStartPosition.CenterParent;

            ReloadDatas();
        }
        
        #region UI

        public void InitializeControls ( )
        {

            GridCtrl=new ABCGridControl();
            GridCtrl.Initialize( TableName );
            ABCGridColumn col=new ABCGridColumn();
            col.FieldName=ABCCommon.ABCConstString.colSelected;
            col.Caption="Chọn";
            col.TableName=TableName;
            col.VisibleIndex=0;
            col.Visible=true;
            col.Width=20;
            col.OptionsColumn.AllowEdit=true;
            col.FilterMode=DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            col.OptionsFilter.AllowAutoFilter=true;
            col.ColumnEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            GridCtrl.GridDefaultView.Columns.Insert( 0 , col );


            GridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            GridCtrl.Parent=this;
            GridCtrl.ShowSaveButton=false;
            GridCtrl.ShowDeleteButton=false;
            GridCtrl.ShowRefreshButton=false;
            GridCtrl.EnableFocusedCell=false;
            GridCtrl.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            GridCtrl.BringToFront();

            this.Shown+=new EventHandler( ABCSelectionView_Shown );
            this.ShowInTaskbar=false;

            this.Text="Danh sách "+DataConfigProvider.GetTableCaption( TableName );
        }

        void ABCSelectionView_Shown ( object sender , EventArgs e )
        {
            if ( this.TopLevel==false )
                this.BringToFront();
        }

        private void InitializeComponent ( )
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ABCSelectionView));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new ABCControls.ABCSimpleButton();
            this.btnSelect = new ABCControls.ABCSimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
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
            // ABCSelectionView
            // 
            this.ClientSize = new System.Drawing.Size(699, 388);
            this.Controls.Add(this.panelControl1);
            this.MinimizeBox = false;
            this.Name = "ABCSelectionView";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        public void ReloadDatas ( )
        {
            if ( !String.IsNullOrWhiteSpace( TableName )&&GridCtrl!=null&&DataStructureProvider.IsExistedTable( TableName ) )
            {

                BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( TableName );

                String strQuery=QueryGenerator.GenSelect( TableName , "*" , true );
                strQuery=QueryGenerator.AddCondition( strQuery , ConditionString );

                if ( DataStructureProvider.IsTableColumn( TableName , ABCCommon.ABCConstString.colDocumentDate ) )
                    strQuery=strQuery+String.Format( @" ORDER BY {0} DESC" , ABCCommon.ABCConstString.colDocumentDate );

                GridCtrl.GridDataSource=ctrl.GetListByQuery( strQuery );
                GridCtrl.RefreshDataSource();
                this.GridCtrl.GridDefaultView.BestFitColumns();

            }
        }

        private void btnSelect_Click ( object sender , EventArgs e )
        {
            SelectedObjects.Clear();
            foreach ( BusinessObject obj in (List<BusinessObject>)GridCtrl.GridDataSource )
            {
                if ( obj.Selected )
                    SelectedObjects.Add( obj );
            }
            this.Close();
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            SelectedObjects.Clear();
            this.Close();
        }

    }

}
