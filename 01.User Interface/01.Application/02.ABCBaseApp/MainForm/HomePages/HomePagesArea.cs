using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ABCScreen;

using ABCProvider;

namespace ABCApp
{
    public partial class HomePagesArea : DevExpress.XtraEditors.XtraUserControl
    {

        public Dictionary<Guid , ABCBaseScreen> ScreenList=new Dictionary<Guid , ABCBaseScreen>();

        MainForm mainForm;
        public HomePagesArea (MainForm form )
        {
            mainForm=form;
            InitializeComponent();          
        }

        public void LoadHomePages ( )
        {
            if ( ABCScreenManager.Instance==null )
                return;
            List<Guid> lstViewIDs=ABCUserProvider.GetHomepages( ABCUserProvider.CurrentUser.ADUserID );
            if ( lstViewIDs.Count>0 )
                xtraTabControl1.TabPages.Clear();

            foreach ( Guid strViewID in lstViewIDs )
                if ( ABCScreenManager.Instance.CheckViewPermission( strViewID , ABCCommon.ViewPermission.AllowView ) )
                    OpenHomePage( strViewID );
        }
        public void OpenHomePage ( Guid iViewID )
        {
            if ( ScreenList.ContainsKey( iViewID )==false )
            {
                ABCBaseScreen scr=ABCScreen.ABCScreenFactory.GetABCScreen( iViewID );
                if ( scr==null )
                    return;

                ScreenList.Add( iViewID , scr );
                scr.UIManager.View.Dock=DockStyle.Fill;

                DevExpress.XtraEditors.PanelControl pnl=new PanelControl();
                pnl.Controls.Add( scr.UIManager.View );
                pnl.Dock=DockStyle.Fill;

                DevExpress.XtraTab.XtraTabPage page=new DevExpress.XtraTab.XtraTabPage();
                page.Text=scr.UIManager.View.Caption;
                page.Controls.Add( pnl );
                xtraTabControl1.TabPages.Add( page );
            }
        }

    }
}
