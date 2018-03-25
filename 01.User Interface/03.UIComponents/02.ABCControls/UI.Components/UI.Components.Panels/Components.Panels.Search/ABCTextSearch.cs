using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABCProvider;

namespace ABCControls
{
    public class ABCTextSearch : ABCTextEdit , ISearchControl
    {
       public String SearchString
       {
           get
           {
               if(String.IsNullOrWhiteSpace(this.Text))
                   return String.Empty;

               return String.Format( " [{0}] LIKE '%{1}%'" , this.DataMember , this.Text );
           }
       }

       public void SetFormatInfo ( )
       {
           DataFormatProvider.SetControlFormat( this , TableName , DataMember );
       }
    }
}
