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
    public partial class ABCDateTimeSearch : DevExpress.XtraEditors.XtraUserControl, ISearchControl
    {
        public String DataMember { get; set; }

        public String TableName { get; set; }

        public ABCDateTimeSearch ( )
        {
            InitializeComponent();

        //    dateEdit1.EditValue=new DateTime( ABCApp.ABCDataGlobal.WorkingDate.Year , 1 , 1 );
        //    dateEdit2.EditValue=new DateTime( ABCApp.ABCDataGlobal.WorkingDate.Year , 12, 31 );
        }

        public DateTime? StartDate
        {
            get { return dateEdit1.EditValue as DateTime?; }
        }
        public DateTime? EndDate
        {
            get { return dateEdit2.EditValue as DateTime?; }
        }
        public String SearchString
        {
            get
            {
                String strFrom=String.Empty;
                String strTo=String.Empty;

                if ( dateEdit1.EditValue!=null )
                    strFrom=TimeProvider.GenCompareDateTime( this.DataMember , ">=" , Convert.ToDateTime( dateEdit1.EditValue ) );

                if ( dateEdit2.EditValue!=null )
                    strTo=TimeProvider.GenCompareDateTime( this.DataMember , "<=" , Convert.ToDateTime( dateEdit2.EditValue ) );

                if ( String.IsNullOrWhiteSpace( strFrom )==false&&String.IsNullOrWhiteSpace( strTo )==false )
                    return String.Format( " ( {0} AND {1} ) " , strFrom , strTo );

                if ( String.IsNullOrWhiteSpace( strFrom )==false )
                    return strFrom;

                if ( String.IsNullOrWhiteSpace( strTo )==false )
                    return strTo;

                return String.Empty;
            }
        }

        public void SetFormatInfo ( )
        {
            DataFormatProvider.SetControlFormat( dateEdit1 , TableName , DataMember );
            DataFormatProvider.SetControlFormat( dateEdit2 , TableName , DataMember );
        
        }
    }
}
