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

using ABCControls;

using ABCProvider;
namespace ABCStudio
{
    public enum FiledNodeType
    {
        Table,
        Field,
        Control,
        Foreign,
        List
    }
    public class FieldNodeInfo
    {
        public String Display { get; set; }
        public FiledNodeType Type;
        public ABCScreen.ABCBindingConfig Config;
        public ABCToolboxItem ToolboxItem;

        public String FieldName;
        public String TableName;
    }
    public partial class FieldBindingTree : DevExpress.XtraTreeList.TreeList
    {
        public Studio OwnerStudio;
        public FieldBindingTree ()
        {
        }

        public void Initialize ( )
        {
            this.StateImageList=OwnerStudio.ToolbarImageList;

            this.OptionsBehavior.ShowEditorOnMouseUp=false;
            this.OptionsBehavior.CloseEditorOnLostFocus=true;
            this.OptionsBehavior.ImmediateEditor=false;
            this.OptionsSelection.UseIndicatorForSelection=false;
            this.OptionsView.ShowColumns=true;
            this.OptionsView.ShowVertLines=false;
            this.OptionsView.ShowHorzLines=false;
            this.OptionsView.ShowFocusedFrame=false;
            this.OptionsBehavior.DragNodes=true;

            this.GetStateImage+=new GetStateImageEventHandler( Tree_GetStateImage );
            this.CustomDrawNodeCell+=new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler( Tree_CustomDrawNodeCell );
            this.MouseDown+=new MouseEventHandler( FieldBindingTree_MouseDown );
            this.BeforeDragNode+=new BeforeDragNodeEventHandler( Tree_BeforeDragNode );
            this.DragOver+=new DragEventHandler( Tree_DragOver );
            this.DragDrop+=new DragEventHandler( Tree_DragDrop );
         
            #region Columns
            DevExpress.XtraTreeList.Columns.TreeListColumn colField=new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colField.Caption="Field";
            colField.FieldName="Display";
            colField.Visible=true;
            colField.VisibleIndex=0;
            colField.OptionsColumn.AllowEdit=false;

            this.Columns.AddRange( new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colField } );

            #endregion

            InitPopupMenu();

        }


        #region UI
        void Tree_CustomDrawNodeCell ( object sender , DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( e.Node );
            if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                return;

            FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;

            if ( data.Type==FiledNodeType.Table||data.Type==FiledNodeType.List )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );

            if ( e.Node.Selected )
                e.Appearance.Font=new Font( e.Appearance.Font , e.Appearance.Font.Style|FontStyle.Underline );

        }
        void Tree_GetStateImage ( object sender , GetStateImageEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( e.Node );
            if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                return;

            FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;

            if ( data.Type==FiledNodeType.Table )
                e.NodeImageIndex=106;
            else if ( data.Type==FiledNodeType.Field )
                e.NodeImageIndex=107;
            else if ( data.Type==FiledNodeType.Foreign )
                e.NodeImageIndex=109;
            else if ( data.Type==FiledNodeType.List )
                e.NodeImageIndex=106;
            else if ( data.Type==FiledNodeType.Control )
            {
                if ( data.Display=="GridControl"||data.Display=="BandedGridControl"||data.Display=="PivotGridControl" )
                    e.NodeImageIndex=95;
                if ( data.Display=="TextEdit" )
                    e.NodeImageIndex=85;
                if ( data.Display=="GridLookUpEdit" )
                    e.NodeImageIndex=100;
                if ( data.Display=="LookUpEdit" )
                    e.NodeImageIndex=100;
                if ( data.Display=="MemoEdit" )
                    e.NodeImageIndex=101;
                if ( data.Display=="MemoExEdit" )
                    e.NodeImageIndex=102;
                if ( data.Display=="CalcEdit" )
                    e.NodeImageIndex=87;
                if ( data.Display=="SpinEdit" )
                    e.NodeImageIndex=87;
                if ( data.Display=="CheckEdit" )
                    e.NodeImageIndex=88;
                if ( data.Display=="CheckedListBox" )
                    e.NodeImageIndex=89;
                if ( data.Display=="DateEdit" )
                    e.NodeImageIndex=92;
                if ( data.Display=="TimeEdit" )
                    e.NodeImageIndex=92;
                if ( data.Display=="Label" )
                    e.NodeImageIndex=99;
                if ( data.Display=="RadioGroup" )
                    e.NodeImageIndex=104;
                if ( data.Display=="RichEditControl" )
                    e.NodeImageIndex=105;
                if ( data.Display=="SearchControl" )
                    e.NodeImageIndex=112;
                if ( data.Display=="SearchPanel" )
                    e.NodeImageIndex=112;
                if ( data.Display=="DataPanel" )
                    e.NodeImageIndex=113;
            }

        }
        void Tree_DragDrop ( object sender , DragEventArgs e )
        {
            e.Effect=DragDropEffects.None;
        }
        void Tree_BeforeDragNode ( object sender , DevExpress.XtraTreeList.BeforeDragNodeEventArgs e )
        {

            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( e.Node );
            if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                return;

            FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;
            if ( data.Type==FiledNodeType.Table )
            {
                e.CanDrag=false;
                return;
            }

        }
        #endregion

        void Tree_DragOver ( object sender , DragEventArgs e )
        {

            if ( e.Data is DataObject )
            {
                DataObject data=(DataObject)e.Data;

                DevExpress.XtraTreeList.Nodes.TreeListNode treeNode=(DevExpress.XtraTreeList.Nodes.TreeListNode)data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) );
                if ( treeNode!=null )
                {
                    ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( treeNode );
                    if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                        return;

                    FieldNodeInfo fieldNode=(FieldNodeInfo)obj.InnerData;
                    if (( fieldNode.Type==FiledNodeType.Field||fieldNode.Type==FiledNodeType.Foreign)&&obj.ChildrenNodes.Count>0 )
                    {
                        foreach ( ABCCommonTreeListNode childNode in obj.ChildrenNodes )
                        {
                            if ( (FieldNodeInfo)childNode.InnerData==null )
                                continue;

                            if ( ( (FieldNodeInfo)childNode.InnerData ).ToolboxItem!=null )
                            {
                                fieldNode=(FieldNodeInfo)childNode.InnerData;
                                break;
                            }
                        }
                    }
                    if ( fieldNode.ToolboxItem!=null )
                    {
                        OwnerStudio.Toolbox.SelectedToolboxItem=fieldNode.ToolboxItem;
                        data=OwnerStudio.Toolbox.SerializeToolboxItem( fieldNode.ToolboxItem ) as DataObject;
                        try
                        {
                            OwnerStudio.Toolbox.DoDragDrop( data , DragDropEffects.Copy );
                            OwnerStudio.Toolbox.SelectedToolboxItem=null;
                            return;
                        }
                        catch ( Exception ex )
                        {
                            MessageBox.Show( ex.Message , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                        }

                        OwnerStudio.Toolbox.SelectedToolboxItem=null;
                    }
                }
            }

           // e.Effect=DragDropEffects.None;
        }

        #region Data

        public void RefreshFieldNodes ( HostSurface Surface,Boolean IsNeedReload )
        {

            try
            {
                if ( Surface==null||Surface.DesignerHost.RootComponent==null )
                {
                    this.DataSource=null;
                    this.RefreshDataSource();
                    return;
                }
                this.SuspendLayout();

                if ( Surface.TreeFieldNodes==null||IsNeedReload )
                    ReloadFieldNodes( Surface );

                this.DataSource=Surface.TreeFieldNodes;
                this.RefreshDataSource();
                this.Refresh();
                if ( this.Nodes.Count>0 )
                    this.FocusedNode=this.Nodes[0];
      
                this.ResumeLayout( false );
              
                foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode node in this.Nodes )
                    ExpandTableNode( node );
            }
            catch ( Exception ex )
            {
                this.ResumeLayout( false );
            }

        }

        public void ReloadFieldNodes ( HostSurface Surface )
        {
            ABCView view=(ABCView)Surface.DesignerHost.RootComponent;

            Surface.TreeFieldNodes=new ABCCommonTreeListNode( null , null );

            if ( view!=null&&view.DataConfig!=null&&view.DataConfig.BindingList!=null )
            {
                foreach ( ABCScreen.ABCBindingConfig config in view.DataConfig.BindingList.TreeValues )
                    InitTable( Surface.TreeFieldNodes , config );
            }
        }

        private void InitTable ( ABCCommonTreeListNode parentNode , ABCScreen.ABCBindingConfig bindInfo )
        {
            if ( DataStructureProvider.DataTablesList.ContainsKey( bindInfo.TableName )==false )
                return;

            ABCCommonTreeListNode node=null;
            FieldNodeInfo fieldInfo=new FieldNodeInfo();
            fieldInfo.Display=DataConfigProvider.GetTableCaption( bindInfo.TableName )+String.Format( @" ( {0} ) " , bindInfo.Name );
            fieldInfo.Type=FiledNodeType.List;
            fieldInfo.Config=bindInfo;
            node=new ABCCommonTreeListNode( parentNode , fieldInfo );

            InitControl( node , typeof( ABCSearchPanel ) , "SearchPanel" );
            InitControl( node , typeof( ABCAutoSearchPanel ) , "AutoSearchPanel" );
            InitControl( node , typeof( ABCDataPanel ) , "DataPanel" );
            if ( bindInfo.IsList )
            {
                fieldInfo.Type=FiledNodeType.List;

                #region IsList

                #region Grid
                fieldInfo=new FieldNodeInfo();
                fieldInfo.Type=FiledNodeType.Control;
                fieldInfo.Display="GridControl";
                fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCGridControl ) );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
                fieldInfo.ToolboxItem.Binding.TableName=bindInfo.TableName;
                ABCCommonTreeListNode listNode=new ABCCommonTreeListNode( node , fieldInfo );
                #endregion

                #region BandedGrid
                fieldInfo=new FieldNodeInfo();
                fieldInfo.Type=FiledNodeType.Control;
                fieldInfo.Display="BandedGridControl";
                fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCGridBandedControl ) );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
                fieldInfo.ToolboxItem.Binding.TableName=bindInfo.TableName;
                listNode=new ABCCommonTreeListNode( node , fieldInfo );
                #endregion

                #region BandedGrid
                fieldInfo=new FieldNodeInfo();
                fieldInfo.Type=FiledNodeType.Control;
                fieldInfo.Display="PivotGridControl";
                fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCPivotGridControl ) );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
                fieldInfo.ToolboxItem.Binding.TableName=bindInfo.TableName;
                listNode=new ABCCommonTreeListNode( node , fieldInfo );
                #endregion

                #region BandedGrid
                fieldInfo=new FieldNodeInfo();
                fieldInfo.Type=FiledNodeType.Control;
                fieldInfo.Display="TreeList";
                fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCTreeList ) );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
                fieldInfo.ToolboxItem.Binding.TableName=bindInfo.TableName;
                listNode=new ABCCommonTreeListNode( node , fieldInfo );
                #endregion
                #endregion
            }
            else
            {
                fieldInfo.Type=FiledNodeType.Table;
            }

            foreach ( String strFieldName in DataStructureProvider.DataTablesList[bindInfo.TableName].ColumnsList.Keys )
                InitField( node , bindInfo.TableName , strFieldName );


            foreach ( ABCScreen.ABCBindingConfig childInfo in bindInfo.Children.Values )
                InitTable( node , childInfo );
        }

        private void InitField ( ABCCommonTreeListNode parentNode ,String strTableName, String strFieldName )
        {
            ABCScreen.ABCBindingConfig bindInfo=((FieldNodeInfo)parentNode.InnerData).Config;
            String strDiplay=DataConfigProvider.GetFieldCaption( strTableName , strFieldName );
            if ( strDiplay==strFieldName )
                return;

            FieldNodeInfo fieldInfo=new FieldNodeInfo();
            fieldInfo.Display=strDiplay;
            fieldInfo.Type=FiledNodeType.Field;
            fieldInfo.Config=bindInfo;
            if ( String.IsNullOrWhiteSpace( ( (FieldNodeInfo)parentNode.InnerData ).FieldName ) ==false)
                fieldInfo.FieldName=( (FieldNodeInfo)parentNode.InnerData ).FieldName+":";

            fieldInfo.FieldName+=strFieldName;
            fieldInfo.TableName=strTableName;

            ABCCommonTreeListNode node=new ABCCommonTreeListNode( parentNode , fieldInfo );
            InitControls( node );

            if ( DataStructureProvider.IsForeignKey( strTableName , strFieldName ) )
                fieldInfo.Type=FiledNodeType.Foreign;
        }

        private void InitControls ( ABCCommonTreeListNode node )
        {
            FieldNodeInfo fieldNodeInfo=(FieldNodeInfo)node.InnerData;
            List<Type> lstType=ABCControls.ABCPresentHelper.GetControlTypes( fieldNodeInfo.Config.TableName , fieldNodeInfo.FieldName );
            foreach ( Type type in lstType )
            {
                String strName=type.Name.Split( new String[] { "ABC" } , StringSplitOptions.None )[1];
                InitControl( node , type , strName );
            }
        }

       
        private void InitControl ( ABCCommonTreeListNode node , Type ControlType,String strDisplay )
        {
            ABCScreen.ABCBindingConfig bindInfo=( (FieldNodeInfo)node.InnerData ).Config;

            FieldNodeInfo fieldInfo=new FieldNodeInfo();
            fieldInfo.Type=FiledNodeType.Control;
            fieldInfo.Display=strDisplay;
            //fieldInfo.Config=bindInfo;
            //fieldInfo.FieldName=( (FieldNodeInfo)node.InnerData ).FieldName;
            //fieldInfo.TableName=( (FieldNodeInfo)node.InnerData ).TableName;

            if ( ControlType==typeof( ABCCheckEdit )||ControlType==typeof( ABCCheckPanel )||ControlType==typeof( ABCRadioGroup )||ControlType==typeof( ABCLabel ) )
            {
                fieldInfo.ToolboxItem=new ABCToolboxItem( ControlType );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.FieldName=( (FieldNodeInfo)node.InnerData ).FieldName;
                fieldInfo.ToolboxItem.Binding.TableName=( (FieldNodeInfo)node.InnerData ).Config.TableName;
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
            }
            else if ( ControlType==typeof( ABCSearchPanel )||ControlType==typeof( ABCAutoSearchPanel ) )
            {
                fieldInfo.ToolboxItem=new ABCToolboxItem( ControlType );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.TableName=( (FieldNodeInfo)node.InnerData ).Config.TableName;
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
            }
            else if ( ControlType==typeof( ABCDataPanel ) )
            {
                fieldInfo.ToolboxItem=new ABCToolboxItem( ControlType );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.TableName=( (FieldNodeInfo)node.InnerData ).Config.TableName;
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
            }
            else
            {
                if ( ControlType==typeof( ABCSearchControl ) )
                    fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCSearchControl ) );
                else
                    fieldInfo.ToolboxItem=new ABCToolboxItem( typeof( ABCBindingBaseEdit ) );
                fieldInfo.ToolboxItem.Binding=new ABCControls.ABCBindingInfo();
                fieldInfo.ToolboxItem.Binding.FieldName=( (FieldNodeInfo)node.InnerData ).FieldName;
                fieldInfo.ToolboxItem.Binding.TableName=( (FieldNodeInfo)node.InnerData ).Config.TableName;
                fieldInfo.ToolboxItem.Binding.BusName=bindInfo.Name;
                fieldInfo.ToolboxItem.Binding.ControlType=ControlType;
            }
            new ABCCommonTreeListNode( node , fieldInfo );
        }


        private void ExpandTableNode ( DevExpress.XtraTreeList.Nodes.TreeListNode node )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( node );
            if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                return;

            FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;
            if ( data.Type==FiledNodeType.Table||data.Type==FiledNodeType.List)
                node.Expanded=true;
            if ( data.Type==FiledNodeType.Field )
                node.Expanded=false;

            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode childNode in node.Nodes )
                ExpandTableNode( childNode );
        }

        public void ExpandForeignNode ( ABCCommonTreeListNode node )
        {
            FieldNodeInfo fieldInfo=(FieldNodeInfo)node.InnerData;
            if ( fieldInfo==null )
                return;
            String[] strArr=fieldInfo.FieldName.Split( ':' );
            if ( strArr.Length<=0 )
                return;

            String strFK=strArr[strArr.Length-1];

            String strTableName=DataStructureProvider.GetTableNameOfForeignKey( fieldInfo.TableName , strFK );
            if ( DataStructureProvider.DataTablesList.ContainsKey( strTableName )==false )
                return;

            foreach ( String strFieldName in DataStructureProvider.DataTablesList[strTableName].ColumnsList.Keys )
                InitField( node , strTableName , strFieldName );
        }
        #endregion

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

            ContextMenu.Manager=OwnerStudio.StudioBarManager;

        }

        void FieldBindingTree_MouseDown ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right&&ModifierKeys==Keys.None&&this.State==TreeListState.Regular )
            {
                Point pt=this.PointToClient( MousePosition );
                TreeListHitInfo info=this.CalcHitInfo( pt );
                if ( info.HitInfoType==HitInfoType.Cell||info.HitInfoType==HitInfoType.StateImage )
                {
                    this.FocusedNode=info.Node;

                    ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( this.FocusedNode );
                    if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                        return;

                    FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;
                    if ( data.Type==FiledNodeType.Foreign )
                        ContextMenu.ShowPopup( MousePosition );
                }
            }
        }

        void Menu_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Explore"&&this.Selection.Count>0 )
            {
                ABCCommonTreeListNode obj=(ABCCommonTreeListNode)this.GetDataRecordByNode( this.FocusedNode );
                if ( obj==null||(FieldNodeInfo)obj.InnerData==null )
                    return;

                FieldNodeInfo data=(FieldNodeInfo)obj.InnerData;
                if ( data.Type==FiledNodeType.Foreign )
                {
                    ExpandForeignNode( obj );
                    this.RefreshDataSource();
                    ExpandTableNode( this.FocusedNode );
                }
            }

        }

        #endregion
    }
}
