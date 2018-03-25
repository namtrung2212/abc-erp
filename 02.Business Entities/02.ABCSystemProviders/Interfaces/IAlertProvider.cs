using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace Interface
{
    public interface IAlertProvider
    {
        List<GEAlertsInfo> GetAlertConfigs ();
        Boolean IsNeedAlert (Guid alertConfigID);
        DataSet GetAlertData ( Guid alertConfigID);
    }
}
