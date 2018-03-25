using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;

using ABCCommon;


namespace ABCControls
{
    public class ABCSplitterContainer : DevExpress.XtraEditors.SplitContainerControl , IABCControl,IABCCustomControl
    {
        #region IABCControl

        public ABCView OwnerView { get; set; }
        public void InitControl ( )
        {
            
        }
        #endregion


        bool isVisible=true;
        [Category( "External" )]
        public Boolean IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible=value;
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design ) if ( OwnerView.Mode!=ViewMode.Design )
                        this.Visible=value;
            }
        }

        public  void InitLayout ( ABCView view , XmlNode node )
        {
            foreach ( XmlNode nodePanel in node.SelectNodes( "C" ) )
            {
                DevExpress.XtraEditors.SplitGroupPanel panel=null;
                String strPanelName=nodePanel.Attributes["name"].Value.ToString();
                if ( strPanelName.EndsWith("Panel1" ))
                    panel=this.Panel1;
                else if ( strPanelName.EndsWith( "Panel2" ) )
                    panel=this.Panel2;
                else
                    continue;


                foreach ( XmlNode nodeChild in nodePanel.ChildNodes )
                {
                    if ( nodeChild.Name=="P" )
                    {
                        if ( nodeChild.Attributes["name"].Value.ToString()=="Size" )
                        {
                            panel.Size=(Size)TypeDescriptor.GetConverter( typeof( Size ) ).ConvertFromString( nodeChild.InnerText );
                            panel.Width=panel.Size.Width;
                        }
                    }
                    else if ( nodeChild.Name=="C" )
                    {
                        Component comp=ABCPresentHelper.LoadComponent( view  , nodeChild );
                        if ( comp is Control )
                            ( (Control)comp ).Parent=panel;
                    }
                }
            }
        }
    }

    
}
