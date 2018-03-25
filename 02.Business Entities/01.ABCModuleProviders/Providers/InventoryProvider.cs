using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCProvider.ABCSystem;
using ABCProvider.ABCData;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.Linq;
namespace ABCProvider
{
    public class InventoryProvider
    {
        #region Products
        public static List<BusinessObject> GetAllProductsList ( )
        {
            return new List<BusinessObject>();
        }
        #endregion

        #region Configurations

        public static List<ICInventoryConfigsInfo> GetConfigs ( String strTableName )
        {
            List<ICInventoryConfigsInfo> lstConfigs=new ICInventoryConfigsController().GetListByOtherField( "VoucherTableName" , strTableName ).Cast<ICInventoryConfigsInfo>().ToList();
            return lstConfigs.Where( t => !String.IsNullOrWhiteSpace( t.VoucherTableName )&&!String.IsNullOrWhiteSpace( t.QtyField )&&( !String.IsNullOrWhiteSpace( t.VoucherComUnitIDField )||!String.IsNullOrWhiteSpace( t.VoucherRealComUnitIDField ) ) ).ToList();
        }

        public static bool IsNeedCalculateInventory ( String strTableName )
        {
            object iCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM ICInventoryConfigs WHERE VoucherTableName = '{0}'" , strTableName ) );
            if ( iCount==null||iCount.GetType()!=typeof( int ) )
                return false;

            return Convert.ToInt32( iCount )>0;
        }
        public static bool IsNeedValuationCalculate ( String strTableName )
        {
            object iCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM ICInventoryConfigs WHERE VoucherTableName = '{0}' AND COGSEffective='TRUE' " , strTableName ) );
            if ( iCount==null||iCount.GetType()!=typeof( int ) )
                return false;

            return Convert.ToInt32( iCount )>0;
        }
        #endregion

        #region Inventory

        #region Get Inventory
        public static ICInvStatussInfo GetInventory ( Guid itemID )
        {
            return new ICInvStatussController().GetObjectByOtherField( "FK_MAItemID" , itemID ) as ICInvStatussInfo;
        }

        public static ICInvStatusComUnitsInfo GetInventory ( Guid companyUnitID , Guid itemID )
        {
            return new ICInvStatusComUnitsController().GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}'" , companyUnitID , itemID ) ) as ICInvStatusComUnitsInfo;
        }
        public static ICInvStatusComUnitsInfo GetInventory ( String strTableName , Guid realCompanyUnitID , Guid itemID )
        {
            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID );
            return GetInventory( obj , itemID );
        }
        public static ICInvStatusComUnitsInfo GetInventory ( BusinessObject objRealCompanyUnit , Guid itemID )
        {
            GECompanyUnitsInfo obj=CompanyUnitProvider.GetCompanyUnit( objRealCompanyUnit );
            if ( obj==null )
                return null;

            return GetInventory( obj.GECompanyUnitID , itemID );
        }

        public static ICInvStatusComUnitDetailsInfo GetInventory ( Guid companyUnitID , Guid itemID , String strSKU )
        {
            return new ICInvStatusComUnitDetailsController().GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}' AND SKU='{2}' " , companyUnitID , itemID , strSKU ) ) as ICInvStatusComUnitDetailsInfo;
        }
        public static ICInvStatusComUnitDetailsInfo GetInventory ( String strTableName , Guid realCompanyUnitID , Guid itemID , String strSKU )
        {
            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID );
            return GetInventory( obj , itemID , strSKU );
        }
        public static ICInvStatusComUnitDetailsInfo GetInventory ( BusinessObject objRealCompanyUnit , Guid itemID , String strSKU )
        {
            GECompanyUnitsInfo obj=CompanyUnitProvider.GetCompanyUnit( objRealCompanyUnit );
            if ( obj==null )
                return null;

            return GetInventory( obj.GECompanyUnitID , itemID , strSKU );
        }
        #endregion

        public static void DoInventoryAction ( List<BusinessObject> lstObjects,bool isCalcInventoryReport )
        {
            #region Init
            Dictionary<String , List<BusinessObject>> lstDictObjects=new Dictionary<string , List<BusinessObject>>();
            foreach ( BusinessObject obj in lstObjects )
            {
                if ( lstDictObjects.ContainsKey( obj.AATableName )==false )
                    lstDictObjects.Add( obj.AATableName , new List<BusinessObject>() { obj } );
                else
                    lstDictObjects[obj.AATableName].Add( obj );
            }
            #endregion

            Dictionary<Guid , List<Guid>> lstProductIDs=new Dictionary<Guid , List<Guid>>();

            CriteriaToExpressionConverter converter=new CriteriaToExpressionConverter();
            foreach ( String strTableName in lstDictObjects.Keys )
            {
                if ( IsNeedCalculateInventory( strTableName ) )
                {
                    #region Calculate Inventory

                    foreach ( ICInventoryConfigsInfo config in GetConfigs( strTableName ) )
                    {
                        if ( !config.IsShipment||!config.IsLost||!config.IsReceive )
                            continue;

                        DevExpress.Data.Filtering.CriteriaOperator operatorConvert=DevExpress.Data.Filtering.CriteriaOperator.Parse( config.VoucherConditionString );
                        List<BusinessObject> lstFilteredObjects=((IQueryable<BusinessObject>)lstDictObjects[strTableName].AsQueryable().AppendWhere( converter , operatorConvert )).ToList();
                        foreach ( BusinessObject obj in lstFilteredObjects )
                        {

                            #region companyUnitID

                            Guid companyUnitID=Guid.Empty;
                            if ( !String.IsNullOrWhiteSpace( config.VoucherComUnitIDField ) )
                            {
                                companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.VoucherComUnitIDField ) );
                            }
                            else if ( !String.IsNullOrWhiteSpace( config.VoucherRealComUnitIDField )&&!String.IsNullOrWhiteSpace( config.VoucherTableName ) )
                            {
                                companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.VoucherRealComUnitIDField ) );
                                companyUnitID=CompanyUnitProvider.GetCompanyUnitID( config.VoucherTableName , companyUnitID );
                            }
                            if ( companyUnitID==Guid.Empty )
                                continue;
                            #endregion

                            Guid productID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.ProductIDField ) );
                            String strSKU=ABCDynamicInvoker.GetValue( obj , config.SKUField ).ToString();
                            double qty=Convert.ToDouble( ABCDynamicInvoker.GetValue( obj , config.QtyField ) );
                            DateTime date=Convert.ToDateTime( ABCDynamicInvoker.GetValue( obj , config.DateField ) );

                            if ( productID!=Guid.Empty&&qty!=0&&date!=null&&date!=DateTime.MinValue&&date!=DateTime.MaxValue )
                            {
                                if ( config.IsShipment||config.IsLost )
                                    DoShipment( companyUnitID , productID , strSKU , qty , date );
                                else if ( config.IsReceive )
                                    DoReceipt( companyUnitID , productID , strSKU , qty , date );

                                if ( lstProductIDs.ContainsKey( companyUnitID )==false )
                                    lstProductIDs.Add( companyUnitID , new List<Guid>() { productID } );
                                else
                                    lstProductIDs[companyUnitID].Add( productID );
                            }
                        }
                    }
                    #endregion
                }
            }

            if ( isCalcInventoryReport )
            {
                //foreach ( Guid comUnitID in lstProductIDs.Keys )
                //{
                //    foreach(Guid productID in lstProductIDs[comUnitID])
                //        InventoryReportCalculate(comUnitID,productID,
                //}
            }
        }

        public static void DoShipment ( Guid companyUnitID , Guid itemID , String strSKU , double qty , DateTime date )
        {
            double valuation=GetValuation( itemID , date.Year , date.Month );

            #region CompanyUnit Detail

            ICInvStatusComUnitDetailsController detailCtrl=new ICInvStatusComUnitDetailsController();
            ICInvStatusComUnitDetailsInfo detail=detailCtrl.GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}' AND SKU='{2}' " , companyUnitID , itemID , strSKU ) ) as ICInvStatusComUnitDetailsInfo;
            if ( detail==null )
            {
                detail=new ICInvStatusComUnitDetailsInfo();
                detail.FK_GECompanyUnitID=companyUnitID;
                detail.FK_MAItemID=itemID;
                detail.SKUCode=strSKU;
                detailCtrl.CreateObject( detail );
            }

            detail.Qty-=qty;
            detail.UnitCost=valuation;
            detail.COGS=detail.UnitCost*detail.Qty;
            detailCtrl.UpdateObject( detail );
            #endregion

            #region CompanyUnit
            ICInvStatusComUnitsController comUnitCtrl=new ICInvStatusComUnitsController();
            ICInvStatusComUnitsInfo comUnit=comUnitCtrl.GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}' " , companyUnitID , itemID ) ) as ICInvStatusComUnitsInfo;
            if ( comUnit==null )
            {
                comUnit=new ICInvStatusComUnitsInfo();
                comUnit.FK_GECompanyUnitID=companyUnitID;
                comUnit.FK_MAItemID=itemID;
                comUnitCtrl.CreateObject( comUnit );
            }

            comUnit.Qty-=qty;
            comUnit.UnitCost=valuation;
            comUnit.COGS=comUnit.UnitCost*comUnit.Qty;
            comUnit.ShipmentCommitedQty=GetShipmentCommitedQty( companyUnitID , itemID );
            comUnit.ReceiptOrderedQty=GetReceiptOrderedQty( companyUnitID , itemID );
            comUnit.EstAvailableQty=comUnit.Qty+comUnit.ReceiptOrderedQty-comUnit.ShipmentCommitedQty;
            comUnitCtrl.UpdateObject( comUnit );
            #endregion

            #region Company
            ICInvStatussController companyCtrl=new ICInvStatussController();
            ICInvStatussInfo company=companyCtrl.GetObjectByCondition( String.Format( "FK_MAItemID='{0}' " , itemID ) ) as ICInvStatussInfo;
            if ( company==null )
            {
                company=new ICInvStatussInfo();
                company.FK_MAItemID=itemID;
                companyCtrl.CreateObject( company );
            }

            company.Qty-=qty;
            company.UnitCost=valuation;
            company.COGS=company.UnitCost*company.Qty;
            company.ShipmentCommitedQty=GetShipmentCommitedQty( itemID );
            company.ReceiptOrderedQty=GetReceiptOrderedQty( itemID );
            company.EstAvailableQty=comUnit.Qty+comUnit.ReceiptOrderedQty-comUnit.ShipmentCommitedQty;
            companyCtrl.UpdateObject( company );
            #endregion

        }
        public static void DoShipment ( String strTableName , Guid realCompanyUnitID , Guid itemID , String strSKU , double qty , DateTime date )
        {
            GECompanyUnitsInfo obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID ) as GECompanyUnitsInfo;
            if ( obj!=null )
                DoShipment( obj.GECompanyUnitID , itemID , strSKU , qty , date );
        }
        
        public static void DoReceipt ( Guid companyUnitID , Guid itemID , String strSKU , double qty , DateTime date )
        {
            double valuation=GetValuation( itemID , date.Year , date.Month );

            #region CompanyUnit Detail

            ICInvStatusComUnitDetailsController detailCtrl=new ICInvStatusComUnitDetailsController();
            ICInvStatusComUnitDetailsInfo detail=detailCtrl.GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}' AND SKU='{2}' " , companyUnitID , itemID , strSKU ) ) as ICInvStatusComUnitDetailsInfo;
            if ( detail==null )
            {
                detail=new ICInvStatusComUnitDetailsInfo();
                detail.FK_GECompanyUnitID=companyUnitID;
                detail.FK_MAItemID=itemID;
                detail.SKUCode=strSKU;
                detailCtrl.CreateObject( detail );
            }

            detail.Qty+=qty;
            detail.UnitCost=valuation;
            detail.COGS=detail.UnitCost*detail.Qty;
            detailCtrl.UpdateObject( detail );
            #endregion

            #region CompanyUnit
            ICInvStatusComUnitsController comUnitCtrl=new ICInvStatusComUnitsController();
            ICInvStatusComUnitsInfo comUnit=comUnitCtrl.GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND FK_MAItemID='{1}' " , companyUnitID , itemID ) ) as ICInvStatusComUnitsInfo;
            if ( comUnit==null )
            {
                comUnit=new ICInvStatusComUnitsInfo();
                comUnit.FK_GECompanyUnitID=companyUnitID;
                comUnit.FK_MAItemID=itemID;
                comUnitCtrl.CreateObject( comUnit );
            }

            comUnit.Qty+=qty;
            comUnit.UnitCost=valuation;
            comUnit.COGS=comUnit.UnitCost*comUnit.Qty;
            comUnit.ShipmentCommitedQty=GetShipmentCommitedQty( companyUnitID , itemID );
            comUnit.ReceiptOrderedQty=GetReceiptOrderedQty( companyUnitID , itemID );
            comUnit.EstAvailableQty=comUnit.Qty+comUnit.ReceiptOrderedQty-comUnit.ShipmentCommitedQty;
            comUnitCtrl.UpdateObject( comUnit );
            #endregion

            #region Company
            ICInvStatussController companyCtrl=new ICInvStatussController();
            ICInvStatussInfo company=companyCtrl.GetObjectByCondition( String.Format( "FK_MAItemID='{0}' " , itemID ) ) as ICInvStatussInfo;
            if ( company==null )
            {
                company=new ICInvStatussInfo();
                company.FK_MAItemID=itemID;
                companyCtrl.CreateObject( company );
            }

            company.Qty+=qty;
            company.UnitCost=valuation;
            company.COGS=company.UnitCost*company.Qty;
            company.ShipmentCommitedQty=GetShipmentCommitedQty( itemID );
            company.ReceiptOrderedQty=GetReceiptOrderedQty( itemID );
            company.EstAvailableQty=comUnit.Qty+comUnit.ReceiptOrderedQty-comUnit.ShipmentCommitedQty;
            companyCtrl.UpdateObject( company );
            #endregion

        }
        public static void DoReceipt ( String strTableName , Guid realCompanyUnitID , Guid itemID , String strSKU , double qty , DateTime date )
        {
            GECompanyUnitsInfo obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID ) as GECompanyUnitsInfo;
            if ( obj!=null )
                DoReceipt( obj.GECompanyUnitID , itemID , strSKU , qty , date );
        }

        public static double GetShipmentCommitedQty ( Guid itemID )
        {
            return 0;
        }
        public static double GetShipmentCommitedQty ( Guid companyUnitID , Guid itemID )
        {
            return 0;
        }
        public static double GetShipmentCommitedQty ( String strTableName , Guid realCompanyUnitID , Guid itemID )
        {
            GECompanyUnitsInfo obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID ) as GECompanyUnitsInfo;
            if ( obj!=null )
                return GetShipmentCommitedQty( obj.GECompanyUnitID , itemID );
            return 0;
        }

        public static double GetReceiptOrderedQty ( Guid itemID )
        {
            return 0;
        }
        public static double GetReceiptOrderedQty ( Guid companyUnitID , Guid itemID )
        {
            return 0;
        }
        public static double GetReceiptOrderedQty ( String strTableName , Guid realCompanyUnitID , Guid itemID )
        {
            GECompanyUnitsInfo obj=BusinessObjectHelper.GetBusinessObject( strTableName , realCompanyUnitID ) as GECompanyUnitsInfo;
            if ( obj!=null )
                return GetReceiptOrderedQty( obj.GECompanyUnitID , itemID );
            return 0;
        }
        #endregion

        #region Valuation (COGS)
        public static double ValuationCalculate ( Guid itemID , int year , int month )
        {
            return ValuationCalculate( itemID , PeriodProvider.GetPeriod( year , month ) );
        }
        public static double ValuationCalculate ( Guid itemID , Guid periodID )
        {
            return 0;
        }

        public static double GetValuation ( Guid itemID , int year , int month )
        {
            return GetValuation( itemID , PeriodProvider.GetPeriod( year , month ) );
        }
        public static double GetValuation ( Guid itemID , Guid periodID )
        {
            return 0;
        }
        #endregion

        public static void InventoryReportCalculate ( Guid itemID , int year , int month )
        {
            InventoryReportCalculate( itemID , PeriodProvider.GetPeriod( year , month ) );
        }
        public static void InventoryReportCalculate ( Guid itemID , Guid periodID )
        {
            
        }
        public static void InventoryReportCalculate ( Guid companyUnitID , Guid itemID , int year , int month )
        {
            InventoryReportCalculate( companyUnitID , itemID , PeriodProvider.GetPeriod( year , month ) );
        }
        public static void InventoryReportCalculate ( Guid companyUnitID , Guid itemID , Guid periodID )
        {
         
        }

    }


}
