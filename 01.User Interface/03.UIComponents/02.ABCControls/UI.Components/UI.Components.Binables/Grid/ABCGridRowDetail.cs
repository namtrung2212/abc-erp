using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;

using ABCProvider;
using ABCCommon;

namespace ABCControls
{
    public partial class ABCGridRowDetail : DevExpress.XtraEditors.XtraForm
    {
        public IABCGridControl ABCGrid;
        BindingSource BindingSource;
        public ABCGridRowDetail (IABCGridControl grid )
        {
            ABCGrid=grid;
            InitializeComponent();
            Initialize();

            this.VGrid.Appearance.DisabledRow.BackColor=Color.FromArgb( 181 , 200 , 223 );
            this.VGrid.Appearance.DisabledRow.ForeColor=Color.Black;
            this.VGrid.Appearance.DisabledRow.Options.UseBackColor=true;
            this.VGrid.Appearance.DisabledRow.Options.UseForeColor=true;
            this.VGrid.Appearance.DisabledRecordValue.BackColor=Color.FromArgb( 181 , 200 , 223 );
            this.VGrid.Appearance.DisabledRecordValue.ForeColor=Color.Black;
            this.VGrid.Appearance.DisabledRecordValue.Options.UseBackColor=true;
            this.VGrid.Appearance.DisabledRecordValue.Options.UseForeColor=true;
            this.VGrid.Appearance.Empty.BackColor=Color.FromArgb( 181 , 200 , 223 );
            this.VGrid.Appearance.Empty.Options.UseBackColor=true;

            this.Shown+=new EventHandler( ABCGridRowDetail_Shown );
            this.FormClosing+=new FormClosingEventHandler( ABCGridRowDetail_FormClosing );
            this.VGrid.CustomUnboundData+=new DevExpress.XtraVerticalGrid.Events.CustomDataEventHandler( VGrid_CustomUnboundData );
            this.VGrid.CustomDrawRowValueCell+=new DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventHandler( vGridControl1_CustomDrawRowValueCell );
            this.VGrid.MouseClick+=new MouseEventHandler(VGrid_MouseClick);
            (this.ABCGrid as Control).FindForm().LocationChanged+=new EventHandler( ABCGridRowDetail_LocationChanged );
            ( this.ABCGrid as Control ).FindForm().FormClosing+=new FormClosingEventHandler( ParentForm_FormClosing );
        }
        protected override void OnShown ( EventArgs e )
        {
            base.OnShown( e );

            this.VGrid.ViewInfo.RC.treeButSize=Size.Empty;
            this.VGrid.LayoutChanged();
        }

        void ABCGridRowDetail_LocationChanged ( object sender , EventArgs e )
        {
            Form form=sender as Form;
            if ( form!=null&&form.WindowState==FormWindowState.Normal )
            {
                this.Location=new Point( form.Left+form.Width , form.Top );
            }
            if ( this.TopLevel==false )
                this.BringToFront();
        }

     

      
        
        #region Accross Table DisplayText

        void VGrid_CustomUnboundData ( object sender , DevExpress.XtraVerticalGrid.Events.CustomDataEventArgs e )
        {
            if ( e.Row!=null&&( e.Row.Tag is ABCGridColumn.ColumnConfig||e.Row.Tag is ABCGridBandedColumn.ColumnConfig )&&
               ( this.ABCGrid==null||
                       ( this.ABCGrid.OwnerView!=null&&this.ABCGrid.OwnerView.Mode!=ViewMode.Design ) ) )
            {
                if ( e.Row.Index>=0&&String.IsNullOrWhiteSpace( this.ABCGrid.TableName )==false&&e.Row.Properties.FieldName.Contains( ":" ) )
                {
                    EditorRow gridRow=e.Row as EditorRow;
                    object objDataRow=this.VGrid.GetRecordObject( e.ListSourceRowIndex );
                    if ( objDataRow==null )
                        return;

                    String strBaseField=e.Row.Properties.FieldName.Split( ':' )[0];
                    PropertyInfo proInfo=objDataRow.GetType().GetProperty( strBaseField );
                    if ( proInfo==null )
                        return;

                    object objValue=proInfo.GetValue( objDataRow , null );
                    e.Value=DataCachingProvider.GetCachingObjectAccrossTable( this.ABCGrid.TableName , ABCHelper.DataConverter.ConvertToGuid( objValue ) , e.Row.Properties.FieldName );

                }
            }
        }

        void vGridControl1_CustomDrawRowValueCell ( object sender , DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventArgs e )
        {

            if ( e.Row!=null&&( e.Row.Tag is ABCGridColumn.ColumnConfig||e.Row.Tag is ABCGridBandedColumn.ColumnConfig )&&
               ( this.ABCGrid==null||
                       ( this.ABCGrid.OwnerView!=null&&this.ABCGrid.OwnerView.Mode!=ViewMode.Design ) ) )
            {
                if ( e.Row.Index>=0&&String.IsNullOrWhiteSpace( this.ABCGrid.TableName )==false&&e.Row.Properties.FieldName.Contains( ":" ) )
                    e.CellText=DataFormatProvider.DoFormat( e.CellValue , this.ABCGrid.TableName , e.Row.Properties.FieldName );
            }

            DrawLinkButton( e );
        }
        
        #endregion

        #region UI
        void ParentForm_FormClosing ( object sender , FormClosingEventArgs e )
        {
            this.Dispose();
            this.Close();
        }
        void ABCGridRowDetail_FormClosing ( object sender , FormClosingEventArgs e )
        {
            e.Cancel=true;
            this.Hide();
        }
        void ABCGridRowDetail_Shown ( object sender , EventArgs e )
        {
            InvalidateData();

            this.VGrid.BestFit();
            this.VGrid.RecordWidth=this.VGrid.RowHeaderWidth+iMaxWidth;
            this.Size=new Size( this.VGrid.PreferredSize.Width+2 , this.VGrid.PreferredSize.Height+50 );
            this.VGrid.RecordWidth=this.Width-this.VGrid.RowHeaderWidth-20;
            if ( this.ABCGrid!=null )
            {
                Form form=( this.ABCGrid as Control ).FindForm();
                if ( form!=null&&form.WindowState==FormWindowState.Normal )
                {
                    this.Location=new Point( form.Left+form.Width , form.Top );
                }
            }

            this.TopMost=true;
            this.BringToFront();
        }

      
        int iMaxWidth=0;
        public void Initialize ( )
        {

            foreach ( DevExpress.XtraGrid.Columns.GridColumn gridCol in ABCGrid.GridDefaultView.Columns )
            {
                EditorRow row=new EditorRow();
                row.Properties.FieldName=gridCol.FieldName;
                row.Properties.Caption=gridCol.Caption;
                row.Enabled=gridCol.OptionsColumn.AllowEdit;
                row.Properties.RowEdit=gridCol.ColumnEdit;
                row.Visible=true;
                row.Properties.Format.Format=gridCol.DisplayFormat.Format;
                row.Properties.Format.FormatString=gridCol.DisplayFormat.FormatString;
                row.Properties.Format.FormatType=gridCol.DisplayFormat.FormatType;

                row.Properties.UnboundType=gridCol.UnboundType;
                if ( gridCol is ABCGridBandedColumn )
                    row.Tag=( gridCol as ABCGridBandedColumn ).Config;
                if ( gridCol is ABCGridColumn )
                    row.Tag=( gridCol as ABCGridColumn ).Config;

                this.VGrid.Rows.Add( row );

                if ( gridCol.VisibleIndex<0 )
                    row.Index=this.VGrid.Rows.Count-1;
                else
                    row.Index=gridCol.VisibleIndex;

                if ( gridCol.Width>iMaxWidth )
                    iMaxWidth=gridCol.Width;

            }

            BindingSource=new System.Windows.Forms.BindingSource();
            this.VGrid.DataSource=BindingSource;

            if ( this.ABCGrid.GridDataSource is BindingSource )
                ( this.ABCGrid.GridDataSource as BindingSource ).PositionChanged+=new EventHandler( ABCGridRowDetail_PositionChanged );
            else
                this.ABCGrid.GridDefaultView.FocusedRowChanged+=new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler( GridDefaultView_FocusedRowChanged );

            this.Text="Chi tiết "+ABCGrid.ViewCaption;
        }

      
        #endregion

        #region InvalidateData

        void ABCGridRowDetail_PositionChanged ( object sender , EventArgs e )
        {
            InvalidateData();
        }
        void GridDefaultView_FocusedRowChanged ( object sender , DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e )
        {
            InvalidateData();
        } 
        public void InvalidateData ( )
        {
            try
            {
                if ( this.Visible&&ABCGrid!=null&&ABCGrid.GridDataSource!=null )
                {
                    if ( this.ABCGrid.GridDataSource is BindingSource )
                        BindingSource.DataSource=( this.ABCGrid.GridDataSource as BindingSource ).Current;
                    else if ( this.ABCGrid.GridDefaultView.FocusedRowHandle>=0 )
                        BindingSource.DataSource=this.ABCGrid.GridDefaultView.GetRow( this.ABCGrid.GridDefaultView.FocusedRowHandle );
                //    BindingSource.ResetBindings( false );

                    this.VGrid.RefreshDataSource();
                }
            }
            catch ( Exception ex )
            {
            }
        }
        
        #endregion

        public void RunFocusedLink ( )
        {
            if ( String.IsNullOrWhiteSpace( this.ABCGrid.TableName )==false )
            {
                if ( this.ABCGrid==null||( this.ABCGrid.OwnerView!=null&&this.ABCGrid.OwnerView.Mode!=ViewMode.Design ) )
                {

                    String strTableName=this.ABCGrid.TableName;
                    String strFieldName=VGrid.FocusedRow.Properties.FieldName;

                    String strLinkTableName=String.Empty;
                    Guid iLinkID=Guid.Empty;


                    EditorRow gridRow=this.VGrid.FocusedRow as EditorRow;
                    object objCellValue=this.VGrid.GetCellValue( gridRow , VGrid.FocusedRecord );
                    if ( objCellValue==null||objCellValue.GetType()!=typeof( Guid ) )
                        return;
                    if ( gridRow.Properties.FieldName.Contains( ":" ) )
                    {
                        DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( strTableName , ABCHelper.DataConverter.ConvertToGuid( objCellValue ) , gridRow.Properties.FieldName );
                        if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                            ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                        {
                            strLinkTableName=acrrosInfo.TableName;
                            iLinkID=acrrosInfo.TableID;
                        }
                    }
                    else if ( DataStructureProvider.IsForeignKey( strTableName , strFieldName ) )
                    {
                        strLinkTableName=DataStructureProvider.GetTableNameOfForeignKey( strTableName , strFieldName );
                        iLinkID=ABCHelper.DataConverter.ConvertToGuid( objCellValue );

                    }

                    if ( iLinkID!=Guid.Empty )
                    {
                        if ( this.ABCGrid.OwnerView!=null )
                             ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , this.ABCGrid.OwnerView.Mode , false , iLinkID , ABCScreenAction.None );
                        else
                             ABCScreen.ABCScreenHelper.Instance.RunLink( strLinkTableName , ViewMode.Runtime , false , iLinkID , ABCScreenAction.None );

                    }
                }
            }
        }

        #region Cell Click

        void VGrid_MouseClick ( object sender , MouseEventArgs e )
        {
            VGridHitInfo hitInfo=VGrid.CalcHitInfo( VGrid.PointToClient( this.PointToScreen(e.Location )) );
            if ( hitInfo.Row!=null && e.Button==System.Windows.Forms.MouseButtons.Left
                &&e.Location.X-VGrid.ViewInfo[hitInfo.Row].ValuesRect.Left<18 &&e.Location.X-VGrid.ViewInfo[hitInfo.Row].ValuesRect.Left>0)
                RunFocusedLink();
        }
       
        Dictionary<String , bool> ForeignColumnList=new Dictionary<string , bool>();
        private void DrawLinkButton ( DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventArgs e )
        {
            if ( this.ABCGrid==null
                ||( this.ABCGrid.OwnerView!=null&&this.ABCGrid.OwnerView.Mode!=ViewMode.Design ) )
            {
                if ( e.Row!=null&&e.Row.Tag is ABCGridColumn.ColumnConfig&&String.IsNullOrWhiteSpace( this.ABCGrid.TableName )==false&&e.CellValue!=null&&e.CellValue.GetType()==typeof(int))
                {
                    Boolean isDraw=false;

                    EditorRow gridRow=e.Row as EditorRow;
                    String strTableName=this.ABCGrid.TableName;
                    if ( gridRow.Properties.FieldName.Contains( ":" )==false )
                    {
                  
                        if ( ForeignColumnList.TryGetValue( gridRow.Properties.FieldName , out isDraw )==false )
                        {
                            if ( DataStructureProvider.IsForeignKey( strTableName , gridRow.Properties.FieldName ) )
                                ForeignColumnList.Add( gridRow.Properties.FieldName , true );
                            else
                                ForeignColumnList.Add( gridRow.Properties.FieldName , false );
                        }

                    }
                    else
                    {
                        DataCachingProvider.AccrossStructInfo acrrosInfo=DataCachingProvider.GetAccrossStructInfo( strTableName , ABCHelper.DataConverter.ConvertToGuid( e.CellValue ) , e.Row.Properties.FieldName );
                        if ( acrrosInfo!=null&&( acrrosInfo.FieldName==DataStructureProvider.GetNOColumn( acrrosInfo.TableName )
                                                            ||acrrosInfo.FieldName==DataStructureProvider.GetNAMEColumn( acrrosInfo.TableName ) ) )
                            isDraw=true;
                    }

                    if ( isDraw )
                    {
                        e.Graphics.DrawImage( ABCControls.ABCImageList.GetImage16x16( "DocLink" ) , e.Bounds.Location );

                        Rectangle r=e.Bounds;
                        r.Width-=18;
                        r.X+=18;
                        e.Appearance.DrawString( e.Cache , e.CellText , r );

                        e.Handled=true;
                    }
                }
            }
        }

        #endregion


        class Win32
        {
            [DllImport( "user32.dll" , EntryPoint="SetWindowPos" )]
            public static extern bool SetWindowPos (
               int hWnd ,               // window handle
               int hWndInsertAfter ,    // placement-order handle
               int X ,                  // horizontal position
               int Y ,                  // vertical position
               int cx ,                 // width
               int cy ,                 // height
               uint uFlags );           // window positioning flags
            public const int HWND_BOTTOM=0x1;
            public const int HWND_TOP=0x0;
            public const uint SWP_NOSIZE=0x1;
            public const uint SWP_NOMOVE=0x2;
            public const uint SWP_SHOWWINDOW=0x40;
        }

        private void ShowToTop ( )
        {
            Win32.SetWindowPos( (int)this.Handle ,
               Win32.HWND_TOP ,
               0 , 0 , 0 , 0 ,
               Win32.SWP_NOMOVE|Win32.SWP_NOSIZE|
               Win32.SWP_SHOWWINDOW );
        }

    }
}