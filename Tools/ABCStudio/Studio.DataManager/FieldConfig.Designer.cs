namespace ABCStudio
{
    partial class FieldConfigScreen
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
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.GridTableList=new DevExpress.XtraGrid.GridControl();
            this.ViewTableList=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GridFieldConfig=new DevExpress.XtraGrid.GridControl();
            this.ViewFieldConfig=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn7=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1=new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemButtonEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.GridTableList ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewTableList ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.GridFieldConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewFieldConfig ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemComboBox1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.GridTableList );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.GridFieldConfig );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 717 , 341 );
            this.splitContainerControl1.SplitterPosition=189;
            this.splitContainerControl1.TabIndex=0;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // GridTableList
            // 
            this.GridTableList.Dock=System.Windows.Forms.DockStyle.Fill;
            this.GridTableList.Location=new System.Drawing.Point( 0 , 0 );
            this.GridTableList.MainView=this.ViewTableList;
            this.GridTableList.Name="GridTableList";
            this.GridTableList.Size=new System.Drawing.Size( 189 , 341 );
            this.GridTableList.TabIndex=0;
            this.GridTableList.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewTableList} );
            // 
            // ViewTableList
            // 
            this.ViewTableList.GridControl=this.GridTableList;
            this.ViewTableList.Name="ViewTableList";
            this.ViewTableList.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.False;
            this.ViewTableList.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.False;
            this.ViewTableList.OptionsBehavior.Editable=false;
            this.ViewTableList.OptionsView.EnableAppearanceEvenRow=true;
            this.ViewTableList.OptionsView.EnableAppearanceOddRow=true;
            this.ViewTableList.OptionsView.ShowAutoFilterRow=true;
            this.ViewTableList.OptionsView.ShowColumnHeaders=false;
            this.ViewTableList.OptionsView.ShowGroupPanel=false;
            this.ViewTableList.OptionsView.ShowViewCaption=true;
            this.ViewTableList.ViewCaption="Choose TableName";
            // 
            // GridFieldConfig
            // 
            this.GridFieldConfig.Dock=System.Windows.Forms.DockStyle.Fill;
            this.GridFieldConfig.Location=new System.Drawing.Point( 0 , 0 );
            this.GridFieldConfig.MainView=this.ViewFieldConfig;
            this.GridFieldConfig.Name="GridFieldConfig";
            this.GridFieldConfig.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemComboBox1,
            this.repositoryItemButtonEdit1} );
            this.GridFieldConfig.Size=new System.Drawing.Size( 523 , 341 );
            this.GridFieldConfig.TabIndex=0;
            this.GridFieldConfig.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewFieldConfig} );
            // 
            // ViewFieldConfig
            // 
            this.ViewFieldConfig.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8} );
            this.ViewFieldConfig.GridControl=this.GridFieldConfig;
            this.ViewFieldConfig.Name="ViewFieldConfig";
            this.ViewFieldConfig.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.False;
            this.ViewFieldConfig.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.False;
            this.ViewFieldConfig.OptionsView.EnableAppearanceEvenRow=true;
            this.ViewFieldConfig.OptionsView.EnableAppearanceOddRow=true;
            this.ViewFieldConfig.OptionsView.ShowAutoFilterRow=true;
            this.ViewFieldConfig.OptionsView.ShowGroupPanel=false;
            this.ViewFieldConfig.OptionsView.ShowViewCaption=true;
            this.ViewFieldConfig.ViewCaption="Field Configuration";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption="Field Name";
            this.gridColumn1.FieldName="FieldName";
            this.gridColumn1.Name="gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit=false;
            this.gridColumn1.OptionsColumn.AllowFocus=false;
            this.gridColumn1.Visible=true;
            this.gridColumn1.VisibleIndex=0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption="Caption (VN)";
            this.gridColumn2.FieldName="CaptionVN";
            this.gridColumn2.Name="gridColumn2";
            this.gridColumn2.Visible=true;
            this.gridColumn2.VisibleIndex=1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption="Description (VN)";
            this.gridColumn3.FieldName="DescriptionVN";
            this.gridColumn3.Name="gridColumn3";
            this.gridColumn3.Visible=true;
            this.gridColumn3.VisibleIndex=2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption="Caption (EN)";
            this.gridColumn4.FieldName="CaptionEN";
            this.gridColumn4.Name="gridColumn4";
            this.gridColumn4.Visible=true;
            this.gridColumn4.VisibleIndex=3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption="Description (EN)";
            this.gridColumn5.FieldName="DescriptionEN";
            this.gridColumn5.Name="gridColumn5";
            this.gridColumn5.Visible=true;
            this.gridColumn5.VisibleIndex=4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption="Default";
            this.gridColumn6.ColumnEdit=this.repositoryItemCheckEdit1;
            this.gridColumn6.FieldName="IsDefault";
            this.gridColumn6.Name="gridColumn6";
            this.gridColumn6.Visible=true;
            this.gridColumn6.VisibleIndex=5;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight=false;
            this.repositoryItemCheckEdit1.Name="repositoryItemCheckEdit1";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption="Enum";
            this.gridColumn7.FieldName="AssignedEnum";
            this.gridColumn7.Name="gridColumn7";
            this.gridColumn7.Visible=true;
            this.gridColumn7.VisibleIndex=6;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption="FilterString";
            this.gridColumn8.FieldName="FilterString";
            this.gridColumn8.Name="gridColumn8";
            this.gridColumn8.Visible=true;
            this.gridColumn8.VisibleIndex=7;
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
            // FieldConfigScreen
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainerControl1 );
            this.Name="FieldConfigScreen";
            this.Size=new System.Drawing.Size( 717 , 341 );
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.GridTableList ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewTableList ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.GridFieldConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ViewFieldConfig ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemComboBox1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemButtonEdit1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl GridTableList;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewTableList;
        private DevExpress.XtraGrid.GridControl GridFieldConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewFieldConfig;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
    }
}