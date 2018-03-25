using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ABCProvider;
using ABCProvider;
using ABCProvider;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class BaseVoucher
    {
        public GEVouchersInfo Config;
        public List<GEVoucherItemsInfo> ConfigItems=new List<GEVoucherItemsInfo>();
        public String TableName;

        public virtual bool ReCalculate ( String strTableName , Guid ID , bool isSave )
        {
            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( strTableName );
            if ( ctrl==null )
                return false;

            return ReCalculate( ctrl.GetObjectByID( ID ) , isSave );
        }

        public virtual bool ReCalculate ( BusinessObject obj ,bool isSave)
        {
            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colLockStatus ) )
                if ( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colLockStatus ).ToString()==ABCCommon.ABCConstString.LockStatusLocked )
                    return false;

            if ( obj.GetID()!=Guid.Empty )
            {
                Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems=new Dictionary<string , IEnumerable<BusinessObject>>();
                foreach ( GEVoucherItemsInfo configItem in ConfigItems )
                {
                    if ( !DataStructureProvider.IsForeignKey( configItem.ItemTableName , configItem.ItemFKField ) )
                        continue;

                    BusinessObjectController itemCtrl=BusinessControllerFactory.GetBusinessController( configItem.ItemTableName );

                    if ( !lstObjecItems.ContainsKey( configItem.ItemTableName ) )
                        lstObjecItems.Add( configItem.ItemTableName , (IEnumerable<BusinessObject>)itemCtrl.GetListByForeignKey( configItem.ItemFKField , obj.GetID() ) );
                }

                return ReCalculate( obj , lstObjecItems , false , String.Empty , isSave );
            }

            return ReCalculate( obj , null , false , String.Empty , isSave );
        }

        public virtual bool ReCalculate ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isCalcMainOnly , String strAfterValidateFieldName , bool isSave )
        {
            if ( obj==null||obj.GetID()==Guid.Empty )
                return false;

            if ( DataStructureProvider.IsTableColumn( obj.AATableName , ABCCommon.ABCConstString.colLockStatus ) )
                if ( ABCDynamicInvoker.GetValue( obj , ABCCommon.ABCConstString.colLockStatus ).ToString()==ABCCommon.ABCConstString.LockStatusLocked )
                    return false;

            if ( !BeforeReCalculate( obj , lstObjecItems , isCalcMainOnly , strAfterValidateFieldName ) )
                return false;

            if ( !DoReCalculate( obj , lstObjecItems , isCalcMainOnly , strAfterValidateFieldName , isSave ) )
                return false;

            AfterReCalculate( obj , lstObjecItems , isCalcMainOnly , strAfterValidateFieldName );
             return true;
        }

        public virtual bool BeforeReCalculate ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isCalcMainOnly , String strAfterValidateFieldName )
        {
            return true;
        }

        public virtual bool DoReCalculate ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isCalcMainOnly , String strAfterValidateFieldName , bool isSave )
        {
            bool isCalculated=false;
            if ( !isCalcMainOnly&&lstObjecItems!=null )
            {
                foreach ( String strItemTableName in lstObjecItems.Keys )
                {
                    foreach ( BusinessObject objItem in lstObjecItems[strItemTableName] )
                        isCalculated=FormulaProvider.Calculate( this , objItem , strAfterValidateFieldName , isSave )||isCalculated;
                }
            }

            if ( lstObjecItems!=null )
                isCalculated=FormulaProvider.CalculateMainOnly( this , obj , lstObjecItems , strAfterValidateFieldName , isSave )||isCalculated;
            else
                isCalculated=FormulaProvider.Calculate( this , obj , strAfterValidateFieldName , isSave )||isCalculated;

            return isCalculated;
        }

        public virtual bool AfterReCalculate ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isCalcMainOnly , String strAfterValidateFieldName )
        {
            bool isUpdated=false;
            //if ( !isCalcMainOnly&&lstObjecItems!=null )
            //{
            //    foreach ( String strItemTableName in lstObjecItems.Keys )
            //    {
            //        BusinessObjectController itemCtrl=BusinessControllerFactory.GetBusinessController( strItemTableName );
            //        if ( itemCtrl!=null )
            //        {
            //            foreach ( BusinessObject objItem in lstObjecItems[strItemTableName] )
            //                if ( objItem.GetID()!=Guid.Empty )
            //                {
            //                    itemCtrl.UpdateObject( objItem );
            //                    isUpdated=true;
            //                }
            //        }
            //    }
            //}
            //if ( obj.GetID()!=Guid.Empty )
            //{
            //    BusinessControllerFactory.GetBusinessController( obj.AATableName ).UpdateObject( obj );
            //    isUpdated=true;
            //}

            return isUpdated;
        }

        public virtual bool CustomFormulaCalc ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , GEFormulaItemsInfo formula )
        {
            return true;
        }

        #region Actions
        public virtual bool BeforeSave ( Guid ID )
        {
            return true;
        }
        public virtual void AfterSaved ( Guid ID )
        {
        }

        public virtual bool BeforeDelete ( Guid ID )
        {
            return true;
        }
        public virtual void AfterDeleted ( Guid ID )
        {
        }

        public virtual bool BeforeApprove ( Guid ID )
        {
            return true;
        }
        public virtual void AfterApproved ( Guid ID )
        {
        }

        public virtual bool BeforeReject ( Guid ID )
        {
            return true;
        }
        public virtual void AfterRejected ( Guid ID )
        {
        }

        public virtual bool BeforeLock ( Guid ID )
        {
            return true;
        }
        public virtual void AfterLocked ( Guid ID )
        {
        }

        public virtual bool BeforeUnLock ( Guid ID )
        {
            return true;
        }
        public virtual void AfterUnLocked ( Guid ID )
        {
        }

        public virtual void Post ( Guid ID )
        {
        }
        public virtual void UnPost ( Guid ID )
        {
        }
        #endregion
    }
}
