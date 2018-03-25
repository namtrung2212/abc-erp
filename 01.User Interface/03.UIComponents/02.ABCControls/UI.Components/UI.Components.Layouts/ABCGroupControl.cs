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
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.GroupControl ))]
    [Designer(typeof(ABCGroupControlDesigner))]
    public class ABCGroupControl : DevExpress.XtraEditors.GroupControl,IABCControl
    {
        public ABCView OwnerView { get; set; }

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

        bool isSizeable=false;
        [Category( "External" )]
        public Boolean IsSizeable
        {
            get
            {
                return isSizeable;
            }
            set
            {
                isSizeable=value;
                if ( isSizeable )
                    InitSizeable();
            }
        }
        #endregion

        public ABCGroupControl ()
        {

            this.AppearanceCaption.Font=new Font( this.AppearanceCaption.Font , FontStyle.Bold );
            this.AppearanceCaption.Options.UseFont=true;
        }

        #region IABCControl

        public void InitControl ( )
        {
        } 
        #endregion


        #region Sizeable

        enum ResizeType
        {
            None ,
            Left ,
            Right ,
            Top ,
            Bottom ,
            TopLeft ,
            TopRight ,
            BottomLeft ,
            BottomRight
        }
        private const int cstGripSize=10;
        private ResizeType CurrentResizeType;

        private Point DragPosition;

        public void InitSizeable ( )
        {
            this.DoubleBuffered=true;
            this.SetStyle( ControlStyles.ResizeRedraw , true );
        }

        private ResizeType GetResizeType ( Point pos )
        {
            int isRight=0;
            int isTop=0;

            if ( 0<=pos.Y&&pos.Y<=4 ) //top
                isTop=1;
            else if ( this.ClientSize.Height-4<=pos.Y&&pos.Y<=this.ClientSize.Height +4) //bottom
                isTop=-1;

            if ( this.ClientSize.Width-4<=pos.X&&pos.X<=this.ClientSize.Width+4 ) //right
                isRight=1;
            else if ( 0<=pos.X&&pos.X<=4 ) //left
                isRight=-1;

            if ( isRight==0&&isTop==0 )
                return ResizeType.None;

            if ( isRight==1 )
            {
                if ( isTop==1 )
                    return ResizeType.TopRight;
                else if ( isTop==-1 )
                    return ResizeType.BottomRight;
            }
            else if ( isRight==-1 )
            {
                if ( isTop==1 )
                    return ResizeType.TopLeft;
                else if ( isTop==-1 )
                    return ResizeType.BottomLeft;
            }

            if ( isRight==0 )
            {
                if ( isTop==1 )
                    return ResizeType.Top;
                else if ( isTop==-1 )
                    return ResizeType.Bottom;
            }
            if ( isTop==0 )
            {
                if ( isRight==1 )
                    return ResizeType.Right;
                else if ( isRight==-1 )
                    return ResizeType.Left;
            }

            return ResizeType.None;
        }

        protected override void OnMouseDown ( MouseEventArgs e )
        {
            if ( isSizeable )
            {
                CurrentResizeType=GetResizeType( e.Location );
                SetCursor( CurrentResizeType );
                DragPosition=e.Location;
            }
            base.OnMouseDown( e );
        }

        protected override void OnMouseUp ( MouseEventArgs e )
        {
            if ( isSizeable )
                CurrentResizeType=ResizeType.None;
            base.OnMouseUp( e );
        }

        protected override void OnMouseMove ( MouseEventArgs e )
        {
            if ( isSizeable )
            {
                if ( CurrentResizeType!=ResizeType.None )
                {
                    int newW=this.Width+e.X-DragPosition.X;
                    int newH=this.Height+e.Y-DragPosition.Y;
                    switch ( CurrentResizeType )
                    {
                        case ResizeType.Top:
                            this.Location=new Point( this.Location.X , this.Location.Y+this.Height-newH );
                            this.Size=new Size( this.Width , newH );
                            break;
                        case ResizeType.Bottom:
                            this.Size=new Size( this.Width , newH );
                            break;

                        case ResizeType.Left:
                            this.Location=new Point( this.Location.X+this.Width-newW , this.Location.Y );
                            this.Size=new Size( newW , this.Height );
                            break;
                        case ResizeType.Right:
                            this.Size=new Size( newW , this.Height );
                            break;
                        default:
                            this.Size=new Size( newW , newH );
                            break;
                    }
                    DragPosition=e.Location;
                }
                else
                {
                    ResizeType type=GetResizeType( e.Location );
                    SetCursor( type );
                }
            }
            base.OnMouseMove( e );
        }
        void SetCursor ( ResizeType type )
        {
            if ( type==ResizeType.Top||type==ResizeType.Bottom )
                Cursor.Current=Cursors.SizeNS;
            else if ( type==ResizeType.Right||type==ResizeType.Left )
                Cursor.Current=Cursors.SizeWE;
            else if ( type==ResizeType.TopLeft||type==ResizeType.BottomRight )
                Cursor.Current=Cursors.SizeNWSE;
            else if ( type==ResizeType.TopRight||type==ResizeType.BottomLeft )
                Cursor.Current=Cursors.SizeNESW;
            else
                Cursor.Current=Cursors.Default;
        }
        #endregion
    }

    public class ABCGroupControlDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public ABCGroupControlDesigner ( )
        {
        }
    }
}
