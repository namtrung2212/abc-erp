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

    public partial class CashFlowDirectStatement : UserControl
    {
        BindingList<CashFlowDirectInfo> BindDataList=new BindingList<CashFlowDirectInfo>();
        Dictionary<String , CashFlowDirectInfo> DataList=new Dictionary<string , CashFlowDirectInfo>();

        public String CompanyName=String.Empty;
        public String CompanyAddres=String.Empty;
        public FinanceStatisticTime Time;
        public CashFlowDirectStatement ( String strCompany , String strAddress , FinanceStatisticTime time )
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

            CashFlowDirectInfo info=gridView1.GetRow( e.RowHandle ) as CashFlowDirectInfo;

            if ( info!=null&&info.Bold )
            {
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
                e.Appearance.Options.UseFont=true;
            }
            else
                e.Appearance.Options.UseFont=false;
        }

        public void InitInformation ( )
        {
            BindDataList.Clear();

            BindDataList.Add( new CashFlowDirectInfo( true , "I" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG  KINH DOANH" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "1" , "Tiền thu từ bán hàng, cung cấp dịch vụ và doanh thu khác" , "01" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "2" , "Tiền chi trả cho người cung cấp hàng hóa và dịch vụ" , "02" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "3" , "Tiền chi trả cho người lao động" , "03" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "4" , "Tiền chi trả lãi vay" , "04" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "5" , "Tiền chi nộp thuế Thu nhập doanh nghiệp" , "05" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "6" , "Tiền thu khác từ hoạt động kinh doanh" , "06" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "7" , "Tiền chi khác cho hoạt động kinh doanh" , "07" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động kinh doanh " , "20" , "" , 0 , 0 ) );

            BindDataList.Add( new CashFlowDirectInfo( true , "II" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG ĐẦU TƯ" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "1" , "Tiền chi để mua sắm, xây dựng TSCĐ và các tài sản dài hạn khác" , "21" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "2" , "Tiền thu từ thanh lý, nhượng bán TSCĐ và các tài sản dài hạn khác" , "22" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "3" , "Tiền chi cho vay, mua các công cụ nợ của đơn vị khác" , "23" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "4" , "Tiền thu hồi cho vay, bán lại các công cụ nợ của đơn vị khác" , "24" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "5" , "Tiền chi đầu tư góp vốn vào các đơn vị khác" , "25" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "6" , "Tiền thu hồi đầu tư góp vốn vào đơn vị khác" , "26" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "7" , "Tiền thu lãi cho vay, cổ tức và lợi nhuận được chia" , "27" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động đầu tư " , "30" , "" , 0 , 0 ) );

            BindDataList.Add( new CashFlowDirectInfo( true , "III" , "LƯU CHUYỂN TIỀN TỪ HOẠT ĐỘNG TÀI CHÍNH" , "" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "1" , "Tiền thu từ phát hành cổ phiếu, nhận vốn góp của chủ sở hữu" , "31" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "2" , "Tiền thu từ thanh lý, nhượng bán TSCĐ và các tài sản dài hạn khác" , "32" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "3" , "Tiền vay ngắn hạn, dài hạn nhận được " , "33" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "4" , "Tiền chi  trả nợ gốc vay" , "34" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "5" , "Tiền chi trả nợ thuê tài chính" , "35" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "6" , "Cổ tức, lợi nhuận đã trả cho chủ sở hữu" , "36" , "" , 0 , 0 ) );

            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Lưu chuyển tiền thuần từ hoạt động tài chính " , "40" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Lưu chuyển tiền thuần trong kỳ  (50=20+30+40 )" , "50" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Tiền và tương đương tiền  đầu kỳ " , "60" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( false , "" , "Ảnh hưởng của thay đổi tỷ giá hối đoái quy đổi ngoại tệ" , "61" , "" , 0 , 0 ) );
            BindDataList.Add( new CashFlowDirectInfo( true , "" , "Tiền và tương đương tiền cuối kỳ  (70=50+60+61 )" , "70" , "" , 0 , 0 ) );


            this.gridControl1.DataSource=BindDataList;
            this.gridControl1.RefreshDataSource();

            DataList.Clear();
            foreach ( CashFlowDirectInfo info in BindDataList )
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
            DateTime dtPastStart=DateTime.MinValue;
            DateTime dtPastEnd=DateTime.MinValue;
            if ( Time.StatisticType==FinanceStatisticType.Year )
            {
                dtPastStart=Time.StartDate.AddYears( -1 );
                dtPastEnd=Time.EndDate.AddYears( -1 );
            }
            else if ( Time.StatisticType==FinanceStatisticType.Quater )
            {
                dtPastStart=Time.StartDate.AddMonths( -3 );
                dtPastEnd=Time.EndDate.AddMonths( -3 );
            }
            else if ( Time.StatisticType==FinanceStatisticType.RangeDate )
            {
                dtPastStart=DateTime.MinValue;
                dtPastEnd=Time.StartDate.AddSeconds( -1 );
            }

            #region 20
            if ( DataList.ContainsKey( "01" ) )
            {
                DataList["01"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "111" , "112" } , new List<string>() { "511" , "131" , "121" , "515" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["01"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "111" , "112" } , new List<string>() { "511" , "131" , "121" , "515" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "02" ) )
            {
                DataList["02"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "331" } , new List<string>() { "111" , "112" , "113" , "131" , "311" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["02"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "331" } , new List<string>() { "111" , "112" , "113" , "131" , "311" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "03" ) )
            {
                DataList["03"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "334" } , new List<string>() { "111" , "112" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["03"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "334" } , new List<string>() { "111" , "112" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "04" ) )
            {
                DataList["04"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "335" , "635" , "142" , "242" } , new List<string>() { "111" , "112" , "113" , "131" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["04"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "335" , "635" , "142" , "242" } , new List<string>() { "111" , "112" , "113" , "131" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "05" ) )
            {
                DataList["05"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "3334" } , new List<string>() { "111" , "112" , "113" , "131" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["05"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "3334" } , new List<string>() { "111" , "112" , "113" , "131" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "06" ) )
            {
                DataList["06"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "111" , "112" } , new List<string>() { "711" , "133" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["06"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "111" , "112" } , new List<string>() { "711" , "133" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "07" ) )
            {
                DataList["07"].Current=AccountingProvider.GetJournalAmount( new List<string>() { "811" , "333" , "161" , "351" , "352" } , new List<string>() { "111" , "112" , "113" } , Time.StartDate , Time.EndDate , "" , true );
                DataList["07"].Past=AccountingProvider.GetJournalAmount( new List<string>() { "811" , "333" , "161" , "351" , "352" } , new List<string>() { "111" , "112" , "113" } , dtPastStart , dtPastEnd , "" , true );
            }
            if ( DataList.ContainsKey( "20" ) )
            {
                DataList["20"].Current=DataList["01"].Current+DataList["02"].Current+DataList["03"].Current+DataList["04"].Current+DataList["05"].Current+DataList["06"].Current+DataList["07"].Current;
                DataList["20"].Past=DataList["01"].Past+DataList["02"].Past+DataList["03"].Past+DataList["04"].Past+DataList["05"].Past+DataList["06"].Past+DataList["07"].Past;
            } 
            #endregion
        
            
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
