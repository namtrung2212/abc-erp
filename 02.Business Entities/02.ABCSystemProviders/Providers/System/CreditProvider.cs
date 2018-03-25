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

    public class CreditProvider
    {
        #region Partner
        public static List<MAPartnersInfo> GetPartners ( )
        {
            return new MAPartnersController().GetListAllObjects().Cast<MAPartnersInfo>().ToList();
        }
        #endregion

        #region Configs
        public enum CreditConfigType
        {
            None=0 ,
            Increase=1 ,
            Decrease=2 ,
            Purchase=4 ,
            Sale=8
        }
        public static List<CRCreditConfigsInfo> GetCreditConfigs ( String strTableName )
        {
            CRCreditConfigsController ctrl=new CRCreditConfigsController();
            List<CRCreditConfigsInfo> lstConfigs=new List<CRCreditConfigsInfo>();
            lstConfigs.AddRange( ctrl.GetListByColumn( "TableName" , strTableName ).Cast<CRCreditConfigsInfo>().ToList() );
            lstConfigs=lstConfigs.Where( t => !String.IsNullOrWhiteSpace( t.TableName )&&!String.IsNullOrWhiteSpace( t.PartnerIDField )&&( !String.IsNullOrWhiteSpace( t.AmtField )||!String.IsNullOrWhiteSpace( t.AmtFCField ) ) ).ToList();

            Dictionary<Guid , CRCreditConfigsInfo> lstResults=new Dictionary<Guid , CRCreditConfigsInfo>();
            foreach ( CRCreditConfigsInfo config in lstConfigs )
                if ( lstResults.ContainsKey( config.CRCreditConfigID )==false )
                    lstResults.Add( config.CRCreditConfigID , config );

            return lstResults.Values.ToList();
        }
        public static List<CRCreditConfigsInfo> GetCreditConfigs ( String strTableName , CreditConfigType type )
        {
            List<CRCreditConfigsInfo> lstConfigs=GetCreditConfigs( strTableName );

            if ( ( type&CreditConfigType.Increase )==CreditConfigType.Increase )
                lstConfigs=lstConfigs.Where( t => t.IsIncrease ).ToList();

            if ( ( type&CreditConfigType.Decrease )==CreditConfigType.Decrease )
                lstConfigs=lstConfigs.Where( t => t.IsDecrease ).ToList();

            if ( ( type&CreditConfigType.Purchase )==CreditConfigType.Purchase )
                lstConfigs=lstConfigs.Where( t => t.IsPurchase ).ToList();

            if ( ( type&CreditConfigType.Sale )==CreditConfigType.Sale )
                lstConfigs=lstConfigs.Where( t => t.IsSale ).ToList();

            return lstConfigs;

        }
        public static List<CRCreditConfigsInfo> GetCreditConfigs ( CreditConfigType type )
        {
            List<CRCreditConfigsInfo> lstConfigs=new CRCreditConfigsController().GetListAllObjects().Cast<CRCreditConfigsInfo>().ToList();
            lstConfigs=lstConfigs.Where( t => !String.IsNullOrWhiteSpace( t.TableName )&&DataStructureProvider.IsExistedTable( t.TableName )&&!String.IsNullOrWhiteSpace( t.PartnerIDField )&&( !String.IsNullOrWhiteSpace( t.AmtField )||!String.IsNullOrWhiteSpace( t.AmtFCField ) ) ).ToList();

            if ( ( type&CreditConfigType.Increase )==CreditConfigType.Increase )
                lstConfigs=lstConfigs.Where( t => t.IsIncrease ).ToList();

            if ( ( type&CreditConfigType.Decrease )==CreditConfigType.Decrease )
                lstConfigs=lstConfigs.Where( t => t.IsDecrease ).ToList();

            if ( ( type&CreditConfigType.Purchase )==CreditConfigType.Purchase )
                lstConfigs=lstConfigs.Where( t => t.IsPurchase ).ToList();

            if ( ( type&CreditConfigType.Sale )==CreditConfigType.Sale )
                lstConfigs=lstConfigs.Where( t => t.IsSale ).ToList();


            return lstConfigs;
        }

        public static bool IsNeedCalculateCredit ( String strTableName )
        {
            object iCount=BusinessObjectController.GetData( String.Format( "SELECT COUNT(*) FROM CRCreditConfigs WHERE TableName = '{0}'" , strTableName ) );
            if ( iCount==null||iCount.GetType()!=typeof( int ) )
                return false;

            return Convert.ToInt32( iCount )>0;
        }

        #endregion

        #region Credit

        public static double GetCredit ( Guid partnerID , CreditConfigType configType )
        {

            double amt=0;
            foreach ( CRCreditConfigsInfo config in GetCreditConfigs( configType ) )
            {
                String strFKPartnerIDCol=config.PartnerIDField;
                if ( String.IsNullOrWhiteSpace( strFKPartnerIDCol ) )
                    strFKPartnerIDCol="FK_MAPartnerID";

                String strQuery=QueryGenerator.GenSelect( config.TableName , String.Format( "SUM({0})" , config.AmtFCField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );

                if ( DataStructureProvider.IsTableColumn( config.TableName , strFKPartnerIDCol ) )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , strFKPartnerIDCol , partnerID );
                else
                {
                    if ( DataStructureProvider.IsExistedTable( config.ParentTableName ) )
                    {
                        if ( DataStructureProvider.IsTableColumn( config.ParentTableName , strFKPartnerIDCol ) )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( config.TableName , config.ParentTableName );
                            String strQuery2=QueryGenerator.GenSelect( config.ParentTableName , DataStructureProvider.GetPrimaryKeyColumn( config.ParentTableName ) , true );
                            strQuery2=QueryGenerator.AddEqualCondition( strQuery2 , strFKPartnerIDCol , partnerID );
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN ({1})" , strFK , strQuery2 ) );
                        }
                    }
                }

                object obj=BusinessObjectController.GetData( strQuery );
                if ( obj!=null&&obj!=DBNull.Value )
                    amt+=Convert.ToDouble( obj );
            }
            return amt;
        }
        public static double GetCredit ( Guid partnerID , Guid currencyID , CreditConfigType configType , bool isFCAmt )
        {

            double amt=0;
            foreach ( CRCreditConfigsInfo config in GetCreditConfigs( configType ) )
            {
                if ( DataStructureProvider.IsExistedTable( config.TableName )==false )
                    continue;

                String strFKCurrencyIDCol=config.CurrencyIDField;
                if ( String.IsNullOrWhiteSpace( strFKCurrencyIDCol ) )
                    strFKCurrencyIDCol="FK_GECurrencyID";

                String strFKPartnerIDCol=config.PartnerIDField;
                if ( String.IsNullOrWhiteSpace( strFKPartnerIDCol ) )
                    strFKPartnerIDCol="FK_MAPartnerID";

                String strQuery=QueryGenerator.GenSelect( config.TableName , String.Format( "SUM({0})" , isFCAmt?config.AmtFCField:config.AmtField ) , true );
                strQuery=QueryGenerator.AddCondition( strQuery , config.ConditionString );

                if ( DataStructureProvider.IsTableColumn( config.TableName , strFKPartnerIDCol ) )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , strFKPartnerIDCol , partnerID );
                else
                {
                    if ( DataStructureProvider.IsExistedTable( config.ParentTableName ) )
                    {
                        if ( DataStructureProvider.IsTableColumn( config.ParentTableName , strFKPartnerIDCol ) )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( config.TableName , config.ParentTableName );
                            String strQuery2=QueryGenerator.GenSelect( config.ParentTableName , DataStructureProvider.GetPrimaryKeyColumn( config.ParentTableName ) , true );
                            strQuery2=QueryGenerator.AddEqualCondition( strQuery2 , strFKPartnerIDCol , partnerID );
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN ({1})" , strFK , strQuery2 ) );
                        }
                    }
                }

                if ( ( configType&CreditConfigType.Increase )==CreditConfigType.Increase )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , "IsIncrease" , true );

                if ( ( configType&CreditConfigType.Decrease )==CreditConfigType.Decrease )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , "IsDecrease" , true );

                if ( ( configType&CreditConfigType.Purchase )==CreditConfigType.Purchase )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , "IsPurchase" , true );

                if ( ( configType&CreditConfigType.Sale )==CreditConfigType.Sale )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , "IsSale" , true );

                if ( DataStructureProvider.IsTableColumn( config.TableName , strFKCurrencyIDCol ) )
                    strQuery=QueryGenerator.AddEqualCondition( strQuery , strFKCurrencyIDCol , currencyID );
                else
                {
                    if ( DataStructureProvider.IsExistedTable( config.ParentTableName ) )
                    {
                        if ( DataStructureProvider.IsTableColumn( config.ParentTableName , strFKCurrencyIDCol ) )
                        {
                            String strFK=DataStructureProvider.GetForeignKeyOfTableName( config.TableName , config.ParentTableName );
                            String strQuery2=QueryGenerator.GenSelect( config.ParentTableName , DataStructureProvider.GetPrimaryKeyColumn( config.ParentTableName ) , true );
                            strQuery2=QueryGenerator.AddEqualCondition( strQuery2 , strFKCurrencyIDCol , currencyID );
                            strQuery=QueryGenerator.AddCondition( strQuery , String.Format( "{0} IN ({1})" , strFK , strQuery2 ) );
                        }
                    }
                }

                object obj=BusinessObjectController.GetData( strQuery );
                if ( obj!=null&&obj!=DBNull.Value )
                    amt+=Convert.ToDouble( obj );
            }
            return amt;
        }

        public static CRCreditsInfo GetCredit ( Guid partnerID )
        {
            return new CRCreditsController().GetObjectByColumn( "FK_MAPartnerID" , partnerID ) as CRCreditsInfo;
        }
        public static CRCreditDetailsInfo GetCredit ( Guid currencyID , Guid partnerID )
        {
            return new CRCreditDetailsController().GetObjectByCondition( String.Format( " FK_MAPartnerID='{0}' AND FK_GECurrencyID='{1}' " , currencyID , partnerID ) ) as CRCreditDetailsInfo;
        }

        public static void CalculateCredit ( Guid partnerID )
        {
            MAPartnersController partnerCtrl=new MAPartnersController();
            MAPartnersInfo partner=partnerCtrl.GetObjectByID( partnerID ) as MAPartnersInfo;
            if ( partner==null )
                return;

            #region Credit
            CRCreditsController Ctrl=new CRCreditsController();
            CRCreditsInfo credit=Ctrl.GetObjectByColumn( "FK_MAPartnerID" , partnerID ) as CRCreditsInfo;
            if ( credit==null )
            {
                credit=new CRCreditsInfo();
                credit.FK_MAPartnerID=partnerID;
                Ctrl.CreateObject( credit );
            }

            credit.PurchaseAmt=GetCredit( partnerID , CreditConfigType.Purchase|CreditConfigType.Increase );
            credit.SaleAmt=GetCredit( partnerID , CreditConfigType.Sale|CreditConfigType.Increase );
            credit.CreditAmt=GetCredit( partnerID , CreditConfigType.Increase )-GetCredit( partnerID , CreditConfigType.Decrease );

            Ctrl.UpdateObject( credit );
            #endregion

            partner.CreditAmt=credit.CreditAmt;
            partner.OverCreditLimit=credit.CreditAmt-partner.CreditLimit;
            partnerCtrl.UpdateObject( partner );

            #region Credit Detail

            CRCreditDetailsController ctrlDetail=new CRCreditDetailsController();
            foreach ( GECurrencysInfo currency in new GECurrencysController().GetListAllObjects() )
            {
                CRCreditDetailsInfo detail=ctrlDetail.GetObjectByCondition( String.Format( " FK_MAPartnerID='{0}' AND FK_GECurrencyID='{1}' " , partnerID , currency.GECurrencyID ) ) as CRCreditDetailsInfo;
                if ( detail==null )
                {
                    detail=new CRCreditDetailsInfo();
                    detail.FK_MAPartnerID=partnerID;
                    detail.FK_GECurrencyID=currency.GECurrencyID;
                    ctrlDetail.CreateObject( detail );
                }

                detail.PurchaseAmt=GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Purchase|CreditConfigType.Increase , false );
                detail.SaleAmt=GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Sale|CreditConfigType.Increase , false );
                detail.CreditAmt=GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Increase , false )-GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Decrease , false );
                detail.CreditFCAmt=GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Increase , true )-GetCredit( partnerID , currency.GECurrencyID , CreditConfigType.Decrease , true );

                ctrlDetail.UpdateObject( detail );
            }
            #endregion
        }

        #endregion

        public static void CalculateCredits ( )
        {
            using ( System.Transactions.TransactionScope scope=new System.Transactions.TransactionScope() )
            {
                foreach ( MAPartnersInfo partner in new MAPartnersController().GetListAllObjects() )
                    CalculateCredit( partner.MAPartnerID );
                scope.Complete();
            }
        }
        public static void CalculateCredit ( String strTableName , Guid ID )
        {
            List<Guid> lstPartnerIDs=new List<Guid>();
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl!=null )
            {
                BusinessObject obj=ctrl.GetObjectByID( ID );
                if ( obj!=null )
                {
                    String strPartnerFKCol=DataStructureProvider.GetForeignKeyOfTableName( strTableName , "MAPartners" );
                    if ( String.IsNullOrWhiteSpace( strPartnerFKCol )==false )
                    {
                        Guid partnerID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , strPartnerFKCol ) );
                        if ( partnerID!=Guid.Empty&&!lstPartnerIDs.Contains( partnerID ) )
                            lstPartnerIDs.Add( partnerID );
                    }

                    foreach ( GEVoucherItemsInfo voucherItem in VoucherProvider.GetConfigItems( strTableName , "" ) )
                    {
                        strPartnerFKCol=DataStructureProvider.GetForeignKeyOfTableName( voucherItem.ItemTableName , "MAPartners" );
                        if ( String.IsNullOrWhiteSpace( strPartnerFKCol )==false )
                        {
                            Guid partnerID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , strPartnerFKCol ) );
                            if ( partnerID!=Guid.Empty&&!lstPartnerIDs.Contains( partnerID ) )
                                lstPartnerIDs.Add( partnerID );
                        }
                    }
                }
            }
            foreach ( Guid partnerID in lstPartnerIDs )
                CreditProvider.CalculateCredit( partnerID );
        }

    }
}
