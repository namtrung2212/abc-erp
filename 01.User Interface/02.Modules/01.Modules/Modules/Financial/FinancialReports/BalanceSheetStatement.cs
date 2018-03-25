using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Reflection;
using System.Reflection.Emit;
using ABCCommon;
using ABCBusinessEntities;
using ABCProvider;
using ABCControls;
using ABCScreen.Data;
using ABCModules;
using ABCProvider;

namespace ABCScreen
{


    public partial class BalanceSheetStatement : UserControl
    {
        BindingList<BalanceSheetInfo> BindAssetDataList=new BindingList<BalanceSheetInfo>();
        BindingList<BalanceSheetInfo> BindCapitalDataList=new BindingList<BalanceSheetInfo>();
        Dictionary<String , BalanceSheetInfo> DataList=new Dictionary<string , BalanceSheetInfo>();

        public String CompanyName=String.Empty;
        public String CompanyAddres=String.Empty;
        public FinanceStatisticTime Time;
        public BalanceSheetStatement ( String strCompany , String strAddress , FinanceStatisticTime time )
        {
            InitializeComponent();

            #region UI
            lblCompanyName.Text=strCompany;
            lblCompanyAddress.Text=strAddress;

            CompanyName=strCompany;
            CompanyAddres=strAddress;

            Time=time;



            for ( int i=ABCApp.ABCDataGlobal.WorkingDate.Year-10; i<=ABCApp.ABCDataGlobal.WorkingDate.Year+10; i++ )
                cmbYear.Properties.Items.Add( i );

            #endregion

            this.viewAsset.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( viewAsset_CustomDrawCell );
            this.viewCapital.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( viewCapital_CustomDrawCell );

            InitInformation();

            if ( Time.StatisticType==FinanceStatisticType.Year )
            {
                cmbYear.EditValue=time.EndDate.Year.ToString();
                cmbViewType.SelectedIndex=0;
            }
            if ( Time.StatisticType==FinanceStatisticType.Quater )
            {
                cmbYear.EditValue=time.EndDate.Year.ToString();
                cmbQuater.SelectedIndex=Convert.ToInt32( time.EndDate.Month/3 );
                cmbViewType.SelectedIndex=1;
            }
            if ( Time.StatisticType==FinanceStatisticType.RangeDate )
            {
                dtAtTime.EditValue=time.EndDate;
                cmbViewType.SelectedIndex=2;
            }

            InvalidateViewType();
            LoadData();
        }

        void InvalidateViewType ( )
        {
            if ( cmbViewType.SelectedIndex==0 )//Year
            {
                Time.StatisticType=FinanceStatisticType.Year;
                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
                Time.SetTime( Convert.ToInt32( cmbYear.EditValue ) );
            }
            if ( cmbViewType.SelectedIndex==1 )//Quater
            {
                Time.StatisticType=FinanceStatisticType.Quater;
                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
                if ( cmbQuater.SelectedIndex<0 )
                    cmbQuater.SelectedIndex=0;
                Time.SetTime( Convert.ToInt32( cmbYear.EditValue ) , cmbQuater.SelectedIndex+1 );
            }
            if ( cmbViewType.SelectedIndex==2 )//At Time
            {
                Time.StatisticType=FinanceStatisticType.RangeDate;
                if ( dtAtTime.EditValue==null )
                    dtAtTime.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
                Time.SetTime( (DateTime)dtAtTime.EditValue );
            }

        }
        void viewAsset_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
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

            BalanceSheetInfo info=viewAsset.GetRow( e.RowHandle ) as BalanceSheetInfo;

            if ( info!=null&&info.Bold )
            {
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
                e.Appearance.Options.UseFont=true;
            }
            else
                e.Appearance.Options.UseFont=false;
        }

        void viewCapital_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
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

            BalanceSheetInfo info=viewCapital.GetRow( e.RowHandle ) as BalanceSheetInfo;

            if ( info!=null&&info.Bold )
            {
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
                e.Appearance.Options.UseFont=true;
            }
            else
                e.Appearance.Options.UseFont=false;
        }

        void chkDetail_CheckedChanged ( object sender , EventArgs e )
        {
            if ( chkDetail.Checked )
            {
                viewAsset.ActiveFilterString="[Bold] = 'true'";
                viewCapital.ActiveFilterString="[Bold] = 'true'";
            }
            else
            {
                viewAsset.ActiveFilterString="";
                viewCapital.ActiveFilterString="";
            }
        }

        public void InitInformation ( )
        {
            #region Asset
            BindAssetDataList.Clear();

            BindAssetDataList.Add( new BalanceSheetInfo( true , "A" , "TÀI SẢN NGẮN HẠN  (100) =110+120+130+140+150)" , "100" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( true , "I" , "Tiền và các khoản tương đương tiền" , "110" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Tiền" , "111" , "V.01" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Các khoản tương đương tiền" , "112" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "II" , "Các khoản đầu tư tài chính ngắn hạn" , "120" , "V.02" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Đầu tư ngắn hạn" , "121" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Dự phòng giảm giá đầu tư ngắn hạn (*)" , "129" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "III" , "Các khoản phải thu" , "130" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Phải thu khách hàng " , "131" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Trả trước cho người bán" , "132" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Phải thu nội bộ ngắn hạn" , "133" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "4" , "Phải thu theo tiến độ kế hoạch hợp đồng xây dựng" , "134" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "5" , "Các khoản phải thu khác" , "135" , "V.03" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "6" , "Dự phòng các khoản phải thu khó đòi (*)" , "139" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "IV" , "Hàng tồn kho" , "140" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Hàng tồn kho" , "141" , "V.04" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Dự phòng giảm giá hàng tồn kho (*) " , "149" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "V" , "Tài sản ngắn hạn khác" , "150" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Chi phí trả trước ngắn hạn" , "151" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Thuế GTGT được khấu trừ" , "152" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Thuế và các khoản khác phải thu của nhà nước" , "154" , "V.05" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "4" , "Tài sản ngắn hạn khác" , "158" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "B" , "TÀI SẢN DÀI HẠN (200=210+220+240+250+260)" , "200" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( true , "I" , "Các khoản phải thu dài hạn " , "210" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Phải thu dài hạn của khách hàng" , "211" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Vốn kinh doanh ở đơn vị trực thuộc" , "212" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Phải thu nội bộ dài hạn" , "213" , "V.06" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "4" , " Phải thu dài hạn khác" , "218" , "V.07" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "5" , " Dự phòng phải thu dài hạn khó đòi (*)" , "219" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "II" , "Tài sản cố định" , "220" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Tài sản cố định hữu hình" , "221" , "V.08" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Nguyên giá" , "222" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Giá trị hao mòn luỹ kế (*)" , "223" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Tài sản cố định thuê tài chính" , "224" , "V.09" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Nguyên giá" , "225" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Giá trị hao mòn luỹ kế (*)" , "226" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Tài sản cố định vô hình" , "227" , "V.10" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Nguyên giá" , "228" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Giá trị hao mòn luỹ kế (*)" , "229" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "4" , "Chi phí xây dựng cơ bản dở dang" , "230" , "V.11" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "III" , "Bất động sản đầu tư" , "240" , "V12" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Nguyên giá" , "241" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "" , "      - Giá trị hao mòn luỹ kế (*)" , "242" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "IV" , "Các khoản đầu tư tài chính dài hạn" , "250" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Đầu tư vào công ty con" , "251" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Đầu tư vào công ty liên kết, liên doanh" , "252" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Đầu tư dài hạn khác" , "258" , "V.13" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "4" , "Dự phòng giảm giá chứng khoán đầu tư dài hạn (*)" , "259" , "" , 0 , 0 ) );

            BindAssetDataList.Add( new BalanceSheetInfo( true , "V" , "Tài sản dài hạn khác" , "260" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "1" , "Chi phí trả trước dài hạn" , "261" , "V.14" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "2" , "Tài sản thuế thu nhập hoãn lại" , "262" , "V.21" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( false , "3" , "Tài sản dài hạn khác" , "268" , "" , 0 , 0 ) );
            BindAssetDataList.Add( new BalanceSheetInfo( true , "" , "TỔNG CÔNG TÀI SẢN (270 = 100 + 200)" , "270" , "" , 0 , 0 ) );

            this.gridAssets.DataSource=BindAssetDataList;
            this.gridAssets.RefreshDataSource();
            #endregion

            #region Capital
            BindCapitalDataList.Clear();

            BindCapitalDataList.Add( new BalanceSheetInfo( true , "A" , "NỢ PHẢI TRẢ (300 = 310 + 320)" , "300" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( true , "I" , " Nợ ngắn hạn" , "310" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "1" , "Vay và nợ ngắn hạn" , "311" , "V.15" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "2" , "Phải trả người bán" , "312" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "3" , "Người mua trả tiền trước" , "313" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "4" , "Thuế và các khoản phải nộp Nhà nước" , "314" , "V.16" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "5" , "Phải trả người lao động" , "315" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "6" , "Chi phí phải trả" , "316" , "V.17" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "7" , "Phải trả nội bộ" , "317" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "8" , "Phải trả theo tiến độ kế hoạch hợp đồng xây dựng" , "318" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "9" , "Các khoản phải trả, phải nộp ngắn hạn khác" , "319" , "V.18" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "10" , "Dự phòng phải trả ngắn hạn" , "320" , "" , 0 , 0 ) );

            BindCapitalDataList.Add( new BalanceSheetInfo( true , "II" , "Nợ dài hạn" , "330" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "1" , "Phải trả dài hạn người bán" , "331" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "2" , "Phải trả dài hạn nội bộ" , "332" , "V.19" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "3" , "Phải trả dài hạn khác" , "333" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "4" , "Vay và nợ dài hạn" , "334" , "V.20" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "5" , "Thuế thu nhập hoãn lại phải trả" , "335" , "V.21" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "6" , "Dự phòng trợ cấp mất việc làm" , "336" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "7" , "Dự phòng phải trả dài hạn" , "337" , "" , 0 , 0 ) );

            BindCapitalDataList.Add( new BalanceSheetInfo( true , "B" , "VỐN CHỦ SỞ HỮU (400 = 410 + 420)" , "400" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( true , "I" , "Vốn chủ sở hữu" , "410" , "V.22" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "1" , "Vốn đầu tư của chủ sở hữu" , "411" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "2" , "Thặng dư vốn cổ phần" , "412" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "3" , "Vốn khác của chủ sở hữu" , "413" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "4" , "Cổ phiếu quỹ" , "414" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "5" , "Chênh lệch đánh giá lại tài sản" , "415" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "6" , "Chênh lệch tỷ giá hối đoái" , "416" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "7" , "Quỹ đầu tư phát triển " , "417" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "8" , "Quỹ dự phòng tài chính" , "418" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "9" , "Quỹ khác thuộc vốn chủ sở hữu" , "419" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "10" , "Lợi nhuận sau thuế chưa phân phối" , "420" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "11" , "Nguồn vốn đầu tư XDCB" , "421" , "" , 0 , 0 ) );

            BindCapitalDataList.Add( new BalanceSheetInfo( true , "II" , "Nguồn kinh phí và quỹ khác" , "430" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "1" , "Quỹ khen thưởng, phúc lợi" , "431" , "" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "2" , "Nguồn kinh phí" , "432" , "V.23" , 0 , 0 ) );
            BindCapitalDataList.Add( new BalanceSheetInfo( false , "3" , "Nguồn kinh phí đã hình thành TSCĐ " , "433" , "" , 0 , 0 ) );

            BindCapitalDataList.Add( new BalanceSheetInfo( true , "" , "TỔNG CỘNG NGUỒN VỐN (430 = 300 + 400)" , "440" , "" , 0 , 0 ) );

            this.gridCapital.DataSource=BindCapitalDataList;
            this.gridCapital.RefreshDataSource();
            #endregion

            DataList.Clear();
            foreach ( BalanceSheetInfo info in BindAssetDataList )
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
            foreach ( BalanceSheetInfo info in BindCapitalDataList )
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

        void AddTo ( String strFrom , String strTo )
        {
            if ( DataList.ContainsKey( strTo ) )
            {
                if ( DataList.ContainsKey( strFrom ) )
                {
                    DataList[strTo].Current+=DataList[strFrom].Current;
                    DataList[strTo].Past+=DataList[strFrom].Past;
                }
            }
        }
        public void LoadData ( )
        {

            #region Asset

            #region 100

            #region 110
            if ( DataList.ContainsKey( "111" ) )
            {
                DataList["111"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "111" , "112" , "113" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["111"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "111" , "112" , "113" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "112" ) )
            {
                DataList["112"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "121" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["112"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "121" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "110" ) )
            {
                DataList["110"].Current=0;
                DataList["110"].Past=0;
                AddTo( "111" , "110" );
                AddTo( "112" , "110" );
            }
            #endregion

            #region 120
            if ( DataList.ContainsKey( "121" ) )
            {
                DataList["121"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "121" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["121"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "121" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "129" ) )
            {
                DataList["129"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "129" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["129"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "129" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "120" ) )
            {
                DataList["120"].Current=0;
                DataList["120"].Past=0;
                AddTo( "121" , "120" );
                AddTo( "129" , "120" );
            }
            #endregion

            #region 130
            if ( DataList.ContainsKey( "131" ) )
            {
                DataList["131"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "131" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["131"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "131" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "132" ) )
            {
                DataList["132"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "331" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["132"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "331" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "133" ) )
            {
                DataList["133"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1368" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["133"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1368" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "134" ) )
            {
                DataList["134"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "337" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["134"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "337" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "135" ) )
            {
                DataList["135"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1385" , "1388" , "334" , "338" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["135"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1385" , "1388" , "334" , "338" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "139" ) )
            {
                DataList["139"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "139" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["139"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "139" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "130" ) )
            {
                DataList["130"].Current=0;
                DataList["130"].Past=0;
                AddTo( "131" , "130" );
                AddTo( "132" , "130" );
                AddTo( "133" , "130" );
                AddTo( "134" , "130" );
                AddTo( "135" , "130" );
                AddTo( "139" , "130" );
            }

            #endregion

            #region 140
            if ( DataList.ContainsKey( "141" ) )
            {
                DataList["141"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "151" , "152" , "153" , "154" , "155" , "156" , "157" , "158" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["141"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "151" , "152" , "153" , "154" , "155" , "156" , "157" , "158" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "149" ) )
            {
                DataList["149"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "159" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["149"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "159" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "140" ) )
            {
                DataList["140"].Current=0;
                DataList["140"].Past=0;
                AddTo( "141" , "140" );
                AddTo( "149" , "140" );
            }
            #endregion

            #region 150
            if ( DataList.ContainsKey( "151" ) )
            {
                DataList["151"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "142" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["151"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "142" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "152" ) )
            {
                DataList["152"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "133" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["152"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "133" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "154" ) )
            {
                DataList["154"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "333" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["154"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "333" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "158" ) )
            {
                DataList["158"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1381" , "141" , "144" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["158"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1381" , "141" , "144" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "150" ) )
            {
                DataList["150"].Current=0;
                DataList["150"].Past=0;
                AddTo( "151" , "150" );
                AddTo( "152" , "150" );
                AddTo( "154" , "150" );
                AddTo( "158" , "150" );
            }
            #endregion

            if ( DataList.ContainsKey( "100" ) )
            {
                DataList["100"].Current=0;
                DataList["100"].Past=0;
                AddTo( "110" , "100" );
                AddTo( "120" , "100" );
                AddTo( "130" , "100" );
                AddTo( "140" , "100" );
                AddTo( "150" , "100" );
            }
            #endregion

            #region 200

            #region 210
            if ( DataList.ContainsKey( "211" ) )
            {
                DataList["211"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "131" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["211"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "131" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "212" ) )
            {
                DataList["212"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1361" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["212"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1361" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "213" ) )
            {
                DataList["213"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1368" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["213"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "1368" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "218" ) )
            {
                DataList["218"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "138" , "331" , "338" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["218"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "138" , "331" , "338" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "219" ) )
            {
                DataList["219"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "139" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["219"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "139" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "210" ) )
            {
                DataList["210"].Current=0;
                DataList["210"].Past=0;
                AddTo( "211" , "210" );
                AddTo( "212" , "210" );
                AddTo( "213" , "210" );
                AddTo( "218" , "210" );
                AddTo( "219" , "210" );
            }
            #endregion

            #region 220

            #region 221
            if ( DataList.ContainsKey( "222" ) )
            {
                DataList["222"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "211" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["222"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "211" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "223" ) )
            {
                DataList["223"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2141" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["223"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2141" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "221" ) )
            {
                DataList["221"].Current=0;
                DataList["221"].Past=0;
                AddTo( "222" , "221" );
                AddTo( "223" , "221" );
            }
            #endregion

            #region 224
            if ( DataList.ContainsKey( "225" ) )
            {
                DataList["225"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "212" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["225"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "211" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "226" ) )
            {
                DataList["226"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2142" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["226"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2142" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "224" ) )
            {
                DataList["224"].Current=0;
                DataList["224"].Past=0;
                AddTo( "225" , "224" );
                AddTo( "226" , "224" );

            }
            #endregion

            #region 227
            if ( DataList.ContainsKey( "228" ) )
            {
                DataList["228"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "213" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["228"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "213" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "229" ) )
            {
                DataList["229"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2143" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["229"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2143" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "227" ) )
            {
                DataList["227"].Current=0;
                DataList["227"].Past=0;
                AddTo( "228" , "227" );
                AddTo( "229" , "227" );
            }
            #endregion

            if ( DataList.ContainsKey( "230" ) )
            {
                DataList["230"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "241" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["230"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "241" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }

            if ( DataList.ContainsKey( "220" ) )
            {
                DataList["220"].Current=0;
                DataList["220"].Past=0;
                AddTo( "221" , "220" );
                AddTo( "224" , "220" );
                AddTo( "227" , "220" );
                AddTo( "230" , "220" );
            }
            #endregion

            #region 240
            if ( DataList.ContainsKey( "241" ) )
            {
                DataList["241"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "217" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["241"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "217" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "242" ) )
            {
                DataList["242"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2147" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["242"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "2147" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "240" ) )
            {
                DataList["240"].Current=0;
                DataList["240"].Past=0;
                AddTo( "241" , "240" );
                AddTo( "242" , "240" );
            }
            #endregion

            #region 250
            if ( DataList.ContainsKey( "251" ) )
            {
                DataList["251"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "221" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["251"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "221" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "252" ) )
            {
                DataList["252"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "223" , "222" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["252"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "223" , "222" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "258" ) )
            {
                DataList["258"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "228" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["258"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "228" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "259" ) )
            {
                DataList["259"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "229" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["259"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "229" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }

            if ( DataList.ContainsKey( "250" ) )
            {
                DataList["250"].Current=0;
                DataList["250"].Past=0;
                AddTo( "251" , "250" );
                AddTo( "252" , "250" );
                AddTo( "258" , "250" );
                AddTo( "259" , "250" );
            }
            #endregion

            #region 260
            if ( DataList.ContainsKey( "261" ) )
            {
                DataList["261"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "242" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["261"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "242" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "262" ) )
            {
                DataList["262"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "243" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["262"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "243" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "268" ) )
            {
                DataList["268"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "244" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["268"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "244" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }

            if ( DataList.ContainsKey( "260" ) )
            {
                DataList["260"].Current=0;
                DataList["260"].Past=0;
                AddTo( "261" , "260" );
                AddTo( "262" , "260" );
                AddTo( "268" , "260" );
            }
            #endregion

            if ( DataList.ContainsKey( "200" ) )
            {
                DataList["200"].Current=0;
                DataList["200"].Past=0;
                AddTo( "210" , "200" );
                AddTo( "220" , "200" );
                AddTo( "240" , "200" );
                AddTo( "250" , "200" );
                AddTo( "260" , "200" );
            }
            #endregion

            if ( DataList.ContainsKey( "270" ) )
            {
                DataList["270"].Current=0;
                DataList["270"].Past=0;
                AddTo( "100" , "270" );
                AddTo( "200" , "270" );
            }

            #endregion

            #region Capital

            #region 300

            #region 310

            if ( DataList.ContainsKey( "311" ) )
            {
                DataList["311"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "315" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["311"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "315" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "312" ) )
            {
                DataList["312"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "331" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["312"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "331" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "313" ) )
            {
                DataList["313"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "131" , "3387" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["313"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "131" , "3387" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "314" ) )
            {
                DataList["314"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "333" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["314"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "333" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "315" ) )
            {
                DataList["315"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "334" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["315"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "334" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "316" ) )
            {
                DataList["316"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "335" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["316"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "335" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "317" ) )
            {
                DataList["317"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "336" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["317"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "336" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "318" ) )
            {
                DataList["318"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "337" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["318"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "337" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "319" ) )
            {
                DataList["319"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "338" , "138" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["319"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "338" , "138" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "320" ) )
            {
                DataList["320"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "352" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["320"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "352" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "310" ) )
            {
                DataList["310"].Current=0;
                DataList["310"].Past=0;
                AddTo( "311" , "310" );
                AddTo( "312" , "310" );
                AddTo( "313" , "310" );
                AddTo( "314" , "310" );
                AddTo( "315" , "310" );
                AddTo( "316" , "310" );
                AddTo( "317" , "310" );
                AddTo( "318" , "310" );
                AddTo( "319" , "310" );
                AddTo( "320" , "310" );
            }

            #endregion

            #region 330

            if ( DataList.ContainsKey( "331" ) )
            {
                DataList["331"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "331" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["331"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "331" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "332" ) )
            {
                DataList["332"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "336" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["332"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "336" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "333" ) )
            {
                DataList["333"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "338" , "344" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["333"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "338" , "344" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "334" ) )
            {
                DataList["334"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "341" , "342" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["334"].Current+=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "3431" , "3433" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["334"].Current-=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "3432" } , DateTime.MinValue , Time.EndDate , "" , true );

                DataList["334"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "341" , "342" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
                DataList["334"].Past+=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "3431" , "3433" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
                DataList["334"].Past-=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "3432" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "335" ) )
            {
                DataList["335"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "347" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["335"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "347" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "336" ) )
            {
                DataList["336"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "351" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["336"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "351" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "337" ) )
            {
                DataList["337"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "352" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["337"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "352" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }

            if ( DataList.ContainsKey( "330" ) )
            {
                DataList["330"].Current=0;
                DataList["330"].Past=0;
                AddTo( "331" , "330" );
                AddTo( "332" , "330" );
                AddTo( "333" , "330" );
                AddTo( "334" , "330" );
                AddTo( "335" , "330" );
                AddTo( "336" , "330" );
                AddTo( "337" , "330" );
            }

            #endregion

            if ( DataList.ContainsKey( "300" ) )
            {
                DataList["300"].Current=0;
                DataList["300"].Past=0;
                AddTo( "310" , "300" );
                AddTo( "330" , "300" );
            }

            #endregion

            #region 400

            #region 410

            if ( DataList.ContainsKey( "411" ) )
            {
                DataList["411"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4111" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["411"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4111" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "412" ) )
            {
                DataList["412"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4112" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["412"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4112" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "413" ) )
            {
                DataList["413"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4118" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["413"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "4118" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "414" ) )
            {
                DataList["414"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "419" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["414"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "419" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "415" ) )
            {
                DataList["415"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "412" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["415"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "412" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "416" ) )
            {
                DataList["416"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "413" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["416"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "413" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "417" ) )
            {
                DataList["417"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "414" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["417"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "414" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "418" ) )
            {
                DataList["418"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "415" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["418"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "415" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "419" ) )
            {
                DataList["419"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "418" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["419"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "418" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "420" ) )
            {
                DataList["420"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "421" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["420"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "421" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "421" ) )
            {
                DataList["421"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "441" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["421"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "441" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "410" ) )
            {
                DataList["410"].Current=0;
                DataList["410"].Past=0;
                AddTo( "411" , "410" );
                AddTo( "412" , "410" );
                AddTo( "413" , "410" );
                AddTo( "414" , "410" );
                AddTo( "415" , "410" );
                AddTo( "416" , "410" );
                AddTo( "417" , "410" );
                AddTo( "418" , "410" );
                AddTo( "419" , "410" );
                AddTo( "420" , "410" );
                AddTo( "421" , "410" );
            }

            #endregion

            #region 430

            if ( DataList.ContainsKey( "431" ) )
            {
                DataList["431"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "431" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["431"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "431" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "432" ) )
            {
                DataList["432"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "461" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["432"].Current-=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "161" } , DateTime.MinValue , Time.EndDate , "" , true );

                DataList["432"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "461" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
                DataList["432"].Past-=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Debit , new List<string>() { "161" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }
            if ( DataList.ContainsKey( "433" ) )
            {
                DataList["433"].Current=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "466" } , DateTime.MinValue , Time.EndDate , "" , true );
                DataList["433"].Past=AccountingProvider.GetAccountAmount( AccountingProvider.AccountType.Credit , new List<string>() { "466" } , DateTime.MinValue , Time.StartDate.AddSeconds( -1 ) , "" , true );
            }

            if ( DataList.ContainsKey( "430" ) )
            {
                DataList["430"].Current=0;
                DataList["430"].Past=0;
                AddTo( "431" , "430" );
                AddTo( "432" , "430" );
                AddTo( "433" , "430" );
            }

            #endregion

            if ( DataList.ContainsKey( "400" ) )
            {
                DataList["400"].Current=0;
                DataList["400"].Past=0;
                AddTo( "410" , "400" );
                AddTo( "430" , "400" );
            }

            #endregion

            if ( DataList.ContainsKey( "440" ) )
            {
                DataList["440"].Current=0;
                DataList["440"].Past=0;
                AddTo( "300" , "440" );
                AddTo( "400" , "440" );
            }

            #endregion

            this.gridAssets.DataSource=BindAssetDataList;
            this.gridAssets.RefreshDataSource();

            this.gridCapital.DataSource=BindCapitalDataList;
            this.gridCapital.RefreshDataSource();

            if ( Time.StatisticType==FinanceStatisticType.Year )
            {
                lblTime.Text="Năm "+Time.EndDate.Year;
                colCurrent.Caption="Cuối năm "+Time.EndDate.Year;
                colPast.Caption="Đầu năm "+Time.EndDate.Year;
            }
            else if ( Time.StatisticType==FinanceStatisticType.Quater )
            {
                if ( Time.EndDate.Month<=3 )
                {
                    lblTime.Text="Quý I"+" Năm "+Time.EndDate.Year;
                    colCurrent.Caption="Cuối Quý I";
                    colPast.Caption="Đầu Quý I";
                }
                if ( 4<=Time.StartDate.Month&&Time.EndDate.Month<=6 )
                {
                    lblTime.Text="Quý II"+" Năm "+Time.EndDate.Year;
                    colCurrent.Caption="Cuối Quý II";
                    colPast.Caption="Đầu Quý II";
                }
                if ( 7<=Time.StartDate.Month&&Time.EndDate.Month<=9 )
                {
                    lblTime.Text="Quý III"+" Năm "+Time.EndDate.Year;
                    colCurrent.Caption="Cuối Quý III";
                    colPast.Caption="Đầu Quý III";
                }
                if ( 10<=Time.StartDate.Month&&Time.EndDate.Month<=12 )
                {
                    lblTime.Text="Quý IV"+" Năm "+Time.EndDate.Year;
                    colCurrent.Caption="Cuối Quý IV";
                    colPast.Caption="Đầu Quý IV";
                }
            }
            else if ( Time.StatisticType==FinanceStatisticType.RangeDate )
            {
                lblTime.Text="";
                if ( Time.EndDate!=DateTime.MaxValue )
                    lblTime.Text="Tại ngày "+Time.EndDate.ToString( "dd/MM/yyyy" );

                colCurrent.Caption=Time.EndDate.ToString( "dd/MM/yyyy" );
                colPast.Caption="Đầu năm "+Time.EndDate.Year;
            }
            colCurrent2.Caption=colCurrent.Caption;
            colPast2.Caption=colPast.Caption;
        }

        private void btnView_Click ( object sender , EventArgs e )
        {
            ABCHelper.ABCWaitingDialog.Show();
            InvalidateViewType();
            LoadData();
            ABCHelper.ABCWaitingDialog.Close();
        }

        private void cmbViewType_SelectedIndexChanged ( object sender , EventArgs e )
        {
            if ( cmbViewType.SelectedIndex==0 )//Year
            {
                dtAtTime.Visible=false;
                cmbYear.Visible=true;
                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate.Year.ToString();

                cmbQuater.Visible=false;
            }
            if ( cmbViewType.SelectedIndex==1 )//Quater
            {
                dtAtTime.Visible=false;
                cmbYear.Visible=true;
                cmbQuater.Visible=true;

                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate.Year.ToString();
                if ( cmbQuater.EditValue==null )
                    cmbQuater.SelectedIndex=0;

            }
            if ( cmbViewType.SelectedIndex==2 )//At Time
            {
                dtAtTime.Visible=true;
                if ( dtAtTime.EditValue==null )
                    dtAtTime.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
                cmbYear.Visible=false;
                cmbQuater.Visible=false;
            }
        }

        private void gridAssets_DoubleClick ( object sender , EventArgs e )
        {
            BalanceSheetInfo row=viewAsset.GetFocusedRow() as BalanceSheetInfo;
            if ( row!=null )
            {
                GC.Collect();
                ABCHelper.ABCWaitingDialog.Show();
                ABCScreen.ABCBaseScreen scr=ABCScreen.ABCScreenFactory.GetABCScreen( "ModifiedEntrys" );
                if ( scr==null )
                    return;

                List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();
               
                #region 100

                #region 110

                if ( row.No=="100"||row.No=="110"||row.No=="111" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "111" , "112" , "113" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="110"||row.No=="112" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "112" } , Time.StartDate , Time.EndDate , "" , true ) );
           
                #endregion

                #region 120
                if ( row.No=="100"||row.No=="120"||row.No=="121" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "121" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="120"||row.No=="129" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "129" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #region 130
                if ( row.No=="100"||row.No=="130"||row.No=="131" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "131" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="130"||row.No=="132" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "331" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="130"||row.No=="133" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "1368" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="130"||row.No=="134" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "337" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="130"||row.No=="135" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "1385" , "1388" , "334" , "338" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="130"||row.No=="139" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "139" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #region 140
                if ( row.No=="100"||row.No=="140"||row.No=="141" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "151" , "152" , "153" , "154" , "155" , "156" , "157" , "158" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="140"||row.No=="149" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "159" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #region 150
                if ( row.No=="100"||row.No=="150"||row.No=="151" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "142" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="150"||row.No=="152" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "133" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="150"||row.No=="154" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "333" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="100"||row.No=="150"||row.No=="158" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "1381" , "141" , "144" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #endregion

                #region 200

                #region 210
                if ( row.No=="200"||row.No=="210"||row.No=="211" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "131" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="210"||row.No=="212" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "1361" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="210"||row.No=="213" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "1368" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="210"||row.No=="218" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "138" , "331" , "338" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="210"||row.No=="219" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "139" } , Time.StartDate , Time.EndDate , "" , true ) );

                #endregion

                #region 220

                #region 221
                if ( row.No=="200"||row.No=="221"||row.No=="222" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "211" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="221"||row.No=="223" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "2141" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #region 224
                if ( row.No=="200"||row.No=="224"||row.No=="225" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "212" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="224"||row.No=="226" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "2142" } , Time.StartDate , Time.EndDate , "" , true ) );
              
                #endregion

                #region 227

                if ( row.No=="200"||row.No=="227"||row.No=="228" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "213" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="227"||row.No=="229" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "2143" } , Time.StartDate , Time.EndDate , "" , true ) );
             
                #endregion

                if ( row.No=="200"||row.No=="230" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "241" } , Time.StartDate , Time.EndDate , "" , true ) );

                #endregion

                #region 240

                if ( row.No=="200"||row.No=="240"||row.No=="241" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "217" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="240"||row.No=="242" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "2147" } , Time.StartDate , Time.EndDate , "" , true ) );

                #endregion

                #region 250

                if ( row.No=="200"||row.No=="250"||row.No=="251" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "221" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="250"||row.No=="252" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "223" , "222" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="250"||row.No=="258" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "228" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="250"||row.No=="259" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "229" } , Time.StartDate , Time.EndDate , "" , true ) );

                #endregion

                #region 260
                if ( row.No=="200"||row.No=="260"||row.No=="261" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "242" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="260"||row.No=="262" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "243" } , Time.StartDate , Time.EndDate , "" , true ) );

                if ( row.No=="200"||row.No=="260"||row.No=="268" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "244" } , Time.StartDate , Time.EndDate , "" , true ) );
                #endregion

                #endregion

          
                ABCList<GLJournalEntrysInfo> lstEntrys=(ABCList<GLJournalEntrysInfo>)scr.DataManager["objGLJournalEntrys"];
                lstEntrys.Invalidate( lstResults );
                scr.Show( String.Format( @"Phát sinh : {0} - {1}" , row.No , row.Factor ) );

                ABCHelper.ABCWaitingDialog.Close();
            }
        }

        private void gridCapital_DoubleClick ( object sender , EventArgs e )
        {
            BalanceSheetInfo row=viewCapital.GetFocusedRow() as BalanceSheetInfo;
            if ( row!=null )
            {
                GC.Collect();
                ABCHelper.ABCWaitingDialog.Show();
                ABCScreen.ABCBaseScreen scr=ABCScreen.ABCScreenFactory.GetABCScreen( "ModifiedEntrys" );
                if ( scr==null )
                    return;

                List<GLJournalEntrysInfo> lstResults=new List<GLJournalEntrysInfo>();

                #region 300

                #region 310
                if ( row.No=="300"||row.No=="310"||row.No=="311" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "315" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="312" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "331" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="313" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "131" , "3387" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="314" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "333" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="315" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "334" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="316" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "335" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="317" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "336" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="318" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "337" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="319" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "338" , "138" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="310"||row.No=="320" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "352" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                #endregion

                #region 330
                if ( row.No=="300"||row.No=="330"||row.No=="331" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "331" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="330"||row.No=="332" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "336" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="330"||row.No=="333" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "338" , "344" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="330"||row.No=="334" )
                {
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "341" , "342" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "3431" , "3433" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "3432" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );
                }

                if ( row.No=="300"||row.No=="330"||row.No=="335" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "347" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="330"||row.No=="336" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "351" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="300"||row.No=="330"||row.No=="337" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "352" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                #endregion

                #endregion

                #region 400

                #region 410
                if ( row.No=="400"||row.No=="410"||row.No=="411" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "4111" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="412" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "4112" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="413" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "4118" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="414" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "419" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="415" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "412" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="416" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "413" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="417" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "414" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="418" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "415" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="419" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "418" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="420" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "421" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="410"||row.No=="421" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "441" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                #endregion

                #region 430
                if ( row.No=="400"||row.No=="430"||row.No=="431" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "431" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                if ( row.No=="400"||row.No=="430"||row.No=="432" )
                {
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "461" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "161" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );
                }

                if ( row.No=="400"||row.No=="430"||row.No=="433" )
                    lstResults.AddRange( AccountingProvider.GetEntrys( new List<string>() { "466" } , Time.StartDate , Time.EndDate , " (EntryType IS NULL OR EntryType != 'PeriodEnding') " , true ) );

                #endregion

                #endregion

                ABCList<GLJournalEntrysInfo> lstEntrys=(ABCList<GLJournalEntrysInfo>)scr.DataManager["objGLJournalEntrys"];
                lstEntrys.Invalidate( lstResults );
                scr.Show( String.Format( @"Phát sinh : {0} - {1}" , row.No , row.Factor ) );
                ABCHelper.ABCWaitingDialog.Close();
            }
        }


    }

}
