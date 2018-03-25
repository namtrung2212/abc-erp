using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Menu;
using DevExpress.Utils.Menu;
using ABCDataLib;
using ABCPresentLib;

namespace ABCStudio
{
    public partial class StoredProConfigScreen : DevExpress.XtraEditors.XtraUserControl
    {
        public StoredProConfigScreen ( )
        {
            InitializeComponent();
            this.barManager1.Images=ABCPresentLib.ABCImageList.List16x16;
            this.barManager1.ItemClick+=new DevExpress.XtraBars.ItemClickEventHandler( Toolbar_ItemClick );
            ScriptTabControl.HeaderButtons=DevExpress.XtraTab.TabButtons.Close|DevExpress.XtraTab.TabButtons.Next|DevExpress.XtraTab.TabButtons.Prev;
            ScriptTabControl.HeaderButtonsShowMode=DevExpress.XtraTab.TabButtonShowMode.Always;
            ScriptTabControl.CloseButtonClick+=new EventHandler( Script_CloseButtonClick );

            GetStoredProcedureList( chkShowDefaultSP.Checked );
            this.GridView.DoubleClick+=new EventHandler( SPList_DoubleClick );
            this.GridView.MouseUp+=new MouseEventHandler(SPList_MouseUp);
            this.GridView.KeyDown+=new KeyEventHandler( SPList_KeyDown );
            PopupMenu=new ABCGridViewMenu( GridView );

            ResultRichText.Text=String.Empty;
        }


        void Toolbar_ItemClick ( object sender , DevExpress.XtraBars.ItemClickEventArgs e )
        {
            if ( e.Item.Tag==null )
                return;

            if ( e.Item.Tag.ToString()=="New" )
            {
                AddNewTab();
            }
            else if ( e.Item.Tag.ToString()=="Run" )
            {
                ExecuteScript();
            }
            else if ( e.Item.Tag.ToString()=="GenSP" )
            {
                GenerateSPs();
            }
            else if ( e.Item.Tag.ToString()=="Refresh" )
            {
                GetStoredProcedureList( chkShowDefaultSP.Checked );
            }
           
        }
       
        void SPList_DoubleClick ( object sender , EventArgs e )
        {
            OpenSP();
        }
        void SPList_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode==Keys.Delete )
                DeleteSP();
            if ( e.KeyCode==Keys.Enter )
                OpenSP();
        }
        void Script_PreviewKeyDown ( object sender , PreviewKeyDownEventArgs e )
        {
            if ( e.KeyCode==Keys.F5 )
                ExecuteScript();
        }
        void Script_CloseButtonClick ( object sender , EventArgs e )
        {
            if ( e is DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs )
            {
                DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs ex=(DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
                ScriptTabControl.TabPages.Remove( (DevExpress.XtraTab.XtraTabPage)ex.Page );
            }
        }
      
        public void ShowStoredProcedure ( String strSPname )
        {

            ScintillaNet.Scintilla richText=AddNewTab();

            DataSet ds=ABCDataLib.ConnectionManager.DatabaseHelper.RunQuery( String.Format( "sp_helptext [{0}];" , strSPname ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                    richText.Text+=dr[0].ToString();
            }

            richText.Text=richText.Text.Replace( "CREATE PROCEDURE" , "ALTER PROCEDURE" );
        }
        private ScintillaNet.Scintilla AddNewTab ( )
        {
            DevExpress.XtraTab.XtraTabPage page=this.ScriptTabControl.TabPages.Add( "Script"+this.ScriptTabControl.TabPages.Count );
            ScriptTabControl.SuspendLayout();

            ScintillaNet.Scintilla richText=new ScintillaNet.Scintilla();
            richText.ConfigurationManager.Language="sql";
            richText.Dock=System.Windows.Forms.DockStyle.Fill;
            richText.Indentation.ShowGuides=true;
            richText.Indentation.SmartIndentType=ScintillaNet.SmartIndent.Simple;
            richText.Location=new System.Drawing.Point( 0 , 0 );
            richText.Margins.Margin0.Width=40;
            richText.Margins.Margin2.Width=20;
            richText.Name="Script";
            richText.Size=new System.Drawing.Size( 644 , 452 );
            richText.Styles.BraceBad.FontName="Verdana";
            richText.Styles.BraceLight.FontName="Verdana";
            richText.Styles.ControlChar.FontName="Verdana";
            richText.Styles.Default.FontName="Verdana";
            richText.Styles.Default.Size=8;
            richText.Styles.IndentGuide.FontName="Verdana";
            richText.Styles.LastPredefined.FontName="Verdana";
            richText.Styles.LineNumber.FontName="Verdana";
            richText.Styles.Max.FontName="Verdana";
            richText.TabIndex=2;
            richText.Whitespace.Mode=ScintillaNet.WhitespaceMode.VisibleAfterIndent;
            richText.Dock=DockStyle.Fill;
            //     richText.Font=new Font( richText.Font.FontFamily , 9 );
            richText.PreviewKeyDown+=new PreviewKeyDownEventHandler( Script_PreviewKeyDown );
            page.Controls.Add( richText );
            ScriptTabControl.SelectedTabPage=page;
            ScriptTabControl.ResumeLayout();
            return richText;
        }
        public void ExecuteScript ( )
        {
            if ( ScriptTabControl.SelectedTabPage==null )
                return;

            ScintillaNet.Scintilla rtbScript=(ScintillaNet.Scintilla)ScriptTabControl.SelectedTabPage.Controls[0];
            String stQuery=rtbScript.Selection.Text.Trim();
            if ( String.IsNullOrEmpty( stQuery ) )
                stQuery=rtbScript.Text;

            if ( String.IsNullOrEmpty( stQuery ) )
                return;

            Cursor.Current=Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm waiting=new DevExpress.Utils.WaitDialogForm();
            waiting.SetCaption( "Executing . . .!" );
            waiting.Show();
           
            DataSet ds=ABCDataLib.ConnectionManager.DatabaseHelper.RunQuery( stQuery );
            if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count==1 )
                ResultRichText.Text=ds.Tables[0].Rows[0][0].ToString();
        
            waiting.Close();
            Cursor.Current=Cursors.Default;

        }
        public void GetStoredProcedureList ( bool isIncludeDefaultSP)
        {
            List<String> lstSPs=new List<string>();
            foreach ( String strTableName in ABCDataLib.Tables.StructureProvider.DataTablesList.Keys )
            {
                lstSPs.AddRange( ABCDataLib.Generation.StoredProcedureGenerator.GetSPNameList( strTableName ) );
            }

            
            DataSet ds=ABCDataLib.ConnectionManager.DatabaseHelper.RunQuery( "SELECT name FROM sys.procedures;" );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                DataTable dt=ds.Tables[0].Clone();
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    String strName=dr["name"].ToString();
                    if (isIncludeDefaultSP || lstSPs.Contains( strName )==false )
                        dt.ImportRow( dr );
                }
                GridCtrl.DataSource=dt;

            }

          

        }
        private void GenerateSPs ( )
        {

            using ( ABCPresentLib.TableChooserForm form=new ABCPresentLib.TableChooserForm() )
            {
                List<String> lstResult=form.ShowChoose();
                if ( form.DialogResult==DialogResult.OK )
                {
                    ResultRichText.Text=String.Empty;
                    foreach ( String strTableName in lstResult )
                    {
                        ResultRichText.Text+=( Environment.NewLine+String.Format( "Generate Default Stored Procedures for Table    :    {0} " , strTableName )+Environment.NewLine+Environment.NewLine );
                        foreach ( String strItem in ABCDataLib.Generation.StoredProcedureGenerator.GetSPNameList( strTableName ) )
                        {
                            ResultRichText.Text+=( String.Format( "             + Stored Procedure     :    {0} " , strItem )+Environment.NewLine );
                            ResultRichText.Select( ResultRichText.Text.Length-1 , 0 );
                            ResultRichText.ScrollToCaret();
                        }
                      
                        ABCDataLib.Generation.StoredProcedureGenerator.GenSP( strTableName );
                    }
                }
            }
        }
        public void OpenSP ( )
        {
            String strSPName=GridView.GetRowCellDisplayText( GridView.FocusedRowHandle , "name" );
            if ( String.IsNullOrEmpty( strSPName )==false )
                ShowStoredProcedure( strSPName );
        }
        public void DeleteSP ( )
        {
            String strSPName=GridView.GetRowCellDisplayText( GridView.FocusedRowHandle , "name" );
            if ( String.IsNullOrEmpty( strSPName )==false )
            {
                DialogResult result=DevExpress.XtraEditors.XtraMessageBox.Show( String.Format( "Do you want to delete SP : [{0}]  ? " , strSPName ) , "Message" , MessageBoxButtons.YesNo , MessageBoxIcon.Question );
                if ( result==System.Windows.Forms.DialogResult.Yes )
                {
                    ABCDataLib.ConnectionManager.DatabaseHelper.RunQuery( String.Format( "DROP PROCEDURE  [{0}] " , strSPName ) );
                    GetStoredProcedureList(chkShowDefaultSP.Checked);
                }
            }
        }
    
        #region PopUp Menu

        ABCGridViewMenu PopupMenu;
        private void DoShowPopupMenu ( DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=(DevExpress.XtraGrid.Views.Grid.GridView)GridCtrl.DefaultView;

            if ( hi.Column!=null&&hi.Column.FieldName.Equals( "name" )&&( hi.HitTest==DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell||hi.HitTest==DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.Row ) )
            {
                if ( PopupMenu.Items.Count<=0 )
                {
                    PopupMenu.AddItem( "Open" , "Open", ABCImageList.GetImage16x16("Open") );
                    PopupMenu.Items[PopupMenu.Items.Count-1].Click+=new EventHandler( PopupMenu_Click );

                    PopupMenu.AddItem( "Delete" , "Delete" , ABCImageList.GetImage16x16( "Delete" ) );
                    PopupMenu.Items[PopupMenu.Items.Count-1].Click+=new EventHandler( PopupMenu_Click );
                }
                PopupMenu.Show( hi.HitPoint );
            }


        }

        private void SPList_MouseUp ( object sender , MouseEventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.GridView view=(DevExpress.XtraGrid.Views.Grid.GridView)GridCtrl.DefaultView;
            if ( e.Button==MouseButtons.Right )
                DoShowPopupMenu( view.CalcHitInfo( new Point( e.X , e.Y ) ) );
        }

        public virtual void PopupMenu_Click ( object sender , EventArgs e )
        {
            DevExpress.XtraGrid.Views.Grid.GridView gridView=(DevExpress.XtraGrid.Views.Grid.GridView)GridCtrl.DefaultView;

            DXMenuItem item=sender as DXMenuItem;
            if ( item.Tag.ToString().Equals( "Open" ) )
                OpenSP();
            if ( item.Tag.ToString().Equals( "Delete" ) )
                DeleteSP();
        }
        #endregion

        private void chkShowDefaultSP_CheckedChanged ( object sender , EventArgs e )
        {
            GetStoredProcedureList( chkShowDefaultSP.Checked );
        }

    }
}