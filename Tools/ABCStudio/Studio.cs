using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ABCControls;

using ABCProvider;

namespace ABCStudio
{
    public partial class Studio : DevExpress.XtraEditors.XtraForm
    {
        public HostSurfaceManager SurfaceManager;
        public UIWorker Worker;

        public Studio ( bool isEndUser)
        {
            InitializeComponent();

            Initialize( isEndUser );
        }

        public void Initialize ( bool isEndUser )
        {
            Worker=new UIWorker( this );
            SurfaceManager=new HostSurfaceManager();
            SurfaceManager.AddService( typeof( IToolboxService ) , this.Toolbox );

          //  InitializeSkins();

            #region ControlListsGrid
            ControlListsGrid=new ControlCollectionGrid( this );
            ControlListsGrid.Dock=DockStyle.Fill;
            dockPanel6.Controls.Add( ControlListsGrid );

            #endregion


            #region ViewTreeList
            ViewTreeList.OwnerStudio=this;
            ViewTreeList.Initialize();
            ViewTreeList.RefreshViewList();

            #endregion


            #region Toolbox

            this.Toolbox.InitializeToolbox();

            #endregion

            #region FieldBindingTree
            FieldBindingTree.OwnerStudio=this;
            FieldBindingTree.Initialize();

            #endregion

            #region TabViewControl

            TabViewControl.SelectedPageChanged+=new DevExpress.XtraTab.TabPageChangedEventHandler( TabViewControl_SelectedPageChanged );
            TabViewControl.CloseButtonClick+=new EventHandler( TabViewControl_CloseButtonClick );

            #endregion

            #region BarManager

            StudioBarManager.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Toolbar_ItemClick );

            #endregion

            #region PropertyGrid
        //    PropertyGrid.AutoGenerateRows=true;
          //  PropertyGrid.CustomPropertyDescriptors+=new DevExpress.XtraVerticalGrid.Events.CustomPropertyDescriptorsEventHandler( PropertyGrid_CustomPropertyDescriptors );

            #endregion

        }
  
        #region PropertyGrid

        void PropertyGrid_CustomPropertyDescriptors ( object sender , DevExpress.XtraVerticalGrid.Events.CustomPropertyDescriptorsEventArgs e )
        {
            //if ( e.Source is IABCControl )
            //{
            try
            {
                PropertyDescriptorCollection filteredCollection=new PropertyDescriptorCollection( null );
                Type type=e.Source.GetType();
                List<String> lstFilter=ABCPresentHelper.GetUsablePropertiesList( type );
                foreach ( PropertyInfo proInfo in e.Source.GetType().GetProperties() )
                {
                    if ( lstFilter.Contains( proInfo.Name )||proInfo.Name.StartsWith( "Name_" ) )
                        if ( e.Properties[proInfo.Name]!=null )
                            filteredCollection.Add( e.Properties[proInfo.Name] );
                }
                e.Properties=filteredCollection;
            }catch(Exception ex)
            {
            }
            //    }
        }
        
        #endregion

        #region SurfaceTab
        void TabViewControl_CloseButtonClick ( object sender , EventArgs e )
        {
            
            if ( e is DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs )
            {
                DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs ee=(DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
                HostControl hostCtrl = ( (DevExpress.XtraTab.XtraTabPage)ee.Page ).Tag as HostControl;
                if ( hostCtrl.HostSurface!=null )
                    Worker.CloseSurface( hostCtrl.HostSurface );

                //if ( this.TabViewControl.TabPages.Contains( (DevExpress.XtraTab.XtraTabPage)ee.Page ) )
                //    this.TabViewControl.TabPages.Remove( (DevExpress.XtraTab.XtraTabPage)ee.Page );
            }
        }
        void TabViewControl_SelectedPageChanged ( object sender , DevExpress.XtraTab.TabPageChangedEventArgs e )
        {
            if ( TabViewControl.TabPages.Count<=0 )
                logoPanel.Visible=true;
            else
                logoPanel.Visible=false;

            try
            {
                if ( e.Page!=null&&e.Page.Tag!=null )
                {
                    HostControl hostCtrl=(HostControl)e.Page.Tag;
                    if ( hostCtrl.HostSurface!=null )
                        SurfaceManager.ActiveDesignSurface=hostCtrl.HostSurface;
                }
                else
                    SurfaceManager.ActiveDesignSurface=null;
            }
            catch ( Exception ex )
            {
            }

            this.ControlListsGrid.RefreshList();

            this.RefreshFieldBindingTree(false);

        
        }
        #endregion

        #region Toolbar && Menu
       
        #region Skin
        private void InitializeSkins ( )
        {

            int i=100;
            foreach ( DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins )
            {
                i++;
                DevExpress.XtraBars.BarCheckItem menuItem=new DevExpress.XtraBars.BarCheckItem();
                menuItem.Caption=skin.SkinName;
                menuItem.Name="Skin:"+skin.SkinName;
                menuItem.Tag="Skin:"+skin.SkinName;
                menuItem.Id=i;
                menuItem.Checked=false;

                StudioBarManager.Items.Add( menuItem );
                barMenuSkins.LinksPersistInfo.Add( new DevExpress.XtraBars.LinkPersistInfo( menuItem ) );
       //         barMenuSkins.AddItem( menuItem );
            }
        }

        private void SetSkin ( DevExpress.XtraBars.BarCheckItem menuItem )
        {
            foreach ( DevExpress.XtraBars.BarItemLink link in barMenuSkins.ItemLinks )
                (link.Item as   DevExpress.XtraBars.BarCheckItem ).Checked=false;
            menuItem.Checked=true;

            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName=menuItem.Tag.ToString().Split( ':' )[1];
        }
        #endregion

        #region Undo-Redo

        public void CheckUndoRedo ( )
        {
            if ( (HostSurface)SurfaceManager.ActiveDesignSurface!=null )
            {
                barUndo.Enabled=( (HostSurface)SurfaceManager.ActiveDesignSurface ).UndoEngine.CanUndo;
                barRedo.Enabled=( (HostSurface)SurfaceManager.ActiveDesignSurface ).UndoEngine.CanRedo;
            }
        }

        public void HostSurface_CanUndoRedoChanged ( object sender , EventArgs e )
        {
            CheckUndoRedo();
        }

        #endregion


        void Toolbar_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag==null )
                return;


            if ( e.Item.Tag.ToString().Contains( "Skin" ) )
                SetSkin( e.Item as DevExpress.XtraBars.BarCheckItem );

            HostSurface surface=(HostSurface)SurfaceManager.ActiveDesignSurface;

            #region Tools
            if ( e.Item.Tag.ToString()=="DataTable" )
            {
                DatabaseManagement form=new DatabaseManagement( DatabaseManagement.ActiveTab.DataTable,this );
                form.ShowDialog();
            }
            if ( e.Item.Tag.ToString()=="SP" )
            {
                DatabaseManagement form=new DatabaseManagement( DatabaseManagement.ActiveTab.StoredProcedure , this );
                form.ShowDialog();
            }
            if ( e.Item.Tag.ToString()=="TableConfig" )
            {
                DatabaseManagement form=new DatabaseManagement( DatabaseManagement.ActiveTab.TableConfig , this );
                form.ShowDialog();
            }
            if ( e.Item.Tag.ToString()=="FieldConfig" )
            {
                DatabaseManagement form=new DatabaseManagement( DatabaseManagement.ActiveTab.FieldConfig , this );
                form.ShowDialog();
            }
            if ( e.Item.Tag.ToString()=="EnumDefine" )
            {
                DatabaseManagement form=new DatabaseManagement( DatabaseManagement.ActiveTab.EnumDefine , this );
                form.ShowDialog();
            }
            if ( e.Item.Tag.ToString()=="GenDLL" )
            {
                GenerationProvider.GenerateAll();
                ABCHelper.ABCMessageBox.Show( "Done!" );
            }
            #endregion

            #region Screens
            if ( e.Item.Tag.ToString()=="BindConfig" )
            {
                ConfigBinding();
            }
            if ( e.Item.Tag.ToString()=="BindRefresh" )
            {
                RefreshFieldBindingTree(true);
            }
            #endregion

            #region Standard
            if ( e.Item.Tag.ToString()=="NewBlank" )
                Worker.AddNewForm();
            if ( e.Item.Tag.ToString()=="NewWizard" )
                new Wizard.NewView(this).ShowDialog();
            if ( e.Item.Tag.ToString()=="Save" )
                Worker.SaveCurrentView();
            if ( e.Item.Tag.ToString()=="Open" )
            {
                OpenFileDialog dlg=new OpenFileDialog();
                dlg.Filter="xml files (*.xml)|*.xml|All files (*.*)|*.*";
                dlg.FilterIndex=2;
                dlg.RestoreDirectory=true;
                dlg.Title="Open Layout from XML file";
                if ( dlg.ShowDialog()==DialogResult.OK )
                    Worker.OpenFromXMLFile( dlg.FileName );
            }
            #endregion

            if ( this.SurfaceManager.ActiveDesignSurface==null )
                return;

            #region Surface

            if ( e.Item.Tag.ToString()=="Close" )
                Worker.CloseActiveSurface();

            if ( e.Item.Tag.ToString()=="Refresh" )
                Worker.RefreshActiveSurface();

            if ( e.Item.Tag.ToString()=="Run" )
                Worker.TestCurrentScreen();


            #region Layout
            if ( e.Item.Tag.ToString()=="SelectAll" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.SelectAll );
            if ( e.Item.Tag.ToString()=="Delete" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Delete );
            if ( e.Item.Tag.ToString()=="Cut" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Cut );
            if ( e.Item.Tag.ToString()=="Copy" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Copy );
            if ( e.Item.Tag.ToString()=="Paste" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Paste );
            if ( e.Item.Tag.ToString()=="Undo" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Undo );
            if ( e.Item.Tag.ToString()=="Redo" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.Redo );
            if ( e.Item.Tag.ToString()=="Left" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignLeft );
            if ( e.Item.Tag.ToString()=="Center" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignVerticalCenters );
            if ( e.Item.Tag.ToString()=="Right" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignRight );
            if ( e.Item.Tag.ToString()=="Top" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignTop );
            if ( e.Item.Tag.ToString()=="Middle" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignHorizontalCenters );
            if ( e.Item.Tag.ToString()=="Bottom" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.AlignBottom );
            if ( e.Item.Tag.ToString()=="SameWidth" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.SizeToControlWidth );
            if ( e.Item.Tag.ToString()=="SameHeight" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.SizeToControlHeight );
            if ( e.Item.Tag.ToString()=="SameSize" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.SizeToControl );
            if ( e.Item.Tag.ToString()=="BringFront" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.BringToFront );
            if ( e.Item.Tag.ToString()=="BringEnd" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.SendToBack );

            if ( e.Item.Tag.ToString()=="HorizontalSpacingEqual" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.HorizSpaceMakeEqual );
            if ( e.Item.Tag.ToString()=="IncreaseHorizontal" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.HorizSpaceIncrease );
            if ( e.Item.Tag.ToString()=="DecreaseHorizontal" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.HorizSpaceDecrease );
            if ( e.Item.Tag.ToString()=="RemoveHorizontal" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.HorizSpaceConcatenate );

            if ( e.Item.Tag.ToString()=="VerticalSpacingEqual" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.VertSpaceMakeEqual );
            if ( e.Item.Tag.ToString()=="IncreaseVertial" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.VertSpaceIncrease );
            if ( e.Item.Tag.ToString()=="DecreaseVertical" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.VertSpaceDecrease );
            if ( e.Item.Tag.ToString()=="RemoveVertical" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.VertSpaceConcatenate );

            if ( e.Item.Tag.ToString()=="Locked" )
                surface.PerformAction( System.ComponentModel.Design.StandardCommands.LockControls );

            #endregion

            #region DOCK
            if ( e.Item.Tag.ToString()=="DockNone" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.None;
            }

            if ( e.Item.Tag.ToString()=="DockFill" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.Fill;
            }
            if ( e.Item.Tag.ToString()=="DockLeft" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.Left;
            }
            if ( e.Item.Tag.ToString()=="DockRight" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.Right;
            }
            if ( e.Item.Tag.ToString()=="DockTop" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.Top;
            }
            if ( e.Item.Tag.ToString()=="DockBottom" )
            {
                foreach ( Control ctrl in surface.ServiceSelection.GetSelectedComponents() )
                    ctrl.Dock=DockStyle.Bottom;
            }
            #endregion

            #endregion

        }

        #endregion

        #region Field Binding

        private void btnBindingConfig_Click ( object sender , EventArgs e )
        {
            ConfigBinding();
        }

        private void btnRefreshBinding_Click ( object sender , EventArgs e )
        {
            RefreshFieldBindingTree(true);
        }

        public void RefreshFieldBindingTree (bool isReloadNodes )
        {

            this.FieldBindingTree.RefreshFieldNodes( HostSurfaceManager.CurrentManager.ActiveDesignSurface as HostSurface , isReloadNodes );

        }

        public void ConfigBinding ( )
        {
            if ( HostSurfaceManager.CurrentManager.ActiveDesignSurface==null )
                return;

            ABCView view=(ABCView)( HostSurfaceManager.CurrentManager.ActiveDesignSurface as HostSurface ).DesignerHost.RootComponent;

            using ( ABCBusinessConfigEditorForm form=new ABCBusinessConfigEditorForm( view ) )
            {
                form.DataConfig=view.DataConfig;
                if ( form.DataConfig==null )
                    form.DataConfig=new ABCScreen.ABCScreenConfig( view );
                if ( form.ShowDialog()==DialogResult.OK )
                    view.DataConfig=form.NewDataConfig;
            }
        }
        #endregion
    }
}
