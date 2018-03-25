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

using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.CheckEdit ) )]
    [Designer( typeof( ABCCheckEditDesigner ) )]
    public class ABCCheckEdit : DevExpress.XtraEditors.CheckEdit , IABCControl , IABCBindableControl
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
                return "EditValue";
            }
        }
        #endregion

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup { get; set; }

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
                if ( OwnerView!=null &&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

        [Category( "External" )]
        public String GridControl { get; set; }

        #endregion

        public ABCCheckEdit ( )
        {
            this.CheckedChanged+=new EventHandler( ABCCheckEdit_CheckedChanged );
            this.CheckStateChanged+=new EventHandler( ABCCheckEdit_CheckStateChanged );
            this.Margin=new System.Windows.Forms.Padding( 3 ,1 , 3 , 1 );
            this.Height=20;
        }

        void ABCCheckEdit_CheckStateChanged ( object sender , EventArgs e )
        {
            if (OwnerView!=null &&  OwnerView.Mode!=ViewMode.Design && String.IsNullOrWhiteSpace(GridControl)==false)
            {
                Control[] controls=OwnerView.Controls.Find( GridControl ,true );
                if ( controls.Length>0 )
                {
                    if ( controls[0] is ABCGridControl )
                    {
                        ABCGridControl grid=controls[0] as ABCGridControl;
                        if ( grid.GridDefaultView.Columns[ABCCommon.ABCConstString.colSelected]!=null )
                        {
                            if ( grid.GridDataSource is System.Windows.Forms.BindingSource&&( grid.GridDataSource as System.Windows.Forms.BindingSource ).DataSource is System.Collections.IList )
                            {
                                foreach ( BusinessObject obj in (( grid.GridDataSource as System.Windows.Forms.BindingSource ).DataSource as System.Collections.IList ) )
                                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colSelected , this.CheckState==System.Windows.Forms.CheckState.Checked );
                            }
                        }
                    }
                    if ( controls[0] is ABCGridBandedControl )
                    {
                        ABCGridBandedControl grid=controls[0] as ABCGridBandedControl;
                        if ( grid.BandedView.Columns[ABCCommon.ABCConstString.colSelected]!=null )
                        {
                            if ( grid.GridDataSource is System.Windows.Forms.BindingSource&&( grid.GridDataSource as System.Windows.Forms.BindingSource ).DataSource is System.Collections.IList )
                            {
                                foreach ( BusinessObject obj in ( ( grid.GridDataSource as System.Windows.Forms.BindingSource ).DataSource as System.Collections.IList ) )
                                    ABCDynamicInvoker.SetValue( obj , ABCCommon.ABCConstString.colSelected , this.CheckState==System.Windows.Forms.CheckState.Checked );
                            }
                        }
                    }
                }
            }
        }

        void ABCCheckEdit_CheckedChanged ( object sender , EventArgs e )
        {
            if ( OwnerView!=null )
            {
                OwnerView.SelectNextControl( OwnerView.ActiveControl , true , true , true , true );
                Application.DoEvents();
            }
        }

        #region IABCControl
        public void InitControl ( )
        {
            InitBothMode();

              if ( OwnerView!=null &&OwnerView.Mode==ViewMode.Design)
                InitDesignTime();
            else
                InitRunTime();
        }

        public void InitBothMode ( )
        {

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;
       
            

            if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
                this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
        }
        public void InitRunTime ( )
        {

        }
        public void InitDesignTime ( )
        {
        }

        #endregion

        public void Initialize ( ABCView view , ABCBindingInfo bindingInfo )
        {
            this.DataSource=bindingInfo.BusName;
            this.DataMember=bindingInfo.FieldName;
         //   this.TableName=bindingInfo.TableName;
            this.Text=ABCPresentHelper.GetLabelCaption( view , this.DataSource , this.DataMember );
        }
    }

    public class ABCCheckEditDesigner : ControlDesigner
    {
        public ABCCheckEditDesigner ( )
        {
        }
    }
}
