using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABCHelper
{
    public class ABCWaitingDialog
    {
        static DevExpress.Utils.WaitDialogForm waiting=new DevExpress.Utils.WaitDialogForm();

        public static void SetCaption ( String strCaption )
        {
            if ( waiting==null||waiting.Visible==false )
                Show( "" , strCaption );

            waiting.SetCaption( strCaption );

        }
        public static void Show ( )
        {
            Show( "Đang xử lý, vui lòng chờ..." , "" );
        }
        public static void Show ( String strTitle , String strCaption )
        {
            Close();

            if ( string.IsNullOrWhiteSpace( strTitle ) )
                strTitle="Đang xử lý, vui lòng chờ...";

            waiting=new DevExpress.Utils.WaitDialogForm( strCaption , strTitle );
            Cursor.Current=Cursors.WaitCursor;

            SetCaption( strCaption );

            waiting.Show();
        }


        public static void Close ( )
        {
            Cursor.Current=Cursors.Default;
            waiting.Close();
            waiting.Dispose();
        }
    }

}
