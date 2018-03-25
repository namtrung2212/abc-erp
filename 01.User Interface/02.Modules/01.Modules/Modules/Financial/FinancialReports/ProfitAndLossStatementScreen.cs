using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using ABCCommon;
using ABCBusinessEntities;
using ABCProvider;
using ABCControls;

namespace ABCScreen
{

    public class ProfitAndLossStatementScreen : ABCBaseScreen
    {
        public ProfitAndLossStatementScreen ( )
        {
            this.UILoadedEvent+=new ABCScreenUILoadedEventHandler( ProfitAndLossStatementScreen_UILoadedEvent );
        }

        void ProfitAndLossStatementScreen_UILoadedEvent ( )
        {
            ProfitAndLossStatement state=new ProfitAndLossStatement( "CÔNG TY TNHH THIẾT BỊ AN PHÚ" , "L52 , Đường số 7, KDC Phú Mỹ, Phường Phú Mỹ, Quận 7, TPHCM" , new ABCModules.FinanceStatisticTime( 2012 ) );
            state.Dock=DockStyle.Fill;
            this.UIManager.View.Controls.Add( state );
        }
    }
}
