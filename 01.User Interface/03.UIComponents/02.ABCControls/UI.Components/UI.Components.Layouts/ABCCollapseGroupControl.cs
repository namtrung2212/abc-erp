using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;
using DevExpress.XtraEditors;


using ABCCommon;

namespace ABCControls
{
    [ToolboxBitmapAttribute( typeof( DevExpress.XtraEditors.GroupControl ))]
    [Designer(typeof(ABCCollapseGroupControlDesigner))]
    public class ABCCollapseGroupControl : DevExpress.XtraEditors.GroupControl , IABCControl
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

        bool isDefaultCollapse=true;
        [Category( "External" )]
        public Boolean IsDefaultCollapse
        {
            get
            {
                return isDefaultCollapse;
            }
            set
            {
                isDefaultCollapse=value;
                if ( OwnerView!=null && OwnerView.Mode!=ViewMode.Design )
                {
                    this.isDefaultCollapse=value;
                    if ( isDefaultCollapse )
                    {
                        orgHeight=this.Height;
                        this.Height=20;
                    }
                    else
                        this.Height=orgHeight;
                }
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

        public ABCCollapseGroupControl ()
        {
            InitCollapase();
            InitDrag();
            this.AppearanceCaption.Font=new Font( this.AppearanceCaption.Font , FontStyle.Bold );
            this.AppearanceCaption.Options.UseFont=true;
        }

        #region IABCControl

        public void InitControl ( )
        {
        } 
        #endregion

        #region Collapse
        public void InitCollapase ( )
        {
            this.Paint+=new PaintEventHandler( CollapseGroupControl_Paint );
            this.MouseDown+=new MouseEventHandler( CollapseGroupControl_MouseDown );
            this.MouseUp+=new MouseEventHandler( CollapseGroupControl_MouseUp );
            this.MouseDoubleClick+=new MouseEventHandler( CollapseGroupControl_MouseDoubleClick );
            timer.Interval=10;
            timer.Tick+=new EventHandler( timer_Tick );
        }
        bool isPressed=false;
        bool isCollapse=false;
        int orgHeight;
        Timer timer=new Timer();


        void CollapseGroupControl_MouseDoubleClick ( object sender , MouseEventArgs e )
        {
            if ( CurrentResizeType==ResizeType.None&&e.Y<16 )
            {
                isCollapse=( !isCollapse );
                UpdateCollapse();
                Invalidate( false );
            }
        }
        void CollapseGroupControl_MouseUp ( object sender , MouseEventArgs e )
        {
            if ( e.X<this.Width-3&&e.X>this.Width-15&&e.Y<16)// &&( this.Dock==DockStyle.Top||this.Dock==DockStyle.Bottom))
            {
                isPressed=false;
                Invalidate( false );
            }
        }
        void CollapseGroupControl_MouseDown ( object sender , MouseEventArgs e )
        {
            if ( CurrentResizeType==ResizeType.None&&e.X<this.Width-2&&e.X>this.Width-15&&e.Y<16 )//&&( this.Dock==DockStyle.Top||this.Dock==DockStyle.Bottom ) )
            {
                isPressed=true;
                isCollapse=( !isCollapse );
                UpdateCollapse();
                Invalidate( false );
            }
        }

        void timer_Tick ( object sender , EventArgs e )
        {
            if ( isCollapse )
            {
                this.Height-=15;
                if ( this.Height<20 )
                {
                    this.Height=20;
                    timer.Stop();
                }
            }
            else
            {
                this.Height+=15;
                if ( this.Height>orgHeight )
                {
                    this.Height=orgHeight;
                    timer.Stop();
                }
            }
            //if ( timer.Interval>10 )
            //    timer.Interval-=10;
            //else
            //    timer.Interval=5;
        }
        public void UpdateCollapse ( )
        {

            if ( isCollapse )
                orgHeight=this.Height;

            timer.Interval=10;
            timer.Start();
        }
        void CollapseGroupControl_Paint ( object sender , PaintEventArgs e )
        {
            int aaa=GetCaptionHorizontal();
            aaa=GetCaptionVertical();
            VisualStyleRenderer renderer=null;

            if ( !isCollapse )
            {
                if ( isPressed )
                    renderer=new VisualStyleRenderer( VisualStyleElement.ExplorerBar.NormalGroupCollapse.Pressed );
                else
                    renderer=new VisualStyleRenderer( VisualStyleElement.ExplorerBar.NormalGroupCollapse.Normal );
            }
            else
            {
                if ( isPressed )
                    renderer=new VisualStyleRenderer( VisualStyleElement.ExplorerBar.NormalGroupExpand.Pressed );
                else
                    renderer=new VisualStyleRenderer( VisualStyleElement.ExplorerBar.NormalGroupExpand.Normal );
            }
            renderer.DrawBackground( e.Graphics , new Rectangle( this.Width-18 , 1 , 16 , 16 ) );
        }
        #endregion
   
        public String GetPropertyBindingName ( )
        {
            return "";
        }

        #region Drag
        //Check radius for begin drag n drop
        public bool AllowDrag { get; set; }
        private bool _isDragging=false;
        private int _DDradius=40;
        private int startX=0;
        private int startY=0;

        void InitDrag ( )
        {
            AllowDrag=true;
            this.MouseDown+=new MouseEventHandler( DragMouseDown );
            this.MouseMove+=new MouseEventHandler( DragMouseMove );
            this.MouseUp+=new MouseEventHandler( DragoMouseUp );
        }

        void DragMouseDown ( object sender , MouseEventArgs e )
        {
            this.Focus();
            if (CurrentResizeType==ResizeType.None&& e.Y<16 )
            {
                startX=e.X;
                startY=e.Y;
                this._isDragging=false;
            }
        }

        void DragMouseMove ( object sender , MouseEventArgs e )
        {
            if ( !_isDragging )
            {
                if ( CurrentResizeType==ResizeType.None&&e.Button==MouseButtons.Left&&_DDradius>0&&this.AllowDrag )
                {
                    int num1=startX-e.X;
                    int num2=startY-e.Y;
                    if ( ( ( num1*num1 )+( num2*num2 ) )>_DDradius )
                    {
                        DoDragDrop( this , DragDropEffects.All );
                        _isDragging=true;
                        return;
                    }
                }
            }
        }

        void DragoMouseUp ( object sender , MouseEventArgs e )
        {
            _isDragging=false;
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
        private Point OldLocation;
        private Size OldSize;
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
            else if ( this.ClientSize.Height-4<=pos.Y&&pos.Y<=this.ClientSize.Height+4 ) //bottom
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
                if ( CurrentResizeType!=ResizeType.None )
                {
                    DragPosition=e.Location;
                    OldLocation=this.Location;
                    OldSize=this.Size;
                }
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
                    if ( newH<20 )
                        newH=20;
                    if ( newW<20 )
                        newW=20;

                    switch ( CurrentResizeType )
                    {
                        case ResizeType.Top:
                     
                            this.Size=new Size( this.Width , newH );     
                       //     this.Location=new Point( this.Location.X , this.Location.Y-e.Y+DragPosition.Y );
                            break;
                        case ResizeType.Bottom:
                            this.Size=new Size( this.Width , newH );
                            break;

                        case ResizeType.Left:
                       //     this.Location=new Point( this.Location.X-e.X+DragPosition.X , this.Location.Y );
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

    public class ABCCollapseGroupControlDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public ABCCollapseGroupControlDesigner ( )
        {

        }
      
    }
}
