namespace ABCControls
{
    partial class UserChooserForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( UserChooserForm ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new ABCControls.ABCSimpleButton();
            this.btnNext=new ABCControls.ABCSimpleButton();
            this.chkAll=new DevExpress.XtraEditors.CheckEdit();
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSelect=new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colUser=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmployee=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCompanyUnit=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.chkAll.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnNext );
            this.panelControl1.Controls.Add( this.chkAll );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 304 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 385 , 30 );
            this.panelControl1.TabIndex=1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.ButtonType=ABCControls.ABCSimpleButton.ABCButtonType.None;
            this.btnCancel.Comment=null;
            this.btnCancel.FieldGroup=null;
            this.btnCancel.IconType=ABCControls.ABCSimpleButton.ABCIconType.Cancel;
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.ImageIndex=0;
            this.btnCancel.IsVisible=true;
            this.btnCancel.Location=new System.Drawing.Point( 308 , 2 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.OwnerView=null;
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=4;
            this.btnCancel.Text="&Hủy";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnNext
            // 
            this.btnNext.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnNext.ButtonType=ABCControls.ABCSimpleButton.ABCButtonType.None;
            this.btnNext.Comment=null;
            this.btnNext.FieldGroup=null;
            this.btnNext.IconType=ABCControls.ABCSimpleButton.ABCIconType.Approve;
            this.btnNext.Image=( (System.Drawing.Image)( resources.GetObject( "btnNext.Image" ) ) );
            this.btnNext.ImageIndex=0;
            this.btnNext.IsVisible=true;
            this.btnNext.Location=new System.Drawing.Point( 230 , 2 );
            this.btnNext.Name="btnNext";
            this.btnNext.OwnerView=null;
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=3;
            this.btnNext.Text="&Nhận";
            this.btnNext.Click+=new System.EventHandler( this.btnNext_Click );
            // 
            // chkAll
            // 
            this.chkAll.Location=new System.Drawing.Point( 5 , 6 );
            this.chkAll.Name="chkAll";
            this.chkAll.Properties.Caption="Chọn tất cả";
            this.chkAll.Size=new System.Drawing.Size( 88 , 19 );
            this.chkAll.TabIndex=2;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1} );
            this.gridControl1.Size=new System.Drawing.Size( 385 , 304 );
            this.gridControl1.TabIndex=2;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Empty.BackColor=System.Drawing.SystemColors.ActiveCaption;
            this.gridView1.Appearance.Empty.Options.UseBackColor=true;
            this.gridView1.Appearance.HeaderPanel.BackColor=System.Drawing.SystemColors.ActiveCaption;
            this.gridView1.Appearance.HeaderPanel.Font=new System.Drawing.Font( "Tahoma" , 8.25F , System.Drawing.FontStyle.Bold );
            this.gridView1.Appearance.HeaderPanel.Options.UseBackColor=true;
            this.gridView1.Appearance.HeaderPanel.Options.UseFont=true;
            this.gridView1.Appearance.Row.BackColor=System.Drawing.SystemColors.ActiveCaption;
            this.gridView1.Appearance.Row.Options.UseBackColor=true;
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSelect,
            this.colUser,
            this.colEmployee,
            this.colCompanyUnit} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsView.ShowAutoFilterRow=true;
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowIndicator=false;
            // 
            // colSelect
            // 
            this.colSelect.Caption=" ";
            this.colSelect.ColumnEdit=this.repositoryItemCheckEdit1;
            this.colSelect.FieldName="Select";
            this.colSelect.MaxWidth=40;
            this.colSelect.Name="colSelect";
            this.colSelect.Visible=true;
            this.colSelect.VisibleIndex=0;
            this.colSelect.Width=20;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight=false;
            this.repositoryItemCheckEdit1.Name="repositoryItemCheckEdit1";
            // 
            // colUser
            // 
            this.colUser.Caption="Người dùng";
            this.colUser.FieldName="User";
            this.colUser.Name="colUser";
            this.colUser.OptionsColumn.AllowEdit=false;
            // 
            // colEmployee
            // 
            this.colEmployee.Caption="Nhân viên";
            this.colEmployee.FieldName="Employee";
            this.colEmployee.Name="colEmployee";
            this.colEmployee.OptionsColumn.AllowEdit=false;
            this.colEmployee.Visible=true;
            this.colEmployee.VisibleIndex=1;
            // 
            // colCompanyUnit
            // 
            this.colCompanyUnit.Caption="Phòng ban";
            this.colCompanyUnit.FieldName="CompanyUnit";
            this.colCompanyUnit.Name="colCompanyUnit";
            this.colCompanyUnit.Visible=true;
            this.colCompanyUnit.VisibleIndex=2;
            // 
            // UserChooserForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 385 , 334 );
            this.Controls.Add( this.gridControl1 );
            this.Controls.Add( this.panelControl1 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="UserChooserForm";
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text="Chọn nhân viên";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.chkAll.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemCheckEdit1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit chkAll;
        private ABCSimpleButton btnCancel;
        private ABCSimpleButton btnNext;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colSelect;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colUser;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn colCompanyUnit;
    }
}