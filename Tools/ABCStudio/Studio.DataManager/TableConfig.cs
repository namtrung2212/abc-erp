using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ABCDataLib;
using ABCPresentLib;

namespace ABCStudio
{
    public partial class TableConfigScreen : DevExpress.XtraEditors.XtraUserControl
    {
        public TableConfigScreen ( )
        {
            InitializeComponent();

            this.Load+=new EventHandler( TableConfigScreen_Load );
    
        }

        void TableConfigScreen_Load ( object sender , EventArgs e )
        {
            Cursor.Current=Cursors.WaitCursor;
            SynchronizeTableConfig();

            InvalidateData();

            Cursor.Current=Cursors.Default;
        }

        private void InvalidateData ( )
        {
            DataSet ds=new STTableConfigController().GetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
                this.gridControl1.DataSource=ds.Tables[0];
        }

        public void SynchronizeTableConfig ( )
        {
          
            Dictionary<String,STTableConfigInfo> lstConfig = new Dictionary<String,STTableConfigInfo>();
        
            STTableConfigController aliasCtrl=new STTableConfigController();
            DataSet ds=aliasCtrl.GetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    STTableConfigInfo configInfo=(STTableConfigInfo)aliasCtrl.GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                        lstConfig.Add( configInfo.TableName ,configInfo);
                }
            }


            foreach ( String strTableName in ABCDataLib.Tables.StructureProvider.DataTablesList.Keys )
            {
                if ( lstConfig.ContainsKey( strTableName )==false )
                {
                    STTableConfigInfo newInfo=new STTableConfigInfo();
                    newInfo.TableName=strTableName;
                    newInfo.CaptionEN=strTableName;
                    newInfo.IsCaching=false;
                    aliasCtrl.CreateObject( newInfo );
                }
            }


        }

        private void btnSave_Click ( object sender , EventArgs e )
        {
            DevExpress.Utils.WaitDialogForm waiting=new DevExpress.Utils.WaitDialogForm();
            waiting.SetCaption( "Saving . . .!" );
            waiting.Text="";
            waiting.Show();
            Cursor.Current=Cursors.WaitCursor;
            STTableConfigController aliasCtrl=new STTableConfigController();

            foreach ( DataRow dr in ( (DataTable)this.gridControl1.DataSource ).Rows )
            {
                STTableConfigInfo aliasInfo=(STTableConfigInfo)aliasCtrl.GetObjectFromDataRow( dr );
                if ( aliasInfo!=null )
                    aliasCtrl.UpdateObject( aliasInfo );
            }

            ABCDataLib.Tables.ConfigProvider.InvalidateConfigList();
            InvalidateData();
            Cursor.Current=Cursors.Default;
            waiting.Close();
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.FindForm().DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.FindForm().Close();
        }

    }
}