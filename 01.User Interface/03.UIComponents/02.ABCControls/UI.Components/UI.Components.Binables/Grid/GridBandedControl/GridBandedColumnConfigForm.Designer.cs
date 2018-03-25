namespace ABCControls
{
    partial class GridBandedColumnConfigForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( GridBandedColumnConfigForm ) );
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2=new DevExpress.XtraEditors.SplitContainerControl();
            this.ColumnConfigGridCtrl=new ABCControls.ABCBaseGridControl();
            this.ColumnConfigGridView=new ABCControls.ABCGridView();
            this.gridColFieldName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoFieldNameChooser=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repoFilterStringEditor=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repoAllowEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColCaption=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColCustomizeCaption=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColUseAlias=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColFixed=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColBandFixed=new DevExpress.XtraGrid.Columns.GridColumn();
        //    this.gridColRepoType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSumType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGroupBy=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColFilterString=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAllowEdit=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationParentField=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationParentControl=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationChildField=new DevExpress.XtraGrid.Columns.GridColumn();
            this.BandConfigGridCtrl=new ABCControls.ABCBaseGridControl();
            this.BandConfigGridView=new ABCControls.ABCGridView();
            this.gridColBandName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.DisplayGridCtrl=new ABCControls.ABCBaseBandedGridControl();
            this.DisplayGridView=new ABCControls.ABCGridBandedView();
            this.gridBand1=new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.gridColTableName=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFilterStringEditor ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoAllowEdit ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.BandConfigGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.BandConfigGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal=false;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.splitContainerControl2 );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.DisplayGridCtrl );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 776 , 406 );
            this.splitContainerControl1.SplitterPosition=251;
            this.splitContainerControl1.TabIndex=2;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl2.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl2.Name="splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add( this.ColumnConfigGridCtrl );
            this.splitContainerControl2.Panel1.Text="Panel1";
            this.splitContainerControl2.Panel2.Controls.Add( this.BandConfigGridCtrl );
            this.splitContainerControl2.Panel2.Text="Panel2";
            this.splitContainerControl2.Size=new System.Drawing.Size( 776 , 251 );
            this.splitContainerControl2.SplitterPosition=191;
            this.splitContainerControl2.TabIndex=4;
            this.splitContainerControl2.Text="splitContainerControl2";
            // 
            // ColumnConfigGridCtrl
            // 
            this.ColumnConfigGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ColumnConfigGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.ColumnConfigGridCtrl.MainView=this.ColumnConfigGridView;
            this.ColumnConfigGridCtrl.Name="ColumnConfigGridCtrl";
            this.ColumnConfigGridCtrl.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repoFieldNameChooser,this.repoFilterStringEditor,this.repoFilterStringEditor} );
            this.ColumnConfigGridCtrl.Size=new System.Drawing.Size( 580 , 251 );
            this.ColumnConfigGridCtrl.TabIndex=2;
            this.ColumnConfigGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ColumnConfigGridView} );
            // 
            // ColumnConfigGridView
            // 
            this.ColumnConfigGridView.ABCGridControl=null;
            this.ColumnConfigGridView.ActiveFilterEnabled=false;
            this.ColumnConfigGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColTableName,
            this.gridColFieldName,
            this.gridColCaption,
            this.gridColCustomizeCaption,
              this.gridColUseAlias,
            this.gridColFixed,
         //   this.gridColRepoType,
            this.gridColSumType,
            this.gridColGroupBy,
            this.gridColAllowEdit,
            this.gridColFilterString,
            this.gridColRelationParentField,
            this.gridColRelationParentControl,
            this.gridColRelationChildField} );

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
            // gridColFieldName
            // 
            this.gridColFieldName.Caption="Column";
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
            // repoFilterStringEditor
            // 
            this.repoFilterStringEditor.AutoHeight=false;
            this.repoFilterStringEditor.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repoFilterStringEditor.Name="repoFilterStringEditor";          // 
            // repositoryItemCheckEdit1
            // 
            this.repoAllowEdit.AutoHeight=false;
            this.repoAllowEdit.Name="repositoryItemCheckEdit1";
            // 
            // gridColDisplay
            // 
            this.gridColCaption.Caption="Caption";
            this.gridColCaption.FieldName="Caption";
            this.gridColCaption.Name="gridColDisplay";
            this.gridColCaption.Visible=true;
            this.gridColCaption.VisibleIndex=1;
            // 
            // gridColCustomizeCaption
            // 
            this.gridColCustomizeCaption.Caption="Caption in ColumnChooser";
            this.gridColCustomizeCaption.FieldName="CustomizationCaption";
            this.gridColCustomizeCaption.Name="gridColCustomizeCaption";
            this.gridColCustomizeCaption.Visible=true;
            this.gridColCustomizeCaption.VisibleIndex=1;  // 
            // gridColUseAlias
            // 
            this.gridColUseAlias.Caption="Use Alias";
            this.gridColUseAlias.FieldName="IsUseAlias";
            this.gridColUseAlias.Name="gridColUseAlias";
            this.gridColUseAlias.Visible=true;
            this.gridColUseAlias.VisibleIndex=1;            // 

            // 
            // gridColFixed
            // 
            this.gridColFixed.Caption="Fixed";
            this.gridColFixed.FieldName="Fixed";
            this.gridColFixed.Name="gridColFixed";
            this.gridColFixed.Visible=true;
            this.gridColFixed.VisibleIndex=2;
            // 
            // gridColRepoType
            // 
            //this.gridColRepoType.Caption="Repository";
            //this.gridColRepoType.FieldName="RepoType";
            //this.gridColRepoType.Name="gridColRepoType";
            //this.gridColRepoType.Visible=true;
            //this.gridColRepoType.VisibleIndex=3;
            // 
            // gridColSumType
            // 
            this.gridColSumType.Caption="Summary";
            this.gridColSumType.FieldName="SumType";
            this.gridColSumType.Name="gridColSumType";
            this.gridColSumType.Visible=true;
            this.gridColSumType.VisibleIndex=4;            // 
            // 
            // gridColGroupBy
            // 
            this.gridColGroupBy.Caption="GroupBy";
            this.gridColGroupBy.FieldName="GroupBy";
            this.gridColGroupBy.Name="gridColGroupBy";
            this.gridColGroupBy.Visible=true;
            this.gridColGroupBy.VisibleIndex=4;            // 

            // gridColAllowEdit
            // 
            this.gridColAllowEdit.Caption="AllowEdit";
            this.gridColAllowEdit.ColumnEdit=this.repoAllowEdit;
            this.gridColAllowEdit.FieldName="AllowEdit";
            this.gridColAllowEdit.Name="gridColAllowEdit";
            this.gridColAllowEdit.Visible=true;
            this.gridColAllowEdit.VisibleIndex=5;  // 
              // 
            // gridColFilterString
            // 
            this.gridColFilterString.Caption="FilterString";
            this.gridColFilterString.FieldName="FilterString";
            this.gridColFilterString.ColumnEdit=this.repoFilterStringEditor;
            this.gridColFilterString.Name="gridColFilterString";
            this.gridColFilterString.Visible=true;
            this.gridColFilterString.VisibleIndex=6;

            // 
            // gridColRelationParentCol
            // 
            this.gridColRelationParentField.Caption="Parent Column";
            this.gridColRelationParentField.FieldName="RelationParentField";
            this.gridColRelationParentField.Name="gridColRelationParentField";
            this.gridColRelationParentField.Visible=true;
            this.gridColRelationParentField.VisibleIndex=7;   // 

            // 
            // gridColRelationParentCol
            // 
            this.gridColRelationParentControl.Caption="Parent Control";
            this.gridColRelationParentControl.FieldName="RelationParentControl";
            this.gridColRelationParentControl.Name="gridColRelationParentControl";
            this.gridColRelationParentControl.Visible=true;
            this.gridColRelationParentControl.VisibleIndex=8;   // 

            // 
            // gridColRelationChildField
            // 
            this.gridColRelationChildField.Caption="Child Field";
            this.gridColRelationChildField.FieldName="RelationChildField";
            this.gridColRelationChildField.Name="gridColRelationChildField";
            this.gridColRelationChildField.Visible=true;
            this.gridColRelationChildField.VisibleIndex=8;   // 

             
            // BandConfigGridCtrl
            // 
            this.BandConfigGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.BandConfigGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.BandConfigGridCtrl.MainView=this.BandConfigGridView;
            this.BandConfigGridCtrl.Name="BandConfigGridCtrl";
            this.BandConfigGridCtrl.Size=new System.Drawing.Size( 191 , 251 );
            this.BandConfigGridCtrl.TabIndex=3;
            this.BandConfigGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.BandConfigGridView} );
            // 
            // BandConfigGridView
            // 
            this.BandConfigGridView.ABCGridControl=null;
            this.BandConfigGridView.ActiveFilterEnabled=false;
            this.BandConfigGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColBandName,this.gridColBandFixed} );
            this.BandConfigGridView.GridControl=this.BandConfigGridCtrl;
            this.BandConfigGridView.Name="BandConfigGridView";
            this.BandConfigGridView.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.True;
            this.BandConfigGridView.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.True;
            this.BandConfigGridView.OptionsView.NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.BandConfigGridView.OptionsView.ShowAutoFilterRow=true;
            this.BandConfigGridView.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.BandConfigGridView.OptionsView.ShowGroupPanel=false;
            this.BandConfigGridView.OptionsView.ShowViewCaption=true;
            this.BandConfigGridView.TableName=null;
            this.BandConfigGridView.ViewCaption="Bands";
            // 
            // gridColBandName
            // 
            this.gridColBandName.Caption="Band Name";
            this.gridColBandName.FieldName="Caption";
            this.gridColBandName.Name="gridColBandName";
            this.gridColBandName.Visible=true;
            this.gridColBandName.VisibleIndex=0;
               // 
            // gridColBandFixed
            // 
            this.gridColBandFixed.Caption="Fixed";
            this.gridColBandFixed.FieldName="Fixed";
            this.gridColBandFixed.Name="gridColBandFixed";
            this.gridColBandFixed.Visible=true;
            this.gridColBandFixed.VisibleIndex=1;
            // 
            // DisplayGridCtrl
            // 
            this.DisplayGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DisplayGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DisplayGridCtrl.MainView=this.DisplayGridView;
            this.DisplayGridCtrl.Name="DisplayGridCtrl";
            this.DisplayGridCtrl.Size=new System.Drawing.Size( 776 , 150 );
            this.DisplayGridCtrl.TabIndex=1;
            this.DisplayGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DisplayGridView} );
            // 
            // DisplayGridView
            // 
            this.DisplayGridView.ABCGridControl=null;
            this.DisplayGridView.ActiveFilterEnabled=false;
            this.DisplayGridView.Bands.AddRange( new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1} );
            this.DisplayGridView.GridControl=this.DisplayGridCtrl;
            this.DisplayGridView.Name="DisplayGridView";
            this.DisplayGridView.OptionsCustomization.AllowChangeBandParent=true;
            this.DisplayGridView.OptionsCustomization.AllowChangeColumnParent=true;
            this.DisplayGridView.OptionsView.ShowAutoFilterRow=true;
            this.DisplayGridView.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.DisplayGridView.OptionsView.ShowGroupPanel=false;
            this.DisplayGridView.TableName=null;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption="gridBand1";
            this.gridBand1.Name="gridBand1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 406 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 776 , 35 );
            this.panelControl1.TabIndex=3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 696 , 5 );
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
            this.btnSave.Location=new System.Drawing.Point( 613 , 5 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // gridColTableName
            // 
            this.gridColTableName.Caption="TableName";
            this.gridColTableName.FieldName="TableName";
            this.gridColTableName.Name="gridColTableName";
            this.gridColTableName.Visible=true;
            this.gridColTableName.VisibleIndex=5;
            // 
            // GridBandedColumnConfigForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 776 , 441 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="GridBandedColumnConfigForm";
            this.Text="GridBandedColumnConfigForm";
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).EndInit();
            this.splitContainerControl2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ColumnConfigGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFilterStringEditor ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoAllowEdit ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.BandConfigGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.BandConfigGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private ABCBaseGridControl ColumnConfigGridCtrl;
        private ABCGridView ColumnConfigGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFieldName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCaption;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCustomizeCaption;
        private DevExpress.XtraGrid.Columns.GridColumn gridColUseAlias;

        private DevExpress.XtraGrid.Columns.GridColumn gridColFixed;
     //   private DevExpress.XtraGrid.Columns.GridColumn gridColRepoType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColGroupBy;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSumType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFilterString;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAllowEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationParentField;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationParentControl;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationChildField;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFieldNameChooser;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFilterStringEditor;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repoAllowEdit;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private ABCBaseGridControl BandConfigGridCtrl;
        private ABCGridView BandConfigGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColBandName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColBandFixed;
        private ABCBaseBandedGridControl DisplayGridCtrl;
        private ABCGridBandedView DisplayGridView;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTableName;
    }
}