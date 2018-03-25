namespace ABCControls.Designer.ReportDesigner
{
    partial class XtraReport1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing&&( components!=null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.Detail=new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin=new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin=new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel1=new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2=new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3=new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1=new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1=new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1=new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2=new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3=new DevExpress.XtraReports.UI.XRTableCell();
            ( (System.ComponentModel.ISupportInitialize)( this.xrTable1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this ) ).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange( new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrLabel2} );
            this.Detail.Name="Detail";
            this.Detail.Padding=new DevExpress.XtraPrinting.PaddingInfo( 0 , 0 , 0 , 0 , 100F );
            this.Detail.TextAlignment=DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange( new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1} );
            this.TopMargin.Name="TopMargin";
            this.TopMargin.Padding=new DevExpress.XtraPrinting.PaddingInfo( 0 , 0 , 0 , 0 , 100F );
            this.TopMargin.TextAlignment=DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange( new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3} );
            this.BottomMargin.Name="BottomMargin";
            this.BottomMargin.Padding=new DevExpress.XtraPrinting.PaddingInfo( 0 , 0 , 0 , 0 , 100F );
            this.BottomMargin.TextAlignment=DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.LocationFloat=new DevExpress.Utils.PointFloat( 202.0833F , 53.08334F );
            this.xrLabel1.Name="xrLabel1";
            this.xrLabel1.Padding=new DevExpress.XtraPrinting.PaddingInfo( 2 , 2 , 0 , 0 , 96F );
            this.xrLabel1.SizeF=new System.Drawing.SizeF( 100F , 23F );
            this.xrLabel1.Text="xrLabel1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.LocationFloat=new DevExpress.Utils.PointFloat( 165.625F , 27.04166F );
            this.xrLabel2.Name="xrLabel2";
            this.xrLabel2.Padding=new DevExpress.XtraPrinting.PaddingInfo( 2 , 2 , 0 , 0 , 96F );
            this.xrLabel2.SizeF=new System.Drawing.SizeF( 100F , 23F );
            this.xrLabel2.Text="xrLabel2";
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat=new DevExpress.Utils.PointFloat( 165.625F , 27.04166F );
            this.xrLabel3.Name="xrLabel3";
            this.xrLabel3.Padding=new DevExpress.XtraPrinting.PaddingInfo( 2 , 2 , 0 , 0 , 96F );
            this.xrLabel3.SizeF=new System.Drawing.SizeF( 100F , 23F );
            this.xrLabel3.Text="xrLabel3";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat=new DevExpress.Utils.PointFloat( 294.7917F , 10.00001F );
            this.xrTable1.Name="xrTable1";
            this.xrTable1.Rows.AddRange( new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1} );
            this.xrTable1.SizeF=new System.Drawing.SizeF( 300F , 25F );
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange( new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3} );
            this.xrTableRow1.Name="xrTableRow1";
            this.xrTableRow1.Weight=1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name="xrTableCell1";
            this.xrTableCell1.Text="xrTableCell1";
            this.xrTableCell1.Weight=1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name="xrTableCell2";
            this.xrTableCell2.Text="xrTableCell2";
            this.xrTableCell2.Weight=1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name="xrTableCell3";
            this.xrTableCell3.Text="xrTableCell3";
            this.xrTableCell3.Weight=1D;
            // 
            // XtraReport1
            // 
            this.Bands.AddRange( new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin} );
            this.Version="11.2";
            ( (System.ComponentModel.ISupportInitialize)( this.xrTable1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this ) ).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    }
}
