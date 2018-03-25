using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCProvider;
using ABCHelper;
using ABCBusinessEntities;
namespace ABCStudio
{
    public partial class EnumDefineScreen : DevExpress.XtraEditors.XtraUserControl
    {
        public EnumDefineScreen ( )
        {
            InitializeComponent();
            this.Load+=new EventHandler( EnumDefine_Load );
            
        }

        void EnumDefine_Load ( object sender , EventArgs e )
        {
            InvalidateData();
        }

        private void InvalidateData ( )
        {
            STEnumDefinesController ctrl=new STEnumDefinesController();
            DataSet ds=ctrl.GetDataSet( "SELECT * FROM STEnumDefines ORDER BY EnumName" );
            if ( ds!=null&&ds.Tables.Count>0 )
                this.gridControl1.DataSource=ds.Tables[0];
        }

        private void btnSave_Click ( object sender , EventArgs e )
        {
            ABCWaitingDialog.Show( "" , "Saving . . .!" );

            STEnumDefinesController ctrl=new STEnumDefinesController();
            foreach ( DataRow dr in ( (DataTable)this.gridControl1.DataSource ).Rows )
            {
                STEnumDefinesInfo info=(STEnumDefinesInfo)ctrl.GetObjectFromDataRow( dr );
                if ( info!=null )
                {
                    if ( info.STEnumDefineID!=Guid.Empty )
                        ctrl.UpdateObject( info );
                    else
                        ctrl.CreateObject( info );
                }
            }
            InvalidateData();
            EnumProvider.GetAllEnums();

            ABCWaitingDialog.Close();

        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.FindForm().DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.FindForm().Close();
        }
    }
}
