using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using System.Xml;


using ABCCommon;


namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraTab.XtraTabControl ) )]
    [Designer( typeof( ABCTabControlDesigner ) )]
    public class ABCTabControl : DevExpress.XtraTab.XtraTabControl , IABCControl , IABCCustomControl
    {
        public ABCView OwnerView { get; set; }

        #region External Properties
        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }
        #endregion

        protected override void OnTabPageAdded ( DevExpress.XtraTab.XtraTabPage page )
        {
            base.OnTabPageAdded( page );

            if ( this.OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
            {
                foreach ( Component comp in this.Container.Components )
                {
                    if ( comp==page )
                        return;
                }
                this.Container.Add( page );
            }

        }
        public ABCTabControl ( )
        {
        }

        #region IABCControl

        public void InitControl ( )
        {
        }
        
        #endregion


        #region TabControl

        public void InitLayout ( ABCView view , XmlNode node )
        {
            this.TabPages.Clear();

            foreach ( XmlNode nodePage in node.SelectNodes( "C" ) )
            {
                Component comp=ABCPresentHelper.LoadComponent( view , nodePage );
                if ( comp!=null&&comp is DevExpress.XtraTab.XtraTabPage )
                    this.TabPages.Add( (DevExpress.XtraTab.XtraTabPage)comp );
            }
        }

        #endregion
    }

    public class ABCTabControlDesigner :DevExpress.XtraTab.Design.XtraTabControlDesigner
    {
        public ABCTabControlDesigner ( )
        {
        }

    }
}
