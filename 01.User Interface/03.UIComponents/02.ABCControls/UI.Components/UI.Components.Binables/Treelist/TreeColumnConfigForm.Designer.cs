namespace ABCControls
{
    partial class TreeColumnConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing&&( components!=null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.components=new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( TreeColumnConfigForm ) );
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.ColumnConfigGridCtrl=new ABCControls.ABCBaseGridControl();
            this.ColumnConfigGridView=new ABCControls.ABCGridView();
            this.gridColDisplay=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAllowEdit=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColFixed=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSumType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl3=new DevExpress.XtraEditors.SplitContainerControl();
            this.DataConfigTreeCtrl=new DevExpress.XtraTreeList.TreeList();
            this.DataConfigDetailVGridCtrl=new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.repoFieldNameChooser=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repoTableNameChooser=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnRefreshDisplayTree=new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.DisplayTreeListCtrl=new ABCControls.ABCTreeList();
            this.barManager1=new DevExpress.XtraBars.BarManager( this.components );
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl3 ) ).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.DataConfigTreeCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DataConfigDetailVGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoTableNameChooser ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.ColumnConfigGridCtrl );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.DisplayTreeListCtrl );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 817 , 283 );
            this.splitContainerControl1.SplitterPosition=392;
            this.splitContainerControl1.TabIndex=2;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // ColumnConfigGridCtrl
            // 
            this.ColumnConfigGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ColumnConfigGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.ColumnConfigGridCtrl.MainView=this.ColumnConfigGridView;
            this.ColumnConfigGridCtrl.Name="ColumnConfigGridCtrl";
            this.ColumnConfigGridCtrl.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1} );
            this.ColumnConfigGridCtrl.Size=new System.Drawing.Size( 392 , 283 );
            this.ColumnConfigGridCtrl.TabIndex=2;
            this.ColumnConfigGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ColumnConfigGridView} );
            // 
            // ColumnConfigGridView
            // 
            this.ColumnConfigGridView.ABCGridControl=null;
            this.ColumnConfigGridView.ActiveFilterEnabled=false;
            this.ColumnConfigGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColDisplay,
            this.gridColAllowEdit,
            this.gridColFixed,
            this.gridColSumType} );
            this.ColumnConfigGridView.GridControl=this.ColumnConfigGridCtrl;
            this.ColumnConfigGridView.Name="ColumnConfigGridView";
            this.ColumnConfigGridView.OptionsView.NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.ColumnConfigGridView.OptionsView.ShowAutoFilterRow=true;
            this.ColumnConfigGridView.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.ColumnConfigGridView.OptionsView.ShowGroupPanel=false;
            this.ColumnConfigGridView.OptionsView.ShowViewCaption=true;
            this.ColumnConfigGridView.TableName=null;
            this.ColumnConfigGridView.ViewCaption="Column Configuration";
            // 
            // gridColDisplay
            // 
            this.gridColDisplay.Caption="Display";
            this.gridColDisplay.FieldName="Caption";
            this.gridColDisplay.Name="gridColDisplay";
            this.gridColDisplay.Visible=true;
            this.gridColDisplay.VisibleIndex=0;
            // 
            // gridColAllowEdit
            // 
            this.gridColAllowEdit.Caption="AllowEdit";
            this.gridColAllowEdit.ColumnEdit=this.repositoryItemCheckEdit1;
            this.gridColAllowEdit.FieldName="AllowEdit";
            this.gridColAllowEdit.Name="gridColAllowEdit";
            this.gridColAllowEdit.Visible=true;
            this.gridColAllowEdit.VisibleIndex=2;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight=false;
            this.repositoryItemCheckEdit1.Name="repositoryItemCheckEdit1";
            // 
            // gridColFixed
            // 
            this.gridColFixed.Caption="Fixed";
            this.gridColFixed.FieldName="Fixed";
            this.gridColFixed.Name="gridColFixed";
            this.gridColFixed.Visible=true;
            this.gridColFixed.VisibleIndex=1;
            // 
            // gridColSumType
            // 
            this.gridColSumType.Caption="Summary";
            this.gridColSumType.FieldName="SumType";
            this.gridColSumType.Name="gridColSumType";
            this.gridColSumType.Visible=true;
            this.gridColSumType.VisibleIndex=3;
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.splitContainerControl3.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl3.Location=new System.Drawing.Point( 0 , 283 );
            this.splitContainerControl3.Name="splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add( this.DataConfigTreeCtrl );
            this.splitContainerControl3.Panel1.Text="Panel1";
            this.splitContainerControl3.Panel2.Controls.Add( this.DataConfigDetailVGridCtrl );
            this.splitContainerControl3.Panel2.Text="Panel2";
            this.splitContainerControl3.Size=new System.Drawing.Size( 817 , 181 );
            this.splitContainerControl3.SplitterPosition=244;
            this.splitContainerControl3.TabIndex=2;
            this.splitContainerControl3.Text="splitContainerControl3";
            // 
            // DataConfigTreeCtrl
            // 
            this.DataConfigTreeCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DataConfigTreeCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DataConfigTreeCtrl.Name="DataConfigTreeCtrl";
            this.DataConfigTreeCtrl.Size=new System.Drawing.Size( 568 , 181 );
            this.DataConfigTreeCtrl.TabIndex=0;
            // 
            // DataConfigDetailVGridCtrl
            // 
            this.DataConfigDetailVGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DataConfigDetailVGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DataConfigDetailVGridCtrl.Name="DataConfigDetailVGridCtrl";
            this.DataConfigDetailVGridCtrl.Size=new System.Drawing.Size( 244 , 181 );
            this.DataConfigDetailVGridCtrl.TabIndex=1;
            // 
            // repoFieldNameChooser
            // 
            this.repoFieldNameChooser.AutoHeight=false;
            this.repoFieldNameChooser.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repoFieldNameChooser.Name="repoFieldNameChooser";
            // 
            // repoTableNameChooser
            // 
            this.repoTableNameChooser.AutoHeight=false;
            this.repoTableNameChooser.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repoTableNameChooser.Name="repoTableNameChooser";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnRefreshDisplayTree );
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 464 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 817 , 35 );
            this.panelControl1.TabIndex=3;
            // 
            // btnRefreshDisplayTree
            // 
            this.btnRefreshDisplayTree.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnRefreshDisplayTree.Location=new System.Drawing.Point( 5 , 6 );
            this.btnRefreshDisplayTree.Name="btnRefreshDisplayTree";
            this.btnRefreshDisplayTree.Size=new System.Drawing.Size( 152 , 26 );
            this.btnRefreshDisplayTree.TabIndex=4;
            this.btnRefreshDisplayTree.Text="&Refresh Display Tree";
            this.btnRefreshDisplayTree.Click+=new System.EventHandler( this.btnRefreshDisplayTree_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 737 , 5 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=3;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnSave
            // 
            this.btnSave.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnSave.Image=( (System.Drawing.Image)( resources.GetObject( "btnSave.Image" ) ) );
            this.btnSave.Location=new System.Drawing.Point( 654 , 5 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // DisplayTreeListCtrl
            // 
            this.DisplayTreeListCtrl.ColumnConfigs=null;
            this.DisplayTreeListCtrl.Comment=null;
            this.DisplayTreeListCtrl.DataMember=null;
            this.DisplayTreeListCtrl.DataSource=null;
            this.DisplayTreeListCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DisplayTreeListCtrl.FieldGroup=null;
            this.DisplayTreeListCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DisplayTreeListCtrl.Name="DisplayTreeListCtrl";
            this.DisplayTreeListCtrl.OwnerView=null;
            this.DisplayTreeListCtrl.Script=null;
            this.DisplayTreeListCtrl.ShowColumnChooser=true;
            this.DisplayTreeListCtrl.ShowDeleteButton=true;
            this.DisplayTreeListCtrl.ShowExportButton=true;
            this.DisplayTreeListCtrl.ShowFilterButton=true;
            this.DisplayTreeListCtrl.ShowMenuBar=true;
            this.DisplayTreeListCtrl.ShowPrintButton=true;
            this.DisplayTreeListCtrl.ShowRefreshButton=true;
            this.DisplayTreeListCtrl.ShowSaveButton=true;
            this.DisplayTreeListCtrl.Size=new System.Drawing.Size( 420 , 283 );
            this.DisplayTreeListCtrl.TabIndex=1;
            this.DisplayTreeListCtrl.TreeListDataSource=null;
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add( this.barDockControlTop );
            this.barManager1.DockControls.Add( this.barDockControlBottom );
            this.barManager1.DockControls.Add( this.barDockControlLeft );
            this.barManager1.DockControls.Add( this.barDockControlRight );
            this.barManager1.Form=this;
            this.barManager1.MaxItemId=0;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation=false;
            this.barDockControlTop.Dock=System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlTop.Size=new System.Drawing.Size( 817 , 0 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 499 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 817 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 499 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 817 , 0 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 499 );
            // 
            // TreeColumnConfigForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 817 , 499 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.splitContainerControl3 );
            this.Controls.Add( this.panelControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name="TreeColumnConfigForm";
            this.Text="GridColumnConfigForm";
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl3 ) ).EndInit();
            this.splitContainerControl3.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.DataConfigTreeCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DataConfigDetailVGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoTableNameChooser ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private ABCControls.ABCBaseGridControl ColumnConfigGridCtrl;
        private ABCGridView ColumnConfigGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFixed;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSumType;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFieldNameChooser;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoTableNameChooser;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAllowEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraTreeList.TreeList DataConfigTreeCtrl;
        private ABCTreeList DisplayTreeListCtrl;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl3;
        private DevExpress.XtraVerticalGrid.PropertyGridControl DataConfigDetailVGridCtrl;
        private DevExpress.XtraEditors.SimpleButton btnRefreshDisplayTree;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}