using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ABCControls
{
    public partial class ABCViewDlg : DevExpress.XtraEditors.XtraForm
    {
        ABCView OwnerView;

        public ABCViewDlg ( ABCView view )
        {
            InitView( view );
        }
        public ABCViewDlg ()
        {
          
        }

        public void InitView ( ABCView view )
        {
            this.Controls.Clear();
            if ( OwnerView!=null )
            {
                OwnerView.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers(); 
            }

            this.ClientSize=view.Size;
            if ( view.ShowToolbar )
                this.ClientSize=new System.Drawing.Size( this.ClientSize.Width , this.ClientSize.Height+30 );
            OwnerView=view;
            OwnerView.Parent=this;
            OwnerView.Dock=DockStyle.Fill;
            OwnerView.AutoScroll=true;
    
            if ( OwnerView.DataField!=null )
                this.Text=OwnerView.DataField.STViewName;
            this.StartPosition=OwnerView.StartPosition;
            this.Text=OwnerView.Caption;
            this.WindowState=OwnerView.WindowState;
            this.FormBorderStyle=OwnerView.FormBorderStyle;
            this.ControlBox=OwnerView.ControlBox;
            this.MinimizeBox=OwnerView.MinimizelBox;
            this.MaximizeBox=OwnerView.MaximizeBox;
            this.ShowInTaskbar=false;
            this.Shown+=new EventHandler( ABCViewDlg_Shown );
            this.FormClosed+=new System.Windows.Forms.FormClosedEventHandler( ABCViewDlg_FormClosed );
        }
       

        void ABCViewDlg_Shown ( object sender , EventArgs e )
        {
            //foreach ( var ctrl in GetAll( this , typeof( ABCGridControl ) ) )
            //{
            //    ( ctrl as ABCGridControl ).DefaultView.ShowEditor();
            //    return;
            //}
            //foreach ( var ctrl in GetAll( this , typeof( ABCGridBandedControl ) ) )
            //{
            //    ( ctrl as ABCGridBandedControl ).BandedView.ShowEditor();
            //    return;
            //}
          
        }

        public IEnumerable<Control> GetAll ( Control control , Type type )
        {
            var controls=control.Controls.Cast<Control>();

            return controls.SelectMany( ctrl => GetAll( ctrl , type ) )
                                      .Concat( controls )
                                      .Where( c => c.GetType()==type );
        }

        void ABCViewDlg_FormClosed ( object sender , System.Windows.Forms.FormClosedEventArgs e )
        {
          //  OwnerView.Dispose();
            GC.Collect();

            if ( ABCStudio.ABCStudioHelper.Instance!=null&&
                ABCStudio.ABCStudioHelper.Instance.MainStudio!=null&&
                ABCStudio.ABCStudioHelper.Instance.MainStudio.Visible )
            {
                ABCStudio.ABCStudioHelper.Instance.MainStudio.Activate();
                return;
            }
            if ( ABCApp.ABCAppHelper.Instance!=null&&
               ABCApp.ABCAppHelper.Instance.MainForm!=null&&
               ABCApp.ABCAppHelper.Instance.MainForm.Visible )
            {
                ABCApp.ABCAppHelper.Instance.MainForm.Activate();
                return;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing&&( components!=null ) )
            {
                if ( OwnerView!=null )
                    OwnerView.Dispose();
                components.Dispose();
            }
            base.Dispose( disposing );

            GC.Collect();
        }

    }
}