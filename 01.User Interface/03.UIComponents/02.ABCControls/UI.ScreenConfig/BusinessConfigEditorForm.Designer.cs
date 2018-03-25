namespace ABCControls
{
    partial class ABCBusinessConfigEditorForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( ABCBusinessConfigEditorForm ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1=new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.treeBinding=new DevExpress.XtraTreeList.TreeList();
            this.PropertyGrid=new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.barManager1=new DevExpress.XtraBars.BarManager();
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl2=new DevExpress.XtraEditors.SplitContainerControl();
            this.FieldFilterGrid=new DevExpress.XtraGrid.GridControl();
            this.FieldFilterGridView=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.treeBinding ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.PropertyGrid ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.FieldFilterGrid ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.FieldFilterGridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.simpleButton1 );
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 344 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 594 , 38 );
            this.panelControl1.TabIndex=1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Left ) ) );
            this.simpleButton1.Image=( (System.Drawing.Image)( resources.GetObject( "simpleButton1.Image" ) ) );
            this.simpleButton1.Location=new System.Drawing.Point( 5 , 6 );
            this.simpleButton1.Name="simpleButton1";
            this.simpleButton1.Size=new System.Drawing.Size( 150 , 26 );
            this.simpleButton1.TabIndex=2;
            this.simpleButton1.Text="&Get BusinessObjects";
            this.simpleButton1.Click+=new System.EventHandler( this.AddRoot_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 512 , 6 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=1;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnSave
            // 
            this.btnSave.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnSave.Image=( (System.Drawing.Image)( resources.GetObject( "btnSave.Image" ) ) );
            this.btnSave.Location=new System.Drawing.Point( 429 , 6 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=0;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.treeBinding );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.splitContainerControl2 );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 594 , 344 );
            this.splitContainerControl1.SplitterPosition=255;
            this.splitContainerControl1.TabIndex=2;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // treeBinding
            // 
            this.treeBinding.Dock=System.Windows.Forms.DockStyle.Fill;
            this.treeBinding.Location=new System.Drawing.Point( 0 , 0 );
            this.treeBinding.Name="treeBinding";
            this.treeBinding.Size=new System.Drawing.Size( 334 , 344 );
            this.treeBinding.TabIndex=0;
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.Dock=System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.Location=new System.Drawing.Point( 0 , 0 );
            this.PropertyGrid.Name="PropertyGrid";
            this.PropertyGrid.Size=new System.Drawing.Size( 255 , 207 );
            this.PropertyGrid.TabIndex=0;
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
            this.barDockControlTop.Size=new System.Drawing.Size( 594 , 0 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 382 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 594 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 382 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 594 , 0 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 382 );
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal=false;
            this.splitContainerControl2.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl2.Name="splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add( this.PropertyGrid );
            this.splitContainerControl2.Panel1.Text="Panel1";
            this.splitContainerControl2.Panel2.Controls.Add( this.FieldFilterGrid );
            this.splitContainerControl2.Panel2.Text="Panel2";
            this.splitContainerControl2.Size=new System.Drawing.Size( 255 , 344 );
            this.splitContainerControl2.SplitterPosition=207;
            this.splitContainerControl2.TabIndex=1;
            this.splitContainerControl2.Text="splitContainerControl2";
            // 
            // FieldFilterGrid
            // 
            this.FieldFilterGrid.Dock=System.Windows.Forms.DockStyle.Fill;
            this.FieldFilterGrid.Location=new System.Drawing.Point( 0 , 0 );
            this.FieldFilterGrid.MainView=this.FieldFilterGridView;
            this.FieldFilterGrid.MenuManager=this.barManager1;
            this.FieldFilterGrid.Name="FieldFilterGrid";
            this.FieldFilterGrid.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1} );
            this.FieldFilterGrid.Size=new System.Drawing.Size( 255 , 132 );
            this.FieldFilterGrid.TabIndex=0;
            this.FieldFilterGrid.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.FieldFilterGridView} );
            // 
            // FieldFilterGridView
            // 
            this.FieldFilterGridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2} );
            this.FieldFilterGridView.GridControl=this.FieldFilterGrid;
            this.FieldFilterGridView.Name="FieldFilterGridView";
            this.FieldFilterGridView.OptionsView.ShowColumnHeaders=false;
            this.FieldFilterGridView.OptionsView.ShowGroupPanel=false;
            this.FieldFilterGridView.OptionsView.ShowIndicator=false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption="Field";
            this.gridColumn1.FieldName="Field";
            this.gridColumn1.Name="gridColumn1";
            this.gridColumn1.Visible=true;
            this.gridColumn1.VisibleIndex=0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption="FilterString";
            this.gridColumn2.ColumnEdit=this.repositoryItemButtonEdit1;
            this.gridColumn2.FieldName="FilterString";
            this.gridColumn2.Name="gridColumn2";
            this.gridColumn2.Visible=true;
            this.gridColumn2.VisibleIndex=1;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight=false;
            this.repositoryItemButtonEdit1.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repositoryItemButtonEdit1.Name="repositoryItemButtonEdit1";
            // 
            // DataConfigEditorForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 594 , 382 );
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name="DataConfigEditorForm";
            this.Text="Binding Configuration";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.treeBinding ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.PropertyGrid ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).EndInit();
            this.splitContainerControl2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.FieldFilterGrid ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.FieldFilterGridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTreeList.TreeList treeBinding;
        private DevExpress.XtraVerticalGrid.PropertyGridControl PropertyGrid;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraGrid.GridControl FieldFilterGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView FieldFilterGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
    }
}