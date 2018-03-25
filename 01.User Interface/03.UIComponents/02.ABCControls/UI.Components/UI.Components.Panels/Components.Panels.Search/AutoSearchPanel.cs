using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;

namespace ABCControls
{

    [Designer( typeof( AutoSearchPanelDesigner ) )]
    public partial class ABCAutoSearchPanel : ABCSearchPanel , IABCCustomControl
    {

        public override ScrollableControl DropZone
        {
            get { return flowLayoutPanel1; }
        }

        public bool ShowSearchButton
        {
            get {
                return this.btnSearch.Visible;
            }
            set
            {
                this.btnSearch.Visible=value;
            }
        }

        public ABCAutoSearchPanel ( )
        {
            InitializeComponent();
            this.Size=new Size( 472 , 128 );

            this.btnSearch.Image=ABCImageList.GetImage16x16( "SearchDoc" );
            this.btnSearch.Click+=new EventHandler( btnSearch_Click );
        }
      
        void btnSearch_Click ( object sender , EventArgs e )
        {
            OnSearch();
        }

    }

    public class AutoSearchPanelDesigner : ParentControlDesigner
    {
        ISelectionService ServiceSelection;
        ABCAutoSearchPanel panel;
        public override void Initialize ( System.ComponentModel.IComponent component )
        {
            base.Initialize( component );
            panel=(ABCAutoSearchPanel)this.Control;

            this.EnableDesignMode( panel.DropZone , "DropZone" );

          //  ServiceSelection=(ISelectionService)GetService( typeof( ISelectionService ) );
        //    ServiceSelection.SelectionChanged+=new EventHandler( OnSelectionChanged );
        }

        void OnSelectionChanged ( object sender , EventArgs e )
        {
            if ( ServiceSelection!=null&&ServiceSelection.PrimarySelection==this.panel.DropZone )
                ServiceSelection.SetSelectedComponents( new IComponent[] { this.panel } );
        }


        protected override void Dispose ( bool disposing )
        {
            ISelectionService s=(ISelectionService)GetService( typeof( ISelectionService ) );
            IComponentChangeService c=(IComponentChangeService)GetService( typeof( IComponentChangeService ) );

            // Unhook events
            s.SelectionChanged-=new EventHandler( OnSelectionChanged );

            base.Dispose( disposing );
        }

    }


}
