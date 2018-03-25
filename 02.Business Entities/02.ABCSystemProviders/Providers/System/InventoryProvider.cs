using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCProvider;
using ABCProvider;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.Linq;
namespace ABCProvider
{

    public class InventoryProvider
    {
        #region Products
        static Dictionary<String , List<BusinessObject>> InventoryItems=new Dictionary<string , List<BusinessObject>>();
        public static List<BusinessObject> GetInventoryItems ( String strItemTableName )
        {
            if ( !DataStructureProvider.IsExistedTable( strItemTableName ) )
                return new List<BusinessObject>();

            if ( InventoryItems.ContainsKey( strItemTableName ) )
                return InventoryItems[strItemTableName];

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strItemTableName );
            if ( ctrl!=null )
            {
                if ( DataStructureProvider.IsTableColumn( strItemTableName , "SaveInStock" ) )
                    InventoryItems.Add( strItemTableName , ctrl.GetListByColumn( "SaveInStock" , true ).ToList() );
                else
                    InventoryItems.Add( strItemTableName , ctrl.GetListAllObjects() );
            }
            else
            {
                InventoryItems.Add( strItemTableName , new List<BusinessObject>() );
            }

            return InventoryItems[strItemTableName];
        }
   
        public static List<Guid> GetInventoryItemIDs ( String strItemTableName )
        {
            return GetInventoryItems( strItemTableName ).Select( t => t.GetID() ).ToList();
        }
        
        public static List<Guid> GetListInventoryUnits ( String strItemTableName , Guid itemID )
        {
            List<Guid> lstInvIDs=new List<Guid>();

            if ( strItemTableName=="MAItems" )
            {
                #region MAItems

                MAItemsInfo itemInfo=new MAItemsController().GetObjectByID( itemID ) as MAItemsInfo;
                if ( itemInfo==null||!itemInfo.SaveInStock )
                    return lstInvIDs;

                if ( !itemInfo.FK_ICInvUnitID.HasValue&&!itemInfo.FK_ICInvUnitID_2.HasValue&&!itemInfo.FK_ICInvUnitID_3.HasValue )
                {
                    ICInvUnitsInfo unit=new ICInvUnitsController().GetObjectByColumn( "IsDefault" , true ) as ICInvUnitsInfo;
                    if ( unit!=null )
                        lstInvIDs.Add( unit.ICInvUnitID );
                }
                else
                {
                    if ( itemInfo.FK_ICInvUnitID.HasValue )
                        lstInvIDs.Add( itemInfo.FK_ICInvUnitID.Value );

                    if ( itemInfo.FK_ICInvUnitID_2.HasValue )
                        lstInvIDs.Add( itemInfo.FK_ICInvUnitID_2.Value );

                    if ( itemInfo.FK_ICInvUnitID_3.HasValue )
                        lstInvIDs.Add( itemInfo.FK_ICInvUnitID_3.Value );
                }
                #endregion
            }
            else
            {
                #region Other Table

                BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strItemTableName );
                if ( ctrl==null )
                    return lstInvIDs;

                BusinessObject itemInfo=ctrl.GetObjectByID( itemID );
                if ( itemInfo==null )
                    return lstInvIDs;

                if ( DataStructureProvider.IsTableColumn( strItemTableName , "SaveInStock" ) )
                {
                    object objValue=ABCDynamicInvoker.GetValue( itemInfo , "SaveInStock" );
                    if ( objValue==null||objValue==DBNull.Value||Convert.ToBoolean( objValue )==false )
                        return lstInvIDs;

                    String strFK_ICInvUnit=DataStructureProvider.GetForeignKeyOfTableName( strItemTableName , "ICInvUnits" );
                    if ( !String.IsNullOrWhiteSpace( strFK_ICInvUnit ) )
                    {
                        objValue=ABCDynamicInvoker.GetValue( itemInfo , strFK_ICInvUnit );
                        if ( objValue==null||objValue==DBNull.Value||ABCHelper.DataConverter.ConvertToGuid( objValue )==Guid.Empty )
                            return lstInvIDs;

                        lstInvIDs.Add( ABCHelper.DataConverter.ConvertToGuid( objValue ) );
                    }
                    else
                    {
                        ICInvUnitsInfo unit=new ICInvUnitsController().GetObjectByColumn( "IsDefault" , true ) as ICInvUnitsInfo;
                        if ( unit!=null )
                            lstInvIDs.Add( unit.ICInvUnitID );
                    }
                }
                #endregion
            }
            return lstInvIDs;
        }
        #endregion

        #region Configs
        public enum InventoryConfigType
        {
            None=0 ,
            Shipment=1 ,
            Receipt=2 ,
            Lost=4 ,
            ShipmentCommitment=8 ,
            ReceiptOrder=16 ,
            COGSEffective=32 ,
            CompanyQtyEffective=64

        }
        public static List<ICInventoryConfigsInfo> GetInventoryConfigs ( String strVoucherItemTableName , String strItemTableName , Guid invUnitID )
        {
            ICInventoryConfigsController ctrl=new ICInventoryConfigsController();
            List<ICInventoryConfigsInfo> lstConfigs=new List<ICInventoryConfigsInfo>();

            if ( !String.IsNullOrWhiteSpace( strItemTableName ) )
            {
                if ( invUnitID==null||invUnitID==Guid.Empty )
                    lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " TableName ='{0}' AND ItemTableName ='{1}'" , strVoucherItemTableName , strItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                else
                    lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " TableName ='{0}' AND ItemTableName ='{1}' AND FK_ICInvUnitID = '{2}'" , strVoucherItemTableName , strItemTableName , invUnitID ) ).Cast<ICInventoryConfigsInfo>().ToList() );
            }
            else
            {
                if ( invUnitID==null||invUnitID==Guid.Empty )
                    lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " TableName ='{0}'" , strVoucherItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                else
                    lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " TableName ='{0}' AND FK_ICInvUnitID = '{1}'" , strVoucherItemTableName , invUnitID ) ).Cast<ICInventoryConfigsInfo>().ToList() );
            }

            foreach ( GEVoucherItemsInfo voucherConfigItem in VoucherProvider.GetConfigItems( "" , strVoucherItemTableName ) )
            {
                if ( !String.IsNullOrWhiteSpace( strItemTableName ) )
                {
                    if ( invUnitID==null||invUnitID==Guid.Empty )
                        lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " FK_GEVoucherItemID ='{0}' AND ItemTableName ='{1}'" , voucherConfigItem.GetID() , strItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                    else
                        lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " FK_GEVoucherItemID ='{0}' AND ItemTableName ='{1}' AND FK_ICInvUnitID = '{2}'" , voucherConfigItem.GetID() , strItemTableName , invUnitID ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                }
                else
                {
                    if ( invUnitID==null||invUnitID==Guid.Empty )
                        lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " FK_GEVoucherItemID ='{0}'" , voucherConfigItem.GetID() ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                    else
                        lstConfigs.AddRange( ctrl.GetListByCondition( String.Format( " FK_GEVoucherItemID ='{0}' AND FK_ICInvUnitID = '{1}'" , voucherConfigItem.GetID() , invUnitID ) ).Cast<ICInventoryConfigsInfo>().ToList() );

                }
            }
            lstConfigs=lstConfigs.Where( t => !String.IsNullOrWhiteSpace( t.TableName )&&!String.IsNullOrWhiteSpace( t.QtyField )&&( !String.IsNullOrWhiteSpace( t.ComUnitIDField )||!String.IsNullOrWhiteSpace( t.RealComUnitIDField ) ) ).ToList();

            Dictionary<Guid , ICInventoryConfigsInfo> lstResults=new Dictionary<Guid , ICInventoryConfigsInfo>();
            foreach ( ICInventoryConfigsInfo config in lstConfigs )
                if ( lstResults.ContainsKey( config.ICInventoryConfigID )==false )
                    lstResults.Add( config.ICInventoryConfigID , config );

            return lstResults.Values.ToList();
        }
        public static List<ICInventoryConfigsInfo> GetInventoryConfigs ( String strVoucherItemTableName , InventoryConfigType type , String strItemTableName , Guid invUnitID )
        {
            List<ICInventoryConfigsInfo> lstConfigs=GetInventoryConfigs( strVoucherItemTableName , strItemTableName , invUnitID );

            if ( ( type&InventoryConfigType.Shipment )==InventoryConfigType.Shipment )
                lstConfigs=lstConfigs.Where( t => t.IsShipment ).ToList();

            if ( ( type&InventoryConfigType.Receipt )==InventoryConfigType.Receipt )
                lstConfigs=lstConfigs.Where( t => t.IsReceive ).ToList();

            if ( ( type&InventoryConfigType.Lost )==InventoryConfigType.Lost )
                lstConfigs=lstConfigs.Where( t => t.IsLost ).ToList();

            if ( ( type&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                lstConfigs=lstConfigs.Where( t => t.IsShipmentCommitment ).ToList();

            if ( ( type&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder )
                lstConfigs=lstConfigs.Where( t => t.IsReceiveOrder ).ToList();

            if ( ( type&InventoryConfigType.COGSEffective )==InventoryConfigType.COGSEffective )
                lstConfigs=lstConfigs.Where( t => t.COGSEffective ).ToList();

            if ( ( type&InventoryConfigType.CompanyQtyEffective )==InventoryConfigType.CompanyQtyEffective )
                lstConfigs=lstConfigs.Where( t => t.CompanyQtyEffective ).ToList();

            return lstConfigs;

        }
        public static List<ICInventoryConfigsInfo> GetInventoryConfigs ( InventoryConfigType type , String strItemTableName , Guid invUnitID )
        {
            List<ICInventoryConfigsInfo> lstConfigs=new List<ICInventoryConfigsInfo>();
            if ( !String.IsNullOrWhiteSpace( strItemTableName ) )
            {
                if ( invUnitID==null||invUnitID==Guid.Empty )
                    lstConfigs.AddRange( new ICInventoryConfigsController().GetListByCondition( String.Format( " ItemTableName ='{0}' " , strItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                else
                {
                    lstConfigs.AddRange( new ICInventoryConfigsController().GetListByCondition( String.Format( " FK_ICInvUnitID = '{0}' AND ItemTableName ='{1}'  " , invUnitID , strItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                    if ( lstConfigs.Count<=0 )
                        lstConfigs.AddRange( new ICInventoryConfigsController().GetListByCondition( String.Format( " ItemTableName ='{0}' " , strItemTableName ) ).Cast<ICInventoryConfigsInfo>().ToList() );

                }
            }
            else
            {
                if ( invUnitID==null||invUnitID==Guid.Empty )
                    lstConfigs.AddRange( new ICInventoryConfigsController().GetListAllObjects().Cast<ICInventoryConfigsInfo>().ToList() );
                else
                {
                    lstConfigs.AddRange( new ICInventoryConfigsController().GetListByCondition( String.Format( " FK_ICInvUnitID = '{0}'" , invUnitID ) ).Cast<ICInventoryConfigsInfo>().ToList() );
                    if ( lstConfigs.Count<=0 )
                        lstConfigs.AddRange( new ICInventoryConfigsController().GetListAllObjects().Cast<ICInventoryConfigsInfo>().ToList() );
                }
            }

            lstConfigs=lstConfigs.Where( t => !String.IsNullOrWhiteSpace( t.TableName )&&DataStructureProvider.IsExistedTable( t.TableName )&&!String.IsNullOrWhiteSpace( t.QtyField )&&( !String.IsNullOrWhiteSpace( t.ComUnitIDField )||!String.IsNullOrWhiteSpace( t.RealComUnitIDField ) ) ).ToList();

            if ( ( type&InventoryConfigType.Shipment )==InventoryConfigType.Shipment )
                lstConfigs=lstConfigs.Where( t => t.IsShipment ).ToList();

            if ( ( type&InventoryConfigType.Receipt )==InventoryConfigType.Receipt )
                lstConfigs=lstConfigs.Where( t => t.IsReceive ).ToList();

            if ( ( type&InventoryConfigType.Lost )==InventoryConfigType.Lost )
                lstConfigs=lstConfigs.Where( t => t.IsLost ).ToList();

            if ( ( type&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                lstConfigs=lstConfigs.Where( t => t.IsShipmentCommitment ).ToList();

            if ( ( type&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder )
                lstConfigs=lstConfigs.Where( t => t.IsReceiveOrder ).ToList();

            if ( ( type&InventoryConfigType.COGSEffective )==InventoryConfigType.COGSEffective )
                lstConfigs=lstConfigs.Where( t => t.COGSEffective ).ToList();

            if ( ( type&InventoryConfigType.CompanyQtyEffective )==InventoryConfigType.CompanyQtyEffective )
                lstConfigs=lstConfigs.Where( t => t.CompanyQtyEffective ).ToList();

            return lstConfigs;
        }

        public static bool IsNeedCalculateInventory ( String strVoucherItemTableName )
        {
            object iCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM ICInventoryConfigs WHERE TableName = '{0}'" , strVoucherItemTableName ) );
            if ( iCount==null||iCount.GetType()!=typeof( int ) )
                return false;

            return Convert.ToInt32( iCount )>0;
        }
        public static bool IsNeedValuationCalculate ( String strVoucherItemTableName )
        {
            object iCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM ICInventoryConfigs WHERE TableName = '{0}' AND COGSEffective='TRUE' " , strVoucherItemTableName ) );
            if ( iCount==null||iCount.GetType()!=typeof( int ) )
                return false;

            return Convert.ToInt32( iCount )>0;
        }


        public static double GetQtyFromVouchers ( String strItemTableName , Guid itemID , InventoryConfigType configType , Guid periodID , Guid invUnitID )
        {
            return GetQtyFromVouchers( strItemTableName , itemID , configType , PeriodProvider.GetFirstDay( periodID ) , PeriodProvider.GetLastDay( periodID ) , invUnitID );
        }
        public static double GetQtyFromVouchers ( String strItemTableName , Guid itemID , InventoryConfigType configType , DateTime? fromDate , DateTime? toDate , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return 0;

            int iCount=0;
            String strQueryTot=String.Empty;
            foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( configType , strItemTableName , invUnitID ) )
            {
                if ( String.IsNullOrWhiteSpace( config.QtyField )||!DataStructureProvider.IsTableColumn( config.TableName , config.QtyField ) )
                    continue;

                #region Generate Query String
                iCount++;
                String strQuery=String.Format( @"DECLARE @Qty{0} float; " , iCount )+Environment.NewLine;

                strQuery=strQuery+QueryGenerator.GenSelect( config.TableName , String.Format( @" @Qty{0} = SUM({1})" , iCount , config.QtyField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemIDField , itemID );

                if ( fromDate.HasValue&&toDate.HasValue&&fromDate.Value<toDate.Value )
                {
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , ">=" , fromDate.Value ) );
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , "<=" , toDate.Value ) );
                }
                if ( ( ( configType&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                    ||( ( configType&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder ) )
                {
                    if ( DataStructureProvider.IsTableColumn( config.TableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
                }

                strQuery=strQuery+Environment.NewLine+String.Format( @"IF(@Qty{0} IS NULL) SET @Qty{0}=0;" , iCount )+Environment.NewLine;
                strQueryTot=strQueryTot+Environment.NewLine+strQuery;
                #endregion

            }

            if ( iCount>0 )
            {
                strQueryTot=strQueryTot+Environment.NewLine+"SELECT @Qty1";
                if ( iCount>=2 )
                {
                    for ( int i=2; i<=iCount; i++ )
                        strQueryTot=strQueryTot+"+@Qty"+i;
                }

                object obj=BusinessObjectController.GetData( strQueryTot );
                if ( obj!=null&&obj!=DBNull.Value )
                    return Convert.ToDouble( obj );
            }
            return 0;
        }
        public static double GetQtyFromVouchers ( Guid companyUnitID , String strItemTableName , Guid itemID , InventoryConfigType configType , Guid periodID , Guid invUnitID )
        {
            return GetQtyFromVouchers( companyUnitID , strItemTableName , itemID , configType , PeriodProvider.GetFirstDay( periodID ) , PeriodProvider.GetLastDay( periodID ) , invUnitID );
        }
        public static double GetQtyFromVouchers ( Guid companyUnitID , String strItemTableName , Guid itemID , InventoryConfigType configType , DateTime? fromDate , DateTime? toDate , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return 0;

            Guid realComUnitID=CompanyUnitProvider.GetRealCompanyUnitID( companyUnitID );
            int iCount=0;
            String strQueryTot=String.Empty;

            foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( configType , strItemTableName , invUnitID ) )
            {
                #region Generate Query String
                iCount++;
                String strQuery=String.Format( @"DECLARE @Qty{0} float; " , iCount )+Environment.NewLine;

                strQuery=strQuery+QueryGenerator.GenSelect( config.TableName , String.Format( @" @Qty{0} = SUM({1})" , iCount , config.QtyField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemIDField , itemID );
                if ( fromDate.HasValue&&toDate.HasValue&&fromDate.Value<toDate.Value )
                {
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , ">=" , fromDate.Value ) );
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , "<=" , toDate.Value ) );
                }
                if ( DataStructureProvider.IsTableColumn( config.TableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                {
                    if ( ( ( configType&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                        ||( ( configType&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder ) )
                    {

                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
                    }
                    else
                    {
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeApproved );
                    }
                }
                if ( !String.IsNullOrWhiteSpace( config.ComUnitIDField ) )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ComUnitIDField , companyUnitID );
                if ( realComUnitID!=Guid.Empty )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , config.RealComUnitIDField , realComUnitID );

                strQuery=strQuery+Environment.NewLine+String.Format( @"IF(@Qty{0} IS NULL) SET @Qty{0}=0;" , iCount )+Environment.NewLine;
                strQueryTot=strQueryTot+Environment.NewLine+strQuery;
                #endregion

            }

            if ( iCount>0 )
            {
                strQueryTot=strQueryTot+Environment.NewLine+"SELECT @Qty1";
                if ( iCount>=2 )
                {
                    for ( int i=2; i<=iCount; i++ )
                        strQueryTot=strQueryTot+"+@Qty"+i;
                }

                object obj=BusinessObjectController.GetData( strQueryTot );
                if ( obj!=null&&obj!=DBNull.Value )
                    return Convert.ToDouble( obj );
            }
            return 0;
        }

        public static double GetAmtFromVouchers ( String strItemTableName , Guid itemID , InventoryConfigType configType , Guid periodID )
        {
            return GetAmtFromVouchers( strItemTableName , itemID , configType , PeriodProvider.GetFirstDay( periodID ) , PeriodProvider.GetLastDay( periodID ) );
        }
        public static double GetAmtFromVouchers ( String strItemTableName , Guid itemID , InventoryConfigType configType , DateTime? fromDate , DateTime? toDate )
        {
            List<String> lstTemps=new List<string>();

            int iCount=0;
            String strQueryTot=String.Empty;

            foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( configType , strItemTableName , Guid.Empty ) )
            {

                if ( String.IsNullOrWhiteSpace( config.COGSField )||!DataStructureProvider.IsTableColumn( config.TableName , config.COGSField ) )
                    continue;

                if ( lstTemps.Contains( config.TableName+config.COGSField ) )
                    continue;
                lstTemps.Add( config.TableName+config.COGSField );

                #region Generate Query String
                iCount++;
                String strQuery=String.Format( @"DECLARE @Amt{0} float; " , iCount )+Environment.NewLine;
                strQuery=strQuery+QueryGenerator.GenSelect( config.TableName , String.Format( @" @Amt{0} = SUM({1})" , iCount , config.COGSField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemIDField , itemID );
                if ( fromDate.HasValue&&toDate.HasValue&&fromDate.Value<toDate.Value )
                {
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , ">=" , fromDate.Value ) );
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , "<=" , toDate.Value ) );
                }
                if ( ( ( configType&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                    ||( ( configType&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder ) )
                {
                    if ( DataStructureProvider.IsTableColumn( config.TableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
                }

                strQuery=strQuery+Environment.NewLine+String.Format( @"IF(@Amt{0} IS NULL) SET @Amt{0}=0;" , iCount )+Environment.NewLine;
                strQueryTot=strQueryTot+Environment.NewLine+strQuery;
                #endregion
            }

            if ( iCount>0 )
            {
                strQueryTot=strQueryTot+Environment.NewLine+"SELECT @Amt1";
                if ( iCount>=2 )
                {
                    for ( int i=2; i<=iCount; i++ )
                        strQueryTot=strQueryTot+"+@Amt"+i;
                }

                object obj=BusinessObjectController.GetData( strQueryTot );
                if ( obj!=null&&obj!=DBNull.Value )
                    return Convert.ToDouble( obj );
            }
            return 0;
        }
        public static double GetAmtFromVouchers ( Guid companyUnitID , String strItemTableName , Guid itemID , InventoryConfigType configType , Guid periodID )
        {
            return GetAmtFromVouchers( companyUnitID , strItemTableName , itemID , configType , PeriodProvider.GetFirstDay( periodID ) , PeriodProvider.GetLastDay( periodID ) );
        }
        public static double GetAmtFromVouchers ( Guid companyUnitID , String strItemTableName , Guid itemID , InventoryConfigType configType , DateTime? fromDate , DateTime? toDate )
        {
            Guid realComUnitID=CompanyUnitProvider.GetRealCompanyUnitID( companyUnitID );
          
            int iCount=0;
            String strQueryTot=String.Empty;

            List<String> lstTemps=new List<string>();

            foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( configType , strItemTableName , Guid.Empty ) )
            {
                if ( String.IsNullOrWhiteSpace( config.COGSField )||!DataStructureProvider.IsTableColumn( config.TableName , config.COGSField ) )
                    continue;

                if ( lstTemps.Contains( config.TableName+config.COGSField ) )
                    continue;
                lstTemps.Add( config.TableName+config.COGSField );

                #region Generate Query String
                iCount++;
                String strQuery=String.Format( @"DECLARE @Amt{0} float; " , iCount )+Environment.NewLine;
                strQuery=strQuery+QueryGenerator.GenSelect( config.TableName , String.Format( @" @Amt{0} = SUM({1})" , iCount , config.COGSField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemIDField , itemID );
                if ( fromDate.HasValue&&toDate.HasValue&&fromDate.Value<toDate.Value )
                {
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , ">=" , fromDate.Value ) );
                    strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , "<=" , toDate.Value ) );
                }
                if ( DataStructureProvider.IsTableColumn( config.TableName , ABCCommon.ABCConstString.colApprovalStatus ) )
                {
                    if ( ( ( configType&InventoryConfigType.ShipmentCommitment )==InventoryConfigType.ShipmentCommitment )
                        ||( ( configType&InventoryConfigType.ReceiptOrder )==InventoryConfigType.ReceiptOrder ) )
                    {
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeNew );
                    }
                    else
                    {
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , ABCCommon.ABCConstString.colApprovalStatus , ABCCommon.ABCConstString.ApprovalTypeApproved );
                    }
                }

                if ( !String.IsNullOrWhiteSpace( config.ComUnitIDField ) )
                {
                    if ( DataStructureProvider.IsTableColumn( config.TableName , config.ComUnitIDField ) )
                        strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ComUnitIDField , companyUnitID );
                    else
                    {
                        //       VoucherProvider.GetConfigs( config.TableName );
                    }
                }
                if ( realComUnitID!=Guid.Empty )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , config.RealComUnitIDField , realComUnitID );


                strQuery=strQuery+Environment.NewLine+String.Format( @"IF(@Amt{0} IS NULL) SET @Amt{0}=0;" , iCount )+Environment.NewLine;
                strQueryTot=strQueryTot+Environment.NewLine+strQuery;
                #endregion
            }

            if ( iCount>0 )
            {
                strQueryTot=strQueryTot+Environment.NewLine+"SELECT @Amt1";
                if ( iCount>=2 )
                {
                    for ( int i=2; i<=iCount; i++ )
                        strQueryTot=strQueryTot+"+@Amt"+i;
                }

                object obj=BusinessObjectController.GetData( strQueryTot );
                if ( obj!=null&&obj!=DBNull.Value )
                    return Convert.ToDouble( obj );
            }
            return 0;
        }
        #endregion

        #region Inventory

        #region Get
        public static ICInvStatussInfo GetInventory ( String strItemTableName , Guid itemID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return null;

            return new ICInvStatussController().GetObjectByCondition( String.Format( " TableName ='{0}' AND ID='{1}'  AND FK_ICInvUnitID = '{2}' " , strItemTableName , itemID , invUnitID ) ) as ICInvStatussInfo;
        }

        public static ICInvStatusComUnitsInfo GetInventory ( Guid companyUnitID , String strItemTableName , Guid itemID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return null;

            return new ICInvStatusComUnitsController().GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND TableName ='{1}' AND ID='{2}'  AND FK_ICInvUnitID = '{3}'" , companyUnitID , strItemTableName , itemID , invUnitID ) ) as ICInvStatusComUnitsInfo;
        }
        public static ICInvStatusComUnitsInfo GetInventory ( String strRealComUnitTableName , Guid realCompanyUnitID , String strItemTableName , Guid itemID , Guid invUnitID )
        {
            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strRealComUnitTableName , realCompanyUnitID );
            return GetInventory( obj , strItemTableName , itemID , invUnitID );
        }
        public static ICInvStatusComUnitsInfo GetInventory ( BusinessObject objRealCompanyUnit , String strItemTableName , Guid itemID , Guid invUnitID )
        {
            GECompanyUnitsInfo obj=CompanyUnitProvider.GetCompanyUnit( objRealCompanyUnit );
            if ( obj==null )
                return null;

            return GetInventory( obj.GECompanyUnitID , strItemTableName , itemID , invUnitID );
        }

        public static ICInvStatusComUnitDetailsInfo GetInventory ( Guid companyUnitID , String strItemTableName , Guid itemID , String strSKU , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return null;

            return new ICInvStatusComUnitDetailsController().GetObjectByCondition( String.Format( " FK_GECompanyUnitID='{0}' AND  TableName ='{1}' AND ID='{2}'   AND SKUCode='{3}' AND FK_ICInvUnitID = '{4}' " , companyUnitID , strItemTableName , itemID , strSKU , invUnitID ) ) as ICInvStatusComUnitDetailsInfo;
        }
        public static ICInvStatusComUnitDetailsInfo GetInventory ( String strRealComUnitTableName , Guid realCompanyUnitID , String strItemTableName , Guid itemID , String strSKU , Guid invUnitID )
        {
            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strRealComUnitTableName , realCompanyUnitID );
            return GetInventory( obj , strItemTableName , itemID , strSKU , invUnitID );
        }
        public static ICInvStatusComUnitDetailsInfo GetInventory ( BusinessObject objRealCompanyUnit , String strItemTableName , Guid itemID , String strSKU , Guid invUnitID )
        {
            GECompanyUnitsInfo obj=CompanyUnitProvider.GetCompanyUnit( objRealCompanyUnit );
            if ( obj==null )
                return null;

            return GetInventory( obj.GECompanyUnitID , strItemTableName , itemID , strSKU , invUnitID );
        }
        #endregion

        #region Caculation

        public static void DoInventoryAction ( String strVoucherTableName , Guid voucherID , bool isRevert )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {

                foreach ( GEVoucherItemsInfo config in VoucherProvider.GetConfigItems( strVoucherTableName , "" ) )
                {
                    if ( IsNeedCalculateInventory( config.ItemTableName ) )
                    {
                        BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( config.ItemTableName );
                        DoInventoryAction( ctrl.GetListByForeignKey( config.ItemFKField , voucherID ) , isRevert );
                    }
                }
                scope.Complete();
            }
        }
        public static void DoInventoryAction ( List<BusinessObject> lstObjects , bool isRevert )
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

            GEVouchersController voucherCtrl=new GEVouchersController();
            GEVoucherItemsController voucherItemCtrl=new GEVoucherItemsController();
            Dictionary<Guid , Dictionary<String , List<Guid>>> lstItemIDs=new Dictionary<Guid , Dictionary<String , List<Guid>>>();

            CriteriaToExpressionConverter converter=new CriteriaToExpressionConverter();
            foreach ( String strVoucherItemTableName in lstDictObjects.Keys )
            {
                if ( IsNeedCalculateInventory( strVoucherItemTableName ) )
                {

                    #region Calculate Inventory

                    foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( strVoucherItemTableName , String.Empty , Guid.Empty ) )
                    {
                        if ( !config.IsShipment&&!config.IsLost&&!config.IsReceive )
                            continue;

                        #region Get GEVouchersInfo
                        GEVouchersInfo voucher=null;

                        if ( config.FK_GEVoucherItemID.HasValue&&config.FK_GEVoucherItemID.Value!=Guid.Empty )
                        {
                            GEVoucherItemsInfo voucherItemInfo=voucherItemCtrl.GetObjectByID( config.FK_GEVoucherItemID.Value ) as GEVoucherItemsInfo;
                            if ( voucherItemInfo!=null )
                                voucher=voucherCtrl.GetObjectByID( voucherItemInfo.FK_GEVoucherID.Value ) as GEVouchersInfo;
                        }
                        #endregion

                        List<BusinessObject> lstFilteredObjects=lstDictObjects[strVoucherItemTableName];
                        if ( !String.IsNullOrWhiteSpace( config.ConditionString ) )
                        {
                            DevExpress.Data.Filtering.CriteriaOperator operatorConvert=DevExpress.Data.Filtering.CriteriaOperator.TryParse( config.ConditionString );
                            if ( operatorConvert!=null )
                                lstFilteredObjects=( (IQueryable<BusinessObject>)lstDictObjects[strVoucherItemTableName].AsQueryable().AppendWhere( converter , operatorConvert ) ).ToList();
                            else
                            {

                                String strQuery=QueryGenerator.GenSelect( strVoucherItemTableName , DataStructureProvider.GetPrimaryKeyColumn( strVoucherItemTableName ) , false );
                                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                                List<Guid> lstIDs=BusinessObjectController.GetListObjects( strQuery ).Select( t => ABCHelper.DataConverter.ConvertToGuid( t ) ).ToList();
                                lstFilteredObjects=lstFilteredObjects.Where( t => lstIDs.Contains( t.GetID() ) ).ToList();
                            }
                        }

                        foreach ( BusinessObject obj in lstFilteredObjects )
                        {

                            #region companyUnitID

                            Guid companyUnitID=Guid.Empty;
                            if ( !String.IsNullOrWhiteSpace( config.ComUnitIDField ) )
                            {
                                if ( DataStructureProvider.IsTableColumn( obj.AATableName , config.ComUnitIDField ) )
                                    companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.ComUnitIDField ) );
                                else if ( voucher!=null )
                                {
                                    Guid realVoucherID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetForeignKeyOfTableName( obj.AATableName , voucher.TableName ) ) );
                                    BusinessObject realVoucher=BusinessControllerFactory.GetBusinessController( voucher.TableName ).GetObjectByID( realVoucherID );

                                    if ( DataStructureProvider.IsTableColumn( realVoucher.AATableName , config.ComUnitIDField ) )
                                        companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( realVoucher , config.ComUnitIDField ) );
                                }
                            }

                            if ( companyUnitID==Guid.Empty )
                            {
                                if ( !String.IsNullOrWhiteSpace( config.RealComUnitIDField )&&!String.IsNullOrWhiteSpace( config.RealComUnitTableName ) )
                                {
                                    if ( DataStructureProvider.IsTableColumn( obj.AATableName , config.RealComUnitIDField ) )
                                    {
                                        companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.RealComUnitIDField ) );
                                        companyUnitID=CompanyUnitProvider.GetCompanyUnitID( config.RealComUnitTableName , companyUnitID );
                                    }
                                    else if ( voucher!=null )
                                    {
                                        Guid realVoucherID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , DataStructureProvider.GetForeignKeyOfTableName( obj.AATableName , voucher.TableName ) ) );
                                        BusinessObject realVoucher=BusinessControllerFactory.GetBusinessController( voucher.TableName ).GetObjectByID( realVoucherID );

                                        if ( DataStructureProvider.IsTableColumn( realVoucher.AATableName , config.ComUnitIDField ) )
                                        {
                                            companyUnitID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( realVoucher , config.RealComUnitIDField ) );
                                            companyUnitID=CompanyUnitProvider.GetCompanyUnitID( config.RealComUnitTableName , companyUnitID );
                                        }
                                    }
                                }
                            }

                            if ( companyUnitID==Guid.Empty )
                                continue;
                            #endregion

                            Guid itemID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , config.ItemIDField ) );
                            String strSKU=ABCDynamicInvoker.GetValue( obj , config.SKUField ).ToString();
                            double qty=Convert.ToDouble( ABCDynamicInvoker.GetValue( obj , config.QtyField ) );
                            DateTime date=Convert.ToDateTime( ABCDynamicInvoker.GetValue( obj , config.DateField ) );

                            if ( itemID!=Guid.Empty&&qty!=0&&date!=null&&date!=DateTime.MinValue&&date!=DateTime.MaxValue )
                            {
                                if ( !isRevert )
                                {
                                    if ( config.IsShipment||config.IsLost )
                                        DoShipment( companyUnitID , config.ItemTableName , itemID , strSKU , qty , date );
                                    else if ( config.IsReceive )
                                        DoReceipt( companyUnitID , config.ItemTableName , itemID , strSKU , qty , date );
                                }
                                else
                                {
                                    if ( config.IsShipment||config.IsLost )
                                        DoReceipt( companyUnitID , config.ItemTableName , itemID , strSKU , qty , date );
                                    else if ( config.IsReceive )
                                        DoShipment( companyUnitID , config.ItemTableName , itemID , strSKU , qty , date );
                                }

                                if ( lstItemIDs.ContainsKey( companyUnitID )==false )
                                    lstItemIDs.Add( companyUnitID , new Dictionary<String , List<Guid>>() );

                                if ( lstItemIDs[companyUnitID].ContainsKey( config.ItemTableName )==false )
                                    lstItemIDs[companyUnitID].Add( config.ItemTableName , new List<Guid>() );

                                if ( lstItemIDs[companyUnitID][config.ItemTableName].Contains( itemID )==false )
                                    lstItemIDs[companyUnitID][config.ItemTableName].Add( itemID );

                            }
                        }
                    }
                    #endregion
                }
            }


        }
        public static void DoShipment ( Guid companyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date )
        {
            foreach ( Guid invID in GetListInventoryUnits( strItemTableName , itemID ) )
                DoShipment( companyUnitID , strItemTableName , itemID , strSKU , qty , date , invID );
        }


        public static void DoShipment ( Guid companyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return;

            double valuation=GetValuation( strItemTableName , itemID , date.Year , date.Month , invUnitID );
            String strQuery=String.Empty;

            #region Company
            ICInvStatussController companyCtrl=new ICInvStatussController();
            strQuery=String.Format( " TableName ='{0}' AND ID='{1}'  AND FK_ICInvUnitID = '{2}'" , strItemTableName , itemID , invUnitID );
            ICInvStatussInfo company=companyCtrl.GetObjectByCondition( strQuery ) as ICInvStatussInfo;
            if ( company==null )
            {
                company=new ICInvStatussInfo();
                company.ID=itemID;
                company.TableName=strItemTableName;
                if ( invUnitID!=null&&invUnitID!=Guid.Empty )
                    company.FK_ICInvUnitID=invUnitID;
                companyCtrl.CreateObject( company );
            }

            company.Qty-=qty;
            company.UnitCost=valuation;
            company.COGS=company.UnitCost*company.Qty;
            company.ShipmentCommitedQty=GetQtyFromVouchers( strItemTableName , itemID , InventoryConfigType.ShipmentCommitment , null , null , invUnitID );
            company.ReceiptOrderedQty=GetQtyFromVouchers( strItemTableName , itemID , InventoryConfigType.ReceiptOrder , null , null , invUnitID );
            company.EstAvailableQty=company.Qty+company.ReceiptOrderedQty-company.ShipmentCommitedQty;
            companyCtrl.UpdateObject( company );
            #endregion

            #region CompanyUnit
            ICInvStatusComUnitsController comUnitCtrl=new ICInvStatusComUnitsController();
            strQuery=String.Format( " FK_GECompanyUnitID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_ICInvUnitID = '{3}'" , companyUnitID , strItemTableName , itemID , invUnitID );
            ICInvStatusComUnitsInfo comUnit=comUnitCtrl.GetObjectByCondition( strQuery ) as ICInvStatusComUnitsInfo;
            if ( comUnit==null )
            {
                comUnit=new ICInvStatusComUnitsInfo();
                comUnit.FK_ICInvStatusID=company.ICInvStatusID;
                comUnit.FK_GECompanyUnitID=companyUnitID;
                comUnit.ID=itemID;
                comUnit.TableName=strItemTableName;
                if ( invUnitID!=null&&invUnitID!=Guid.Empty )
                    comUnit.FK_ICInvUnitID=invUnitID;
                comUnitCtrl.CreateObject( comUnit );
            }

            comUnit.Qty-=qty;
            comUnit.UnitCost=valuation;
            comUnit.COGS=comUnit.UnitCost*comUnit.Qty;
            comUnit.ShipmentCommitedQty=GetQtyFromVouchers( companyUnitID , strItemTableName , itemID , InventoryConfigType.ShipmentCommitment , null , null , invUnitID );
            comUnit.ReceiptOrderedQty=GetQtyFromVouchers( companyUnitID , strItemTableName , itemID , InventoryConfigType.ReceiptOrder , null , null , invUnitID );
            comUnit.EstAvailableQty=comUnit.Qty+comUnit.ReceiptOrderedQty-comUnit.ShipmentCommitedQty;
            comUnitCtrl.UpdateObject( comUnit );
            #endregion

            #region CompanyUnit Detail

            ICInvStatusComUnitDetailsController detailCtrl=new ICInvStatusComUnitDetailsController();
            strQuery=String.Format( " FK_GECompanyUnitID='{0}' AND TableName ='{1}' AND ID='{2}' AND SKUCode='{3}' AND FK_ICInvUnitID = '{4}'" , companyUnitID , strItemTableName , itemID , strSKU , invUnitID );
            ICInvStatusComUnitDetailsInfo detail=detailCtrl.GetObjectByCondition( strQuery ) as ICInvStatusComUnitDetailsInfo;
            if ( detail==null )
            {
                detail=new ICInvStatusComUnitDetailsInfo();
                detail.FK_ICInvStatusID=company.ICInvStatusID;
                detail.FK_ICInvStatusComUnitID=comUnit.ICInvStatusComUnitID;
                detail.FK_GECompanyUnitID=companyUnitID;
                detail.ID=itemID;
                detail.TableName=strItemTableName;
                detail.SKUCode=strSKU;
                if ( invUnitID!=null&&invUnitID!=Guid.Empty )
                    detail.FK_ICInvUnitID=invUnitID;
                detailCtrl.CreateObject( detail );
            }

            detail.Qty-=qty;
            detail.UnitCost=valuation;
            detail.COGS=detail.UnitCost*detail.Qty;
            detailCtrl.UpdateObject( detail );
            #endregion

          

      

        }
        public static void DoShipment ( String strRealComUnitTableName , Guid realCompanyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date , Guid invUnitID )
        {
            GECompanyUnitsInfo obj=BusinessObjectHelper.GetBusinessObject( strRealComUnitTableName , realCompanyUnitID ) as GECompanyUnitsInfo;
            if ( obj!=null )
                DoShipment( obj.GECompanyUnitID , strItemTableName , itemID , strSKU , qty , date , invUnitID );
        }

        public static void DoReceipt ( Guid companyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date )
        {
            DoShipment( companyUnitID , strItemTableName , itemID , strSKU , -qty , date );
        }
        public static void DoReceipt ( Guid companyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date , Guid invUnitID )
        {
            DoShipment( companyUnitID , strItemTableName , itemID , strSKU , -qty , date , invUnitID );
        }
        public static void DoReceipt ( String strRealComUnitTableName , Guid realCompanyUnitID , String strItemTableName , Guid itemID , String strSKU , double qty , DateTime date , Guid invUnitID )
        {
            DoShipment( strRealComUnitTableName , realCompanyUnitID , strItemTableName , itemID , strSKU , -qty , date , invUnitID );
        }

        #endregion

        #endregion

        #region Valuation (COGS)

        public static double GetValuation ( String strItemTableName , Guid itemID , int year , int month , Guid invUnitID )
        {
            return GetValuation( strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) , invUnitID );
        }
        public static double GetValuation ( String strItemTableName , Guid itemID , Guid periodID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return 0;

            ICInvReportsController invCtrl=new ICInvReportsController();
            String strQuery=String.Format( " FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_ICInvUnitID = '{3}'" , periodID , strItemTableName , itemID , invUnitID );
            ICInvReportsInfo report=invCtrl.GetObjectByCondition( strQuery ) as ICInvReportsInfo;
            if ( report!=null )
                return report.COGSUnit;

            List<Guid> lstPeriods=new List<Guid>();
            for ( int i=1; i<=10; i++ )
            {
                Guid id=PeriodProvider.GetPeriod( periodID , -i );
                strQuery=String.Format( " FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_ICInvUnitID = '{3}'" , id , strItemTableName , itemID , invUnitID );
                report=invCtrl.GetObjectByCondition( strQuery ) as ICInvReportsInfo;
                if ( report==null )
                    lstPeriods.Add( id );
                else
                    break;
            }
            if ( lstPeriods.Count==10 )
                return 0;

            for ( int i=lstPeriods.Count-1; i>=0; i-- )
                ReportCalculation( strItemTableName , itemID , lstPeriods[i] , invUnitID );

            ReportCalculation( strItemTableName , itemID , periodID , invUnitID );

            strQuery=String.Format( " FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_ICInvUnitID = '{3}'" , periodID , strItemTableName , itemID , invUnitID );
            report=invCtrl.GetObjectByCondition( strQuery ) as ICInvReportsInfo;
            if ( report!=null )
                return report.COGSUnit;

            return 0;
        }

        public static void UpdateValuationToVouchers ( String strItemTableName , Guid itemID , int year , int month )
        {
            UpdateValuationToVouchers( strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) );
        }
        public static void UpdateValuationToVouchers ( String strItemTableName , Guid itemID , Guid periodID )
        {
            DateTime fromDate=PeriodProvider.GetFirstDay( periodID );
            DateTime toDate=PeriodProvider.GetLastDay( periodID );

            Dictionary<Guid , BusinessObject> lstVoucherItems=new Dictionary<Guid , BusinessObject>();

            foreach ( Guid invID in GetListInventoryUnits( strItemTableName , itemID ) )
            {
                double unitCost=GetValuation( strItemTableName , itemID , periodID , invID );

                foreach ( ICInventoryConfigsInfo config in GetInventoryConfigs( InventoryConfigType.Shipment , strItemTableName , invID ) )
                {
                    #region Generate Query String
                    String strQuery=QueryGenerator.GenSelectAll( config.TableName , false );
                    strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , config.ItemIDField , itemID );
                    if ( fromDate<toDate )
                    {
                        strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , ">=" , fromDate ) );
                        strQuery=QueryGenerator.AddCondition( strQuery , TimeProvider.GenCompareDateTime( config.DateField , "<=" , toDate ) );
                    }

                    #endregion

                    BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( config.TableName );
                    foreach ( BusinessObject obj in ctrl.GetList( strQuery ) )
                    {
                        double qty=Convert.ToDouble( ABCDynamicInvoker.GetValue( obj , config.QtyField ) );
                        double amt=Convert.ToDouble( ABCDynamicInvoker.GetValue( obj , config.COGSField ) );
                        if ( amt!=qty*unitCost )
                        {
                            ABCDynamicInvoker.SetValue( obj , config.COGSField , qty*unitCost );
                            ctrl.UpdateObject( obj );
                            if ( !lstVoucherItems.ContainsKey( obj.GetID() ) )
                                lstVoucherItems.Add( obj.GetID() , obj );
                        }
                    }
                }
            }


            Dictionary<Guid , BusinessObject> lstVouchers=new Dictionary<Guid , BusinessObject>();
            foreach ( Guid id in lstVoucherItems.Keys )
            {
                foreach ( BusinessObject voucher in VoucherProvider.GetVouchersByItemID( lstVoucherItems[id].AATableName , id ) )
                {
                    if ( lstVouchers.ContainsKey( voucher.GetID() )==false )
                        lstVouchers.Add( voucher.GetID() , voucher );
                }
            }

            foreach ( BusinessObject voucher in lstVouchers.Values )
                VoucherProvider.ReCalculateVoucher( voucher );
        }
        #endregion

        #region Report

        #region Calculation

        public static List<ICInvReportsInfo> ReportCalculation ( String strItemTableName , Guid itemID , int year , int month , Guid invUnitID )
        {
            return ReportCalculation( strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) , invUnitID );
        }
        public static List<ICInvReportsInfo> ReportCalculation ( String strItemTableName , Guid itemID , DateTime date , Guid invUnitID )
        {
            return ReportCalculation( strItemTableName , itemID , PeriodProvider.GetPeriod( date.Year , date.Month ) , invUnitID );
        }
        public static List<ICInvReportsInfo> ReportCalculation ( String strItemTableName , Guid itemID , Guid periodID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return new List<ICInvReportsInfo>();

            #region Init
            DateTime date=PeriodProvider.GetFirstDay( periodID );

            List<Guid> lstItemIDs=new List<Guid>();

            if ( itemID==Guid.Empty||itemID==null )
                lstItemIDs.AddRange( GetInventoryItemIDs( strItemTableName ) );
            else
                lstItemIDs.Add( itemID );
            #endregion

            #region Report Calculation
            ICInvReportsController reportCtrl=new ICInvReportsController();
            List<ICInvReportsInfo> lstReports=new List<ICInvReportsInfo>();
            foreach ( Guid iID in lstItemIDs )
            {
                String strQuery=String.Format( " FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}'  AND FK_ICInvUnitID = '{3}'" , periodID , strItemTableName , iID , invUnitID );
                ICInvReportsInfo report=reportCtrl.GetObjectByCondition( strQuery ) as ICInvReportsInfo;
                if ( report==null )
                {
                    report=new ICInvReportsInfo();
                    report.ID=iID;
                    report.TableName=strItemTableName;
                    report.FK_GEPeriodID=periodID;
                    report.FK_ICInvUnitID=invUnitID;
                }

                #region Qty
                ICInvReportsInfo lastReport=GetReport( strItemTableName , iID , PeriodProvider.GetPeriod( periodID , -1 ) , invUnitID );
                if ( lastReport!=null )
                    report.BeginQty=lastReport.EndQty;
                else
                    report.BeginQty=0;

                report.ShipmentQty=GetQtyFromVouchers( strItemTableName , iID , InventoryConfigType.Shipment|InventoryConfigType.CompanyQtyEffective , periodID , invUnitID );
                report.ReceiveQty=GetQtyFromVouchers( strItemTableName , iID , InventoryConfigType.Receipt|InventoryConfigType.CompanyQtyEffective , periodID , invUnitID );
                report.DiffQty=GetQtyFromVouchers( strItemTableName , iID , InventoryConfigType.Lost , periodID , invUnitID );
                report.EndQty=report.BeginQty+report.ReceiveQty-report.ShipmentQty-report.DiffQty;

                #endregion

                #region Amt
                report.BeginAmt=report.BeginQty*GetValuation( strItemTableName , iID , PeriodProvider.GetPeriod( periodID , -1 ) , invUnitID );
                report.ReceiveAmt=GetAmtFromVouchers( strItemTableName , iID , InventoryConfigType.Receipt|InventoryConfigType.COGSEffective , periodID );

                if ( ( report.BeginQty+report.ReceiveQty )==0 )
                    report.COGSUnit=0;
                else
                    report.COGSUnit=( report.BeginAmt+report.ReceiveAmt )/( report.BeginQty+report.ReceiveQty );

                report.ShipmentAmt=report.COGSUnit*report.ShipmentQty;
                report.DiffAmt=report.COGSUnit*report.DiffQty;
                report.EndAmt=report.COGSUnit*report.EndQty;
                #endregion

                lstReports.Add( report );
            }
            foreach ( ICInvReportsInfo report in lstReports )
                reportCtrl.SaveObject( report );
            #endregion

            List<Guid> lstComUnitIDs=new List<Guid>();
            lstComUnitIDs.AddRange( CompanyUnitProvider.GetInventoryCompanyUnits().Where( t => t.IsInventory ).Select( t => t.GECompanyUnitID ) );

            foreach ( Guid comUnitID in lstComUnitIDs )
                ReportComUnitCalculation( comUnitID , strItemTableName , itemID , periodID , invUnitID );

            return lstReports;
        }

        public static List<ICInvReportComUnitsInfo> ReportComUnitCalculation ( Guid companyUnitID , String strItemTableName , Guid itemID , int year , int month , Guid invUnitID )
        {
            return ReportComUnitCalculation( companyUnitID , strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) , invUnitID );
        }
        public static List<ICInvReportComUnitsInfo> ReportComUnitCalculation ( Guid companyUnitID , String strItemTableName , Guid itemID , DateTime date , Guid invUnitID )
        {
            return ReportComUnitCalculation( companyUnitID , strItemTableName , itemID , PeriodProvider.GetPeriod( date.Year , date.Month ) , invUnitID );
        }
        public static List<ICInvReportComUnitsInfo> ReportComUnitCalculation ( Guid companyUnitID , String strItemTableName , Guid itemID , Guid periodID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return new List<ICInvReportComUnitsInfo>();

            #region Init
            DateTime date=PeriodProvider.GetFirstDay( periodID );

            List<Guid> lstComUnitIDs=new List<Guid>();
            List<Guid> lstItemIDs=new List<Guid>();

            if ( companyUnitID==Guid.Empty||companyUnitID==null )
                lstComUnitIDs.AddRange( CompanyUnitProvider.GetInventoryCompanyUnits().Where( t => t.IsInventory ).Select( t => t.GECompanyUnitID ) );
            else
                lstComUnitIDs.Add( companyUnitID );

            if ( itemID==Guid.Empty||itemID==null )
                lstItemIDs.AddRange( GetInventoryItemIDs( strItemTableName ) );
            else
                lstItemIDs.Add( itemID );
            #endregion

            #region Report Calculation
            ICInvReportComUnitsController reportCtrl=new ICInvReportComUnitsController();
            List<ICInvReportComUnitsInfo> lstReports=new List<ICInvReportComUnitsInfo>();
            foreach ( Guid comUnitID in lstComUnitIDs )
            {
                foreach ( Guid iID in lstItemIDs )
                {
                    String strQuery=String.Format( " FK_GECompanyUnitID='{0}' AND FK_GEPeriodID='{1}' AND TableName ='{2}' AND ID='{3}'  AND FK_ICInvUnitID = '{4}'" , comUnitID , periodID , strItemTableName , iID , invUnitID );
                    ICInvReportComUnitsInfo report=reportCtrl.GetObjectByCondition( strQuery ) as ICInvReportComUnitsInfo;
                    if ( report==null )
                    {
                        report=new ICInvReportComUnitsInfo();
                        report.FK_GECompanyUnitID=comUnitID;
                        report.ID=iID;
                        report.TableName=strItemTableName;
                        report.FK_GEPeriodID=periodID;
                        report.FK_ICInvUnitID=invUnitID;

                        strQuery=String.Format( "SELECT ICInvReportID FROM ICInvReports WHERE FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}'  AND FK_ICInvUnitID = '{3}'" , periodID , strItemTableName , iID , invUnitID );
                        object objID=BusinessObjectController.GetData( strQuery );
                        if ( objID!=null&&objID!=DBNull.Value )
                            report.FK_ICInvReportID=ABCHelper.DataConverter.ConvertToGuid( objID );
                    }

                    #region Qty
                    ICInvReportComUnitsInfo lastReport=GetReportComUnit( comUnitID , strItemTableName , iID , PeriodProvider.GetPeriod( periodID , -1 ) , invUnitID );
                    if ( lastReport!=null )
                        report.BeginQty=lastReport.EndQty;
                    else
                        report.BeginQty=0;

                    report.ShipmentQty=GetQtyFromVouchers( comUnitID , strItemTableName , iID , InventoryConfigType.Shipment , periodID , invUnitID );
                    report.ReceiveQty=GetQtyFromVouchers( comUnitID , strItemTableName , iID , InventoryConfigType.Receipt , periodID , invUnitID );
                    report.DiffQty=GetQtyFromVouchers( comUnitID , strItemTableName , iID , InventoryConfigType.Lost , periodID , invUnitID );
                    report.EndQty=report.BeginQty+report.ReceiveQty-report.ShipmentQty-report.DiffQty;

                    #endregion

                    #region Amt
                    report.BeginAmt=report.BeginQty*GetValuation( strItemTableName , iID , PeriodProvider.GetPeriod( periodID , -1 ) , invUnitID );
                    report.ReceiveAmt=GetAmtFromVouchers( comUnitID , strItemTableName , iID , InventoryConfigType.Receipt , periodID );

                    if ( ( report.BeginQty+report.ReceiveQty )==0 )
                        report.COGSUnit=0;
                    else
                        report.COGSUnit=( report.BeginAmt+report.ReceiveAmt )/( report.BeginQty+report.ReceiveQty );

                    report.ShipmentAmt=report.COGSUnit*report.ShipmentQty;
                    report.DiffAmt=report.COGSUnit*report.DiffQty;
                    report.EndAmt=report.COGSUnit*report.EndQty;
                    #endregion

                    lstReports.Add( report );
                }
            }

            foreach ( ICInvReportComUnitsInfo report in lstReports )
                reportCtrl.SaveObject( report );
            #endregion

            return lstReports;
        }

        #endregion

        #region Get
        public static List<ICInvReportsInfo> GetReport ( int year , int month , String strItemTableName , Guid invUnitID )
        {
            return GetReport( PeriodProvider.GetPeriod( year , month ) , strItemTableName , invUnitID );
        }
        public static List<ICInvReportsInfo> GetReport ( Guid periodID , String strItemTableName , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return new List<ICInvReportsInfo>();

            String strQuery=String.Format( " SELECT * FROM ICInvReports WHERE FK_GEPeriodID='{0}' AND TableName ='{1}' AND FK_ICInvUnitID = '{2}'" , periodID , strItemTableName , invUnitID );

            return new ICInvReportsController().GetList( strQuery ).Cast<ICInvReportsInfo>().ToList();
        }
        public static ICInvReportsInfo GetReport ( Guid itemID , int year , int month , String strItemTableName , Guid invUnitID )
        {
            return GetReport( strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) , invUnitID );
        }
        public static ICInvReportsInfo GetReport ( String strItemTableName , Guid itemID , Guid periodID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return null;

            String strQuery=String.Format( " SELECT * FROM ICInvReports WHERE FK_GEPeriodID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_ICInvUnitID = '{3}'" , periodID , strItemTableName , itemID , invUnitID );

            return new ICInvReportsController().GetObject( strQuery ) as ICInvReportsInfo;
        }

        public static List<ICInvReportComUnitsInfo> GetReportComUnit ( Guid companyUnitID , int year , int month , String strItemTableName , Guid invUnitID )
        {
            return GetReportComUnit( companyUnitID , PeriodProvider.GetPeriod( year , month ) , strItemTableName , invUnitID );
        }
        public static List<ICInvReportComUnitsInfo> GetReportComUnit ( Guid companyUnitID , Guid periodID , String strItemTableName , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return new List<ICInvReportComUnitsInfo>();

            String strQuery=String.Format( " SELECT * FROM ICInvReportComUnits WHERE FK_GECompanyUnitID='{0}' AND FK_GEPeriodID='{1}' AND TableName ='{2}' AND FK_ICInvUnitID = '{3}'" , companyUnitID , periodID , strItemTableName , invUnitID );

            return new ICInvReportComUnitsController().GetList( strQuery ).Cast<ICInvReportComUnitsInfo>().ToList();
        }
        public static ICInvReportComUnitsInfo GetReportComUnit ( Guid companyUnitID , String strItemTableName , Guid itemID , int year , int month , Guid invUnitID )
        {
            return GetReportComUnit( companyUnitID , strItemTableName , itemID , PeriodProvider.GetPeriod( year , month ) , invUnitID );
        }
        public static ICInvReportComUnitsInfo GetReportComUnit ( Guid companyUnitID , String strItemTableName , Guid itemID , Guid periodID , Guid invUnitID )
        {
            if ( invUnitID==null||invUnitID==Guid.Empty )
                return null;

            String strQuery=String.Format( " SELECT * FROM ICInvReportComUnits WHERE FK_GECompanyUnitID='{0}' AND TableName ='{1}' AND ID='{2}' AND FK_GEPeriodID='{3}' AND FK_ICInvUnitID = '{4}'" , companyUnitID , strItemTableName , itemID , periodID , invUnitID );

            return new ICInvReportComUnitsController().GetObject( strQuery ) as ICInvReportComUnitsInfo;
        }
        #endregion

        #endregion

        public static void PeriodEndingProcessings ( )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                DataSet ds=BusinessObjectController.RunQuery( @"SELECT DISTINCT  ItemTableName FROM ICInventoryConfigs" );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    Guid currentPeriodID=PeriodProvider.GetCurrentPeriod();
                    String strQuery="SELECT * FROM GEPeriods WHERE GEPeriodID NOT IN ( SELECT FK_GEPeriodID FROM ICInvReports) ORDER BY Year ASC,Month ASC";
                    List<GEPeriodsInfo> lstPeriods=new GEPeriodsController().GetList( strQuery ).Cast<GEPeriodsInfo>().ToList();
                    foreach ( GEPeriodsInfo period in lstPeriods )
                    {
                        foreach ( DataRow dr in ds.Tables[0].Rows )
                            if ( dr[0]!=null&&dr[0]!=DBNull.Value&&DataStructureProvider.IsExistedTable( dr[0].ToString() ) )
                                PeriodEndingProcessing( period.GEPeriodID , dr[0].ToString() );
                    }
                }
                //  PeriodEndingProcessing( 2014 , 3 );
                //  PeriodEndingProcessing( 2014 , 4 );
                scope.Complete();
            }
        }

        public static void PeriodEndingProcessing ( Guid periodID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                DataSet ds=BusinessObjectController.RunQuery( @"SELECT DISTINCT  ItemTableName FROM ICInventoryConfigs" );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        if ( dr[0]!=null&&dr[0]!=DBNull.Value&&DataStructureProvider.IsExistedTable( dr[0].ToString() ) )
                            PeriodEndingProcessing( periodID , dr[0].ToString() );

                }
                scope.Complete();
            }
        }

        public static void PeriodEndingProcessing ( Guid periodID , String strItemTableName )
        {
            foreach ( ICInvUnitsInfo unit in new ICInvUnitsController().GetListAllObjects() )
            {
                ReportCalculation( strItemTableName , Guid.Empty , periodID , unit.ICInvUnitID );
           //     ReportComUnitCalculation( Guid.Empty , strItemTableName , Guid.Empty , periodID , unit.ICInvUnitID );
            }

            UpdateValuationToVouchers( strItemTableName , Guid.Empty , periodID );
        }
      
        public static void PeriodEndingProcessing ( int year , int month , String strItemTableName )
        {
            PeriodEndingProcessing( PeriodProvider.GetPeriod( year , month ) , strItemTableName );
        }


        public static void PeriodEndingProcessing ( Guid periodID , Guid companyUnitID )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                DataSet ds=BusinessObjectController.RunQuery( @"SELECT DISTINCT  ItemTableName FROM ICInventoryConfigs" );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    foreach ( DataRow dr in ds.Tables[0].Rows )
                        if ( dr[0]!=null&&dr[0]!=DBNull.Value&&DataStructureProvider.IsExistedTable( dr[0].ToString() ) )
                            PeriodEndingProcessing( periodID , companyUnitID , dr[0].ToString() );

                }
                scope.Complete();
            }
        }
        public static void PeriodEndingProcessing ( Guid periodID , Guid companyUnitID , String strItemTableName )
        {
            foreach ( ICInvUnitsInfo unit in new ICInvUnitsController().GetListAllObjects() )
            {
                //ReportCalculation( strItemTableName , Guid.Empty , periodID , unit.ICInvUnitID );
                ReportComUnitCalculation( companyUnitID , strItemTableName , Guid.Empty , periodID , unit.ICInvUnitID );
            }
        }


    }
}
