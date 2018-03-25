namespace ABCStudio
{
    partial class DatabaseManagement
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
            this.SPPanel=new DevExpress.XtraEditors.PanelControl();
            this.backstageViewItemSeparator1=new DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator();
            this.backstageViewTabItem1=new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.backstageViewItemSeparator5=new DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator();
            this.backstageViewTabItem5=new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.backstageViewItemSeparator6=new DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator();
            this.backstageViewTabItem2=new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.viewClientSP=new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.xtraTabControl1=new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1=new DevExpress.XtraTab.XtraTabPage();
            this.DataTablePanel=new DevExpress.XtraEditors.PanelControl();
            this.xtraTabPage2=new DevExpress.XtraTab.XtraTabPage();
            this.EnumDefinePanel=new DevExpress.XtraEditors.PanelControl();
            this.tabDictionary=new DevExpress.XtraTab.XtraTabPage();
            this.DictionaryPanel=new DevExpress.XtraEditors.PanelControl();
            ( (System.ComponentModel.ISupportInitialize)( this.SPPanel ) ).BeginInit();
            this.viewClientSP.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.xtraTabControl1 ) ).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.DataTablePanel ) ).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.EnumDefinePanel ) ).BeginInit();
            this.tabDictionary.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.DictionaryPanel ) ).BeginInit();
            this.SuspendLayout();
            // 
            // SPPanel
            // 
            this.SPPanel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.SPPanel.Location=new System.Drawing.Point( 0 , 0 );
            this.SPPanel.Name="SPPanel";
            this.SPPanel.Size=new System.Drawing.Size( 0 , 0 );
            this.SPPanel.TabIndex=3;
            // 
            // backstageViewItemSeparator1
            // 
            this.backstageViewItemSeparator1.Name="backstageViewItemSeparator1";
            // 
            // backstageViewTabItem1
            // 
            this.backstageViewTabItem1.Caption="DataTables";
            this.backstageViewTabItem1.ImageIndex=13;
            this.backstageViewTabItem1.Name="backstageViewTabItem1";
            this.backstageViewTabItem1.Selected=false;
            // 
            // backstageViewItemSeparator5
            // 
            this.backstageViewItemSeparator5.Name="backstageViewItemSeparator5";
            // 
            // backstageViewTabItem5
            // 
            this.backstageViewTabItem5.Caption="Enum Define";
            this.backstageViewTabItem5.ImageIndex=24;
            this.backstageViewTabItem5.Name="backstageViewTabItem5";
            this.backstageViewTabItem5.Selected=false;
            // 
            // backstageViewItemSeparator6
            // 
            this.backstageViewItemSeparator6.Name="backstageViewItemSeparator6";
            // 
            // backstageViewTabItem2
            // 
            this.backstageViewTabItem2.Caption="Stored Procedures";
            this.backstageViewTabItem2.ContentControl=null;
            this.backstageViewTabItem2.ImageIndex=45;
            this.backstageViewTabItem2.Name="backstageViewTabItem2";
            this.backstageViewTabItem2.Selected=false;
            // 
            // viewClientSP
            // 
            this.viewClientSP.Controls.Add( this.SPPanel );
            this.viewClientSP.Name="viewClientSP";
            this.viewClientSP.TabIndex=1;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.xtraTabControl1.Name="xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage=this.xtraTabPage1;
            this.xtraTabControl1.Size=new System.Drawing.Size( 1006 , 440 );
            this.xtraTabControl1.TabIndex=1;
            this.xtraTabControl1.TabPages.AddRange( new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.tabDictionary} );
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add( this.DataTablePanel );
            this.xtraTabPage1.Name="xtraTabPage1";
            this.xtraTabPage1.Size=new System.Drawing.Size( 1000 , 412 );
            this.xtraTabPage1.Text="Database Management";
            // 
            // DataTablePanel
            // 
            this.DataTablePanel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DataTablePanel.Location=new System.Drawing.Point( 0 , 0 );
            this.DataTablePanel.Name="DataTablePanel";
            this.DataTablePanel.Size=new System.Drawing.Size( 1000 , 412 );
            this.DataTablePanel.TabIndex=3;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add( this.EnumDefinePanel );
            this.xtraTabPage2.Name="xtraTabPage2";
            this.xtraTabPage2.Size=new System.Drawing.Size( 1000 , 412 );
            this.xtraTabPage2.Text="Enums Configuration";
            // 
            // EnumDefinePanel
            // 
            this.EnumDefinePanel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.EnumDefinePanel.Location=new System.Drawing.Point( 0 , 0 );
            this.EnumDefinePanel.Name="EnumDefinePanel";
            this.EnumDefinePanel.Size=new System.Drawing.Size( 1000 , 412 );
            this.EnumDefinePanel.TabIndex=4;
            // 
            // tabDictionary
            // 
            this.tabDictionary.Controls.Add( this.DictionaryPanel );
            this.tabDictionary.Name="tabDictionary";
            this.tabDictionary.Size=new System.Drawing.Size( 1000 , 412 );
            this.tabDictionary.Text="      Dictionary      ";
            // 
            // DictionaryPanel
            // 
            this.DictionaryPanel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.DictionaryPanel.Location=new System.Drawing.Point( 0 , 0 );
            this.DictionaryPanel.Name="DictionaryPanel";
            this.DictionaryPanel.Size=new System.Drawing.Size( 1000 , 412 );
            this.DictionaryPanel.TabIndex=4;
            // 
            // DatabaseManagement
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 1006 , 440 );
            this.Controls.Add( this.xtraTabControl1 );
            this.Name="DatabaseManagement";
            this.Text="DatabaseManagement";
            ( (System.ComponentModel.ISupportInitialize)( this.SPPanel ) ).EndInit();
            this.viewClientSP.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.xtraTabControl1 ) ).EndInit();
            this.xtraTabControl1.ResumeLayout( false );
            this.xtraTabPage1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.DataTablePanel ) ).EndInit();
            this.xtraTabPage2.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.EnumDefinePanel ) ).EndInit();
            this.tabDictionary.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.DictionaryPanel ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator backstageViewItemSeparator1;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem1;
        private DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator backstageViewItemSeparator5;
        private DevExpress.XtraEditors.PanelControl SPPanel;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem5;
        private DevExpress.XtraBars.Ribbon.BackstageViewItemSeparator backstageViewItemSeparator6;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem2;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl viewClientSP;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.PanelControl DataTablePanel;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.PanelControl EnumDefinePanel;
        private DevExpress.XtraTab.XtraTabPage tabDictionary;
        private DevExpress.XtraEditors.PanelControl DictionaryPanel;

    }
}