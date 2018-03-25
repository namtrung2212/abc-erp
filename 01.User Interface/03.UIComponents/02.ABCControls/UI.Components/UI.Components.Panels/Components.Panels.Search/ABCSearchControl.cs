using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Design;

using ABCCommon;

using ABCProvider;
namespace ABCControls
{

    public interface ISearchControl
    {
        String SearchString { get; }
        void SetFormatInfo ( );
    }
    public class ABCSearchInfo
    {
        public String DataSource;
        public String DataMember;
        public String TableName;
    }

    [Designer( typeof( ABCSearchControlDesigner ) )]
    [DefaultEvent( "InvalidateControl" )]
    public partial class ABCSearchControl : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCCustomControl
    {

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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
                if ( OwnerView!=null && OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

        private String textLabel;
        [Category( "Text" )]
        public String TextLabel
        {
            get
            {
                    return textLabel;
            }
            set
            {
                textLabel=value;
                UpdateLabelText();
            }
        }

        [Category( "Text" )]
        public Boolean TextVisible
        {
            get
            {
                return this.LayoutItem.TextVisible;
            }
            set
            {
                this.LayoutItem.TextVisible=value;
            }
        }
        private Boolean displayImage;
        [Category( "Text" )]
        public Boolean DisplayImage
        {
            get
            {
                displayImage=this.LayoutItem.Image!=null;
                return displayImage;
            }
            set
            {
                displayImage=value;
                if ( displayImage )
                {
                    this.LayoutItem.Image=global::ABCControls.Properties.Resources.InfoIcon;
                    this.LayoutItem.TextToControlDistance=4;
                }
                else
                {
                    this.LayoutItem.Image=null;
                    this.LayoutItem.TextToControlDistance=25;
                }
            }
        }

        [Category( "Text" )]
        public int TextWidth
        {
            get
            {
                return this.LayoutItem.TextSize.Width;
            }
            set
            {
                this.LayoutItem.TextSize=new Size( value , this.LayoutItem.TextSize.Height );
            }
        }

        public ISearchControl EditControl;

        [Category( "Control" )]
        public Control Editor { get { return (Control)EditControl; } set { EditControl=(ISearchControl)value; } }

        #endregion

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        #endregion


        public object EditValue
        {
            get
            {
                if ( EditControl!=null&&EditControl is IABCBindableControl &&String.IsNullOrWhiteSpace(( EditControl as IABCBindableControl ).BindingProperty)==false)
                    return EditControl.GetType().GetProperty( ( EditControl as IABCBindableControl ).BindingProperty ).GetValue( EditControl , null );
                return null;
            }
            set
            {
                if ( EditControl!=null )
                    EditControl.GetType().GetProperty( "EditValue" ).SetValue( EditControl , value , null );
            }
        }
        public ABCView OwnerView { get; set; }

        public ABCSearchControl ( )
        {
            InitializeComponent();

        }

        #region IABCControl
        public void InitControl ( )
        {
        }

        #endregion

        #region Design Time

        public void Initialize ( ABCView view , ABCSearchInfo searchInfo )
        {
            OwnerView=view;
            this.DataSource=searchInfo.DataSource;
            this.DataMember=searchInfo.DataMember;
            this.TableName=searchInfo.TableName;
            InvalidateControl();
        }
        #endregion

        public void InvalidateControl ( )
        {

            if ( EditControl==null )
            {
                #region init
                String strType=DataStructureProvider.GetCodingType( this.TableName , this.DataMember.Split( ':' )[0] );
                if ( strType=="String"||strType=="Nullable<String>" )
                {
                    EditControl=(ISearchControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeof( ABCTextSearch ) );
                }
                else if ( strType=="bool"||strType=="Nullable<bool>" )
                {
                    EditControl=(ISearchControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeof( ABCBoolSearch ) );
                }
                else if ( strType=="DateTime"||strType=="Nullable<DateTime>" )
                {
                    EditControl=(ISearchControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeof( ABCDateTimeSearch ) );
                }
                else if ( strType=="int"||strType=="Nullable<int>"||strType=="Guid"||strType=="Nullable<Guid>"||strType=="double"||strType=="Nullable<double>"||strType=="decimal"||strType=="Nullable<decimal>" )
                {
                    if(DataStructureProvider.IsForeignKey(this.TableName, this.DataMember.Split( ':' )[0] ) ||
                        DataStructureProvider.IsPrimaryKey( this.TableName , this.DataMember.Split( ':' )[0] ) )
                        EditControl=(ISearchControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeof( ABCGridLookUpEditSearch ) );
                    else
                        EditControl=(ISearchControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( typeof( ABCNumbSearch ) );
                }

                if ( EditControl!=null )
                {
                    EditControl.GetType().GetProperty( "DataMember" ).SetValue( EditControl , this.DataMember , null );
                    EditControl.GetType().GetProperty( "TableName" ).SetValue( EditControl , this.TableName , null );
                    if ( EditControl is ABCGridLookUpEdit )
                        ( (ABCGridLookUpEdit)EditControl ).Initialize( OwnerView );
                }

                #endregion
            }

            if ( EditControl!=null )
            {
                EditControl.GetType().GetProperty( "DataMember" ).SetValue( EditControl , this.DataMember , null );
                EditControl.GetType().GetProperty( "TableName" ).SetValue( EditControl , this.TableName , null );
                EditControl.SetFormatInfo();
                this.layoutControl1.Controls.Add( ( this.EditControl as Control ) );
                this.LayoutItem.Control=( this.EditControl as Control );

            }
            
          UpdateLabelText();
        }

        public void InitLayout ( ABCView view , XmlNode node )
        {
            OwnerView=view;
            InvalidateControl();
        }
        private void UpdateLabelText ( )
        {
            String strCaption=String.Empty;
            if ( String.IsNullOrWhiteSpace( this.DataSource ) )
                strCaption=ABCPresentHelper.GetLabelCaption( this.TableName , this.DataMember );
            else
                strCaption=ABCPresentHelper.GetLabelCaption( OwnerView , this.DataSource , this.DataMember );
       
            if ( String.IsNullOrWhiteSpace( textLabel ) )
                this.LayoutItem.Text=strCaption;
            else
                this.LayoutItem.Text=textLabel;
        }

        #region Size

        protected override void SetBoundsCore ( int x , int y , int width , int height , BoundsSpecified specified )
        {
            int iMax=20;

            if ( this.EditControl==null||( this.EditControl is ABCMemoEdit||this.EditControl is ABCRichTextEdit||this.EditControl is ABCRichEditControl||this.EditControl is ABCCheckedListBox ) )
                iMax=height;

            if ( ( specified&BoundsSpecified.Height )==0||height==iMax )
                base.SetBoundsCore( x , y , width , iMax , specified );

        }
        #endregion

    }

    public class ABCSearchControlDesigner : DevExpress.XtraEditors.Design.BaseEditDesigner
    {
        public ABCSearchControlDesigner ( )
        {
        }
        public override IList SnapLines
        {
            get
            {

                ArrayList snapLines=base.SnapLines as ArrayList;
                snapLines.Add( new SnapLine( SnapLineType.Baseline , 0 , SnapLinePriority.Medium ) );
                snapLines.Add( new SnapLine( SnapLineType.Bottom , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Horizontal , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Left , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Right , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Top , 0 , SnapLinePriority.High ) );
                snapLines.Add( new SnapLine( SnapLineType.Vertical , 0 , SnapLinePriority.High ) );
                return snapLines;
            }
        }
        public override bool ParticipatesWithSnapLines
        {
            get
            {
                return true;
            }
        }
    }
}
