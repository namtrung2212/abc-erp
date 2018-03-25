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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Accessibility;
using System.ComponentModel;
using DevExpress.XtraEditors.ListControls;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Utils;

using ABCProvider;

namespace ABCControls
{

    [UserRepositoryItem( "RegisterCustomEdit" )]
    public partial class ABCRepositoryGridLookupEdit : DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit 
    {

        private ABCGridView _OwnerGridView;
        public ABCGridView OwnerGridView
        {
            get { return _OwnerGridView; }
            set
            {
                if ( _OwnerGridView!=value )
                {
                    _OwnerGridView=value;
                    OnPropertiesChanged();
                }
            }
        }
     
        static ABCRepositoryGridLookupEdit ( )
        {
            RegisterCustomEdit();
        }

        #region Register
    
        #region Register
        public const string CustomEditName="ABCGridLookUpEdit";

        public override string EditorTypeName
        {
            get { return CustomEditName; }
        }

        public FormatInfo ABCActiveFormat=null;
        protected override DevExpress.Utils.FormatInfo ActiveFormat
        {
            get
            {
                ABCActiveFormat=base.ActiveFormat;
                return base.ActiveFormat;
            }
        }
        public static void RegisterCustomEdit ( )
        {
            EditorRegistrationInfo.Default.Editors.Add(
                            new EditorClassInfo( CustomEditName,
                            typeof( ABCGridLookUpEdit ) , typeof( ABCRepositoryGridLookupEdit ) ,
                            typeof( GridLookUpEditBaseViewInfo ) , new ButtonEditPainter() ,
                            true , null, typeof(PopupEditAccessible) ) );
        }

        public override void Assign ( RepositoryItem item )
        {
            BeginUpdate();
            try
            {
                base.Assign( item );
                ABCRepositoryGridLookupEdit source=item as ABCRepositoryGridLookupEdit;
                if ( source==null )
                    return;
                _OwnerGridView=source.OwnerGridView;
            }
            finally
            {
                EndUpdate();
            }
        }

        protected override DevExpress.XtraGrid.Views.Grid.GridView CreateViewInstance ( )
        {
            switch ( ViewType )
            {
                case GridLookUpViewType.BandedView:
                case GridLookUpViewType.AdvBandedView:
                    return new ABCGridBandedView();
            }
            return new ABCGridView();
        }

        protected override DevExpress.XtraGrid.GridControl CreateGrid ( )
        {
            switch ( ViewType )
            {
                case GridLookUpViewType.BandedView:
                case GridLookUpViewType.AdvBandedView:
                    return new ABCBaseBandedGridControl();
            }
            return new ABCBaseGridControl();
        }

        #endregion

        #endregion

        #region Init

        public ABCRepositoryGridLookupEdit ( )
        {

            InitDefaultProperties();

        }
        public ABCRepositoryGridLookupEdit ( IContainer container )
        {
            container.Add( this );

            InitDefaultProperties();
        }

        private void InitDefaultProperties ( )
        {
            this.AutoHeight=true;
            this.ShowDropDown=DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;
            this.TextEditStyle=DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.PopupFilterMode=DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.NullText=String.Empty;
            this.PopupFormSize=new System.Drawing.Size( 550 , 800 );
            this.QueryPopUp+=new System.ComponentModel.CancelEventHandler( ABCRepositoryGridLookupEdit_QueryPopUp );
            if ( DesignMode==false && this.OwnerEdit==null)
            {
                try
                {
                    DevExpress.XtraEditors.Controls.EditorButton searchBtn=new DevExpress.XtraEditors.Controls.EditorButton();
                    searchBtn.Kind=DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                    searchBtn.Image=ABCControls.ABCImageList.GetImage16x16( "DocLink" );
                    searchBtn.Appearance.Options.UseImage=true;
                    searchBtn.GlyphAlignment=HorzAlignment.Near;
                    searchBtn.Tag="Link";

                    searchBtn.IsLeft=true;
                    searchBtn.EnableImageTransparency=true;
                    this.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( ABCRepositoryGridLookupEdit_ButtonClick );
                    this.Buttons.Add( searchBtn );
                }
                catch ( Exception ex )
                {
                }
            }
        }

        void ABCRepositoryGridLookupEdit_RowCellClick ( object sender , DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e )
        {
           
        }

        void ABCRepositoryGridLookupEdit_QueryPopUp ( object sender , System.ComponentModel.CancelEventArgs e )
        {
            if ( sender is ABCGridLookUpEdit&&this.Grid!=null&&this.Grid.DefaultView!=null )
                ( this.Grid.DefaultView as DevExpress.XtraGrid.Views.Grid.GridView ).ClearColumnsFilter();
          
            this.View.ClearColumnsFilter();

            String strTableName=String.Empty;
            if ( sender is ABCGridLookUpEdit )
            {
                strTableName=( sender as ABCGridLookUpEdit ).LookupTableName;
                if ( String.IsNullOrWhiteSpace( strTableName ) )
                {
                    if ( this.Grid!=null&&this.Grid.DefaultView!=null )
                    {
                        DevExpress.XtraGrid.Columns.GridColumn col=( this.Grid.DefaultView as DevExpress.XtraGrid.Views.Grid.GridView ).FocusedColumn as DevExpress.XtraGrid.Columns.GridColumn;
                        if ( col==null )
                            return;

                        object obj=col.GetType().GetField( "TableName" ).GetValue( col );
                        if ( obj!=null )
                            strTableName=obj.ToString();
                    }
                }
              
                DataCachingProvider.RefreshLookupTable( strTableName );

            }

        }

        void ABCRepositoryGridLookupEdit_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
           if ( e.Button.Tag!=null&&e.Button.Tag.Equals( "Link" ) && sender is ABCGridLookUpEdit)
            {
                if ( ( sender as ABCGridLookUpEdit ).Parent is DevExpress.XtraGrid.GridControl )
                {
                    DevExpress.XtraGrid.GridControl gridCtrl=( sender as ABCGridLookUpEdit ).Parent as DevExpress.XtraGrid.GridControl;
                    if ( gridCtrl!=null&&gridCtrl.DefaultView!=null )
                    {
                        if ( gridCtrl.DefaultView is ABCGridView )
                        {
                            ABCGridView view=gridCtrl.DefaultView as ABCGridView;
                            if ( view!=null )
                                view.RunFocusedLink();
                        }
                        if ( gridCtrl.DefaultView is ABCGridBandedView )
                        {
                            ABCGridBandedView view=gridCtrl.DefaultView as ABCGridBandedView;
                            if ( view!=null )
                                view.RunFocusedLink();
                        
                        }
                    }
                }
                else if ( ( sender as ABCGridLookUpEdit ).Parent is DevExpress.XtraVerticalGrid.VGridControl )
                {
                    DevExpress.XtraVerticalGrid.VGridControl gridCtrl=( sender as ABCGridLookUpEdit ).Parent as DevExpress.XtraVerticalGrid.VGridControl;
                    if ( gridCtrl!=null&&gridCtrl.Parent is ABCGridRowDetail)
                    {
                        ABCGridRowDetail rowDetail=(ABCGridRowDetail)gridCtrl.Parent;
                        rowDetail.RunFocusedLink();
                    }
                }

            }
        }
 
        protected internal virtual bool IsNullValue ( object editValue )
        {
            if ( editValue is int&&Convert.ToInt32( editValue )==0 )
                return true;
            return base.IsNullValue( editValue );
        }
       
        #endregion
     
    }
}
