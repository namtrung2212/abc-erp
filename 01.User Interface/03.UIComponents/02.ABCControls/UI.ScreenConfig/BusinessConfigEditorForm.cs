using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

using ABCProvider;
using ABCScreen;


namespace ABCControls
{
    public partial class ABCBusinessConfigEditorForm : DevExpress.XtraEditors.XtraForm
    {
        public ABCView OwnerView;

        public ABCBusinessConfigEditorForm ( ABCView view)
        {
            OwnerView=view;

            InitializeComponent();

            InitTreelist();

            this.Load+=new EventHandler( Form_Load );
            this.PropertyGrid.CellValueChanged+=new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler( PropertyGrid_CellValueChanged );
            this.repositoryItemButtonEdit1.ButtonPressed+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( repositoryItemButtonEdit1_ButtonPressed );
        }

        #region Data
        public ABCScreen.ABCScreenConfig DataConfig;

        List<ABCScreen.ABCBindingConfig> AddedList=new List<ABCScreen.ABCBindingConfig>();
   
        #region Init Data
  
        private void InitNodes ( )
        {
            AddedList.Clear();

            ABCCommonTreeListNode rootNode=new ABCCommonTreeListNode( null , null );
            foreach ( ABCScreen.ABCBindingConfig bindInfo in this.DataConfig.BindingList.TreeValues)
                InitNodes( rootNode , bindInfo );

            treeBinding.DataSource=rootNode;
            treeBinding.ExpandAll();
            if ( treeBinding.Nodes.Count>0 )
                treeBinding.FocusedNode=treeBinding.Nodes[0];
        }
        private ABCScreen.ABCBindingConfig InitNodes ( ABCCommonTreeListNode parentNode , ABCScreen.ABCBindingConfig bindInfo )
        {
            if ( AddedList.Contains( bindInfo ) )
                return null;
            AddedList.Add( bindInfo );

            ABCScreen.ABCBindingConfig newBindInfo=(ABCScreen.ABCBindingConfig )bindInfo.Clone();
            ABCCommonTreeListNode node=new ABCCommonTreeListNode( parentNode , newBindInfo );

            foreach ( ABCScreen.ABCBindingConfig childInfo in bindInfo.Children.Values )
            {
                ABCScreen.ABCBindingConfig newChildBindInfo=InitNodes( node , childInfo );
                if ( newChildBindInfo!=null )
                {
                    newChildBindInfo.Parent=newBindInfo;
                    newBindInfo.Children.Add( newChildBindInfo.Name , newChildBindInfo );
                }
            }
            return newBindInfo;
        }

        #endregion

        #region FormClosed  => GetNewBindingConfig
        public ABCScreenConfig NewDataConfig;
        public void GetNewDataConfig ( )
        {
            NewDataConfig=new ABCScreenConfig( OwnerView );
        
            AddedList.Clear();
            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode node in this.treeBinding.Nodes )
            {

                ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( node );
                if ( obj==null||(ABCBindingConfig)obj.InnerData==null )
                    continue;

                ABCBindingConfig bindInfo=(ABCBindingConfig)obj.InnerData;
                if ( AddedList.Contains( bindInfo ) )
                    continue;

                ABCBindingConfig info=GetNewBindingInfo( node );
                if ( NewDataConfig.BindingList.ContainsKey( bindInfo.Name )==false )
                    NewDataConfig.BindingList.Add( bindInfo.Name , bindInfo );

            }
        }
        private ABCBindingConfig GetNewBindingInfo ( DevExpress.XtraTreeList.Nodes.TreeListNode node )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( node );
            if ( obj==null||(ABCScreen.ABCBindingConfig)obj.InnerData==null )
                return null;

            ABCBindingConfig bindInfo=(ABCBindingConfig)obj.InnerData;
            bindInfo.Children.Clear();

            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode childNode in node.Nodes )
            {
                ABCBindingConfig childInfo=GetNewBindingInfo( childNode );
                childInfo.Parent=bindInfo;
                childInfo.ParentName=bindInfo.Name;
                bindInfo.Children.Add( childInfo.Name , childInfo );
            }

            if ( AddedList.Contains( bindInfo )==false )
                AddedList.Add( bindInfo );

            return bindInfo;
        }

        #endregion

        public void UpdateFieldName ( ABCCommonTreeListNode obj )
        {
          
            if ( obj==null||obj.InnerData==null )
                return;

            if ( obj.ParentNode!=null )
            {
                ABCCommonTreeListNode objparent=obj.ParentNode;

                ABCBindingConfig currentBind=(ABCBindingConfig)obj.InnerData;
                ABCBindingConfig parentBind=(ABCBindingConfig)objparent.InnerData;

                if ( currentBind!=null&&parentBind!=null )
                {
                    if ( currentBind.ParentName!=parentBind.TableName||( currentBind.ParentName==parentBind.TableName&&( String.IsNullOrWhiteSpace( currentBind.ParentField )||String.IsNullOrWhiteSpace( currentBind.ChildField ) ) ) )
                    {
                        if ( DataStructureProvider.IsExistedTable( parentBind.TableName ) )
                            currentBind.ParentField=DataStructureProvider.GetPrimaryKeyColumn( parentBind.TableName );
                        if ( DataStructureProvider.IsExistedTable( currentBind.TableName ) )
                            currentBind.ChildField=DataStructureProvider.GetTable( currentBind.TableName ).GetForeignKeyOfTableName( parentBind.TableName );
                    }
                    currentBind.ParentName=parentBind.Name;

                }
            }
            //foreach ( TreeListNode nodeChild in node.Nodes )
            //    UpdateFieldName( nodeChild );

        }
        #endregion

        #region Treelist
      
        System.Windows.Forms.ImageList curImageList=new System.Windows.Forms.ImageList();
        private void InitTreelist ( )
        {
            #region Image
            curImageList.Images.Add( ABCImageList.GetImage16x16( "Catagory" ) );
            curImageList.Images.Add( ABCImageList.GetImage16x16( "View" ) );
            this.treeBinding.StateImageList=curImageList;
            this.treeBinding.GetStateImage+=new DevExpress.XtraTreeList.GetStateImageEventHandler( treeBinding_GetStateImage );
            #endregion

            this.treeBinding.OptionsBehavior.Editable=false;
            this.treeBinding.OptionsBehavior.ShowEditorOnMouseUp=false;
            this.treeBinding.OptionsBehavior.CloseEditorOnLostFocus=true;
            this.treeBinding.OptionsBehavior.ImmediateEditor=false;
            this.treeBinding.OptionsSelection.UseIndicatorForSelection=false;
            this.treeBinding.OptionsView.ShowColumns=true;
            this.treeBinding.OptionsView.ShowVertLines=false;
            this.treeBinding.OptionsView.ShowHorzLines=false;
            this.treeBinding.OptionsView.ShowFocusedFrame=false;
            this.treeBinding.OptionsBehavior.DragNodes=true;
            this.treeBinding.OptionsSelection.MultiSelect=false;

            this.treeBinding.FocusedNodeChanged+=new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler( treeBinding_FocusedNodeChanged );
            treeBinding.BindingContextChanged+=new EventHandler( treeBinding_BindingContextChanged );
            treeBinding.AfterDragNode+=new NodeEventHandler( treeBinding_AfterDragNode );
            treeBinding.DragDrop+=new DragEventHandler( treeBinding_DragDrop );
            treeBinding.CustomDrawNodeCell+=new CustomDrawNodeCellEventHandler( treeBinding_CustomDrawNodeCell );

            #region Column
            treeBinding.BeginUpdate();
            treeBinding.Columns.Add();
            treeBinding.Columns[0].FieldName="Name";
            treeBinding.Columns[0].Caption="Name";
            treeBinding.Columns[0].VisibleIndex=0;

            treeBinding.Columns.Add();
            treeBinding.Columns[1].FieldName="TableName";
            treeBinding.Columns[1].Caption="TableName";
            treeBinding.Columns[1].VisibleIndex=1;
            treeBinding.EndUpdate(); 
            #endregion

            InitPopupMenu();
            this.treeBinding.MouseDown+=new MouseEventHandler( treeBinding_MouseDown );
        }

        void treeBinding_CustomDrawNodeCell ( object sender , CustomDrawNodeCellEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( e.Node );
            if ( obj==null||(ABCScreen.ABCBindingConfig)obj.InnerData==null )
                return;

            if ( ((ABCScreen.ABCBindingConfig)obj.InnerData).IsMainObject )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
        }
      
        void treeBinding_DragDrop ( object sender , DragEventArgs e )
        {
            object obj=e.Data.GetData( typeof( TreeListNode ) );
            if ( obj is TreeListNode==false )
            {
                e.Effect=DragDropEffects.None;
                return;
            }

            TreeListNode dragNode=(TreeListNode)obj;

            Point p=treeBinding.PointToClient( new Point( e.X , e.Y ) );
            TreeListNode targetNode=treeBinding.CalcHitInfo( p ).Node;

            if ( dragNode!=null&&targetNode!=null&&dragNode!=targetNode )
            {
                ABCCommonTreeListNode objDrag=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( dragNode );
                ABCCommonTreeListNode objTarget=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( targetNode );
                if ( objDrag!=null&&objTarget!=null )
                {
                    if ( objDrag.ParentNode!=null )
                        objDrag.ParentNode.ChildrenNodes.Remove( objDrag );

                    objDrag.ParentNode=objTarget;
                    objTarget.ChildrenNodes.Add( objDrag );

                    e.Effect=DragDropEffects.Move;
                    treeBinding.RefreshDataSource();
                    treeBinding.ExpandAll();
                    UpdateFieldName( objDrag );
                    return;
                }
            }
            e.Effect=DragDropEffects.None;
        }

        void treeBinding_BindingContextChanged ( object sender , EventArgs e )
        {
            this.treeBinding.ExpandAll();
        }
        void treeBinding_AfterDragNode ( object sender , NodeEventArgs e )
        {
            ABCCommonTreeListNode node=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( e.Node );
            if ( node==null||(ABCBindingConfig)node.InnerData==null )
                UpdateFieldName( node );
        
            treeBinding.RefreshDataSource();
            this.treeBinding.ExpandAll();
        }
        void treeBinding_FocusedNodeChanged ( object sender , DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( e.Node );
            if ( obj==null||(ABCScreen.ABCBindingConfig)obj.InnerData==null )
            {
                PropertyGrid.SelectedObject=null;
                FieldFilterGrid.DataSource=null;
                FieldFilterGrid.RefreshDataSource();
                return;
            }
            PropertyGrid.SelectedObject=(ABCScreen.ABCBindingConfig)obj.InnerData;
            FieldFilterGrid.DataSource=( (ABCScreen.ABCBindingConfig)obj.InnerData ).FieldFilterConditions;
            FieldFilterGrid.RefreshDataSource();
        }
        void treeBinding_GetStateImage ( object sender , DevExpress.XtraTreeList.GetStateImageEventArgs e )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( e.Node );
            if ( obj==null||(ABCScreen.ABCBindingConfig)obj.InnerData==null )
                return;

            if ( ( (ABCScreen.ABCBindingConfig)obj.InnerData ).IsList )
                e.NodeImageIndex=0;
            else
                e.NodeImageIndex=1;

        }

        #region Context Menu

        DevExpress.XtraBars.PopupMenu ContextMenu=new DevExpress.XtraBars.PopupMenu();

        public void InitPopupMenu ( )
        {
            #region ColumnContextMenu

            DevExpress.XtraBars.BarButtonItem itemNew=new DevExpress.XtraBars.BarButtonItem();
            itemNew.Caption="New";
            itemNew.Tag="New";
            itemNew.ImageIndex=54;
            itemNew.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemNew );

            DevExpress.XtraBars.BarButtonItem itemNewRoot=new DevExpress.XtraBars.BarButtonItem();
            itemNewRoot.Caption="New Root";
            itemNewRoot.Tag="NewRoot";
            itemNewRoot.ImageIndex=54;
            itemNewRoot.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemNewRoot );

            DevExpress.XtraBars.BarButtonItem itemDelete=new DevExpress.XtraBars.BarButtonItem();
            itemDelete.Caption="Delete";
            itemDelete.Tag="Delete";
            itemDelete.ImageIndex=54;
            itemDelete.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Menu_ItemClick );
            ContextMenu.AddItem( itemDelete );

          ContextMenu.Manager=this.barManager1;
            #endregion

        }

        void treeBinding_MouseDown ( object sender , MouseEventArgs e )
        {
            if ( e.Button==MouseButtons.Right&&ModifierKeys==Keys.None&&treeBinding.State==TreeListState.Regular )
            {
                Point pt=treeBinding.PointToClient( MousePosition );
                TreeListHitInfo info=treeBinding.CalcHitInfo( pt );
                if ( info.Node!=null )
                    treeBinding.FocusedNode=info.Node;
                ContextMenu.ShowPopup( MousePosition );
            }
        }

        void Menu_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="NewRoot" )
            {
                foreach ( String strNewItem in new TableChooserForm().ShowChoose() )
                {
                    ABCBindingConfig newInfo=new ABCBindingConfig( strNewItem );
                    newInfo.Name=GetNewName( newInfo.Name );
                    if ( this.treeBinding.FocusedNode==null )
                        newInfo.IsMainObject=true;
                    new ABCCommonTreeListNode( (ABCCommonTreeListNode)this.treeBinding.DataSource , newInfo );
                }
                this.treeBinding.RefreshDataSource();
            }
        

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="New" )
            {
                foreach ( String strNewItem in new TableChooserForm().ShowChoose() )
                {
                    ABCBindingConfig newInfo=new ABCBindingConfig( strNewItem );
                    newInfo.Name=GetNewName( newInfo.Name );

                    if ( this.treeBinding.FocusedNode==null )
                    {
                        new ABCCommonTreeListNode( (ABCCommonTreeListNode)this.treeBinding.DataSource , newInfo );
                        newInfo.IsMainObject=true;
                    }
                    else
                    {
                        ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( this.treeBinding.FocusedNode );
                        if ( obj==null||(ABCBindingConfig)obj.InnerData==null )
                            return;
                        ABCCommonTreeListNode node=new ABCCommonTreeListNode( obj , newInfo );
                        UpdateFieldName( node );
                    }
                }
                this.treeBinding.RefreshDataSource();
              //  this.treeBinding.FocusedNode.ExpandAll();
            }

            if ( e.Item.Tag!=null&&e.Item.Tag.ToString()=="Delete" )
            {
                if ( this.treeBinding.FocusedNode!=null )
                {
                    DialogResult result=ABCHelper.ABCMessageBox.Show( "Do you want remove this Binding Information and all children ? " , "Message" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
                    if ( result==System.Windows.Forms.DialogResult.Yes )
                    {
                        this.treeBinding.DeleteSelectedNodes();
                        this.treeBinding.RefreshDataSource();
                    }
                }
            }

        }


        #endregion

        #endregion

        #region Events

        void Form_Load ( object sender , EventArgs e )
        {
            InitNodes();
        }
        private void btnSave_Click ( object sender , EventArgs e )
        {
            GetNewDataConfig();
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.OK;
        }
        private void btnCancel_Click ( object sender , EventArgs e )
        {
            this.Close();
            this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
        }

        private void AddRoot_Click ( object sender , EventArgs e )
        {
            foreach ( String strNewItem in new TableChooserForm().ShowChoose() )
            {
                ABCBindingConfig newInfo=new ABCBindingConfig( strNewItem );
                newInfo.Name=GetNewName( newInfo.Name );
                new ABCCommonTreeListNode( (ABCCommonTreeListNode)this.treeBinding.DataSource , newInfo );
            }
            this.treeBinding.RefreshDataSource();
            treeBinding.ExpandAll();
        }

        void PropertyGrid_CellValueChanged ( object sender , DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e )
        {
            ABCCommonTreeListNode node=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( treeBinding.FocusedNode );
            if ( node==null||(ABCBindingConfig)node.InnerData==null )
                return;

            if ( e.Row.Name=="rowTableName" )
                UpdateFieldName( node );

            if ( e.Row.Name=="rowIsList" )
            {
                if ( e.Value!=null&&Convert.ToBoolean( e.Value )&&( (ABCBindingConfig)node.InnerData ).IsMainObject )
                    ( (ABCBindingConfig)node.InnerData ).IsMainObject=false;
            }
            if ( e.Row.Name=="rowDisplayOnly" )
            {
                if ( e.Value!=null&&Convert.ToBoolean( e.Value )&&( (ABCBindingConfig)node.InnerData ).AutoSave )
                    ( (ABCBindingConfig)node.InnerData ).AutoSave=false;
            }
            if ( e.Row.Name=="rowAutoSave" )
            {
                if ( e.Value!=null&&Convert.ToBoolean( e.Value ) )
                {
                    ( (ABCBindingConfig)node.InnerData ).DisplayOnly=false;
                    ( (ABCBindingConfig)node.InnerData ).IsMainObject=false;
                }
            }
            if ( e.Row.Name=="rowIsMainObject" )
            {
                if ( e.Value!=null&&Convert.ToBoolean( e.Value ) )
                {
                    ( (ABCBindingConfig)node.InnerData ).AutoSave=false;

                    if ( ( (ABCBindingConfig)node.InnerData ).IsList )
                        ( (ABCBindingConfig)node.InnerData ).IsList=false;

                    foreach ( DevExpress.XtraTreeList.ViewInfo.RowInfo row in this.treeBinding.ViewInfo.RowsInfo.Rows )
                    {
                        ABCCommonTreeListNode nodeData=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( row.Node );
                        if ( nodeData== node|| nodeData==null||(ABCBindingConfig)nodeData.InnerData==null )
                            continue;

                        ( (ABCBindingConfig)nodeData.InnerData ).IsMainObject=false;
                    }
                }

            }
            PropertyGrid.Refresh();
            treeBinding.RefreshDataSource();
            
        }


        void repositoryItemButtonEdit1_ButtonPressed ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {

            ABCBindingConfig.FieldFilterConfig data=FieldFilterGridView.GetRow( FieldFilterGridView.FocusedRowHandle ) as ABCBindingConfig.FieldFilterConfig;
            if ( data!=null )
            {
                ABCCommonForms.FilterBuilderForm form=new ABCCommonForms.FilterBuilderForm( data.TableName );
                form.SetFilterString( data.FilterString );
                if ( form.ShowDialog()==System.Windows.Forms.DialogResult.OK )
                    data.FilterString=form.FilterString;
            }
        }
    
        #endregion

        #region Utils

        private String GetNewName ( String strOldName )
        {
            String strNewName=strOldName;

            bool isOK=true;
            int i=0;
            do
            {
                isOK=true;
                foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode node in treeBinding.Nodes )
                {
                    if ( IsExistedName( node , strNewName ) )
                    {
                        i++;
                        strNewName=strOldName+i.ToString();
                        isOK=false;
                        break;
                    }
                }

            } while ( isOK==false );

            return strNewName;
        }

        private bool IsExistedName ( DevExpress.XtraTreeList.Nodes.TreeListNode node , String strName )
        {
            ABCCommonTreeListNode obj=(ABCCommonTreeListNode)treeBinding.GetDataRecordByNode( node );
            if ( obj==null||(ABCBindingConfig)obj.InnerData==null )
                return false;

            if ( ( (ABCBindingConfig)obj.InnerData ).Name==strName )
                return true;

            foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode nodeChild in node.Nodes )
            {
                if ( IsExistedName( nodeChild , strName ) )
                    return true;
            }

            return false;
        } 
        #endregion
    }
}