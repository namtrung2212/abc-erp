using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ABCBusinessEntities;
using ABCScreen;
using ABCControls;
using ABCProvider;

namespace ABCApp.Screens
{
    public class InventoryReportScreen : ABCBaseScreen
    {
        public InventoryReportScreen ( )
        {
            this.ScreenLoadedEvent+=new ABCScreenLoadedEventHandler( OnScreenLoadedEvent );
        }

        void OnScreenLoadedEvent ( )
        {
            ABCSimpleButton btnRecalcInventory=this.UIManager["btnRecalcInventory"] as ABCSimpleButton;
            if ( btnRecalcInventory!=null )
                btnRecalcInventory.Click+=new EventHandler( btnRecalcInventory_Click );
        }

        void btnRecalcInventory_Click ( object sender , EventArgs e )
        {

            Control period=UIManager.GetControl( "period" );
            if ( period!=null&&( period as ABCPeriodEdit ).EditValue!=null )
            {
                Guid periodID=ABCHelper.DataConverter.ConvertToGuid( ( period as ABCPeriodEdit ).EditValue );
                if ( periodID!=Guid.Empty )
                {
                    GEPeriodsInfo preriodInfo=new GEPeriodsController().GetObjectByID( periodID ) as GEPeriodsInfo;
                    if ( preriodInfo!=null )
                    {
                        ABCHelper.ABCWaitingDialog.Show( "" , String.Format( "Tính tồn kho tháng {0}/{1}. . .!" , preriodInfo.Month , preriodInfo.Year ) );
                        InventoryProvider.PeriodEndingProcessing( periodID );

                        DoAction( ABCCommon.ABCScreenAction.Refresh , false );

                        ABCHelper.ABCWaitingDialog.Close();
                    }
                }
            }
        }

    }

}
