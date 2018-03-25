using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

using System.Reflection;
using System.Reflection.Emit;
using ABCCommon;
using ABCBusinessEntities;
using ABCProvider;
using ABCControls;


namespace ABCScreen
{


    public class CashFlowInDirectStatementScreen : ABCBaseScreen
    {
        public CashFlowInDirectStatementScreen ( )
        {
            this.UILoadedEvent+=new ABCScreenUILoadedEventHandler( CashFlowInDirectStatementScreen_UILoadedEvent );
        }

        void CashFlowInDirectStatementScreen_UILoadedEvent ( )
        {
            CashFlowInDirectStatement state=new CashFlowInDirectStatement( "CÔNG TY TNHH THIẾT BỊ AN PHÚ" , "L52 , Đường số 7, KDC Phú Mỹ, Phường Phú Mỹ, Quận 7, TPHCM" , new ABCModules.FinanceStatisticTime( 2012 ) );
            state.Dock=DockStyle.Fill;
            this.UIManager.View.Controls.Add( state );
        }
    }
}
