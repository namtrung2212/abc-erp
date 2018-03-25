using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABCControls
{
    public class ABCBoolSearch : ABCCheckEdit , ISearchControl
    {

        String TempSearchString=String.Empty;
        public String SearchString
        {
            get
           {
                if(String.IsNullOrWhiteSpace(TempSearchString))
                    TempSearchString=String.Format( " [{0}] = '{1}'" , this.DataMember , false );

                String strResult= String.Format( " [{0}] = '{1}'" , this.DataMember , this.EditValue );
                if ( strResult==TempSearchString )
                    return String.Empty;
                return strResult;
           }
        }

        public void SetFormatInfo ( )
        {
        }
    }
}
