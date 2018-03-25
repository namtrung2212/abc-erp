using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;
using ABCProvider;

namespace ABCVoucher
{
    public class SaleInvoiceVoucher:BaseVoucher
    {
        public override bool CustomFormulaCalc ( BusinessObject obj , Dictionary<string , IEnumerable<BusinessObject>> lstObjecItems , GEFormulaItemsInfo formula )
        {
            if ( obj is ARSaleInvoiceItemsInfo)
            {
                if ( formula.FormulaName=="ItemUnitPrice" )
                {
             //       ( (ARSaleInvoiceItemsInfo)obj ).ItemUnitPrice=1500;
                    return true;
                }
            }

            return false;
        }
    }
}
