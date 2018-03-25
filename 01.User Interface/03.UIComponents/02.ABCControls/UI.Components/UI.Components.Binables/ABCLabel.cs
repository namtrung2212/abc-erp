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


using ABCCommon;
using ABCProvider;

namespace ABCControls
{


    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.LabelControl ))]
    [Designer(typeof(ABCLabelDesigner))]
    public class ABCLabel : DevExpress.XtraEditors.LabelControl , IABCControl , IABCBindableControl
    {
        public ABCView OwnerView { get; set; }

        #region IBindingableControl

        [Category( "ABC.BindingValue" )]
        [Editor( typeof( ColumnChooserEditor ) , typeof( UITypeEditor ) )]
        public String DataMember { get; set; }

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }

        [Browsable( false )]
        public String BindingProperty
        {
            get
            {
                return "Text";
            }
        }
        #endregion

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
                if ( OwnerView!=null &&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }
        #endregion
   
        public ABCLabel ()
        {
            this.AutoSizeMode=LabelAutoSizeMode.Vertical;
        }
       
        #region IABCControl

        public void InitControl ( )
        {
            this.DataBindings.CollectionChanged+=new CollectionChangeEventHandler( DataBindings_CollectionChanged );
        }

        void DataBindings_CollectionChanged ( object sender , CollectionChangeEventArgs e )
        {
            if ( e.Action==CollectionChangeAction.Add )
                ( e.Element as Binding ).Format+=new ConvertEventHandler( ABCLabel_Format );
        }

        void ABCLabel_Format ( object sender , ConvertEventArgs e )
        {
            if ( this.DataMember.Contains( ":" ) )
            {
                e.Value=DataCachingProvider.GetCachingObjectAccrossTable( this.TableName , ABCHelper.DataConverter.ConvertToGuid( e.Value ) , this.DataMember );
                e.Value=DataFormatProvider.DoFormat( e.Value , this.TableName , this.DataMember );
            }

        }
        #endregion

        public void Initialize ( ABCView view , ABCBindingInfo bindingInfo )
        {
            this.DataSource=bindingInfo.BusName;
            this.DataMember=bindingInfo.FieldName;
            this.TableName=bindingInfo.TableName;
            this.Text=ABCPresentHelper.GetLabelCaption( view , this.DataSource , this.DataMember );
        }
    }

    public class ABCLabelDesigner : ControlDesigner
    {
        public ABCLabelDesigner ( )
        {
        }
    }
}
