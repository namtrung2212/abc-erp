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
    public partial class FieldConfigScreen : DevExpress.XtraEditors.XtraUserControl
    {
         
        public Studio OwnerStudio;
        public FieldConfigScreen ( Studio studio )
        {
            OwnerStudio=studio;
            InitializeComponent();

            this.Load+=new EventHandler( FieldConfigScreen_Load );
            this.ViewTableList.FocusedRowChanged+=new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler( ViewTableList_FocusedRowChanged );
            this.ViewFieldConfig.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ViewFieldConfig_CellValueChanged );

        }

        void FieldConfigScreen_FormClosing ( object sender , FormClosingEventArgs e )
        {
            if ( isNeedSave )
                SaveFieldConfig();

            if ( isModified )
            {
                ABCDataLib.Tables.ConfigProvider.InvalidateConfigList();
                foreach ( HostSurface surface in OwnerStudio.SurfaceManager.DesignSurfaces )
                    ( surface.DesignerHost.RootComponent as ABCView ).RefreshBindingControl();
            }
        }

        void FieldConfigScreen_Load ( object sender , EventArgs e )
        {
            BindingList<String> lstrBind=new BindingList<string>();
            foreach ( String strItem in ABCDataLib.Tables.StructureProvider.DataTablesList.Keys )
                lstrBind.Add( strItem );

            this.GridTableList.DataSource=lstrBind;
            this.GridTableList.DefaultView.PopulateColumns();

            this.FindForm().FormClosing+=new FormClosingEventHandler( FieldConfigScreen_FormClosing );


            ABCDataLib.ABCEnums.EnumProvider.GetAllEnums();
            this.repositoryItemComboBox1.Items.AddRange( ABCDataLib.ABCEnums.EnumProvider.EnumList.Keys );
            this.repositoryItemButtonEdit1.ButtonPressed+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repositoryItemButtonEdit1_ButtonPressed );
            this.ViewFieldConfig.CustomRowCellEdit+=new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler( ViewFieldConfig_CustomRowCellEdit );
            this.ViewFieldConfig.ShowingEditor+=new CancelEventHandler( ViewFieldConfig_ShowingEditor );
        }

        void ViewFieldConfig_ShowingEditor ( object sender , CancelEventArgs e )
        {
            if ( ViewFieldConfig.FocusedColumn.FieldName=="FilterString" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( ViewFieldConfig.FocusedRowHandle );
                if ( dr!=null )
                {
                    STFieldConfigInfo configInfo=(STFieldConfigInfo)new STFieldConfigController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( ABCDataLib.Tables.StructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
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
                     STFieldConfigInfo configInfo=(STFieldConfigInfo)new STFieldConfigController().GetObjectFromDataRow( dr );
                     if ( configInfo!=null )
                     {
                         if ( ABCDataLib.Tables.StructureProvider.DataTablesList.ContainsKey( configInfo.TableName ) )
                         {
                             if ( ABCDataLib.Tables.StructureProvider.GetCSharpVariableType( configInfo.TableName , configInfo.FieldName )=="String" )
                                 return;
                         }
                     }
                 }
                 e.Cancel=true;
            }
        }
        void repositoryItemButtonEdit1_ButtonPressed ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            DataRow dr=ViewFieldConfig.GetDataRow( ViewFieldConfig.FocusedRowHandle );
            if ( dr!=null )
            {
                STFieldConfigInfo configInfo=(STFieldConfigInfo)new STFieldConfigController().GetObjectFromDataRow( dr );
                if ( configInfo!=null )
                {
                    if ( ABCDataLib.Tables.StructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                    {
                     
                        String strPKTableName=ABCDataLib.Tables.StructureProvider.GetTableNameOfForeignKey( configInfo.TableName , configInfo.FieldName );

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
            #region AssignedEnum
            if ( e.Column.FieldName=="AssignedEnum" )
            {
                DataRow dr=ViewFieldConfig.GetDataRow( e.RowHandle );
                if ( dr!=null )
                {
                    STFieldConfigInfo configInfo=(STFieldConfigInfo)new STFieldConfigController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( ABCDataLib.Tables.StructureProvider.DataTablesList.ContainsKey( configInfo.TableName ) )
                        {
                            if ( ABCDataLib.Tables.StructureProvider.GetCSharpVariableType( configInfo.TableName , configInfo.FieldName )=="String" )
                            {
                                e.RepositoryItem=repositoryItemComboBox1;
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
                    STFieldConfigInfo configInfo=(STFieldConfigInfo)new STFieldConfigController().GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        if ( ABCDataLib.Tables.StructureProvider.IsForeignKey( configInfo.TableName , configInfo.FieldName ) )
                        {
                            e.RepositoryItem=this.repositoryItemButtonEdit1;
                            return;
                       
                        }
                    }
                }

                e.RepositoryItem=null;
            }

        }

        bool isNeedSave=false;
        bool isModified=false;
        void ViewFieldConfig_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            isNeedSave=true;
        }

        void ViewTableList_FocusedRowChanged ( object sender , DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e )
        {
            if ( isNeedSave )
                SaveFieldConfig();

            String strTableName=ViewTableList.GetFocusedDisplayText();
            if ( String.IsNullOrEmpty( strTableName ) )
                return;

            DataSet ds=ABCDataLib.ConnectionManager.DatabaseHelper.RunQuery( String.Format( "SELECT * FROM  STFieldConfig WHERE TableName='{0}'" , strTableName ) );
            if ( ds!=null&&ds.Tables.Count>0 )
                this.GridFieldConfig.DataSource=ds.Tables[0];

        }

        private void SaveFieldConfig ( )
        {
            String strTableName=ViewTableList.GetFocusedDisplayText();
            if ( String.IsNullOrEmpty( strTableName ) )
                return;

            Cursor.Current=Cursors.WaitCursor;

            #region Save
            DialogResult result=DevExpress.XtraEditors.XtraMessageBox.Show( String.Format( "Do you want to save FieldConfig of table '{0}' ?" , strTableName ) , "Message" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
            if ( result==DialogResult.Yes )
            {
               STFieldConfigController configCtrl=new STFieldConfigController();
                foreach ( DataRow dr in ( (DataView)this.ViewFieldConfig.DataSource ).Table.Rows )
                {
                    STFieldConfigInfo configInfo=(STFieldConfigInfo)configCtrl.GetObjectFromDataRow( dr );
                    if ( configInfo!=null )
                    {
                        configCtrl.UpdateObject( configInfo );
                        isModified=true;
                    }
                }
            }
            #endregion

            isNeedSave=false;
         
            Cursor.Current=Cursors.Default;
        }

   

    }
}