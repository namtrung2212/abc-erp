using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;

using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;

namespace ABCControls
{

    public partial class UserChooserForm : DevExpress.XtraEditors.XtraForm
    {
        bool IsAloneCheck=false;
        List<String> UserChoosedList=new List<string>();

        public UserChooserForm ( )
        {
            InitializeComponent();
            LoadAllUsers();

            this.Shown+=new EventHandler( UserChooserForm_Shown );

            chkAll.CheckedChanged+=new EventHandler( chkAllColumns_CheckedChanged );

            this.gridView1.ValidatingEditor+=new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler( gridView1_ValidatingEditor );
        }

    
        #region LoadAllUsers
        void UserChooserForm_Shown ( object sender , EventArgs e )
        {
            this.gridControl1.DataSource=lstAllUsers;
            this.gridControl1.RefreshDataSource();
            this.colEmployee.SortOrder=DevExpress.Data.ColumnSortOrder.Ascending;
            this.gridView1.MoveLast();
        }

        BindingList<ABCUserInfo> lstAllUsers=new BindingList<ABCUserInfo>();
        public void LoadAllUsers ( )
        {
            lstAllUsers.Clear();
            List<ABCUserInfo> lstTemp=ABCUserProvider.GetAllUsers(false,false);
            foreach ( ABCUserInfo user in lstTemp )
                lstAllUsers.Add( user );

            this.gridControl1.DataSource=lstAllUsers;
            this.gridControl1.RefreshDataSource();
        }

        #endregion

        public ABCUserInfo ShowChooseOne ( params String[] lstOldValue )
        {
            IsAloneCheck=true;
            chkAll.Visible=false;
            List<ABCUserInfo> lstResults=ShowChoose( lstOldValue );
            if ( lstResults.Count>0 )
                return lstResults[0];

            return null;
        }
        public List<ABCUserInfo> ShowChoose ( params String[] lstOldValue )
        {
            UserChoosedList.Clear();
            UserChoosedList.AddRange( lstOldValue );

            foreach ( ABCUserInfo user in lstAllUsers )
            {
                if ( lstOldValue.Contains( user.User ) )
                    user.Select=true;
            }

            this.ShowDialog();


            if ( this.DialogResult==System.Windows.Forms.DialogResult.OK )
            {
                List<ABCUserInfo> lstResults=new List<ABCUserInfo>();
                foreach ( ABCUserInfo user in lstAllUsers )
                {
                    if ( user.Select )
                        lstResults.Add( user );
                }
                return lstResults;
            }
            return null;
        }

        #region Events

        private void btnNext_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;

        }
           
        void gridView1_ValidatingEditor ( object sender , DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if ( view.FocusedColumn.FieldName=="Select" )
            {
                if ( IsAloneCheck )
                {
                    CheckAll( false );
                    e.Value=true;
                }
            }
        }


        void chkAllColumns_CheckedChanged ( object sender , EventArgs e )
        {
            CheckAll( chkAll.Checked );
        }

        void CheckAll ( bool isCheck )
        {
            foreach ( ABCUserInfo user in lstAllUsers )
                user.Select=isCheck;

            this.gridControl1.RefreshDataSource();
        }

        #endregion

        public static ABCUserInfo ShowChooseOne ( List<String> lstOldValue )
        {
            UserChooserForm form=new UserChooserForm();
            return form.ShowChooseOne( lstOldValue.ToArray() );
        }
        public static List<ABCUserInfo> ShowChoose ( List<String> lstOldValue )
        {
            UserChooserForm form=new UserChooserForm();
            return form.ShowChoose( lstOldValue.ToArray() );
        }

    }
}