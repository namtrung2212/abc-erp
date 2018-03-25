using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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


namespace ABCControls
{

    [UserRepositoryItem( "RegisterCustomEdit" )]
    public partial class ABCRepositoryLookUpEdit : DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit 
    {
     
        private DevExpress.XtraGrid.Views.Grid.GridView _OwnerGridView;
        public DevExpress.XtraGrid.Views.Grid.GridView OwnerGridView
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
     
        static ABCRepositoryLookUpEdit ( )
        {
            RegisterCustomEdit();
        }

        #region Register
    
        #region Register
        public const string CustomEditName="ABCLookupEdit";

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
                            new EditorClassInfo( CustomEditName ,
                            typeof( ABCLookUpEdit ) , typeof( ABCRepositoryLookUpEdit ) ,
                            typeof( LookUpEditViewInfo ) , new ButtonEditPainter() ,
                            true , null ) );
        }

        public override void Assign ( RepositoryItem item )
        {
            BeginUpdate();
            try
            {
                base.Assign( item );
                ABCRepositoryLookUpEdit source=item as ABCRepositoryLookUpEdit;
                if ( source==null )
                    return;
                _OwnerGridView=source.OwnerGridView;
            }
            finally
            {
                EndUpdate();
            }
        }
        
        #endregion

        #region DataAdapter - Search Contain
        public ABCLookUpListDataAdapter ABCRefAdapter=null;
        public class ABCLookUpListDataAdapter : LookUpListDataAdapter
        {
            public ABCLookUpListDataAdapter ( ABCRepositoryLookUpEdit item )
                : base( item )
            {
            }

            protected override string CreateFilterExpression ( )
            {
                if ( string.IsNullOrWhiteSpace( FilterPrefix ) )
                {
                    return string.Empty;
                }
                string likeClause="%"+DevExpress.Data.Filtering.Helpers.LikeData.CreateStartsWithPattern( FilterPrefix );
                string lsFilterExp=new BinaryOperator( FilterField , likeClause , BinaryOperatorType.Like ).ToString();
                return lsFilterExp;
            }
        }
        protected override LookUpListDataAdapter CreateDataAdapter ( )
        {
            return new ABCLookUpListDataAdapter( this );
        }
        #endregion

        #endregion

        #region Init

        public ABCRepositoryLookUpEdit ( )
        {

            InitDefaultProperties();

        }
        public ABCRepositoryLookUpEdit ( IContainer container )
        {
            container.Add( this );

            InitDefaultProperties();
        }

        private void InitDefaultProperties ( )
        {
            this.AutoHeight=true;
            this.AutoSearchColumnIndex=0;
            this.ShowDropDown=DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;
            this.TextEditStyle=DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.HotTrackItems=true;
            this.SearchMode=DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            this.CaseSensitiveSearch=false;
            this.NullText="";
            this.BestFitMode=DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            if ( DesignMode==false&&this.OwnerEdit==null )
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
                    this.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( ABCRepositoryLookUpEdit_ButtonClick );
                    this.Buttons.Add( searchBtn );
                }
                catch ( Exception ex )
                {
                }
            }
        }

        void ABCRepositoryLookUpEdit_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            if ( e.Button.Tag!=null&&e.Button.Tag.Equals( "Link" )&&sender is ABCLookUpEdit )
            {
                if ( ( sender as ABCLookUpEdit ).Parent is DevExpress.XtraGrid.GridControl )
                {
                    DevExpress.XtraGrid.GridControl gridCtrl=( sender as ABCLookUpEdit ).Parent as DevExpress.XtraGrid.GridControl;
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
                else if ( ( sender as ABCLookUpEdit ).Parent is DevExpress.XtraVerticalGrid.VGridControl )
                {
                    DevExpress.XtraVerticalGrid.VGridControl gridCtrl=( sender as ABCGridLookUpEdit ).Parent as DevExpress.XtraVerticalGrid.VGridControl;
                    if ( gridCtrl!=null&&gridCtrl.Parent is ABCGridRowDetail )
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
