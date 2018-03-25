using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class DocumentProvider //: Interface.IDocumentProvider
    {
        public static bool AttachDocumentToVoucher ( Guid documentID , String strTableName , Guid ID )
        {
            return false;
        }
        public static bool AttachDocumentToVoucher ( ADDocumentsInfo documentInfo , String strTableName , Guid ID )
        {
            return false;
        }
    }
}