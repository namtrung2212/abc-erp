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
    public class InventoryReportByStockScreen : ABCBaseScreen
    {
        public InventoryReportByStockScreen ( )
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
            Control stock=UIManager.GetControl( "stock" );

            if ( period!=null&&stock!=null&&( period as ABCPeriodEdit ).EditValue!=null&&( stock as ABCSearchControl ).EditValue!=null )
            {
                Guid periodID=ABCHelper.DataConverter.ConvertToGuid( ( period as ABCPeriodEdit ).EditValue );
                Guid stockID=ABCHelper.DataConverter.ConvertToGuid( ( stock as ABCSearchControl ).EditValue );
                if ( periodID!=Guid.Empty&&stockID!=Guid.Empty )
                {
                    GEPeriodsInfo preriodInfo=new GEPeriodsController().GetObjectByID( periodID ) as GEPeriodsInfo;
                    GECompanyUnitsInfo stockInfo=new GECompanyUnitsController().GetObjectByID( stockID ) as GECompanyUnitsInfo;
                    if ( preriodInfo!=null&&stockInfo !=null)
                    {
                        ABCHelper.ABCWaitingDialog.Show( "" , String.Format( "Tính tồn kho tháng {0}/{1} {2}. . .!" , preriodInfo.Month , preriodInfo.Year , stockInfo.No ) );
                        InventoryProvider.PeriodEndingProcessing( periodID , stockID );

                        if ( this.DataManager.DataObjectsList.ContainsKey( ( stock as ABCSearchControl ).DataSource ) )
                            this.DataManager.DataObjectsList[( stock as ABCSearchControl ).DataSource].Refresh();

                        ABCHelper.ABCWaitingDialog.Close();
                    }
                }
            }
        }

    }

}
