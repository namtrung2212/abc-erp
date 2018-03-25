using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using System.Xml;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using ABCControls;
using ABCCommon;
using ABCScreen;
using ABCProvider;
using ABCBusinessEntities;

namespace ABCApp
{
    public partial class ModulesExplorer : DevExpress.XtraTreeList.TreeList
    {

        public MainForm OwnerMainForm;
        public ModulesExplorer ( )
        {
        }


        public void Initialize ( MainForm mainForm )
        {
            OwnerMainForm=mainForm;
            this.StateImageList=ABCControls.ABCImageList.List16x16;

            // this.OptionsBehavior.ShowEditorOnMouseUp=false;
            //   this.OptionsBehavior.CloseEditorOnLostFocus=true;
            this.OptionsBehavior.ImmediateEditor=false;
            this.OptionsSelection.UseIndicatorForSelection=false;
            this.OptionsView.ShowColumns=false;
            this.OptionsView.ShowVertLines=false;
            this.OptionsView.ShowHorzLines=false;
            this.OptionsView.ShowFocusedFrame=false;
            this.OptionsBehavior.DragNodes=false;
            this.OptionsBehavior.Editable=false;

            this.MouseClick+=new MouseEventHandler( ModulesExplorer_MouseClick );
            this.GetStateImage+=new GetStateImageEventHandler( ViewTree_GetStateImage );
            this.CustomDrawNodeCell+=new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler( ViewTree_CustomDrawNodeCell );
            this.NodeCellStyle+=new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler( ModulesExplorer_NodeCellStyle );
            this.MouseMove+=new MouseEventHandler( ModulesExplorer_MouseMove );
            this.MouseLeave+=new EventHandler( ModulesExplorer_MouseLeave );

            #region Columns

            DevExpress.XtraTreeList.Columns.TreeListColumn colName=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colName.Caption="Name";
            colName.Visible=true;
            colName.VisibleIndex=0;
            colName.OptionsColumn.AllowEdit=false;

            DevExpress.XtraTreeList.Columns.TreeListColumn colID=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colID.Caption="ID";
            colID.Visible=true;
            colID.VisibleIndex=-1;
            this.Columns.AddRange( new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colName , colID } );

            #endregion

            RefreshViewList();
        }

      
        void ModulesExplorer_MouseClick ( object sender , MouseEventArgs e )
        {
            TreeList tree=sender as TreeList;
            if ( e.Button==MouseButtons.Left&&ModifierKeys==Keys.None&&tree.State==TreeListState.NodePressed )
            {

                Point pt=tree.PointToClient( MousePosition );
                TreeListHitInfo info=tree.CalcHitInfo( pt );
                if ( info.HitInfoType==HitInfoType.Cell||info.HitInfoType==HitInfoType.StateImage )
                {
                    tree.FocusedNode=info.Node;

                    ViewNode obj=(ViewNode)this.GetDataRecordByNode( info.Node );
                    if ( obj==null||obj.InnerData==null||obj.InnerData is STViewsInfo==false )
                        return;

                    STViewsInfo viewInfo=(STViewsInfo)obj.InnerData;
                    if ( viewInfo!=null )
                    {
                        ABCHelper.ABCWaitingDialog.Show( "" , "Đang mở . . .!" );

                        ABCScreen.ABCBaseScreen scr=ABCScreen.ABCScreenFactory.GetABCScreen( viewInfo , ViewMode.Runtime );
                        if ( scr!=null )
                            scr.Show();

                        ABCHelper.ABCWaitingDialog.Close();

                    }
                }
            }
        }

        void ViewTree_CustomDrawNodeCell ( object sender , DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e )
        {
            ViewNode obj=(ViewNode)this.GetDataRecordByNode( e.Node );
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.InnerData is STViewGroupsInfo )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );

            if ( e.Node.Selected )
                e.Appearance.Font=new Font( e.Appearance.Font , e.Appearance.Font.Style|FontStyle.Underline );

        }

        void ViewTree_GetStateImage ( object sender , GetStateImageEventArgs e )
        {
            ViewNode obj=(ViewNode)this.GetDataRecordByNode( e.Node );
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.InnerData is STViewGroupsInfo )
                e.NodeImageIndex=39;
            else if ( obj.InnerData is STViewsInfo )
                e.NodeImageIndex=37;
            //else if ( e.Node.Level==0 )
            //    e.NodeImageIndex=78;
        }


        #region Data
        public void RefreshViewList ( )
        {

            List<Guid> lstViewIDs=ABCUserProvider.GetViews( ABCUserProvider.CurrentUser.ADUserID );

            STViewGroupsController viewGroupCtrl=new STViewGroupsController();
            STViewsController viewCtrl=new STViewsController();

            ViewNode rootNode=new ViewNode( null , null , null );

            Dictionary<Guid , ViewNode> lstGroupNodes=new Dictionary<Guid , ViewNode>();

            List<BusinessObject> lstViews=viewCtrl.GetList( "SELECT * FROM STViews WHERE (DisplayInTree IS NULL OR DisplayInTree ='TRUE') ORDER BY SortOrder" );

            List<BusinessObject> lstGroups=viewGroupCtrl.GetList( "SELECT * FROM STViewGroups ORDER BY SortOrder" );

            foreach ( STViewGroupsInfo group in lstGroups )
            {
                ViewNode groupNode=null;

                foreach ( STViewsInfo view in lstViews )
                {
                    if ( view.FK_STViewGroupID.HasValue&&view.FK_STViewGroupID.Value==group.STViewGroupID&&lstViewIDs.Contains( view.STViewID ) )
                    {
                        if ( groupNode==null )
                        {
                            groupNode=new ViewNode( rootNode , new object[] { group.Name , group.STViewGroupID } , group );
                            lstGroupNodes.Add( group.STViewGroupID , groupNode );
                        }
                        String strCaption=view.STViewName;
                        if ( String.IsNullOrWhiteSpace( strCaption ) )
                            strCaption=view.STViewNo;

                        ViewNode viewNode=new ViewNode( groupNode , new object[] { strCaption , view.STViewID } , view );
                        viewNode.ViewCount=1;
                        groupNode.ViewCount++;
                    }
                }

            }

            foreach ( Guid iID in lstGroupNodes.Keys )
            {
                if ( ( lstGroupNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID.HasValue&&( lstGroupNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID.Value!=Guid.Empty )
                {
                    foreach ( Guid iID2 in lstGroupNodes.Keys )
                    {

                        if ( iID2!=iID )
                        {
                            if ( ( lstGroupNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID.Value==( lstGroupNodes[iID2].InnerData as STViewGroupsInfo ).STViewGroupID )
                            {
                                lstGroupNodes[iID].parentCore.childrenCore.Remove( lstGroupNodes[iID] );
                                lstGroupNodes[iID].parentCore=lstGroupNodes[iID2];
                                lstGroupNodes[iID2].childrenCore.Add( lstGroupNodes[iID] );
                                lstGroupNodes[iID2].ViewCount+=lstGroupNodes[iID].ViewCount;
                            }
                        }

                    }
                }
            }
            if ( ABCUserProvider.CurrentUser!=null&&!ABCUserProvider.CurrentUser.No.Contains( "admin" ) )
            {
                foreach ( Guid iID in lstGroupNodes.Keys )
                {
                    if ( lstGroupNodes[iID].ViewCount<=0 )
                        lstGroupNodes[iID].parentCore.childrenCore.Remove( lstGroupNodes[iID] );
                }
            }

            this.DataSource=rootNode;
            this.CollapseAll();
        }
        public class ViewNode : TreeList.IVirtualTreeListData
        {
            public ViewNode parentCore;
            public ArrayList childrenCore=new ArrayList();
            public int ViewCount=0;

            protected object[] cellsCore;
            protected int Level=0;
            public object InnerData;

            public ViewNode ( ViewNode parent , object[] cells , object data )
            {
                // Specifies the parent node for the new node.
                this.parentCore=parent;
                if ( parent!=null )
                    this.Level=parent.Level+1;
                this.InnerData=data;

                // Provides data for the node's cell.
                this.cellsCore=cells;
                if ( this.parentCore!=null )
                    this.parentCore.childrenCore.Add( this );
            }
            void TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes ( VirtualTreeGetChildNodesInfo info )
            {
                info.Children=childrenCore;
            }
            void TreeList.IVirtualTreeListData.VirtualTreeGetCellValue ( VirtualTreeGetCellValueInfo info )
            {
                if ( info.Column.VisibleIndex>=0 )
                    info.CellData=cellsCore[info.Column.VisibleIndex];
            }

            void TreeList.IVirtualTreeListData.VirtualTreeSetCellValue ( VirtualTreeSetCellValueInfo info )
            {
                if ( info.Column.VisibleIndex>=0 )
                    cellsCore[info.Column.VisibleIndex]=info.NewCellData;
            }
        }

        #endregion
     
        #region Hot track

        private TreeListNode hotTrackNode=null;
        private TreeListNode HotTrackNode
        {
            get { return hotTrackNode; }
            set
            {
                if ( hotTrackNode!=value )
                {
                    TreeListNode prevHotTrackNode=hotTrackNode;
                    hotTrackNode=value;
                    if ( this.ActiveEditor!=null )
                        this.PostEditor();
                    this.RefreshNode( prevHotTrackNode );
                    this.RefreshNode( hotTrackNode );
                }
            }
        }

        void ModulesExplorer_NodeCellStyle ( object sender , DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e )
        {
            if ( e.Node==HotTrackNode )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Underline );
        }

        void ModulesExplorer_MouseMove ( object sender , MouseEventArgs e )
        {
            TreeList treelist=sender as DevExpress.XtraTreeList.TreeList;
            TreeListHitInfo info=treelist.CalcHitInfo( new Point( e.X , e.Y ) );
            HotTrackNode=info.HitInfoType==HitInfoType.Cell?info.Node:null;
        }

        void ModulesExplorer_MouseLeave ( object sender , EventArgs e )
        {
            HotTrackNode=null;
        }
        #endregion
    }
}
