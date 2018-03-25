using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCBusinessEntities;
using ABCHelper;
namespace ABCStudio
{
    public partial class DictionaryDefineScreen : DevExpress.XtraEditors.XtraUserControl
    {
        public DictionaryDefineScreen ( )
        {
            InitializeComponent();
            this.Load+=new EventHandler( DictionaryDefine_Load );
        }

        void DictionaryDefine_Load ( object sender , EventArgs e )
        {
            InvalidateData();
        }

        private void InvalidateData ( )
        {
            STDictionarysController ctrl=new STDictionarysController();
            DataSet ds=ctrl.GetDataSetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
                this.gridControl1.DataSource=ds.Tables[0];
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.FindForm().DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.FindForm().Close();
        }

        private void btnSave_Click ( object sender , EventArgs e )
        {
            ABCWaitingDialog.Show( "" , "Saving . . .!" );

            STDictionarysController ctrl=new STDictionarysController();
            foreach ( DataRow dr in ( (DataTable)this.gridControl1.DataSource ).Rows )
            {
                STDictionarysInfo info=(STDictionarysInfo)ctrl.GetObjectFromDataRow( dr );
                if ( info!=null )
                {
                    if ( info.STDictionaryID!=Guid.Empty)
                        ctrl.UpdateObject( info );
                    else
                        ctrl.CreateObject( info );
                }
            }
            InvalidateData();
            ABCWaitingDialog.Close();
        }
    }
}
