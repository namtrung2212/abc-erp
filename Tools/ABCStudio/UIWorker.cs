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
using ABCControls;
using ABCCommon;
using ABCBusinessEntities;
using ABCHelper;

namespace ABCStudio
{
    public class UIWorker
    {
        public Studio OwnerStudio;
        public UIWorker ( Studio studio )
        {
            OwnerStudio=studio;
        }

        public void AddHostToTabManager ( HostControl hc )
        {
            if ( hc==null )
                return;
            hc.SuspendLayout();

            hc.HostSurface.UndoEngine.Enabled=true;
            hc.HostSurface.CanRedoChanged+=new EventHandler( OwnerStudio.HostSurface_CanUndoRedoChanged );
            hc.HostSurface.CanUndoChanged+=new EventHandler( OwnerStudio.HostSurface_CanUndoRedoChanged );
            
            #region Init Services

            if(hc.HostSurface.ServiceSelection!=null)
                hc.HostSurface.ServiceSelection.SelectionChanged+=new EventHandler( SelectionService_SelectionChanged );

            if ( hc.HostSurface.ServiceComponentChange!=null )
            {
                hc.HostSurface.ServiceComponentChange.ComponentChanged+=new ComponentChangedEventHandler( ServiceComponentChange_ComponentChanged );
                hc.HostSurface.ServiceComponentChange.ComponentAdded+=new ComponentEventHandler( ComponentChangeService_OnComponentAdded );
                hc.HostSurface.ServiceComponentChange.ComponentRemoved+=new ComponentEventHandler( ComponentChangeService_OnComponentRemoved );
            }

            #endregion

            hc.Dock=DockStyle.Fill;
            hc.HostSurface.UndoEngine.CleanEngine();

            hc.HostSurface.ServiceSelection.SetSelectedComponents( null );
            hc.HostSurface.ServiceSelection.SetSelectedComponents( new IComponent[] { hc.HostSurface.DesignerHost.RootComponent } );
            hc.ResumeLayout( false );

            String strText=( (ABCControls.ABCView)hc.HostSurface.DesignerHost.RootComponent ).Caption;
            DevExpress.XtraTab.XtraTabPage tabpage=OwnerStudio.TabViewControl.TabPages.Add( strText );
            tabpage.Tag=hc;
            tabpage.Text=strText;
            hc.Parent=tabpage;
       
       
            OwnerStudio.SurfaceManager.ActiveDesignSurface=hc.HostSurface;
            OwnerStudio.Toolbox.DesignerHost=hc.DesignerHost;
            OwnerStudio.CheckUndoRedo();
            OwnerStudio.ControlListsGrid.RefreshList();
            OwnerStudio.RefreshFieldBindingTree( true );

            OwnerStudio.TabViewControl.SelectedTabPage=tabpage;
       //     Application.DoEvents();

            //    InvalidatePropertyGrid();
           
        }

        #region IComponentChangeService

        void ServiceComponentChange_ComponentChanged ( object sender , ComponentChangedEventArgs e )
        {
            OwnerStudio.CheckUndoRedo();
            OwnerStudio.ControlListsGrid.RefreshList();
     
            InvalidatePropertyGrid();


            if ( e.Member!=null &&e.Member.Name=="Caption" )
            {
                ABCView view=(ABCView)( ( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface ).DesignerHost.RootComponent );
                if ( e.Component==view )
                    OwnerStudio.TabViewControl.SelectedTabPage.Text=view.Caption;
            }

            if ( e.Member!=null&&e.Member.Name=="Url" && e.Component is System.Windows.Forms.WebBrowser)
            {
                ( (System.Windows.Forms.WebBrowser)e.Component ).Url=new Uri( e.NewValue.ToString() );
            }
        }
        void ComponentChangeService_OnComponentAdded ( object sender , ComponentEventArgs e )
        {
           
            OwnerStudio.CheckUndoRedo();
            OwnerStudio.ControlListsGrid.RefreshList();
        }
        void ComponentChangeService_OnComponentRemoved ( object sender , ComponentEventArgs e )
        {
            OwnerStudio.CheckUndoRedo();
            OwnerStudio.ControlListsGrid.RefreshList();
        }
        #endregion

        #region ISelectionService
        private void SelectionService_SelectionChanged ( object sender , EventArgs e )
        {
            InvalidatePropertyGrid();
        }

        private  void InvalidatePropertyGrid ( )
        {
            try
            {
                HostSurface surface=(HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface;
                if ( surface!=null&&surface.ServiceSelection!=null )
                {
                    ICollection selectedComponents=surface.ServiceSelection.GetSelectedComponents();

                    object[] comps=new object[selectedComponents.Count];
                    int i=0;

                    foreach ( Object o in selectedComponents )
                    {
                        comps[i]=o;
                        i++;
                    }

                    OwnerStudio.PropertyGrid.SelectedObjects=comps;
                    OwnerStudio.PropertyGrid.Refresh();
                    if ( surface.ServiceSelection.PrimarySelection!=null )
                        OwnerStudio.ControlListsGrid.SetFocusComponent( ( (Component)surface.ServiceSelection.PrimarySelection ).Site.Name );
                }
            }
            catch ( Exception ex )
            {
            }
        }
        #endregion

        public void SaveCurrentView ()
        {
            if ( OwnerStudio.SurfaceManager.ActiveDesignSurface==null )
                return;

            ABCView view=(ABCView)( ( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface ).DesignerHost.RootComponent );
            view.Save();
        }
        public void OpenFromXMLFile ( String strFileName )
        {
          
            ABCWaitingDialog.Show( "" , "Opening . . .!" );

            HostControl hc=OwnerStudio.SurfaceManager.OpenNewForm( strFileName );
            AddHostToTabManager( hc );

            ABCWaitingDialog.Close();
        }
        public void OpenFromDatabase ( STViewsInfo viewInfo )
        {

            ABCWaitingDialog.Show( "" , "Opening . . .!" );

            HostControl hc=OwnerStudio.SurfaceManager.OpenNewForm( viewInfo );
            AddHostToTabManager( hc );

            ABCWaitingDialog.Close();

        }

        public void AddNewForm ()
        {
            HostControl hc=OwnerStudio.SurfaceManager.AddNewForm();
            AddHostToTabManager( hc );
            Application.DoEvents();
        }

        public void TestCurrentScreen ( )
        {
            if ( OwnerStudio.SurfaceManager.ActiveDesignSurface==null )
                return;

            ABCView currentView=(ABCView)( ( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface ).DesignerHost.RootComponent );
            currentView.SaveToXML( "temp.xml" );

            ABCScreen.ABCBaseScreen scr=null;

            String strSourceCode=( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface ).OwnerHostControl.SourceCSharpEditor.Text;

            if ( currentView.DataField==null )
            {
                if ( currentView.IsUseSourceCode&&String.IsNullOrWhiteSpace( strSourceCode )==false )
                    ABCScreen.ABCScreenFactory.DebugScreen( "temp.xml" , strSourceCode , "ABCTemp" , ViewMode.Test );
                else
                    ABCScreen.ABCScreenFactory.RunScreen( "temp.xml" ,null , ViewMode.Test );
            }
            else
            {
                if ( String.IsNullOrWhiteSpace( strSourceCode ) )
                    strSourceCode=currentView.DataField.STViewCode;

                if ( currentView.DataField.STViewUseCode&&String.IsNullOrWhiteSpace( strSourceCode )==false )
                    ABCScreen.ABCScreenFactory.DebugScreen( "temp.xml" , strSourceCode , currentView.DataField.STViewNo , ViewMode.Test );
                else
                    ABCScreen.ABCScreenFactory.RunScreen( "temp.xml" , currentView.DataField , ViewMode.Test );
            }
        }

        public void CloseActiveSurface ( )
        {
            CloseSurface( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface );
            OwnerStudio.PropertyGrid.SelectedObject=null;
        
            GC.Collect();
        }
        public void CloseSurface ( HostSurface surface )
        {
            if ( surface==null )
                return;

            DevExpress.XtraTab.XtraTabPage currentTab=GetTabPageFromHostSurface( surface );
            if ( currentTab==null )
                return;

            if ( surface.UndoEngine.CanUndo )
            {
                 ABCView view=(ABCView)surface.DesignerHost.RootComponent;
                 if ( view!=null )
                 {
                     DialogResult dlgResult=ABCHelper.ABCMessageBox.Show( String.Format( @"Do you want to save '{0}' View   ?" , view.Name ) , "Message" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Question );
                     if ( dlgResult==DialogResult.Cancel )
                         return;

                     if ( dlgResult==DialogResult.Yes )
                         view.Save();
                 }
            }

            OwnerStudio.SurfaceManager.CloseSurface( surface );

            OwnerStudio.TabViewControl.TabPages.Remove( currentTab );
            OwnerStudio.PropertyGrid.SelectedObject=null;
        }

        public void RefreshActiveSurface ( )
        {
            if ( OwnerStudio.SurfaceManager.ActiveDesignSurface==null )
                return;

          ((ABCView)( (HostSurface)OwnerStudio.SurfaceManager.ActiveDesignSurface ).DesignerHost.RootComponent).RefreshLayout();
        }

        public DevExpress.XtraTab.XtraTabPage GetTabPageFromHostSurface ( HostSurface surface )
        {
            foreach ( DevExpress.XtraTab.XtraTabPage tabPage in OwnerStudio.TabViewControl.TabPages )
                if ( tabPage.Tag is HostControl&&( (HostControl)tabPage.Tag ).HostSurface!=null&&( (HostControl)tabPage.Tag ).HostSurface==surface )
                    return tabPage;

            return null;
        }


    }
}
