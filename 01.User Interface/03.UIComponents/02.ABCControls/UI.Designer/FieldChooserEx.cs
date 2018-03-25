using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;

using ABCControls;

using ABCProvider;
namespace ABCControls
{
    public class DataStructInfo
    {
        public String FieldName { get; set; }
        public String TableName { get; set; }
        public String TotalFieldName { get; set; }

        public Boolean Selected { get; set; }
        public DataStructInfo ( String strTableName , String strName )
        {
            FieldName=strName;
            TableName=strTableName;
            TotalFieldName=FieldName;
            Selected=false;
        }
    }

    public partial class FieldChooserEx : DevExpress.XtraEditors.XtraForm
    {
        public String TableName=String.Empty;
        public String Result=String.Empty;

        public FieldChooserEx (String strTableName ,String strResult)
        {
            TableName=strTableName;
            Result=strResult;

            InitializeComponent();

            Initialize();

            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
            this.FormClosing+=new FormClosingEventHandler( FieldChooserEx_FormClosing );
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repoCheck;
        public void Initialize ( )
        {

            treeList1.OptionsBehavior.ShowEditorOnMouseUp=false;
            treeList1.OptionsBehavior.CloseEditorOnLostFocus=true;
            treeList1.OptionsBehavior.ImmediateEditor=false;
            treeList1.OptionsSelection.UseIndicatorForSelection=false;
            treeList1.OptionsView.ShowColumns=true;
            treeList1.OptionsView.ShowVertLines=false;
            treeList1.OptionsView.ShowHorzLines=false;
            treeList1.OptionsView.ShowFocusedFrame=false;
            treeList1.OptionsBehavior.DragNodes=false;

            #region Columns
            DevExpress.XtraTreeList.Columns.TreeListColumn colField=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colField.Caption="Field";
            colField.FieldName="FieldName";
            colField.Visible=true;
            colField.VisibleIndex=0;
            colField.OptionsColumn.AllowEdit=false;

            DevExpress.XtraTreeList.Columns.TreeListColumn colCheck=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colCheck.Caption="Select";
            colCheck.FieldName="Selected";
            colCheck.Visible=true;
            colCheck.VisibleIndex=0;
            colCheck.OptionsColumn.AllowEdit=true;
            colCheck.Width=20;

            repoCheck=new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            colCheck.ColumnEdit=repoCheck;
           

            treeList1.Columns.AddRange( new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colCheck , colField } );

            #endregion

            ABCCommonTreeListNode root=new ABCCommonTreeListNode( null , null );
            foreach ( String strFieldName in DataStructureProvider.DataTablesList[this.TableName].ColumnsList.Keys )
            {
                DataStructInfo dataInfo=new DataStructInfo( TableName , strFieldName );
                new ABCCommonTreeListNode( root , dataInfo );
                lstData.Add( dataInfo );
            }

            treeList1.DataSource=root;
            treeList1.RefreshDataSource();
            treeList1.Refresh();
            if ( treeList1.Nodes.Count>0 )
                treeList1.FocusedNode=treeList1.Nodes[0];

            InitOldResult( root );

            treeList1.ExpandAll();
            colCheck.Width=20;

            treeList1.MouseClick+=new MouseEventHandler( treeList1_MouseClick );
            treeList1.CustomDrawNodeCell+=new CustomDrawNodeCellEventHandler( treeList1_CustomDrawNodeCell );
            repoCheck.EditValueChanged+=new EventHandler( repoCheck_EditValueChanged );

            InitPopupMenu();
        }

        void treeList1_FocusedNodeChanged ( object sender , FocusedNodeChangedEventArgs e )
        {
            
        }

        void repoCheck_EditValueChanged ( object sender , EventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeList1.GetDataRecordByNode( treeList1.FocusedNode );
            if ( obj==null||(DataStructInfo)obj.InnerData==null )
                return;
            ( obj.InnerData as DataStructInfo ).Selected=( sender as DevExpress.XtraEditors.CheckEdit ).Checked;
            if ( ( obj.InnerData as DataStructInfo ).Selected )
            {
                foreach ( DataStructInfo data in lstData )
                {
                    if ( data!=( obj.InnerData as DataStructInfo )&&data.TotalFieldName!=( obj.InnerData as DataStructInfo ) .TotalFieldName)
                        data.Selected=false;
                }
            }
            treeList1.RefreshDataSource();
        }

     

        List<DataStructInfo> lstData=new List<DataStructInfo>();

        public void InitOldResult (ABCCommonTreeListNode root)
        {
            try
            {

                if ( String.IsNullOrWhiteSpace( Result ) )
                    return;

                String[] strArr=Result.Split( ':' );
                if ( strArr.Length<=0 )
                    return;

                ABCCommonTreeListNode currentNode=root;
                for ( int i=0; i<strArr.Length; i++ )
                {
                    String strFieldName=strArr[i];
                    foreach ( ABCCommonTreeListNode child in currentNode.ChildrenNodes )
                    {
                        if ( ( child.InnerData as DataStructInfo ).FieldName==strFieldName )
                        {
                            ExploreFKNode( child );
                            currentNode=child;
                            break;
                        }
                    }
                }

                ( currentNode.InnerData as DataStructInfo ).Selected=true;

                treeList1.RefreshDataSource();
                treeList1.ExpandAll();
            }
            catch ( Exception ex )
            {
            }
        }
        public void ExploreFKNode ( ABCCommonTreeListNode obj )
        {
            DataStructInfo data=(DataStructInfo)obj.InnerData;
            if ( DataStructureProvider.IsForeignKey( data.TableName , data.FieldName ) )
            {
                String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( data.TableName , data.FieldName );
                if ( DataStructureProvider.DataTablesList.ContainsKey( strPKTableName )==false )
                    return;

                foreach ( String strFieldName in DataStructureProvider.DataTablesList[strPKTableName].ColumnsList.Keys )
                {
                    DataStructInfo dataInfo=new DataStructInfo( strPKTableName , strFieldName );
                    dataInfo.TotalFieldName=data.TotalFieldName+":"+strFieldName;
                    lstData.Add( dataInfo );
                    new ABCCommonTreeListNode( obj , dataInfo );
                }
            }
        }

        void FieldChooserEx_FormClosing ( object sender , FormClosingEventArgs e )
        {
            Result=String.Empty;

            foreach ( DataStructInfo data in lstData )
                if ( data.Selected )
                    Result=data.TotalFieldName;
          
        }
        void treeList1_CustomDrawNodeCell ( object sender , CustomDrawNodeCellEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeList1.GetDataRecordByNode( e.Node );
            if ( obj==null||(DataStructInfo)obj.InnerData==null )
                return;
            if(((DataStructInfo)obj.InnerData).Selected)
                e.Appearance.Font=new Font( e.Appearance.Font , e.Appearance.Font.Style|FontStyle.Bold);
        }

        #region Context Menu

        DevExpress.XtraBars.PopupMenu ContextMenu=new DevExpress.XtraBars.PopupMenu();

        public void InitPopupMenu ( )
        {

            DevExpress.XtraBars.BarButtonItem itemExplore=new DevExpress.XtraBars.BarButtonItem();
            itemExplore.Caption="Explore";
            itemExplore.Tag="Explore";
            itemExplore.ImageIndex=108;
            itemExplore.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemExplore );

            ContextMenu.Manager=this.barManager1;
            this.barManager1.Images=ABCControls.ABCImageList.List16x16;

        }

        void treeList1_MouseClick ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right&&ModifierKeys==Keys.None&&treeList1.State==TreeListState.Regular )
            {
                Point pt=this.PointToClient( MousePosition );
                TreeListHitInfo info=treeList1.CalcHitInfo( pt );
                if ( info.HitInfoType==HitInfoType.Cell||info.HitInfoType==HitInfoType.StateImage )
                {
                    treeList1.FocusedNode=info.Node;

                    ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeList1.GetDataRecordByNode( treeList1.FocusedNode );
                    if ( obj==null||(DataStructInfo)obj.InnerData==null )
                        return;

                    DataStructInfo data=(DataStructInfo)obj.InnerData;
                    if ( DataStructureProvider.IsForeignKey( data.TableName , data.FieldName ) )
                        ContextMenu.ShowPopup( MousePosition );
                }
            }

            else if ( e.Button==MouseButtons.Left&&ModifierKeys==Keys.None&&treeList1.State==TreeListState.NodePressed )
            {
                ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeList1.GetDataRecordByNode( treeList1.FocusedNode );
                if ( obj==null||(DataStructInfo)obj.InnerData==null )
                    return;

                ( obj.InnerData as DataStructInfo ).Selected=!( obj.InnerData as DataStructInfo ).Selected;

                if ( ( obj.InnerData as DataStructInfo ).Selected )
                {
                    foreach ( DataStructInfo data in lstData )
                    {
                        if ( data!=( obj.InnerData as DataStructInfo )&&data.TotalFieldName!=( obj.InnerData as DataStructInfo ).TotalFieldName )
                            data.Selected=false;
                    }
                }
                treeList1.RefreshDataSource();
            }
        }

        void Menu_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Explore"&&treeList1.Selection.Count>0 )
            {
                ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeList1.GetDataRecordByNode( treeList1.FocusedNode );
                if ( obj==null||(DataStructInfo)obj.InnerData==null )
                    return;

                ExploreFKNode( obj );

                treeList1.RefreshDataSource();
                treeList1.ExpandAll();
            }

        }
        #endregion
     
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
    }
}