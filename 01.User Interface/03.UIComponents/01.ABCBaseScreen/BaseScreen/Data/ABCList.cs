using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Collections.Generic;

using ABCControls;
using ABCBusinessEntities;
using ABCProvider;
using ABCCommon;
namespace ABCScreen.Data
{
    public delegate void ABCListActionStartHandler ( object sender , ABCStandardEventArg arg );
    public delegate void ABCListActionEndHandler ( object sender , ABCStandardEventArg arg );

    public interface IABCList : IList
    {
        IABCGridControl GridControl { get; set; }
        void InitActions ( IABCGridControl grid );

        event ABCListActionStartHandler ActionStartEvent;
        event ABCListActionEndHandler ActionEndEvent;
        void ActionRefresh ( );
        void ActionSave ( bool isShowWaitingDlg );
        void ActionRemove ( );

        void DeleteAllItems ( );
        void DeleteItem ( BusinessObject objT );

        void Select ( int iIndex );
        BusinessObject SelectedItem { get; }

        void SetAllIsNew ( );
        void MarkAsChangedItem ( BusinessObject objT );
        BusinessObject GetItemByID ( Guid iID );
    }

    public class ABCList<T> : BindingList<T> , IABCList
       where T : BusinessObject , new()
    {
        public String TableName=String.Empty;
        public BusinessObjectController Controller;

        public ABCDataObject Binding;
        public IABCGridControl GridControl { get; set; }

        List<T> BackupInnerList=new List<T>();


        public ABCList ( )
        {
        }
        public ABCList ( ABCDataObject bindInfo )
        {
            Binding=bindInfo;
            TableName=bindInfo.Config.TableName;
            Controller=BusinessControllerFactory.GetBusinessController( TableName );
        }

        #region Binding - BackupInnerList

        public void SetBinding ( ABCDataObject bindInfo )
        {
            Binding=bindInfo;
            TableName=bindInfo.Config.TableName;
            Controller=BusinessControllerFactory.GetBusinessController( TableName );

            this.ListChanged+=new ListChangedEventHandler( ABCList_ListChanged );

        }

        #region Modify detection
        //   BindingList<T> RemovedItems=new BindingList<T>();
        public Dictionary<Guid , T> RemovedItems=new System.Collections.Generic.Dictionary<Guid , T>();
        public List<int> NewItems=new List<int>();
        public List<Guid> ChangedItems=new List<Guid>();

        protected override void RemoveItem ( int index )
        {

            if ( this.RaiseListChangedEvents )
            {
                if ( NewItems.Contains( index ) )
                    NewItems.Remove( index );

                for ( int i=0; i<NewItems.Count; i++ )
                {
                    if ( NewItems[i]>index )
                        NewItems[i]-=1;
                }

                Guid iID=BusinessObjectHelper.GetIDValue( this[index] );
                if ( iID!=Guid.Empty )
                {
                    if ( ChangedItems.Contains( iID ) )
                        ChangedItems.Remove( iID );

                    if ( RemovedItems.ContainsKey( iID )==false )
                        RemovedItems.Add( iID , this[index] );
                }
            }

            base.RemoveItem( index );
            if ( this.Binding!=null )
                this.Binding.IsModified=true;
        }

        private void DoClear ( )
        {
            if ( this.Binding.InRelationWithMainObject()&&this.Binding.Config.DisplayOnly==false&&
                ( this.Binding.DataManager.Screen.ScreenStatus==ABCScreenStatus.New||this.Binding.DataManager.Screen.ScreenStatus==ABCScreenStatus.Edit ) )
            {
                NewItems.Clear();
                ChangedItems.Clear();

                foreach ( T obj in this )
                {
                    Guid iID=BusinessObjectHelper.GetIDValue( obj );
                    if ( iID!=Guid.Empty&&RemovedItems.ContainsKey( iID )==false )
                        RemovedItems.Add( iID , obj );
                }
            }
            else
            {
                BackupInnerList.Clear();
                ClearDetections();
            }
            this.Clear();
            if ( this.Binding!=null )
                this.Binding.IsModified=false;
        }

        void ABCList_ListChanged ( object sender , ListChangedEventArgs e )
        {
            if ( e.ListChangedType==ListChangedType.ItemAdded )
                if ( NewItems.Contains( e.NewIndex )==false )
                {
                    NewItems.Add( e.NewIndex );
                    if ( this.Binding!=null )
                        this.Binding.IsModified=true;
                }

            if ( e.ListChangedType==ListChangedType.ItemChanged )
                if ( NewItems.Contains( e.NewIndex )==false )
                {
                    if ( this[e.NewIndex]!=null )
                    {
                        Guid iID=BusinessObjectHelper.GetIDValue( this[e.NewIndex] );
                        if ( iID!=Guid.Empty )
                        {
                            if ( ChangedItems.Contains( iID )==false )
                            {
                                ChangedItems.Add( iID );
                                if ( this.Binding!=null )
                                    this.Binding.IsModified=true;
                            }
                        }
                    }
                }

            //if ( e.ListChangedType==ListChangedType.ItemChanged )
            //{
            //    int iID=(int)ABCDynamicInvoker.GetValue( this[e.NewIndex] , ABCTable.StructureProvider.GetPrimaryKeyColumn( this.TableName ) );
            //    if ( iID>0 )
            //        Controller.UpdateObject( this[e.NewIndex] );
            //    else
            //        Controller.CreateObject( this[e.NewIndex] );
            //}
        }

        void ClearDetections ( )
        {
            RemovedItems.Clear();
            NewItems.Clear();
            ChangedItems.Clear();
            if ( this.Binding!=null )
                this.Binding.IsModified=false;
        }
        public void SetAllIsNew ( )
        {
            ClearDetections();

            for ( int iIndex=0; iIndex<this.Count; iIndex++ )
            {
                T objT=this[iIndex];
                BusinessObjectHelper.SetIDValue( objT as BusinessObject , Guid.Empty );
                NewItems.Add( iIndex );
                if ( this.Binding!=null )
                    this.Binding.IsModified=true;
            }

        }
        #endregion

        public BusinessObject GetItemByID ( Guid iID )
        {
            for ( int iIndex=0; iIndex<this.Count; iIndex++ )
            {
                T objT=this[iIndex];
                Guid iIDValue=BusinessObjectHelper.GetIDValue( objT as BusinessObject );
                if ( iIDValue==iID )
                    return objT as BusinessObject;
            }

            return null;
        }

        public void MarkAsChangedItem ( BusinessObject objT )
        {
            Guid iID=BusinessObjectHelper.GetIDValue( objT );
            if ( iID!=Guid.Empty )
            {
                if ( ChangedItems.Contains( iID )==false )
                {
                    ChangedItems.Add( iID );
                    if ( this.Binding!=null )
                        this.Binding.IsModified=true;
                }
            }
        }
        #endregion

        #region BarItems

        public event ABCListActionStartHandler ActionStartEvent;
        public event ABCListActionEndHandler ActionEndEvent;
        private void OnActionStart ( object sender , ABCStandardEventArg arg )
        {
            if ( ActionStartEvent!=null )
                ActionStartEvent( sender , arg );
        }
        private void OnActionEnd ( object sender , ABCStandardEventArg arg )
        {
            if ( ActionEndEvent!=null )
                ActionEndEvent( sender , arg );
        }

        public void InitActions ( IABCGridControl grid )
        {
            GridControl=grid;
            if ( GridControl!=null )
            {
                if ( this.Binding.DataManager.MainObject!=null||this.Binding.Config.AutoSave )
                {
                    if ( GridControl is ABCGridControl )
                        ( GridControl as ABCGridControl ).ShowSaveButton=false;
                    if ( GridControl is ABCGridBandedControl )
                        ( GridControl as ABCGridBandedControl ).ShowSaveButton=false;
                }
                GridControl.BarItemClick+=new ABCDefineEvents.ABCBarItemClickEventHandler( GridControl_BarItemClick );

                #region Permissions
                if ( ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowView )==false )
                {
                    if ( GridControl is ABCGridControl )
                    {
                        ( GridControl as ABCGridControl ).ShowSaveButton=false;
                        ( GridControl as ABCGridControl ).ShowDeleteButton=false;
                        ( GridControl as ABCGridControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    }
                    if ( GridControl is ABCGridBandedControl )
                    {
                        ( GridControl as ABCGridBandedControl ).ShowSaveButton=false;
                        ( GridControl as ABCGridBandedControl ).ShowDeleteButton=false;
                        ( GridControl as ABCGridBandedControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    }
                }
                if ( this.Binding.Config.DisplayOnly||ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowNew )==false )
                {
                    if ( GridControl is ABCGridControl )
                        ( GridControl as ABCGridControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    if ( GridControl is ABCGridBandedControl )
                        ( GridControl as ABCGridBandedControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                }
                if ( this.Binding.Config.DisplayOnly||ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowDelete )==false )
                {
                    if ( GridControl is ABCGridControl )
                        ( GridControl as ABCGridControl ).ShowDeleteButton=false;
                    if ( GridControl is ABCGridBandedControl )
                        ( GridControl as ABCGridBandedControl ).ShowDeleteButton=false;
                }
                if ( this.Binding.Config.DisplayOnly||ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowEdit )==false )
                {
                    if ( GridControl is ABCGridControl )
                    {
                        ( GridControl as ABCGridControl ).DefaultView.OptionsBehavior.Editable=false;
                        ( GridControl as ABCGridControl ).DefaultView.OptionsSelection.EnableAppearanceFocusedCell=false;
                        ( GridControl as ABCGridControl ).DefaultView.OptionsSelection.EnableAppearanceFocusedRow=true;
                        ( GridControl as ABCGridControl ).DefaultView.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                    }
                    if ( GridControl is ABCGridBandedControl )
                    {
                        ( GridControl as ABCGridBandedControl ).BandedView.OptionsBehavior.Editable=false;
                        ( GridControl as ABCGridBandedControl ).BandedView.OptionsSelection.EnableAppearanceFocusedCell=false;
                        ( GridControl as ABCGridBandedControl ).BandedView.OptionsSelection.EnableAppearanceFocusedRow=true;
                        ( GridControl as ABCGridBandedControl ).BandedView.FocusRectStyle=DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                    }
                }

                if ( this.Binding.Config.DisplayOnly )
                {
                    if ( GridControl is ABCGridControl )
                    {
                        ( GridControl as ABCGridControl ).ShowSaveButton=false;
                        ( GridControl as ABCGridControl ).ShowDeleteButton=false;
                        ( GridControl as ABCGridControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    }
                    if ( GridControl is ABCGridBandedControl )
                    {
                        ( GridControl as ABCGridBandedControl ).ShowSaveButton=false;
                        ( GridControl as ABCGridBandedControl ).ShowDeleteButton=false;
                        ( GridControl as ABCGridBandedControl ).NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    }
                }
                #endregion

            }
        }

        void GridControl_BarItemClick ( object sender , string strTag )
        {
            ABCStandardEventArg arg=new ABCStandardEventArg( strTag );
            OnActionStart( sender , arg );
            if ( arg.Cancel )
                return;

            switch ( strTag )
            {
                case "Save":
                    DialogResult dlgResult=ABCHelper.ABCMessageBox.Show( "Bạn có muốn lưu dữ liệu ? " , "Thông báo" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
                    if ( dlgResult==DialogResult.Yes )
                    {
                        if ( this.Binding!=null )
                            this.Binding.Save( true , true );
                        else
                            ActionSave( true );
                    }
                    break;
                case "Refresh":
                    ActionRefresh();
                    break;
                case "Delete":
                    ActionRemove();

                    break;
            }

            OnActionEnd( sender , new ABCStandardEventArg( strTag ) );
        }

        #endregion

        #region Actions

        public delegate void ABCListSavingHandler ( object sender , ref List<T> lstDeletings , ref List<T> lstUpdatings , ref List<T> lstCreatings , ABCStandardEventArg arg );
        public delegate void ABCListnSavedHandler ( object sender , ref List<T> lstDeletings , ref List<T> lstUpdatings , ref List<T> lstCreatings , ABCStandardEventArg arg );

        public event ABCListSavingHandler SavingEvent;
        public event ABCListnSavedHandler SavedEvent;
        private void OnSaving ( object sender , ref List<T> lstDeletings , ref List<T> lstUpdatings , ref List<T> lstCreatings , ABCStandardEventArg arg )
        {
            if ( SavingEvent!=null )
                SavingEvent( sender , ref lstDeletings , ref lstUpdatings , ref lstCreatings , arg );
        }
        private void OnSaved ( object sender , ref List<T> lstDeletings , ref List<T> lstUpdatings , ref List<T> lstCreatings , ABCStandardEventArg arg )
        {
            if ( SavedEvent!=null )
                SavedEvent( sender , ref lstDeletings , ref lstUpdatings , ref lstCreatings , arg );
        }


        public void ActionRemove ( )
        {
            if ( ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowEdit )==false )
                return;
            if ( ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowDelete )==false )
                return;

            if ( GridControl.GridDefaultView!=null&&GridControl.GridDefaultView.SelectedRowsCount>0 )
            {
                if ( ABCHelper.ABCMessageBox.Show( "Bạn thực sự muốn xóa dữ liệu?" , "Thông báo" , MessageBoxButtons.YesNo )!=DialogResult.Yes )
                    return;

                GridControl.GridDefaultView.DeleteSelectedRows();

                if ( this.Binding!=null&&this.Binding.Config.AutoSave&&this.Binding.Config.DisplayOnly==false&this.Binding.InRelationWithMainObject()==false )
                    this.Binding.Save( false , true );
            }
        }

        public void ActionSave ( bool isShowWaitingDlg )
        {
            if ( ChangedItems.Count>0&&ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowEdit )==false )
                return;
            if ( RemovedItems.Count>0&&ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowDelete )==false )
                return;
            if ( NewItems.Count>0&&ABCScreenManager.Instance.CheckTablePermission( this.TableName , TablePermission.AllowNew )==false )
                return;

            if ( isShowWaitingDlg )
                ABCHelper.ABCWaitingDialog.Show( "" , "Đang lưu ...." );

            List<T> lstDeletings=new List<T>();
            List<T> lstUpdatings=new List<T>();
            List<T> lstCreatings=new List<T>();

            #region Init
            foreach ( T objT in RemovedItems.Values )
                lstDeletings.Add( objT );

            foreach ( Guid iID in ChangedItems )
            {
                BusinessObject objT=GetItemByID( iID );
                if ( objT!=null )
                {
                    BusinessObjectHelper.SetAutoValue( objT );
                    lstUpdatings.Add( (T)objT );
                }
            }

            foreach ( int iIndex in NewItems )
            {
                BusinessObjectHelper.SetDefaultValue( this[iIndex] );
                BusinessObjectHelper.SetAutoValue( this[iIndex] );
                Guid iID=BusinessObjectHelper.GetIDValue( this[iIndex] );
                if ( iID==Guid.Empty )
                    lstCreatings.Add( this[iIndex] );
                else
                    lstUpdatings.Add( this[iIndex] );
            }

            #endregion

            ABCStandardEventArg arg=new ABCStandardEventArg();
            OnSaving( this , ref lstDeletings , ref lstUpdatings , ref lstCreatings , arg );
            if ( arg.Cancel )
                return;

            foreach ( T objT in lstDeletings )
                DeleteItem( objT );

            foreach ( T objT in lstUpdatings )
                Controller.UpdateObject( objT );

            foreach ( T objT in lstCreatings )
                Controller.CreateObject( objT );

            ClearDetections();

            DataCachingProvider.RefreshLookupTable( this.TableName );

            OnSaved( this , ref lstDeletings , ref lstUpdatings , ref lstCreatings , new ABCStandardEventArg() );

            if ( isShowWaitingDlg )
                ABCHelper.ABCWaitingDialog.Close();
        }

        public void ActionRefresh ( )
        {
            ABCHelper.ABCWaitingDialog.Show( "" , "Đang lấy dữ liệu ...." );

            Binding.Refresh();

            ClearDetections();

            //  Binding.ReloadObject();

            //List<T> lstTemp=new System.Collections.Generic.List<T>( BackupInnerList );
            //Invalidate( lstTemp );
            this.ResetBindings();

            ABCHelper.ABCWaitingDialog.Close();
        }
        #endregion

        public void DeleteAllItems ( )
        {
            foreach ( T objT in this )
                DeleteItem( objT );
        }
        public void DeleteItem ( BusinessObject objT )
        {
            if ( objT is T==false )
                return;

            foreach ( String strChildName in Binding.Config.Children.Keys )
            {
                if ( Binding.DataManager.DataObjectsList[strChildName].Config.DisplayOnly )
                    continue;

                String strFK=Binding.DataManager.DataObjectsList[strChildName].Config.ChildField;
                String strFKTableName=Binding.DataManager.DataObjectsList[strChildName].TableName;
                Guid iID=ABCHelper.DataConverter.ConvertToGuid( ABCBusinessEntities.ABCDynamicInvoker.GetValue( objT , DataStructureProvider.GetPrimaryKeyColumn( this.TableName ) ) );
                if ( iID!=Guid.Empty )
                {
                    BusinessObjectController ctrller=BusinessControllerFactory.GetBusinessController( strFKTableName );
                    if ( ctrller!=null )
                        ctrller.DeleteObjectsByFK( strFK , iID );
                }
            }
            Controller.DeleteObject( objT );
        }

        #region Invalidate Data

        public void Append ( DataSet ds )
        {
            if ( ds==null||ds.Tables.Count<=0 )
                return;

            foreach ( DataRow row in ds.Tables[0].Rows )
            {
                BusinessObject objItemInfo=(BusinessObject)Controller.GetObjectFromDataRow( row );
                if ( objItemInfo!=null )
                {
                    this.Add( (T)objItemInfo );
                    BackupInnerList.Add( (T)objItemInfo.Clone() );
                }
            }

            this.ResetBindings();
        }
        public void Append ( IList<T> lst )
        {
            foreach ( T obj in lst )
            {
                T objT=(T)obj.Clone();
                this.Add( objT );
                BackupInnerList.Add( (T)objT.Clone() );
            }
        }

        public void LoadData ( DataSet ds )
        {
            this.Clear();
            ClearDetections();
            DoInvalidate( ds );
        }
        public void LoadData ( IList<T> lst )
        {
            this.Clear();
            ClearDetections();
            DoInvalidate( lst );
        }
        public void LoadData ( List<BusinessObject> lst )
        {

            Converter<BusinessObject , T> converter=new Converter<BusinessObject , T>(
                                                                delegate( BusinessObject value )
                                                                { return (T)value; } );

            LoadData( lst.ConvertAll( converter ) );
        }

        public void Invalidate ( DataSet ds )
        {
            DoClear();
            DoInvalidate( ds );
        }
        public void Invalidate ( IList<T> lst )
        {
            DoClear();
            DoInvalidate( lst );
        }
        public void Invalidate ( List<BusinessObject> lst )
        {

            Converter<BusinessObject , T> converter=new Converter<BusinessObject , T>(
                                                                delegate( BusinessObject value )
                                                                { return (T)value; } );

            Invalidate( lst.ConvertAll( converter ) );

        }

        private void DoInvalidate ( DataSet ds )
        {
            bool raiseListChangedEvents=this.RaiseListChangedEvents;
            this.RaiseListChangedEvents=false;
            Append( ds );
            this.RaiseListChangedEvents=raiseListChangedEvents;

            GC.Collect();

            this.ResetBindings();
        }
        private void DoInvalidate ( IList<T> lst )
        {
            bool raiseListChangedEvents=this.RaiseListChangedEvents;
            this.RaiseListChangedEvents=false;
            Append( lst );
            this.RaiseListChangedEvents=raiseListChangedEvents;
            GC.Collect();

            this.ResetBindings();
        }

        #endregion

        public BusinessObject SelectedItem
        {
            get
            {
                return (BusinessObject)this.Binding.Binding.Current;
            }
        }
        public void Select ( int iIndex )
        {
            this.Binding.Binding.Position=iIndex;
            //  this.ResetBindings();
        }

        public Boolean ContainID ( Guid iID )
        {
            foreach ( T objT in this )
            {
                Guid objID=BusinessObjectHelper.GetIDValue( objT );
                if ( objID!=Guid.Empty&&objID==iID )
                    return true;
            }

            return false;
        }
        public Boolean ContainProperty ( String strProName , object objValue )
        {
            TypeConverter converter=TypeDescriptor.GetConverter( objValue.GetType() );

            foreach ( T objT in this )
            {
                object obj=ABCBusinessEntities.ABCDynamicInvoker.GetValue( objT , strProName );
                if ( obj!=null&&obj.ToString()==objValue.ToString() )
                    return true;
            }

            return false;
        }
    }
}
