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
    [ToolboxBitmapAttribute( typeof( System.Windows.Forms.FlowLayoutPanel ))]
    [Designer(typeof(ABCFlowPanelControlDesigner))]
    public class ABCFlowPanelControl : System.Windows.Forms.FlowLayoutPanel , IABCControl
    {
        public ABCView OwnerView { get; set; }

        #region External Properties

        [Category( "ABC" )]
        public String Comment { get; set; }

        [Category( "ABC.Format" )]
        public String FieldGroup{ get; set; }

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
        #endregion

        public ABCFlowPanelControl ()
        {
            this.DragEnter+=new DragEventHandler( DoDragEnter );
            this.DragDrop+=new DragEventHandler( DoDragDrop );
            this.AllowDrop=true;
        //    this.WrapContents
        }

        void DoDragDrop ( object sender , DragEventArgs e )
        {
            Control data=(Control)e.Data.GetData( e.Data.GetFormats()[0] );
            FlowLayoutPanel _destination=(FlowLayoutPanel)sender;
         //   FlowLayoutPanel _source=(FlowLayoutPanel)data.Parent;

            //if ( _source!=_destination )
            //{
            //    // Add control to panel
            //    _destination.Controls.Add( data );
            //    data.Size=new Size( _destination.Width , 50 );

            //    // Reorder
            //    Point p=_destination.PointToClient( new Point( e.X , e.Y ) );
            //    var item=_destination.GetChildAtPoint( p );
            //    int index=_destination.Controls.GetChildIndex( item , false );
            //    _destination.Controls.SetChildIndex( data , index );

            //    // Invalidate to paint!
            //    _destination.Invalidate();
            //    _source.Invalidate();
            //}
            //else
            //{
                // Just add the control to the new panel.
                // No need to remove from the other panel, this changes the Control.Parent property.
                Point p=_destination.PointToClient( new Point( e.X , e.Y ) );
                var item=_destination.GetChildAtPoint( p );
                int index=_destination.Controls.GetChildIndex( item , false );
                _destination.Controls.SetChildIndex( data , index );
                _destination.Invalidate();
         //   }
        }

        void DoDragEnter ( object sender , DragEventArgs e )
        {
            e.Effect=DragDropEffects.Move;
        }


        #region IABCControl

        public void InitControl ( )
        {
        } 
        #endregion

    }

    public class ABCFlowPanelControlDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public ABCFlowPanelControlDesigner ( )
        {
        }
    }
}
