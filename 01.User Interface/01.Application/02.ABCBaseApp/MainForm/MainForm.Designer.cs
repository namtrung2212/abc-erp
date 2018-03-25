namespace ABCApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barMenuSkin = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager();
            this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelContainer3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelContainer4=new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelMeeting = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelMeeting_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanelMyTask=new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelMyTask_Container=new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanelAssignedTask=new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelAssignedTask_Container=new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanelNotify = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelNotify_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanelUser = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelUser_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ModulesExplorerTree = new ABCApp.ModulesExplorer();
            this.dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.favouriteTree = new ABCApp.ModulesExplorer();
            this.dockPanel7 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel7_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.ScreenManager = new DevExpress.XtraBars.Docking2010.DocumentManager();
            this.noDocumentsView1 = new DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView();
            this.nativeMdiView1 = new DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.panelContainer2.SuspendLayout();
            this.panelContainer3.SuspendLayout();
            this.panelContainer4.SuspendLayout();
            this.dockPanelMeeting.SuspendLayout();
            this.dockPanelMyTask.SuspendLayout();
            this.dockPanelAssignedTask.SuspendLayout();
            this.dockPanelNotify.SuspendLayout();
            this.dockPanelUser.SuspendLayout();
            this.panelContainer1.SuspendLayout();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModulesExplorerTree)).BeginInit();
            this.dockPanel2.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.favouriteTree)).BeginInit();
            this.dockPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nativeMdiView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barSubItem2,
            this.barButtonItem3,
            this.barMenuSkin});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barMenuSkin, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barMenuSkin
            // 
            this.barMenuSkin.Caption = "Giao &Diện";
            this.barMenuSkin.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMenuSkin.Id = 5;
            this.barMenuSkin.Name = "barMenuSkin";
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "&Giới Thiệu";
            this.barSubItem2.Id = 3;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "&About";
            this.barButtonItem3.Id = 4;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1264, 22);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 730);
            this.barDockControlBottom.Size = new System.Drawing.Size(1264, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 708);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1264, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 708);
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.MenuManager = this.barManager1;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer2,
            this.panelContainer1});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // panelContainer2
            // 
            this.panelContainer2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panelContainer2.Appearance.Options.UseBackColor = true;
            this.panelContainer2.Controls.Add(this.panelContainer3);
            this.panelContainer2.Controls.Add( this.panelContainer4 );         
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer2.ID = new System.Guid("e7700c74-3e63-45f4-9e75-d36536db5ed7");
            this.panelContainer2.Location = new System.Drawing.Point(1017, 22);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(247, 200);
            this.panelContainer2.Size = new System.Drawing.Size(247, 708);
            this.panelContainer2.TabsPosition = DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.panelContainer2.Text = "panelContainer2";
            // 
            // panelContainer3
            // 
            this.panelContainer3.ActiveChild=this.dockPanelNotify;
            this.panelContainer3.Appearance.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 235 ) ) ) ) , ( (int)( ( (byte)( 236 ) ) ) ) , ( (int)( ( (byte)( 239 ) ) ) ) );
            this.panelContainer3.Appearance.Options.UseBackColor=true;
            this.panelContainer3.Controls.Add( this.dockPanelNotify );
            this.panelContainer3.Controls.Add( this.dockPanelMyTask );
            this.panelContainer3.Controls.Add( this.dockPanelAssignedTask );
            this.panelContainer3.Dock=DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer3.FloatVertical=true;
            this.panelContainer3.ID=new System.Guid( "7cfe2143-f97d-4f91-adc0-2bf63fa55826" );
            this.panelContainer3.Location=new System.Drawing.Point( 0 , 0 );
            this.panelContainer3.Name="panelContainer3";
            this.panelContainer3.OriginalSize=new System.Drawing.Size( 247 , 360 );
            this.panelContainer3.Size=new System.Drawing.Size( 247 , 354 );
            this.panelContainer3.Tabbed=true;
            this.panelContainer3.TabsPosition=DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.panelContainer3.Text="panelContainer3"; // 
            // panelContainer4
            // 
            this.panelContainer4.ActiveChild=this.dockPanelUser;
            this.panelContainer4.Appearance.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 235 ) ) ) ) , ( (int)( ( (byte)( 236 ) ) ) ) , ( (int)( ( (byte)( 239 ) ) ) ) );
            this.panelContainer4.Appearance.Options.UseBackColor=true;
            this.panelContainer4.Controls.Add( this.dockPanelUser );
            this.panelContainer4.Controls.Add( this.dockPanelMeeting );
            this.panelContainer4.Dock=DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer4.FloatVertical=true;
            this.panelContainer4.ID=new System.Guid( "7cfe2143-f97d-4f91-adc0-2bf63fa55826" );
            this.panelContainer4.Location=new System.Drawing.Point( 0 , 0 );
            this.panelContainer4.Name="panelContainer4";
            this.panelContainer4.OriginalSize=new System.Drawing.Size( 247 , 360 );
            this.panelContainer4.Size=new System.Drawing.Size( 247 , 354 );
            this.panelContainer4.Tabbed=true;
            this.panelContainer4.TabsPosition=DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.panelContainer4.Text="panelContainer4";
            // 
            // dockPanel6
            // 
            this.dockPanelMeeting.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanelMeeting.Appearance.Options.UseBackColor = true;
            this.dockPanelMeeting.Controls.Add(this.dockPanelMeeting_Container);
            this.dockPanelMeeting.Dock=DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanelMeeting.ID = new System.Guid("8ed0277d-b7ad-4171-aafa-c6908ca4fd00");
            this.dockPanelMeeting.Location = new System.Drawing.Point(0, 354);
            this.dockPanelMeeting.Name="dockPanelMeeting";
            this.dockPanelMeeting.OriginalSize=new System.Drawing.Size( 247 , 354 );
            this.dockPanelMeeting.Size=new System.Drawing.Size( 247 , 354 );
            this.dockPanelMeeting.TabsPosition = DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.dockPanelMeeting.Text = "Trao đổi nhóm";


            // 
            // dockPanel6
            // 
            this.dockPanelMyTask.Appearance.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 235 ) ) ) ) , ( (int)( ( (byte)( 236 ) ) ) ) , ( (int)( ( (byte)( 239 ) ) ) ) );
            this.dockPanelMyTask.Appearance.Options.UseBackColor=true;
            this.dockPanelMyTask.Controls.Add( this.dockPanelMyTask_Container );
            this.dockPanelMyTask.Dock=DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelMyTask.ID=new System.Guid( "8ed0277d-b7ad-4171-aafa-c6908ca4fd00" );
            this.dockPanelMyTask.Location=new System.Drawing.Point( 4 , 52 );
            this.dockPanelMyTask.Name="dockPanel6";
            this.dockPanelMyTask.OriginalSize=new System.Drawing.Size( 239 , 304 );
            this.dockPanelMyTask.Size=new System.Drawing.Size( 239 , 298 );
            this.dockPanelMyTask.TabsPosition=DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.dockPanelMyTask.Text="Được giao";   // 
            // dockPanel6
            // 
            this.dockPanelAssignedTask.Appearance.BackColor=System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 235 ) ) ) ) , ( (int)( ( (byte)( 236 ) ) ) ) , ( (int)( ( (byte)( 239 ) ) ) ) );
            this.dockPanelAssignedTask.Appearance.Options.UseBackColor=true;
            this.dockPanelAssignedTask.Controls.Add( this.dockPanelAssignedTask_Container );
            this.dockPanelAssignedTask.Dock=DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelAssignedTask.ID=new System.Guid( "8ed0277d-b7ad-4171-aafa-c6908ca4fd00" );
            this.dockPanelAssignedTask.Location=new System.Drawing.Point( 4 , 52 );
            this.dockPanelAssignedTask.Name="dockPanelAssignedTask";
            this.dockPanelAssignedTask.OriginalSize=new System.Drawing.Size( 239 , 304 );
            this.dockPanelAssignedTask.Size=new System.Drawing.Size( 239 , 298 );
            this.dockPanelAssignedTask.TabsPosition=DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.dockPanelAssignedTask.Text="Giao việc";
            // 
            // dockPanel6_Container
            // 
            this.dockPanelMeeting_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanelMeeting_Container.Name = "dockPanel6_Container";
            this.dockPanelMeeting_Container.Size = new System.Drawing.Size(239, 298);
            this.dockPanelMeeting_Container.TabIndex = 0;
            // 
            // dockPanel6_Container
            // 
            this.dockPanelMyTask_Container.Location=new System.Drawing.Point( 0 , 0 );
            this.dockPanelMyTask_Container.Name="dockPanelMyTask_Container";
            this.dockPanelMyTask_Container.Size=new System.Drawing.Size( 239 , 298 );
            this.dockPanelMyTask_Container.TabIndex=0;     // 
            // dockPanel6_Container
            // 
            this.dockPanelAssignedTask_Container.Location=new System.Drawing.Point( 0 , 0 );
            this.dockPanelAssignedTask_Container.Name="dockPanelAssignedTask_Container";
            this.dockPanelAssignedTask_Container.Size=new System.Drawing.Size( 239 , 298 );
            this.dockPanelAssignedTask_Container.TabIndex=0;
            // 
            // dockPanelNotify
            // 
            this.dockPanelNotify.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanelNotify.Appearance.Options.UseBackColor = true;
            this.dockPanelNotify.Controls.Add(this.dockPanelNotify_Container);
            this.dockPanelNotify.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelNotify.FloatVertical = true;
            this.dockPanelNotify.ID = new System.Guid("e3525023-f5a6-40f1-8dea-ff8552f14b79");
            this.dockPanelNotify.Location = new System.Drawing.Point(4, 52);
            this.dockPanelNotify.Name = "dockPanelNotify";
            this.dockPanelNotify.OriginalSize = new System.Drawing.Size(239, 304);
            this.dockPanelNotify.Size = new System.Drawing.Size(239, 298);
            this.dockPanelNotify.TabsPosition = DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.dockPanelNotify.Text = "Thông báo";
            this.dockPanelNotify.Click += new System.EventHandler(this.dockPanelNotify_Click);
            // 
            // dockPanelNotify_Container
            // 
            this.dockPanelNotify_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanelNotify_Container.Name = "dockPanelNotify_Container";
            this.dockPanelNotify_Container.Size = new System.Drawing.Size(239, 298);
            this.dockPanelNotify_Container.TabIndex = 0;
            // 
            // dockPanelUser
            // 
            this.dockPanelUser.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanelUser.Appearance.Options.UseBackColor = true;
            this.dockPanelUser.Controls.Add(this.dockPanelUser_Container);
            this.dockPanelUser.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanelUser.ID = new System.Guid("590fc884-46b9-4ce1-bfb5-e2678f6c3ebc");
            this.dockPanelUser.Location = new System.Drawing.Point(0, 354);
            this.dockPanelUser.Name = "dockPanelUser";
            this.dockPanelUser.OriginalSize = new System.Drawing.Size(247, 354);
            this.dockPanelUser.Size=new System.Drawing.Size( 247 , 354 );
            this.dockPanelUser.TabsPosition=DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.dockPanelUser.Text = "   Nhân viên    ";
            this.dockPanelUser.Click += new System.EventHandler(this.dockPanelUser_Click);
            // 
            // dockPanelUser_Container
            // 
            this.dockPanelUser_Container.Location = new System.Drawing.Point(4, 25);
            this.dockPanelUser_Container.Name = "dockPanelUser_Container";
            this.dockPanelUser_Container.Size = new System.Drawing.Size(239, 325);
            this.dockPanelUser_Container.TabIndex = 0;
            // 
            // panelContainer1
            // 
            this.panelContainer1.ActiveChild = this.dockPanel1;
            this.panelContainer1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panelContainer1.Appearance.Options.UseBackColor = true;
            this.panelContainer1.Controls.Add(this.dockPanel2);
            this.panelContainer1.Controls.Add(this.dockPanel1);
            this.panelContainer1.Controls.Add(this.dockPanel7);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelContainer1.FloatVertical = true;
            this.panelContainer1.ID = new System.Guid("48e7d7f9-4441-4715-af25-c5f83add4c10");
            this.panelContainer1.Location = new System.Drawing.Point(0, 22);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(231, 200);
            this.panelContainer1.Size = new System.Drawing.Size(231, 708);
            this.panelContainer1.Tabbed = true;
            this.panelContainer1.TabsPosition = DevExpress.XtraBars.Docking.TabsPosition.Top;
            this.panelContainer1.Text = "panelContainer1";
            // 
            // dockPanel1
            // 
            this.dockPanel1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanel1.Appearance.Options.UseBackColor = true;
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel1.FloatVertical = true;
            this.dockPanel1.ID = new System.Guid("33b6a7e8-8a16-4378-b7f9-2bbe1b5b9ffa");
            this.dockPanel1.Location = new System.Drawing.Point(4, 52);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(223, 664);
            this.dockPanel1.Size = new System.Drawing.Size(223, 652);
           
            this.dockPanel1.Text = "Phân hệ";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.ModulesExplorerTree);
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(223, 652);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // ModulesExplorerTree
            // 
            this.ModulesExplorerTree.Appearance.Empty.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModulesExplorerTree.Appearance.Empty.Options.UseBackColor = true;
            this.ModulesExplorerTree.Appearance.FocusedCell.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModulesExplorerTree.Appearance.FocusedCell.Options.UseBackColor = true;
            this.ModulesExplorerTree.Appearance.FocusedRow.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModulesExplorerTree.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.ModulesExplorerTree.Appearance.FocusedRow.Options.UseBackColor = true;
            this.ModulesExplorerTree.Appearance.FocusedRow.Options.UseForeColor = true;
            this.ModulesExplorerTree.Appearance.Row.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModulesExplorerTree.Appearance.Row.Options.UseBackColor = true;
            this.ModulesExplorerTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModulesExplorerTree.Location = new System.Drawing.Point(0, 0);
            this.ModulesExplorerTree.Name = "ModulesExplorerTree";
            this.ModulesExplorerTree.OptionsPrint.UsePrintStyles = true;
            this.ModulesExplorerTree.OptionsView.ShowColumns = false;
            this.ModulesExplorerTree.OptionsView.ShowIndicator = false;
            this.ModulesExplorerTree.Size = new System.Drawing.Size(223, 652);
            this.ModulesExplorerTree.TabIndex = 0;
            this.ModulesExplorerTree.TreeLevelWidth = 12;
            // 
            // dockPanel2
            // 
            this.dockPanel2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanel2.Appearance.Options.UseBackColor = true;
            this.dockPanel2.Controls.Add(this.dockPanel2_Container);
            this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel2.ID = new System.Guid("04ae9c65-1775-4efa-9f86-3e7670ad1ce6");
            this.dockPanel2.Location = new System.Drawing.Point(4, 52);
            this.dockPanel2.Name = "dockPanel2";
            this.dockPanel2.OriginalSize = new System.Drawing.Size(223, 664);
            this.dockPanel2.Size = new System.Drawing.Size(223, 652);        
            this.dockPanel2.Text = "Báo cáo";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.favouriteTree);
            this.dockPanel2_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(223, 652);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // favouriteTree
            // 
            this.favouriteTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favouriteTree.Location = new System.Drawing.Point(0, 0);
            this.favouriteTree.Name = "favouriteTree";
            this.favouriteTree.OptionsPrint.UsePrintStyles = true;
            this.favouriteTree.Size = new System.Drawing.Size(223, 652);
            this.favouriteTree.TabIndex = 6;
            // 
            // dockPanel7
            // 
            this.dockPanel7.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dockPanel7.Appearance.Options.UseBackColor = true;
            this.dockPanel7.Controls.Add(this.dockPanel7_Container);
            this.dockPanel7.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel7.ID = new System.Guid("6dfd9df7-fe70-4e16-b889-db2c320b3d86");
            this.dockPanel7.Location = new System.Drawing.Point(4, 52);
            this.dockPanel7.Name = "dockPanel7";
            this.dockPanel7.OriginalSize = new System.Drawing.Size(223, 664);
            this.dockPanel7.Size = new System.Drawing.Size(223, 652);
            this.dockPanel7.Visibility=DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            this.dockPanel7.Text = "Thư viện";
            // 
            // dockPanel7_Container
            // 
            this.dockPanel7_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel7_Container.Name = "dockPanel7_Container";
            this.dockPanel7_Container.Size = new System.Drawing.Size(223, 652);
            this.dockPanel7_Container.TabIndex = 0;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "&File";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "&Exit";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "&Help";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // ScreenManager
            // 
            this.ScreenManager.MenuManager = this.barManager1;
            this.ScreenManager.View = this.noDocumentsView1;
            this.ScreenManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.noDocumentsView1,
            this.nativeMdiView1});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 730);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.panelContainer2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.panelContainer2.ResumeLayout(false);
            this.panelContainer3.ResumeLayout(false);
            this.panelContainer4.ResumeLayout( false );
            this.dockPanelMeeting.ResumeLayout(false);
            this.dockPanelMyTask.ResumeLayout( false );
            this.dockPanelAssignedTask.ResumeLayout( false );
            this.dockPanelNotify.ResumeLayout(false);
            this.dockPanelUser.ResumeLayout(false);
            this.panelContainer1.ResumeLayout(false);
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ModulesExplorerTree)).EndInit();
            this.dockPanel2.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.favouriteTree)).EndInit();
            this.dockPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScreenManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nativeMdiView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel2;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        public DevExpress.XtraBars.Docking2010.DocumentManager ScreenManager;
        private ModulesExplorer ModulesExplorerTree;
        private DevExpress.XtraBars.BarSubItem barMenuSkin;
        private ModulesExplorer favouriteTree;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer3;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer4;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelNotify;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanelNotify_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelMeeting;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelMyTask;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelAssignedTask;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanelMeeting_Container;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanelMyTask_Container;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanelAssignedTask_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelUser;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanelUser_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel7;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel7_Container;
        private DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView nativeMdiView1;
        private DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView noDocumentsView1;
      
    }
}