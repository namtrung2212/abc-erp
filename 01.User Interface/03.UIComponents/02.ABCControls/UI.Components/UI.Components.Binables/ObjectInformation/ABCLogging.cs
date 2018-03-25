using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;

using ABCBusinessEntities;
using ABCProvider;


namespace ABCControls
{
    public partial class ABCLogging : DevExpress.XtraEditors.XtraUserControl
    {
        public ABCLogging ( )
        {
            InitializeComponent();

            UICaching.AssignEnums( this.colAction , "GEActionLogs" , "Action" ,String.Empty);
            this.gridView1.OptionsView.RowAutoHeight=true;
            this.gridView1.OptionsBehavior.Editable=false;
         
            this.gridView1.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedRow=false;
        }

        public void LoadLogs ( String strTableName , Guid iID )
        {
            if ( iID==Guid.Empty )
                return;

            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT * FROM GEActionLogs WHERE TableName ='{0}' AND ID ='{1}' AND ID IS NOT NULL ORDER BY Time" , strTableName , iID ) );
            if ( ds!=null&&ds.Tables.Count>0 )
                this.gridControl1.DataSource=ds.Tables[0];
            else
                this.gridControl1.DataSource=null;

            this.gridControl1.RefreshDataSource();
            this.gridView1.MoveLast();
        }
    }
}
