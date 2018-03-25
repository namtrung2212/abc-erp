using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABCHelper
{
    public class DataConverter
    {
        public static Guid ConvertToGuid ( object objId )
        {
            if ( objId!=null&&objId!=DBNull.Value )
            {
                if ( objId is Guid )
                    return (Guid)objId;
                else if ( objId is Nullable<Guid>&&( (Nullable<Guid>)objId ).HasValue )
                    return ( (Nullable<Guid>)objId ).Value;
                else
                {
                    Guid result=Guid.Empty;
                     Guid.TryParse( objId.ToString() , out result );
                     return result;
                }
            }
            return Guid.Empty;
        }
    }
}
