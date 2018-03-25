namespace ABCStudio
{
    partial class TableDesignScreen
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( TableDesignScreen ) );
            this.repositoryItemCheckEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1=new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.GridTableConfig=new DevExpress.XtraGrid.GridControl();
            this.ViewTableConfig=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17=new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.GridFieldConfig=new DevExpress.XtraGrid.GridControl();
            this.ViewFieldConfig=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn7=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit2=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColFormat=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1=new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemButtonEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColSortOrder=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.GridTableConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewTableConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.GridFieldConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewFieldConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit2 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemComboBox1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight=false;
            this.repositoryItemCheckEdit1.Name="repositoryItemCheckEdit1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.simpleButton1 );
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 390 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 1019 , 33 );
            this.panelControl1.TabIndex=2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.simpleButton1.Image=( (System.Drawing.Image)( resources.GetObject( "simpleButton1.Image" ) ) );
            this.simpleButton1.Location=new System.Drawing.Point( 724 , 3 );
            this.simpleButton1.Name="simpleButton1";
            this.simpleButton1.Size=new System.Drawing.Size( 135 , 26 );
            this.simpleButton1.TabIndex=4;
            this.simpleButton1.Text="&Get From Dictionary";
            this.simpleButton1.Click+=new System.EventHandler( this.simpleButton1_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 939 , 3 );
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
            this.btnSave.Location=new System.Drawing.Point( 861 , 3 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // GridTableConfig
            // 
            this.GridTableConfig.Dock=System.Windows.Forms.DockStyle.Fill;
            this.GridTableConfig.Location=new System.Drawing.Point( 0 , 0 );
            this.GridTableConfig.MainView=this.ViewTableConfig;
            this.GridTableConfig.Name="GridTableConfig";
            this.GridTableConfig.Size=new System.Drawing.Size( 397 , 390 );
            this.GridTableConfig.TabIndex=3;
            this.GridTableConfig.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewTableConfig} );
            // 
            // ViewTableConfig
            // 
            this.ViewTableConfig.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn17} );
            this.ViewTableConfig.GridControl=this.GridTableConfig;
            this.ViewTableConfig.Name="ViewTableConfig";
            this.ViewTableConfig.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.True;
            this.ViewTableConfig.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.False;
            this.ViewTableConfig.OptionsView.EnableAppearanceEvenRow=true;
            this.ViewTableConfig.OptionsView.EnableAppearanceOddRow=true;
            this.ViewTableConfig.OptionsView.ShowAutoFilterRow=true;
            this.ViewTableConfig.OptionsView.ShowGroupPanel=false;
            this.ViewTableConfig.OptionsView.ShowViewCaption=true;
            this.ViewTableConfig.ViewCaption="Tables";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption="TableName";
            this.gridColumn1.FieldName="TableName";
            this.gridColumn1.Name="gridColumn1";
            this.gridColumn1.Visible=true;
            this.gridColumn1.VisibleIndex=0;
            this.gridColumn1.Width=175;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption="Caption (VN)";
            this.gridColumn2.FieldName="CaptionVN";
            this.gridColumn2.Name="gridColumn2";
            this.gridColumn2.Visible=true;
            this.gridColumn2.VisibleIndex=1;
            this.gridColumn2.Width=131;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption="Description (VN)";
            this.gridColumn3.FieldName="DescriptionVN";
            this.gridColumn3.Name="gridColumn3";
            this.gridColumn3.Width=131;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption="Caption (EN)";
            this.gridColumn4.FieldName="CaptionEN";
            this.gridColumn4.Name="gridColumn4";
            this.gridColumn4.Visible=true;
            this.gridColumn4.VisibleIndex=2;
            this.gridColumn4.Width=131;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption="Description (EN)";
            this.gridColumn5.FieldName="DescriptionEN";
            this.gridColumn5.Name="gridColumn5";
            this.gridColumn5.Width=132;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption="Caching";
            this.gridColumn6.ColumnEdit=this.repositoryItemCheckEdit1;
            this.gridColumn6.FieldName="IsCaching";
            this.gridColumn6.Name="gridColumn6";
            this.gridColumn6.Visible=true;
            this.gridColumn6.VisibleIndex=3;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption="PrefixNo";
            this.gridColumn17.FieldName="PrefixNo";
            this.gridColumn17.Name="gridColumn17";
            this.gridColumn17.Visible=true;
            this.gridColumn17.VisibleIndex=2;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.GridTableConfig );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.GridFieldConfig );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 1019 , 390 );
            this.splitContainerControl1.SplitterPosition=397;
            this.splitContainerControl1.TabIndex=4;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // GridFieldConfig
            // 
            this.GridFieldConfig.Dock=System.Windows.Forms.DockStyle.Fill;
            this.GridFieldConfig.Location=new System.Drawing.Point( 0 , 0 );
            this.GridFieldConfig.MainView=this.ViewFieldConfig;
            this.GridFieldConfig.Name="GridFieldConfig";
            this.GridFieldConfig.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2,
            this.repositoryItemComboBox1,
            this.repositoryItemButtonEdit1} );
            this.GridFieldConfig.Size=new System.Drawing.Size( 617 , 390 );
            this.GridFieldConfig.TabIndex=0;
            this.GridFieldConfig.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewFieldConfig} );
            // 
            // ViewFieldConfig
            // 
            this.ViewFieldConfig.AppearancePrint.Row.BorderColor=System.Drawing.Color.Black;
            this.ViewFieldConfig.AppearancePrint.Row.Options.UseBorderColor=true;
            this.ViewFieldConfig.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn15, 
            this.gridColumn16,
            this.gridColumn18,
            this.gridColFormat,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColSortOrder} );
            this.ViewFieldConfig.GridControl=this.GridFieldConfig;
            this.ViewFieldConfig.Name="ViewFieldConfig";
            this.ViewFieldConfig.OptionsView.EnableAppearanceEvenRow=true;
            this.ViewFieldConfig.OptionsView.EnableAppearanceOddRow=true;
            this.ViewFieldConfig.OptionsView.ShowAutoFilterRow=true;
            this.ViewFieldConfig.OptionsView.ShowGroupPanel=false;
            this.ViewFieldConfig.OptionsView.ShowViewCaption=true;
            this.ViewFieldConfig.ViewCaption="Field Configuration";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption="Field Name";
            this.gridColumn7.FieldName="FieldName";
            this.gridColumn7.Name="gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit=false;
            this.gridColumn7.OptionsColumn.AllowFocus=false;
            this.gridColumn7.Visible=true;
            this.gridColumn7.VisibleIndex=0;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption="Caption (VN)";
            this.gridColumn8.FieldName="CaptionVN";
            this.gridColumn8.Name="gridColumn8";
            this.gridColumn8.Visible=true;
            this.gridColumn8.VisibleIndex=1;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption="Description (VN)";
            this.gridColumn9.FieldName="DescriptionVN";
            this.gridColumn9.Name="gridColumn9";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption="Caption (EN)";
            this.gridColumn10.FieldName="CaptionEN";
            this.gridColumn10.Name="gridColumn10";
            this.gridColumn10.Visible=true;
            this.gridColumn10.VisibleIndex=2;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption="Description (EN)";
            this.gridColumn11.FieldName="DescriptionEN";
            this.gridColumn11.Name="gridColumn11";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption="Default";
            this.gridColumn12.ColumnEdit=this.repositoryItemCheckEdit2;
            this.gridColumn12.FieldName="IsDefault";
            this.gridColumn12.Name="gridColumn12";
            this.gridColumn12.Visible=true;
            this.gridColumn12.VisibleIndex=3;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption="DisplayField";
            this.gridColumn15.ColumnEdit=this.repositoryItemCheckEdit2;
            this.gridColumn15.FieldName="IsDisplayField";
            this.gridColumn15.Name="gridColumn15";
            this.gridColumn15.Visible=true;
            this.gridColumn15.VisibleIndex=4;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption="DefaultGrouping";
            this.gridColumn16.ColumnEdit=this.repositoryItemCheckEdit2;
            this.gridColumn16.FieldName="IsGrouping";
            this.gridColumn16.Name="gridColumn16";
            this.gridColumn15.Visible=true;
            this.gridColumn16.VisibleIndex=5;   
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption="Use";
            this.gridColumn18.ColumnEdit=this.repositoryItemCheckEdit2;
            this.gridColumn18.FieldName="InUse";
            this.gridColumn18.Name="gridColumn18";
            this.gridColumn18.Visible=true;
            this.gridColumn18.VisibleIndex=6;

            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight=false;
            this.repositoryItemCheckEdit2.Name="repositoryItemCheckEdit2";
            // 
            // gridColFormat
            // 
            this.gridColFormat.Caption="Format";
            this.gridColFormat.FieldName="Format";
            this.gridColFormat.Name="gridColFormat";
            this.gridColFormat.Visible=true;
            this.gridColFormat.VisibleIndex=5;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption="Enum";
            this.gridColumn13.FieldName="AssignedEnum";
            this.gridColumn13.Name="gridColumn13";
            this.gridColumn13.Visible=true;
            this.gridColumn13.VisibleIndex=6;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption="FilterString";
            this.gridColumn14.FieldName="FilterString";
            this.gridColumn14.Name="gridColumn14";
            this.gridColumn14.Visible=true;
            this.gridColumn14.VisibleIndex=7;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight=false;
            this.repositoryItemComboBox1.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.repositoryItemComboBox1.Name="repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle=DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight=false;
            this.repositoryItemButtonEdit1.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.repositoryItemButtonEdit1.Name="repositoryItemButtonEdit1";
            // 
            // gridColSortOrder
            // 
            this.gridColSortOrder.Caption="SortOrder";
            this.gridColSortOrder.FieldName="SortOrder";
            this.gridColSortOrder.Name="gridColSortOrder";
            this.gridColSortOrder.Visible=true;
            this.gridColSortOrder.VisibleIndex=4;
            // 
            // TableDesignScreen
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="TableDesignScreen";
            this.Size=new System.Drawing.Size( 1019 , 423 );
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.GridTableConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewTableConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.GridFieldConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewFieldConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit2 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemComboBox1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraGrid.GridControl GridTableConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewTableConfig;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl GridFieldConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewFieldConfig;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoEnum;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoFormat;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repoFilter;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFormat;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSortOrder;
    }
}