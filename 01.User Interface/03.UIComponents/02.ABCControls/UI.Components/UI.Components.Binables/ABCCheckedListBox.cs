using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;


using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.CheckedListBoxControl ) )]
    [Designer( typeof( ABCCheckedListBoxDesigner ) )]
    public class ABCCheckedListBox : DevExpress.XtraEditors.CheckedListBoxControl , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "";
            }
        }
        #endregion

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
                 if(OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }
        #endregion

        public ABCCheckedListBox ( )
        {
        }

        #region IABCControl
        public void InitControl ( )
        {
            InitBothMode();

              if ( OwnerView!=null &&OwnerView.Mode==ViewMode.Design)
                InitDesignTime();
            else
                InitRunTime();
        }

        public void InitBothMode ( )
        {
            this.SelectionMode=System.Windows.Forms.SelectionMode.MultiExtended;
        }
        public void InitRunTime ( )
        {
        
        }
        public void InitDesignTime ( )
        {
        }
        
        #endregion

        public String GetPropertyBindingName ( )
        {
            return "";
        }

        #region Runtime Utils
        public List<System.Data.DataRow> GetCheckedObjects ( )
        {
            List<System.Data.DataRow> lstResult=new List<System.Data.DataRow>();
            foreach ( object item in this.CheckedItems)
            {
                System.Data.DataRowView view=(System.Data.DataRowView)item;
                lstResult.Add( ( (System.Data.DataRowView)item ).Row );
            }
            return lstResult;
        }
        #endregion

    }

    public class ABCCheckedListBoxDesigner : ControlDesigner
    {
        public ABCCheckedListBoxDesigner ( )
        {
        }
    }
}
