using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.Utils.Paint;

using ABCBusinessEntities;
using ABCControls;
namespace ABCApp
{
    public class ABCChatBox : ABCControls.ABCPanelControl
    {
        public ABCChatArea ChatArea;
        public ABCChatScreen ChatScreen;
        public ABCChatBox ( ABCChatScreen form , String strUser2 )
        {
            ChatScreen=form;

            this.IsSizeable=true;
            ChatArea=new ABCChatArea( this , strUser2 );
            this.Size=ChatArea.Size;
            ChatArea.Dock=DockStyle.Fill;
            this.Controls.Add( ChatArea );
            this.MouseMove+=new MouseEventHandler( ABCChatBox_MouseMove );
        }

        void ABCChatBox_MouseMove ( object sender , MouseEventArgs e )
        {
         //   Cursor.Current=Cursor.Default'
        }
        protected override void Dispose ( bool disposing )
        {
            if ( ChatArea!=null )
            {
                ChatArea.StopTimer();
                ChatArea.Dispose();
            }
            base.Dispose( disposing );
        }
    }
}
