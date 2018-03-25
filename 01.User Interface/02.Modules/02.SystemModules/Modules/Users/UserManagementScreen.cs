using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using ABCScreen;

using ABCControls;
using System.Reflection;
using System.Reflection.Emit;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCApp.Screens
{


    public class UserManagementScreen : ABCBaseScreen
    {
        public UserManagementScreen ( )
        {
        }

        bool isModified=false;
        public override void OnDataObjectChangedFromUI ( ABCScreen.Data.DataManager.ABCDataChangedStructer arg )
        {
            base.OnDataObjectChangedFromUI( arg );
            isModified=true;
        }
        public override void OnScreenClosed ( )
        {
            base.OnScreenClosed();
            if ( isModified )
            {
                //ABCHelper.ABCWaitingDialog.Show( "Cập nhật cấu hình người dùng" , "Vui lòng đợi..." );
                //ABCUserProvider.SynchronizePermission();
                //ABCHelper.ABCWaitingDialog.Close();
            }
        }
    }
}
