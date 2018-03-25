using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace Interface
{
    public interface IDocumentProvider
    {
        bool AttachDocumentToVoucher ( Guid documentID , String strTableName , Guid ID );
        bool AttachDocumentToVoucher ( ADDocumentsInfo documentInfo , String strTableName , Guid ID );
    }
}
