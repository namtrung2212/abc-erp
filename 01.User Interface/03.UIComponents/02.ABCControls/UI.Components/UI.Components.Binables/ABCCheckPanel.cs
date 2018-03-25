using System;
using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Design;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.XtraEditors;

using ABCCommon;


namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.PanelControl ) )]
    [Designer( typeof( ABCCheckPanelDesigner ) )]
    public partial class ABCCheckPanel : DevExpress.XtraEditors.XtraUserControl , IABCControl , IABCBindableControl
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

        //[Category( "ABC" )]
        //public String Comment { get; set; }

        //[Category( "ABC.Format" )]
        //public String FieldGroup { get; set; }

        //[Category( "External" )]
        //public Boolean ReadOnly
        //{
        //    get
        //    {
        //        return checkEdit.Properties.ReadOnly;
        //    }
        //    set
        //    {
        //        checkEdit.Properties.ReadOnly=value;
        //    }
        //}
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
                if ( OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }

        #endregion

        //public object EditValue
        //{
        //    get
        //    {
        //        return checkEdit.EditValue;
        //    }
        //    set
        //    {
        //        checkEdit.EditValue=value;
        //    }
        //}

        //[Category( "External" )]
        //public String Caption
        //{
        //    get
        //    {
        //        return checkEdit.Text;
        //    }
        //    set
        //    {
        //        checkEdit.Text=value;
        //    }
        //}

        public ABCCheckPanel ( )
        {
            InitializeComponent();
           this.checkEdit.EditValueChanged+=new EventHandler( checkEdit_EditValueChanged );
            
        }

        void checkEdit_EditValueChanged ( object sender , EventArgs e )
        {
            this.panelControl.Enabled=this.checkEdit.Checked;
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

            //if ( this.Anchor==AnchorStyles.None )
            //    this.Anchor=AnchorStyles.Left|AnchorStyles.Top;



            //if ( this.RightToLeft==System.Windows.Forms.RightToLeft.Yes )
            //    checkEdit.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
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
      //      this.Caption=ABCPresentUtils.GetLabelCaption( view , this.DataSource , this.DataMember );
        }
    }

    public class ABCCheckPanelDesigner : DevExpress.XtraEditors.Design.BaseEditDesigner
    {
        public ABCCheckPanelDesigner ( )
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
