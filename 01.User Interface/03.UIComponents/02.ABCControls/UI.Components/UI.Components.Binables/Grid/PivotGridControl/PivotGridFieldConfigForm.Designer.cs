namespace ABCControls
{
    partial class PivotGridFieldConfigForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( PivotGridFieldConfigForm ) );
            this.DisplayGridCtrl=new ABCControls.ABCPivotGridControl();
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2=new DevExpress.XtraEditors.SplitContainerControl();
            this.ConfigGridCtrl=new ABCBaseGridControl();
            this.ConfigGridView=new ABCControls.ABCGridView();
            this.gridColFieldName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoFieldNameChooser=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColDisplay=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRepoType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColTableName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DisplayGridCtrl
            // 
            this.DisplayGridCtrl.ChartType=DevExpress.XtraCharts.ViewType.Line;
            this.DisplayGridCtrl.Comment=null;
            this.DisplayGridCtrl.DataMember=null;
            this.DisplayGridCtrl.DataSource=null;
            this.DisplayGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DisplayGridCtrl.DrawFocusedCellRect=true;
            this.DisplayGridCtrl.FieldConfigs=null;
            this.DisplayGridCtrl.FieldGroup=null;
            this.DisplayGridCtrl.GridDataSource=null;
            this.DisplayGridCtrl.GroupFieldsInCustomizationWindow=true;
            this.DisplayGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DisplayGridCtrl.Name="DisplayGridCtrl";
            this.DisplayGridCtrl.OwnerView=null;
            this.DisplayGridCtrl.RowTreeWidth=100;
            this.DisplayGridCtrl.Script=null;
            this.DisplayGridCtrl.ShowColumnChooser=true;
            this.DisplayGridCtrl.ShowColumnGrandTotalHeader=true;
            this.DisplayGridCtrl.ShowColumnGrandTotals=true;
            this.DisplayGridCtrl.ShowColumnHeaders=true;
            this.DisplayGridCtrl.ShowColumnTotals=true;
            this.DisplayGridCtrl.ShowCustomTotalsForSingleValues=false;
            this.DisplayGridCtrl.ShowDataHeaders=true;
            this.DisplayGridCtrl.ShowExportButton=true;
            this.DisplayGridCtrl.ShowFilterHeaders=true;
            this.DisplayGridCtrl.ShowFilterSeparatorBar=true;
            this.DisplayGridCtrl.ShowGrandTotalsForSingleValues=false;
            this.DisplayGridCtrl.ShowHorzLines=true;
            this.DisplayGridCtrl.ShowMenuBar=true;
            this.DisplayGridCtrl.ShowPrintButton=true;
            this.DisplayGridCtrl.ShowRefreshButton=true;
            this.DisplayGridCtrl.ShowRowGrandTotalHeader=true;
            this.DisplayGridCtrl.ShowRowGrandTotals=true;
            this.DisplayGridCtrl.ShowRowHeaders=true;
            this.DisplayGridCtrl.ShowRowTotals=true;
            this.DisplayGridCtrl.ShowTotalsForSingleValues=false;
            this.DisplayGridCtrl.ShowVertLines=true;
            this.DisplayGridCtrl.Size=new System.Drawing.Size( 575 , 578 );
            this.DisplayGridCtrl.TabIndex=1;
            this.DisplayGridCtrl.UseChartControl=false;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.splitContainerControl2 );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.DisplayGridCtrl );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 932 , 578 );
            this.splitContainerControl1.SplitterPosition=352;
            this.splitContainerControl1.TabIndex=2;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl2.Horizontal=false;
            this.splitContainerControl2.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl2.Name="splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add( this.ConfigGridCtrl );
            this.splitContainerControl2.Panel1.Text="Panel1";
            this.splitContainerControl2.Panel2.Text="Panel2";
            this.splitContainerControl2.Size=new System.Drawing.Size( 352 , 578 );
            this.splitContainerControl2.SplitterPosition=303;
            this.splitContainerControl2.TabIndex=3;
            this.splitContainerControl2.Text="splitContainerControl2";
            // 
            // ConfigGridCtrl
            // 
            this.ConfigGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ConfigGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.ConfigGridCtrl.MainView=this.ConfigGridView;
            this.ConfigGridCtrl.Name="ConfigGridCtrl";
            this.ConfigGridCtrl.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repoFieldNameChooser} );
            this.ConfigGridCtrl.Size=new System.Drawing.Size( 352 , 270 );
            this.ConfigGridCtrl.TabIndex=2;
            this.ConfigGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ConfigGridView} );
            // 
            // ConfigGridView
            // 
            this.ConfigGridView.ABCGridControl=null;
            this.ConfigGridView.ActiveFilterEnabled=false;
            this.ConfigGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColFieldName,
            this.gridColDisplay,
            this.gridColRepoType,
            this.gridColTableName} );
            this.ConfigGridView.GridControl=this.ConfigGridCtrl;
            this.ConfigGridView.Name="ConfigGridView";
            this.ConfigGridView.OptionsView.NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.ConfigGridView.OptionsView.ShowAutoFilterRow=true;
            this.ConfigGridView.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.ConfigGridView.OptionsView.ShowGroupPanel=false;
            this.ConfigGridView.OptionsView.ShowViewCaption=true;
            this.ConfigGridView.TableName=null;
            this.ConfigGridView.ViewCaption="Column Configuration";
            // 
            // gridColFieldName
            // 
            this.gridColFieldName.Caption="Field";
            this.gridColFieldName.ColumnEdit=this.repoFieldNameChooser;
            this.gridColFieldName.FieldName="FieldName";
            this.gridColFieldName.Name="gridColFieldName";
            this.gridColFieldName.Visible=true;
            this.gridColFieldName.VisibleIndex=0;
            // 
            // repoFieldNameChooser
            // 
            this.repoFieldNameChooser.AutoHeight=false;
            this.repoFieldNameChooser.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repoFieldNameChooser.Name="repoFieldNameChooser";
            // 
            // gridColDisplay
            // 
            this.gridColDisplay.Caption="Display";
            this.gridColDisplay.FieldName="Caption";
            this.gridColDisplay.Name="gridColDisplay";
            this.gridColDisplay.Visible=true;
            this.gridColDisplay.VisibleIndex=1;
            // 
            // gridColRepoType
            // 
            this.gridColRepoType.Caption="Repository";
            this.gridColRepoType.FieldName="RepoType";
            this.gridColRepoType.Name="gridColRepoType";
            this.gridColRepoType.Visible=true;
            this.gridColRepoType.VisibleIndex=2;
            // 
            // gridColTableName
            // 
            this.gridColTableName.Caption="TableName";
            this.gridColTableName.FieldName="TableName";
            this.gridColTableName.Name="gridColTableName";
            this.gridColTableName.Visible=true;
            this.gridColTableName.VisibleIndex=3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 578 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 932 , 35 );
            this.panelControl1.TabIndex=3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 852 , 5 );
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
            this.btnSave.Location=new System.Drawing.Point( 769 , 5 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // PivotGridFieldConfigForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 932 , 613 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="PivotGridFieldConfigForm";
            this.Text="GridColumnConfigForm";
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).EndInit();
            this.splitContainerControl2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private ABCPivotGridControl DisplayGridCtrl;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private ABCBaseGridControl ConfigGridCtrl;
        private ABCGridView ConfigGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFieldName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRepoType;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFieldNameChooser;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTableName;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
    }
}