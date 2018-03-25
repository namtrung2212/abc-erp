using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraEditors;
using  DevExpress.XtraCharts;

using ABCProvider;
using ABCBusinessEntities;

namespace ABCControls
{
    public class ABCChartBaseSeries : DevExpress.XtraCharts.Series
    {

        #region Binding

        [Category( "ABC.BindingValue" )]
        public String DataSourceName { get; set; }
        [Category( "ABC.BindingValue" )]
        public String TableName { get; set; }
        [Category( "ABC.BindingValue" )]
        public String ArgumentDisplayCol { get; set; }

        String valueMembers=String.Empty;
        [Category( "ABC.BindingValue" )]
        public String ValueMembers
        {
            get { return valueMembers.Trim(); }
            set
            {
                valueMembers=value.Trim();
                this.ValueDataMembers.Clear();
                this.ValueDataMembers.AddRange( value.ToString().Split( ';' ) );

            }
        }

        #endregion

        public ChartControl ChartParent;

        public ABCChartBaseSeries ( ABCChartBaseControl parentChart )
        {
            ChartParent=parentChart.InnerChart;
        }

        public DevExpress.XtraCharts.SeriesLabelBase seriesLabel;
        public DevExpress.XtraCharts.SeriesViewBase seriesView;
        public DevExpress.XtraCharts.PointOptions pointOptions;

        public virtual void BeginInitialize ( )
        {
            if ( seriesLabel==null )
                seriesLabel=new DevExpress.XtraCharts.SideBySideBarSeriesLabel();

            if ( seriesView==null )
                seriesView=new DevExpress.XtraCharts.SideBySideBarSeriesView();

            if ( pointOptions==null )
                pointOptions=new DevExpress.XtraCharts.PointOptions();

            ( (System.ComponentModel.ISupportInitialize)( this ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( seriesLabel ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( seriesView ) ).BeginInit();

        }

        public virtual void EndInitialize ( )
        {
            ( (System.ComponentModel.ISupportInitialize)( seriesLabel ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( seriesView ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this ) ).EndInit();
        }

        public virtual void InitSeries ( )
        {
            this.ArgumentScaleType=DevExpress.XtraCharts.ScaleType.Qualitative;
            seriesLabel.Antialiasing=true;
            seriesLabel.BackColor=ABCPresentHelper.GetSkinBackColor();
            seriesLabel.Border.Visible=false;
            seriesLabel.FillStyle.FillMode=DevExpress.XtraCharts.FillMode.Empty;
            seriesLabel.LineColor=ABCPresentHelper.GetSkinForeColor();//System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 64 ) ) ) ) , ( (int)( ( (byte)( 64 ) ) ) ) , ( (int)( ( (byte)( 64 ) ) ) ) );
            seriesLabel.LineVisible=true;

            pointOptions.PointView=DevExpress.XtraCharts.PointView.Values;
            pointOptions.ValueNumericOptions.Format=DevExpress.XtraCharts.NumericFormat.General;
            seriesLabel.PointOptions=pointOptions;

            seriesLabel.ResolveOverlappingMode=DevExpress.XtraCharts.ResolveOverlappingMode.Default;
            seriesLabel.TextColor=ABCPresentHelper.GetSkinForeColor();//System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 0 ) ) ) ) , ( (int)( ( (byte)( 0 ) ) ) ) , ( (int)( ( (byte)( 128 ) ) ) ) );
            this.Label=seriesLabel;
            this.Name="Biểu đồ 1";
            this.View=seriesView;
            this.LegendPointOptions.PointView=PointView.Argument;
            ChartParent.Series.Add( this );
            ColorEach=true;

            if ( this.seriesView is SideBySideBarSeriesView )
            {
                ( this.seriesView as SideBySideBarSeriesView ).Shadow.Visible=true;
                ( this.seriesView as SideBySideBarSeriesView ).Shadow.Size=2;
                ( (BarSeriesLabel)Label ).ShowForZeroValues=false;
            }
            if ( this.seriesView is LineSeriesView )
            {
                ( this.seriesView as LineSeriesView ).Shadow.Visible=false;
                ( this.seriesView as LineSeriesView ).LineMarkerOptions.Visible=false;

            }

   
        }

        #region Properties

        [Category( "Appearance" )]
        public String Caption
        {
            get { return this.LegendText; }
            set { this.LegendText=value; }
        }

        [Category( "Appearance" )]
        public DevExpress.XtraCharts.ViewType SeriesViewType
        {
            get { return DevExpress.XtraCharts.Native.SeriesViewFactory.GetViewType( this.View ); }
            set
            {
                ChangeChartType( value );
            }
        }

        public void ChangeChartType ( DevExpress.XtraCharts.ViewType type )
        {
            try
            {
                this.ChangeView( type );

                if ( ( this.View as DevExpress.XtraCharts.SimpleDiagramSeriesViewBase )==null )
                    ChartParent.Legend.Visible=true;

                if ( ChartParent!=null&&ChartParent.Diagram is DevExpress.XtraCharts.Diagram3D )
                {
                    DevExpress.XtraCharts.Diagram3D diagram=(DevExpress.XtraCharts.Diagram3D)ChartParent.Diagram;
                    diagram.RuntimeRotation=true;
                    diagram.RuntimeZooming=true;
                    diagram.RuntimeScrolling=true;
                }
                foreach ( DevExpress.XtraCharts.Series series in ChartParent.Series )
                {
                    DevExpress.XtraCharts.ISupportTransparency supportTransparency=series.View as DevExpress.XtraCharts.ISupportTransparency;
                    if ( supportTransparency!=null )
                    {
                        if ( ( series.View is DevExpress.XtraCharts.AreaSeriesView )||( series.View is DevExpress.XtraCharts.Area3DSeriesView )
                            ||( series.View is DevExpress.XtraCharts.RadarAreaSeriesView )||( series.View is DevExpress.XtraCharts.Bar3DSeriesView ) )
                            supportTransparency.Transparency=135;
                        else
                            supportTransparency.Transparency=0;
                    }
                }
            }
            catch ( Exception ex )
            {
            }
        }

        [Category( "Appearance" )]
        public bool ColorEach
        {
            get
            {
                if ( this.View is DevExpress.XtraCharts.Native.IColorEachSupportView )
                    return ( this.View as DevExpress.XtraCharts.Native.IColorEachSupportView ).ColorEach;
                else
                    return false;
            }
            set
            {
                if ( this.View is DevExpress.XtraCharts.Native.IColorEachSupportView )
                    ( this.View as DevExpress.XtraCharts.Native.IColorEachSupportView ).ColorEach=value;
            }
        }

        [Category( "Value" )]
        public bool ValueAsPercent
        {
            get
            {
                if ( this.Label.PointOptions is SimplePointOptions )
                    return ( this.Label.PointOptions as SimplePointOptions ).PercentOptions.ValueAsPercent;
                else
                    return false;
            }
            set
            {
                if ( this.Label.PointOptions is SimplePointOptions )
                    ( this.Label.PointOptions as SimplePointOptions ).PercentOptions.ValueAsPercent=value;
            }
        }



        #endregion



        protected BindingSource bindingSource;

        public virtual void InitBinding ( String strObjectName , BindingSource binding )
        {
            try
            {
                this.bindingSource=binding;
                bindingSource.ListChanged+=new ListChangedEventHandler( BindingSource_ListChanged );
                bindingSource.PositionChanged+=new EventHandler( bindingSource_PositionChanged );
                if ( this.ValueMembers.Contains( ";" )==false )
                    this.DataSource=binding;
            }
            catch ( Exception ex )
            {
                ABCHelper.ABCMessageBox.Show( ex.Message );
            }
        }

        void bindingSource_PositionChanged ( object sender , EventArgs e )
        {
                if ( this.ValueMembers.Contains( ";" ) )
                    CreatePoints( bindingSource.Current );
        }

        public virtual void BindingSource_ListChanged ( object sender , ListChangedEventArgs e )
        {
            if ( e.ListChangedType==ListChangedType.Reset )
                if ( this.ValueMembers.Contains( ";" ) )
                    CreatePoints( bindingSource.Current );
        }

        public void CreatePoints ( object obj )
        {
            this.Points.Clear();
            if ( obj==null )
                return;

            String[] strArrays=this.ValueMembers.Split( ';' );
            int i=0;
            foreach ( String strItem in strArrays )
            {
                PropertyInfo proInfo=obj.GetType().GetProperty( strItem );
                if ( proInfo==null )
                    return;

                object objValue=proInfo.GetValue( obj , null );

                i++;
                String strCaption=String.Empty;
                if ( obj is BusinessObject )
                    strCaption=DataConfigProvider.GetFieldCaption( ( obj as BusinessObject ).AATableName , strItem );

                SeriesPoint point=new SeriesPoint( strCaption , new object[] { objValue } , i );
                this.Points.Add( point );
            }
        }

    }
}
