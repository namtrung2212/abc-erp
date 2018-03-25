using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

using System.Reflection;
using System.Reflection.Emit;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class ABCPeriodEndingProvider
    {
        public static void DetectAndDoPeriodEnding ( )
        {
            InventoryProvider.PeriodEndingProcessings();
            CreditProvider.CalculateCredits();
        }
    }
}
