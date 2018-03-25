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
using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.DateEdit ) )]
    [Designer( typeof( ABCDateEditDesigner ) )]
    public class ABCDateEdit : DevExpress.XtraEditors.DateEdit , IABCControl , IABCBindableControl
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
        [Category( "External" )]
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


        #endregion

        #region Format
        public enum ABCDateFormat
        {
            DateAndTime ,
            Date ,
            Time ,
            MonthAndYear ,
            Year ,
            Month
        }

        ABCDateFormat formatType;
        [Category( "Format" )]
        public ABCDateFormat FormatType
        {
            get
            {
                return formatType;
            }
            set
            {
                formatType=value;
                RefreshFormat();
            }
        }
        public void RefreshFormat ( )
        {
            DataFormatProvider.FieldFormat format=DataFormatProvider.FieldFormat.Date;
            switch ( formatType )
            {
                case ABCDateFormat.Date:
                    format=DataFormatProvider.FieldFormat.Date;
                    break;
                case ABCDateFormat.DateAndTime:
                    format=DataFormatProvider.FieldFormat.DateAndTime;
                    break;
                case ABCDateFormat.Month:
                    format=DataFormatProvider.FieldFormat.Month;
                    break;
                case ABCDateFormat.MonthAndYear:
                    format=DataFormatProvider.FieldFormat.MonthAndYear;
                    break;
                case ABCDateFormat.Time:
                    format=DataFormatProvider.FieldFormat.Time;
                    break;
                case ABCDateFormat.Year:
                    format=DataFormatProvider.FieldFormat.Year;
                    break;
            }

            DataFormatProvider.ABCFormatInfo formatInfo=DataFormatProvider.GetFormatInfo( format );
            //     this.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
            this.Properties.DisplayFormat.FormatType=formatInfo.FormatInfo.FormatType;
            this.Properties.DisplayFormat.FormatString=formatInfo.FormatInfo.FormatString;
            this.Properties.EditFormat.FormatType=formatInfo.FormatInfo.FormatType;
            this.Properties.EditFormat.FormatString=formatInfo.FormatInfo.FormatString;

            this.Properties.Mask.UseMaskAsDisplayFormat=true;
            this.Properties.Mask.EditMask=formatInfo.FormatInfo.FormatString;
            this.Properties.Mask.MaskType=formatInfo.MaskType;



        }
        
        #endregion
        public ABCDateEdit ( )
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
            this.Properties.NullValuePrompt=DummyText;

            if ( this.Anchor==AnchorStyles.None )
                this.Anchor=AnchorStyles.Left|AnchorStyles.Top;
        
            

       //     if ( !String.IsNullOrWhiteSpace( EditMask.ToString() )&&!String.IsNullOrWhiteSpace( MaskType.ToString() ) )
                this.Properties.Mask.UseMaskAsDisplayFormat=true;

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
        }

        #endregion

    }

    public class ABCDateEditDesigner :  DevExpress.XtraEditors.Design.DateEditDesigner
    {
        public ABCDateEditDesigner ( )
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
