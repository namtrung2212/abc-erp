namespace ABCApp
{
    partial class UserOnlineList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.components=new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( UserOnlineList ) );
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colUser=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmployee=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCompanyUnit=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsOnline=new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1=new DevExpress.XtraBars.BarManager( this.components );
            this.bar2=new DevExpress.XtraBars.Bar();
            this.btnShowOnlineOnly=new DevExpress.XtraBars.BarCheckItem();
            this.btnSoundOn=new DevExpress.XtraBars.BarCheckItem();
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 24 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 263 , 180 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Appearance.GroupButton.BackColor=System.Drawing.Color.White;
            this.gridView1.Appearance.GroupButton.Options.UseBackColor=true;
            this.gridView1.Appearance.GroupRow.BackColor=System.Drawing.Color.White;
            this.gridView1.Appearance.GroupRow.Options.UseBackColor=true;
            this.gridView1.Appearance.HorzLine.BackColor=System.Drawing.Color.Transparent;
            this.gridView1.Appearance.HorzLine.Options.UseBackColor=true;
            this.gridView1.Appearance.VertLine.BackColor=System.Drawing.Color.Transparent;
            this.gridView1.Appearance.VertLine.Options.UseBackColor=true;
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUser,
            this.colEmployee,
            this.colCompanyUnit,
            this.colIsOnline} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.GroupCount=1;
            this.gridView1.GroupFormat="[#image]{1} {2}";
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsBehavior.Editable=false;
            this.gridView1.OptionsView.ShowColumnHeaders=false;
            this.gridView1.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowIndicator=false;
            this.gridView1.SortInfo.AddRange( new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCompanyUnit, DevExpress.Data.ColumnSortOrder.Ascending)} );
            // 
            // colUser
            // 
            this.colUser.FieldName="User";
            this.colUser.Name="colUser";
            // 
            // colEmployee
            // 
            this.colEmployee.FieldName="Employee";
            this.colEmployee.Name="colEmployee";
            this.colEmployee.Visible=true;
            this.colEmployee.VisibleIndex=0;
            // 
            // colCompanyUnit
            // 
            this.colCompanyUnit.FieldName="CompanyUnit";
            this.colCompanyUnit.Name="colCompanyUnit";
            // 
            // colIsOnline
            // 
            this.colIsOnline.FieldName="IsOnline";
            this.colIsOnline.Name="colIsOnline";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange( new DevExpress.XtraBars.Bar[] {
            this.bar2} );
            this.barManager1.DockControls.Add( this.barDockControlTop );
            this.barManager1.DockControls.Add( this.barDockControlBottom );
            this.barManager1.DockControls.Add( this.barDockControlLeft );
            this.barManager1.DockControls.Add( this.barDockControlRight );
            this.barManager1.Form=this;
            this.barManager1.Items.AddRange( new DevExpress.XtraBars.BarItem[] {
            this.btnShowOnlineOnly,
            this.btnSoundOn} );
            this.barManager1.MainMenu=this.bar2;
            this.barManager1.MaxItemId=5;
            // 
            // bar2
            // 
            this.bar2.BarName="Main menu";
            this.bar2.DockCol=0;
            this.bar2.DockRow=0;
            this.bar2.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnShowOnlineOnly),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSoundOn)} );
            this.bar2.OptionsBar.AllowQuickCustomization=false;
            this.bar2.OptionsBar.DrawDragBorder=false;
            this.bar2.OptionsBar.MultiLine=true;
            this.bar2.OptionsBar.UseWholeRow=true;
            this.bar2.Text="Main menu";
            // 
            // btnShowOnlineOnly
            // 
            this.btnShowOnlineOnly.Glyph=( (System.Drawing.Image)( resources.GetObject( "btnShowOnlineOnly.Glyph" ) ) );
            this.btnShowOnlineOnly.Hint="Hiển thị/không hiển thị nhân viên offline";
            this.btnShowOnlineOnly.Id=2;
            this.btnShowOnlineOnly.Name="btnShowOnlineOnly";
            this.btnShowOnlineOnly.CheckedChanged+=new DevExpress.XtraBars.ItemClickEventHandler( this.btnShowOnlineOnly_CheckedChanged );
            // 
            // btnSoundOn
            // 
            this.btnSoundOn.Hint="Bật/tắt âm tin nhắn";
            this.btnSoundOn.Id=4;
            this.btnSoundOn.Name="btnSoundOn";
            this.btnSoundOn.CheckedChanged+=new DevExpress.XtraBars.ItemClickEventHandler( this.btnSoundOn_CheckedChanged );
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation=false;
            this.barDockControlTop.Dock=System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlTop.Size=new System.Drawing.Size( 263 , 24 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 204 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 263 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 24 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 180 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 263 , 24 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 180 );
            // 
            // UserOnlineList
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name="UserOnlineList";
            this.Size=new System.Drawing.Size( 263 , 204 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colUser;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn colCompanyUnit;
        private DevExpress.XtraGrid.Columns.GridColumn colIsOnline;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarCheckItem btnShowOnlineOnly;
        private DevExpress.XtraBars.BarCheckItem btnSoundOn;
    }
}
