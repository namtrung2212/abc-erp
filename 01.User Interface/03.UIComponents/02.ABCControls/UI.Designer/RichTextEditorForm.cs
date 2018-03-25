using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ABCControls
{
    public partial class RichTextEditorForm : DevExpress.XtraEditors.XtraForm
    {

        public String Content
        {
            get { return this.richEditControl1.RtfText; } 
            set { this.richEditControl1.RtfText=value; }
        }
        public RichTextEditorForm ( )
        {
            InitializeComponent();
            this.FormClosing+=new FormClosingEventHandler( RichTextEditorForm_FormClosing );
        }

        void RichTextEditorForm_FormClosing ( object sender , FormClosingEventArgs e )
        {
           DialogResult result= ABCHelper.ABCMessageBox.Show( "Do you want to save and update the content?" , "Message" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Question );

           if ( result==System.Windows.Forms.DialogResult.Cancel )
           {
               e.Cancel=true;
               return;
           }
           this.DialogResult=result;
        }
    }
}