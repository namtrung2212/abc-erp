using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Drawing.Design;
using System.Reflection;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.ViewInfo;

using ABCProvider;


using ABCCommon;
namespace ABCControls
{

    [UserRepositoryItem( "RegisterCustomEdit" )]
    public class ABCRepositoryTextEdit : RepositoryItemTextEdit
    {
        #region Register
        static ABCRepositoryTextEdit ( ) { RegisterCustomEdit(); }
        public ABCRepositoryTextEdit ( )
        {
        }

        public const string CustomEditName="ABCTextEdit";
        public override string EditorTypeName
        {
            get
            {
                return CustomEditName;
            }
        }
        public static void RegisterCustomEdit ( )
        {
            Image img=null;
            // load image
            EditorRegistrationInfo.Default.Editors.Add(
              new EditorClassInfo( CustomEditName ,
              typeof( ABCTextEdit ) ,
              typeof( ABCRepositoryTextEdit ) ,
              typeof( TextEditViewInfo ) ,
              new TextEditPainter() ,
              true , img ) );
        }
        public override void Assign ( RepositoryItem item )
        {
            BeginUpdate();
            try
            {
                base.Assign( item );
                ABCRepositoryTextEdit source=item as ABCRepositoryTextEdit;
                if ( source==null )
                    return;
            }
            finally
            {
                EndUpdate();
            }
        }
        #endregion

        protected override DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs DoFormatEditValue ( object val )
        {
            object objValue=val;
            if ( objValue!=null&&objValue!=DBNull.Value )
            {
                if ( this.OwnerEdit!=null )
                {
                    ABCTextEdit textEdit=( this.OwnerEdit as ABCTextEdit );

                    String strDataMember=textEdit.DataMember;
                    if ( DataStructureProvider.IsForeignKey( textEdit.TableName , textEdit.DataMember ) )
                        strDataMember=textEdit.DataMember+":"+DataStructureProvider.GetDisplayColumn( textEdit.TableName );

                    if ( textEdit.BindingManager.Position >=0 && textEdit.BindingManager.Current!=null&&textEdit.BindingManager.Current is ABCBusinessEntities.BusinessObject )
                    {
                        if ( strDataMember.Contains( ":" )&&objValue is Guid&&ABCHelper.DataConverter.ConvertToGuid( objValue )!=Guid.Empty )
                        {
                            ABCBusinessEntities.BusinessObject objBinds=textEdit.BindingManager.Current as ABCBusinessEntities.BusinessObject;
                            objValue=DataCachingProvider.GetCachingObjectAccrossTable( objBinds , ABCHelper.DataConverter.ConvertToGuid( objValue ) , strDataMember );
                        }
                    }
                }
            }
            return base.DoFormatEditValue( objValue );
        }
        public override string GetDisplayText ( object editValue )
        {
            if ( editValue!=null&&editValue !=DBNull.Value)
            {
                try
                {
                    if ( this.OwnerEdit!=null )
                    {
                        ABCTextEdit textEdit=( this.OwnerEdit as ABCTextEdit );
                        if ( String.IsNullOrWhiteSpace( textEdit.DataMember )==false&&String.IsNullOrWhiteSpace( textEdit.TableName )==false )
                        {
                            String strDataMember=textEdit.DataMember;
                            if ( DataStructureProvider.IsForeignKey( textEdit.TableName , textEdit.DataMember ) )
                                strDataMember=textEdit.DataMember+":"+DataStructureProvider.GetDisplayColumn( textEdit.TableName );

                            if ( String.IsNullOrWhiteSpace( strDataMember )==false )
                            {
                                if ( !String.IsNullOrWhiteSpace( textEdit.DataSource ) )
                                {
                                    if ( textEdit.BindingManager.Position>=0&&textEdit.BindingManager.Current!=null&&textEdit.BindingManager.Current is ABCBusinessEntities.BusinessObject )
                                    {
                                        object objValue=editValue;
                                        if ( strDataMember.Contains( ":" )&&editValue is Guid&&ABCHelper.DataConverter.ConvertToGuid(editValue)!=Guid.Empty )
                                        {
                                            ABCBusinessEntities.BusinessObject objBinds=textEdit.BindingManager.Current as ABCBusinessEntities.BusinessObject;
                                            objValue=DataCachingProvider.GetCachingObjectAccrossTable( objBinds , ABCHelper.DataConverter.ConvertToGuid( editValue ) , strDataMember );
                                        }
                                        return DataFormatProvider.DoFormat( objValue , textEdit.TableName , strDataMember );
                                    }
                                }
                            }
                        }
                    }
                }
                catch ( Exception ex )
                { }
            }

            return base.GetDisplayText( editValue );
        }
    }


    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.TextEdit ) )]
    [Designer( typeof( ABCTextEditDesigner ) )]
    public class ABCTextEdit : DevExpress.XtraEditors.TextEdit , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }

        #region Register
        public override string EditorTypeName
        {
            get
            {
                return ABCRepositoryTextEdit.CustomEditName;
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public new ABCRepositoryTextEdit Properties
        {
            get { return base.Properties as ABCRepositoryTextEdit; }
        }
        static ABCTextEdit ( )
        {
            ABCRepositoryTextEdit.RegisterCustomEdit();
        }
        #endregion

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "EditValue";
            }
        }

        [ReadOnly( true )]
        public String TableName { get; set; }
        #endregion

        #region External Properties
        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

        [Category( "External" )]
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
        [Category( "External" )]
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
        [Category( "External" )]
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
                 if(OwnerView.Mode!=ViewMode.Design) this.Visible=value;
            }
        }

        private String dummyText;
        [Category( "External" )]
        public String DummyText
        {
            get
            {
                return dummyText;
            }
            set
            {
                this.Properties.NullValuePrompt=value;
                dummyText=value;
            }
        }
        #endregion

        public ABCTextEdit ( )
        {

        }

        #region IABCControl
        public void InitControl ( )
        {
            InitBothMode();

            if ( OwnerView!=null&&OwnerView.Mode==ViewMode.Design )
                InitDesignTime();
            else
                InitRunTime();
        }
        public void InitBothMode ( )
        {
            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;
          //  this.Properties.Mask.UseMaskAsDisplayFormat=true;
            if ( !String.IsNullOrWhiteSpace( this.EditMask.ToString() )&&!String.IsNullOrWhiteSpace( this.MaskType.ToString() ) )
                this.Properties.Mask.UseMaskAsDisplayFormat=false;
            this.Properties.NullValuePromptShowForEmptyValue=true;

            if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
                this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;

      

        }
        public void InitRunTime ( )
        {
            this.Properties.Appearance.ForeColor=Color.Black;
            this.Properties.Appearance.Options.UseForeColor=true;
        }
        public void InitDesignTime ( )
        {
            if ( String.IsNullOrWhiteSpace( this.DataMember )==false )
                this.Properties.NullValuePrompt="["+this.DataMember+"]";

        }

        #endregion

        protected override void Dispose ( bool disposing )
        {

            base.Dispose( disposing );
        }
    }

    public class ABCTextEditDesigner : ControlDesigner
    {
        public ABCTextEditDesigner ( )
        {
        }

    }


  
}
