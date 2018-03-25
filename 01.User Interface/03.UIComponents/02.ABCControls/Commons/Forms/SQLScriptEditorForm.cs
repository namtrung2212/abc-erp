using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCProvider;
namespace ABCControls
{
    public partial class SQLScriptEditorForm : DevExpress.XtraEditors.XtraForm
    {

        public String SQLScript
        {
            get
            {
             return   this.SourceSQLEditor.Text;
            }
            set
            {
                this.SourceSQLEditor.Text=value;
            }
        }

        public SQLScriptEditorForm ( )
        {
            InitializeComponent();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.SourceSQLEditor.PreviewKeyDown+=new PreviewKeyDownEventHandler( SourceSQLEditor_PreviewKeyDown );
        }

      
        private void btnNext_Click ( object sender , EventArgs e )
        {
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        void SourceSQLEditor_PreviewKeyDown ( object sender , PreviewKeyDownEventArgs e )
        {
            if ( e.KeyCode==Keys.F5 )
                ExecuteScript();
        }

  
        public void ExecuteScript ( )
        {

            String stQuery=SourceSQLEditor.Selection.Text.Trim();
            if ( String.IsNullOrWhiteSpace( stQuery ) )
                stQuery=SourceSQLEditor.Text;

            if ( String.IsNullOrWhiteSpace( stQuery ) )
                return;

          
           ABCHelper.ABCWaitingDialog.Show( "" , "Executing . . .!" );

            DataSet ds=DataQueryProvider.RunQuery( stQuery );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                gridControl1.DataSource=ds.Tables[0];
                gridView1.PopulateColumns();
                gridControl1.RefreshDataSource();
                gridView1.BestFitColumns();
            }

           ABCHelper.ABCWaitingDialog.Close();

        }

    }
}