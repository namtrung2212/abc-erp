using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;

using ABCProvider;
using ABCCommon;
using ABCBusinessEntities;

namespace ABCControls
{

    [Designer( typeof( SearchPanelDesigner ) )]
    public partial class ABCSearchPanel : ABCControls.ABCPanelControl, IABCCustomControl
    {

        public ABCView ViewOwner;

        public virtual ScrollableControl DropZone
        {
            get { return this; }
        }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [Editor( typeof( ABCControls.TypeEditor.ComponentEditor<ABCSimpleButton> ) , typeof( UITypeEditor ) )]
        public String SearchButton { get; set; }

        public ABCSearchPanel ( )
        {
            InitializeComponent();
            this.Size=new Size( 472 , 128 );
         }

        public delegate void ABCSearchEventHandler ( object sender );
        public event ABCSearchEventHandler Search;
        public virtual void OnSearch ()
        {
            if ( this.Search!=null )
                this.Search( this);
        }

        public String GetSearchQuery ( )
        {
            ABCHelper.ConditionBuilder strBuilder=new ABCHelper.ConditionBuilder();
            GetSearchQuery( strBuilder,this );
            return strBuilder.ToString();
        }
        public void GetSearchQuery ( ABCHelper.ConditionBuilder strBuilder , Control current )
        {
            if ( current is ISearchControl )
            {
                String strResult=( (ISearchControl)current ).SearchString;
                if ( String.IsNullOrWhiteSpace( strResult )==false )
                    strBuilder.AddCondition( strResult );
            }
            else if ( current is IABCBindableControl)
            {
                System.Reflection.PropertyInfo proInfo=current.GetType().GetProperty( ( current as IABCBindableControl ).BindingProperty );
                if ( proInfo!=null )
                {
                    object objValue=proInfo.GetValue( current , null );
                    if ( objValue!=null )
                    {
                        String strType=DataStructureProvider.GetCodingType( ( current as IABCBindableControl ).TableName , ( current as IABCBindableControl ).DataMember );
                        if ( strType=="int"||strType=="Nullable<int>"||strType=="double"||strType=="Nullable<double>"||strType=="bool"||strType=="Nullable<bool>" )
                            strBuilder.AddCondition( String.Format( " [{0}] = {1} " , ( current as IABCBindableControl ).DataMember , objValue ) );
                        if ( strType=="String"||strType=="Nullable<String>" )
                            strBuilder.AddCondition( String.Format( " [{0}]  LIKE '%{1}%' " , ( current as IABCBindableControl ).DataMember , objValue.ToString() ) );
                        if ( strType=="Guid"||strType=="Nullable<Guid>" )
                            strBuilder.AddCondition( String.Format( " [{0}]  = '{1}' " , ( current as IABCBindableControl ).DataMember , objValue.ToString() ) );
                        if ( strType=="DateTime"||strType=="Nullable<DateTime>" )
                        {

                            DataFormatProvider.FieldFormat format=DataFormatProvider.FieldFormat.None;
                            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( ( current as IABCBindableControl ).TableName , ( current as IABCBindableControl ).DataMember );
                            if ( formatInfo!=null )
                                format=formatInfo.ABCFormat;
                            if ( format==DataFormatProvider.FieldFormat.None )
                                format=DataFormatProvider.FieldFormat.Date;


                            DateTime dtValue1=DateTime.MinValue;
                            DateTime dtValue2=DateTime.MaxValue;
                            if ( objValue is DateTime )
                                dtValue1=Convert.ToDateTime( objValue );
                            if ( objValue is Nullable<DateTime>&&( objValue as Nullable<DateTime> ).HasValue )
                                dtValue1=( objValue as Nullable<DateTime> ).Value;
                        
                           if ( format==DataFormatProvider.FieldFormat.Month|| format==DataFormatProvider.FieldFormat.MonthAndYear )
                            {
                                dtValue1=new DateTime( dtValue1.Year , dtValue1.Month , 1 );
                                dtValue2=dtValue1.AddMonths(1).AddSeconds(-30);
                            }
                            else if ( format==DataFormatProvider.FieldFormat.Year )
                            {
                                dtValue1=new DateTime( dtValue1.Year , 1 , 1 );
                                dtValue2=dtValue1.AddYears( 1 ).AddSeconds( -30 );
                            }
                            else
                            {
                                dtValue1=new DateTime( dtValue1.Year , dtValue1.Month , dtValue1.Day );
                                dtValue2=dtValue1;
                            }

                            String strFrom=TimeProvider.GenCompareDateTime( ( current as IABCBindableControl ).DataMember , ">=" , dtValue1 );
                            String strTo=TimeProvider.GenCompareDateTime( ( current as IABCBindableControl ).DataMember , "<=" , dtValue2 );
                            strBuilder.AddCondition( String.Format( " ( {0} AND {1} ) " , strFrom , strTo ) );
                        }

                    }
                }
            }

     
            foreach ( Control ctrl in current.Controls )
                GetSearchQuery( strBuilder,ctrl );
        }
     
        public void PopulateControls ( String strTableName )
        {
            PopulateControls( null , null , strTableName );
        }
        public void PopulateControls ( ABCView view , String BusName , String strTableName )
        {
            ViewOwner=view;
            DataSource=BusName;
            TableName=strTableName;
            int iCount=0;
            ABCSearchControl ctrl=null;
            foreach ( DataConfigProvider.FieldConfig fieldConfig in DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values )
            {
                if ( fieldConfig.InUse==false )
                    continue;

                if ( DataStructureProvider.IsPrimaryKey( strTableName , fieldConfig.FieldName )
                    ||DataStructureProvider.IsForeignKey( strTableName , fieldConfig.FieldName ) )
                    continue;

                if ( fieldConfig.IsDefault )
                {

                    if ( view!=null&&view.Mode==ViewMode.Design )
                        ctrl=(ABCSearchControl)view.Surface.DesignerHost.CreateComponent( typeof( ABCSearchControl ) );
                    else
                        ctrl=(ABCSearchControl)ABCDynamicInvoker.CreateInstanceObject( typeof( ABCSearchControl ) );

                    ABCSearchInfo searchInfo=new ABCSearchInfo();
                    searchInfo.DataSource=BusName;
                    searchInfo.TableName=TableName;
                    searchInfo.DataMember=fieldConfig.FieldName;

                    ctrl.Initialize( view , searchInfo );
                    ctrl.Parent=this.DropZone;
                    iCount++;
                }
            }

            if ( iCount%2!=0 )
                iCount++;
            iCount=iCount/2;
            if ( ctrl!=null )
                this.Size=new Size( ctrl.Width*2+4*3 , 20*iCount+( iCount+1 )*5+25 );
        }


        #region Layout

        public void GetChildrenXMLLayout ( ABCView ownView , XmlElement AutoSearchPanelEle )
        {
            foreach ( Control ctrl in this.DropZone.Controls )
            {
                XmlElement childEle=ownView.ComponentSerialization( AutoSearchPanelEle.OwnerDocument , ctrl );
                AutoSearchPanelEle.AppendChild( childEle );
            }
        }
        public void InitLayout ( ABCView view , XmlNode node )
        {
            ViewOwner=view;

            this.DropZone.Controls.Clear();

            foreach ( XmlNode nodeCtrl in node.SelectNodes( "C" ) )
            {
                Component comp=ABCPresentHelper.LoadComponent( view , nodeCtrl );
                if ( comp!=null )
                    this.DropZone.Controls.Add( (Control)comp );
            }

            if ( ViewOwner!=null&&ViewOwner.Mode!=ViewMode.Design )
                SetSearchButton( this );
        }

        public void SetSearchButton ( Control ctrl )
        {
            if ( ctrl is ABCSimpleButton&&ctrl.Name==this.SearchButton )
                ( ctrl as ABCSimpleButton ).Click+=new EventHandler( ABCSearchPanel_Click );

            foreach ( Control childCtrl in ctrl.Controls )
                SetSearchButton( childCtrl );
        }

        void ABCSearchPanel_Click ( object sender , EventArgs e )
        {
            OnSearch();
        }


        #endregion

    }

    public class SearchPanelDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public SearchPanelDesigner ( )
        {
        }
    }


}
