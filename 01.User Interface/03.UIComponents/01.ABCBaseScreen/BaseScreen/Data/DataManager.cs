using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using ABCHelper;

using ABCControls;
using ABCProvider;
using ABCProvider;
using ABCScreen.UI;
using ABCBusinessEntities;
using ABCCommon;
namespace ABCScreen.Data
{
    public class DataManager
    {
        public ABCBaseScreen Screen;
        public ABCScreenConfig ScreenConfig;
        public ABCDataObject MainObject;

        public Dictionary<String , ABCDataObject> DataObjectsList=new Dictionary<string , ABCDataObject>();
        public DataManager ( ABCBaseScreen scr )
        {
            Screen=scr;
        }

        #region Utils
        public object this[String strBusName]
        {
            get
            {
                if ( this.DataObjectsList.ContainsKey( strBusName ) )
                    return this.DataObjectsList[strBusName].DataObject;
                else
                    return null;
            }
            set
            {
                this.DataObjectsList[strBusName].DataObject=value;
                this.DataObjectsList[strBusName].RefreshUI();
            }
        }
        public bool Contain ( string strBusName )
        {
            return this.DataObjectsList.ContainsKey( strBusName );
        }

        public void BeginEdit ( String strBusName )
        {
            this.DataObjectsList[strBusName].BeginEdit();
        }
        public void CancelEdit ( String strBusName )
        {
            this.DataObjectsList[strBusName].CancelEdit();
        }
        public void EndEdit ( String strBusName )
        {
            this.DataObjectsList[strBusName].EndEdit();
        }

        #endregion

        public void InitDataConfig ( ABCScreenConfig config )
        {
            if ( config==null )
                return;

            ScreenConfig=config;
            DataObjectsList.Clear();
            foreach ( ABCBindingConfig bindInfo in ScreenConfig.BindingList.Values )
            {
                ABCDataObject obj=new ABCDataObject( bindInfo );
                obj.DataManager=this;
                obj.Binding.ListChanged+=new System.ComponentModel.ListChangedEventHandler( BindingSource_ListChanged );
                DataObjectsList.Add( bindInfo.Name , obj );

                if ( bindInfo.IsMainObject )
                    MainObject=obj;
            }
        }


        #region Invalidate
        public void Invalidate ( )
        {
            foreach ( ABCDataObject obj in DataObjectsList.Values )
            {
                if ( obj.Config.Parent==null )
                {
                    obj.ReloadObject();
                    if ( obj.Config.IsMainObject )
                        this.Screen.ChangeStatus( ABCScreenStatus.LoadedData );
                }
            }
        }
        public void Invalidate ( ABCDataObject mainObj )
        {
            foreach ( ABCDataObject obj in DataObjectsList.Values )
            {
                if ( obj.Config.Name==mainObj.Config.Name )
                {
                    if ( obj.Config.IsMainObject )
                    {
                        if ( ( mainObj.DataObject as BusinessObject ).GetID()!=Guid.Empty )
                        {
                            obj.ReloadObject( ( mainObj.DataObject as BusinessObject ).GetID() );
                            this.Screen.ChangeStatus( ABCScreenStatus.LoadedData );
                            return;
                        }
                    }

                    obj.ReloadObject();

                    break;
                }
            }
        }
        public void Invalidate ( String strBusObjectName )
        {
            foreach ( ABCDataObject obj in DataObjectsList.Values )
            {
                if ( obj.Config.Name==strBusObjectName )
                {
                    obj.ReloadObject();

                    if ( obj.Config.IsMainObject )
                        this.Screen.ChangeStatus( ABCScreenStatus.LoadedData );

                    break;
                }
            }
        }

        public void Invalidate ( String strBusObjectName , Guid iID )
        {
            foreach ( ABCDataObject obj in DataObjectsList.Values )
            {
                if ( obj.Config.IsMainObject&&obj.Config.Name==strBusObjectName )
                {
                    obj.ReloadObject( iID );
                    this.Screen.ChangeStatus( ABCScreenStatus.LoadedData );
                    break;
                }
            }
        }
        #endregion

        #region ImplementBinding
        public void ImplementBinding ( ABCView root , Control control )
        {
            if ( control is ABCSearchControl||control is ABCSearchPanel )
            {
                if ( control is ABCSearchPanel )
                {
                    ABCSearchPanel searchPanel=control as ABCSearchPanel;
                    if ( String.IsNullOrWhiteSpace( searchPanel.DataSource )==false )
                    {
                        foreach ( string item in searchPanel.DataSource.Split( ';' ) )
                        {
                            if ( SearchPanelList.ContainsKey( item )==false )
                                SearchPanelList.Add( item , searchPanel );
                        }
                    }
                }
                return;
            }
            if ( control is IABCBindableControl )
                BindingToControl( (IABCBindableControl)control );

            if ( control==null||( control is ABCView==false ) )//||( ( control is ABCView )&&( control as ABCView ).IsJoinBindingWithParent ) )
            {
                if ( control==null )
                    control=root;

                foreach ( Control child in control.Controls )
                    ImplementBinding( root , child );
            }
        }
        public void BindingToControl ( IABCBindableControl control )
        {
            if ( control is ABCChartBaseControl )
            {
                foreach ( ABCDataObject data in this.DataObjectsList.Values )
                    ( (ABCChartBaseControl)control ).InitBinding( data.Config.Name , data.Binding );
                return;
            }

            #region Init
            String strDataMember=String.Empty;
            String strTableName=String.Empty;
            String strDataSource=String.Empty;

            PropertyInfo proDataMemberInfo=control.GetType().GetProperty( "DataMember" );
            if ( proDataMemberInfo!=null )
                strDataMember=(String)proDataMemberInfo.GetValue( control , null );

            PropertyInfo proTableNameInfo=control.GetType().GetProperty( "TableName" );
            if ( proTableNameInfo!=null )
                strTableName=(String)proTableNameInfo.GetValue( control , null );

            PropertyInfo proDataSourceInfo=control.GetType().GetProperty( "DataSource" );
            if ( proDataSourceInfo!=null )
                strDataSource=(String)proDataSourceInfo.GetValue( control , null );

            #endregion

            if ( String.IsNullOrWhiteSpace( strTableName )==false&&String.IsNullOrWhiteSpace( strDataMember )==false )
                DataFormatProvider.SetControlFormat( (Control)control , strTableName , strDataMember );

            if ( control is DevExpress.XtraEditors.BaseEdit )
            {
                ( control as DevExpress.XtraEditors.BaseEdit ).Validating+=new System.ComponentModel.CancelEventHandler( DataManager_Validating );
                ( control as DevExpress.XtraEditors.BaseEdit ).Validated+=new EventHandler( DataManager_Validated );
            }

            if ( String.IsNullOrWhiteSpace( strDataSource )==false&&this.DataObjectsList.ContainsKey( strDataSource ) )
            {
                if ( control is IABCGridControl )
                {
                    ( (IABCGridControl)control ).GridDataSource=this.DataObjectsList[strDataSource].Binding;
                    if ( this.DataObjectsList[strDataSource].DataObject is IABCList )
                        ( this.DataObjectsList[strDataSource].DataObject as IABCList ).InitActions( (IABCGridControl)control );
                    ( (IABCGridControl)control ).GridDefaultView.ValidatingEditor+=new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler( GridView_ValidatingEditor );
                    ( (IABCGridControl)control ).GridDefaultView.CellValueChanged+=new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler( GridView_CellValueChanged );
                    ( (IABCGridControl)control ).GridDefaultView.InitNewRow+=new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler( GridDefaultView_InitNewRow );
                }
                else if ( control is ABCPivotGridControl )
                {
                    ( (ABCPivotGridControl)control ).GridDataSource=this.DataObjectsList[strDataSource].Binding;
                }
                else if ( control is ABCTreeList )
                {
                    ( (ABCTreeList)control ).TreeListDataSource=this.DataObjectsList[strDataSource].Binding;
                }

                else
                {
                    #region General Control

                    if ( String.IsNullOrWhiteSpace( control.BindingProperty ) )
                        return;

                    Binding bd=new Binding( control.BindingProperty , this.DataObjectsList[strDataSource].Binding ,
                                                      strDataMember.Split( ':' )[0] , true , DataSourceUpdateMode.OnValidation );
                    ( (Control)control ).DataBindings.Add( bd );
                    //    bd.Parse+=new ConvertEventHandler( BindingControl_ParseFromUI );
                    if ( this.DataObjectsList[strDataSource].Config.DisplayOnly )
                    {
                        if ( control is ABCBindingBaseEdit==false && control is Label ==false)
                        {
                            if ( control is ABCTextEdit )
                                ( control as ABCTextEdit ).ReadOnly=true;
                            else if ( control is ABCMemoEdit )
                                ( control as ABCMemoEdit ).ReadOnly=true;

                            if ( control is DevExpress.XtraEditors.BaseEdit )
                            {
                                ( control as DevExpress.XtraEditors.BaseEdit ).Properties.ReadOnly=true;
                                ( control as DevExpress.XtraEditors.BaseEdit ).Properties.Appearance.BackColor=Color.FromArgb( 181 , 200 , 223 );
                                ( control as DevExpress.XtraEditors.BaseEdit ).Properties.Appearance.ForeColor=Color.Black;
                            }
                        }
                    }
                    #endregion
                }
            }
        }


        #endregion

        #region Search
        public Dictionary<String , ABCSearchPanel> SearchPanelList=new Dictionary<String , ABCSearchPanel>();
        public void InitSearchPanels ( )
        {
            foreach ( ABCSearchPanel panel in this.SearchPanelList.Values )
            {
                panel.Search+=new ABCSearchPanel.ABCSearchEventHandler( SearchPanel_Search );

            }
        }

        public String GetSearchQuery ( String strDataSource )
        {
            if ( SearchPanelList.ContainsKey( strDataSource )&&this.DataObjectsList.ContainsKey( strDataSource ) )
            {
                ABCSearchPanel panel=SearchPanelList[strDataSource];

                ConditionBuilder strBuilder=this.DataObjectsList[strDataSource].GenerateQuery();
                if ( String.IsNullOrWhiteSpace( strBuilder.ToString() ) )
                    return String.Empty;

                panel.GetSearchQuery( strBuilder , panel );
                return strBuilder.ToString();
            }

            return String.Empty;
        }


        public void SearchPanel_Search ( object sender )
        {
            ABCSearchPanel panel=sender as ABCSearchPanel;
            foreach ( string item in panel.DataSource.Split( ';' ) )
            {
                String strQuery=GetSearchQuery( item );
                if ( String.IsNullOrWhiteSpace( strQuery ) )
                    return;

                this.Screen.OnSearch( item , strQuery );
            }
        }

        #endregion

        #region Events

        #region DataInvalidate

        void BindingSource_ListChanged ( object sender , System.ComponentModel.ListChangedEventArgs e )
        {
            if ( e.ListChangedType==System.ComponentModel.ListChangedType.Reset )
            {
                //   BindingSource bdSource=(BindingSource)sender;

            }
        }
        #endregion

        #region DataChange

        public class ABCCancelEventArg
        {
            public bool Cancel=false;
        }
        public class ABCDataChangingStructer : ABCCancelEventArg
        {
            public Control Control;
            public object DataSource;
            //     public String DataSourceName;
            public String DataFieldName;
            public object OldValue;
            public object NewValue;

            public String Error;

            public ABCDataChangingStructer ( Control ctrl , object objDataSource , String strFieldName , object oldValue , object newValue )
            {
                Control=ctrl;
                DataSource=objDataSource;
                //        DataSourceName=strDataSource;
                DataFieldName=strFieldName;
                OldValue=oldValue;
                NewValue=newValue;
                Cancel=false;
            }
        }
        public class ABCDataChangedStructer
        {
            public Control Control;
            public object DataSource;
            public String DataFieldName;
            public object OldValue;
            public object NewValue;

            public ABCDataChangedStructer ( Control ctrl , object objDataSource , String strFieldName , object oldValue , object newValue )
            {
                Control=ctrl;
                DataSource=objDataSource;
                DataFieldName=strFieldName;
                OldValue=oldValue;
                NewValue=newValue;
            }
        }

        private void BindingControl_ParseFromUI ( object sender , ConvertEventArgs e )
        {
            Binding bding=(Binding)sender;
            if ( bding.DataSourceUpdateMode==DataSourceUpdateMode.OnValidation&&bding.ControlUpdateMode==ControlUpdateMode.OnPropertyChanged )
            {
                if ( bding.DataSource!=null&&bding.DataSource is BindingSource )
                {
                    BindingSource bdSource=(BindingSource)bding.DataSource;
                    PropertyInfo property=bdSource.DataSource.GetType().GetProperty( bding.BindingMemberInfo.BindingField );
                    if ( property==null )
                        return;

                    object OldValue=property.GetValue( bdSource.DataSource , null );
                    if ( OldValue!=e.Value&&( OldValue==null||e.Value==null||OldValue.ToString()!=e.Value.ToString() ) )
                    {
                        ABCDataChangingStructer arg=new ABCDataChangingStructer( bding.Control , bdSource.DataSource , bding.BindingMemberInfo.BindingField , OldValue , e.Value );
                        this.Screen.OnDataObjectChangingFromUI( arg );
                        if ( arg.Cancel )
                        {
                            //  e.Value=value;

                            this.Screen.ErrorProvider.SetError( bding.Control , arg.Error );
                        }
                        else
                        {
                            if ( String.IsNullOrWhiteSpace( this.Screen.ErrorProvider.GetError( bding.Control ) )==false )
                                this.Screen.ErrorProvider.SetError( bding.Control , null );
                        }
                    }
                }
            }
        }

        object OldValue=null;
        bool OldValueFirstTime=true;
        private void DataManager_Validating ( object sender , System.ComponentModel.CancelEventArgs e )
        {
            OldValue=null;

            DevExpress.XtraEditors.BaseEdit control=( sender as DevExpress.XtraEditors.BaseEdit );
            if ( control.DataBindings.Count<=0 )
                return;

            Binding bding=(Binding)control.DataBindings[0];
            if ( bding.DataSourceUpdateMode==DataSourceUpdateMode.OnValidation&&bding.ControlUpdateMode==ControlUpdateMode.OnPropertyChanged )
            {
                if ( bding.DataSource!=null&&bding.DataSource is BindingSource )
                {
                    BindingSource bdSource=(BindingSource)bding.DataSource;
                    if ( bdSource.Current==null )
                        return;

                    PropertyInfo property=bdSource.Current.GetType().GetProperty( bding.BindingMemberInfo.BindingField );
                    if ( property==null )
                        return;

                    #region Get oldValue - newValue
                    object oldValue=property.GetValue( bdSource.Current , null );
                    if ( oldValue==control.EditValue )
                    {
                        OldValue=oldValue;
                        return;
                    }

                    object newValue=null;

                    if ( ( oldValue!=null&&control.EditValue!=null )&&( oldValue.ToString()!=control.EditValue.ToString() ) )
                    {
                        System.ComponentModel.TypeConverter converter=System.ComponentModel.TypeDescriptor.GetConverter( oldValue.GetType() );

                        try
                        {
                            newValue=converter.ConvertFromString( null , System.Globalization.CultureInfo.CurrentUICulture , control.EditValue.ToString() );
                        }
                        catch ( Exception ex )
                        {
                        }
                        if ( newValue==null )
                        {
                            try
                            {
                                newValue=converter.ConvertFromString( null , System.Globalization.CultureInfo.InvariantCulture , control.EditValue.ToString() );
                            }
                            catch ( Exception ex )
                            {
                            }
                        }
                    }
                    #endregion

                    if ( oldValue==newValue||( oldValue!=null&&newValue!=null&&oldValue.ToString()==newValue.ToString() )
                        ||( oldValue==null&&newValue!=null&&newValue.ToString()=="" )
                        ||( newValue==null&&oldValue!=null&&oldValue.ToString()=="" ) )
                        return;

                    ABCDataChangingStructer arg=new ABCDataChangingStructer( bding.Control , bdSource.DataSource , bding.BindingMemberInfo.BindingField , oldValue , newValue );
                    this.Screen.OnDataObjectChangingFromUI( arg );
                    if ( arg.Cancel )
                    {
                        if ( String.IsNullOrWhiteSpace( arg.Error )==false )
                        {
                            e.Cancel=arg.Cancel;
                            control.ErrorText=arg.Error;
                        }
                        else
                        {
                            control.EditValue=oldValue;
                        }
                    }
                    else
                    {
                        OldValue=oldValue;
                    }

                }
            }
        }
        private void DataManager_Validated ( object sender , EventArgs e )
        {
            DevExpress.XtraEditors.BaseEdit control=( sender as DevExpress.XtraEditors.BaseEdit );
            if ( control.DataBindings.Count<=0 )
                return;

            Binding bding=(Binding)control.DataBindings[0];
            if ( bding.DataSourceUpdateMode==DataSourceUpdateMode.OnValidation&&bding.ControlUpdateMode==ControlUpdateMode.OnPropertyChanged )
            {
                if ( bding.DataSource!=null&&bding.DataSource is BindingSource )
                {
                    BindingSource bdSource=(BindingSource)bding.DataSource;
                    if ( bdSource.Current==null )
                        return;

                    PropertyInfo property=bdSource.Current.GetType().GetProperty( bding.BindingMemberInfo.BindingField );
                    if ( property==null )
                        return;

                    object newValue=property.GetValue( bdSource.Current , null );
                    if ( newValue!=null&&String.IsNullOrWhiteSpace( newValue.ToString() ) )
                        newValue=null;

                    if ( OldValue==newValue||( OldValue!=null&&newValue!=null&&OldValue.ToString()==newValue.ToString() )
                    ||( OldValue==null&&newValue!=null&&newValue.ToString()=="" )
                    ||( newValue==null&&OldValue!=null&&OldValue.ToString()=="" ) )
                        return;

                    ABCDataChangedStructer arg=new ABCDataChangedStructer( bding.Control , bdSource.DataSource , bding.BindingMemberInfo.BindingField , OldValue , newValue );
                    this.Screen.OnDataObjectChangedFromUI( arg );

                    if ( this.MainObject!=null )
                        if ( this.MainObject.DataObject!=null&&this.MainObject.DataObject is BusinessObject )
                            if ( this.Screen.UIManager.IsToolBarButtonVisibility( ABCView.ScreenBarButton.Edit ) )
                                this.Screen.DoAction( ABCScreenAction.Edit , false );
                }
            }

        }

        private void GridView_ValidatingEditor ( object sender , DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e )
        {
            OldValue=null;

            DevExpress.XtraGrid.Views.Grid.GridView view=sender as DevExpress.XtraGrid.Views.Grid.GridView;
            object obj=view.GetRow( view.FocusedRowHandle );
            if ( obj==null||obj is BusinessObject==false )
                return;

            if ( DataStructureProvider.IsTableColumn( ( obj as BusinessObject ).AATableName , view.FocusedColumn.FieldName )==false )
                return;

            object oldValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj as BusinessObject , view.FocusedColumn.FieldName );
            if ( oldValue!=e.Value&&( oldValue==null||e.Value==null||oldValue.ToString()!=e.Value.ToString() ) ) 
            {
                ABCDataChangingStructer arg=new ABCDataChangingStructer( ( view as IABCGridView ).ABCGridControl as Control , obj , view.FocusedColumn.FieldName , oldValue , e.Value );
                this.Screen.OnDataObjectChangingFromUI( arg );
                if ( arg.Cancel )
                {
                    if ( String.IsNullOrWhiteSpace( arg.Error )==false )
                    {
                        e.Valid=false;
                        e.ErrorText=arg.Error;
                    }
                    else
                        e.Value=oldValue;
                }
                else
                {
                    OldValue=oldValue;
                }
            }
        }
        private void GridView_CellValueChanged ( object sender , DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=sender as DevExpress.XtraGrid.Views.Grid.GridView;
            object obj=view.GetRow( e.RowHandle );
            if ( obj==null||obj is BusinessObject==false )
                return;

            if ( DataStructureProvider.IsTableColumn( ( obj as BusinessObject ).AATableName , e.Column.FieldName.Split( ':' )[0] )==false )
                return;

            object newValue=e.Value;
            if ( OldValue!=newValue&&( OldValue==null||newValue==null||OldValue.ToString()!=newValue.ToString() ) )
            {
                ABCDataChangedStructer arg=new ABCDataChangedStructer( ( view as IABCGridView ).ABCGridControl as Control , obj , e.Column.FieldName.Split( ':' )[0] , OldValue , newValue );
                this.Screen.OnDataObjectChangedFromUI( arg );
            }

        }
        private void GridDefaultView_InitNewRow ( object sender , DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=sender as DevExpress.XtraGrid.Views.Grid.GridView;
            object obj=view.GetRow( e.RowHandle );
            if ( obj==null||obj is BusinessObject==false )
                return;

            ABCDataChangedStructer arg=new ABCDataChangedStructer( ( view as IABCGridView ).ABCGridControl as Control , obj , "" , null , null );
            this.Screen.OnDataObjectChangedFromUI( arg );
        }


        #endregion

        #endregion

    }

    public class ABCDataObject
    {
        public DataManager DataManager;

        public ABCBindingConfig Config;
        public BindingSource Binding;
        public String TableName;

        object innerData;
        public object DataObject
        {
            get
            {
                return innerData;
            }
            set
            {
                innerData=value;
                if ( innerData==null )
                {
                    innerData=DataType;
                    if ( typeof( BusinessObject ).IsAssignableFrom( DataType ) )
                    {
                        innerData=(BusinessObject)ABCDynamicInvoker.CreateInstanceObject( DataType );
                        ( innerData as BusinessObject ).PropertyChanged+=new System.ComponentModel.PropertyChangedEventHandler( ABCDataObject_PropertyChanged );
                    }
                }
            }
        }

        public BusinessObjectController Controller;
        public Type DataType;

        public ABCDataObject ( ABCBindingConfig info )
        {
            Binding=new System.Windows.Forms.BindingSource();
            Binding.ListChanged+=new System.ComponentModel.ListChangedEventHandler( BindingSource_ListChanged );
            Binding.PositionChanged+=new EventHandler( BindingSource_PositionChanged );
            Config=info;


            TableName=Config.TableName;
            Controller=BusinessControllerFactory.GetBusinessController( TableName );

            if ( Config.IsList==false )
            {
                DataObject=BusinessObjectFactory.GetBusinessObject( TableName );
                DataType=BusinessObjectFactory.GetBusinessObjectType( TableName );
            }
            else
            {
                Type type=BusinessObjectFactory.GetBusinessObjectType( TableName );
                if ( type!=null )
                {
                    type=typeof( ABCList<> ).MakeGenericType( type );
                    MethodInfo method;
                    if ( DataObject==null&&type!=null )
                    {
                        DataObject=ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( type );
                        method=type.GetMethod( "SetBinding" );
                        method.Invoke( DataObject , new object[] { this } );

                        DataType=type;
                    }
                }

            }



            RefreshUI();
        }

        public bool IsModified
        {
            get;
            set;
        }

        public bool IsChildrendModified
        {
            get
            {

                foreach ( ABCBindingConfig config in this.Config.Children.Values )
                {
                    ABCDataObject child=this.DataManager.DataObjectsList[config.Name];
                    if ( child.IsModified||child.IsChildrendModified )
                        return true;
                }
                return false;
            }
        }

        #region Utils
        public void BeginEdit ( )
        {
            Binding.RaiseListChangedEvents=false;
            if ( DataObject is BusinessObject )
                ( (BusinessObject)DataObject ).BeginEdit();
        }
        public void CancelEdit ( )
        {
            if ( DataObject is BusinessObject )
                ( (BusinessObject)DataObject ).CancelEdit();

            Binding.RaiseListChangedEvents=true;
            Binding.ResetCurrentItem();
        }
        public void EndEdit ( )
        {
            if ( DataObject is BusinessObject )
                ( (BusinessObject)DataObject ).EndEdit();

            Binding.RaiseListChangedEvents=true;
            Binding.ResetCurrentItem();
        }

        public bool InRelationWithMainObject ( )
        {
            if ( Config.IsMainObject )
                return true;

            if ( Config.Parent!=null&&this.DataManager.DataObjectsList.ContainsKey( Config.Parent.Name ) )
                return this.DataManager.DataObjectsList[Config.Parent.Name].InRelationWithMainObject();

            return false;
        }
        #endregion

        #region PropertyChanged
        bool IsLookPositionChange=false;
        int position=-1;
        void BindingSource_PositionChanged ( object sender , EventArgs e )
        {
            object previous=( position!=-1&&Binding.Count>position )?(object)Binding[position]:null;
            int current=Binding.Position;

            if ( DataManager!=null&&previous!=null&&current!=position )
            {
                if ( this.Config.IsMainObject&&( DataManager.Screen.ScreenStatus==ABCScreenStatus.New||DataManager.Screen.ScreenStatus==ABCScreenStatus.Edit ) )
                {
                    ABCHelper.ABCMessageBox.Show( "Phải lưu trước khi chọn phiếu khác!" , "Thông báo" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                    IsLookPositionChange=true;
                    Binding.Position=position;
                    IsLookPositionChange=false;
                    return;
                }

                if ( this.Config.ConfirmSaveChildren )
                {
                    if ( this.IsChildrendModified )
                    {
                        DialogResult result=ABCHelper.ABCMessageBox.Show( "Bạn có muốn lưu hay không?" , "Thông báo" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                        if ( result==DialogResult.Yes )
                        {
                            IsLookPositionChange=true;
                            Binding.Position=position;
                            IsLookPositionChange=false;
                            this.SaveChildren( true , true );
                        }
                        if ( result==DialogResult.Cancel )
                        {
                            IsLookPositionChange=true;
                            Binding.Position=position;
                            IsLookPositionChange=false;
                            return;
                        }
                    }
                }
            }

            position=current;
            Binding.Position=current;

            if ( IsLookPositionChange==false )
                ReloadChildren();
        }
        void BindingSource_ListChanged ( object sender , System.ComponentModel.ListChangedEventArgs e )
        {
            if ( e.ListChangedType==System.ComponentModel.ListChangedType.Reset )
                ReloadChildren();
        }

        void ABCDataObject_PropertyChanged ( object sender , System.ComponentModel.PropertyChangedEventArgs e )
        {
            DataManager.Screen.OnDataObjectChangedFromCode( this , e.PropertyName );
        }

        #endregion

        #region Invalidate

        #region Invalidate Current Object
        //Search from DB
        public void ReloadObject ( )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData();

            RefreshUI();
        }
        public void ReloadObject ( Guid iID )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData( iID );
            ReloadChildren();
            RefreshUI();

            IsModified=false;
        }
        public void ReloadObject ( object obj )//BusinessObject or ABCList
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData( obj );
            ReloadChildren();
            RefreshUI();

            IsModified=false;
        }
        public void ReloadObject ( DataSet ds )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData( ds );
            ReloadChildren();
            RefreshUI();

            IsModified=false;
        }
        public void ReloadObject ( IList list )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData( list );
            ReloadChildren();
            RefreshUI();

            IsModified=false;
        }
        public void ReloadObject ( String strQuery )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.Config.TableName , TablePermission.AllowView )==false )
                return;

            ReloadObjectData( strQuery );
            ReloadChildren();
            RefreshUI();

            IsModified=false;
        }

        public void ReloadObjectData ( )
        {

            ConditionBuilder strBuilder=GenerateQuery();
            if ( strBuilder==null||String.IsNullOrWhiteSpace( strBuilder.ToString() ) )
                return;

            DataSet ds=DataQueryProvider.RunQuery( strBuilder.ToString() );
            ReloadObjectData( ds );

            IsModified=false;
        }
        public void ReloadObjectData ( Guid iID )
        {
            DataManager.ABCCancelEventArg arg=new DataManager.ABCCancelEventArg();
            this.DataManager.Screen.OnDataObjectInvalidating( this , arg );
            if ( arg.Cancel )
                return;

            DataObject=BusinessObjectHelper.GetBusinessObject( this.TableName , iID );
            if ( DataObject==null&&this.Config.IsMainObject )
                DataObject=BusinessObjectFactory.GetBusinessObject( this.TableName );

            this.DataManager.Screen.OnDataObjectInvalidated( this );

            IsModified=false;
        }
        public void ReloadObjectData ( object obj )//BusinessObject or ABCList
        {
            if ( obj is DataSet )
                ReloadObjectData( obj as DataSet );
            else if ( Config.IsList==false )
            {
                DataManager.ABCCancelEventArg arg=new DataManager.ABCCancelEventArg();
                this.DataManager.Screen.OnDataObjectInvalidating( this , arg );
                if ( arg.Cancel )
                    return;
                DataObject=BusinessObjectHelper.GetBusinessObject( ( obj as BusinessObject ).AATableName , ( obj as BusinessObject ).GetID() );

                this.DataManager.Screen.OnDataObjectInvalidated( this );
            }

            IsModified=false;
        }
        public void ReloadObjectData ( DataSet ds )
        {
            DataManager.ABCCancelEventArg arg=new DataManager.ABCCancelEventArg();
            this.DataManager.Screen.OnDataObjectInvalidating( this , arg );
            if ( arg.Cancel )
                return;

            if ( Config.IsList )
            {
                Type genericType=typeof( ABCList<> );
                Type type=genericType.MakeGenericType( BusinessObjectFactory.GetBusinessObjectType( TableName ) );
                MethodInfo method=type.GetMethod( "LoadData" , new Type[] { typeof( DataSet ) } );
                method.Invoke( DataObject , new object[] { ds } );
            }
            else
            {
                DataObject=BusinessObjectHelper.GetBusinessObject( ds , this.TableName );
            }

            this.DataManager.Screen.OnDataObjectInvalidated( this );

            IsModified=false;
        }
        public void ReloadObjectData ( IList list )
        {
            DataManager.ABCCancelEventArg arg=new DataManager.ABCCancelEventArg();
            this.DataManager.Screen.OnDataObjectInvalidating( this , arg );
            if ( arg.Cancel )
                return;

            if ( Config.IsList )
            {
                Type type=typeof( ABCList<> ).MakeGenericType( BusinessObjectFactory.GetBusinessObjectType( TableName ) );

                Type typeIList=typeof( IList<> ).MakeGenericType( BusinessObjectFactory.GetBusinessObjectType( TableName ) );

                MethodInfo method=type.GetMethod( "LoadData" , new Type[] { typeIList } );
                method.Invoke( DataObject , new object[] { list } );
            }
            else
            {
                if ( list.Count>0 )
                    DataObject=list[0];
            }

            this.DataManager.Screen.OnDataObjectInvalidated( this );

            IsModified=false;
        }
        public void ReloadObjectData ( String strQuery )
        {
            DataSet ds=DataQueryProvider.RunQuery( strQuery );
            ReloadObjectData( ds );

            IsModified=false;
        }

        #endregion

        public void ReloadChildren ( )
        {
            if ( DataManager!=null&&DataManager.DataObjectsList!=null&&Binding!=null )
            {
                foreach ( ABCBindingConfig childInfo in Config.Children.Values )
                    DataManager.DataObjectsList[childInfo.Name].ReloadObject();
            }

            this.DataManager.Screen.OnDataObjectInvalidated( this );
        }

        public ABCHelper.ConditionBuilder GenerateQuery ( )
        {
            return GenerateQuery( Config.IsList );
        }
        public ABCHelper.ConditionBuilder GenerateQuery ( Boolean isList )
        {
            ConditionBuilder strBuilder=new ConditionBuilder();

            if ( String.IsNullOrWhiteSpace( Config.SQLQuery )==false )
            {
                strBuilder.Append( " "+Config.SQLQuery+" " );
                return strBuilder;
            }

            if ( Config.Parent==null )
            {
                #region Root
                if ( isList )
                {
                    if ( this.Config.TopCount>0 )
                        strBuilder.Append( String.Format( @"SELECT TOP {0} * FROM {1}" , this.Config.TopCount , this.TableName ) );
                    else
                        strBuilder.Append( String.Format( @"SELECT * FROM {0}" , this.TableName ) );
                }
                else  //MainObject
                {
                    strBuilder.Append( String.Format( @"SELECT TOP 1 * FROM {0}" , this.TableName ) );

                    String strPK=DataStructureProvider.GetPrimaryKeyColumn( this.TableName );
                    if ( DataManager.ScreenConfig.ParameterList.ContainsKey( strPK ) )
                        strBuilder.AddCondition( String.Format( @" {0} = {1} " , strPK , DataManager.ScreenConfig.ParameterList[strPK].ToString() ) );
                }
                #endregion
            }
            else
            {
                #region Children

                if ( isList )
                {
                    if ( this.Config.TopCount>0 )
                        strBuilder.Append( String.Format( @"SELECT TOP {0} * FROM {1}" , this.Config.TopCount , this.TableName ) );
                    else
                        strBuilder.Append( String.Format( @"SELECT * FROM {0}" , this.TableName ) );
                }
                else
                    strBuilder.Append( String.Format( @"SELECT TOP 1 * FROM {0} " , this.TableName ) );

                #region Relation Condition
                if ( String.IsNullOrWhiteSpace( Config.ParentField )||String.IsNullOrWhiteSpace( Config.ChildField ) )
                {
                    String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , Config.Parent.TableName );
                    if ( String.IsNullOrWhiteSpace( strFK )==false )
                    {
                        Guid iID=BusinessObjectHelper.GetIDValue( DataManager.DataObjectsList[Config.Parent.Name].Binding.Current as BusinessObject );
                        strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , strFK , iID ) );
                    }
                }
                else
                {
                    String strTypeName=DataStructureProvider.GetCodingType( Config.Parent.TableName , Config.ParentField );
                    object obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( DataManager.DataObjectsList[Config.Parent.Name].Binding.Current as BusinessObject , Config.ParentField );
                    if ( obj!=null&&DataStructureProvider.IsTableColumn( Config.TableName , Config.ChildField ) )
                    {
                        if ( strTypeName=="String"||strTypeName=="DateTime"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )
                            strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , Config.ChildField , obj.ToString() ) );
                        else
                            strBuilder.AddCondition( String.Format( @" {0} = {1} " , Config.ChildField , obj.ToString() ) );

                        if ( !String.IsNullOrWhiteSpace( Config.ParentField1 )&&!String.IsNullOrWhiteSpace( Config.ChildField1 ) )
                        {
                            strTypeName=DataStructureProvider.GetCodingType( Config.Parent.TableName , Config.ParentField1 );
                            obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( DataManager.DataObjectsList[Config.Parent.Name].Binding.Current as BusinessObject , Config.ParentField1 );
                            if ( obj!=null&&DataStructureProvider.IsTableColumn( Config.TableName , Config.ChildField1 ) )
                            {
                                if ( strTypeName=="String"||strTypeName=="DateTime"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )
                                    strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , Config.ChildField1 , obj.ToString() ) );
                                else
                                    strBuilder.AddCondition( String.Format( @" {0} = {1} " , Config.ChildField1 , obj.ToString() ) );

                            }
                        }

                        if ( !String.IsNullOrWhiteSpace( Config.ParentField2 )&&!String.IsNullOrWhiteSpace( Config.ChildField2) )
                        {
                            strTypeName=DataStructureProvider.GetCodingType( Config.Parent.TableName , Config.ParentField2 );
                            obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( DataManager.DataObjectsList[Config.Parent.Name].Binding.Current as BusinessObject , Config.ParentField2 );
                            if ( obj!=null&&DataStructureProvider.IsTableColumn( Config.TableName , Config.ChildField2 ) )
                            {
                                if ( strTypeName=="String"||strTypeName=="DateTime"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )
                                    strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , Config.ChildField2 , obj.ToString() ) );
                                else
                                    strBuilder.AddCondition( String.Format( @" {0} = {1} " , Config.ChildField2 , obj.ToString() ) );

                            }
                        }

                        if ( !String.IsNullOrWhiteSpace( Config.ParentField3 )&&!String.IsNullOrWhiteSpace( Config.ChildField3) )
                        {
                            strTypeName=DataStructureProvider.GetCodingType( Config.Parent.TableName , Config.ParentField3 );
                            obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( DataManager.DataObjectsList[Config.Parent.Name].Binding.Current as BusinessObject , Config.ParentField3 );
                            if ( obj!=null&&DataStructureProvider.IsTableColumn( Config.TableName , Config.ChildField3 ) )
                            {
                                if ( strTypeName=="String"||strTypeName=="DateTime"||strTypeName=="Guid"||strTypeName=="Nullable<Guid>" )
                                    strBuilder.AddCondition( String.Format( @" {0} = '{1}' " , Config.ChildField3 , obj.ToString() ) );
                                else
                                    strBuilder.AddCondition( String.Format( @" {0} = {1} " , Config.ChildField3 , obj.ToString() ) );

                            }
                        }
                    }
                    else
                    {
                        ReloadObjectData( new DataSet() );
                        return null;
                    }
                }
                #endregion

                #endregion
            }

            if ( DataStructureProvider.IsExistABCStatus( this.TableName ) )
                strBuilder.AddCondition( QueryGenerator.GenerateCondition( this.TableName , ABCCommon.ABCColumnType.ABCStatus ) );

            #region Filter with current User

            if ( this.Config.CurrentUserOnly )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , "ADUsers" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUser!=null )
                    strBuilder.AddCondition( String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentUser.ADUserID ) );
            }
            if ( this.Config.CurrentUserGroupOnly )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , "ADUsers" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUser!=null )
                {
                    if ( ABCUserProvider.CurrentUserGroup!=null )
                    {
                        String strQueryUserID=QueryGenerator.AddEqualCondition( QueryGenerator.GenSelect( "ADUsers" , "ADUserID" , false ) , "FK_ADUserGroupID" , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
                        strBuilder.AddCondition( String.Format( " {0} IN ({1})" , strFK , strQueryUserID ) );
                    }
                }
                else
                {
                    strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , "ADUserGroups" );
                    if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentUserGroup!=null )
                        strBuilder.AddCondition( String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentUserGroup.ADUserGroupID ) );
                }
            }
            if ( this.Config.CurrentEmployeeOnly )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , "HREmployees" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentEmployee!=null )
                    strBuilder.AddCondition( String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentEmployee.HREmployeeID ) );
            }
            if ( this.Config.CurrentCompanyUnitOnly )
            {
                String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.TableName , "GECompanyUnits" );
                if ( !String.IsNullOrWhiteSpace( strFK )&&ABCUserProvider.CurrentCompanyUnit!=null )
                    strBuilder.AddCondition( String.Format( " {0} = '{1}'" , strFK , ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID ) );
                else
                {
                    if ( !String.IsNullOrWhiteSpace( strFK )&&!String.IsNullOrWhiteSpace( DataStructureProvider.GetForeignKeyOfTableName( "HREmployees" , "GECompanyUnits" ) ) )
                    {
                        String strTemp=QueryGenerator.GenSelect( "GECompanyUnits" , DataStructureProvider.GetForeignKeyOfTableName( "HREmployees" , "GECompanyUnits" ) , false );
                        strBuilder.AddCondition( String.Format( " {0} IN ({1})" , strFK , strTemp ) );
                    }
                }
            }
            #endregion

            #region FilterCondintion

            String strFilterCondintion=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( Config.FilterCondition ) );
            if ( String.IsNullOrWhiteSpace( strFilterCondintion )==false )
                strBuilder.AddCondition( strFilterCondintion );
            if ( String.IsNullOrWhiteSpace( Config.SQLFilterCondition )==false )
                strBuilder.AddCondition( Config.SQLFilterCondition );

            if ( this.Config.IsMainObject )
            {
                if ( this.Config.TableName==this.DataManager.Screen.UIManager.View.MainTableName&&
                   String.IsNullOrWhiteSpace( this.DataManager.Screen.UIManager.View.MainFieldName )==false&&
                   String.IsNullOrWhiteSpace( this.DataManager.Screen.UIManager.View.MainValue )==false )
                {
                    if ( DataStructureProvider.IsTableColumn( this.Config.TableName , this.DataManager.Screen.UIManager.View.MainFieldName ) )
                        strBuilder.AddCondition( String.Format( "{0} ='{1}' " , this.DataManager.Screen.UIManager.View.MainFieldName , this.DataManager.Screen.UIManager.View.MainValue ) );
                }
            }
            if ( isList )
            {
                foreach ( ABCScreen.ABCBindingConfig.FieldFilterConfig fieldConfig in this.Config.FieldFilterConditions )
                {
                    String strPK=DataStructureProvider.GetPrimaryKeyColumn( fieldConfig.TableName );
                    if ( String.IsNullOrWhiteSpace( fieldConfig.FilterString )==false&&String.IsNullOrWhiteSpace( strPK )==false )
                    {
                        String strFilterString=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( fieldConfig.FilterString ) );
                        String strFilterQuery=string.Format( "( {0} IS NULL OR {0} IN (SELECT {1} FROM {2} WHERE {3}) )" , fieldConfig.Field , strPK , fieldConfig.TableName , strFilterString );
                        strBuilder.AddCondition( strFilterQuery );
                    }
                }
            }

            //       strBuilder.AddCondition( Security.DataAuthentication.GetAuthenticationString( this.TableName ) );

            #endregion

            if ( String.IsNullOrWhiteSpace( Config.SQLExtension )==false )
                strBuilder.AppendEndString( " "+Config.SQLExtension+" " );

            this.DataManager.Screen.OnGeneratingFilterQuery( this , strBuilder );

            if ( !strBuilder.ToString().Contains( "ORDER BY" ) )
            {
                if ( !isList )
                {
                    if ( DataStructureProvider.IsTableColumn( this.TableName , ABCCommon.ABCConstString.colDocumentDate ) )
                        strBuilder.AppendEndString( String.Format( @" ORDER BY {0} DESC" , ABCCommon.ABCConstString.colDocumentDate ) );
                    else
                        if ( DataStructureProvider.IsTableColumn( this.TableName , ABCCommon.ABCConstString.colCreateTime ) )
                            strBuilder.AppendEndString( String.Format( @" ORDER BY {0} ASC" , ABCCommon.ABCConstString.colCreateTime ) );
                }
                else
                    if ( DataStructureProvider.IsTableColumn( this.TableName , ABCCommon.ABCConstString.colCreateTime ) )
                        strBuilder.AppendEndString( String.Format( @" ORDER BY {0} ASC" , ABCCommon.ABCConstString.colCreateTime ) );
            }

            return strBuilder;
        }
        public void RefreshUI ( )
        {
            try
            {
                IsLookPositionChange=true;

                Binding.DataSource=DataObject;

                if ( DataObject is IABCList )
                    Binding.ResetBindings( false );

                IsLookPositionChange=false;

            }
            catch ( Exception ex )
            {
                IsLookPositionChange=false;
            }
        }
        #endregion

        #region Refresh
        public void Refresh ( )
        {
            if ( this.DataManager.MainObject==null&&( this.IsModified||( this.IsChildrendModified&&this.Config.ConfirmSaveChildren ) ) )
            {
                DialogResult result=ABCHelper.ABCMessageBox.Show( "Bạn có muốn lưu hay không?" , "Thông báo" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                if ( result==DialogResult.Yes )
                {
                    if ( !this.IsModified )
                        this.SaveChildren( true , true );
                    else
                        this.Save( true , true );
                }

                if ( result==DialogResult.Cancel )
                    return;
            }
          
            if ( this.Config.IsMainObject )
            {
                if ( DataObject!=null )
                    ReloadObject( DataObject );
            }
            else
                ReloadObject();

            RefreshUI();
        }
        #endregion

        #region New - Delete - Save
        public void New ( )
        {
            if ( DataObject is BusinessObject||typeof( System.Type ).IsAssignableFrom( DataObject.GetType() ) )
                DataObject=BusinessObjectFactory.GetBusinessObject( TableName );

            if ( DataObject is IABCList )
                ( DataObject as IABCList ).Clear();

            RefreshUI();
        }
        public void Delete ( )
        {
            if ( DataObject==null||this.Config.DisplayOnly||!ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowDelete ) )
                return;

            #region Delete Children
            if ( DataManager!=null&&DataManager.DataObjectsList!=null )
            {
                foreach ( ABCBindingConfig childInfo in Config.Children.Values )
                    DataManager.DataObjectsList[childInfo.Name].Delete();
            }
            #endregion

            #region Delete Current

            if ( DataObject is BusinessObject )
            {
                Controller.DeleteObject( DataObject as BusinessObject );
                DataObject=null;
            }

            if ( DataObject is IABCList )
            {
                if ( this.Config.Parent!=null&&DataManager.DataObjectsList!=null&&DataManager.DataObjectsList.ContainsKey( this.Config.Parent.Name ) )
                {
                    ABCDataObject objParent=DataManager.DataObjectsList[this.Config.Parent.Name];
                    Guid iID=BusinessObjectHelper.GetIDValue( objParent.Binding.Current as BusinessObject );
                    Controller.DeleteObjectsByFK( this.Config.ChildField , iID );

                    if ( DataStructureProvider.IsForeignKey( this.Config.TableName , this.Config.ChildField ) )
                    {
                        String strQuery=QueryGenerator.GenSelect( this.Config.Parent.TableName , DataStructureProvider.GetPrimaryKeyColumn( this.Config.Parent.TableName ) , false );
                        if ( DataStructureProvider.IsTableColumn( this.Config.TableName , ABCCommon.ABCConstString.colABCStatus ) )
                            strQuery=String.Format( "UPDATE {0} SET ABCStatus ='Deleted' WHERE {1} NOT IN ({2})" , this.Config.TableName , this.Config.ChildField , strQuery );
                        else
                            strQuery=String.Format( "DELETE FROM {0} WHERE {1} NOT IN ({2})" , this.Config.TableName , this.Config.ChildField , strQuery );
                        BusinessObjectController.RunQuery( strQuery );
                    }
                }
                else
                {
                    foreach ( object obj in ( DataObject as IABCList ) )
                    {
                        if ( obj is BusinessObject )
                            Controller.DeleteObject( obj as BusinessObject );
                    }
                }

                ( DataObject as IABCList ).Clear();
            }

            #endregion

            RefreshUI();
        }

        public void Save ( bool isSaveChildren , bool isShowWaitingDlg )
        {
            if ( DataObject==null||this.Config.DisplayOnly )
                return;

            if ( !ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowNew )&&
                 !ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowEdit ) )
                return;

            Boolean isSaved=false;

            Guid iParentID=Guid.Empty;
            ABCDataObject objParent=null;
            if ( this.Config.Parent!=null&&DataManager.DataObjectsList!=null&&DataManager.DataObjectsList.ContainsKey( this.Config.Parent.Name ) )
            {
                objParent=DataManager.DataObjectsList[this.Config.Parent.Name];
                if ( objParent.DataObject!=null )
                {
                    if ( objParent.Binding.Current!=null&&objParent.Binding.Current is BusinessObject )
                        iParentID=BusinessObjectHelper.GetIDValue( objParent.Binding.Current as BusinessObject );
                    else if ( objParent.DataObject is BusinessObject )
                        iParentID=BusinessObjectHelper.GetIDValue( objParent.DataObject as BusinessObject );
                }
            }

            #region Save Current

            if ( DataObject is BusinessObject )
            {
                if ( BusinessObjectHelper.IsCleanObject( DataObject as BusinessObject )==false )
                {
                    object objOldValue=null;

                    #region BusinessObject

                    if ( iParentID!=Guid.Empty&&objParent!=null&&String.IsNullOrWhiteSpace( this.Config.ChildField )==false
                     &&DataStructureProvider.IsPrimaryKey( objParent.TableName , this.Config.ParentField )==false )
                    {
                        objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , this.Config.ChildField );
                        ABCBusinessEntities.ABCDynamicInvoker.SetValue( DataObject as BusinessObject , this.Config.ChildField , iParentID );
                        if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=iParentID )
                            this.IsModified=true;
                    }

                    if ( BusinessObjectHelper.SetAutoValue( DataObject as BusinessObject ) )
                        this.IsModified=true;

                    if ( CurrencyProvider.GenerateCurrencyValue( DataObject as BusinessObject ) )
                        this.IsModified=true;

                    if ( DataStructureProvider.IsTableColumn( this.Config.TableName , this.Config.DefaultField ) )
                    {
                        objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , this.Config.DefaultField );
                        ABCDynamicInvoker.SetValue( DataObject as BusinessObject , this.Config.DefaultField , this.Config.DefaultValue );
                        if ( objOldValue.ToString()!=this.Config.DefaultValue )
                            this.IsModified=true;
                    }

                    #region Set Default User Info
                    if ( this.Config.CurrentUserOnly )
                    {
                        String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "ADUsers" );
                        if ( !String.IsNullOrWhiteSpace( strFK ) )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , strFK );
                            ABCDynamicInvoker.SetValue( DataObject as BusinessObject , strFK , ABCUserProvider.CurrentUser.ADUserID );
                            if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentUser.ADUserID )
                                this.IsModified=true;
                        }
                    }
                    if ( this.Config.CurrentEmployeeOnly )
                    {
                        String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "HREmployees" );
                        if ( !String.IsNullOrWhiteSpace( strFK ) )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , strFK );
                            ABCDynamicInvoker.SetValue( DataObject as BusinessObject , strFK , ABCUserProvider.CurrentEmployee.HREmployeeID );
                            if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentEmployee.HREmployeeID )
                                this.IsModified=true;
                        }
                    }
                    if ( this.Config.CurrentUserGroupOnly )
                    {
                        String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "ADUserGroups" );
                        if ( !String.IsNullOrWhiteSpace( strFK ) )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , strFK );
                            ABCDynamicInvoker.SetValue( DataObject as BusinessObject , strFK , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
                            if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentUserGroup.ADUserGroupID )
                                this.IsModified=true;
                        }
                    }
                    if ( this.Config.CurrentCompanyUnitOnly )
                    {
                        String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "GECompanyUnits" );
                        if ( !String.IsNullOrWhiteSpace( strFK ) )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( DataObject as BusinessObject , strFK );
                            ABCDynamicInvoker.SetValue( DataObject as BusinessObject , strFK , ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID );
                            if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID )
                                this.IsModified=true;
                        }
                    }
                    #endregion

                    if ( this.IsModified )
                    {
                        Guid objID=BusinessObjectHelper.GetIDValue( DataObject as BusinessObject );
                        if ( objID!=Guid.Empty )
                        {
                            if ( this.InRelationWithMainObject()&&DataManager.Screen.ScreenStatus==ABCScreenStatus.New )
                                Controller.CreateObject( DataObject as BusinessObject );
                            else
                                Controller.UpdateObject( DataObject as BusinessObject );
                        }
                        else
                            Controller.CreateObject( DataObject as BusinessObject );

                        #region Set No
                        objID=BusinessObjectHelper.GetIDValue( DataObject as BusinessObject );
                        if ( objID!=Guid.Empty )
                        {
                            String strNoCol=DataStructureProvider.GetNOColumn( ( DataObject as BusinessObject ).AATableName );
                            if ( String.IsNullOrWhiteSpace( strNoCol )==false )
                            {
                                String strNo=ABCBusinessEntities.ABCDynamicInvoker.GetValue( DataObject as BusinessObject , strNoCol ).ToString();
                                if ( String.IsNullOrWhiteSpace( strNo ) )
                                {
                                    ABCBusinessEntities.ABCDynamicInvoker.SetValue( DataObject as BusinessObject , strNoCol , DataConfigProvider.TableConfigList[( DataObject as BusinessObject ).AATableName].PrefixNo+objID.ToString() );

                                    Controller.UpdateObject( DataObject as BusinessObject );
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    isSaved=true;
                    IsModified=false;
                }
            }
            else if ( DataObject is IABCList )
            {

                #region IABCList

                #region InRelationWithMainObject
                //if ( this.InRelationWithMainObject() )
                //{
                if ( iParentID!=Guid.Empty&&objParent!=null&&String.IsNullOrWhiteSpace( this.Config.ChildField )==false
                    &&DataStructureProvider.IsTableColumn( objParent.TableName , this.Config.ParentField ) )
                {

                    if ( DataManager.Screen.ScreenStatus==ABCScreenStatus.New )
                        ( DataObject as IABCList ).SetAllIsNew();

                    object objOldValue=null;
                    foreach ( object obj in ( DataObject as IABCList ) )
                    {
                        if ( obj is BusinessObject )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , this.Config.ChildField );
                            ABCBusinessEntities.ABCDynamicInvoker.SetValue( obj as BusinessObject , this.Config.ChildField , iParentID );
                            if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=iParentID )
                                this.IsModified=true;
                            if ( BusinessObjectHelper.CopyFKFields( objParent.DataObject as BusinessObject , obj as BusinessObject , true ) )
                                this.IsModified=true;
                        }
                    }
                }
                //  }
                #endregion

                #region Default Properties
                foreach ( object obj in ( DataObject as IABCList ) )
                {
                    if ( obj is BusinessObject )
                    {
                        bool isCopied=false;
                        if ( iParentID!=Guid.Empty&&objParent!=null )
                        {
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colDocumentDate , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colVoucher , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colVoucherDate , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colJournalDate , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colApprovalStatus , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colApprovedDate , false ) )
                                isCopied=true;
                            if ( BusinessObjectHelper.CopyField( objParent.Binding.Current as BusinessObject , obj as BusinessObject , ABCCommon.ABCConstString.colLockStatus , false ) )
                                isCopied=true;
                        }
                        if ( BusinessObjectHelper.SetAutoValue( obj as BusinessObject ) )
                            isCopied=true;
                        if ( CurrencyProvider.GenerateCurrencyValue( obj as BusinessObject ) )
                            isCopied=true;

                        object objOldValue=null;
                        if ( DataStructureProvider.IsTableColumn( this.Config.TableName , this.Config.DefaultField ) )
                        {
                            objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , this.Config.DefaultField );
                            ABCDynamicInvoker.SetValue( obj as BusinessObject , this.Config.DefaultField , this.Config.DefaultValue );
                            if ( objOldValue.ToString()!=this.Config.DefaultValue )
                                isCopied=true;
                        }

                        #region Set Default User Info
                        if ( this.Config.CurrentUserOnly )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "ADUsers" );
                            if ( !String.IsNullOrWhiteSpace( strFK ) )
                            {
                                objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , strFK );
                                ABCDynamicInvoker.SetValue( obj as BusinessObject , strFK , ABCUserProvider.CurrentUser.ADUserID );
                                if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentUser.ADUserID )
                                    isCopied=true;
                            }
                        }
                        if ( this.Config.CurrentEmployeeOnly )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "HREmployees" );
                            if ( !String.IsNullOrWhiteSpace( strFK ) )
                            {
                                objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , strFK );
                                ABCDynamicInvoker.SetValue( obj as BusinessObject , strFK , ABCUserProvider.CurrentEmployee.HREmployeeID );
                                if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentEmployee.HREmployeeID )
                                    isCopied=true;
                            }
                        }
                        if ( this.Config.CurrentUserGroupOnly )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "ADUserGroups" );
                            if ( !String.IsNullOrWhiteSpace( strFK ) )
                            {
                                objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , strFK );
                                ABCDynamicInvoker.SetValue( obj as BusinessObject , strFK , ABCUserProvider.CurrentUserGroup.ADUserGroupID );
                                if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentUserGroup.ADUserGroupID )
                                    isCopied=true;
                            }
                        }
                        if ( this.Config.CurrentCompanyUnitOnly )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( this.Config.TableName , "GECompanyUnits" );
                            if ( !String.IsNullOrWhiteSpace( strFK ) )
                            {
                                objOldValue=ABCDynamicInvoker.GetValue( obj as BusinessObject , strFK );
                                ABCDynamicInvoker.SetValue( obj as BusinessObject , strFK , ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID );
                                if ( ABCHelper.DataConverter.ConvertToGuid( objOldValue )!=ABCUserProvider.CurrentCompanyUnit.GECompanyUnitID )
                                    isCopied=true;
                            }
                        }
                        #endregion


                        if ( isCopied )
                            ( DataObject as IABCList ).MarkAsChangedItem( obj as BusinessObject );

                    }
                }
                #endregion

                if ( this.IsModified )
                    ( DataObject as IABCList ).ActionSave( isShowWaitingDlg );

                #endregion

                isSaved=true;
                IsModified=false;
            }


            #endregion

            //   if ( isSaved&&isSaveChildren )
            if ( isSaveChildren )
            {
                SaveChildren( isSaveChildren , isShowWaitingDlg );
            }

            if ( isSaved )
                RefreshUI();
        }

        public void SaveChildren ( bool isSaveChildren , bool isShowWaitingDlg )
        {
            if ( DataManager!=null&&DataManager.DataObjectsList!=null )
            {
                foreach ( ABCBindingConfig childInfo in Config.Children.Values )
                {
                    //  if ( DataStructureProvider.IsTableColumn( this.TableName , childInfo.ParentField ) )
                    DataManager.DataObjectsList[childInfo.Name].Save( isSaveChildren , isShowWaitingDlg );
                }
            }
        }
        #endregion
    }

}
