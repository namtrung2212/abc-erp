using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking;

using ABCCommon;


namespace ABCControls
{
    public class ABCDockPanel : DevExpress.XtraBars.Docking.DockPanel
    {

        public ABCDockPanel ( )
        {
           
        }

        public static ABCDockPanel AddNewDockPanel ( ABCView view )
        {
            ABCDockPanel panel=new ABCDockPanel();
            panel.Dock=DevExpress.XtraBars.Docking.DockingStyle.Left;

            DevExpress.XtraBars.Docking.ControlContainer ctrl=new ControlContainer();
            ctrl.Dock=DockStyle.Fill;
            ctrl.Parent=panel;

            return panel;
        }

     
        #region Layout

        public static void GetChildrenXMLLayout ( ABCView ownView ,DockPanel panel ,XmlElement panelElement )
        {

            foreach ( Control ctrl in panel.Controls )
            {
                if ( ctrl is DockPanel )
                {
                    XmlElement childEle=ownView.ComponentSerialization( panelElement.OwnerDocument , ctrl );
                    panelElement.AppendChild( childEle );
                }
                else if ( ctrl is ControlContainer )
                {
                    #region Child
                    ComponentDesigner designer=(ComponentDesigner)ownView.Surface.DesignerHost.GetDesigner( ctrl );
                    if ( designer!=null&&designer.AssociatedComponents!=null )
                    {
                        List<XmlElement> lstTemp=new List<XmlElement>();
                        foreach ( object associatedComponent in designer.AssociatedComponents )
                        {
                            XmlElement eleChild=ownView.ComponentSerialization( panelElement.OwnerDocument , (IComponent)associatedComponent );
                            if ( eleChild!=null )
                                panelElement.AppendChild( eleChild );
                        }
                    }
                    #endregion
                }

            }
        }
        
        #region Load
        public void InitLayout ( ABCView view , XmlNode panelNode )
        {
            InitializeFromXmlNode( panelNode );

            foreach ( XmlNode node in panelNode.SelectNodes( "C" ) )
            {
                Component comp=ABCPresentHelper.LoadComponent( view , node );
                if ( comp!=null )
                {
                    if ( comp is DockPanel)
                        this.Controls.Add( (Control)comp );
                    else
                        this.ControlContainer.Controls.Add( (Control)comp );
                }
            }

     
            PerformDock( view );

        }
        public void InitializeFromXmlNode ( XmlNode node )
        {
            this.Name=node.Attributes["name"].Value.ToString();
            foreach ( XmlNode nodeChild in node.ChildNodes )
            {
                if ( nodeChild.Name=="P" )
                {
                    String strProName=nodeChild.Attributes["name"].Value.ToString();
                    String strValue=nodeChild.InnerText;
                    PropertyInfo proInfo=null;
                    try
                    {
                        proInfo=this.GetType().GetProperty( strProName );
                    }
                    catch ( Exception ee )
                    {
                        if ( strProName=="Dock" )
                            proInfo=this.GetType().GetProperty( strProName , typeof( DevExpress.XtraBars.Docking.DockingStyle ) );
                    }
                    if ( proInfo==null )
                        continue;

                    try
                    {
                        TypeConverter converter=TypeDescriptor.GetConverter( proInfo.PropertyType );
                        object obj=converter.ConvertFromString( strValue );
                        proInfo.SetValue( this , obj , null );
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            }
        }
        public void PerformDock ( ABCView view )
        {
            //if ( this.Tabbed )
            //    this.ActiveChild=(ABCDockPanel)this.Controls[0];
            ABCDockManager manager=ABCDockManager.GetDockManager( view );

            this.Register( manager );
            this.DockTo( this.Dock );

        }
        #endregion

        #endregion
    }

    public class ABCDockManager : DockManager
    {
        public ABCDockManager ( ContainerControl form )
            : base( form )
        {
        }
        public ABCDockManager ( IContainer container )
            : base( container )
        {
        }
        public ABCDockManager ( )
        {
            this.RegisterDockPanel+=new DockPanelEventHandler( ABCDockManager_RegisterDockPanel );
        }

        void ABCDockManager_RegisterDockPanel ( object sender , DockPanelEventArgs e )
        {
            if ( this.DesignMode )
            {
                foreach ( Component comp in this.Container.Components )
                {
                    if ( comp==e.Panel )
                        return;
                }
                this.Container.Add( e.Panel );

                if ( e.Panel.ControlContainer!=null )
                    this.Container.Add( e.Panel.ControlContainer );

            }
        }

        public XmlElement GetXMLLayout ( XmlDocument doc )
        {

            XmlElement elManager=ABCPresentHelper.Serialization( doc , this , "DCK" );

            foreach ( DockPanel ctrl in this.RootPanels )
            {
                XmlElement childEle=doc.CreateElement( "PL" );
                childEle.InnerText=ctrl.Name;
                elManager.AppendChild( childEle );
            }

            return elManager;
        }

        public static void InitDockManager (ABCView view, XmlDocument doc )
        {
            XmlNodeList nodeList=doc.GetElementsByTagName( "DCK" );
                if(nodeList.Count<=0)
                    return;

            if ( view.CurrentDockManager==null )
                view.CurrentDockManager=new ABCDockManager( view );


            foreach ( XmlNode node in nodeList[0].SelectNodes( "PL" ) )
            {
                foreach ( DockPanel panel in view.CurrentDockManager.Panels )
                    if ( panel.Name==node.InnerText )
                    {
                        view.CurrentDockManager.RootPanels.AddRange( new DockPanel[] { panel } );
                       panel.DockTo( panel.Dock );
                    }
            }

        }

        public static ABCDockManager GetDockManager (ABCView view )
        {
            if ( view.Mode==ViewMode.Design )
            {
                foreach ( Component comp in view.Surface.DesignerHost.Container.Components )
                {
                    if ( comp is ABCDockManager )
                    {
                        view.CurrentDockManager=(ABCDockManager)comp;
                        return (ABCDockManager)comp;
                    }

                }

                ABCDockManager manager=(ABCDockManager)view.Surface.DesignerHost.CreateComponent( typeof( ABCDockManager ) );
                manager.Form=view;
                view.CurrentDockManager=manager;
                return manager;

            }
            else
            {
                if ( view.CurrentDockManager!=null )
                    return view.CurrentDockManager;

                view.CurrentDockManager=new ABCDockManager( view );
                return view.CurrentDockManager;
            }

        }
    }
}
