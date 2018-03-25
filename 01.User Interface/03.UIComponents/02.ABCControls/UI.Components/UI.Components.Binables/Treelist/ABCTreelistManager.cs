using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text;
using DevExpress.XtraTreeList;
using System.Reflection;
using System.Data;


using ABCControls;
using ABCProvider;

using ABCBusinessEntities;

namespace ABCControls
{
    public class TreeConfigData
    {
        [Editor( typeof( TableChooserEditor ) , typeof( UITypeEditor ) )]
        public String TableName { get; set; }
        public String Name { get; set; }

        
        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String ParentField { get; set; }
 
        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String ChildField { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( TableChooserEditor ) , typeof( UITypeEditor ) )]
        public String ParentTableName { get; set; }

        public int Level { get; set; }

        public bool DefaultLoad { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }

        [Editor( typeof( ABCControls.TypeEditor.FilterConditionEditor ) , typeof( UITypeEditor ) )]
        public String FilterCondition { get; set; }

        [Browsable(false)]
        public Dictionary<String , String> ColumnFieldNames { get; set; }

        public object Clone ( )
        {
            return (TreeConfigData)this.MemberwiseClone();
        }

    }
    public class TreeConfigNode : TreeList.IVirtualTreeListData
    {
        public TreeConfigNode ParentNode;
        public Dictionary<string , TreeConfigNode> ChildrenNodes=new Dictionary<string , TreeConfigNode>();
        public TreeConfigData InnerData;
    
        public TreeConfigNode ( )
        {
        }

        public TreeConfigNode ( TreeConfigNode parent , TreeConfigData _data )
        {
           
            this.ParentNode=parent;
         
            this.InnerData=_data;
            if ( parent!=null&&parent.InnerData!=null )
                this.InnerData.Level=parent.InnerData.Level+1;

            if ( this.ParentNode!=null )
            {
                if ( this.ParentNode.ChildrenNodes.ContainsKey( InnerData.Name )==false )
                    this.ParentNode.ChildrenNodes.Add( InnerData.Name , this );
                else
                    this.ParentNode.ChildrenNodes[InnerData.Name]=this;
            }
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes ( VirtualTreeGetChildNodesInfo info )
        {
            info.Children=ChildrenNodes.Values.ToArray();
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetCellValue ( VirtualTreeGetCellValueInfo info )
        {
            if ( InnerData!=null )
            {
                if ( info.Column.Tag!=null&&info.Column.Tag is ABCTreeListColumn.ColumnConfig )
                {
                    String strTemp=String.Empty;
                    if ( InnerData.ColumnFieldNames.TryGetValue( info.Column.Caption , out strTemp ) )
                    {
                        info.CellData=strTemp;
                        return;
                    }
                }
                else
                {
                    if ( info.Column.FieldName=="Name" )
                    {
                        info.CellData=InnerData.Name;
                        return;
                    }
                    if ( info.Column.FieldName=="TableName" )
                    {
                        info.CellData=InnerData.TableName;
                        return;
                    }
                    if ( info.Column.FieldName=="ParentField" )
                    {
                        info.CellData=InnerData.ParentField;
                        return;
                    }
                    if ( info.Column.FieldName=="ChildField" )
                    {
                        info.CellData=InnerData.ChildField;
                        return;
                    }
                    if ( info.Column.FieldName=="DefaultLoad" )
                    {
                        info.CellData=InnerData.DefaultLoad;
                        return;
                    }
                }
            }
        }

        void TreeList.IVirtualTreeListData.VirtualTreeSetCellValue ( VirtualTreeSetCellValueInfo info )
        {
            if ( InnerData!=null )
            {
                if ( info.Column.Tag!=null&&info.Column.Tag is ABCTreeListColumn.ColumnConfig )
                {
                    if ( InnerData.ColumnFieldNames.ContainsKey( info.Column.Caption ) )
                        InnerData.ColumnFieldNames[info.Column.Caption]=info.NewCellData.ToString();
                    else
                        InnerData.ColumnFieldNames.Add( info.Column.Caption , info.NewCellData.ToString() );
                
                    return;
                }
                else
                {
                    if ( info.Column.FieldName=="Name" )
                    {
                        InnerData.Name=info.NewCellData.ToString();
                        return;
                    }
                    if ( info.Column.FieldName=="TableName" )
                    {
                        InnerData.TableName=info.NewCellData.ToString();
                        return;
                    }
                    if ( info.Column.FieldName=="ParentField" )
                    {
                        InnerData.ParentField=info.NewCellData.ToString();
                        return;
                    }
                    if ( info.Column.FieldName=="ChildField" )
                    {
                        InnerData.ChildField=info.NewCellData.ToString();
                        return;
                    }
                    if ( info.Column.FieldName=="DefaultLoad" )
                    {
                        InnerData.DefaultLoad=(bool)info.NewCellData;
                        return;
                    }
                }
            }
           
        }
    }

    public class ABCTreelistManager
    {
        public TreeConfigNode RootConfig;
        public Dictionary<String , TreeConfigNode> ConfigList=new Dictionary<string , TreeConfigNode>();

        public ABCTreeListNode RootData=new ABCTreeListNode( null , null , null );
        public Dictionary<String , Dictionary<Guid , ABCTreeListNode>> DataList=new Dictionary<string , Dictionary<Guid , ABCTreeListNode>>();
        public ABCTreeList TreeList;

        public ABCTreelistManager ( ABCTreeList treelist )
        {
            RootConfig=new TreeConfigNode( null , null );
            TreeList=treelist;
       
        }

        public void Clear ( )
        {
            RootData=new ABCTreeListNode( null , null , null );
        }
    
        public void Invalidate ( IList dataList )
        {
            InvalidateCachingAllNodes();

            RootData=new ABCTreeListNode( null , null , null );
            RootData.Manager=this;

            if ( RootConfig.ChildrenNodes.Count>0 )
            {
                foreach ( String strObjectName in RootConfig.ChildrenNodes.Keys )
                {
                    String strTableName=RootConfig.ChildrenNodes[strObjectName].InnerData.TableName;
                    BusinessObjectController Controller=BusinessControllerFactory.GetBusinessController( strTableName );
                    foreach ( object objChild in dataList )
                    {
                        if ( objChild is BusinessObject==false )
                            continue;

                        ABCTreeListNode node=new ABCTreeListNode( RootData , strObjectName , objChild as BusinessObject );
                        node.CachingNode();
                    }

                    break;
                }
            }
            ExpandDataAll();

            TreeList.InnerTreeList.DataSource=RootData;
            TreeList.InnerTreeList.RefreshDataSource();
        }
        public void Invalidate ( DataTable table , String strObjectName )
        {
            InvalidateCachingAllNodes();

            RootData=new ABCTreeListNode( null , null , null );
            RootData.Manager=this;

            if ( RootConfig.ChildrenNodes.ContainsKey( strObjectName ) )
            {
                String strTableName=RootConfig.ChildrenNodes[strObjectName].InnerData.TableName;
                BusinessObjectController Controller=BusinessControllerFactory.GetBusinessController( strTableName );
                foreach ( DataRow dr in table.Rows )
                {
                    BusinessObject objChild=Controller.GetObjectFromDataRow( dr );
                    if ( objChild!=null )
                    {
                        ABCTreeListNode node=new ABCTreeListNode( RootData , strObjectName , objChild );
                        node.CachingNode();
                    }
                }
            }

            ExpandDataAll();

            TreeList.InnerTreeList.DataSource=RootData;
            TreeList.InnerTreeList.RefreshDataSource();
        }
        public void Invalidate ( DataTable table )
        {
            if(RootConfig.ChildrenNodes.Count>0)
            {
                foreach(String strKey in RootConfig.ChildrenNodes.Keys)
                {
                    Invalidate( table , strKey );
                    return;
                }
            }
    
        }
        public void Invalidate ( DataSet ds )
        {
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataTable table in ds.Tables )
                {
                    String strObjectName=table.TableName;
                    Invalidate( table , strObjectName );
                }
            }
        }

        public void InvalidateCachingAllNodes ( )
        {
            DataList.Clear();
            foreach ( TreeConfigNode configNode in ConfigList.Values )
            {
                try
                {
                    if ( configNode.InnerData!=null&&configNode.ParentNode!=null )
                    {
                        DataView view= DataCachingProvider.TryToGetDataView(configNode.InnerData.TableName ,false);
                       BusinessObjectController Controller=BusinessControllerFactory.GetBusinessController( configNode.InnerData.TableName );

                        //DataSet ds=Controller.GetAllObjects();
                        //if ( ds!=null&&ds.Tables.Count>0 )
                        //{
                            foreach ( DataRow dr in view.Table.Rows )
                            {
                                BusinessObject obj=Controller.GetObjectFromDataRow( dr );
                                if ( obj!=null )
                                {
                                    ABCTreeListNode node=new ABCTreeListNode( null , configNode.InnerData.Name , obj );
                                    node.Manager=this;
                                    node.CachingNode();
                                }
                            }
                   //     }

                    }
                }
                catch ( Exception ex )
                {
                }
            }
        }
        public void RefreshCachingAllNodes ( )
        {
            foreach ( TreeConfigNode configNode in ConfigList.Values )
                RefreshCachingNodes( configNode );
        }
        public void RefreshCachingNodes ( String strObjectName )
        {
            if(ConfigList.ContainsKey(strObjectName))
                RefreshCachingNodes( ConfigList[strObjectName] );
        }
        public void RefreshCachingNodes ( TreeConfigNode configNode )
        {
            try
            {
                if ( configNode.InnerData!=null&&configNode.ParentNode!=null )
                {
                    Dictionary<Guid , ABCTreeListNode> innerList=null;
                    if ( this.DataList.TryGetValue( configNode.InnerData.Name , out innerList )==false )
                    {
                        innerList=new Dictionary<Guid , ABCTreeListNode>();
                        this.DataList.Add( configNode.InnerData.Name , innerList );
                    }

                    DataView view=DataCachingProvider.TryToGetDataView( configNode.InnerData.TableName , false );
                    BusinessObjectController Controller=BusinessControllerFactory.GetBusinessController( configNode.InnerData.TableName );
                    String strPK=DataStructureProvider.GetPrimaryKeyColumn( configNode.InnerData.TableName );
                    foreach ( DataRow dr in view.Table.Rows )
                    {
                        BusinessObject obj=Controller.GetObjectFromDataRow( dr );
                        if ( obj!=null )
                        {
                            Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , strPK ) );
                            if ( innerList.ContainsKey( iID )==false )
                            {
                                ABCTreeListNode node=new ABCTreeListNode( null , configNode.InnerData.Name , obj );
                                node.Manager=this;
                                node.CachingNode();
                            }
                            else
                                innerList[iID].InnerData=obj;
                        }
                    }

                }
            }
            catch ( Exception ex )
            {
            }
        }

        public void ExpandDataAll ( )
        {
            foreach ( ABCTreeListNode configNode in RootData.ChildrenNodes.Values )
                configNode.ExpandData( true , true );
        }

        public void RefreshConfigList ( )
        {
            ConfigList.Clear();
            RefreshConfigList( RootConfig );
        }
        private void RefreshConfigList (TreeConfigNode currentNode)
        {
            if ( currentNode.InnerData !=null && ConfigList.ContainsKey( currentNode.InnerData.Name )==false )
                ConfigList.Add( currentNode.InnerData.Name , currentNode );

            foreach ( TreeConfigNode child in currentNode.ChildrenNodes.Values )
                RefreshConfigList( child );
        }
    }

    public class ABCTreeListNode : TreeList.IVirtualTreeListData
    {
        public ABCTreelistManager Manager;
        public ABCTreeListNode ParentNode;
        public Dictionary<Guid , ABCTreeListNode> ChildrenNodes=new Dictionary<Guid , ABCTreeListNode>();

        public String ObjectName;
        public BusinessObject InnerData;
        public int Level=0;

        public ABCTreeListNode ( ABCTreeListNode parent , String strObjectName , BusinessObject _data )
        {
            this.ObjectName=strObjectName;
            this.ParentNode=parent;
            if ( parent!=null )
            {
                this.Level=parent.Level+1;
                this.Manager=this.ParentNode.Manager;
            }

            this.InnerData=_data;
            if ( this.InnerData!=null )
            {
                Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( this.InnerData , DataStructureProvider.GetPrimaryKeyColumn( this.InnerData.AATableName ) ) );
                if ( this.ParentNode!=null )
                    this.ParentNode.ChildrenNodes.Add( iID , this );

                CachingNode();
            }
        }

        public void CachingNode ( )
        {
            try
            {
                if ( Manager!=null )
                {
                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( this.InnerData , DataStructureProvider.GetPrimaryKeyColumn( this.InnerData.AATableName ) ) );

                    Dictionary<Guid , ABCTreeListNode> innerList=null;
                    if ( this.Manager.DataList.TryGetValue( this.ObjectName , out innerList )==false )
                    {
                        innerList=new Dictionary<Guid , ABCTreeListNode>();
                        this.Manager.DataList.Add( this.ObjectName , innerList );
                    }
                    if ( innerList.ContainsKey( iID )==false )
                        innerList.Add( iID , this );
                    else
                        innerList[iID]=this;
                }
            }
            catch ( Exception ex )
            {
            }
        }

        void TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes ( VirtualTreeGetChildNodesInfo info )
        {
            info.Children=ChildrenNodes.Values.ToArray();
        }

        void TreeList.IVirtualTreeListData.VirtualTreeGetCellValue ( VirtualTreeGetCellValueInfo info )
        {
            if ( Manager==null )
                return;

            TreeConfigNode config=null;
            if ( Manager.ConfigList.TryGetValue( ObjectName , out config )==false )
                return;

            String strField=String.Empty;
            if ( config.InnerData.ColumnFieldNames.TryGetValue( info.Column.Caption , out strField )==false )
                return;

            if ( String.IsNullOrWhiteSpace( strField ) )
                return;

            String strFirstField=strField.Split( ':' )[0];
            if ( DataStructureProvider.IsForeignKey( InnerData.AATableName , strFirstField ) )
            {
                Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( InnerData , strFirstField ) );
                if ( iID!=Guid.Empty )
                    info.CellData=DataCachingProvider.GetCachingObjectAccrossTable( InnerData.AATableName , iID , strField );
            }
            else
            {
                info.CellData=ABCBusinessEntities.ABCDynamicInvoker.GetValue( InnerData , strFirstField );
                String strEnum=DataConfigProvider.TableConfigList[InnerData.AATableName].FieldConfigList[strFirstField].AssignedEnum;
                if ( info.CellData!=null&&String.IsNullOrWhiteSpace( info.CellData.ToString() )==false&&EnumProvider.EnumList.ContainsKey( strEnum ) )
                {
                    info.CellData=EnumProvider.EnumList[strEnum].Items[info.CellData.ToString()].CaptionVN;
                }
            }

        }

        void TreeList.IVirtualTreeListData.VirtualTreeSetCellValue ( VirtualTreeSetCellValueInfo info )
        {
            if ( Manager==null )
                return;

            TreeConfigNode config=null;
            if ( Manager.ConfigList.TryGetValue( ObjectName , out config )==false )
                return;

            String strField=String.Empty;
            if ( config.InnerData.ColumnFieldNames.TryGetValue( info.Column.Caption , out strField )==false )
                return;

            if ( String.IsNullOrWhiteSpace( strField ) )
                return;

            if ( strField.Contains( ":" ) )
            {
                info.Cancel=true;
                return;
            }

            ABCBusinessEntities.ABCDynamicInvoker.SetValue( InnerData , strField , info.NewCellData );

        }

        public void ExpandData ( Boolean includeChildren , Boolean defaultOnly )
        {
            if ( Manager==null||this.InnerData==null )
                return;

            TreeConfigNode config=null;
            if ( Manager.ConfigList.TryGetValue( ObjectName , out config )==false )
                return;

            #region Current Childrens
            if ( config.ParentNode==null||config.ParentNode.InnerData==null )
            {
                if ( DataStructureProvider.IsTableColumn( config.InnerData.TableName , config.InnerData.ParentField )&&
                     DataStructureProvider.IsTableColumn( config.InnerData.TableName , config.InnerData.ChildField ) )
                {
                    String strPK=DataStructureProvider.GetPrimaryKeyColumn( config.InnerData.TableName );
                    object objParentValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( this.InnerData , config.InnerData.ParentField );
                    foreach ( ABCTreeListNode childNode in Manager.DataList[config.InnerData.Name].Values )
                    {
                        object objChildValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( childNode.InnerData , config.InnerData.ChildField );
                        if ( objChildValue!=null&&(int)objChildValue==(int)objParentValue )
                        {
                            Guid iChildID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( childNode.InnerData , strPK ) );
                            if ( this.ChildrenNodes.ContainsKey(iChildID )==false )
                            {
                                childNode.ParentNode=this;
                                this.ChildrenNodes.Add( iChildID , childNode );
                                childNode.ExpandData( includeChildren , defaultOnly );
                            }
                        }
                    }
                }
            }
            #endregion

            #region Other Config Childrens
            foreach ( TreeConfigNode childConfig in config.ChildrenNodes.Values )
            {
                if ( defaultOnly&&childConfig.InnerData!=null&&childConfig.InnerData.DefaultLoad==false )
                    continue;

                String strPK=DataStructureProvider.GetPrimaryKeyColumn( childConfig.InnerData.TableName );

                object objParentValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( this.InnerData , childConfig.InnerData.ParentField );

                foreach ( ABCTreeListNode childNode in Manager.DataList[childConfig.InnerData.Name].Values )
                {
                    object objChildValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( childNode.InnerData , childConfig.InnerData.ChildField );
                    if ( objChildValue!=null&&(int)objChildValue==(int)objParentValue )
                    {
                        Guid iChildID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( childNode.InnerData , strPK ) );
                        if ( this.ChildrenNodes.ContainsKey( iChildID )==false )
                        {
                            childNode.ParentNode=this;
                            this.ChildrenNodes.Add(iChildID , childNode );
                            if ( includeChildren )
                                childNode.ExpandData( true , defaultOnly );
                        }
                    }
                }

                #region old

                //#region Generate Query
                //ABCHelper.ConditionBuilder strBuilder=new ABCHelper.ConditionBuilder();
                //strBuilder.Append( String.Format( @"SELECT * FROM {0}" , childConfig.InnerData.TableName ) );
                //if ( objParentValue is String||objParentValue  is DateTime)
                //    strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , childConfig.InnerData.ChildField , objParentValue.ToString() ) );
                //else
                //    strBuilder.AddCondition( String.Format( @" {0} = {1} " , childConfig.InnerData.ChildField , objParentValue ) );


                //String strFilterCondintion=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( childConfig.InnerData.FilterCondition ) );
                //strBuilder.AddCondition( strFilterCondintion );

                //if ( DataStructureProvider.IsExistABCStatus( childConfig.InnerData.TableName ) )
                //    strBuilder.AddCondition( Generation.QueryGenerator.GenerateCondition( childConfig.InnerData.TableName , ABCCommon.ABCConstString.ColumnType.ABCStatus ) );

                //strBuilder.AddCondition( Security.DataAuthentication.GetAuthenticationString( childConfig.InnerData.TableName ) );

                //#endregion

                //BusinessObjectController Controller=BusinessControllerFactory.GetBusinessController( childConfig.InnerData.TableName );

                //DataSet ds=DataQueryProvider.RunQuery( strBuilder.ToString() );
                //if ( ds!=null&&ds.Tables.Count>0 )
                //{
                //    foreach ( DataRow dr in ds.Tables[0].Rows )
                //    {
                //        BusinessObject objChild=Controller.GetObjectFromDataRow( dr );
                //        if ( objChild!=null )
                //        {
                //            ABCTreeListNode childNode=new ABCTreeListNode( this , childConfig.InnerData.Name , objChild );
                //            if ( includeChildren )
                //                childNode.Expand( true , defaultOnly );
                //        }
                //    }
                //} 
                #endregion
            }
            #endregion

        }

        public void RefreshData ( Boolean includeParent , Boolean includeChildren , Boolean defaultOnly )
        {
            #region Current Node
            if ( InnerData!=null )
            {
                BusinessObjectController ctrller=BusinessControllerFactory.GetBusinessController( InnerData.AATableName );
                String strPK=DataStructureProvider.GetPrimaryKeyColumn( InnerData.AATableName );
                Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( InnerData , strPK ) );
                InnerData=ctrller.GetObjectByID( iID );
                if ( InnerData==null )
                {
                    if ( this.ParentNode!=null )
                    {
                        this.ParentNode.ChildrenNodes.Remove( iID );
                        this.ParentNode=null;
                    }
                    Dictionary<Guid , ABCTreeListNode> innerList=null;
                    if ( this.Manager.DataList.TryGetValue( this.ObjectName , out innerList ) )
                    {
                        if ( innerList.ContainsKey( iID ) )
                            innerList.Remove( iID );
                    }
                }
            }
            #endregion

            List<ABCTreeListNode> lstTemps=new List<ABCTreeListNode>();
            foreach ( ABCTreeListNode childNode in this.ChildrenNodes.Values )
                lstTemps.Add( childNode );

            if ( includeChildren )
            {
                foreach ( ABCTreeListNode childNode in lstTemps )
                    childNode.RefreshData( false , includeChildren , defaultOnly );
            }
            if ( includeParent&&this.ParentNode!=null )
                this.ParentNode.RefreshData( true , false , defaultOnly );

            ExpandData( includeChildren , defaultOnly );
        }
    }
}
