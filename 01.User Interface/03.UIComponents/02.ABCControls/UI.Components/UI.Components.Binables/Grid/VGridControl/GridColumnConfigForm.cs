using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ABCDataLib;

namespace ABCPresentLib
{
    public partial class GridColumnConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public BindingList<ABCGridColumn.ColumnConfig> ColumnList;
        public String Script;
        public String TableName;

        public GridColumnConfigForm ( BindingList<ABCGridColumn.ColumnConfig> list )
        {
            ColumnList=new BindingList<ABCGridColumn.ColumnConfig>();
            InitializeComponent();
            this.Load+=new EventHandler( Form_Load );
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
            if ( list!=null )
            {
                foreach ( ABCGridColumn.ColumnConfig config in list )
                    ColumnList.Add( config.Clone() as ABCGridColumn.ColumnConfig );
            }
        }

        void Form_Load ( object sender , EventArgs e )
        {

            this.ConfigGridView.KeyDown+=new KeyEventHandler( ConfigGridView_KeyDown );
            this.ConfigGridView.InitNewRow+=new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler( ConfigGridView_InitNewRow );
            this.ConfigGridView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ConfigGridView_CellValueChanged );
            this.ConfigGridView.CustomRowCellEdit+=new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler( ConfigGridView_CustomRowCellEdit );
            this.repoFieldNameChooser.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFieldNameChooser_ButtonClick );
            this.repoFilterStringEditor.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFilterStringEditor_ButtonClick );
   
            this.ConfigGridCtrl.DataSource=ColumnList;
            gridColRepoType.ColumnEdit=ABCPresentUtils.GetRepositoryFromEnum( typeof( ABCRepositoryType ) );
            gridColSumType.ColumnEdit=ABCPresentUtils.GetRepositoryFromEnum( typeof( ABCSummaryType ) );
            gridColFixed.ColumnEdit=ABCPresentUtils.GetRepositoryFromEnum( typeof( DevExpress.XtraGrid.Columns.FixedStyle ) );

            this.DisplayGridView.ColumnPositionChanged+=new EventHandler( DisplayGridView_ColumnPositionChanged );
            this.DisplayGridView.ColumnWidthChanged+=new DevExpress.XtraGrid.Views.Base.ColumnEventHandler( DisplayGridView_ColumnWidthChanged );
    
             RefreshDisplayGrid();

        }

        void ConfigGridView_CustomRowCellEdit ( object sender , DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e )
        {
            if ( e.Column.FieldName=="FilterString" )
            {
                 ABCGridColumn.ColumnConfig config=this.ConfigGridView.GetRow( this.ConfigGridView.FocusedRowHandle ) as ABCGridColumn.ColumnConfig;
                 if ( config==null||ABCDataLib.Tables.StructureProvider.IsForeignKey( config.TableName , config.FieldName )==false )
                 {
                     e.RepositoryItem=null;
                     return;
                 }
            }
        }

        void repoFilterStringEditor_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {

            ABCGridColumn.ColumnConfig config=this.ConfigGridView.GetRow( this.ConfigGridView.FocusedRowHandle ) as ABCGridColumn.ColumnConfig;
            if (config!=null&&ABCDataLib.Tables.StructureProvider.IsForeignKey( config.TableName , config.FieldName ) )
            {
                String strTableName=ABCDataLib.Tables.StructureProvider.GetTableNameOfForeignKey( config.TableName , config.FieldName );
                using ( ABCCommonForms.FilterBuilderForm form=new ABCCommonForms.FilterBuilderForm( strTableName ) )
                {
                    form.SetFilterString( config.FilterString );
                    if ( form.ShowDialog()==DialogResult.OK )
                        config.FilterString=form.FilterString;
                }
            }
        }

        #region ConfigGrid

        void ConfigGridView_InitNewRow ( object sender , DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e )
        {
            if ( String.IsNullOrWhiteSpace( this.TableName )==false )
            {
                ABCGridColumn.ColumnConfig config=this.ConfigGridView.GetRow( e.RowHandle ) as ABCGridColumn.ColumnConfig;
                if ( config!=null )
                    config.TableName=this.TableName;
            }
        }
        void ConfigGridView_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            RefreshDisplayGrid();
        }
        void ConfigGridView_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete )
            {
                if ( this.ConfigGridView.SelectedRowsCount>0 )
                {
                    if ( DevExpress.XtraEditors.XtraMessageBox.Show( "Delete row?" , "Confirmation" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                        return;

                    this.ConfigGridView.DeleteSelectedRows();
                    RefreshDisplayGrid();
                }
            }
        }
        void repoFieldNameChooser_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            String strTableName=this.TableName;

            ABCGridColumn.ColumnConfig config=this.ConfigGridView.GetRow( this.ConfigGridView.FocusedRowHandle ) as ABCGridColumn.ColumnConfig;
            if ( config!=null )
                strTableName=config.TableName;

            if ( String.IsNullOrWhiteSpace( strTableName )==false )
            {
                String strOldFieldName=this.ConfigGridView.GetFocusedDisplayText();
                FieldChooserEx chooser=new FieldChooserEx( strTableName , strOldFieldName );
                if ( chooser.ShowDialog()==System.Windows.Forms.DialogResult.OK )
                    this.ConfigGridView.SetFocusedRowCellValue( this.ConfigGridView.FocusedColumn , chooser.Result );
            }
        }
        
        #endregion

        #region DisplayGrid

        void DisplayGridView_ColumnWidthChanged ( object sender , DevExpress.XtraGrid.Views.Base.ColumnEventArgs e )
        {
            foreach ( ABCGridColumn gridCol in this.DisplayGridView.Columns )
                UpdateConfigGrid( gridCol );
        }
        void DisplayGridView_ColumnPositionChanged ( object sender , EventArgs e )
        {
            ABCGridColumn gridCol=sender as ABCGridColumn;
            UpdateConfigGrid( gridCol );

        }
        
        #endregion

        private void btnSave_Click ( object sender , EventArgs e )
        {
            foreach ( ABCGridColumn gridCol in this.DisplayGridView.Columns )
                UpdateConfigGrid( gridCol );

            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }

        public void UpdateConfigGrid ( ABCGridColumn gridCol )
        {
            gridCol.Config.VisibleIndex=gridCol.VisibleIndex;
            gridCol.Config.Visible=gridCol.Visible;
            gridCol.Config.Width=gridCol.Width;

            ConfigGridCtrl.RefreshDataSource();
        }
        public void RefreshDisplayGrid ( )
        {
            DisplayGridView.TableName=this.TableName;
            DisplayGridView.Columns.Clear();
            DisplayGridView.ColumnConfigs=this.ColumnList;
            DisplayGridView.InitColumns();

            #region Script
            if ( String.IsNullOrWhiteSpace( this.TableName ) )
            {
                if ( String.IsNullOrWhiteSpace( Script )==false )
                {
                    DataSet ds=ConnectionManager.DatabaseHelper.RunQuery( Script );
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        this.DisplayGridCtrl.DataSource=ds.Tables[0];
                        this.DisplayGridCtrl.RefreshDataSource();
                        DisplayGridView.ShowCustomization();
                    }
                }
                return;
            } 
            #endregion

            if ( ABCDataLib.Tables.CachingProvider.LookupTables.ContainsKey( this.TableName ) )
            {
                this.DisplayGridCtrl.DataSource=ABCDataLib.Tables.CachingProvider.LookupTables[this.TableName];

            }
            else
            {
                ConditionBuilder strBuilder=new ConditionBuilder();
                strBuilder.Append( String.Format( @"SELECT TOP 5 * FROM {0} " , this.TableName ) );
                if ( ABCDataLib.Tables.StructureProvider.IsExistABCStatus( this.TableName ) )
                    strBuilder.AddCondition( ABCDataLib.Generation.QueryGenerator.GenerateCondition( this.TableName , DataDefine.ColumnType.ABCStatus ) );
                strBuilder.Append( String.Format( @" ORDER BY {0} DESC" , ABCDataLib.Tables.StructureProvider.GetPrimaryKeyColumn( this.TableName ) ) );
              
                try
                {
                    DataSet ds=ConnectionManager.DatabaseHelper.RunQuery( strBuilder.ToString() );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        this.DisplayGridCtrl.DataSource=ds.Tables[0];
                }
                catch ( Exception ex )
                {

                }
            }

            this.DisplayGridCtrl.RefreshDataSource();
            DisplayGridView.ShowCustomization();
        }
    }
}