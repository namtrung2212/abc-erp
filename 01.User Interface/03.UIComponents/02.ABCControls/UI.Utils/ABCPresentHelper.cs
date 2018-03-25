using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;


using DevExpress;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraNavBar;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Menu;
using DevExpress.Utils.Menu;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using ABCBusinessEntities;

using ABCProvider;
using ABCCommon;

namespace ABCControls
{
    public class ABCPresentHelper
    {

        #region DeSerialization

        public static Component LoadComponent ( ABCView ViewOwner , XmlNode node )
        {
            String strName=node.Attributes["name"].Value.ToString();
            String strType=node.Attributes["type"].Value.ToString();
            Type type=TypeResolutionService.CurrentService.GetType( strType );
            if ( type==null )
                return null;

            #region Node
            Component comp=null;

            #region ABCView
            if ( type==typeof( ABCView ) )
            {
                if ( node.Attributes["isRoot"]!=null&&node.Attributes["isRoot"].Value.ToString()=="true" )
                {
                    #region root
                    DeSerialization( ViewOwner , node );

                    ViewOwner.IsRoot=true;
                    ViewOwner.Name=node.Attributes["name"].Value.ToString();
                    if ( String.IsNullOrWhiteSpace( ViewOwner.Caption ) )
                        ViewOwner.Caption=ViewOwner.Name;

                    comp=ViewOwner;

                    LoadChildrenComponent( ViewOwner , node , comp );

                    #endregion
                }
                else
                {

                    #region Included ABCView
                    if ( ViewOwner.Mode==ViewMode.Design )
                        comp=(Component)ViewOwner.Surface.DesignerHost.CreateComponent( type );
                    else
                        comp=(Component)ABCDynamicInvoker.CreateInstanceObject( type );
              
                    DeSerialization( comp , node );

                    ( (ABCView)comp ).IsRoot=false;

                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( node.Attributes["ID"].Value );
                    STViewsInfo viewInfo=(STViewsInfo)new STViewsController().GetObjectByID( iID );
                    if ( viewInfo!=null )
                    {
                        ( (ABCView)comp ).DataField=viewInfo;
                        ViewOwner.ChildrenView.Add( (ABCView)comp );
                    }

                    #endregion
                }

                ( (IABCControl)comp ).OwnerView=ViewOwner;
                ( (IABCControl)comp ).InitControl();

                return comp;
            }
            #endregion

            #region ABCDockPanel
            if ( type==typeof( DevExpress.XtraBars.Docking.DockPanel )||type==typeof( ABCControls.ABCDockPanel ) )
            {
                comp=(Component)ABCDockPanel.AddNewDockPanel( ViewOwner );
                DeSerialization( comp , node );
                ( (ABCDockPanel)comp ).InitLayout( ViewOwner , node );
                return comp;
            }
            #endregion

            comp=CreateComponent( ViewOwner , type );
            if ( comp is Control )
                ( comp as Control ).SuspendLayout();

            if ( comp is IABCControl )
            {
                ( (IABCControl)comp ).OwnerView=ViewOwner;
                ( (IABCControl)comp ).InitControl();
            }

            DeSerialization( comp , node );

            if ( comp is IABCCustomControl )
                ( (IABCCustomControl)comp ).InitLayout( ViewOwner , node );
            else
                LoadChildrenComponent( ViewOwner , node , comp );

            #endregion

            if ( comp is Control )
                ( comp as Control ).ResumeLayout( false );
        
            if ( comp is ABCSimpleButton )
                ViewOwner.ButtonList.Add( comp as ABCSimpleButton );
            if ( comp is ABCComment )
                ViewOwner.CommentList.Add( comp as ABCComment );

            return comp;
        }
        public static void LoadChildrenComponent ( ABCView ViewOwner , XmlNode nodeParent , Component parent )
        {
            if ( parent is ABCView )
                ( (Control)parent ).Controls.Clear();

            foreach ( XmlNode nodeChild in nodeParent.ChildNodes )
            {
                if ( nodeChild.Name!="C" )
                    continue;

                Component compChild=LoadComponent( ViewOwner , nodeChild );
                if ( compChild is Control&&parent is Control )
                    ( (Control)compChild ).Parent=(Control)parent;

            }
        }
        public static Component CreateComponent ( ABCView ViewOwner , Type type )
        {
            Component comp=null;
            try
            {
                if ( ViewOwner.Mode==ViewMode.Design )
                    comp=(Component)ViewOwner.Surface.DesignerHost.CreateComponent( type );
                else
                    comp=(Component)ABCDynamicInvoker.CreateInstanceObject( type );
            }
            catch ( Exception ex )
            {
                return null;
            }

            return comp;
        }
        
        public static void DeSerialization ( object comp , XmlNode node )
        {
            if ( node.Attributes["name"]!=null )
            {
                comp.GetType().GetProperty( "Name" ).SetValue( comp , node.Attributes["name"].Value.ToString() , null );
                if ( comp is Control&&( comp as Control ).Site!=null )
                    ( comp as Control ).Site.Name=node.Attributes["name"].Value.ToString();
            }
            foreach ( XmlNode nodeChild in node.ChildNodes )
            {
                if ( nodeChild.Name=="P" )
                {
                    String strProName=nodeChild.Attributes["name"].Value.ToString();
                    String strValue=nodeChild.InnerText;
                    if ( String.IsNullOrWhiteSpace( strValue ) )
                        continue;

                    PropertyInfo proInfo=null;
                    try
                    {
                        proInfo=comp.GetType().GetProperty( strProName );
                    }
                    catch ( Exception ee )
                    {
                    }
                    if ( proInfo==null )
                        continue;

                    try
                    {
                        if ( proInfo.PropertyType.FullName=="System.Object" )
                            proInfo.SetValue( comp , strValue , null );
                        else
                        {
                            TypeConverter converter=TypeDescriptor.GetConverter( proInfo.PropertyType );
                            object obj=converter.ConvertFromString( strValue );
                            proInfo.SetValue( comp , obj , null );
                        }
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            }

            if ( comp is DevExpress.XtraEditors.BaseEdit )
            {
                // ( comp as DevExpress.XtraEditors.BaseEdit ).ForeColor=Color.Black;
            }
        }
        public static object DeSerialization ( object obj , Type type , XmlNode node )
        {
            if ( node==null )
                return obj;

            if ( obj==null )
                obj=ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( type );

            if ( obj!=null&&node!=null )
                DeSerialization( obj , node );

            return obj;
        }

      
        #endregion

        #region TemplateList

        static Dictionary<String , object> TemplateList=new Dictionary<String , object>();
        public static object GetTemplateObject ( Type type )
        {
            object obj=null;
            if ( TemplateList.TryGetValue( type.FullName , out obj ) )
                return obj;

            if ( typeof( Component ).IsAssignableFrom( type ) )
            {
                obj=HostSurfaceManager.GetTemplateComponent( type );

            }
            else
            {
                obj=GetTemplateObjectFromSpecialType( type );
                if ( obj==null )
                    obj=ABCDynamicInvoker.CreateInstanceObject( type );
            }
            if ( obj!=null )
                TemplateList.Add( type.FullName , obj );
            return obj;
        }

        public static object GetTemplateObjectFromSpecialType ( Type type )
        {
            #region Chart
            if ( typeof( DevExpress.XtraCharts.AxisY ).IsAssignableFrom( type ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return ( objParent as ABCChartBaseControl ).AxisY;
            }
            if ( typeof( DevExpress.XtraCharts.AxisX ).IsAssignableFrom( type ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return ( objParent as ABCChartBaseControl ).AxisX;
            }
            if ( typeof( DevExpress.XtraCharts.Series ).IsAssignableFrom( type ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartOneSeriesControl ) );
                if ( objParent!=null )
                    return ( objParent as ABCChartOneSeriesControl ).Series1;
            }

            if ( type==typeof( DevExpress.XtraCharts.ScaleBreakOptions ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).ScaleBreakOptions;
            }
            if ( type==typeof( DevExpress.XtraCharts.AutoScaleBreaks ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).AutoScaleBreaks;
            }
            if ( typeof( DevExpress.XtraCharts.RectangleFillStyle ).IsAssignableFrom( type ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).InterlacedFillStyle;
            }
            if ( type==typeof( DevExpress.XtraCharts.Tickmarks ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).Tickmarks;
            }
            if ( type==typeof( DevExpress.XtraCharts.AxisTitle ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).Title;
            }
            if ( type==typeof( DevExpress.XtraCharts.AxisLabel ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).Label;
            }
            if ( type==typeof( DevExpress.XtraCharts.GridLines ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).GridLines;
            }
            if ( type==typeof( DevExpress.XtraCharts.NumericOptions ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).NumericOptions;
            }
            if ( type==typeof( DevExpress.XtraCharts.DateTimeOptions ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).DateTimeOptions;
            }
            if ( type==typeof( DevExpress.XtraCharts.WorkdaysOptions ) )
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return  ( ( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).WorkdaysOptions;
            }
            if ( typeof( DevExpress.XtraCharts.AxisRange ).IsAssignableFrom( type ) ) 
            {
                object objParent=GetTemplateObject( typeof( ABCChartBaseControl ) );
                if ( objParent!=null )
                    return (( objParent as ABCChartBaseControl ).AxisX as DevExpress.XtraCharts.Axis ).Range;
            }
            #endregion

            return null;
        }
        #endregion

        #region Property

        public static Dictionary<String , List<String>> UsablePropertiesList=new Dictionary<string , List<string>>();
        public static List<String> GetUsablePropertiesList ( Type type )
        {
            if ( UsablePropertiesList.ContainsKey( type.FullName ) )
                return UsablePropertiesList[type.FullName];

            List<String> lstResult=new List<string>();

            if ( typeof( IABCControl ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] { "Name" , "IsVisible" } );

            if ( typeof( Control ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] { "BorderStyle","AutoScroll","Name" , "Locked" , "Comment" , "Caption" , "BackColor" , "BorderStyle" , "ForeColor" , "Font" , "Location" , "Size" , "Enabled" , "TabIndex" , "Tag" , "Anchor" , "Dock" , "RightToLeft" , "ReadOnly" , "Text" , "FieldGroup" , "DummyText" , "EditMask" , "MaskType" , "CharacterCasing" } );

            if ( typeof( IABCBindableControl ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] { "DataSource" , "DataMember" , "TableName" , "DisplayMember" , "ValueMember" , "FilterStringEx" , "ReadOnly" } );

            //if ( typeof( DevExpress.XtraEditors.BaseEdit ).IsAssignableFrom( type )||typeof( DevExpress.XtraEditors.LabelControl )==type )
            //    lstResult.AddRange( new String[] { "EditMask" , "MaskType" , "CharacterCasing" } );

            if ( type==typeof( ABCControls.ABCView ) )
                lstResult.AddRange( new String[] {"DisplayInTree","ScreenName", "DataConfig" , "SearchForm" , "ViewID" , "IsJoinBindingWithParent" , "IsUseSourceCode" ,"MainTableName","MainFieldName","MainValue"
                    , "ShowToolbar"  , "ShowNewItem" , "ShowEditItem" , "ShowDeleteItem" , "ShowRefreshItem", "ShowPostItem", "ShowApproveItem", "ShowLockItem", "ShowSearchItem", "ShowPrintItem"
                    , "ShowCustomItem", "ShowInfoItem" , "WindowState" , "StartPosition" , "FormBorderStyle" , "ControlBox" , "MinimizelBox" , "MaximizeBox" } );

            if ( typeof( IABCGridControl ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] {"AllowCellMerge","Script", "GridDataSource", "FocusRectStyle","ShowViewCaption", "ViewCaption","AllowAddRows","NewItemRowPosition","AllowDeleteRows","Editable",
                        "ReadOnly","ShowFind", "MultiSelect","MultiSelectMode","DrawColorEvenRow","DrawColorOddRow","ShowAutoFilterRow","ShowColumnHeaders","ShowFilterPanelMode",
                        "ShowFooter","ShowGroupPanel","ShowGroupExpandCollapseButtons","ShowHorzLines","ShowVertLines","ShowIndicator","ShowPreview","PreviewFieldName","ColumnConfigs"
                        ,"ShowMenuBar","ShowSaveButton","ShowDeleteButton","ShowRefreshButton","ShowFilterButton","ShowExportButton","ShowPrintButton" ,"ShowColumnChooser",
                "DrawBlueEmptyArea","DrawNotFocusSelection","EnableFocusedCell","EnableFocusedRow","Header","Footer"} );

            if ( type==typeof( ABCControls.ABCPivotGridControl ) )
                lstResult.AddRange( new String[] {"ViewCaption","Script", "UseChartControl","ChartType","DataSource" , "FieldConfigs" , "DrawFocusedCellRect" , "GroupFieldsInCustomizationWindow" , "ShowColumnGrandTotalHeader" , "ShowColumnGrandTotals"
                    , "ShowColumnHeaders" , "ShowColumnTotals" , "ShowCustomTotalsForSingleValues" , "ShowDataHeaders","ShowFilterHeaders"  ,"ShowFilterSeparatorBar"  ,"ShowGrandTotalsForSingleValues" 
                    ,"ShowHorzLines"  ,"ShowVertLines"  ,"ShowRowGrandTotalHeader"  ,"ShowRowGrandTotals" ,"ShowRowHeaders", "FieldConfigs", "ShowRowTotals", "ShowTotalsForSingleValues","RowTreeWidth","PageMargins","Landscape","PaperKind","ChartSizeMode","ChartHorizontal"} );

            if ( type==typeof( ABCControls.ABCTreeList ) )
                lstResult.AddRange( new String[] { "ViewCaption" , "Script" , "UseChartControl" , "DataSource" , "ColumnConfigs" , "ShowMenuBar" , "ShowSaveButton" , "ShowDeleteButton" , "ShowRefreshButton" , "ShowFilterButton" , "ShowExportButton" , "ShowPrintButton" , "ShowColumnChooser",
                "MultiSelect","DrawColorEvenRow","DrawColorOddRow","ShowAutoFilterRow","ShowColumnHeaders","ShowFilterPanelMode",
                        "ShowSummaryFooter","ShowRowFooterSummary","ShowHorzLines","ShowVertLines","ShowIndicator","ShowPreview","PreviewFieldName"} );
            #region Chart

            if ( typeof( ABCControls.ABCChartBaseControl ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] { "ShowMenuBar","AxisX" , "AxisY" , "Series1" , "Series2" , "Series3" , "Series4" , "Series5" , "Series6" , "Series7" , "Series8" , "EmptyChartText" , "Caption" , "AppearanceName" , "PaletteName" , "PaletteBaseColorNumber" , "ShowLegend" } );

            //if ( typeof( ABCControls.ABCChartBaseSeries ).IsAssignableFrom( type ) )
            //    lstResult.AddRange( new String[] { "DataSourceName","ArgumentMember" , "ArgumentScale" , "ValueMembers" , "ValueScale" , "SeriesViewType" , "ColorEach" , "ValueAsPercent" 
            //    ,"PercentageAccuracy","Caption","LegenPattern" , "ShowSeriesLabel" , "SeriesLabelPointView" , "SeriesLabelPattern" , "SeriesLabelAntialiasing" , "ArgumentDateTimeOptions" , "ArgumentNumericOptions", "ValueDateTimeOptions" , "ValueNumericOptions" , "ValueFormat"} );

            if ( typeof( ABCControls.ABCChartPieControl ).IsAssignableFrom( type ) )
                lstResult.AddRange( new String[] { "Position" } );

            #endregion

            if ( type==typeof( ABCControls.ABCBindingBaseEdit )||type==typeof( ABCControls.ABCSearchControl )||type==typeof( ABCControls.ABCSearchPanel )||type==typeof( ABCControls.ABCAutoSearchPanel )||type==typeof( ABCControls.ABCDataPanel ) )
            {
                lstResult.Clear();
                lstResult.AddRange( new String[] { "ControlType" , "Name" , "IsVisible" , "Location" , "Size" , "Enabled" , "Tag" , "Anchor" , "Dock" , "TextVisible" , "TextWidth" , "DisplayImage" , "Editor" , "TextLabel" , "DataSource" , "DataMember" , "TableName" , "ReadOnly" , "ShowSearchButton" , "SearchButton" , "LinkScreen" , "ParamFields" } );
            }

            if ( type==typeof( ABCControls.ABCCheckEdit ) )
                lstResult.AddRange( new String[] { "GridControl" } );
            if ( type==typeof( ABCControls.ABCDateEdit ) )
                lstResult.AddRange( new String[] { "FormatType" } );

            if ( type==typeof( DevExpress.XtraTab.XtraTabPage ) )
            {
                lstResult.Clear();
                lstResult.AddRange( new String[] { "Name" , "Text" , "PageEnabled" , "PageVisible" } );
            }
            if ( type==typeof( ABCControls.ABCTabControl ) )
                lstResult.AddRange( new String[] { "TabPages" } );
            if ( type==typeof( ABCSplitterContainer ) )
                lstResult.AddRange( new String[] { "SplitterPosition" , "Horizontal" , "FixedPanel" } );
            if ( type==typeof( ABCControls.ABCTabControl ) )
                lstResult.AddRange( new String[] { "SelectedTabPageIndex","ShowTabHeader" } );
            if ( type==typeof( System.Windows.Forms.WebBrowser ) )
                lstResult.AddRange( new String[] { "Url" } );

            if ( type==typeof( ABCControls.ABCDockPanel )||type==typeof( DevExpress.XtraBars.Docking.DockPanel ) )
            {
                lstResult.Clear();
                lstResult.AddRange( new String[] { "Name" , "Size" , "Dock" , "Text" , "Location" , "FloatVertical" , "Tabbed" , "OriginalSize" , "ID" } );
            }
            if ( type==typeof( ABCControls.ABCDockManager ) )
            {
                lstResult.Clear();
                lstResult.AddRange( new String[] { "DockMode" } );
            }

            if ( type==typeof( System.Windows.Forms.FlowLayoutPanel ) )
                lstResult.AddRange( new String[] { "FlowDirection" } );

            if ( type==typeof( ABCControls.ABCSimpleButton ) )
                lstResult.AddRange( new String[] { "IconType" , "ButtonType" } );

            if ( type==typeof( ABCControls.ABCGridLookUpEdit ) )
                lstResult.AddRange( new String[] { "ParentCtrlName" , "ChildField" } );

            if ( type==typeof( ABCControls.ABCLookUpEdit ) )
                lstResult.AddRange( new String[] { "ParentCtrlName" , "ChildField" } );

            if ( type==typeof( ABCControls.ABCNumbSearch ) )
                lstResult.AddRange( new String[] { "ExtractlySearch" } );    
  
            if ( type==typeof( ABCControls.ABCCollapseGroupControl ) )
                lstResult.AddRange( new String[] { "IsDefaultCollapse" } );

               if ( type==typeof( ABCControls.ABCRichEditControl ) )
                   lstResult.AddRange( new String[] { "ReadOnly" } );

               if ( type==typeof( ABCControls.ABCSimpleButton ) )
                   lstResult.AddRange( new String[] { "DataSource" } );
            
            if ( type==typeof( ABCControls.ABCCollapseGroupControl )||type==typeof( ABCControls.ABCGroupControl )||type==typeof( ABCControls.ABCPanelControl ) )
                lstResult.AddRange( new String[] { "IsSizeable" } );

            if ( type==typeof( ABCControls.ABCFlowPanelControl )||type==typeof( System.Windows.Forms.FlowLayoutPanel ) )
                lstResult.AddRange( new String[] { "WrapContents" } );
            
            //if ( lstResult.Count<=0 )
            //{
                foreach ( PropertyInfo proInfo in type.GetProperties() )
                {
                    #region check browsable
                    bool isBrowsable=true;
                    foreach ( object att in proInfo.GetCustomAttributes( true ) )
                    {
                        if ( att is BrowsableAttribute )
                        {
                            isBrowsable=( (BrowsableAttribute)att ).Browsable;
                            break;
                        }
                    }
                    #endregion

                    if ( isBrowsable&&lstResult.Contains(proInfo.Name)==false)
                        lstResult.Add( proInfo.Name );
                }
     //       }

            if ( type==typeof( DevExpress.XtraEditors.SplitGroupPanel ) )
                lstResult.Clear();


            UsablePropertiesList.Add( type.FullName , lstResult );
            return UsablePropertiesList[type.FullName];

        }

        public static bool IsValidProperty ( PropertyInfo proInfo )
        {

            if ( ABCPresentHelper.GetUsablePropertiesList( proInfo.ReflectedType ).Contains( proInfo.Name ) )
                return true;

            return false;
        }
        public static List<PropertyInfo> GetModifiedPropertiesList ( object obj )
        {
            List<PropertyInfo> lstProperty=new List<PropertyInfo>();

            Type type=obj.GetType();
            object objTemplate=ABCPresentHelper.GetTemplateObject( type );

            foreach ( PropertyInfo proInfo in type.GetProperties() )
            {
                if ( ABCPresentHelper.IsValidProperty( proInfo )==false )
                    continue;
                try
                {
                    #region check temlate component
                    if ( objTemplate!=null )
                    {
                        object obj1=proInfo.GetValue( obj , null );
                        object obj2=proInfo.GetValue( objTemplate , null );
                        if ( obj1==obj2||( obj1==null&&obj2==null )||( obj1!=null&&obj2!=null&&obj1.ToString()==obj2.ToString() ) )
                            continue;
                    }
                    #endregion

                    lstProperty.Add( proInfo );
                }
                catch
                {
                }
            }

            return lstProperty;

        }
        #endregion

        #region Serialization

        public static XmlElement Serialization ( object obj , XmlElement eleParent , String strTagName )
        {
            if ( eleParent==null )
                return null;

            if ( obj is CollectionBase )
            {
                CollectionBase collection=obj as CollectionBase;
                if ( collection.Count>0 )
                {
                    XmlElement eleCollection=eleParent.OwnerDocument.CreateElement( strTagName );

                    foreach ( object child in collection )
                        Serialization( child , eleCollection , "Item" );

                    eleParent.AppendChild( eleCollection );
                    return eleCollection;
                }
            }
            else
            {
                XmlElement eleChild=Serialization( eleParent.OwnerDocument , obj , strTagName );
                eleParent.AppendChild( eleChild );
                return eleChild;
            }
            return null;
        }

        public static XmlElement Serialization ( XmlDocument doc , object obj , String strTag )
        {
            if ( obj==null )
                return null;

            Type type=obj.GetType();

            XmlElement ele=doc.CreateElement( strTag );
            if ( obj is IComponent )
            {
                IComponent objTemp=(IComponent)obj;
                if ( objTemp.Site!=null )
                    ele.SetAttribute( "name" , objTemp.Site.Name );
                else
                    ele.SetAttribute( "name" , ( (Control)objTemp ).Name );
            }
            ele.SetAttribute( "type" , type.Name );


            List<PropertyInfo> lstProperties=ABCPresentHelper.GetModifiedPropertiesList( obj );
            foreach ( PropertyInfo proInfo in lstProperties )
            {
                if ( obj is IComponent&&proInfo.Name=="Name" )
                    continue;

                try
                {

                    object objValue=proInfo.GetValue( obj , null );
                    if ( objValue==null )
                        continue;

                    XmlElement eleP=doc.CreateElement( "P" );
                    eleP.SetAttribute( "name" , proInfo.Name );

                    TypeConverter converter=TypeDescriptor.GetConverter( proInfo.PropertyType );
                    if ( converter!=null )
                        eleP.InnerText=converter.ConvertToString( objValue );
                    else
                        eleP.InnerText=objValue.ToString();
                    //if ( eleP.InnerText.StartsWith( "(" )||eleP.InnerText.EndsWith( ")" ) )
                    //    continue;

                    ele.AppendChild( eleP );
                }
                catch
                {
                }

            }
            return ele;
        }

        #endregion


        public static DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit GetRepositoryFromEnum ( Type type )
        {
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpType=new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            rpType.DataSource=Enum.GetValues( type );

            return rpType;
        }

        public static ABCView FindParentView ( Control ctrl )
        {
            Control currentCtrl=ctrl;
            do
            {
                currentCtrl=currentCtrl.Parent;
            } while ( currentCtrl is ABCView==false&&currentCtrl!=null );

            if ( currentCtrl==null )
                return null;

            return (ABCView)currentCtrl;
        }

        public static String GetLabelCaption ( ABCView OwnerView , String strDataSource , String strDataMember )
        {
            if ( OwnerView==null||String.IsNullOrWhiteSpace( strDataMember )||String.IsNullOrWhiteSpace( strDataSource ) )
                return String.Empty;

            if ( OwnerView.DataConfig.BindingList.ContainsKey( strDataSource )==false )
                return String.Empty;

            String strBusTableName=OwnerView.DataConfig.BindingList[strDataSource].TableName;

            return GetLabelCaption( strBusTableName , strDataMember );
        }
        public static String GetLabelCaption ( String strTableName , String strDataMember )
        {
            if ( string.IsNullOrWhiteSpace( strDataMember )||String.IsNullOrWhiteSpace( strTableName ) )
                return String.Empty;

            if ( strDataMember.Contains( ":" )==false )
                return DataConfigProvider.GetFieldCaption( strTableName , strDataMember );

            String strCurrentTableName=strTableName;
            String[] strArray=strDataMember.Split( ':' );
            foreach ( String strCurrentField in strArray )
            {
                if ( DataStructureProvider.IsForeignKey( strCurrentTableName , strCurrentField ) )
                    strCurrentTableName=DataStructureProvider.GetTableNameOfForeignKey( strCurrentTableName , strCurrentField );
                else
                    return DataConfigProvider.GetFieldCaption( strCurrentTableName , strCurrentField );
            }

            return String.Empty;
        }
        public static List<Type> GetControlTypes ( String strTableName , String strFieldName )
        {
            List<Type> lstTypes=new List<Type>();

            String[] strArr=strFieldName.Split( ':' );
            if ( strArr.Length<=0 )
                return lstTypes;


            if ( strArr.Length==1 )
            {
                #region Level 1

                String strFieldNameTemp=strArr[0];
                String strTableNameTemp=strTableName;
                if ( DataStructureProvider.IsForeignKey( strTableNameTemp , strFieldNameTemp ) )
                {
                    #region Foreign Key
                   
                    lstTypes.Add( typeof( ABCLookUpEdit ) );
                    lstTypes.Add( typeof( ABCTextEdit ) );
                    lstTypes.Add( typeof( ABCGridLookUpEdit ) );
                    lstTypes.Add( typeof( ABCRadioGroup ) );
                    lstTypes.Add( typeof( ABCCheckedListBox ) );
                    if(DataStructureProvider.GetTableNameOfForeignKey(strTableNameTemp,strFieldNameTemp)=="GEPeriods")
                        lstTypes.Add( typeof( ABCPeriodEdit ) );
                    #endregion

                }
                else
                {
                    String strEnumName=DataConfigProvider.TableConfigList[strTableNameTemp].FieldConfigList[strFieldNameTemp].AssignedEnum;
                    if ( String.IsNullOrWhiteSpace( strEnumName )==false&&EnumProvider.EnumList.ContainsKey( strEnumName ) )
                    {
                        #region Enum

                        lstTypes.Add( typeof( ABCLookUpEdit ) );
                        lstTypes.Add( typeof( ABCRadioGroup ) );
                        lstTypes.Add( typeof( ABCCheckedListBox ) );
                        #endregion

                    }
                    else
                    {
                        #region Common
                        String strType=DataStructureProvider.GetCodingType( strTableNameTemp , strFieldNameTemp );
                        if ( strType=="DateTime"||strType=="Nullable<DateTime>" )
                        {
                            lstTypes.Add( typeof( ABCDateEdit ) );
                            lstTypes.Add( typeof( ABCTimeEdit ) );
                            lstTypes.Add( typeof( ABCMonthYearEdit ) );
                        }
                        if ( strType=="int"||strType=="Nullable<int>" )
                        {
                            lstTypes.Add( typeof( ABCTextEdit ) );
                            lstTypes.Add( typeof( ABCSpinEdit ) );
                            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( strTableNameTemp , strFieldNameTemp );
                            if ( formatInfo!=null )
                            {
                                if(formatInfo.ABCFormat==DataFormatProvider.FieldFormat.Year)
                                    lstTypes.Add( typeof( ABCYearEdit ) );
                                if ( formatInfo.ABCFormat==DataFormatProvider.FieldFormat.Month ||
                                     formatInfo.ABCFormat==DataFormatProvider.FieldFormat.MonthAndYear)
                                    lstTypes.Add( typeof( ABCMonthEdit ) );
                            }
                        }
                        if ( strType=="String"||strType=="Nullable<String>" )
                        {
                            lstTypes.Add( typeof( ABCTextEdit ) );
                            lstTypes.Add( typeof( ABCLabel ) );
                            lstTypes.Add( typeof( ABCMemoEdit ) );
                            lstTypes.Add( typeof( ABCMemoExEdit ) );
                            lstTypes.Add( typeof( ABCRichEditControl ) );
                        }
                        if ( strType=="double"||strType=="decimal"||strType=="Nullable<double>"||strType=="Nullable<decimal>" )
                        {
                            lstTypes.Add( typeof( ABCTextEdit ) );
                            lstTypes.Add( typeof( ABCSpinEdit ) );
                            lstTypes.Add( typeof( ABCCalcEdit ) );
                        }
                        if ( strType=="bool"||strType=="Nullable<bool>" )
                        {
                            lstTypes.Add( typeof( ABCCheckEdit ) );
                            lstTypes.Add( typeof( ABCCheckPanel ) );
                            lstTypes.Add( typeof( ABCRadioGroup ) );
                            //     lstTypes.Add( typeof( ABCCheckedListBox ) );
                        }
                        #endregion

                    }
                }
                lstTypes.Add( typeof( ABCSearchControl ) );

                #endregion

            }
            else
            {
                #region Level n
                String strFieldNameTemp=String.Empty;
                String strTableNameTemp=strTableName;
                for ( int i=0; i<strArr.Length; i++ )
                {
                    strFieldNameTemp=strArr[i];
                    if ( DataStructureProvider.IsForeignKey( strTableNameTemp , strFieldNameTemp )==false )
                        break;

                    strTableNameTemp=DataStructureProvider.GetTableNameOfForeignKey( strTableNameTemp , strFieldNameTemp );
                }
                String strType=DataStructureProvider.GetCodingType( strTableNameTemp , strFieldNameTemp );
                if ( strType=="DateTime"||strType=="Nullable<DateTime>" )
                {
                    lstTypes.Add( typeof( ABCDateEdit ) );
                    lstTypes.Add( typeof( ABCTimeEdit ) );
                }
                if ( strType=="int"||strType=="double"||strType=="decimal"||strType=="Nullable<int>"||strType=="Nullable<double>"||strType=="Nullable<decimal>" )
                {
                    lstTypes.Add( typeof( ABCTextEdit ) );
                    lstTypes.Add( typeof( ABCLabel ) );
                }
                if ( strType=="String"||strType=="Nullable<String>" )
                {
                    lstTypes.Add( typeof( ABCTextEdit ) );
                    lstTypes.Add( typeof( ABCLabel ) );
                    lstTypes.Add( typeof( ABCMemoEdit ) );
                    lstTypes.Add( typeof( ABCRichEditControl ) );
                }
                if ( strType=="bool"||strType=="Nullable<bool>" )
                {
                    lstTypes.Add( typeof( ABCCheckEdit ) );
                    lstTypes.Add( typeof( ABCCheckPanel ) );
                }
                #endregion
            }

            return lstTypes;
        }

        public static Color GetSkinBackColor ( )
        {
            Skin currentSkin=CommonSkins.GetSkin( UserLookAndFeel.Default );
            if ( currentSkin[CommonSkins.SkinForm].Color.BackColor!=SystemColors.Control )
                return currentSkin[CommonSkins.SkinForm].Color.BackColor;
            else
                return currentSkin.TranslateColor( SystemColors.Control );
        }
        public static Color GetSkinForeColor ( )
        {
            Skin currentSkin=CommonSkins.GetSkin( UserLookAndFeel.Default );
            if ( currentSkin[CommonSkins.SkinForm].Color.ForeColor!=SystemColors.ActiveCaptionText )
                return currentSkin[CommonSkins.SkinForm].Color.ForeColor;
            else
                return currentSkin.TranslateColor( SystemColors.ActiveCaptionText );
        }

        static Dictionary<String , Boolean> IsMainObjectCachingList=new Dictionary<string , bool>();
        public static bool IsMainObject ( String strTableName )
        {
            if ( String.IsNullOrWhiteSpace( strTableName ) )
                return false;

            bool isOK=false;
            if ( IsMainObjectCachingList.TryGetValue( strTableName , out isOK ) )
                return isOK;

            if ( String.IsNullOrWhiteSpace( strTableName )==false )
            {
                object objCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM STViews WHERE [MainTableName] = '{0}' " , strTableName ) );
                if ( objCount!=null&&objCount!=DBNull.Value )
                {
                    if ( Convert.ToDouble( objCount )>0 )
                        isOK=true;
                }
            }

            IsMainObjectCachingList.Add( strTableName , isOK );
            return isOK;
        }
    }



    public class ABCCommonTreeListNode : TreeList.IVirtualTreeListData
    {
        public ABCCommonTreeListNode ParentNode;
        public ArrayList ChildrenNodes=new ArrayList();
        public object InnerData;
        public int Level=0;

        public ABCCommonTreeListNode ( ABCCommonTreeListNode parent , object _data )
        {
            this.ParentNode=parent;
            if ( parent!=null )
                this.Level=parent.Level+1;

            this.InnerData=_data;

            if ( this.ParentNode!=null )
                this.ParentNode.ChildrenNodes.Add( this );
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes ( VirtualTreeGetChildNodesInfo info )
        {
            info.Children=ChildrenNodes;
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetCellValue ( VirtualTreeGetCellValueInfo info )
        {
            foreach ( String strField in info.Column.FieldName.Split( ';' ) )
            {
                PropertyInfo proInfo=InnerData.GetType().GetProperty( info.Column.FieldName );
                if ( proInfo!=null )
                {
                    info.CellData=proInfo.GetValue( InnerData , null );
                    return;
                }
            }
        }

        void TreeList.IVirtualTreeListData.VirtualTreeSetCellValue ( VirtualTreeSetCellValueInfo info )
        {
            PropertyInfo proInfo=InnerData.GetType().GetProperty( info.Column.FieldName );
            if ( proInfo!=null )
                proInfo.SetValue( InnerData , info.NewCellData , null );
        }
    }


    public class ABCGridViewMenu : GridViewMenu
    {
        public ABCGridViewMenu ( DevExpress.XtraGrid.Views.Grid.GridView view ) : base( view ) { }

        public DXMenuItem AddItem ( String strCaption , object tag , Image img )
        {
            Items.Add( CreateMenuItem( strCaption , img , tag , true ) );
            return Items[Items.Count-1];
        }
        public void AddItem ( DXMenuItem MenuItem )
        {
            Items.Add( MenuItem );
        }

    }

}