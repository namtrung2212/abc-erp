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

    public partial class ProfitAndLossStatement : UserControl
    {
        BindingList<ProfitAndLossInfo> BindDataList=new BindingList<ProfitAndLossInfo>();
        Dictionary<String , ProfitAndLossInfo> DataList=new Dictionary<string , ProfitAndLossInfo>();

        public String CompanyName=String.Empty;
        public String CompanyAddres=String.Empty;
        public FinanceStatisticTime Time;
        public ProfitAndLossStatement ( String strCompany , String strAddress , FinanceStatisticTime time )
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

            this.gridView1.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridView1_CustomDrawCell );
        
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
                dtTimeFrom.EditValue=time.StartDate;
                dtTimeTo.EditValue=time.EndDate;
                cmbViewType.SelectedIndex=2;
            }

            InvalidateViewType();
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
        }


        public void InitInformation ( )
        {
            BindDataList.Clear();

            BindDataList.Add( new ProfitAndLossInfo( "1" , "Doanh thu bán hàng và cung cấp dịch vụ" , "01" , "VI.25" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "2" , "Các khoản giảm trừ doanh thu" , "02" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "3" , "Doanh thu thuần về bán hàng và cung cấp dịch vụ (10 = 01 - 02)" , "10" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "4" , "Giá vốn hàng bán" , "11" , "VI.27" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "5" , "Lợi nhuận gộp về bán hàng và cung cấp dịch vụ (20 = 10-11)" , "20" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "6" , "Doanh thu hoạt động tài chính" , "21" , "VI.26" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "7" , "Chi phí tài chính" , "22" , "VI.28" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "" , "      -  Trong đó : Chi phí lãi vay" , "23" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "8" , "Chi phí bán hàng" , "24" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "9" , "Chi phí quản lý doanh nghiệp " , "25" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "10" , "Lợi nhuận thuần từ hoạt động kinh doanh {30 = 20+(21-22)-(24+25)} " , "30" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "11" , "Thu nhập  khác " , "31" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "12" , "Chi phí khác" , "32" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "13" , "Lợi nhuận khác :  ( 40 = 31 - 32 )" , "40" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "14" , "Tổng lợi nhuận kế toán trước thuế : ( 50 = 30 + 40 )" , "50" , "" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "15" , "Chi phí thuế TNDN hiện hành" , "51" , "VI.30" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "16" , "Chi phí thuế TNDN hoãn lại" , "52" , "VI.30" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "17" , "Lợi nhuận sau thuế thu nhập doanh nghiệp :  (60 = 50 - 51 )" , "60" , "VI.30" , 0 , 0 ) );
            BindDataList.Add( new ProfitAndLossInfo( "18" , "Lãi cơ bản trên cổ phiếu" , "70" , "" , 0 , 0 ) );

            this.gridControl1.DataSource=BindDataList;
            this.gridControl1.RefreshDataSource();

            DataList.Clear();
            foreach ( ProfitAndLossInfo info in BindDataList )
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
            GLBenifitCalcsInfo benifit1=null;//=FinancialProvider.BenifitCalculationOfRange( Time.StartDate , Time.EndDate );
            GLBenifitCalcsInfo benifit2=null;//=FinancialProvider.BenifitCalculationOfRange( Time.StartDate , Time.EndDate );
            if ( Time.StatisticType==FinanceStatisticType.Year )
            {
                benifit1=FinancialProvider.BenifitCalculationOfRange( Time.StartDate , Time.EndDate );
                benifit2=FinancialProvider.BenifitCalculationOfRange( Time.StartDate.AddYears( -1 ) , Time.EndDate.AddYears( -1 ) );
            }
            else if ( Time.StatisticType==FinanceStatisticType.Quater )
            {
                benifit1=FinancialProvider.BenifitCalculationOfRange( Time.StartDate , Time.EndDate );
                benifit2=FinancialProvider.BenifitCalculationOfRange( Time.StartDate.AddMonths( -3 ) , Time.EndDate.AddMonths( -3 ) );
            }
            else if ( Time.StatisticType==FinanceStatisticType.RangeDate )
            {
                benifit1=FinancialProvider.BenifitCalculationOfRange( Time.StartDate , Time.EndDate );
                benifit2=FinancialProvider.BenifitCalculationOfRange( DateTime.MinValue , Time.StartDate.AddSeconds( -1) );
            }
            if ( benifit1==null||benifit2==null )
                return;

           if ( DataList.ContainsKey( "01" ) )
            {
                DataList["01"].Current=benifit1.RevenueGrossSale;
                DataList["01"].Past=benifit2.RevenueGrossSale;
           }
           if ( DataList.ContainsKey( "02" ) )
           {
               DataList["02"].Current=benifit1.RevenueDecrease;
               DataList["02"].Past=benifit2.RevenueDecrease;
           }
           if ( DataList.ContainsKey( "10" ) )
           {
               DataList["10"].Current=benifit1.RevenueNetSale;
               DataList["10"].Past=benifit2.RevenueNetSale;
           }
           if ( DataList.ContainsKey( "11" ) )
           {
               DataList["11"].Current=benifit1.CostOfGoodSolved;
               DataList["11"].Past=benifit2.CostOfGoodSolved;
           }
           if ( DataList.ContainsKey( "20" ) )
           {
               DataList["20"].Current=benifit1.BenifitGrossSale;
               DataList["20"].Past=benifit2.BenifitGrossSale;
           }
           if ( DataList.ContainsKey( "21" ) )
           {
               DataList["21"].Current=benifit1.RevenueCommercial;
               DataList["21"].Past=benifit2.RevenueCommercial;
           }
           if ( DataList.ContainsKey( "22" ) )
           {
               DataList["22"].Current=benifit1.CostCommercial;
               DataList["22"].Past=benifit2.CostCommercial;
           }
           if ( DataList.ContainsKey( "24" ) )
           {
               DataList["24"].Current=benifit1.CostSale;
               DataList["24"].Past=benifit2.CostSale;
           }
           if ( DataList.ContainsKey( "25" ) )
           {
               DataList["25"].Current=benifit1.CostManage;
               DataList["25"].Past=benifit2.CostManage;
           }
           if ( DataList.ContainsKey( "30" ) )
           {
               DataList["30"].Current=benifit1.BenifitNet;
               DataList["30"].Past=benifit2.BenifitNet;
           }
           if ( DataList.ContainsKey( "31" ) )
           {
               DataList["31"].Current=benifit1.RevenueOthers;
               DataList["31"].Past=benifit2.RevenueOthers;
           }
           if ( DataList.ContainsKey( "32" ) )
           {
               DataList["32"].Current=benifit1.CostOthers;
               DataList["32"].Past=benifit2.CostOthers;
           }
           if ( DataList.ContainsKey( "40" ) )
           {
               DataList["40"].Current=benifit1.BenifitOthers;
               DataList["40"].Past=benifit2.BenifitOthers;
           }
           if ( DataList.ContainsKey( "50" ) )
           {
               DataList["50"].Current=benifit1.BenifitBeforeTax;
               DataList["50"].Past=benifit2.BenifitBeforeTax;
           }
           if ( DataList.ContainsKey( "51" ) )
           {
               DataList["51"].Current=benifit1.CostIncomeTax;
               DataList["51"].Past=benifit2.CostIncomeTax;
           }
           if ( DataList.ContainsKey( "52" ) )
           {
               DataList["52"].Current=benifit1.CostIncomeTaxDelay;
               DataList["52"].Past=benifit2.CostIncomeTaxDelay;
           }
           if ( DataList.ContainsKey( "60" ) )
           {
               DataList["60"].Current=benifit1.Benifit;
               DataList["60"].Past=benifit2.Benifit;
           }

            this.gridControl1.DataSource=BindDataList;
            this.gridControl1.RefreshDataSource();


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
                if ( dtTimeFrom.EditValue==null )
                    dtTimeFrom.EditValue=ABCApp.ABCDataGlobal.WorkingDate.AddMonths( -1 );
                if ( dtTimeTo.EditValue==null )
                    dtTimeTo.EditValue=ABCApp.ABCDataGlobal.WorkingDate;

                Time.SetTime( (DateTime)dtTimeFrom.EditValue , (DateTime)dtTimeTo.EditValue );
            }

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
                panelRangeDate.Visible=false;
                cmbYear.Visible=true;
                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate.Year.ToString();

                cmbQuater.Visible=false;
            }
            if ( cmbViewType.SelectedIndex==1 )//Quater
            {
                panelRangeDate.Visible=false;
                cmbYear.Visible=true;
                cmbQuater.Visible=true;

                if ( cmbYear.EditValue==null )
                    cmbYear.EditValue=ABCApp.ABCDataGlobal.WorkingDate.Year.ToString();
                if ( cmbQuater.EditValue==null )
                    cmbQuater.SelectedIndex=0;

            }
            if ( cmbViewType.SelectedIndex==2 )//At Time
            {
                panelRangeDate.Visible=true;
                {
                    if ( dtTimeFrom.EditValue==null )
                        dtTimeFrom.EditValue=ABCApp.ABCDataGlobal.WorkingDate.AddMonths( -1 );
                    if ( dtTimeTo.EditValue==null )
                        dtTimeTo.EditValue=ABCApp.ABCDataGlobal.WorkingDate;
                }
                cmbYear.Visible=false;
                cmbQuater.Visible=false;
            }
        }

    
    }


}
