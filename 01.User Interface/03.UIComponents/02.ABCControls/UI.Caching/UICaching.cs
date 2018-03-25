using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;
using ABCProvider;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

using ABCProvider;

namespace ABCControls
{
    public class UICaching
    {
        public static void InitCachingPresentControls ( )
        {
            CachingGridViews.Clear();
            ReposityGridLookupEdits.Clear();
            ReposityLookupEdits.Clear();

            foreach ( String strTableName in DataConfigProvider.TableConfigList.Keys )
            {
                DataConfigProvider.TableConfig config=DataConfigProvider.TableConfigList[strTableName];
                if ( config.IsCaching&&DataStructureProvider.IsSystemTable( strTableName )==false )
                {
                    //RepositoryLookUpEdit
                    GetDefaultRepositoryLookUpEdit( strTableName,false );
                }
            }

            foreach ( String strTableName in DataConfigProvider.TableConfigList.Keys )
            {
                DataConfigProvider.TableConfig config=DataConfigProvider.TableConfigList[strTableName];
                if ( config.IsCaching )
                {
                    //GridView
                    GridView view=GetDefaultGridView( strTableName );

                    //RepositoryGridLookupEdit
                    RepositoryItemGridLookUpEdit repoLookupEdit=GetDefaultRepositoryGridLookupEdit( strTableName , false );
                    repoLookupEdit.View=view;
                }
            }
        }

        public static RepositoryItemLookUpEditBase GetDefaultRepository ( String strTableName , bool isCreateNew )
        {
            ABCRepositoryGridLookupEdit gridLookupEdit=GetDefaultRepositoryGridLookupEdit( strTableName , isCreateNew );
            if ( gridLookupEdit.View.Columns.Count<=2 )
                return GetDefaultRepositoryLookUpEdit( strTableName , isCreateNew );

            return gridLookupEdit;
        }

        #region Repository

        #region LookupEdits
        public static Dictionary<String , ABCRepositoryLookUpEdit> ReposityLookupEdits=new Dictionary<string , ABCRepositoryLookUpEdit>();
        public static ABCRepositoryLookUpEdit GetDefaultRepositoryLookUpEdit ( String strTableName , bool isCreateNew )
        {
            ABCRepositoryLookUpEdit repo;
            if ( isCreateNew )
            {
                repo=new ABCRepositoryLookUpEdit();
                InitDefaultRepositoryLookupEdit( strTableName , repo );
                return repo;
            }
            
            if ( ReposityLookupEdits.TryGetValue( strTableName , out repo ) )
                return repo;

            repo=new ABCRepositoryLookUpEdit();
            InitDefaultRepositoryLookupEdit( strTableName , repo );
            ReposityLookupEdits.Add( strTableName , repo );

            return repo;
        }
        public static void InitDefaultRepositoryLookupEdit ( String strTableName , ABCRepositoryLookUpEdit repo )
        {
            DataConfigProvider.TableConfig config=DataConfigProvider.TableConfigList[strTableName];
            
            #region Columns
            foreach ( String strField in config.FieldConfigList.Keys )
            {
                if ( config.FieldConfigList[strField].InUse&&config.FieldConfigList[strField].IsDefault )
                {
                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col=new DevExpress.XtraEditors.Controls.LookUpColumnInfo();
                    col.FieldName=strField;
                    col.Caption=DataConfigProvider.GetFieldCaption( strTableName , strField );
                    col.Visible=true;
                    repo.Columns.Add( col );
                }
            }
            #endregion

            repo.ValueMember=DataStructureProvider.DataTablesList[strTableName].PrimaryColumn;

            repo.DisplayMember=DataStructureProvider.GetDisplayColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( repo.DisplayMember )&&repo.Columns.Count>0 )
                repo.DisplayMember=repo.Columns[0].FieldName;

            repo.BestFit();
        }
        #endregion

        #region GridLookUpEdit

        public static Dictionary<String , ABCRepositoryGridLookupEdit> ReposityGridLookupEdits=new Dictionary<string , ABCRepositoryGridLookupEdit>();
        public static ABCRepositoryGridLookupEdit GetDefaultRepositoryGridLookupEdit ( String strTableName , bool isCreateNew )
        {
            ABCRepositoryGridLookupEdit repo;
            if ( isCreateNew )
            {
                repo=new ABCRepositoryGridLookupEdit();
                InitDefaultRepositoryGridLookUpEdit( strTableName , repo , false );
                return repo;
            }

            if ( ReposityGridLookupEdits.TryGetValue( strTableName , out repo ) )
                return repo;

            repo=new ABCRepositoryGridLookupEdit();
            InitDefaultRepositoryGridLookUpEdit( strTableName , repo , false );
            ReposityGridLookupEdits.Add( strTableName , repo );
            return repo;
        }
        public static void InitDefaultRepositoryGridLookUpEdit ( String strTableName , ABCRepositoryGridLookupEdit repo , Boolean isCreateNewView )
        {
            ABCGridView view;
            if ( isCreateNewView )
            {
                view=new ABCGridView( strTableName );
                InitDefaultColumns( strTableName , view );
            }
            else
            {
                view=GetDefaultGridView( strTableName );
            }

            repo.View=view;
            repo.ValueMember=DataStructureProvider.GetPrimaryKeyColumn( strTableName );

            String strDisplayCol=DataStructureProvider.GetDisplayColumn( strTableName );
            if ( String.IsNullOrWhiteSpace( strDisplayCol ) )
                strDisplayCol=repo.ValueMember;
            repo.DisplayMember=strDisplayCol;

            if ( view.Columns.Count<=7 )
                repo.BestFitMode=DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            else
            {
                repo.BestFitMode=DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
                repo.PopupFormSize=new System.Drawing.Size( 700 , 400 );
            }

        }

        #region GridView  for GridLookupEdit
        public static Dictionary<String , ABCGridView> CachingGridViews=new Dictionary<string , ABCGridView>();
        public static ABCGridView GetDefaultGridView ( String strTableName )
        {
            ABCGridView view;
            if ( CachingGridViews.TryGetValue( strTableName , out view ) )
                return view;

            view=new ABCGridView( strTableName );
            InitDefaultColumns( strTableName , view );
            CachingGridViews.Add( strTableName , view );

            return view;
        }

        public static void InitDefaultColumns ( String strTableName , ABCGridView view )
        {
            DataConfigProvider.TableConfig config=DataConfigProvider.TableConfigList[strTableName];

            SortedDictionary<String , ABCGridColumn> sortedListCols=new SortedDictionary<String , ABCGridColumn>();

            foreach ( String strField in config.FieldConfigList.Keys )
            {
                if ( config.FieldConfigList[strField].InUse&&config.FieldConfigList[strField].IsDefault )
                {
                    ABCGridColumn gridCol=new ABCGridColumn();
                    gridCol.Name=strField;
                    gridCol.TableName=strTableName;
                    gridCol.FieldName=strField;
                    gridCol.Caption=DataConfigProvider.GetFieldCaption( strTableName , strField );
                    gridCol.Visible=true;
                    gridCol.VisibleIndex=config.FieldConfigList[strField].SortOrder;

                    InitDefaultRepository( gridCol );

                    gridCol.InitFormat();

                    if ( sortedListCols.ContainsKey( config.FieldConfigList[strField].SortOrder.ToString() )==false )
                        sortedListCols.Add( config.FieldConfigList[strField].SortOrder.ToString() , gridCol );
                    else
                        sortedListCols.Add( config.FieldConfigList[strField].SortOrder.ToString()+strField , gridCol );

                }
            }
            foreach ( ABCGridColumn gridCol in sortedListCols.Values )
                view.Columns.Add( gridCol );

            foreach ( ABCGridColumn gridCol in sortedListCols.Values )
            {
                if ( config.FieldConfigList.ContainsKey( gridCol.FieldName )&&config.FieldConfigList[gridCol.FieldName].IsGrouping )
                {
                    if ( gridCol.FieldName!=null&&gridCol.FieldName.Contains( ":" ) )
                        gridCol.GroupInterval=ColumnGroupInterval.DisplayText;
                    gridCol.Group();
                }
            }

        }

        public static void InitDefaultRepository ( ABCGridColumn gridCol )
        {
            try
            {
                #region RepositoryLookupEdit
                if ( DataStructureProvider.IsForeignKey( gridCol.TableName , gridCol.FieldName ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( gridCol.TableName , gridCol.FieldName );
                    gridCol.ColumnEdit=GetDefaultRepositoryLookUpEdit( strPKTableName,false );
                    ( gridCol.ColumnEdit as ABCRepositoryLookUpEdit ).DataSource=DataCachingProvider.TryToGetDataView( strPKTableName , false );

                }
                #endregion

                #region Enum
                else if ( DataStructureProvider.IsTableColumn( gridCol.TableName , gridCol.FieldName )
                       &&String.IsNullOrWhiteSpace( DataConfigProvider.TableConfigList[gridCol.TableName].FieldConfigList[gridCol.FieldName].AssignedEnum )==false )
                {
                    String strEnum=DataConfigProvider.TableConfigList[gridCol.TableName].FieldConfigList[gridCol.FieldName].AssignedEnum;
                    if ( EnumProvider.EnumList.ContainsKey( strEnum ) )
                    {
                        ABCRepositoryLookUpEdit repo=new ABCRepositoryLookUpEdit();

                        repo.Properties.ValueMember="ItemName";
                        if ( ABCApp.ABCDataGlobal.Language=="VN" )
                            repo.Properties.DisplayMember="CaptionVN";
                        else
                            repo.Properties.DisplayMember="CaptionEN";

                        DevExpress.XtraEditors.Controls.LookUpColumnInfo col=new DevExpress.XtraEditors.Controls.LookUpColumnInfo();
                        col.FieldName=repo.Properties.DisplayMember;
                        col.Caption=DataConfigProvider.GetFieldCaption( gridCol.TableName , gridCol.FieldName );
                        col.Visible=true;
                        repo.Properties.Columns.Add( col );
                        repo.Properties.DataSource=EnumProvider.EnumList[strEnum].Items.Values.ToArray();
                        gridCol.ColumnEdit=repo;
                    }

                }
                #endregion


            }
            catch ( Exception ex )
            {

            }
        }

        #endregion

        #endregion

        #endregion

        public static void AssignEnums ( GridColumn gridCol , String strTableName , String strFieldName,String strFilterString )
        {
            if ( DataStructureProvider.IsTableColumn( strTableName , strFieldName )
                                 &&String.IsNullOrWhiteSpace( DataConfigProvider.TableConfigList[strTableName].FieldConfigList[strFieldName].AssignedEnum )==false )
            {
                String strEnum=DataConfigProvider.TableConfigList[strTableName].FieldConfigList[strFieldName].AssignedEnum;
                if ( EnumProvider.EnumList.ContainsKey( strEnum ) )
                {
                    ABCRepositoryLookUpEdit repo=new ABCRepositoryLookUpEdit();

                    repo.Properties.ValueMember="ItemName";
                    if ( ABCApp.ABCDataGlobal.Language=="VN" )
                        repo.Properties.DisplayMember="CaptionVN";
                    else
                        repo.Properties.DisplayMember="CaptionEN";

                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col=new DevExpress.XtraEditors.Controls.LookUpColumnInfo();
                    col.FieldName=repo.Properties.DisplayMember;
                    col.Caption=DataConfigProvider.GetFieldCaption( strTableName , strFieldName );
                    col.Visible=true;
                    repo.Properties.Columns.Add( col );
                    if ( !String.IsNullOrWhiteSpace( strFilterString ) )
                    {
                        List<String> lstTemps=strFilterString.Split( ';' ).ToList();
                        repo.Properties.DataSource=EnumProvider.EnumList[strEnum].Items.Values.Where( e => lstTemps.Contains( e.ItemName ) ).ToArray();
                    }
                    else
                    {
                        repo.Properties.DataSource=EnumProvider.EnumList[strEnum].Items.Values.ToArray();

                    }
                    gridCol.ColumnEdit=repo;
                }
            }
        }

    }
}
