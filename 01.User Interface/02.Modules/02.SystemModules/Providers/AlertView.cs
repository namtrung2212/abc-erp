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
    public class AlertView : DevExpress.XtraEditors.XtraForm
    {
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        Dictionary<Guid , ABCGridControl> lstGridControls;
        public AlertView ()
        {
            InitializeComponent();
            InitializeControls();
            this.StartPosition=FormStartPosition.CenterParent;

          //  ReloadDatas();
        }
        
        #region UI

        public void InitializeControls ( )
        {
            lstGridControls=new Dictionary<Guid , ABCGridControl>();
            this.xtraTabControl1.TabPages.Clear();
            foreach ( GEAlertsInfo alertInfo in AlertProvider.GetAlertConfigs( ABCUserProvider.CurrentUser.ADUserID ).Values )
            {
                DevExpress.XtraTab.XtraTabPage tabPage=new DevExpress.XtraTab.XtraTabPage();
                tabPage.Name=alertInfo.Name;
                tabPage.Text=alertInfo.ShortCaption;
                this.xtraTabControl1.TabPages.Add( tabPage );

                ABCGridControl GridCtrl=new ABCGridControl();
                GridCtrl.Initialize( alertInfo.TableName );
                GridCtrl.Tag=alertInfo;
                GridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
                GridCtrl.Parent=tabPage;
                GridCtrl.ShowSaveButton=false;
                GridCtrl.ShowDeleteButton=false;
                GridCtrl.ShowRefreshButton=false;
                GridCtrl.EnableFocusedCell=false;
                GridCtrl.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                GridCtrl.BringToFront();
                lstGridControls.Add( alertInfo.GEAlertID , GridCtrl );

             
            }
            if ( this.xtraTabControl1.TabPages.Count<=3 )
                this.xtraTabControl1.HeaderLocation=DevExpress.XtraTab.TabHeaderLocation.Top;
            else
                this.xtraTabControl1.HeaderLocation=DevExpress.XtraTab.TabHeaderLocation.Left;

            this.Shown+=new EventHandler( AlertView_Shown );
            this.ShowInTaskbar=false;
            this.StartPosition=FormStartPosition.CenterScreen;
            this.Text="Thông báo";
        }

        void AlertView_Shown ( object sender , EventArgs e )
        {
            if ( this.TopLevel==false )
                this.BringToFront();

            ReloadDatas();
        }

        private void InitializeComponent ( )
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.xtraTabControl1.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(699, 388);
            this.xtraTabControl1.TabIndex = 0;
            // 
            // AlertView
            // 
            this.ClientSize = new System.Drawing.Size(699, 388);
            this.Controls.Add(this.xtraTabControl1);
            this.MinimizeBox = false;
            this.Name = "AlertView";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

    
        public void ReloadDatas ( )
        {
            foreach ( Guid iD in lstGridControls.Keys )
            {
                List<BusinessObject> lstObjs=AlertProvider.GetAlertData( iD );

                ABCGridControl GridCtrl=lstGridControls[iD];

                #region datasource
                BindingSource binding=new BindingSource();
                GridCtrl.GridDataSource=binding;
                GridCtrl.RefreshDataSource();

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
                        method.Invoke( datasource , new object[] { lstObjs } );

                    }
                }

                binding.DataSource=datasource;
                binding.ResetBindings( false );
                
                #endregion
                if ( lstObjs.Count<=0 )
                    GridCtrl.Parent.Visible=false;
                GridCtrl.GridDefaultView.BestFitColumns();

            }
        }

    }

}
