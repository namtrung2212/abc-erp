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
    public partial class GridBandedColumnConfigForm : DevExpress.XtraEditors.XtraForm
    {

        public BindingList<ABCGridBandedColumn.ColumnConfig> ColumnList;
        public BindingList<ABCGridBand.BandConfig> BandsList;
        public String Script;

        public String TableName;

        public GridBandedColumnConfigForm ( BindingList<ABCGridBandedColumn.ColumnConfig> listCol , BindingList<ABCGridBand.BandConfig> listBands )
        {
        
            InitializeComponent();
            this.Load+=new EventHandler( Form_Load );
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
                
            InitBands( listCol , listBands );

      }

    
        #region InitBands - Columns
        public void InitBands ( BindingList<ABCGridBandedColumn.ColumnConfig> listCol , BindingList<ABCGridBand.BandConfig> listBands )
        {
            ExploredColumns.Clear();

            BandsList=new BindingList<ABCGridBand.BandConfig>();
            ColumnList=new BindingList<ABCGridBandedColumn.ColumnConfig>();

            if ( listBands!=null )
            {
                foreach ( ABCGridBand.BandConfig config in listBands )
                    BandsList.Add( InitBand( listCol , config ) );
            }

            if ( listCol!=null )
            {
                foreach ( ABCGridBandedColumn.ColumnConfig config in listCol )
                {
                    if ( ExploredColumns.Contains( config )==false )
                        ColumnList.Add( config.Clone() as ABCGridBandedColumn.ColumnConfig );
                }
            }
        }

        List<ABCGridBandedColumn.ColumnConfig> ExploredColumns=new List<ABCGridBandedColumn.ColumnConfig>();
        public ABCGridBand.BandConfig InitBand (  BindingList<ABCGridBandedColumn.ColumnConfig>  inputCols,ABCGridBand.BandConfig inputBand )
        {
            ABCGridBand.BandConfig bandResult=(ABCGridBand.BandConfig)inputBand.Clone();
            bandResult.Children.Clear();
            bandResult.Columns.Clear();

            foreach ( ABCGridBand.BandConfig childInputBand in inputBand.Children )
                bandResult.Children.Add( InitBand( inputCols,childInputBand ) );

            foreach ( ABCGridBandedColumn.ColumnConfig childInputColumn in inputBand.Columns )
            {
                if ( inputCols.Contains( childInputColumn )==false )
                    continue;

                ABCGridBandedColumn.ColumnConfig newCol=(ABCGridBandedColumn.ColumnConfig)childInputColumn.Clone();
                bandResult.Columns.Add( newCol );
                ColumnList.Add( newCol );

                ExploredColumns.Add( childInputColumn );
            }

            return bandResult;
        }
        
        #endregion
       
        void Form_Load ( object sender , EventArgs e )
        {
            #region ColumnConfigGridCtrl

            this.ColumnConfigGridView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ConfigGridView_CellValueChanged );
            this.ColumnConfigGridView.ValidateRow+=new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler( ColumnConfigGridView_ValidateRow );
            this.ColumnConfigGridView.KeyDown+=new KeyEventHandler( ColumnConfigGridView_KeyDown );
            this.ColumnConfigGridView.InitNewRow+=new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler( ColumnConfigGridView_InitNewRow );
            this.ColumnConfigGridView.CustomRowCellEdit+=new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler( ColumnConfigGridView_CustomRowCellEdit );

            this.BandConfigGridView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( BandConfigGridView_CellValueChanged );
            this.BandConfigGridView.KeyDown+=new KeyEventHandler( BandConfigGridView_KeyDown );
            this.BandConfigGridView.ValidateRow+=new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler( BandConfigGridView_ValidateRow );


            this.ColumnConfigGridCtrl.DataSource=ColumnList;
       
       //     gridColRepoType.ColumnEdit=ABCPresentUtils.GetRepositoryFromEnum( typeof( ABCRepositoryType ) );
            gridColSumType.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( ABCSummaryType ) );
            gridColFixed.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( DevExpress.XtraGrid.Columns.FixedStyle ) );
            this.repoFieldNameChooser.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFieldNameChooser_ButtonClick );
            this.repoFilterStringEditor.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFilterStringEditor_ButtonClick );
            #endregion
          
            this.BandConfigGridCtrl.DataSource=BandsList;
            gridColBandFixed.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( DevExpress.XtraGrid.Columns.FixedStyle ) );
            this.DisplayGridView.BandWidthChanged+=new DevExpress.XtraGrid.Views.BandedGrid.BandEventHandler( DisplayGridView_BandWidthChanged );
            this.DisplayGridView.ColumnPositionChanged+=new EventHandler( DisplayGridView_ColumnPositionChanged );
            this.DisplayGridView.ColumnWidthChanged+=new DevExpress.XtraGrid.Views.Base.ColumnEventHandler( DisplayGridView_ColumnWidthChanged );

            RefreshDisplayGrid();

        }

        void ColumnConfigGridView_CustomRowCellEdit ( object sender , DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e )
        {
            if ( e.Column.FieldName=="FilterString" )
            {
                ABCGridBandedColumn.ColumnConfig config=this.ColumnConfigGridView.GetRow( this.ColumnConfigGridView.FocusedRowHandle ) as ABCGridBandedColumn.ColumnConfig;
                if ( config==null||DataStructureProvider.IsForeignKey( config.TableName , config.FieldName )==false )
                {
                    e.RepositoryItem=null;
                    return;
                }
            }
        }

        void repoFilterStringEditor_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            ABCGridBandedColumn.ColumnConfig config=this.ColumnConfigGridView.GetRow( this.ColumnConfigGridView.FocusedRowHandle ) as ABCGridBandedColumn.ColumnConfig;
            if ( config!=null&&DataStructureProvider.IsForeignKey( config.TableName , config.FieldName ) )
            {
                String strTableName=DataStructureProvider.GetTableNameOfForeignKey( config.TableName , config.FieldName );
                using ( ABCCommonForms.FilterBuilderForm form=new ABCCommonForms.FilterBuilderForm( strTableName ) )
                {
                    form.SetFilterString( config.FilterString );
                    if ( form.ShowDialog()==DialogResult.OK )
                        config.FilterString=form.FilterString;
                }
            }
        }

        #region Events

        void ColumnConfigGridView_InitNewRow ( object sender , DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e )
        {
            if ( String.IsNullOrWhiteSpace( this.TableName )==false )
            {
                ABCGridBandedColumn.ColumnConfig config=this.ColumnConfigGridView.GetRow( e.RowHandle ) as ABCGridBandedColumn.ColumnConfig;
                if ( config!=null )
                {
                    config.TableName=this.TableName;
                    config.IsUseAlias=true;
                }
            }
        }


        void BandConfigGridView_ValidateRow ( object sender , DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e )
        {
            RefreshDisplayGrid();
        }
        void ColumnConfigGridView_ValidateRow ( object sender , DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e )
        {
            RefreshDisplayGrid();
        }
        void ColumnConfigGridView_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete )
            {
                if ( this.ColumnConfigGridView.SelectedRowsCount>0 )
                {
                    if ( ABCHelper.ABCMessageBox.Show( "Delete column?" , "Confirmation" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                        return;

                    ABCGridBandedColumn.ColumnConfig config=(ABCGridBandedColumn.ColumnConfig)this.ColumnConfigGridView.GetRow( this.ColumnConfigGridView.FocusedRowHandle );
                    config=null;
                    this.ColumnConfigGridView.DeleteSelectedRows();
                    RefreshDisplayGrid();
                }
            }
        }
        void BandConfigGridView_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete )
            {
                if ( this.BandConfigGridView.SelectedRowsCount>0 )
                {
                    if ( ABCHelper.ABCMessageBox.Show( "Delete Band?" , "Confirmation" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                        return;

                    ABCGridBand.BandConfig config=(ABCGridBand.BandConfig)this.BandConfigGridView.GetRow( this.BandConfigGridView.FocusedRowHandle );
                    config=null;
                    this.BandConfigGridView.DeleteSelectedRows();
                    RefreshDisplayGrid();
                }
            }
        }
        void ConfigGridView_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            if ( e.Column.FieldName=="FieldName"||e.Column.FieldName=="IsUseAlias" )
            {

                ABCGridBandedColumn.ColumnConfig config=this.ColumnConfigGridView.GetRow( this.ColumnConfigGridView.FocusedRowHandle ) as ABCGridBandedColumn.ColumnConfig;
                if ( config!=null )
                {
                    if ( config.IsUseAlias )
                    {
                        String strCaption=String.Empty;
                        if ( config.FieldName.Contains( ":" ) )
                        {
                            DataCachingProvider.AccrossStructInfo structInfo=DataCachingProvider.GetAccrossStructInfo( config.TableName , config.FieldName );
                            if ( structInfo!=null )
                                strCaption=DataConfigProvider.GetFieldCaption( structInfo.TableName , structInfo.FieldName );
                        }
                        else
                        {
                            strCaption=DataConfigProvider.GetFieldCaption( config.TableName , config.FieldName );
                        }

                        if ( string.IsNullOrWhiteSpace( strCaption )==false )
                        {
                            if ( config.CustomizationCaption==config.Caption )
                                config.Caption=strCaption;

                            config.CustomizationCaption=strCaption;
                        }
                    }
                }
            }

            if ( e.RowHandle!=DevExpress.XtraGrid.GridControl.AutoFilterRowHandle&&e.RowHandle!=DevExpress.XtraGrid.GridControl.NewItemRowHandle )
                RefreshDisplayGrid();
        }

        void BandConfigGridView_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            RefreshDisplayGrid();
        }
        void repoFieldNameChooser_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            String strTableName=this.TableName;

            ABCGridBandedColumn.ColumnConfig config=this.ColumnConfigGridView.GetRow( this.ColumnConfigGridView.FocusedRowHandle ) as ABCGridBandedColumn.ColumnConfig;
            if ( config!=null )
                strTableName=config.TableName;

            if ( String.IsNullOrWhiteSpace( strTableName )==false )
            {
                String strOldFieldName=this.ColumnConfigGridView.GetFocusedDisplayText();
                FieldChooserEx chooser=new FieldChooserEx( strTableName , strOldFieldName );
                if ( chooser.ShowDialog()==System.Windows.Forms.DialogResult.OK )
                    ( sender as ButtonEdit ).Text=chooser.Result;
            }
        }

        private void btnSave_Click ( object sender , EventArgs e )
        {
            foreach ( ABCGridBand gridBand in this.DisplayGridView.Bands )
                UpdateBandConfigGrid( gridBand );

            foreach ( ABCGridBandedColumn gridCol in this.DisplayGridView.Columns )
                UpdateColumnConfigGrid( gridCol );

            SaveOnClosing();

            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }
        
        #endregion

        #region Refresh 2 ConfigGrids

        void DisplayGridView_ColumnWidthChanged ( object sender , DevExpress.XtraGrid.Views.Base.ColumnEventArgs e )
        {
            foreach ( ABCGridBand gridBand in this.DisplayGridView.Bands )
                UpdateBandConfigGrid( gridBand );
            foreach ( ABCGridBandedColumn gridCol in this.DisplayGridView.Columns )
                UpdateColumnConfigGrid( gridCol );
        }
        void DisplayGridView_ColumnPositionChanged ( object sender , EventArgs e )
        {
            ABCGridBandedColumn gridCol=sender as ABCGridBandedColumn;
            UpdateColumnConfigGrid( gridCol );

        }
        void DisplayGridView_BandWidthChanged ( object sender , DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs e )
        {
            foreach ( ABCGridBand gridBand in this.DisplayGridView.Bands )
                UpdateBandConfigGrid( gridBand );

            foreach ( ABCGridBandedColumn gridCol in this.DisplayGridView.Columns )
                UpdateColumnConfigGrid( gridCol );
        }
        public void UpdateColumnConfigGrid ( ABCGridBandedColumn gridCol )
        {
            gridCol.Config.VisibleIndex=gridCol.VisibleIndex;
            gridCol.Config.Visible=gridCol.Visible;
            gridCol.Config.Width=gridCol.Width;

            ColumnConfigGridCtrl.RefreshDataSource();
        }
        public void UpdateBandConfigGrid ( ABCGridBand gridBand )
        {
            gridBand.Config.Visible=gridBand.Visible;
            gridBand.Config.Width=gridBand.Width;

            BandConfigGridCtrl.RefreshDataSource();
        } 
        #endregion

        #region RefreshDisplayGrid

        public void RefreshDisplayGrid ( )
        {
            DisplayGridView.TableName=this.TableName;
            DisplayGridView.Columns.Clear();
            DisplayGridView.Bands.Clear();

            DisplayGridView.BandConfigs=this.BandsList;
            DisplayGridView.ColumnConfigs=this.ColumnList;
            DisplayGridView.LoadBands();

            #region Script
            if ( String.IsNullOrWhiteSpace( this.TableName ) )
            {
                if ( String.IsNullOrWhiteSpace( Script )==false )
                {
                    DataSet ds=DataQueryProvider.RunQuery( Script );
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


            if ( DataCachingProvider.LookupTables.ContainsKey( this.TableName ) )
            {
                this.DisplayGridCtrl.DataSource=DataCachingProvider.LookupTables[this.TableName];

            }
            else
            {
                ABCHelper.ConditionBuilder strBuilder=new ABCHelper.ConditionBuilder();
                strBuilder.Append( String.Format( @"SELECT TOP 5 * FROM {0}" , this.TableName ) );
                 if ( DataStructureProvider.IsExistABCStatus( this.TableName ) )
                    strBuilder.AddCondition( QueryGenerator.GenerateCondition( this.TableName , ABCCommon.ABCColumnType.ABCStatus ) );

                 strBuilder.Append( String.Format( @" ORDER BY {0} DESC" , DataStructureProvider.GetPrimaryKeyColumn( this.TableName ) ) );
            
                try
                {
                    DataSet ds=DataQueryProvider.RunQuery( strBuilder.ToString() );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        this.DisplayGridCtrl.DataSource=ds.Tables[0];
                }
                catch ( Exception ex )
                {

                }
            }

            this.DisplayGridCtrl.RefreshDataSource();
            if ( DisplayGridView.CustomizationForm==null||DisplayGridView.CustomizationForm.Visible==false )
                DisplayGridView.ShowCustomization();
        }
    
        #endregion

        public void SaveOnClosing ( )
        {
            this.ColumnList.Clear();
            foreach ( ABCGridBandedColumn col in this.DisplayGridView.Columns )
            {
                col.Config.ColIndex=col.ColIndex;
                col.Config.RowIndex=col.RowIndex;
                col.Config.VisibleIndex=col.VisibleIndex;
                this.ColumnList.Add( col.Config );
            }

            this.BandsList.Clear();
            foreach ( ABCGridBand band in this.DisplayGridView.Bands )
                this.BandsList.Add( GetBandConfig( band ) );
        }

        public ABCGridBand.BandConfig GetBandConfig ( ABCGridBand band )
        {
            ABCGridBand.BandConfig newBand=(ABCGridBand.BandConfig)band.Config.Clone();
            foreach ( ABCGridBand childBand in band.Children )
                newBand.Children.Add( GetBandConfig( childBand ) );

            foreach ( ABCGridBandedColumn col in band.Columns )
            {
                if ( this.ColumnList.Contains( col.Config ) )
                    newBand.Columns.Add(col.Config);
            }

            return newBand;
        }
    }

}