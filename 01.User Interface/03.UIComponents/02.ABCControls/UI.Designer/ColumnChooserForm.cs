using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;


using ABCProvider;

namespace ABCControls
{
    public partial class ColumnChooserForm : DevExpress.XtraEditors.XtraForm
    {
        bool IsAloneCheck=false;


        public List<String> ColumnNameList=new List<string>();

        public ColumnChooserForm ( String strTableName)
        {
            InitializeComponent();

            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return;

            foreach ( String strColName in DataStructureProvider.GetAllTableColumns( strTableName ).Keys )
                ColumnListCtrl.Items.Add( strColName , CheckState.Unchecked );

            ColumnListCtrl.ItemChecking+=new DevExpress.XtraEditors.Controls.ItemCheckingEventHandler( ColumnListCtrl_ItemChecking );
            chkAllColumns.CheckedChanged+=new EventHandler( chkAllColumns_CheckedChanged );
        }

        int iChandingIndex=-1;
        void ColumnListCtrl_ItemChecking ( object sender , DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e )
        {

            if ( IsAloneCheck&&iChandingIndex<0 )
            {
                iChandingIndex=e.Index;
                this.ColumnListCtrl.UnCheckAll();
                iChandingIndex=-1;
            }
        }
        void chkAllColumns_CheckedChanged ( object sender , EventArgs e )
        {
            if ( chkAllColumns.Checked )
                this.ColumnListCtrl.CheckAll();
            else
                this.ColumnListCtrl.UnCheckAll();
        }

        public String ShowChooseOne ( params String[] lstOldValue )
        {
            IsAloneCheck=true;
            chkAllColumns.Visible=false;
            ShowChoose( lstOldValue );
            if(ColumnNameList.Count>0)
                return ColumnNameList[0];

            return String.Empty;
        }
        public List<String> ShowChoose ( params String[] lstOldValue )
        {
            ColumnNameList.Clear();

            foreach ( DevExpress.XtraEditors.Controls.CheckedListBoxItem item in ColumnListCtrl.Items )
            {
                if ( lstOldValue.Contains( item.Value.ToString() ) )
                    item.CheckState=CheckState.Checked;
            }

            this.ShowDialog();

            foreach ( DevExpress.XtraEditors.Controls.CheckedListBoxItem item in ColumnListCtrl.CheckedItems )
                ColumnNameList.Add( item.Value.ToString() );

            return ColumnNameList;
        }
        private void btnSave_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }
    }
}