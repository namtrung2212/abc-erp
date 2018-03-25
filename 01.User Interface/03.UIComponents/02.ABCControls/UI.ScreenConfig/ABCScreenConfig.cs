using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;

using ABCProvider;
using ABCControls;

namespace ABCScreen
{
    public class ABCScreenConfig
    {
        public ABCView OwnerView;
        public Dictionary<String , object> ParameterList=new Dictionary<string , object>();

        public ABCScreenConfig (ABCView view  )
        {
            OwnerView=view;
        }

        #region BindingList

        public ABCBindingConfigCollection BindingList=new ABCBindingConfigCollection();
   
        #region DeSerialization

        private List<String> DeSerialedList=new List<string>();
        public void DeSerialization ( XmlDocument doc )
        {
            BindingList.Clear();
            DeSerialedList.Clear();

            XmlNodeList nodeList=doc.GetElementsByTagName( "B" );
            foreach ( XmlNode node in nodeList )
            {
                try
                {
                    String strName=node.Attributes["name"].Value.ToString();
                    if ( !DeSerialedList.Contains( strName ) )
                    {
                        ABCBindingConfig currentInfo=DeSerialization( node );

                        if ( !BindingList.ContainsKey( currentInfo.Name ) )
                            BindingList.Add( currentInfo.Name , currentInfo );
                    }
                }
                catch ( Exception ex )
                {
                    ABCHelper.ABCMessageBox.Show( "Bug : Deserialize DataConfig!" );
                }
            }
        }
        public ABCBindingConfig DeSerialization ( XmlNode currentNode )
        {

            ABCBindingConfig currentInfo=new ABCBindingConfig();

            try
            {

                #region Current Node
                if ( currentInfo!=null )
                    currentInfo.Children.Clear();

                currentInfo.Name=currentNode.Attributes["name"].Value.ToString();
                currentInfo.TableName=currentNode.Attributes["table"].Value.ToString();
                currentInfo.OwnerConfig=this;

                if ( currentNode.Attributes["IsList"]!=null )
                    currentInfo.IsList=Convert.ToBoolean( currentNode.Attributes["IsList"].Value.ToString() );

                if ( currentNode.Attributes["IsMainObject"]!=null )
                    currentInfo.IsMainObject=Convert.ToBoolean( currentNode.Attributes["IsMainObject"].Value.ToString() );

                if ( currentNode.Attributes["DisplayOnly"]!=null )
                    currentInfo.DisplayOnly=Convert.ToBoolean( currentNode.Attributes["DisplayOnly"].Value.ToString() );

                if ( currentNode.Attributes["AutoSave"]!=null )
                    currentInfo.AutoSave=Convert.ToBoolean( currentNode.Attributes["AutoSave"].Value.ToString() );
            
                if ( currentNode.Attributes["ConfirmSaveChildren"]!=null )
                    currentInfo.ConfirmSaveChildren=Convert.ToBoolean( currentNode.Attributes["ConfirmSaveChildren"].Value.ToString() );
                
                if ( currentNode.Attributes["DefaultField"]!=null )
                    currentInfo.DefaultField=currentNode.Attributes["DefaultField"].Value.ToString();

                if ( currentNode.Attributes["DefaultValue"]!=null )
                    currentInfo.DefaultValue=currentNode.Attributes["DefaultValue"].Value.ToString();

                if ( currentNode.Attributes["TopCount"]!=null )
                    currentInfo.TopCount=Convert.ToInt32( currentNode.Attributes["TopCount"].Value.ToString() );

                if ( currentNode.Attributes["CurrentUserOnly"]!=null )
                    currentInfo.CurrentUserOnly=Convert.ToBoolean( currentNode.Attributes["CurrentUserOnly"].Value.ToString() );

                if ( currentNode.Attributes["CurrentUserGroupOnly"]!=null )
                    currentInfo.CurrentUserGroupOnly=Convert.ToBoolean( currentNode.Attributes["CurrentUserGroupOnly"].Value.ToString() );

                if ( currentNode.Attributes["CurrentEmployeeOnly"]!=null )
                    currentInfo.CurrentEmployeeOnly=Convert.ToBoolean( currentNode.Attributes["CurrentEmployeeOnly"].Value.ToString() );

                if ( currentNode.Attributes["CurrentCompanyUnitOnly"]!=null )
                    currentInfo.CurrentCompanyUnitOnly=Convert.ToBoolean( currentNode.Attributes["CurrentCompanyUnitOnly"].Value.ToString() );

                if ( currentNode.Attributes["parentField"]!=null )
                    currentInfo.ParentField=currentNode.Attributes["parentField"].Value.ToString();

                if ( currentNode.Attributes["childField"]!=null )
                    currentInfo.ChildField=currentNode.Attributes["childField"].Value.ToString();

                if ( currentNode.Attributes["parentField1"]!=null )
                    currentInfo.ParentField1=currentNode.Attributes["parentField1"].Value.ToString();

                if ( currentNode.Attributes["childField1"]!=null )
                    currentInfo.ChildField1=currentNode.Attributes["childField1"].Value.ToString();

                if ( currentNode.Attributes["parentField2"]!=null )
                    currentInfo.ParentField2=currentNode.Attributes["parentField2"].Value.ToString();

                if ( currentNode.Attributes["childField2"]!=null )
                    currentInfo.ChildField2=currentNode.Attributes["childField2"].Value.ToString();

                if ( currentNode.Attributes["parentField3"]!=null )
                    currentInfo.ParentField3=currentNode.Attributes["parentField3"].Value.ToString();

                if ( currentNode.Attributes["childField3"]!=null )
                    currentInfo.ChildField3=currentNode.Attributes["childField3"].Value.ToString();

                if ( currentNode.Attributes["SQLExtension"]!=null )
                    currentInfo.SQLExtension=currentNode.Attributes["SQLExtension"].Value.ToString();

                if ( currentNode.Attributes["SQLQuery"]!=null )
                    currentInfo.SQLQuery=currentNode.Attributes["SQLQuery"].Value.ToString();

                if ( currentNode.Attributes["filterString"]!=null )
                    currentInfo.FilterCondition=currentNode.Attributes["filterString"].Value.ToString();

                if ( currentNode.Attributes["SQLFilterString"]!=null )
                    currentInfo.SQLFilterCondition=currentNode.Attributes["SQLFilterString"].Value.ToString();

                foreach ( XmlNode nodeFilter in currentNode.SelectNodes( "Filter" ) )
                {
                    String strField=nodeFilter.Attributes["Field"].Value.ToString();
                    String strFilterString=nodeFilter.InnerText;

                    ABCBindingConfig.FieldFilterConfig config=new ABCBindingConfig.FieldFilterConfig();
                    config.Field=strField;
                    config.FilterString=strFilterString;
                    config.TableName=DataStructureProvider.GetTableNameOfForeignKey( currentInfo.TableName , strField );
                    currentInfo.FieldFilterConditions.Add( config );

                }
                #endregion

                foreach ( XmlNode node in currentNode.SelectNodes( "B" ) )
                {
                    ABCBindingConfig bindInfo=DeSerialization( node );
                    currentInfo.Children.Add( bindInfo.Name , bindInfo );
                    bindInfo.Parent=currentInfo;
                    bindInfo.ParentName=currentInfo.Name;
                }

                DeSerialedList.Add( currentInfo.Name );
            }
            catch ( Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( String.Format( "Can not Deserialize ABCBindingConfig : {0}" , currentInfo.Name ) );
            }
            return currentInfo;
        }
        
        #endregion

        #region Serialization
        private List<String> SerialedList=new List<string>();
        public void Serialization ( XmlElement root )
        {
            SerialedList.Clear();
            foreach ( String strKey in BindingList.TreeKeys )
            {
                if ( SerialedList.Contains( strKey )==false )
                {
                    XmlElement el=Serialization( root.OwnerDocument , BindingList[strKey] );
                    root.AppendChild( el );
                }
            }
        }
        public XmlElement Serialization ( XmlDocument doc , ABCBindingConfig bindInfo )
        {
            #region Current BindInfo
            XmlElement ele=doc.CreateElement( "B" );
            ele.SetAttribute( "name" , bindInfo.Name );
            ele.SetAttribute( "table" , bindInfo.TableName );

            if ( bindInfo.IsList )
                ele.SetAttribute( "IsList" , bindInfo.IsList.ToString() );

            if ( bindInfo.IsMainObject )
                ele.SetAttribute( "IsMainObject" , bindInfo.IsMainObject.ToString() );

            ele.SetAttribute( "DisplayOnly" , bindInfo.DisplayOnly.ToString() );
            ele.SetAttribute( "AutoSave" , bindInfo.AutoSave.ToString() );
            ele.SetAttribute( "ConfirmSaveChildren" , bindInfo.ConfirmSaveChildren.ToString() );
            ele.SetAttribute( "DefaultField" , bindInfo.DefaultField.ToString() );
            ele.SetAttribute( "DefaultValue" , bindInfo.DefaultValue.ToString() );
            ele.SetAttribute( "TopCount" , bindInfo.TopCount.ToString() );

            
            ele.SetAttribute( "CurrentUserOnly" , bindInfo.CurrentUserOnly.ToString() );
            ele.SetAttribute( "CurrentUserGroupOnly" , bindInfo.CurrentUserGroupOnly.ToString() );
            ele.SetAttribute( "CurrentEmployeeOnly" , bindInfo.CurrentEmployeeOnly.ToString() );
            ele.SetAttribute( "CurrentCompanyUnitOnly" , bindInfo.CurrentCompanyUnitOnly.ToString() );

            if ( string.IsNullOrWhiteSpace( bindInfo.SQLExtension )==false )
                ele.SetAttribute( "SQLExtension" , bindInfo.SQLExtension );

            if ( string.IsNullOrWhiteSpace( bindInfo.SQLQuery )==false )
                ele.SetAttribute( "SQLQuery" , bindInfo.SQLQuery );

            if ( string.IsNullOrWhiteSpace( bindInfo.ParentField ) ==false)
                ele.SetAttribute( "parentField" , bindInfo.ParentField );
            if ( string.IsNullOrWhiteSpace( bindInfo.ChildField )==false )
                ele.SetAttribute( "childField" , bindInfo.ChildField );

            if ( string.IsNullOrWhiteSpace( bindInfo.ParentField1 )==false )
                ele.SetAttribute( "parentField1" , bindInfo.ParentField1 );
            if ( string.IsNullOrWhiteSpace( bindInfo.ChildField1 )==false )
                ele.SetAttribute( "childField1" , bindInfo.ChildField1 );

            if ( string.IsNullOrWhiteSpace( bindInfo.ParentField2 )==false )
                ele.SetAttribute( "parentField2" , bindInfo.ParentField2 );
            if ( string.IsNullOrWhiteSpace( bindInfo.ChildField2)==false )
                ele.SetAttribute( "childField2" , bindInfo.ChildField2 );

            if ( string.IsNullOrWhiteSpace( bindInfo.ParentField3 )==false )
                ele.SetAttribute( "parentField3" , bindInfo.ParentField3 );
            if ( string.IsNullOrWhiteSpace( bindInfo.ChildField3 )==false )
                ele.SetAttribute( "childField3" , bindInfo.ChildField3 );

            if ( string.IsNullOrWhiteSpace( bindInfo.FilterCondition )==false )
                ele.SetAttribute( "filterString" , bindInfo.FilterCondition );
            if ( string.IsNullOrWhiteSpace( bindInfo.SQLFilterCondition )==false )
                ele.SetAttribute( "SQLFilterString" , bindInfo.SQLFilterCondition );

            if ( bindInfo.FieldFilterConditions.Count>0 )
            {
                foreach ( ABCBindingConfig.FieldFilterConfig config in bindInfo.FieldFilterConditions )
                {
                    //if ( String.IsNullOrWhiteSpace( config.FilterString )==false )
                    //{
                        XmlElement eleFilter=doc.CreateElement( "Filter" );
                        eleFilter.SetAttribute( "Field" , config.Field );
                        eleFilter.InnerText=config.FilterString;
                        ele.AppendChild( eleFilter );
                //    }
                }
            }
            #endregion

            foreach ( ABCBindingConfig childInfo in bindInfo.Children.Values )
            {
                XmlElement childEle=Serialization( doc , childInfo );
                ele.AppendChild( childEle );
            }

            SerialedList.Add( bindInfo.Name );
            return ele;
        } 
        #endregion 

        #endregion
     
        #region User Variables

        #endregion
    }

    public class ABCVariableConfig
    {

    }

    //can kiem tra lan truyen 3-4 layer
    public class ABCBindingConfig : ICloneable
    {
        public ABCBindingConfig Parent;
        public ABCScreenConfig OwnerConfig;

        #region Own
        [Category( "Bindding" )]
        public String Name { get; set; }

        [Editor( typeof( TableChooserEditor ) , typeof( UITypeEditor ) )]
        [Category( "Bindding" )]
        [DisplayName( "TableName" ) , Description( "TableName of Screen BusinessObject" )]
        public String TableName { get; set; }

        [Category( "Bindding" )]
        public bool IsList { get; set; }

        [Category( "Bindding" )]
        public bool IsMainObject { get; set; }

        [Category( "Data" )]
        public bool DisplayOnly { get; set; }

        [Category( "Data" )]
        public bool AutoSave { get; set; }

        [Category( "Data" )]
        public bool ConfirmSaveChildren { get; set; }

        [Category( "Data" )]
        public String DefaultField { get; set; }

        [Category( "Data" )]
        public String DefaultValue { get; set; }

        [Category( "Data" )]
        public int TopCount { get; set; }

        [Category( "Data" )]
        [DisplayName( "SQLQuery" ) , Description( "Which type to use..." )]
        public String SQLQuery { get; set; }

        [Category( "Data" )]
        [DisplayName( "SQLExtension" ) , Description( "Which type to use..." )]
        public String SQLExtension { get; set; }

        public class FieldFilterConfig
        {
            public String Field { get; set; }
            public String FilterString { get; set; }
            public String TableName { get; set; }
        }
        public List< FieldFilterConfig> FieldFilterConditions=new List<FieldFilterConfig>();
        #endregion
    
        [Browsable(false)]
        public bool HasChildren { get { return ( Children.Count>0 ); }}
      
        #region Relation

        [Category( "Relation" )]
        [DisplayName( "CurrentUserOnly" ) , Description( "Which type to use..." )]
        public Boolean CurrentUserOnly { get; set; }

        [Category( "Relation" )]
        [DisplayName( "CurrentUserGroupOnly" ) , Description( "Which type to use..." )]
        public Boolean CurrentUserGroupOnly { get; set; }

        [Category( "Relation" )]
        [DisplayName( "CurrentEmployeeOnly" ) , Description( "Which type to use..." )]
        public Boolean CurrentEmployeeOnly { get; set; }

        [Category( "Relation" )]
        [DisplayName( "CurrentCompanyUnitOnly" ) , Description( "Which type to use..." )]
        public Boolean CurrentCompanyUnitOnly { get; set; }

        [Category( "Relation" )]
        [ReadOnly( true )]
        public String ParentName { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ParentField" ) , Description( "Which type to use..." )]
        public String ParentField { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ChildField" ) , Description( "Which type to use..." )]
        public String ChildField { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ParentField1" ) , Description( "Which type to use..." )]
        public String ParentField1 { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ChildField1" ) , Description( "Which type to use..." )]
        public String ChildField1 { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ParentField2" ) , Description( "Which type to use..." )]
        public String ParentField2 { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ChildField2" ) , Description( "Which type to use..." )]
        public String ChildField2 { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ParentField3" ) , Description( "Which type to use..." )]
        public String ParentField3 { get; set; }

        [Category( "Relation" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "ChildField3" ) , Description( "Which type to use..." )]
        public String ChildField3 { get; set; }

        [Category( "Filter" )]
        [Editor( typeof(ABCControls.TypeEditor.FilterConditionEditor ) , typeof( UITypeEditor ) )]
        [DisplayName( "FilterCondition" ) , Description( "Which type to use..." )]
        public String FilterCondition { get; set; }

        [Category( "Filter" )]
        [DisplayName( "SQLFilterCondition" ) , Description( "Which type to use..." )]
        public String SQLFilterCondition { get; set; }

        #endregion

        public Dictionary<string , ABCBindingConfig> Children=new Dictionary<string , ABCBindingConfig>();
        
        public ABCBindingConfig ( )
        {
            IsList=false;
            IsMainObject=false;
            DisplayOnly=false;
            AutoSave=false;
            DefaultField=String.Empty;
            DefaultValue=String.Empty;
            TopCount=0;
        }
        public ABCBindingConfig (String strTableName )
        {
            IsList=false;
            IsMainObject=false;
            DisplayOnly=false;
            AutoSave=false;
            DefaultField=String.Empty;
            DefaultValue=String.Empty;
            TopCount=0;
            TableName=strTableName;
            Name="obj"+strTableName;

            if ( DataStructureProvider.DataTablesList.ContainsKey( strTableName ) )
            {
                foreach ( String strFKcol in DataStructureProvider.DataTablesList[strTableName].ForeignColumnsList.Keys )
                {
                    FieldFilterConfig config=new FieldFilterConfig();
                    config.TableName=DataStructureProvider.GetTableNameOfForeignKey(strTableName,strFKcol);
                    config.Field=strFKcol;
                    FieldFilterConditions.Add(config );
                }
            }
        }

        public object Clone ( )
        {
            ABCBindingConfig obj=(ABCBindingConfig)this.MemberwiseClone();
            obj.Children=new Dictionary<string,ABCBindingConfig>();
            return obj;
        }
    }

    public class ABCBindingConfigCollection 
    {
        public Dictionary<string , ABCBindingConfig> InnerList=new Dictionary<string , ABCBindingConfig>();

        public ABCBindingConfig this[String key]
        {
            get
            {
                return (ABCBindingConfig)GetValueFromKey( key );
            }
            set
            {
                SetValueFromKey( key , value );
            }
        }
        public List<String> Keys=new List<string>();
        public List<ABCBindingConfig> Values=new List<ABCBindingConfig>();

        public ICollection TreeKeys
        {
            get
            {
                return InnerList.Keys;
            }
        }
        public ICollection TreeValues
        {
            get
            {
                return InnerList.Values;
            }
        }

        public void Clear ( )
        {
            InnerList.Clear();
            Keys.Clear();
            Values.Clear();
        }
        public void Add ( String key , ABCBindingConfig config )
        {
            InnerList.Add( key , config );
            Invalidate();
        }

        public void Remove ( String key )
        {
            ABCBindingConfig config=GetValueFromKey( key );
            config.Parent.Children.Remove( key );
            Invalidate();
        }

        public bool ContainsKey ( String key )
        {
            return Keys.Contains( key );
        }
        public bool ContainsValue ( ABCBindingConfig config )
        {
            return Values.Contains( config );
        }

        public void Invalidate ( )
        {
            InvalidateKeyList();
            InvalidateValueList();
        }

        #region private
        private void InvalidateKeyList ( )
        {
            Keys.Clear();

            foreach ( ABCBindingConfig info in InnerList.Values )
                GetKeysFromNode( info );
        }
        private void GetKeysFromNode ( ABCBindingConfig current )
        {
            Keys.Add( current.Name );
            foreach ( ABCBindingConfig child in current.Children.Values )
                GetKeysFromNode( child );
        }

        private void InvalidateValueList ( )
        {
            Values.Clear();

            foreach ( ABCBindingConfig info in InnerList.Values )
                GetValuesFromNode( info );
        }
        private void GetValuesFromNode ( ABCBindingConfig current )
        {
            Values.Add( current );
            foreach ( ABCBindingConfig child in current.Children.Values )
                GetValuesFromNode( child );
        }

        private ABCBindingConfig GetValueFromKey ( String strKey )
        {
            foreach ( ABCBindingConfig config in this.Values )
            {
                if ( config.Name==strKey )
                    return config;
            }

            return null;
        }
        private void SetValueFromKey ( string strKey , ABCBindingConfig value )
        {
            for ( int i=0; i<this.Values.Count; i++ )
            {
                if ( this.Values[i].Name==strKey )
                {
                    this.Values[i]=value;
                    break;
                }
            }

            Invalidate();
        } 
        #endregion
    }



}
