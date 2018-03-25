using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCControls;
using ABCProvider;
using ABCBusinessEntities;
using ABCProvider;
using ABCHelper;

namespace ABCStudio
{
    public partial class TableDesignScreen : DevExpress.XtraEditors.XtraUserControl
    {
        public BindingList<STTableConfigsInfo> TableList=new BindingList<STTableConfigsInfo>();

        public Studio OwnerStudio;

        bool isModified=false;

        public TableDesignScreen ( Studio studio )
        {
            OwnerStudio=studio;

            InitializeComponent();

            InvalidateTableList();
        }

        public void InitEvents ( )
        {
            this.FindForm().FormClosing+=new FormClosingEventHandler( Form_FormClosing );

            this.ViewTableConfig.FocusedRowChanged+=new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler( ViewTableConfig_FocusedRowChanged );
            this.ViewTableConfig.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ViewTableConfig_CellValueChanged );
    
            repoEnum=new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repoEnum.Items.AddRange( EnumProvider.EnumList.Keys );
            repoFilter=new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repoFilter.ButtonPressed+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repFilter_ButtonPressed );
            repoFormat=new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repoFormat.Items.AddRange(Enum.GetNames(typeof(DataFormatProvider.FieldFormat)) );

            this.ViewFieldConfig.CustomRowCellEdit+=new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler( ViewFieldConfig_CustomRowCellEdit );
            this.ViewFieldConfig.ShowingEditor+=new CancelEventHandler( ViewFieldConfig_ShowingEditor );
            this.ViewFieldConfig.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(ViewFieldConfig_CellValueChanged);
        }

     
        #region Events

        #region Table

        void ViewTableConfig_FocusedRowChanged ( object sender , DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e )
        {
            STTableConfigsInfo objTableConfig=null;

            if ( isNeedSaveField )
            {
                 objTableConfig=(STTableConfigsInfo)ViewTableConfig.GetRow( e.PrevFocusedRowHandle );
                if ( objTableConfig!=null&&String.IsNullOrWhiteSpace( objTableConfig.TableName )==false )
                {
                    DialogResult result=ABCHelper.ABCMessageBox.Show( String.Format( "Do you want to save the table '{0}' ?" , objTableConfig.TableName ) , "Message" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
                    if ( result==DialogResult.Yes )
                        SaveFieldConfig( objTableConfig.TableName );
                }
                isNeedSaveField=false;
            }

            objTableConfig=(STTableConfigsInfo)ViewTableConfig.GetRow( e.FocusedRowHandle );
            if ( objTableConfig==null||String.IsNullOrWhiteSpace( objTableConfig.TableName ) )
                return;

            DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT * FROM  STFieldConfigs WHERE TableName='{0}'" , objTableConfig.TableName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
                this.GridFieldConfig.DataSource=ds.Tables[0];
        }
        void ViewFieldConfig_ShowingEditor ( object sender , CancelEventArgs e )
        {
            if ( ViewFieldConfig.FocusedColumn.FieldName=="FilterString" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( ViewFieldConfig.FocusedRowHandle );
                if ( dr!=null )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                            return;
                    }
                }

                e.Cancel=true;
            }
            else if ( ViewFieldConfig.FocusedColumn.FieldName=="Enum" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( ViewFieldConfig.FocusedRowHandle );
                if ( dr!=null )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.DataTablesList.ContainsKey( configInfo.TableName ) )
                        {
                            if ( DataStructureProvider.GetCodingType( configInfo.TableName , configInfo.FieldName )=="String" )
                                return;
                        }
                    }
                }
                e.Cancel=true;
            }
        }
        void repFilter_ButtonPressed ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            DataRow dr=ViewFieldConfig.GetDataRow( ViewFieldConfig.FocusedRowHandle );
            if ( dr!=null )
            {
                STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                if ( configInfo!=null )
                {
                    if ( DataStructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                    {

                        String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( configInfo.TableName , configInfo.FieldName );

                        ABCCommonForms.FilterBuilderForm filterBuilder=new ABCCommonForms.FilterBuilderForm( strPKTableName );
                        String strOldValue=ViewFieldConfig.GetRowCellValue( ViewFieldConfig.FocusedRowHandle , "FilterString" ).ToString();
                        filterBuilder.SetFilterString( strOldValue );

                        if ( filterBuilder.ShowDialog()==DialogResult.OK )
                            ViewFieldConfig.SetRowCellValue( ViewFieldConfig.FocusedRowHandle , "FilterString" , filterBuilder.FilterString );
                    }
                }
            }
        }
        void ViewFieldConfig_CustomRowCellEdit ( object sender , DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e )
        {
            if ( e.Column.FieldName=="Format" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( e.RowHandle );
                if ( dr!=null )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.DataTablesList.ContainsKey( configInfo.TableName ) )
                        {
                            String strType= DataStructureProvider.GetCodingType( configInfo.TableName , configInfo.FieldName );
                            if ( strType=="DateTime"||strType=="Nullable<DataTime>"||strType=="int"||strType=="double"||strType=="decimal" )
                            {
                                e.RepositoryItem=repoFormat;
                                return;
                            }
                        }
                    }
                }
                e.RepositoryItem=null;
            }

            #region AssignedEnum
            if ( e.Column.FieldName=="AssignedEnum" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( e.RowHandle );
                if ( dr!=null )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.DataTablesList.ContainsKey( configInfo.TableName ) )
                        {
                            if ( DataStructureProvider.GetCodingType( configInfo.TableName , configInfo.FieldName )=="String" )
                            {
                                e.RepositoryItem=repoEnum;
                                return;
                            }
                        }
                    }
                }
                e.RepositoryItem=null;
            }
            #endregion

            if ( e.Column.FieldName=="FilterString" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( e.RowHandle );
                if ( dr!=null )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)new STFieldConfigsController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                        {
                            e.RepositoryItem=this.repoFilter;
                            return;

                        }
                    }
                }

                e.RepositoryItem=null;
            }

        }
        void ViewTableConfig_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            isNeedSaveTable=true;
        }
        #endregion

        #region Field
        bool isNeedSaveField=false;
    
        void ViewFieldConfig_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            isNeedSaveField=true;
        }
        
        #endregion


        void Form_FormClosing ( object sender , FormClosingEventArgs e )
        {
            if ( isNeedSaveField||isNeedSaveTable )
            {
                DialogResult result=ABCHelper.ABCMessageBox.Show( "Do you want to save ?" , "Message" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
                if ( result==DialogResult.Yes )
                {
                    STTableConfigsInfo objTableConfig=(STTableConfigsInfo)ViewTableConfig.GetRow( ViewTableConfig.FocusedRowHandle );
                    if ( objTableConfig!=null&&String.IsNullOrWhiteSpace( objTableConfig.TableName ) ==false)
                        SaveFieldConfig( objTableConfig.TableName );

                    SaveTables();
                }
            }

            if ( isModified )
            {
                DataConfigProvider.SynchronizeTableConfigs();
                foreach ( HostSurface surface in OwnerStudio.SurfaceManager.DesignSurfaces )
                    ( surface.DesignerHost.RootComponent as ABCView ).RefreshBindingControl();
            }
        }

        #endregion
       
        #region Tables

        bool isNeedSaveTable=false;

        private void InvalidateTableList ( )
        {
            Dictionary<String , STTableConfigsInfo> lstConfig=new Dictionary<String , STTableConfigsInfo>();

            STTableConfigsController aliasCtrl=new STTableConfigsController();
            DataSet ds=aliasCtrl.GetDataSetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    STTableConfigsInfo configInfo=(STTableConfigsInfo)aliasCtrl.GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( DataStructureProvider.DataTablesList.ContainsKey( configInfo.TableName )&&!lstConfig.ContainsKey(configInfo.TableName) )
                            lstConfig.Add( configInfo.TableName , configInfo );
                        else
                        {
                            aliasCtrl.DeleteObject( configInfo );
                        }
                    }
                }
            }


            foreach ( String strTableName in DataStructureProvider.DataTablesList.Keys )
            {
                if ( lstConfig.ContainsKey( strTableName )==false )
                {
                    STTableConfigsInfo newInfo=new STTableConfigsInfo();
                    newInfo.TableName=strTableName;
                    newInfo.CaptionEN=strTableName;
                    newInfo.IsCaching=false;
                    aliasCtrl.CreateObject( newInfo );

                    lstConfig.Add( newInfo.TableName , newInfo );
                }
            }

            TableList.Clear();
            foreach ( STTableConfigsInfo info in lstConfig.Values )
                TableList.Add( info );

            this.GridTableConfig.DataSource=TableList;
            this.GridTableConfig.RefreshDataSource();
        }

        public void SaveTables ( )
        {
            ABCWaitingDialog.Show( "" , "Saving . . .!" );

            if ( this.ViewFieldConfig.DataSource!=null )
            {
                STFieldConfigsController configCtrl=new STFieldConfigsController();
                foreach ( DataRow dr in ( (DataView)this.ViewFieldConfig.DataSource ).Table.Rows )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)configCtrl.GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        configCtrl.UpdateObject( configInfo );
                        isModified=true;
                    }
                }
            }
            STTableConfigsController aliasCtrl=new STTableConfigsController();
            foreach ( STTableConfigsInfo aliasInfo in TableList )
                aliasCtrl.UpdateObject( aliasInfo );

            DataConfigProvider.SynchronizeTableConfigs();
            InvalidateTableList();

            ABCWaitingDialog.Close();

            isNeedSaveTable=false;
        }
        #endregion

        #region Fields

        private void SaveFieldConfig ( String strTableName )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return;

            Cursor.Current=Cursors.WaitCursor;

            #region Save

            STFieldConfigsController configCtrl=new STFieldConfigsController();
            foreach ( DataRow dr in ( (DataView)this.ViewFieldConfig.DataSource ).Table.Rows )
            {
                STFieldConfigsInfo configInfo=(STFieldConfigsInfo)configCtrl.GetObjectFromDataRow( dr );
                if ( configInfo!=null )
                {
                    configCtrl.UpdateObject( configInfo );
                    isModified=true;
                }
            }

            #endregion
          
            Cursor.Current=Cursors.Default;
        }

        private void SetDefaultFieldCaptionFromDictionary ( String strTableName )
        {
            STFieldConfigsController fieldConfigCtrl=new STFieldConfigsController();
            STTableConfigsController tableConfigCtrl=new STTableConfigsController();
            STDictionarysController dictionaryCtrl=new STDictionarysController();
            DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT * FROM  STFieldConfigs WHERE TableName='{0}'" , strTableName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    STFieldConfigsInfo configInfo=(STFieldConfigsInfo)fieldConfigCtrl.GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        String strCaptionVN=configInfo.FieldName;
                        String strCaptionEN=configInfo.FieldName;

                        #region Dictionarys
                        DataSet ds2=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT * FROM  STDictionarys WHERE ('{0}' LIKE '%'+ KeyString+'%'  AND IsContain='TRUE') OR ('{0}' LIKE KeyString+'%' AND IsStartWith='TRUE') OR ('{0}' LIKE '%' +KeyString AND IsEndWith='TRUE') OR (KeyString='{0}' AND (IsContain='FALSE' OR IsContain IS NULL) AND (IsStartWith='FALSE' OR IsStartWith IS NULL) AND (IsEndWith='FALSE' OR IsEndWith IS NULL) )" , configInfo.FieldName ) );
                        if ( ds2!=null&&ds2.Tables.Count>0&&ds2.Tables[0].Rows.Count>0 )
                        {
                            STDictionarysInfo dictInfo=(STDictionarysInfo)dictionaryCtrl.GetObjectFromDataRow( ds2.Tables[0].Rows[0] );
                            if ( dictInfo!=null )
                            {
                                if ( String.IsNullOrWhiteSpace( dictInfo.TranslateVN )==false )
                                    strCaptionVN=dictInfo.TranslateVN;
                                if ( String.IsNullOrWhiteSpace( dictInfo.TranslateEN )==false )
                                    strCaptionEN=dictInfo.TranslateEN;
                            }
                        }
                        #endregion
                   
                      

                            if ( DataStructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                            {
                                String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( configInfo.TableName , configInfo.FieldName );

                                STTableConfigsInfo tableConfigInfo=(STTableConfigsInfo)tableConfigCtrl.GetObject( String.Format( "SELECT * FROM  STTableConfigs WHERE TableName='{0}' " , strPKTableName ) );
                                if (tableConfigInfo!=null && String.IsNullOrWhiteSpace( tableConfigInfo.CaptionVN )==false )
                                    strCaptionVN=tableConfigInfo.CaptionVN;
                                if ( tableConfigInfo!=null&&String.IsNullOrWhiteSpace( tableConfigInfo.CaptionEN )==false )
                                    strCaptionEN=tableConfigInfo.CaptionEN;
                                else
                                    strCaptionEN=strPKTableName.Substring( 2 , strPKTableName.Length-3 );

                            }
                            else if ( configInfo.FieldName.StartsWith( "FK_" ) )
                            {
                                String strPKTableName=DataStructureProvider.GetTableNameOfPrimaryKey( configInfo.FieldName.Substring( 3 , configInfo.FieldName.Length-3 ) );
                                if ( String.IsNullOrWhiteSpace( strPKTableName )==false )
                                {
                                    STTableConfigsInfo tableConfigInfo=(STTableConfigsInfo)tableConfigCtrl.GetObject( String.Format( "SELECT * FROM  STTableConfigs WHERE TableName='{0}' " , strPKTableName ) );
                                    if ( tableConfigInfo!=null&&String.IsNullOrWhiteSpace( tableConfigInfo.CaptionVN )==false )
                                        strCaptionVN=tableConfigInfo.CaptionVN;
                                    if ( tableConfigInfo!=null&&String.IsNullOrWhiteSpace( tableConfigInfo.CaptionEN )==false )
                                        strCaptionEN=tableConfigInfo.CaptionEN;
                                    else
                                        strCaptionEN=strPKTableName.Substring( 2 , strPKTableName.Length-3 );
                                }
                            }
                        
                       
                        //if ( String.IsNullOrWhiteSpace( strCaptionVN )==false||String.IsNullOrWhiteSpace( strCaptionEN )==false )
                        //{
                            if ( strCaptionVN!=configInfo.FieldName )
                                configInfo.CaptionVN=strCaptionVN;
                            else
                                configInfo.CaptionVN=String.Empty;
                            configInfo.CaptionEN=strCaptionEN;
                            fieldConfigCtrl.UpdateObject( configInfo );
                        //}
                    }
                }
            }
        }
        #endregion

        private void btnSave_Click ( object sender , EventArgs e )
        {
            SaveTables();
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.FindForm().DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.FindForm().Close();
        }

        private void simpleButton1_Click ( object sender , EventArgs e )
        {

            STTableConfigsInfo objTableConfig=(STTableConfigsInfo)ViewTableConfig.GetRow( ViewTableConfig.FocusedRowHandle );
            if ( objTableConfig==null||String.IsNullOrWhiteSpace( objTableConfig.TableName ) )
                return;

            SetDefaultFieldCaptionFromDictionary( objTableConfig.TableName );
            DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( String.Format( "SELECT * FROM  STFieldConfigs WHERE TableName='{0}'" , objTableConfig.TableName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
                this.GridFieldConfig.DataSource=ds.Tables[0];
        }

    
    }
}