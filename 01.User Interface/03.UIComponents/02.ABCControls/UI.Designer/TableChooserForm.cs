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
using ABCProvider;
using ABCProvider;

namespace ABCControls
{
    public partial class TableChooserForm : DevExpress.XtraEditors.XtraForm
    {
        bool IsAloneCheck=false;

        public List<String> TableNameList=new List<string>();

        public TableChooserForm ( )
        {
            InitializeComponent();

            foreach ( TableStructureData dt in DataStructureProvider.DataTablesList.Values )
                TableListCtrl.Items.Add( dt.TableName , CheckState.Unchecked );
            
            TableListCtrl.SortOrder=SortOrder.Ascending;
            
            this.TableListCtrl.ItemChecking+=new DevExpress.XtraEditors.Controls.ItemCheckingEventHandler( TableListCtrl_ItemChecking );
            chkAllTables.CheckedChanged+=new EventHandler( chkAllColumns_CheckedChanged );
        }

        int iChandingIndex=-1;
        void TableListCtrl_ItemChecking ( object sender , DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e )
        {

            if ( IsAloneCheck&&iChandingIndex <0)
            {
                iChandingIndex=e.Index;
                this.TableListCtrl.UnCheckAll();
                iChandingIndex=-1;
            }
        }
        void chkAllColumns_CheckedChanged ( object sender , EventArgs e )
        {
            if ( chkAllTables.Checked )
                this.TableListCtrl.CheckAll();
            else
                this.TableListCtrl.UnCheckAll();
        }

        public String ShowChooseOne (params String[] lstOldValue )
        {
            IsAloneCheck=true;
            chkAllTables.Visible=false;
            ShowChoose( lstOldValue );
            if(TableNameList.Count>0)
                return TableNameList[0];

            return String.Empty;
        }
        public List<String> ShowChoose ( params String[] lstOldValue )
        {
            TableNameList.Clear();
            foreach ( DevExpress.XtraEditors.Controls.CheckedListBoxItem item in TableListCtrl.Items )
            {
                if ( lstOldValue.Contains( item.Value.ToString() ) )
                    item.CheckState=CheckState.Checked;
            }
            this.ShowDialog();

            foreach ( DevExpress.XtraEditors.Controls.CheckedListBoxItem item in TableListCtrl.CheckedItems )
            {
                TableNameList.Add( item.Value.ToString() );
            }
            return TableNameList;
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