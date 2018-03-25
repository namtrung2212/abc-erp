using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;

using ABCProvider;
using ABCCommon;

namespace ABCControls
{
    public partial class TreeColumnConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public BindingList<ABCTreeListColumn.ColumnConfig> ColumnList;
        public ABCTreelistManager Manager;
        public String Script;
        public String TableName;

        public TreeColumnConfigForm ( BindingList<ABCTreeListColumn.ColumnConfig> list ,TreeConfigNode rootConfig)
        {
            InitializeComponent();

            ColumnList=new BindingList<ABCTreeListColumn.ColumnConfig>();
            Manager=new ABCTreelistManager( DisplayTreeListCtrl );
            Manager.RootConfig=BackupManager( rootConfig );

      
            this.Load+=new EventHandler( Form_Load );
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;

            InitPopupMenu();

            if ( list!=null )
            {
                foreach ( ABCTreeListColumn.ColumnConfig config in list )
                    ColumnList.Add( config.Clone() as ABCTreeListColumn.ColumnConfig );
            }
        }

        public TreeConfigNode BackupManager ( TreeConfigNode rootConfig )
        {
            TreeConfigNode newNode=new TreeConfigNode();
            if ( rootConfig.InnerData!=null )
                newNode.InnerData=(TreeConfigData)rootConfig.InnerData.Clone();

            foreach ( TreeConfigNode child in rootConfig.ChildrenNodes.Values )
            {
                TreeConfigNode newChild=BackupManager( child );
                newChild.ParentNode=newNode;
                if ( newNode.InnerData!=null )
                    newChild.InnerData.Level=newNode.InnerData.Level+1;
                if ( newNode.ChildrenNodes.ContainsKey( newChild.InnerData.Name )==false )
                    newNode.ChildrenNodes.Add( newChild.InnerData.Name , newChild );
                else
                    newNode.ChildrenNodes[newChild.InnerData.Name]=newChild;

                if ( Manager.ConfigList.ContainsKey( newChild.InnerData.Name )==false )
                    Manager.ConfigList.Add( newChild.InnerData.Name , newChild );
                else
                    Manager.ConfigList[newChild.InnerData.Name]=newChild;
            }

            return newNode;
        }

        void Form_Load ( object sender , EventArgs e )
        {

            #region ColumnConfig
            this.ColumnConfigGridView.KeyDown+=new KeyEventHandler( ColumnConfigGridView_KeyDown );
            this.ColumnConfigGridView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( ColumnConfigGridView_CellValueChanged );
       
            this.ColumnConfigGridCtrl.DataSource=ColumnList;
            gridColSumType.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( ABCSummaryType ) );
            gridColFixed.ColumnEdit=ABCPresentHelper.GetRepositoryFromEnum( typeof( DevExpress.XtraTreeList.Columns.FixedStyle ) );
            
            #endregion

            #region DataConfig
            this.repoFieldNameChooser.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoFieldNameChooser_ButtonClick );
            this.repoTableNameChooser.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repoTableNameChooser_ButtonClick );
            this.DataConfigTreeCtrl.CellValueChanged+=new CellValueChangedEventHandler( DataConfigTreeCtrl_CellValueChanged );
            this.DataConfigTreeCtrl.FocusedNodeChanged+=new FocusedNodeChangedEventHandler( DataConfigTreeCtrl_FocusedNodeChanged );
            this.DataConfigDetailVGridCtrl.CellValueChanged+=new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler( DataConfigDetailVGridCtrl_CellValueChanged );
     
            #endregion

            #region Display

            this.DisplayTreeListCtrl.InnerTreeList.ColumnChanged+=new DevExpress.XtraTreeList.ColumnChangedEventHandler( DisplayTreeListCtrl_ColumnChanged );
            this.DisplayTreeListCtrl.InnerTreeList.ColumnWidthChanged+=new DevExpress.XtraTreeList.ColumnWidthChangedEventHandler( InnerTreeList_ColumnWidthChanged );
            this.DisplayTreeListCtrl.HorizontalScroll.Visible=true;
            #endregion

            RefreshDataConfigTree();

          
        }

        void DataConfigTreeCtrl_FocusedNodeChanged ( object sender , FocusedNodeChangedEventArgs e )
        {
            TreeConfigNode obj=(TreeConfigNode)DataConfigTreeCtrl.GetDataRecordByNode( e.Node );
            if ( obj==null||(TreeConfigData)obj.InnerData==null )
            {
                DataConfigDetailVGridCtrl.SelectedObject=null;
                return;
            }

            DataConfigDetailVGridCtrl.SelectedObject=(TreeConfigData)obj.InnerData;
        }

        #region ColumnConfig

        void ColumnConfigGridView_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete )
            {
                if ( this.ColumnConfigGridView.SelectedRowsCount>0 )
                {
                    if ( ABCHelper.ABCMessageBox.Show( "Delete row?" , "Confirmation" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                        return;

                    this.ColumnConfigGridView.DeleteSelectedRows();
                }
            }
        }
 
        void ColumnConfigGridView_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            RefreshDataConfigTree();
        }
        #endregion

        #region DataConfig
   
        void DataConfigTreeCtrl_CellValueChanged ( object sender , CellValueChangedEventArgs e )
        {
            UpdateDataConfigs();
            DisplayTreeListCtrl.RefreshDataSource();
        }
        void repoFieldNameChooser_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            String strTableName=this.TableName;

            TreeConfigNode obj=(TreeConfigNode)DataConfigTreeCtrl.GetDataRecordByNode( DataConfigTreeCtrl.FocusedNode );
            if ( obj==null||(TreeConfigData)obj.InnerData==null )
                return;

            TreeConfigData configData=(TreeConfigData)obj.InnerData;
            strTableName=configData.TableName;

            if ( String.IsNullOrWhiteSpace( strTableName )==false )
            {
                String strOldFieldName=String.Empty;
                if ( configData.ColumnFieldNames.ContainsKey( this.DataConfigTreeCtrl.FocusedColumn.Caption ) )
                    strOldFieldName=configData.ColumnFieldNames[this.DataConfigTreeCtrl.FocusedColumn.Caption];

                FieldChooserEx chooser=new FieldChooserEx( strTableName , strOldFieldName );
                if ( chooser.ShowDialog()==System.Windows.Forms.DialogResult.OK )
                {
                    if ( configData.ColumnFieldNames.ContainsKey( this.DataConfigTreeCtrl.FocusedColumn.Caption ) )
                        configData.ColumnFieldNames[this.DataConfigTreeCtrl.FocusedColumn.Caption]=chooser.Result;
                    else
                        configData.ColumnFieldNames.Add( this.DataConfigTreeCtrl.FocusedColumn.Caption , chooser.Result );
                }

            
                DataConfigTreeCtrl.RefreshNode( DataConfigTreeCtrl.FocusedNode );
                DataConfigTreeCtrl.RefreshDataSource();
                UpdateDataConfigs();
                DisplayTreeListCtrl.RefreshDataSource();
            }
        }
        void repoTableNameChooser_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            String strTableName=this.TableName;

            TreeConfigNode obj=(TreeConfigNode)DataConfigTreeCtrl.GetDataRecordByNode( DataConfigTreeCtrl.FocusedNode );
            if ( obj==null||(TreeConfigData)obj.InnerData==null )
                return;

            TreeConfigData configData=(TreeConfigData)obj.InnerData;
            strTableName=configData.TableName;
          
            String strOldTableName=strTableName;
            TableChooserForm chooser=new TableChooserForm();
            String strResult=chooser.ShowChooseOne( strOldTableName );
            if ( chooser.DialogResult==DialogResult.OK )
                configData.TableName=strResult;

            DataConfigTreeCtrl.RefreshDataSource();
            DataConfigTreeCtrl.RefreshNode( DataConfigTreeCtrl.FocusedNode );
            
        }

        #region Context Menu

        DevExpress.XtraBars.PopupMenu ContextMenu=new DevExpress.XtraBars.PopupMenu();
        public void InitPopupMenu ( )
        {
            DataConfigTreeCtrl.MouseUp+=new MouseEventHandler( DataConfigTreeCtrl_MouseUp );
       
            #region ContextMenu
            DevExpress.XtraBars.BarButtonItem itemNew=new DevExpress.XtraBars.BarButtonItem();
            itemNew.Caption="New";
            itemNew.Tag="New";
            itemNew.ImageIndex=0;
            itemNew.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemNew );


            DevExpress.XtraBars.BarButtonItem itemDelete=new DevExpress.XtraBars.BarButtonItem();
            itemDelete.Caption="Delete";
            itemDelete.Tag="Delete";
            itemDelete.ImageIndex=54;
            itemDelete.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemDelete );

            barManager1.Images=ABCImageList.List16x16;
            ContextMenu.Manager=barManager1;
            #endregion
        }

        void DataConfigTreeCtrl_MouseUp ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right&&ModifierKeys==Keys.None&&DataConfigTreeCtrl.State==TreeListState.Regular )
            {
                Point pt=DataConfigTreeCtrl.PointToClient( MousePosition );
                TreeListHitInfo info=DataConfigTreeCtrl.CalcHitInfo( pt );
                if ( info.Node!=null )
                    DataConfigTreeCtrl.FocusedNode=info.Node;
                ContextMenu.ShowPopup( MousePosition );
            }

        }

        void Menu_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="New" )
            {
                TableChooserForm form=new TableChooserForm() ;
                String strTableName=form.ShowChooseOne();
                if ( form.DialogResult==DialogResult.Cancel )
                    return;
                if(DataStructureProvider.IsExistedTable(strTableName)==false)
                    return;

                TreeConfigData configData=new TreeConfigData();
                configData.Name="objNew" + this.Manager.ConfigList.Count;
                configData.DefaultLoad=true;
                configData.TableName=strTableName;
                configData.ColumnFieldNames=new Dictionary<string , string>();

                TreeConfigNode obj=(TreeConfigNode)DataConfigTreeCtrl.GetDataRecordByNode( DataConfigTreeCtrl.FocusedNode );
                if ( obj==null )
                    new TreeConfigNode( Manager.RootConfig , configData );
                else
                {
                    configData.ParentTableName=obj.InnerData.TableName;
                    configData.ParentField=DataStructureProvider.GetPrimaryKeyColumn( obj.InnerData.TableName );
                    configData.ChildField=DataStructureProvider.GetForeignKeyOfTableName( configData.TableName , obj.InnerData.TableName );
                    new TreeConfigNode( obj , configData );
                }

                UpdateDataConfigs();
                RefreshDataConfigTree();
         
            }
         
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Delete" )
            {
                TreeConfigNode obj=(TreeConfigNode)DataConfigTreeCtrl.GetDataRecordByNode( DataConfigTreeCtrl.FocusedNode );
                if ( obj==null||(TreeConfigData)obj.InnerData==null )
                    return;

                DialogResult result=ABCHelper.ABCMessageBox.Show( "Do you want to delete selected Object ? " , "Delete Object" , MessageBoxButtons.YesNo );
                if ( result==DialogResult.Yes )
                {
                    obj.ParentNode.ChildrenNodes.Remove( obj.InnerData.Name );
                    obj.ParentNode=null;

                    UpdateDataConfigs();
                    RefreshDataConfigTree();
                }
            }


        }

        #endregion

        #region VGrid


        void DataConfigDetailVGridCtrl_CellValueChanged ( object sender , DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e )
        {
            DataConfigTreeCtrl.RefreshDataSource();
        }

        #endregion

        #endregion

        #region Display

        void InnerTreeList_ColumnWidthChanged ( object sender , DevExpress.XtraTreeList.ColumnChangedEventArgs e )
        {
            foreach ( ABCTreeListColumn gridCol in this.DisplayTreeListCtrl.InnerTreeList.Columns )
                UpdateColumnConfigs( gridCol );
        }

        void DisplayTreeListCtrl_ColumnChanged ( object sender , DevExpress.XtraTreeList.ColumnChangedEventArgs e )
        {
            ABCTreeListColumn gridCol=sender as ABCTreeListColumn;
            UpdateColumnConfigs( gridCol );
        }
        #endregion

        private void btnSave_Click ( object sender , EventArgs e )
        {
            foreach ( ABCTreeListColumn gridCol in this.DisplayTreeListCtrl.InnerTreeList.Columns )
                UpdateColumnConfigs( gridCol );

            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }

        public void UpdateColumnConfigs ( ABCTreeListColumn gridCol )
        {
            if ( gridCol==null )
                return;

            gridCol.Config.VisibleIndex=gridCol.VisibleIndex;
            gridCol.Config.Visible=gridCol.Visible;
            gridCol.Config.Width=gridCol.Width;

            ColumnConfigGridCtrl.RefreshDataSource();
        }
        public void UpdateDataConfigs ( )
        {
            Manager.RefreshConfigList();
        }
        public void RefreshDisplayTree ( )
        {
            DisplayTreeListCtrl.TableName=this.TableName;
            DisplayTreeListCtrl.InnerTreeList.Columns.Clear();
            DisplayTreeListCtrl.ColumnConfigs=this.ColumnList;
            DisplayTreeListCtrl.Manager.ConfigList=this.Manager.ConfigList;//new
            DisplayTreeListCtrl.InitColumns();

            #region Script
            if ( String.IsNullOrWhiteSpace( this.TableName ) )
            {
                if ( String.IsNullOrWhiteSpace( Script )==false )
                {
                    DataSet ds=DataQueryProvider.RunQuery( Script );
                    if ( ds!=null&&ds.Tables.Count>0 )
                    {
                        Manager.Invalidate( ds );
                        DisplayTreeListCtrl.InnerTreeList.ColumnsCustomization();
                    }
                }
                return;
            } 
            #endregion

            if ( DataCachingProvider.LookupTables.ContainsKey( this.TableName ) )
            {
                Manager.Invalidate( DataCachingProvider.LookupTables[this.TableName] );
            }
            else
            {
                ABCHelper.ConditionBuilder strBuilder=new ABCHelper.ConditionBuilder();
                strBuilder.Append( String.Format( @"SELECT TOP 5 * FROM {0} " , this.TableName ) );
                if ( DataStructureProvider.IsExistABCStatus( this.TableName ) )
                    strBuilder.AddCondition( QueryGenerator.GenerateCondition( this.TableName , ABCCommon.ABCColumnType.ABCStatus ) );

                strBuilder.Append( String.Format( @" ORDER BY {0} DESC" , DataStructureProvider.GetPrimaryKeyColumn( this.TableName ) ) );

                try
                {
                    DataSet ds=DataQueryProvider.RunQuery( strBuilder.ToString() );
                    if ( ds!=null&&ds.Tables.Count>0 )
                        this.DisplayTreeListCtrl.InnerTreeList.DataSource=ds.Tables[0];

                }
                catch ( Exception ex )
                {

                }
            }

            this.DisplayTreeListCtrl.InnerTreeList.RefreshDataSource();
            DisplayTreeListCtrl.InnerTreeList.ColumnsCustomization();
        }
        public void RefreshDataConfigTree ( )
        {
            this.DataConfigTreeCtrl.Columns.Clear();

            TreeListColumn colNo=new TreeListColumn();
            colNo.Name="Name";
            colNo.FieldName="Name";
            colNo.Caption="Config Name";
            colNo.Visible=true;
            colNo.VisibleIndex=0;

            TreeListColumn colTableName=new TreeListColumn();
            colTableName.Name="TableName";
            colTableName.FieldName="TableName";
            colTableName.Caption="TableName";
            colTableName.Visible=true;
            colTableName.VisibleIndex=0;
            colTableName.ColumnEdit=this.repoTableNameChooser;

            TreeListColumn[] lstCols=new TreeListColumn[ColumnList.Count+2];
            lstCols[0]=colNo;
            lstCols[1]=colTableName;
            int iCount=1;
            foreach ( ABCTreeListColumn.ColumnConfig col in ColumnList )
            {
                TreeListColumn configCol=new TreeListColumn();
                configCol.Caption=col.Caption;
                configCol.Tag=col;
                configCol.Visible=col.Visible;
                configCol.VisibleIndex=col.VisibleIndex;
                configCol.Width=col.Width;
                configCol.ColumnEdit=this.repoFieldNameChooser;
                iCount++;
                lstCols[iCount]=configCol;
            }

            this.DataConfigTreeCtrl.Columns.AddRange( lstCols );

            this.DataConfigTreeCtrl.DataSource=Manager.RootConfig;
            this.DataConfigTreeCtrl.RefreshDataSource();
            this.DataConfigTreeCtrl.ExpandAll();

            DisplayTreeListCtrl.RefreshDataSource();
            DataConfigTreeCtrl.BestFitColumns();
        }

        private void btnRefreshDisplayTree_Click ( object sender , EventArgs e )
        {
            RefreshDisplayTree();
        }
    }
}