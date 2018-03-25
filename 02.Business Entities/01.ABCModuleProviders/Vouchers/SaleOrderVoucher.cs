using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCProvider;

namespace ABCVoucher
{
    public class SaleOrderVoucher:BaseVoucher
    {
        public override bool CustomFormulaCalc ( BusinessObject obj , Dictionary<string , IEnumerable<BusinessObject>> lstObjecItems , GEFormulaItemsInfo formula )
        {
            if ( obj is ARSaleOrderItemsInfo)
            {
                if ( formula.FormulaName=="ItemUnitPrice" )
                {
                //    ( (ARSaleOrderItemsInfo)obj ).ItemUnitPrice=1500;
                    return true;
                }
            }

            return false;
        }
    }
}
