using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABCProvider;

namespace ABCControls
{
    public class ABCGridLookUpEditSearch : ABCGridLookUpEdit , ISearchControl
    {
        String TempSearchString=String.Empty;
        public String SearchString
        {
            get
            {
                if ( this.EditValue!=null )
                {
                    if ( this.EditValue==DBNull.Value||( ABCHelper.DataConverter.ConvertToGuid( this.EditValue )==Guid.Empty ) )
                        return String.Empty;

                    return String.Format( " [{0}] = '{1}'" , this.DataMember , ABCHelper.DataConverter.ConvertToGuid( this.EditValue) );
                }
                return String.Empty;
            }
        }

        public void SetFormatInfo ( )
        {
            DataFormatProvider.SetControlFormat( this , TableName , DataMember );
        }
    }
}
