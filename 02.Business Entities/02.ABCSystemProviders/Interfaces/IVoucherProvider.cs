using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace Interface
{
    public interface IVoucherProvider
    {
        String GenerateNo(String strTableName,Guid ID);
        void AutomaticUpdateNo ( String strTableName );
    }
}
