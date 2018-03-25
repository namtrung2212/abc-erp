namespace ABCApp
{
    partial class NotifyList
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
            this.gridViewNotifies=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTitle=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTime=new DevExpress.XtraGrid.Columns.GridColumn();
            this.colViewed=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.barManager1=new DevExpress.XtraBars.BarManager( this.components );
            this.bar2=new DevExpress.XtraBars.Bar();
            this.chkShowNewMailOnly=new DevExpress.XtraBars.BarCheckItem();
            this.chkSound=new DevExpress.XtraBars.BarCheckItem();
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            ( (System.ComponentModel.ISupportInitialize)( this.gridViewNotifies ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // gridViewNotifies
            // 
            this.gridViewNotifies.Appearance.HorzLine.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 224 ) ) ) ) , ( (int)( ( (byte)( 224 ) ) ) ) , ( (int)( ( (byte)( 224 ) ) ) ) );
            this.gridViewNotifies.Appearance.HorzLine.Options.UseBackColor=true;
            this.gridViewNotifies.Appearance.VertLine.BackColor=System.Drawing.Color.Transparent;
            this.gridViewNotifies.Appearance.VertLine.Options.UseBackColor=true;
            this.gridViewNotifies.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTitle,
            this.colTime,
            this.colViewed} );
            this.gridViewNotifies.GridControl=this.gridControl1;
            this.gridViewNotifies.Name="gridViewNotifies";
            this.gridViewNotifies.OptionsBehavior.Editable=false;
            this.gridViewNotifies.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridViewNotifies.OptionsSelection.EnableAppearanceFocusedRow=false;
            this.gridViewNotifies.OptionsView.ShowColumnHeaders=false;
            this.gridViewNotifies.OptionsView.ShowFilterPanelMode=DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewNotifies.OptionsView.ShowGroupPanel=false;
            this.gridViewNotifies.OptionsView.ShowIndicator=false;
            // 
            // colTitle
            // 
            this.colTitle.Caption="Tiêu đề";
            this.colTitle.FieldName="NotifyTitle";
            this.colTitle.Name="colTitle";
            this.colTitle.Visible=true;
            this.colTitle.VisibleIndex=0;
            this.colTitle.Width=267;
            // 
            // colTime
            // 
            this.colTime.Caption="Thời gian";
            this.colTime.FieldName="LastTime";
            this.colTime.MaxWidth=45;
            this.colTime.MinWidth=45;
            this.colTime.Name="colTime";
            this.colTime.Visible=true;
            this.colTime.VisibleIndex=1;
            this.colTime.Width=45;
            // 
            // colViewed
            // 
            this.colViewed.Caption="Đã xem";
            this.colViewed.FieldName="Viewed";
            this.colViewed.Name="colViewed";
            this.colViewed.Width=20;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 22 );
            this.gridControl1.MainView=this.gridViewNotifies;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 339 , 247 );
            this.gridControl1.TabIndex=0;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewNotifies} );
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
            this.chkShowNewMailOnly,
            this.chkSound} );
            this.barManager1.MainMenu=this.bar2;
            this.barManager1.MaxItemId=4;
            // 
            // bar2
            // 
            this.bar2.BarName="Main menu";
            this.bar2.DockCol=0;
            this.bar2.DockRow=0;
            this.bar2.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.chkShowNewMailOnly),
            new DevExpress.XtraBars.LinkPersistInfo(this.chkSound)} );
            this.bar2.OptionsBar.AllowQuickCustomization=false;
            this.bar2.OptionsBar.DrawDragBorder=false;
            this.bar2.OptionsBar.MultiLine=true;
            this.bar2.OptionsBar.UseWholeRow=true;
            this.bar2.Text="Main menu";
            // 
            // chkShowUnreadOnly
            // 
            this.chkShowNewMailOnly.Id=1;
            this.chkShowNewMailOnly.Name="chkShowUnreadOnly";
            this.chkShowNewMailOnly.CheckedChanged+=new DevExpress.XtraBars.ItemClickEventHandler( this.chkShowUnreadOnly_CheckedChanged );
            // 
            // chkSound
            // 
            this.chkSound.Id=3;
            this.chkSound.Name="chkSound";
            this.chkSound.CheckedChanged+=new DevExpress.XtraBars.ItemClickEventHandler( this.chkSoundOnOff_CheckedChanged );
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation=false;
            this.barDockControlTop.Dock=System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlTop.Size=new System.Drawing.Size( 339 , 22 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 269 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 339 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 22 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 247 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 339 , 22 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 247 );
            // 
            // NotifyList
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name="NotifyList";
            this.Size=new System.Drawing.Size( 339 , 269 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridViewNotifies ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraGrid.Views.Grid.GridView gridViewNotifies;
        private DevExpress.XtraGrid.Columns.GridColumn colTitle;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colViewed;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarCheckItem chkShowNewMailOnly;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarCheckItem chkSound;

    }
}
