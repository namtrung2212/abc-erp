using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ABCProvider;

namespace ABCControls
{
    public partial class ABCNumbSearch : DevExpress.XtraEditors.XtraUserControl, ISearchControl
    {
        public String DataMember { get; set; }

        public String TableName { get; set; }

        bool extractlySearch;

        
        [Category( "Search" )]
        public bool ExtractlySearch
        {
            get { return extractlySearch; }
            set
            {
                if ( value )
                    layoutControlItem2.Visibility=DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                else
                    layoutControlItem2.Visibility=DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
        }

        public ABCNumbSearch ( )
        {
            InitializeComponent();

  //          spinEdit1.EditValue=0;
  //          spinEdit2.EditValue=1000000;
        }


        String TempSearchString=String.Empty;
        public String SearchString
        {

            get
            {
                if ( String.IsNullOrWhiteSpace( TempSearchString ) )
                    TempSearchString=String.Format( " ( {1} <= [{0}] AND [{0}] <= {2} ) " , this.DataMember , 0 , 1000000 );

                String strResult=String.Empty;

                if ( ExtractlySearch==false )
                {
                    #region 2 params
                    if ( spinEdit1.EditValue==null||spinEdit2.EditValue==null )
                        return String.Empty;

                    if ( spinEdit1.EditValue!=null )
                        strResult=String.Format( System.Globalization.CultureInfo.InvariantCulture.NumberFormat , " {1} <= [{0}]  " , this.DataMember , spinEdit1.EditValue );

                    if ( spinEdit2.EditValue!=null&&String.IsNullOrWhiteSpace( strResult ) )
                        strResult=String.Format( System.Globalization.CultureInfo.InvariantCulture.NumberFormat , " [{0}] <= {1} " , this.DataMember , spinEdit2.EditValue );
                    else
                        strResult=String.Format( System.Globalization.CultureInfo.InvariantCulture.NumberFormat , " ( {1} <= [{0}] AND [{0}] <= {2} ) " , this.DataMember , spinEdit1.EditValue , spinEdit2.EditValue );
                    
                    #endregion
                }
                else
                {
                    strResult=String.Format( System.Globalization.CultureInfo.InvariantCulture.NumberFormat , " {1} = [{0}]  " , this.DataMember , spinEdit1.EditValue );
                }

                if ( strResult==TempSearchString )
                    return String.Empty;

                return strResult;

            }
        }

        public void SetFormatInfo ( )
        {
            DataFormatProvider.SetControlFormat( spinEdit1 , TableName , DataMember );
            DataFormatProvider.SetControlFormat( spinEdit2 , TableName , DataMember );
        }
    }
}
