using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABCHelper
{
    public class ConditionBuilder
    {

        private StringBuilder Builder = new StringBuilder();
        private List<String> ConditionList=new List<string>();
        private String EndString=String.Empty;

        public ConditionBuilder ()
        {
        }
        public void AppendEndString ( String strText )
        {
            EndString+=( " "+strText );
        }

        public void Append ( String strText )
        {
            Builder.Append( strText );
        }
        public void AppendLine ( String strText )
        {
            Builder.AppendLine( strText );
        }
       
        public void AddCondition ( String strCondition )
        {
            if ( String.IsNullOrWhiteSpace( strCondition )==false )
            {
                ConditionList.Add( strCondition );

                //if ( strCondition.ToUpper().Contains( " AND " )||strCondition.ToUpper().Contains( " OR " ) )
                //    strCondition=String.Format( "({0})" , strCondition );
               
                if ( ConditionList.Count==1 )
                    Builder.Append( " WHERE "+strCondition );
                else
                    Builder.Append( " AND "+strCondition );
            }
        }

        public String ToString ( )
        {
            return Builder.ToString()+EndString;
        }

        public void Clear ( )
        {
            Builder.Clear();
            ConditionList.Clear();
            EndString=String.Empty;
        }
    }
}
