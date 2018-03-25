using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraEditors.Repository;

using ABCProvider;

namespace ABCCommonForms
{
    public partial class FilterBuilderForm : DevExpress.XtraEditors.XtraForm
    {
        String TableName=String.Empty;
        public String FilterString=String.Empty;

        public FilterBuilderForm ( String strTableName )
        {
            TableName=strTableName;
            InitializeComponent();

            InitColumnsFromTable();

     //      this.filterEditorControl1.FilterCriteria.
            this.FormClosing+=new FormClosingEventHandler( FilterBuilderForm_FormClosing );
        }

        public void SetFilterString ( string strFIlter )
        {
            this.filterEditorControl1.FilterString=strFIlter;
        }
        void FilterBuilderForm_FormClosing ( object sender , FormClosingEventArgs e )
        {
            if ( this.filterEditorControl1.FilterCriteria!=null )
            {
                //List<CriteriaOperator> lst=new List<CriteriaOperator>();
                //foreach ( CriteriaOperator op in ( this.filterEditorControl1.FilterCriteria as DevExpress.Data.Filtering.GroupOperator ).Operands )
                //{
                  
                //}

                FilterString=this.filterEditorControl1.FilterCriteria.LegacyToString();
            }
        }

        private void FilterBuilderForm_Load ( object sender , EventArgs e )
        {
        }

        public void InitColumnsFromTable ( )
        {
            foreach ( String strField in DataStructureProvider.DataTablesList[TableName].ColumnsList.Keys )
            {

                if ( !DataConfigProvider.TableConfigList[TableName].FieldConfigList.ContainsKey( strField ) )
                    continue;

                DataConfigProvider.FieldConfig config=DataConfigProvider.TableConfigList[TableName].FieldConfigList[strField];
                String strCaption=DataConfigProvider.GetFieldCaption( TableName , strField );

                if ( config.TypeName=="DateTime"||config.TypeName=="Nullable<DateTime>" )
                {
                    this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( DateTime ) , new RepositoryItemDateEdit() , FilterColumnClauseClass.DateTime ) );
                }
                if ( config.TypeName=="int"||config.TypeName=="Nullable<int>" )
                {
                    this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( int ) , new RepositoryItemSpinEdit() , FilterColumnClauseClass.Generic ) );
                }
                if ( config.TypeName=="Guid"||config.TypeName=="Nullable<Guid>" )
                {
                    if ( DataStructureProvider.IsForeignKey( TableName , strField ) )
                    {
                        String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( TableName , strField );
                        RepositoryItemLookUpEditBase repo=ABCControls.UICaching.GetDefaultRepository( strPKTableName,false );
                        repo.DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );

                        this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( Guid ) , repo , FilterColumnClauseClass.Lookup ) );
                    }
                }
                if ( config.TypeName=="String" )
                {
                    this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( String ) , new RepositoryItemTextEdit() , FilterColumnClauseClass.String ) );
                }
                if ( config.TypeName=="double" )
                {
                    this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( double ) , new RepositoryItemTextEdit() , FilterColumnClauseClass.Generic ) );
                }
                if ( config.TypeName=="bool"||config.TypeName=="Nullable<bool>" )
                {
                    this.filterEditorControl1.FilterColumns.Add( new UnboundFilterColumn( strCaption , strField , typeof( bool ) , new RepositoryItemCheckEdit() , FilterColumnClauseClass.Generic ) );
                }
            }
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

        private void filterEditorControl1_Click ( object sender , EventArgs e )
        {

        }
    }
}