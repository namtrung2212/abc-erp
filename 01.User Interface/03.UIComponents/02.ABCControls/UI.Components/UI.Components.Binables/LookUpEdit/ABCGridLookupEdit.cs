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

using ABCProvider;
using ABCCommon;


namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.GridLookUpEdit ) )]
    [Designer( typeof( ABCGridLookUpEditDesigner ) )]
    public class ABCGridLookUpEdit : DevExpress.XtraEditors.GridLookUpEdit , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [Category( "ABC.BindingValue" )]
        //  [ReadOnly( true )]
        public String TableName { get; set; }

        [Category( "ABC.BindingValue" )]
        //  [ReadOnly( true )]
        public String LookupTableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "EditValue";
            }
        }
        #endregion

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }
        [Category( "Format" )]
        public String FieldGroup { get; set; }

        [Category( "Format" )]
        public String EditMask
        {
            get
            {
                return this.Properties.Mask.EditMask;
            }
            set
            {
                this.Properties.Mask.EditMask=value;
            }
        }
        [Category( "Format" )]
        public DevExpress.XtraEditors.Mask.MaskType MaskType
        {
            get
            {
                return this.Properties.Mask.MaskType;
            }
            set
            {
                this.Properties.Mask.MaskType=value;
            }
        }

        [Category( "Format" )]
        public Boolean ReadOnly
        {
            get
            {
                return this.Properties.ReadOnly;
            }
            set
            {
                this.Properties.ReadOnly=value;
            }
        }

        bool isVisible=true;
        [Category( "External" )]
        public Boolean IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible=value;
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }
        [Category( "Format" )]
        public String DummyText
        {
            get
            {
                return this.Properties.NullValuePrompt;
            }
            set
            {
                this.Properties.NullValuePrompt=value;
            }
        }

        [Category( "Data" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DisplayMember { get; set; }

        [Category( "Data" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String ValueMember { get; set; }

        [Category( "Data" )]
        [Editor( typeof( ABCControls.TypeEditor.FilterConditionEditor ) , typeof( UITypeEditor ) )]
        public String FilterStringEx { get; set; }

        #region Field Parent Relation
        [Category( "Parent" )]
        public String ParentCtrlName { get; set; }
        [Category( "Parent" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String ChildField { get; set; }

        public IABCBindableControl ParentControl;
        #endregion

        #endregion

        #region Register Repository
        public override string EditorTypeName
        {
            get { return "ABCGridLookUpEdit"; }
        }
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public new ABCRepositoryGridLookupEdit Properties
        {
            get { return (ABCRepositoryGridLookupEdit)base.Properties; }
        }
        static ABCGridLookUpEdit ( )
        {
            ABCRepositoryGridLookupEdit.RegisterCustomEdit();
        }
    
        #endregion

        public ABCGridLookUpEdit ( )
        {
        }

        #region Design Time

        public void Initialize ( ABCView view )
        {

            OwnerView=view;

            InitControl();

            String strFieldName=this.DataMember.Split( ':' )[0];
            if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName ) )
            {
                this.LookupTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , strFieldName );

                this.ValueMember=DataStructureProvider.GetPrimaryKeyColumn( this.LookupTableName );

                this.DisplayMember=DataStructureProvider.GetDisplayColumn( this.LookupTableName );

            }

            this.DummyText="...";
        }
        #endregion

        #region IABCControl
        public void InitControl ( )
        {

            if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                InitDesignTime();
            else
                InitRunTime();

            InitBothMode();

        }

        public void InitBothMode ( )
        {
            this.Properties.NullValuePrompt=DummyText;
            this.Properties.AllowNullInput=DevExpress.Utils.DefaultBoolean.True;

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;



            if ( !String.IsNullOrWhiteSpace( EditMask.ToString() )&&!String.IsNullOrWhiteSpace( MaskType.ToString() ) )
                this.Properties.Mask.UseMaskAsDisplayFormat=true;

            this.Properties.NullValuePromptShowForEmptyValue=true;

            if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
                this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;

            this.Properties.Buttons[1].Visible=false;
        }
        public void InitRunTime ( )
        {
            if ( OwnerView==null )
                OwnerView=ABCPresentHelper.FindParentView( this );

            InitFilterString();

            InitGridView();

            InitRuntimeEvents();

            this.Properties.Appearance.ForeColor=Color.Black;
            this.Properties.Appearance.Options.UseForeColor=true;
        }

        public void InitDesignTime ( )
        {
        }

        #endregion

        #region Runtime

        public void InitGridView ( )
        {

            String strFieldName=this.DataMember.Split( ':' )[0];
            if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName )||DataStructureProvider.IsPrimaryKey( this.TableName , strFieldName ) )
            {
                if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName ) )
                    this.LookupTableName=DataStructureProvider.GetTableNameOfForeignKey( this.TableName , strFieldName );
                else
                    this.LookupTableName=this.TableName;
                ABCControls.UICaching.InitDefaultRepositoryGridLookUpEdit( this.LookupTableName , this.Properties , true );


                if ( String.IsNullOrWhiteSpace( this.DisplayMember )==false&&DataStructureProvider.IsTableColumn( this.LookupTableName , this.DisplayMember ) )
                    this.Properties.DisplayMember=this.DisplayMember;

                if ( String.IsNullOrWhiteSpace( this.ValueMember )==false&&DataStructureProvider.IsTableColumn( this.LookupTableName , this.ValueMember ) )
                    this.Properties.ValueMember=this.ValueMember;

                #region DataSource
                if ( String.IsNullOrWhiteSpace( DefaultFilterString ) )
                    this.Properties.DataSource=DataCachingProvider.TryToGetDataView( this.LookupTableName , false );
                else
                {
                    DataView newView=DataCachingProvider.TryToGetDataView( this.LookupTableName , true );
                    newView.RowFilter=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( DefaultFilterString ) );
                    this.Properties.DataSource=newView;
                }

                #endregion

                if ( this.Enabled )
                    this.Enabled= ABCScreen.ABCScreenHelper.Instance.CheckTablePermission( this.LookupTableName , TablePermission.AllowView );

            }

        }
        public void InitRuntimeEvents ( )
        {
            this.QueryPopUp+=new CancelEventHandler( ABCGridLookUpEdit_QueryPopUp );
        }

        void ABCGridLookUpEdit_QueryPopUp ( object sender , CancelEventArgs e )
        {
            if ( String.IsNullOrWhiteSpace( this.ParentCtrlName )==false&&String.IsNullOrWhiteSpace( this.ChildField )==false )
            {
                if ( ParentControl==null )
                {
                    Form frm=this.FindForm();
                    Control[] parentCtrls=frm.Controls.Find( this.ParentCtrlName , true );
                    if ( parentCtrls!=null&&parentCtrls.Length>0 )
                    {
                        if ( parentCtrls[0] is IABCBindableControl&&parentCtrls[0] is ABCBindingBaseEdit ==false)
                            ParentControl=parentCtrls[0] as IABCBindableControl;
                        else if ( parentCtrls[0] is ABCBindingBaseEdit&&( parentCtrls[0] as ABCBindingBaseEdit ).EditControl is IABCBindableControl )
                            ParentControl=( parentCtrls[0] as ABCBindingBaseEdit ).EditControl as IABCBindableControl;

                    }
                }

                if ( ParentControl!=null&&ParentControl is IABCBindableControl&&DataStructureProvider.IsTableColumn( LookupTableName , ChildField ) )
                {
                    object objValue=ParentControl.GetType().GetProperty( ( ParentControl as IABCBindableControl ).BindingProperty ).GetValue( ParentControl , null );
                    if ( objValue!=null )
                    {
                        DataView newView=DataCachingProvider.TryToGetDataView( this.LookupTableName , true );
                        newView.RowFilter=DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere( DevExpress.Data.Filtering.CriteriaOperator.Parse( DefaultFilterString ) );

                        String strFilter=String.Empty;
                        if ( objValue.GetType()==typeof( int )||objValue.GetType()==typeof( double )
                            ||objValue.GetType()==typeof( Nullable<int> )||objValue.GetType()==typeof( Nullable<double> )
                            ||objValue.GetType()==typeof( Boolean )||objValue.GetType()==typeof( Nullable<Boolean> ) )
                            strFilter=String.Format( @" {0} = {1} " , ChildField , objValue );
                        if ( objValue.GetType()==typeof( String ) )
                            strFilter=String.Format( @" {0} = '{1}' " , ChildField , objValue.ToString() );
                        if ( String.IsNullOrWhiteSpace( newView.RowFilter )==false )
                            strFilter=" AND "+strFilter;
                        newView.RowFilter+=strFilter;

                        this.Properties.DataSource=newView;
                    }
                }
            }
        }

        protected override Size CalcPopupFormSize ( )
        {
            try
            {
                Size size=base.CalcPopupFormSize();
                size.Height=250;
                return size;
            }
            catch ( Exception ex )
            {
            }
            return new Size( 500 , 350 );
        }

        #endregion

        #region Utils
        public String DefaultFilterString=String.Empty;
        public void InitFilterString ( )
        {
            DefaultFilterString=String.Empty;
            if (OwnerView!=null&& String.IsNullOrWhiteSpace( this.DataSource )==false&&OwnerView.DataConfig.BindingList.ContainsKey( this.DataSource ) )
            {
                String strFieldName=this.DataMember.Split( ':' )[0];
                if ( DataStructureProvider.IsForeignKey( this.TableName , strFieldName ) )
                {
                    foreach ( ABCScreen.ABCBindingConfig.FieldFilterConfig config in OwnerView.DataConfig.BindingList[this.DataSource].FieldFilterConditions )
                    {
                        if ( config.Field==strFieldName )
                        {
                            DefaultFilterString=config.FilterString;
                            break;
                        }
                    }

                    if ( String.IsNullOrWhiteSpace( DefaultFilterString )==false )
                        DefaultFilterString=String.Format( "({0}) And ({1})" , DefaultFilterString , DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strFieldName].FilterString );
                    else
                        DefaultFilterString=DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strFieldName].FilterString;
                }
            }
            if ( String.IsNullOrWhiteSpace( FilterStringEx )==false )
            {
                if ( String.IsNullOrWhiteSpace( DefaultFilterString )==false )
                    DefaultFilterString=String.Format( "({0}) And ({1})" , DefaultFilterString , FilterStringEx );
                else
                    DefaultFilterString=FilterStringEx;
            }
        }

        #endregion

        #region PopupBaseForm - Add Clear Button
        class ABCPopupGridLookUpEditForm : PopupGridLookUpEditForm
        {
            #region Register
            // constructor
            public ABCPopupGridLookUpEditForm ( ABCGridLookUpEdit ownerEdit )
                : base( ownerEdit )
            {
                fShowOkButton=true;
                OkButton.Image=ABCImageList.GetImage16x16( "Clear" );
                OkButton.Text="Clear";
                //         OkButton.ButtonStyle=DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
                //        OkButton.Left=CloseButton.Left+10;
            }

            new ABCRepositoryGridLookupEdit Properties
            {
                get
                {
                    ABCGridLookUpEdit edit=OwnerEdit as ABCGridLookUpEdit;
                    if ( edit==null )
                        return null;
                    return edit.Properties;
                }
            }

            #endregion

            protected override void OnOkButtonClick ( )
            {
                base.OnOkButtonClick();
                OwnerEdit.EditValue=null;
                OwnerEdit.ClosePopup();
            }

        }
        protected override PopupBaseForm CreatePopupForm ( )
        {
            return new ABCPopupGridLookUpEditForm( this );
        }
        #endregion
    }

    public class ABCGridLookUpEditDesigner : ControlDesigner
    {
        public ABCGridLookUpEditDesigner ( )
        {
        }
    }
}
       