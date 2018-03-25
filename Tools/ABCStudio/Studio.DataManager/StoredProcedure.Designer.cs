namespace ABCStudio
{
    partial class StoredProConfigScreen
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( StoredProConfigScreen ) );
            this.splitContainerControl1=new DevExpress.XtraEditors.SplitContainerControl();
            this.GridCtrl=new DevExpress.XtraGrid.GridControl();
            this.GridView=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1=new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.chkShowDefaultSP=new DevExpress.XtraEditors.CheckEdit();
            this.barManager1=new DevExpress.XtraBars.BarManager( this.components );
            this.bar2=new DevExpress.XtraBars.Bar();
            this.barButtonItem1=new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4=new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2=new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3=new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemMarqueeProgressBar1=new DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar();
            this.barDockControlTop=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft=new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight=new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl2=new DevExpress.XtraEditors.SplitContainerControl();
            this.ScriptTabControl=new DevExpress.XtraTab.XtraTabControl();
            this.ResultRichText=new DevExpress.XtraEditors.MemoEdit();
            this.progressBarControl1=new DevExpress.XtraEditors.ProgressBarControl();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.GridCtrl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.GridView ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.chkShowDefaultSP.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemMarqueeProgressBar1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.ScriptTabControl ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ResultRichText.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.progressBarControl1.Properties ) ).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location=new System.Drawing.Point( 0 , 22 );
            this.splitContainerControl1.Name="splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add( this.GridCtrl );
            this.splitContainerControl1.Panel1.Controls.Add( this.panelControl1 );
            this.splitContainerControl1.Panel1.Text="Panel1";
            this.splitContainerControl1.Panel2.Controls.Add( this.splitContainerControl2 );
            this.splitContainerControl1.Panel2.Text="Panel2";
            this.splitContainerControl1.Size=new System.Drawing.Size( 928 , 516 );
            this.splitContainerControl1.SplitterPosition=255;
            this.splitContainerControl1.TabIndex=0;
            this.splitContainerControl1.Text="splitContainerControl1";
            // 
            // GridCtrl
            // 
            this.GridCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.GridCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.GridCtrl.MainView=this.GridView;
            this.GridCtrl.Name="GridCtrl";
            this.GridCtrl.Size=new System.Drawing.Size( 255 , 488 );
            this.GridCtrl.TabIndex=0;
            this.GridCtrl.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView} );
            // 
            // GridView
            // 
            this.GridView.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1} );
            this.GridView.GridControl=this.GridCtrl;
            this.GridView.GroupFormat="[#image]{1} {2}";
            this.GridView.Name="GridView";
            this.GridView.OptionsBehavior.Editable=false;
            this.GridView.OptionsView.ShowAutoFilterRow=true;
            this.GridView.OptionsView.ShowGroupPanel=false;
            this.GridView.OptionsView.ShowViewCaption=true;
            this.GridView.ViewCaption="Stored Procedures";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption="SPName";
            this.gridColumn1.FieldName="name";
            this.gridColumn1.Name="gridColumn1";
            this.gridColumn1.Visible=true;
            this.gridColumn1.VisibleIndex=0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.chkShowDefaultSP );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 488 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 255 , 28 );
            this.panelControl1.TabIndex=1;
            // 
            // chkShowDefaultSP
            // 
            this.chkShowDefaultSP.Location=new System.Drawing.Point( 8 , 4 );
            this.chkShowDefaultSP.MenuManager=this.barManager1;
            this.chkShowDefaultSP.Name="chkShowDefaultSP";
            this.chkShowDefaultSP.Properties.Caption="Show Default Stored Procedures";
            this.chkShowDefaultSP.Size=new System.Drawing.Size( 200 , 19 );
            this.chkShowDefaultSP.TabIndex=0;
            this.chkShowDefaultSP.CheckedChanged+=new System.EventHandler( this.chkShowDefaultSP_CheckedChanged );
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
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4} );
            this.barManager1.MainMenu=this.bar2;
            this.barManager1.MaxItemId=5;
            this.barManager1.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMarqueeProgressBar1} );
            // 
            // bar2
            // 
            this.bar2.BarName="Main menu";
            this.bar2.DockCol=0;
            this.bar2.DockRow=0;
            this.bar2.DockStyle=DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)} );
            this.bar2.OptionsBar.MultiLine=true;
            this.bar2.OptionsBar.UseWholeRow=true;
            this.bar2.Text="Main menu";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption="New Querry";
            this.barButtonItem1.Id=0;
            this.barButtonItem1.ImageIndex=0;
            this.barButtonItem1.Name="barButtonItem1";
            this.barButtonItem1.Tag="New";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption="Refresh List";
            this.barButtonItem4.Id=3;
            this.barButtonItem4.ImageIndex=48;
            this.barButtonItem4.Name="barButtonItem4";
            this.barButtonItem4.Tag="Refresh";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption="Execute";
            this.barButtonItem2.Id=1;
            this.barButtonItem2.ImageIndex=67;
            this.barButtonItem2.Name="barButtonItem2";
            this.barButtonItem2.Tag="Run";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption="Generate Stored Procedure";
            this.barButtonItem3.Id=2;
            this.barButtonItem3.ImageIndex=68;
            this.barButtonItem3.Name="barButtonItem3";
            this.barButtonItem3.Tag="GenSP";
            // 
            // repositoryItemMarqueeProgressBar1
            // 
            this.repositoryItemMarqueeProgressBar1.Name="repositoryItemMarqueeProgressBar1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation=false;
            this.barDockControlTop.Dock=System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location=new System.Drawing.Point( 0 , 0 );
            this.barDockControlTop.Size=new System.Drawing.Size( 928 , 22 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation=false;
            this.barDockControlBottom.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location=new System.Drawing.Point( 0 , 538 );
            this.barDockControlBottom.Size=new System.Drawing.Size( 928 , 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation=false;
            this.barDockControlLeft.Dock=System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location=new System.Drawing.Point( 0 , 22 );
            this.barDockControlLeft.Size=new System.Drawing.Size( 0 , 516 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation=false;
            this.barDockControlRight.Dock=System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location=new System.Drawing.Point( 928 , 22 );
            this.barDockControlRight.Size=new System.Drawing.Size( 0 , 516 );
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock=System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.FixedPanel=DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl2.Horizontal=false;
            this.splitContainerControl2.Location=new System.Drawing.Point( 0 , 0 );
            this.splitContainerControl2.Name="splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add( this.ScriptTabControl );
            this.splitContainerControl2.Panel1.Text="Panel1";
            this.splitContainerControl2.Panel2.Controls.Add( this.ResultRichText );
            this.splitContainerControl2.Panel2.Text="Panel2";
            this.splitContainerControl2.Size=new System.Drawing.Size( 668 , 516 );
            this.splitContainerControl2.SplitterPosition=89;
            this.splitContainerControl2.TabIndex=1;
            this.splitContainerControl2.Text="splitContainerControl2";
            // 
            // ScriptTabControl
            // 
            this.ScriptTabControl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ScriptTabControl.Location=new System.Drawing.Point( 0 , 0 );
            this.ScriptTabControl.Name="ScriptTabControl";
            this.ScriptTabControl.Size=new System.Drawing.Size( 668 , 422 );
            this.ScriptTabControl.TabIndex=0;
            // 
            // ResultRichText
            // 
            this.ResultRichText.Dock=System.Windows.Forms.DockStyle.Fill;
            this.ResultRichText.EditValue=resources.GetString( "ResultRichText.EditValue" );
            this.ResultRichText.Location=new System.Drawing.Point( 0 , 0 );
            this.ResultRichText.Name="ResultRichText";
            this.ResultRichText.Size=new System.Drawing.Size( 668 , 89 );
            this.ResultRichText.TabIndex=0;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.EditValue=50;
            this.progressBarControl1.Location=new System.Drawing.Point( 133 , 395 );
            this.progressBarControl1.MenuManager=this.barManager1;
            this.progressBarControl1.Name="progressBarControl1";
            this.progressBarControl1.Size=new System.Drawing.Size( 364 , 10 );
            this.progressBarControl1.TabIndex=0;
            // 
            // StoredProConfigScreen
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainerControl1 );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name="StoredProConfigScreen";
            this.Size=new System.Drawing.Size( 928 , 538 );
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl1 ) ).EndInit();
            this.splitContainerControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.GridCtrl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.GridView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.chkShowDefaultSP.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.barManager1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.repositoryItemMarqueeProgressBar1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.splitContainerControl2 ) ).EndInit();
            this.splitContainerControl2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.ScriptTabControl ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ResultRichText.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.progressBarControl1.Properties ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl GridCtrl;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraTab.XtraTabControl ScriptTabControl;
        private DevExpress.XtraEditors.MemoEdit ResultRichText;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit chkShowDefaultSP;
        private DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar repositoryItemMarqueeProgressBar1;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
    }
}