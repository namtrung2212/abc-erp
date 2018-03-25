using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Reflection;

namespace ABCControls 
{
    public class HostSurface : DesignSurface
    {
        public HostControl OwnerHostControl;
        public ABCCommonTreeListNode TreeFieldNodes;

        public DesignerLoader Loader;

        public IDesignerHost DesignerHost;
        public ISelectionService ServiceSelection;
        public IComponentChangeService ServiceComponentChange;
        public IMenuCommandService ServiceMenuCommand;
        public IDesignerSerializationService ServiceSerializer;
        public INameCreationService NameCreationService;
        public UndoEngineImpl UndoEngine;

        public bool IsTemplate=false;

        public HostSurface ( ): base()
        {
            this.AddService( typeof( IMenuCommandService ) , new MenuCommandService( this ) );
        }

        public HostSurface ( IServiceProvider parentProvider ): base( parentProvider )
        {
            this.AddService( typeof( IMenuCommandService ) , new MenuCommandService( this ) );
        }

        internal void Initialize ( )
        {
            Control control=null;
            DesignerHost=(IDesignerHost)this.GetService( typeof( IDesignerHost ) );
            if ( DesignerHost==null )
                return;

            ( (ABCControls.ABCView)DesignerHost.RootComponent ).Surface=this;
    
            try
            {
                #region Set the backcolor
                Type hostType=DesignerHost.RootComponent.GetType();
                if ( hostType==typeof( DevExpress.XtraEditors.XtraForm ) )
                {
                    control=this.View as Control;
                    control.BackColor=Color.White;
                }
                else if ( hostType==typeof( UserControl ) )
                {
                    control=this.View as Control;
                    control.BackColor=Color.White;
                }
                else if ( hostType==typeof( Component ) )
                {
                    control=this.View as Control;
                    control.BackColor=Color.FloralWhite;
                }
                else if ( hostType==typeof( ABCControls.ABCView ) )
                {
                    control=this.View as Control;
              //      control.BackColor=Color.FloralWhite;
                }
                else
                {
                    throw new Exception( "Undefined Host Type: "+hostType.ToString() );
                }
                #endregion

                #region  Set Services
                ServiceSelection=(ISelectionService)( this.ServiceContainer.GetService( typeof( ISelectionService ) ) );
                ServiceComponentChange=(IComponentChangeService)this.ServiceContainer.GetService( typeof( IComponentChangeService ) );
                ServiceMenuCommand=(IMenuCommandService)this.ServiceContainer.GetService( typeof( IMenuCommandService ) );
                ServiceSerializer=(IDesignerSerializationService)this.ServiceContainer.GetService( typeof( IDesignerSerializationService ) );

                #region Undo
                UndoEngine=new UndoEngineImpl( this.ServiceContainer );
                UndoEngine.CanUndoChanged+=new EventHandler( UndoEngine_CanUndoChanged );
                UndoEngine.CanRedoChanged+=new EventHandler( UndoEngine_CanRedoChanged );

                //disable the UndoEngine
                UndoEngine.Enabled=false;
                if ( UndoEngine!=null )
                {
                    //- the UndoEngine is ready to be replaced
                    this.ServiceContainer.RemoveService( typeof( UndoEngine ) , false );
                    this.ServiceContainer.AddService( typeof( UndoEngine ) , UndoEngine );
                } 
                #endregion

                NameCreationService=(INameCreationService)( this.GetService( typeof( INameCreationService ) ) );

                #endregion

                ( (System.Windows.Forms.Control)( this.View ) ).PreviewKeyDown+=new PreviewKeyDownEventHandler( HostSurface_PreviewKeyDown );
            }
            catch ( Exception ex )
            {
                Trace.WriteLine( ex.ToString() );
            }
        }

        void HostSurface_PreviewKeyDown ( object sender , PreviewKeyDownEventArgs e )
        {
            if ( e.KeyCode==Keys.Escape )
            {
                ServiceSelection.SetSelectedComponents(new IComponent[] {  DesignerHost.RootComponent });
            }
            else if ( e.KeyCode==Keys.Delete )
                this.PerformAction( System.ComponentModel.Design.StandardCommands.Delete );
            else if ( e.KeyCode==Keys.X && e.Control )
                this.PerformAction( System.ComponentModel.Design.StandardCommands.Cut );
            else if ( e.KeyCode==Keys.C&&e.Control )
                this.PerformAction( System.ComponentModel.Design.StandardCommands.Copy );
            else if ( e.KeyCode==Keys.V&&e.Control )
                this.PerformAction( System.ComponentModel.Design.StandardCommands.Paste );
            else if ( e.KeyCode==Keys.Z&&e.Control )
                this.DoUndo();
            else if ( e.KeyCode==Keys.Y&&e.Control )
                this.DoRedo();
            else if ( e.KeyCode==Keys.A&&e.Control )
                this.PerformAction( System.ComponentModel.Design.StandardCommands.SelectAll );
         
                
                #region Location
            else if ( e.KeyCode==Keys.Left )
            {
                foreach ( Component comp in ServiceSelection.GetSelectedComponents() )
                {
                    if ( comp is Control && comp != DesignerHost.RootComponent)
                        ( (Control)comp ).Left-=2;
                }
            }
            else if ( e.KeyCode==Keys.Right )
            {
                foreach ( Component comp in ServiceSelection.GetSelectedComponents() )
                {
                    if ( comp is Control&&comp!=DesignerHost.RootComponent )
                        ( (Control)comp ).Left+=2;
                }
            }
            else if ( e.KeyCode==Keys.Up )
            {
                foreach ( Component comp in ServiceSelection.GetSelectedComponents() )
                {
                    if ( comp is Control&&comp!=DesignerHost.RootComponent )
                        ( (Control)comp ).Top-=2;
                }
            }
            else if ( e.KeyCode==Keys.Down )
            {
                foreach ( Component comp in ServiceSelection.GetSelectedComponents() )
                {
                    if ( comp is Control&&comp!=DesignerHost.RootComponent )
                        ( (Control)comp ).Top+=2;
                }
            }
            #endregion
        }

        public void AddService ( Type type , object serviceInstance )
        {
            this.ServiceContainer.AddService( type , serviceInstance );
        }

        #region IMenuCommandService

        public void PerformAction ( CommandID cmd )
        {
            try
            {
                ServiceMenuCommand.GlobalInvoke( cmd );
            }
            catch
            {
                //    this.OutputWindow.RichTextBox.Text+="Error in performing the action: "+text.Replace( "&" , "" );
            }
        }

        #region Copy - Paste -Cut - Delete

        //public void Delete ( )
        //{
        //    ICollection componentList=ServiceSelection.GetSelectedComponents();
        //    string description="Delete "+
        //        ( componentList.Count>1?( componentList.Count.ToString()+" controls" ):
        //         ( (IComponent)ServiceSelection.PrimarySelection ).Site.Name );

        //    DesignerTransaction transaction=DesignerHost.CreateTransaction( description );

        //    foreach ( object component in componentList )
        //    {
        //        if ( component!=DesignerHost.RootComponent )
        //        {
        //            ComponentDesigner designer=(ComponentDesigner)DesignerHost.GetDesigner( (IComponent)component );
        //            if ( designer!=null&&designer.AssociatedComponents!=null )
        //            {
        //                foreach ( object associatedComponent in designer.AssociatedComponents )
        //                    DesignerHost.DestroyComponent( (IComponent)associatedComponent );
        //            }
        //            DesignerHost.DestroyComponent( (IComponent)component );
        //        }
        //    }

        //    transaction.Commit();
        //    ( (IDisposable)transaction ).Dispose();
        //}

        //private object OwnClipboard=null;

        //public void Copy ()
        //{

        //    ICollection componentList=ServiceSelection.GetSelectedComponents();
        //    ArrayList toCopy=new ArrayList();
        //    foreach ( object component in componentList )
        //    {
        //        if ( component==DesignerHost.RootComponent )
        //            continue;

        //        toCopy.Add( component );
        //        ComponentDesigner designer=DesignerHost.GetDesigner( (IComponent)component ) as ComponentDesigner;
        //        if ( designer!=null&&designer.AssociatedComponents!=null )
        //            toCopy.AddRange( designer.AssociatedComponents );
        //    }

        //    object stateData=ServiceSerializer.Serialize( toCopy );

        //    OwnClipboard=stateData;

        //}

        //public void Paste ( )
        //{
        //    if ( OwnClipboard==null )
        //        return;

        //    DesignerTransaction transaction=DesignerHost.CreateTransaction( "Paste" );
        //    ICollection components=ServiceSerializer.Deserialize( OwnClipboard );

        //    foreach ( object component in components )
        //    {
        //        Control control=component as Control;
        //        if ( control==null )
        //            continue; // pure Components are added to the ComponentTray by the DocumentDesigner

        //        PropertyDescriptor parentProperty=TypeDescriptor.GetProperties( control )["Parent"];
        //        if ( control.Parent!=null )
        //        {

        //            if ( ServiceComponentChange!=null )
        //            {
        //                ServiceComponentChange.OnComponentChanging( control , parentProperty );
        //                ServiceComponentChange.OnComponentChanged( control , parentProperty , null , control.Parent );
        //            }
        //        }
        //        else
        //        {
        //            ParentControlDesigner parentDesigner=null;
        //            if ( ServiceSelection!=null&&ServiceSelection.PrimarySelection!=null )
        //                parentDesigner=DesignerHost.GetDesigner( (IComponent)ServiceSelection.PrimarySelection ) as ParentControlDesigner;
        //            if ( parentDesigner==null )
        //                parentDesigner=DesignerHost.GetDesigner( DesignerHost.RootComponent ) as DocumentDesigner;
        //            if ( parentDesigner!=null&&parentDesigner.CanParent( control ) )
        //                parentProperty.SetValue( control , parentDesigner.Control );
        //        }
        //    }
        //    OwnClipboard=null;
        //    transaction.Commit();
        //    ( (IDisposable)transaction ).Dispose();
        //}
        #endregion

        #region Undo - Redo
        public void DoUndo ( )
        {
            UndoEngine.Undo();
        }
        public void DoRedo ( )
        {
            UndoEngine.Redo();
        }

        public event EventHandler CanUndoChanged;
        public event EventHandler CanRedoChanged;
        void UndoEngine_CanRedoChanged ( object sender , EventArgs e )
        {
            if ( CanRedoChanged!=null )
                CanRedoChanged( sender , e );
        }
        void UndoEngine_CanUndoChanged ( object sender , EventArgs e )
        {
            if ( CanUndoChanged!=null )
                CanUndoChanged( sender , e );
        }
        
        #endregion

        #endregion


        public void CleanSurface ( )
        {
            foreach ( IComponent comp in this.DesignerHost.Container.Components )
            {
                if ( comp!=this.DesignerHost.RootComponent )
                    this.DesignerHost.DestroyComponent( comp );
            }

        }

        protected override void Dispose ( bool disposing )
        {
            base.Dispose( disposing );


        }
    }
}
