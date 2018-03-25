using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using ABCCommon;

using ABCBusinessEntities;
using ABCCommon;

namespace ABCScreen
{

    public interface IABCScreenManager
    {
        void CallInitialize();
        void RunLink ( STViewsInfo viewIfo , ViewMode mode , bool isShowDialog , Guid iMainID ,ABCScreenAction action);
        void RunLink ( String strTableName , ViewMode mode , bool isShowDialog , Guid iMainID , ABCScreenAction action );
        void RunLink ( String strScreenName , ViewMode mode , bool isShowDialog , ABCScreenAction action , params object[] objParams );

        void ShowForm ( Form form,bool isShowDialog );
        void OpenScreenForNew ( String strTableName , ViewMode mode ,bool isShowDialog  );

        Guid GetCurrentUserID ( );

        bool CheckViewPermission ( Guid viewID , ViewPermission permission );
        bool CheckTablePermission ( String strTableName , TablePermission permission );
        bool CheckFieldPermission ( String strTableName , String strFieldName , FieldPermission permission );
        bool CheckVoucherPermission ( String strTableName , Guid ID , VoucherPermission permission );
        bool CheckVoucherPermission (BusinessObject obj , VoucherPermission permission );

    }

    public class ABCScreenHelper
    {
        public static IABCScreenManager Instance;

    }
}
