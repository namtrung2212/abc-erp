using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;

using ABCBusinessEntities;
using DevExpress.XtraTreeList;
using System.Xml;
using ABCCommon;

namespace ABCStudio
{
    public partial class ViewExplorer : DevExpress.XtraTreeList.TreeList
    {
     
        public Studio OwnerStudio;
        public ViewExplorer (  )
        {
        }


        public void Initialize ( )
        {
            this.StateImageList=OwnerStudio.ToolbarImageList;

       
           this.OptionsBehavior.ShowEditorOnMouseUp=false;
           this.OptionsBehavior.CloseEditorOnLostFocus=true;
            this.OptionsBehavior.ImmediateEditor=false;
            this.OptionsSelection.UseIndicatorForSelection=false;
            this.OptionsView.ShowColumns=false;
            this.OptionsView.ShowVertLines=false;
            this.OptionsView.ShowHorzLines=false;
            this.OptionsView.ShowFocusedFrame=false;
            this.OptionsBehavior.DragNodes=true;

            this.DoubleClick+=new EventHandler( ViewTree_DoubleClick );
            this.MouseUp+=new MouseEventHandler( ViewTree_MouseUp );
            this.GetStateImage+=new GetStateImageEventHandler( ViewTree_GetStateImage );
            this.CustomDrawNodeCell+=new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler( ViewTree_CustomDrawNodeCell );

            this.DragNodesMode=DevExpress.XtraTreeList.TreeListDragNodesMode.Advanced;
            this.DragOver+=new DragEventHandler( ViewTree_DragOver );
            this.BeforeDragNode+=new DevExpress.XtraTreeList.BeforeDragNodeEventHandler( ViewTree_BeforeDragNode );
            this.CellValueChanged+=new DevExpress.XtraTreeList.CellValueChangedEventHandler( ViewExplorer_CellValueChanged );
         
            #region Columns
            DevExpress.XtraTreeList.Columns.TreeListColumn colNo=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colNo.Caption="No";
            colNo.Visible=true;
            colNo.VisibleIndex=0;
          //  colNo.OptionsColumn.AllowEdit=true;
            
            DevExpress.XtraTreeList.Columns.TreeListColumn colID=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colID.Caption="ID";
            colID.Visible=true;
            colID.VisibleIndex=-1;
            this.Columns.AddRange( new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colNo , colID } );

            #endregion

            InitPopupMenu();
        }

        void ViewExplorer_CellValueChanged ( object sender , DevExpress.XtraTreeList.CellValueChangedEventArgs e )
        {
         
            MyData obj=(MyData)this.GetDataRecordByNode( this.FocusedNode );
            if ( obj==null||obj.InnerData==null )
                return;


            String strNo=String.Empty;

            if ( obj.InnerData is STViewsInfo )
                strNo=( obj.InnerData as STViewsInfo ).STViewNo;
            if ( obj.InnerData is STViewGroupsInfo )
                strNo=( obj.InnerData as STViewGroupsInfo ).No;

            if ( strNo==e.Value.ToString() )
            {
                this.OptionsBehavior.Editable=false;
                return;
            }

          

            if ( obj.InnerData is STViewsInfo )
            {
                STViewsController viewCtrl=new STViewsController();
                //if ( viewCtrl.GetObjectByNo( e.Value.ToString() )!=null )
                //{
                //    ABCHelper.ABCMessageBox.Show( String.Format( "View Name : '{0}' is already existed. " , e.Value.ToString() ) , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                //    e.Value=strNo;
                //    this.OptionsBehavior.Editable=false;
                //    return;
                //}
                STViewsInfo info=(STViewsInfo)obj.InnerData;
                if ( info!=null )
                {
                    info.STViewNo=e.Value.ToString();
                    info.STViewName=e.Value.ToString();
                    viewCtrl.UpdateObject( info );
                }
            }
            if ( obj.InnerData is STViewGroupsInfo )
            {
                STViewGroupsController groupCtrl=new STViewGroupsController();
                //if ( groupCtrl.GetObjectByNo( e.Value.ToString() )!=null )
                //{
                //    ABCHelper.ABCMessageBox.Show( String.Format( "Group Name : '{0}' is already existed. " , e.Value.ToString() ) , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                //    e.Value=strNo;
                //    this.OptionsBehavior.Editable=false;
                //    return;
                //}
                STViewGroupsInfo info=(STViewGroupsInfo)obj.InnerData;
                if ( info!=null )
                {
                    info.No=e.Value.ToString();
                    info.Name=e.Value.ToString();
                    groupCtrl.UpdateObject( info );
                }
            }

            this.OptionsBehavior.Editable=false;
        }

        void ViewTree_BeforeDragNode ( object sender , DevExpress.XtraTreeList.BeforeDragNodeEventArgs e )
        {
            MyData obj=(MyData)this.GetDataRecordByNode( e.Node );
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.InnerData is STViewGroupsInfo)
                e.CanDrag=false;
        }

        void ViewTree_ValidatingEditor ( object sender , DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e )
        {
          

         
        }

        void ViewTree_DragOver ( object sender , DragEventArgs e )
        {

            if ( e.Data is DataObject )
            {
                DataObject data=(DataObject)e.Data;
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode=(DevExpress.XtraTreeList.Nodes.TreeListNode)data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) );
                if ( objNode!=null )
                {
                    MyData obj=(MyData)this.GetDataRecordByNode( objNode );
                    if ( obj==null||obj.InnerData==null )
                        return;

                    STViewsInfo info=(STViewsInfo)obj.InnerData;
                    ABCToolboxItem toolboxItem=new ABCToolboxItem( typeof( ABCControls.ABCView ) );
                    toolboxItem.Binding=new ABCControls.ABCBindingInfo();
                    toolboxItem.Binding.ViewInfo=info;

                    data=OwnerStudio.Toolbox.SerializeToolboxItem( toolboxItem ) as DataObject;
                    try
                    {
                        OwnerStudio.Toolbox.DoDragDrop( data , DragDropEffects.Copy );
                    }
                    catch ( Exception ex )
                    {
                        MessageBox.Show( ex.Message , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    }

                }

            }
        }

        #region Context Menu

        DevExpress.XtraBars.PopupMenu ViewContextMenu=new DevExpress.XtraBars.PopupMenu();
        DevExpress.XtraBars.PopupMenu GroupContextMenu=new DevExpress.XtraBars.PopupMenu();
        DevExpress.XtraBars.PopupMenu RootContextMenu=new DevExpress.XtraBars.PopupMenu();

        public void InitPopupMenu ( )
        {
            #region ViewContextMenu
            DevExpress.XtraBars.BarButtonItem itemOpen=new DevExpress.XtraBars.BarButtonItem();
            itemOpen.Caption="Open";
            itemOpen.Tag="OpenView";
            itemOpen.ImageIndex=1;
            itemOpen.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemOpen );

            DevExpress.XtraBars.BarButtonItem itemRun=new DevExpress.XtraBars.BarButtonItem();
            itemRun.Caption="Run";
            itemRun.Tag="RunView";
            itemRun.ImageIndex=71;
            itemRun.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemRun );

            DevExpress.XtraBars.BarButtonItem itemImmort=new DevExpress.XtraBars.BarButtonItem();
            itemImmort.Caption="Import";
            itemImmort.Tag="Import";
            itemImmort.ImageIndex=110;
            itemImmort.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemImmort );

            DevExpress.XtraBars.BarButtonItem itemExport=new DevExpress.XtraBars.BarButtonItem();
            itemExport.Caption="Export";
            itemExport.Tag="Export";
            itemExport.ImageIndex=108;
            itemExport.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemExport );


            DevExpress.XtraBars.BarButtonItem itemRename1=new DevExpress.XtraBars.BarButtonItem();
            itemRename1.Caption="Rename";
            itemRename1.Tag="RenameView";
            itemRename1.ImageIndex=75;
            itemRename1.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemRename1 );


            DevExpress.XtraBars.BarButtonItem itemDelete=new DevExpress.XtraBars.BarButtonItem();
            itemDelete.Caption="Delete";
            itemDelete.Tag="DeleteView";
            itemDelete.ImageIndex=54;
            itemDelete.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemDelete );

            DevExpress.XtraBars.BarButtonItem itemRefresh=new DevExpress.XtraBars.BarButtonItem();
            itemRefresh.Caption="Refresh";
            itemRefresh.Tag="Refresh";
            itemRefresh.ImageIndex=48;
            itemRefresh.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ViewContextMenu.AddItem( itemRefresh );

            ViewContextMenu.Manager=OwnerStudio.StudioBarManager; 
            #endregion

            #region GroupContextMenu

            DevExpress.XtraBars.BarButtonItem itemNewView=new DevExpress.XtraBars.BarButtonItem();
            itemNewView.Caption="New View...";
            itemNewView.Tag="NewView";
            itemNewView.ImageIndex=0;
            itemNewView.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            GroupContextMenu.AddItem( itemNewView );

            DevExpress.XtraBars.BarButtonItem itemNewChildGroup=new DevExpress.XtraBars.BarButtonItem();
            itemNewChildGroup.Caption="New Group...";
            itemNewChildGroup.Tag="NewChildGroup";
            itemNewChildGroup.ImageIndex=0;
            itemNewChildGroup.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            GroupContextMenu.AddItem( itemNewChildGroup );

            DevExpress.XtraBars.BarButtonItem itemRename=new DevExpress.XtraBars.BarButtonItem();
            itemRename.Caption="Rename";
            itemRename.Tag="RenameGroup";
            itemRename.ImageIndex=76;
            itemRename.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            GroupContextMenu.AddItem( itemRename );


            DevExpress.XtraBars.BarButtonItem itemDelete2=new DevExpress.XtraBars.BarButtonItem();
            itemDelete2.Caption="Delete";
            itemDelete2.Tag="DeleteGroup";
            itemDelete2.ImageIndex=54;
            itemDelete2.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            GroupContextMenu.AddItem( itemDelete2 );

            itemRefresh=new DevExpress.XtraBars.BarButtonItem();
            itemRefresh.Caption="Refresh";
            itemRefresh.Tag="Refresh";
            itemRefresh.ImageIndex=48;
            itemRefresh.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            GroupContextMenu.AddItem( itemRefresh );

            GroupContextMenu.Manager=OwnerStudio.StudioBarManager;
            #endregion

            #region RootContextMenu

            DevExpress.XtraBars.BarButtonItem itemNewGroup=new DevExpress.XtraBars.BarButtonItem();
            itemNewGroup.Caption="New Group...";
            itemNewGroup.Tag="NewRootGroup";
            itemNewGroup.ImageIndex=0;
            itemNewGroup.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            RootContextMenu.AddItem( itemNewGroup );

            itemRefresh=new DevExpress.XtraBars.BarButtonItem();
            itemRefresh.Caption="Refresh";
            itemRefresh.Tag="Refresh";
            itemRefresh.ImageIndex=48;
            itemRefresh.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            RootContextMenu.AddItem( itemRefresh );

            
            RootContextMenu.Manager=OwnerStudio.StudioBarManager;
            #endregion
        }
        void ViewTree_MouseUp ( object sender , MouseEventArgs e )
        {
            TreeList tree=sender as TreeList;
            if ( e.Button==MouseButtons.Right&&ModifierKeys==Keys.None&&tree.State==TreeListState.Regular )
            {

                Point pt=tree.PointToClient( MousePosition );
                TreeListHitInfo info=tree.CalcHitInfo( pt );
                if ( info.HitInfoType==HitInfoType.Cell||info.HitInfoType==HitInfoType.StateImage )
                {
                    tree.FocusedNode=info.Node;

                    MyData obj=(MyData)this.GetDataRecordByNode( info.Node );
                    if ( obj==null||obj.InnerData==null )
                        return;

                    if ( obj.InnerData is STViewGroupsInfo )
                        GroupContextMenu.ShowPopup( MousePosition );
                    else if ( obj.InnerData is STViewsInfo )
                        ViewContextMenu.ShowPopup( MousePosition );
                }
                else if ( info.HitInfoType==HitInfoType.Empty )
                {
                    RootContextMenu.ShowPopup( MousePosition );
                }
            }
          
        }

        void Menu_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            MyData obj=(MyData)this.GetDataRecordByNode( this.FocusedNode );

            STViewGroupsController groupCtrl=new STViewGroupsController();
            STViewsController viewCtrl=new STViewsController();

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Refresh" )
            {
                RefreshViewList();
            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="NewView"&&this.Selection.Count>0 )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewGroupsInfo groupInfo=(STViewGroupsInfo)obj.InnerData;
                if ( groupInfo!=null )
                {
                    #region Create ViewInfo
                    STViewsInfo viewInfo=new STViewsInfo();
                    viewInfo.FK_STViewGroupID=groupInfo.STViewGroupID;
                    viewInfo.STViewNo="View";
                    int i=0;
                    while ( viewCtrl.GetObjectByNo( viewInfo.STViewNo )!=null )
                    {
                        viewInfo.STViewNo+=i;
                    }
                    viewInfo.STViewName=viewInfo.STViewNo;
                    String strXML=ABCControls.ABCView.GetEmptyXMLLayout( viewInfo.STViewName ).InnerXml;
                    viewInfo.STViewXML=ABCHelper.StringCompressor.CompressString( strXML );
                    viewCtrl.CreateObject( viewInfo );

                    #endregion

                    new MyData( obj , new object[] { viewInfo.STViewNo , viewInfo.STViewID } , viewInfo );
                    this.RefreshDataSource();
                }
            }
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="NewGroup"&&this.Selection.Count>0 )
            {

                #region Create GroupInfo
                STViewGroupsInfo groupInfo=new STViewGroupsInfo();
                groupInfo.No="Group";
                int i=0;
                while ( groupCtrl.GetObjectByNo( groupInfo.No )!=null )
                {
                    groupInfo.No+=i;
                }
                groupInfo.Name=groupInfo.No;
                groupCtrl.CreateObject( groupInfo );

                #endregion

                new MyData( this.DataSource as MyData , new object[] { groupInfo.No , groupInfo.STViewGroupID } , groupInfo );
                this.RefreshDataSource();
            }
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="NewChildGroup"&&this.Selection.Count>0 )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewGroupsInfo groupInfo=(STViewGroupsInfo)obj.InnerData;
                if ( groupInfo!=null )
                {

                    #region Create GroupInfo
                    STViewGroupsInfo newGroupInfo=new STViewGroupsInfo();
                    newGroupInfo.No="Group";
                    int i=0;
                    while ( groupCtrl.GetObjectByNo( newGroupInfo.No )!=null )
                    {
                        newGroupInfo.No+=i;
                    }
                    newGroupInfo.Name=newGroupInfo.No;
                    newGroupInfo.FK_STViewGroupID=groupInfo.STViewGroupID;
                    groupCtrl.CreateObject( newGroupInfo );

                    #endregion

                    new MyData( obj , new object[] { newGroupInfo.No , newGroupInfo.STViewGroupID } , newGroupInfo );
                    this.RefreshDataSource();
                }
            }
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="NewRootGroup" )
            {

                #region Create GroupInfo
                STViewGroupsInfo newGroupInfo=new STViewGroupsInfo();
                newGroupInfo.No="Group";
                int i=0;
                while ( groupCtrl.GetObjectByNo( newGroupInfo.No )!=null )
                {
                    newGroupInfo.No+=i;
                }
                newGroupInfo.Name=newGroupInfo.No;
                groupCtrl.CreateObject( newGroupInfo );

                #endregion

                new MyData( this.DataSource as MyData , new object[] { newGroupInfo.No , newGroupInfo.STViewGroupID } , newGroupInfo );
                this.RefreshDataSource();

            }
            if ( e.Item.Tag!=null&&( e.Item.Tag.ToString()=="RenameView"||e.Item.Tag.ToString()=="RenameGroup" )&&this.Selection.Count>0 )
            {
                this.OptionsBehavior.Editable=true;
                this.ShowEditor();
            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="OpenView" )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewsInfo info=(STViewsInfo)new STViewsController().GetObjectByID( ( (STViewsInfo)obj.InnerData ).GetID() );
                if ( info==null )
                    return;

                OwnerStudio.Worker.OpenFromDatabase( info );

            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="RunView" )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewsInfo info=(STViewsInfo)new STViewsController().GetObjectByID( ( (STViewsInfo)obj.InnerData ).GetID() );
                if ( info==null )
                    return;

                ABCScreen.ABCScreenFactory.RunScreen( info , ViewMode.Test );

            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="DeleteView" )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                DialogResult result=ABCHelper.ABCMessageBox.Show( "Do you want to delete selected View ? " , "Delete View" , MessageBoxButtons.YesNo );
                if ( result==DialogResult.Yes )
                {

                    STViewsInfo info=(STViewsInfo)obj.InnerData;
                    viewCtrl.DeleteObject( info.STViewID );
                    obj.parentCore.childrenCore.Remove( obj );
                    obj.parentCore=null;
                    this.RefreshDataSource();
                }
            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="DeleteGroup" )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                DialogResult result=ABCHelper.ABCMessageBox.Show( "Do you want to delete selected Group and all children View ? " , "Delete Group" , MessageBoxButtons.YesNo );
                if ( result==DialogResult.Yes )
                {

                    STViewGroupsInfo info=(STViewGroupsInfo)obj.InnerData;
                    viewCtrl.DeleteObjectsByFK( "FK_STViewGroupID" , info.STViewGroupID );
                    groupCtrl.DeleteObject( info.STViewGroupID );
                    obj.parentCore.childrenCore.Remove( obj );
                    obj.parentCore=null;
                    this.RefreshDataSource();
                }
            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Import"&&this.Selection.Count>0 )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewsInfo info=(STViewsInfo)new STViewsController().GetObjectByID( ( (STViewsInfo)obj.InnerData ).GetID() );
                if ( info==null )
                    return;

                OpenFileDialog dlg=new OpenFileDialog();
                dlg.Filter="xml files (*.xml)|*.xml|All files (*.*)|*.*";
                dlg.FilterIndex=0;
                dlg.RestoreDirectory=true;
                dlg.Title="Import Layout from XML file";
                if ( dlg.ShowDialog()==DialogResult.OK )
                {
                    XmlDocument doc=new XmlDocument();
                    doc.Load( dlg.FileName );
                    info.STViewXML=ABCHelper.StringCompressor.CompressString( doc.InnerXml );
                    new STViewsController().UpdateObject( info );
                    ABCHelper.ABCMessageBox.Show( "Saved ...!" , "Import From XML" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                }


            }
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Export"&&this.Selection.Count>0 )
            {
                if ( obj==null||obj.InnerData==null )
                    return;

                STViewsInfo info=(STViewsInfo)new STViewsController().GetObjectByID( ( (STViewsInfo)obj.InnerData ).GetID() );
                if ( info==null)
                    return;

                SaveFileDialog dlg=new SaveFileDialog();
                dlg.Filter="xml files (*.xml)|*.xml|All files (*.*)|*.*";
                dlg.FilterIndex=0;
                dlg.RestoreDirectory=true;
                dlg.Title="Export Layout to XML file";
                if ( dlg.ShowDialog()==DialogResult.OK&&!String.IsNullOrWhiteSpace( dlg.FileName ) )
                {
                    XmlDocument doc=new XmlDocument();
                    string strDecompress=ABCHelper.StringCompressor.DecompressString( info.STViewXML );
                    doc.LoadXml( strDecompress );
                    doc.Save( dlg.FileName );
                    ABCHelper.ABCMessageBox.Show( "Export ...!" , "Export To XML" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                }
            }


        }
        
        #endregion

        void ViewTree_CustomDrawNodeCell ( object sender , DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e )
        {
            MyData obj=(MyData)this.GetDataRecordByNode( e.Node );
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.InnerData is STViewGroupsInfo )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );

            if ( e.Node.Selected )
                e.Appearance.Font=new Font( e.Appearance.Font ,e.Appearance.Font.Style |  FontStyle.Underline );
           
        }
   
        void ViewTree_GetStateImage ( object sender , GetStateImageEventArgs e )
        {
            MyData obj=(MyData)this.GetDataRecordByNode( e.Node );
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.InnerData is STViewGroupsInfo)
                e.NodeImageIndex=39;
            else if ( obj.InnerData is STViewsInfo)
                e.NodeImageIndex=37;
            //else if ( e.Node.Level==0 )
            //    e.NodeImageIndex=78;
        }

        void ViewTree_DoubleClick ( object sender , EventArgs e )
        {
            if ( this.Selection.Count>0&&this.Selection[0].Level>=1 )
            {
                MyData obj=(MyData)this.GetDataRecordByNode( this.Selection[0] );
                if ( obj==null||obj.InnerData==null )
                    return;

                if ( obj.InnerData is STViewsInfo )
                {
                    //      Nodes.TreeListNode node = this.FindNodeByID( this.Selection[0].Id);
                    //  object strID=this.GetNodeValue(this. this.Selection[0] , this.Columns[1] );
                    //   object strID=this.Selection[0].GetValue(this.Columns[1] );
                    STViewsController viewCtrl=new STViewsController();
                    STViewsInfo info=(STViewsInfo)viewCtrl.GetObjectByID( ( obj.InnerData as STViewsInfo ).STViewID );
                    if ( info!=null )
                        OwnerStudio.Worker.OpenFromDatabase( info );
                }
            }
        }

        #region Data
        public void RefreshViewList ( )
        {
            MyData tlDataSource=new MyData( null , null , null );

            //   MyData root=new MyData( tlDataSource , new string[] { "Root" , "" } , null );

            STViewGroupsController viewGroupCtrl=new STViewGroupsController();
            STViewsController viewCtrl=new STViewsController();

            DataSet ds=viewGroupCtrl.GetDataSet( "SELECT * FROM STViewGroups ORDER BY SortOrder" );
            List<BusinessObject> lst=viewGroupCtrl.GetListFromDataset( ds );
            Dictionary<Guid , MyData> listNodes=new Dictionary<Guid , MyData>();
            foreach ( STViewGroupsInfo objGroup in lst )
            {
                MyData group=new MyData( tlDataSource , new object[] { objGroup.No , objGroup.STViewGroupID } , objGroup );
                listNodes.Add( objGroup.STViewGroupID , group );

            }
            foreach ( Guid iID in listNodes.Keys )
            {
                if ( ( listNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID.HasValue
                    &&( listNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID.Value!=Guid.Empty )
                {
                    foreach ( Guid iID2 in listNodes.Keys )
                    {

                        if ( iID2!=iID )
                        {
                            if ( ( listNodes[iID].InnerData as STViewGroupsInfo ).FK_STViewGroupID==( listNodes[iID2].InnerData as STViewGroupsInfo ).STViewGroupID )
                            {
                                listNodes[iID].parentCore.childrenCore.Remove( listNodes[iID] );
                                listNodes[iID].parentCore=listNodes[iID2];
                                listNodes[iID2].childrenCore.Add( listNodes[iID] );
                            }
                        }

                    }
                }
            }

            foreach ( Guid iID in listNodes.Keys )
            {
                DataSet dsView=viewCtrl.GetDataSet( String.Format( "SELECT * FROM STViews WHERE FK_STViewGroupID ='{0}' ORDER BY SortOrder" , iID ) );
                if ( dsView==null||dsView.Tables.Count<=0 )
                    continue;

                foreach ( DataRow dr in dsView.Tables[0].Rows )
                {
                    STViewsInfo objView=(STViewsInfo)viewCtrl.GetObjectFromDataRow( dr );
                    if ( objView!=null )
                        new MyData( listNodes[iID] , new object[] { objView.STViewNo , objView.STViewID } , objView );
                }
            }




            this.DataSource=tlDataSource;
            this.CollapseAll();
        }
        public class MyData : TreeList.IVirtualTreeListData
        {
            public MyData parentCore;
            public ArrayList childrenCore=new ArrayList();
            protected object[] cellsCore;
            protected int Level=0;
            public object InnerData;

            public MyData ( MyData parent , object[] cells , object data )
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
    }
}
