using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCProvider;

using ABCCommon;

namespace ABCControls
{
    public partial class PivotGridFieldConfigForm : DevExpress.XtraEditors.XtraForm
    {

        public BindingList<ABCPivotGridField.FieldConfig> FieldsList;
        public int RowTreeWidth;
        public bool UseChartControl;
        public String Script;

        public String TableName;

        public PivotGridFieldConfigForm ( BindingList<ABCPivotGridField.FieldConfig> list )
        {
            FieldsList=new BindingList<ABCPivotGridField.FieldConfig>();
            InitializeComponent();
            this.Load+=new EventHandler( Form_Load );
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;

            if ( list!=null )
            {
                foreach ( ABCPivotGridField.FieldConfig config in list )
                    FieldsList.Add( config.Clone() as ABCPivotGridField.FieldConfig );
            }

        }
        void Form_Load ( object sender , EventArgs e )
        {
            this.ConfigGridView.KeyDown+=new KeyEventHandler( ConfigGridView_KeyDown );
            this.ConfigGridView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ConfigGridView_CellValueChanged );
            this.ConfigGridView.InitNewRow+=new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler( ConfigGridView_InitNewRow );

            this.ConfigGridCtrl.DataSource=FieldsList;
            gridColRepoType.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( ABCRepositoryType ) );

            this.DisplayGridCtrl.Grid.FieldPropertyChanged+=new DevExpress.XtraPivotGrid.PivotFieldPropertyChangedEventHandler( DisplayGridCtrl_FieldPropertyChanged );
      
            this.repoFieldNameChooser.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFieldNameChooser_ButtonClick );
            RefreshDisplayGrid();

        }

        #region ConfigGrid
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
                    if ( ABCHelper.ABCMessageBox.Show( "Delete row?" , "Confirmation" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                        return;

                    this.ConfigGridView.DeleteSelectedRows();
                    RefreshDisplayGrid();
                }
            }
        }
        void ConfigGridView_InitNewRow ( object sender , DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e )
        {
            if ( String.IsNullOrWhiteSpace( this.TableName )==false )
            {
                ABCPivotGridField.FieldConfig config=this.ConfigGridView.GetRow( e.RowHandle ) as ABCPivotGridField.FieldConfig;
                if ( config!=null )
                    config.TableName=this.TableName;
            }
        }
        void repoFieldNameChooser_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            ABCPivotGridField.FieldConfig config=this.ConfigGridView.GetRow( this.ConfigGridView.FocusedRowHandle ) as ABCPivotGridField.FieldConfig;
            if ( config!=null )
            {

                String strOldFieldName=this.ConfigGridView.GetFocusedDisplayText();
                FieldChooserEx chooser=new FieldChooserEx( config.TableName , strOldFieldName );
                if ( chooser.ShowDialog()==System.Windows.Forms.DialogResult.OK )
                    this.ConfigGridView.SetFocusedRowCellValue( this.ConfigGridView.FocusedColumn , chooser.Result );
            }
        }
        #endregion

        #region DisplayGrid
        void DisplayGridCtrl_FieldPropertyChanged ( object sender , DevExpress.XtraPivotGrid.PivotFieldPropertyChangedEventArgs e )
        {
            RefreshDisplayGrid();
        }
        void DisplayGridView_ColumnWidthChanged ( object sender , DevExpress.XtraGrid.Views.Base.ColumnEventArgs e )
        {
            foreach ( ABCPivotGridField gridCol in this.DisplayGridCtrl.Grid.Fields )
                UpdateConfigGrid( gridCol );
        }
        void DisplayGridView_ColumnPositionChanged ( object sender , EventArgs e )
        {
            ABCPivotGridField gridCol=sender as ABCPivotGridField;
            UpdateConfigGrid( gridCol );

        }
        #endregion

        private void btnSave_Click ( object sender , EventArgs e )
        {
            foreach ( ABCPivotGridField gridCol in this.DisplayGridCtrl.Grid.Fields )
                UpdateConfigGrid( gridCol );

            RowTreeWidth=DisplayGridCtrl.Grid.OptionsView.RowTreeWidth;
            UseChartControl=DisplayGridCtrl.UseChartControl;
            Script=DisplayGridCtrl.Script;

            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }

        public void UpdateConfigGrid ( ABCPivotGridField gridCol )
        {
            gridCol.Config.Area=gridCol.Area;
            gridCol.Config.AreaIndex=gridCol.AreaIndex;
            gridCol.Config.Visible=gridCol.Visible;
            gridCol.Config.Width=gridCol.Width;
            gridCol.Config.Index=gridCol.Index;

            ConfigGridCtrl.RefreshDataSource();
        }
        public void RefreshDisplayGrid ( )
        {
            DisplayGridCtrl.TableName=this.TableName;
            DisplayGridCtrl.Grid.Fields.Clear();
            DisplayGridCtrl.FieldConfigs=this.FieldsList;
            DisplayGridCtrl.InitFields();
            DisplayGridCtrl.Grid.OptionsView.RowTreeWidth=RowTreeWidth;
            DisplayGridCtrl.UseChartControl=UseChartControl;
            DisplayGridCtrl.Script=Script;

            if ( String.IsNullOrWhiteSpace( this.TableName ) )
            {
                DisplayGridCtrl.LoadDataSourceFromScript();
                return;
            }
            if ( DataCachingProvider.LookupTables.ContainsKey( this.TableName ) )
            {
                this.DisplayGridCtrl.GridDataSource=DataCachingProvider.LookupTables[this.TableName];
      
            }
            else
            {
                ABCHelper.ConditionBuilder strBuilder=new ABCHelper.ConditionBuilder();
                strBuilder.Append( String.Format( @"SELECT TOP 5 * FROM {0}" , this.TableName) );
              

                if ( DataStructureProvider.IsExistABCStatus( this.TableName ) )
                    strBuilder.AddCondition( QueryGenerator.GenerateCondition( this.TableName , ABCCommon.ABCColumnType.ABCStatus ) );
              
                strBuilder.Append( String.Format( @" ORDER BY {0} DESC" ,  DataStructureProvider.GetPrimaryKeyColumn( this.TableName ) ) );
             
                try
                {
                    DataSet ds= DataQueryProvider.RunQuery( strBuilder.ToString() );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        this.DisplayGridCtrl.GridDataSource=ds.Tables[0];
                }
                catch ( Exception ex )
                {

                }
            }

            this.DisplayGridCtrl.RefreshDataSource();
            DisplayGridCtrl.Grid.FieldsCustomization(splitContainerControl2.Panel2);
        }

    }
}