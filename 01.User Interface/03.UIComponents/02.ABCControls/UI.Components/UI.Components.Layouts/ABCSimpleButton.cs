using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;


using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.SimpleButton ) )]
    [Designer( typeof( ABCSimpleButtonDesigner ) )]
    public class ABCSimpleButton : DevExpress.XtraEditors.SimpleButton , IABCControl
    {
        public ABCView OwnerView { get; set; }

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC" )]
        public int ImageIndex { get; set; }

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
                if ( OwnerView!=null&&OwnerView.Mode!=ViewMode.Design )
                    this.Visible=value;
            }
        }
        #endregion

        public enum ABCIconType
        {
            None=0 ,
            Approve=1 ,
            New=2 ,
            Edit=3 ,
            Delete=4 ,
            Save=5 ,
            Info=6 ,
            Refresh=7 ,
            Print=8 ,
            Config=9 ,
            Report=10 ,
            Chart=11 ,
            Add=12 ,
            Cancel=13 ,
            Wizard=14 ,
            Copy=15 ,
            Next=16 ,
            Previous=17 ,
            Lock=18 ,
            Post=19 ,
            Mail=20 ,
            Calc=21 ,
            Close=22 ,
            Calendar=23 ,
            Warning=24
        }
        [Category( "Function" )]
        public String DataSource { get; set; }

        ABCIconType iconType;
        [Category( "Function" )]
        public ABCIconType IconType
        {
            get
            {
                return iconType;
            }
            set
            {
                iconType=value;
                InvalidateIcon();
            }

        }

        public enum ABCButtonType
        {
            None=0 ,
            Cancel=1 ,
            Save=2 ,
            Delete=3
        }
         [Category( "Function" )]
        public ABCButtonType ButtonType { get; set; }

        public ABCSimpleButton ( )
        {
           // this.Click+=new EventHandler( ABCSimpleButton_Click );

        }

        //void ABCSimpleButton_Click ( object sender , EventArgs e )
        //{
        //    switch ( ButtonType )
        //    {
        //        case ABCButtonType.Cancel:
        //            this.FindForm().Close();
        //            break;
        //    }
        //}

        public void InvalidateIcon ( )
        {
            switch ( IconType )
            {
                case ABCIconType.Add:
                    this.Image=ABCImageList.GetImage16x16( "Add" );
                    break;
                case ABCIconType.Approve:
                    this.Image=ABCImageList.GetImage16x16( "Approve" );
                    break;
                case ABCIconType.Cancel:
                    this.Image=ABCImageList.GetImage16x16( "Cancel" );
                    break;
                case ABCIconType.Chart:
                    this.Image=ABCImageList.GetImage16x16( "Chart" );
                    break;
                case ABCIconType.Config:
                    this.Image=ABCImageList.GetImage16x16( "Config" );
                    break;
                case ABCIconType.Delete:
                    this.Image=ABCImageList.GetImage16x16( "Delete" );
                    break;
                case ABCIconType.Edit:
                    this.Image=ABCImageList.GetImage16x16( "Edit" );
                    break;
                case ABCIconType.New:
                    this.Image=ABCImageList.GetImage16x16( "New" );
                    break;
                case ABCIconType.Info:
                    this.Image=ABCImageList.GetImage16x16( "Info" );
                    break;
                case ABCIconType.Refresh:
                    this.Image=ABCImageList.GetImage16x16( "Refresh" );
                    break;
                case ABCIconType.Print:
                    this.Image=ABCImageList.GetImage16x16( "Print" );
                    break;
                case ABCIconType.Save:
                    this.Image=ABCImageList.GetImage16x16( "Save" );
                    break;
                case ABCIconType.Report:
                    this.Image=ABCImageList.GetImage16x16( "Note" );
                    break;
                case ABCIconType.Wizard:
                    this.Image=ABCImageList.GetImage16x16( "Wizard" );
                    break;
                case ABCIconType.Copy:
                    this.Image=ABCImageList.GetImage16x16( "Copy" );
                    break;
                case ABCIconType.Next:
                    this.Image=ABCImageList.GetImage16x16( "Right" );
                    break;
                case ABCIconType.Previous:
                    this.Image=ABCImageList.GetImage16x16( "Left" );
                    break;
                case ABCIconType.Lock:
                    this.Image=ABCImageList.GetImage16x16( "Lock" );
                    break;
                case ABCIconType.Post:
                    this.Image=ABCImageList.GetImage16x16( "Post" );
                    break;
                case ABCIconType.Mail:
                    this.Image=ABCImageList.GetImage16x16( "Mail" );
                    break;
                case ABCIconType.Calc:
                    this.Image=ABCImageList.GetImage16x16( "Calc" );
                    break;
                case ABCIconType.Close:
                    this.Image=ABCImageList.GetImage16x16( "Close" );
                    break;
                case ABCIconType.Calendar:
                    this.Image=ABCImageList.GetImage16x16( "Calendar" );
                    break;
                case ABCIconType.Warning:
                    this.Image=ABCImageList.GetImage16x16( "Warning" );
                    break;

            }
        }

        #region IABCControl

        public void InitControl ( )
        {
        }

        #endregion
    }

    public class ABCSimpleButtonDesigner : ControlDesigner
    {
        public ABCSimpleButtonDesigner ( )
        {
        }
    }
}