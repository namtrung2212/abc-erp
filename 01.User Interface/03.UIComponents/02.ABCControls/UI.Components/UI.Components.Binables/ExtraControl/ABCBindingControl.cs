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

using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{

    public class ABCBindingInfo
    {
        public String BusName;
        public String FieldName;
        public String TableName;//TableName of Screen BusinessObject
        public Type ControlType;
        public STViewsInfo ViewInfo;
    }

    [Designer( typeof( ABCBindingBaseEditDesigner ) )]
    [DefaultEvent( "InvalidateControl" )]
    public partial class ABCBindingBaseEdit : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl , IABCCustomControl
    {

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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
                    this.LayoutItem.Image=ABCControls.ABCImageList.GetImage16x16( "DocLink" );
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
             return   this.LayoutItem.TextSize.Width;
            }
            set
            {
                this.LayoutItem.TextSize = new Size(  value,this.LayoutItem.TextSize.Height);
            }
        }

        [Category( "Control" )]
        public bool ReadOnly
        {

            get
            {
                if ( this.EditControl!=null&&( this.EditControl is DevExpress.XtraEditors.BaseEdit )&&( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties!=null )
                    return ( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.ReadOnly;
                else
                    return false;
            }
            set
            {
                if ( this.EditControl!=null&&( this.EditControl is DevExpress.XtraEditors.BaseEdit )&&( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties!=null )
                {
                    ( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.ReadOnly=value;
                    if ( value==false )
                        ( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.Appearance.BackColor=Color.White;
                }
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
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )this.Visible=value;
            }
        }
        private String _UIControlType;
        [Category( "Control" )]
        [Browsable( false )]
        public String UIControlType
        {
            get
            {
                return _UIControlType;
            }
            set
            {
                _UIControlType=value;
                InvalidateControl();
            }
        }

        public IABCBindableControl EditControl;

        [Category( "Control" )]
        public Control Editor { get { return (Control)EditControl; } set { EditControl=(IABCBindableControl)value; } }

        [Category( "Link" )]
        public String LinkScreen { get; set; }
        [Category( "Link" )]
        public String ParamFields { get; set; }

        #endregion

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        
        [Category( "ABC.BindingValue" )]
      //  [ReadOnly( true )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "";
            }
        }
        #endregion


        public object EditValue
        {
            get
            {
                if ( EditControl!=null&&EditControl is IABCBindableControl&&String.IsNullOrWhiteSpace( ( EditControl as IABCBindableControl ).BindingProperty )==false )
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

        public ABCBindingBaseEdit ( )
        {
            InitializeComponent();
        }

        #region IABCControl
        public void InitControl ( )
        {
            this.LayoutItem.MouseUp+=new MouseEventHandler( LayoutItem_MouseUp );
           
        }

        void LayoutItem_MouseUp ( object sender , MouseEventArgs e )
        {
            //if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design)
            //{
                if ( e.Button==System.Windows.Forms.MouseButtons.Left&&this.DisplayImage && EditControl!=null
                    &&this.LayoutItem.TextSize.Width-18<e.X &&e.X<this.LayoutItem.TextSize.Width)
                {
                    BindingSource binding=( this.EditControl as Control ).DataBindings[0].DataSource as BindingSource;
                    BusinessObject busObj=binding.DataSource as BusinessObject;

                    #region Link by ID
                    
                    object obj=EditControl.GetType().GetProperty( "EditValue" ).GetValue( EditControl , null );
                    Guid iID=ABCHelper.DataConverter.ConvertToGuid( obj );

                    String strLinkTableName=String.Empty;
                    Guid iLinkID=Guid.Empty;
                    if ( this.DataMember.Contains( ":" ) )
                    {
                     
                        DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( busObj , iID , this.DataMember );
                        if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                            ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                        {
                            strLinkTableName=acrrosInfo.TableName;
                            iLinkID=acrrosInfo.TableID;
                        }
                    }
                    else if ( DataStructureProvider.IsForeignKey( TableName , this.DataMember ) )
                    {
                        strLinkTableName=DataStructureProvider.GetTableNameOfForeignKey( TableName , this.DataMember );
                        iLinkID=iID;

                    }
                    else
                    {
                        strLinkTableName=TableName;
                        iLinkID= BusinessObjectHelper.GetIDValue( busObj ) ;
                    }


                    if ( iLinkID!=Guid.Empty && String.IsNullOrWhiteSpace(strLinkTableName)==false)
                    {
                         ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , this.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                        return;
                    }
                    #endregion

                    if ( String.IsNullOrWhiteSpace( this.LinkScreen )==false )
                    {
                        String[] strArrays=this.ParamFields.Split( ';' );
                        object[] lstParams=new object[strArrays.Length];
                        for ( int i=0; i<strArrays.Length; i++ )
                        {
                            object objParam=strArrays[i];
                            if ( DataStructureProvider.IsTableColumn( busObj.AATableName , strArrays[i] ) )
                                objParam=ABCBusinessEntities.ABCDynamicInvoker.GetValue( busObj , strArrays[i] );
                            lstParams[i]=objParam;
                        }

                         ABCScreen.ABCScreenHelper.Instance.RunLink( this.LinkScreen , this.OwnerView.Mode , false , ABCScreenAction.None , lstParams );
                    }
                }
            //}
        }

        #endregion

        #region Design Time

        public void Initialize ( ABCView view , ABCBindingInfo bindingInfo )
        {
            try
            {
                OwnerView=view;
                this.DataSource=bindingInfo.BusName;
                this.DataMember=bindingInfo.FieldName;
                this.TableName=bindingInfo.TableName;
                if ( bindingInfo.ControlType==null||String.IsNullOrWhiteSpace( bindingInfo.ControlType.FullName ) )
                    this.UIControlType=typeof( ABCTextEdit ).FullName;
                else
                    this.UIControlType=bindingInfo.ControlType.FullName;
            }
            catch ( Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( "Binding cancel" );
            }
        }
        #endregion

        public void InvalidateControl ( )
        {

            if ( EditControl==null&&String.IsNullOrWhiteSpace( UIControlType )==false )
            {
                #region Design - FirstTIme
                EditControl=(IABCBindableControl)ABCBusinessEntities.ABCDynamicInvoker.CreateInstanceObject( Type.GetType( UIControlType ) );
                EditControl.DataSource=this.DataSource;
                EditControl.DataMember=this.DataMember;
                EditControl.TableName=this.TableName;

                if ( this.DataMember.Contains( ":" ) )
                    SetToReadOnly();
                #endregion
            }

            if ( EditControl!=null )
            {
                EditControl.DataSource=this.DataSource;
                EditControl.DataMember=this.DataMember;
                EditControl.TableName=this.TableName;

                this.layoutControl1.Controls.Add( ( this.EditControl as Control ) );
                this.LayoutItem.Control=( this.EditControl as Control );

                #region DisplayImage
                if ( this.DataMember.Contains( ":" ) )
                {
                    DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( TableName , this.DataMember );
                    if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                        ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                    {
                        STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , acrrosInfo.TableName ) );
                        if ( viewIfo!=null )
                            this.DisplayImage=true;
                    }
                }
                else if ( DataStructureProvider.IsForeignKey( TableName , this.DataMember ) )
                {
                    String strPKTableName=DataStructureProvider.GetTableNameOfForeignKey( TableName , this.DataMember );
                    STViewsInfo viewIfo=(STViewsInfo)new STViewsController().GetObject( String.Format( "SELECT * FROM STViews WHERE [MainTableName] = '{0}' " , strPKTableName ) );
                    if ( viewIfo!=null )
                        this.DisplayImage=true;
                }
                #endregion

                if ( DataStructureProvider.IsNOColumn( this.TableName , this.DataMember ) )
                    ReadOnly=true;

                #region Permission

                DataCachingProvider.AccrossStructInfo accross=DataCachingProvider.GetAccrossStructInfo( this.TableName , this.DataMember );
                if ( ABCScreen.ABCScreenHelper.Instance.CheckFieldPermission( accross.TableName , accross.FieldName , FieldPermission.AllowView )==false )
                {
                    isAllowView=false;
                    ReadOnly=true;
                 }

                if ( ReadOnly==false )
                    ReadOnly=!ABCScreen.ABCScreenHelper.Instance.CheckFieldPermission( accross.TableName , accross.FieldName , FieldPermission.AllowEdit );
                #endregion

                if(isAllowView)
                    ( this.EditControl as DevExpress.XtraEditors.BaseEdit ).CustomDisplayText+=new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler( ABCBindingBaseEdit_CustomDisplayText );
          
                if ( ReadOnly )
                    SetToReadOnly();
                else
                {
                    if ( this.EditControl is DevExpress.XtraEditors.BaseEdit&&( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties!=null )
                        ( this.EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.AppearanceFocused.BackColor=Color.Bisque;
                }
            }

            UpdateLabelText();
        }

        private void SetToReadOnly ( )
        {
            if ( EditControl is ABCBindingBaseEdit==false&&EditControl is Label==false )
            {
                if ( EditControl is ABCTextEdit )
                    ( EditControl as ABCTextEdit ).ReadOnly=true;
                else if ( EditControl is ABCMemoEdit )
                    ( EditControl as ABCMemoEdit ).ReadOnly=true;

                if ( EditControl is DevExpress.XtraEditors.BaseEdit )
                {
                    ( EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.ReadOnly=true;
                    ( EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.Appearance.BackColor=Color.FromArgb( 181 , 200 , 223 );
                }
            }
        }

        bool isAllowView=true;
        void ABCBindingBaseEdit_CustomDisplayText ( object sender , DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e )
        {
            ( EditControl as DevExpress.XtraEditors.BaseEdit ).Properties.Appearance.ForeColor=Color.Black;
            if ( isAllowView==false )
                e.DisplayText="<Dữ liệu ẩn>";
        }

        private void UpdateLabelText ( )
        {
            if ( this.LayoutItem.Control==null )
                return;

            String strCaption=String.Empty;

            if ( String.IsNullOrWhiteSpace( this.DataSource )==false&&string.IsNullOrWhiteSpace( this.DataMember )==false )
                strCaption=ABCPresentHelper.GetLabelCaption( OwnerView , this.DataSource , this.DataMember );
            else
            {
                if ( String.IsNullOrWhiteSpace( this.TableName )==false&&string.IsNullOrWhiteSpace( this.DataMember )==false )
                    strCaption=ABCPresentHelper.GetLabelCaption( this.TableName , this.DataMember );
                else
                    strCaption=DataConfigProvider.GetTableCaption( this.TableName );
            }
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

        #region Layout
        public void GetChildrenXMLLayout ( XmlElement containerElement )
        {
            XmlNode nodeCol=ABCHelper.DOMXMLUtil.GetFirstNode( containerElement , "P" , "Editor" );
            if ( nodeCol!=null )
                containerElement.RemoveChild( nodeCol );

            XmlElement eleEditor=ABCPresentHelper.Serialization( containerElement.OwnerDocument , this.EditControl , "C" );
            if ( eleEditor!=null )
                containerElement.AppendChild( eleEditor );

        }

        public void InitLayout ( ABCView view , XmlNode node )
        {
            OwnerView=view;

            XmlNode child=node.SelectNodes( "C" )[0];
            if ( child==null )
                return;

            String strType=child.Attributes["type"].Value.ToString();

            String strChildType=child.Attributes["type"].Value.ToString();
            Type type=TypeResolutionService.CurrentService.GetType( strChildType );
            if ( type==null )
                return;

            Component comp=(Component)ABCDynamicInvoker.CreateInstanceObject( type );
            ABCPresentHelper.DeSerialization( comp , child );
            ( (IABCControl)comp ).OwnerView=view;
            ( (IABCControl)comp ).InitControl();
            ( comp as Control ).Parent=this;
            this.EditControl=(IABCBindableControl)comp;

            if ( view.Mode!=ViewMode.Design&&comp is ABCGridLookUpEdit )
            {
                if ( ( (ABCGridLookUpEdit)comp ).Properties.View.Columns.Count<=2 )
                {
                    Component comp2=(Component)ABCDynamicInvoker.CreateInstanceObject( typeof( ABCLookUpEdit ) );
                    ( (IABCBindableControl)comp2 ).DataSource=this.DataSource;
                    ( (IABCBindableControl)comp2 ).DataMember=this.DataMember;
                    ( (IABCBindableControl)comp2 ).TableName=this.TableName;
        
                    ( (IABCControl)comp2 ).OwnerView=view;
                    ( (IABCControl)comp2 ).InitControl();
                    ( comp2 as Control ).Parent=this;
                    this.EditControl=(IABCBindableControl)comp2;
                }
            }

            InvalidateControl();

            if ( this.OwnerView==null||( this.OwnerView!=null&&this.OwnerView.Mode!=ViewMode.Design ) )
            {
                bool isUse=true;
                String strRealCol=this.DataMember.Split( ':' )[0];
                if ( !DataStructureProvider.IsTableColumn( this.TableName , strRealCol ) )
                    isUse=false;

                else if ( !DataConfigProvider.TableConfigList.ContainsKey( this.TableName )||!DataConfigProvider.TableConfigList[this.TableName].FieldConfigList.ContainsKey( strRealCol ) )
                    isUse=false;

                else if ( !DataConfigProvider.TableConfigList[this.TableName].FieldConfigList[strRealCol].InUse )
                    isUse=false;

                if ( !isUse )
                    this.Visible=false;
            }
        }
        #endregion
    }

    public class ABCBindingBaseEditDesigner : ControlDesigner
    {
        public ABCBindingBaseEditDesigner ( )
        {
        }

   //     ABCBindingBaseEdit bindControl;
        public override void Initialize ( IComponent component )
        {
            base.Initialize( component );
        //    bindControl=(ABCBindingBaseEdit)this.Control;

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
