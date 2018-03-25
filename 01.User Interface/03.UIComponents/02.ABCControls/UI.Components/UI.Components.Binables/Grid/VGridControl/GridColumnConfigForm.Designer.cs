namespace ABCPresentLib
{
    partial class GridColumnConfigForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( GridColumnConfigForm ) );
            this.DisplayGridCtrl=new ABCPresentLib.ABCBaseGridControl();
            this.DisplayGridView=new ABCPresentLib.ABCGridView();
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.ConfigGridCtrl=new ABCBaseGridControl();
            this.ConfigGridView=new ABCPresentLib.ABCGridView();
            this.gridColFieldName=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoFieldNameChooser=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repoFilterStringEditor=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColDisplay=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAllowEdit=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoAllowEdit=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColFixed=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRepoType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSumType=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGroupBy=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColFilterString=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationParentField=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationParentControl=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRelationChildField=new DevExpress.XtraGrid.Columns.GridColumn();

            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.gridColTableName=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridView ) ).BeginInit();
            
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFilterStringEditor ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoAllowEdit ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DisplayGridCtrl
            // 
            this.DisplayGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DisplayGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.DisplayGridCtrl.MainView=this.DisplayGridView;
            this.DisplayGridCtrl.Name="DisplayGridCtrl";
            this.DisplayGridCtrl.Size=new System.Drawing.Size( 776 , 149 );
            this.DisplayGridCtrl.TabIndex=1;
            this.DisplayGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DisplayGridView} );
            // 
            // DisplayGridView
            // 
            this.DisplayGridView.ABCGridControl=null;
            this.DisplayGridView.ActiveFilterEnabled=false;
            this.DisplayGridView.GridControl=this.DisplayGridCtrl;
            this.DisplayGridView.Name="DisplayGridView";
            this.DisplayGridView.OptionsView.ShowAutoFilterRow=true;
            this.DisplayGridView.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.DisplayGridView.OptionsView.ShowGroupPanel=false;
            this.DisplayGridView.TableName=null;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal=false;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.ConfigGridCtrl );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.DisplayGridCtrl );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 776 , 406 );
            this.splitContainerControl1.SplitterPosition=252;
            this.splitContainerControl1.TabIndex=2;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // ConfigGridCtrl
            // 
            this.ConfigGridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ConfigGridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.ConfigGridCtrl.MainView=this.ConfigGridView;
            this.ConfigGridCtrl.Name="ConfigGridCtrl";
            this.ConfigGridCtrl.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repoFieldNameChooser,
            this.repoAllowEdit,this.repoFilterStringEditor} );
            this.ConfigGridCtrl.Size=new System.Drawing.Size( 776 , 252 );
            this.ConfigGridCtrl.TabIndex=2;
            this.ConfigGridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ConfigGridView} );
            // 
            // ConfigGridView
            // 
            this.ConfigGridView.ABCGridControl=null;
            this.ConfigGridView.ActiveFilterEnabled=false;
            this.ConfigGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColTableName,
            this.gridColFieldName,
            this.gridColDisplay,
            this.gridColAllowEdit,
            this.gridColFixed,
            this.gridColRepoType,
            this.gridColSumType,
            this.gridColGroupBy,
            this.gridColFilterString,
            this.gridColRelationParentField,
            this.gridColRelationParentControl,
            this.gridColRelationChildField} );
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
            this.repoFilterStringEditor.Name="repoFilterStringEditor";
            // 
            // gridColDisplay
            // 
            this.gridColDisplay.Caption="Display";
            this.gridColDisplay.FieldName="Caption";
            this.gridColDisplay.Name="gridColDisplay";
            this.gridColDisplay.Visible=true;
            this.gridColDisplay.VisibleIndex=1;
            // 
            // gridColAllowEdit
            // 
            this.gridColAllowEdit.Caption="AllowEdit";
            this.gridColAllowEdit.ColumnEdit=this.repoAllowEdit;
            this.gridColAllowEdit.FieldName="AllowEdit";
            this.gridColAllowEdit.Name="gridColAllowEdit";
            this.gridColAllowEdit.Visible=true;
            this.gridColAllowEdit.VisibleIndex=5;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repoAllowEdit.AutoHeight=false;
            this.repoAllowEdit.Name="repositoryItemCheckEdit1";
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
            this.gridColRepoType.Caption="Repository";
            this.gridColRepoType.FieldName="RepoType";
            this.gridColRepoType.Name="gridColRepoType";
            this.gridColRepoType.Visible=true;
            this.gridColRepoType.VisibleIndex=3;
            // 
            // gridColSumType
            // 
            this.gridColSumType.Caption="Summary";
            this.gridColSumType.FieldName="SumType";
            this.gridColSumType.Name="gridColSumType";
            this.gridColSumType.Visible=true;
            this.gridColSumType.VisibleIndex=4;   // 
         
            // 
            // gridColGroupBy
            // 
            this.gridColGroupBy.Caption="GroupBy";
            this.gridColGroupBy.FieldName="GroupBy";
            this.gridColGroupBy.Name="gridColGroupBy";
            this.gridColGroupBy.Visible=true;
            this.gridColGroupBy.VisibleIndex=4;   // 


            // gridColFilterString
            // 
            this.gridColFilterString.Caption="FilterString";
            this.gridColFilterString.FieldName="FilterString";
            this.gridColFilterString.ColumnEdit=this.repoFilterStringEditor;
            this.gridColFilterString.Name="gridColFilterString";
            this.gridColFilterString.Visible=true;
            this.gridColFilterString.VisibleIndex=5;

             // 
            // gridColRelationParentCol
            // 
            this.gridColRelationParentField.Caption="Parent Column";
            this.gridColRelationParentField.FieldName="RelationParentField";
            this.gridColRelationParentField.Name="gridColRelationParentField";
            this.gridColRelationParentField.Visible=true;
            this.gridColRelationParentField.VisibleIndex=6;   //   // 
            // gridColRelationParentCol
            // 
            this.gridColRelationParentControl.Caption="Parent Control";
            this.gridColRelationParentControl.FieldName="RelationParentControl";
            this.gridColRelationParentControl.Name="gridColRelationParentControl";
            this.gridColRelationParentControl.Visible=true;
            this.gridColRelationParentControl.VisibleIndex=7;   // 

             // 
            // gridColRelationChildField
            // 
            this.gridColRelationChildField.Caption="Child Field";
            this.gridColRelationChildField.FieldName="RelationChildField";
            this.gridColRelationChildField.Name="gridColRelationChildField";
            this.gridColRelationChildField.Visible=true;
            this.gridColRelationChildField.VisibleIndex=8;   // 

             

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
            this.gridColTableName.VisibleIndex=6;
            // 
            // GridColumnConfigForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 776 , 441 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="GridColumnConfigForm";
            this.Text="GridColumnConfigForm";
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.DisplayGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ConfigGridView ) ).EndInit();
            
            ( (System.ComponentModel.ISupportInitialize)( this.repoFieldNameChooser ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoFilterStringEditor ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repoAllowEdit ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private ABCPresentLib.ABCBaseGridControl DisplayGridCtrl;
        private ABCGridView DisplayGridView;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private ABCBaseGridControl ConfigGridCtrl;
        private ABCGridView ConfigGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFieldName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFixed;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRepoType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSumType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColGroupBy;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationParentField;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationParentControl;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRelationChildField;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFilterString;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFieldNameChooser;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFilterStringEditor;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAllowEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repoAllowEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTableName;
    }
}