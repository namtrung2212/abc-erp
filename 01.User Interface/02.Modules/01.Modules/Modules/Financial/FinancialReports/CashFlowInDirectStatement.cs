using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ABCBusinessEntities;
using ABCModules;
using ABCProvider;
using ABCScreen.Data;


namespace ABCScreen
{

    public partial class CashFlowInDirectStatement : UserControl
    {
        BindingList<CashFlowInDirectInfo> BindDataList=new BindingList<CashFlowInDirectInfo>();
        Dictionary<String , CashFlowInDirectInfo> DataList=new Dictionary<string , CashFlowInDirectInfo>();

        public String CompanyName=String.Empty;
        public String CompanyAddres=String.Empty;
        public FinanceStatisticTime Time;
        public CashFlowInDirectStatement ( String strCompany , String strAddress , FinanceStatisticTime time )
        {
            InitializeComponent();

            #region UI
            lblCompanyName.Text=strCompany;
            lblCompanyAddress.Text=strAddress;

            CompanyName=strCompany;
            CompanyAddres=strAddress;

            Time=time;
            if ( Time.StatisticType==FinanceStatisticType.Year )
            {
                lblTime.Text="Năm "+Time.StartDate.Year;
                colCurrent.Caption="Năm nay";
                colPast.Caption="Năm trước";
            }
            else if ( Time.StatisticType==FinanceStatisticType.Quater )
            {
                if ( Time.StartDate.Month<=3 )
                    lblTime.Text="Quý I"+" Năm "+Time.StartDate.Year;
                if ( 4<=Time.StartDate.Month&&Time.StartDate.Month<=6 )
                    lblTime.Text="Quý II"+" Năm "+Time.StartDate.Year;
                if ( 7<=Time.StartDate.Month&&Time.StartDate.Month<=9 )
                    lblTime.Text="Quý III"+" Năm "+Time.StartDate.Year;
                if ( 10<=Time.StartDate.Month&&Time.StartDate.Month<=12 )
                    lblTime.Text="Quý IV"+" Năm "+Time.StartDate.Year;


                colCurrent.Caption="Quý này";
                colPast.Caption="Quý trước";
            }
            else if ( Time.StatisticType==FinanceStatisticType.RangeDate )
            {
                lblTime.Text="";
                if ( Time.StartDate!=DateTime.MinValue )
                    lblTime.Text="Từ ngày "+Time.StartDate.ToString( "dd/MM/yyyy" );
                if ( Time.EndDate!=DateTime.MaxValue )
                    lblTime.Text+=" đến ngày "+Time.EndDate.ToString( "dd/MM/yyyy" );

                colCurrent.Caption="Kỳ này";
                colPast.Caption="Kỳ trước";
            }
            
            #endregion

            this.gridView1.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridView1_CustomDrawCell );
          
            InitInformation();
            LoadData();
        }

        void gridView1_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.Column.FieldName=="Current"||e.Column.FieldName=="Past" )
            {
                e.DisplayText=e.DisplayText.Replace( "₫" , "" );
                e.DisplayText=e.DisplayText.Replace( " " , "" );
                if ( e.CellValue==null||Convert.ToDouble( e.CellValue )==0 )
                {
                    e.DisplayText="";
                }
                else if ( Convert.ToDouble( e.CellValue )<0&&e.DisplayText.Contains( "(" )==false )
                {
                    e.DisplayText=String.Format( "({0})" , e.DisplayText.Replace( "-" , "" ) );
                }

            }
            if ( e.Column.FieldName=="Factor"||e.Column.FieldName=="Index" )
            {
                CashFlowInDirectInfo info=gridView1.GetRow( e.RowHandle ) as CashFlowInDirectInfo;
                if ( info!=null&&info.Bold )
                {
                    e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
                    e.Appearance.Options.UseFont=true;
                }
            }
        }


        public void InitInformation ( )
        {
            BindDataList.Clear();

            BindDataList.Add( new CashFlowInDirectInfo( true , "I" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG  KINH DOANH" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "1" , "Lợi nhuận trước thuế" , "01" , "" , 13423079571 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "2" , "Điều chỉnh cho các khoản" , "" , "" , 0 , -2894324210 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Khấu hao TSCĐ" , "02" , "" , 468092521746 , 2880298395 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Các khoản dự phòng" , "03" , "" , 16763788108 , 14025815 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Lãi, lỗ chênh lệch tỷ giá hối đoái chưa thực hiện" , "04" , "" , 228581593 , 2320756 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Lãi, lỗ từ hoạt động đầu tư" , "05" , "" , 2612271973 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Chi phí lãi vay " , "06" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "3" , "Lợi nhuận từ hoạt động kinh doanh trước thay đổi vốn  lưu động" , "08" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tăng, giảm các khoản phải thu" , "09" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tăng, giảm hàng tồn kho" , "10" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tăng, giảm các khoản phải trả (Không kể lãi vay phải trả, thuế thu nhập doanh nghiệp phải nộp)" , "11" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tăng, giảm chi phí trả trước " , "12" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tiền lãi vay đã trả" , "13" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Thuế thu nhập doanh nghiệp đã nộp" , "14" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tiền thu khác từ hoạt động kinh doanh" , "15" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "    - Tiền chi khác cho hoạt động kinh doanh" , "16" , "" , 1463102177 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động kinh doanh" , "20" , "" , 1463102177 , 0 ) );

            BindDataList.Add( new CashFlowInDirectInfo( true , "II" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG ĐẦU TƯ" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "1" , "Tiền chi để mua sắm, xây dựng TSCĐ và các tài sản dài hạn khác" , "21" , "" , 12948355841 , 8186054 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "2" , "Tiền thu từ thanh lý, nhượng bán TSCĐ và các tài sản dài hạn khác" , "22" , "" , 574747064 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "3" , "Tiền chi cho vay, mua các công cụ nợ của đơn vị khác" , "23" , "" , 100023334 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "4" , "Tiền thu hồi cho vay, bán lại các công cụ nợ của đơn vị khác" , "24" , "" , 474723730 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "5" , "Tiền chi đầu tư góp vốn vào các đơn vị khác" , "25" , "" , 13423079571 , 8186054 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "6" , "Tiền thu hồi đầu tư góp vốn vào đơn vị khác" , "26" , "" , 3355769892.75 , 2046513.5 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "7" , "Tiền thu lãi cho vay, cổ tức và lợi nhuận được chia" , "27" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động đầu tư " , "30" , "" , 10067309678.25 , 6139540.5 ) );

            BindDataList.Add( new CashFlowInDirectInfo( true , "III" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG TÀI CHÍNH" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "1" , "Tiền thu từ phát hành cổ phiếu, nhận vốn góp của chủ sở hữu" , "31" , "" , 6139540.5 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "2" , "Tiền thu từ thanh lý, nhượng bán TSCĐ và các tài sản dài hạn khác" , "32" , "" , 574747064 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "3" , "Tiền vay ngắn hạn, dài hạn nhận được " , "33" , "" , 100023334 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "4" , "Tiền chi  trả nợ gốc vay" , "34" , "" , 474723730 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "5" , "Tiền chi trả nợ thuê tài chính" , "35" , "" , 13423079571 , 8186054 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "6" , "Cổ tức, lợi nhuận đã trả cho chủ sở hữu" , "36" , "" , 3355769892.75 , 2046513.5 ) );

            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động tài chính " , "40" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Lưu chuyển tiền thuần trong kỳ  (50=20+30+40 )" , "50" , "" , 10067309678.25 , 6139540.5 ) );
            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Tiền và tương đương tiền  đầu kỳ " , "60" , "" , 10067309678.25 , 6139540.5 ) );
            BindDataList.Add( new CashFlowInDirectInfo( false , "" , "Ảnh hưởng của thay đổi tỷ giá hối đoái quy đổi ngoại tệ" , "61" , "" , 10067309678.25 , 6139540.5 ) );
            BindDataList.Add( new CashFlowInDirectInfo( true , "" , "Tiền và tương đương tiền cuối kỳ  (70=50+60+61 )" , "70" , "" , 10067309678.25 , 6139540.5 ) );


            this.gridControl1.DataSource=BindDataList;
            this.gridControl1.RefreshDataSource();

            DataList.Clear();
            foreach ( CashFlowInDirectInfo info in BindDataList )
            {
                if ( String.IsNullOrEmpty( info.No )==false )
                {
                    if ( DataList.ContainsKey( info.No )==false )
                        DataList.Add( info.No , info );
                    else
                    {
                        MessageBox.Show( "Mã số sai" );
                        return;
                    }
                }
            }
        }

        public void LoadData ( )
        {
            foreach ( CashFlowInDirectInfo info in DataList.Values )
                info.Current=0;

            this.gridControl1.DataSource=BindDataList;
            this.gridControl1.RefreshDataSource();
        }


        private void chkDetail_CheckedChanged ( object sender , EventArgs e )
        {
            if ( chkDetail.Checked )
            {
                gridView1.ActiveFilterString="[Bold] = 'true'";
            }
            else
            {
                gridView1.ActiveFilterString="";
            }
        }


    }
  

}
