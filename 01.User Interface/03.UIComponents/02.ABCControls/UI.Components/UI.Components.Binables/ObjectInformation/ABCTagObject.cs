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
    public partial class ABCTagObject : DevExpress.XtraEditors.XtraUserControl
    {
        public ABCTagObject ( )
        {
            InitializeComponent();
            this.btnTag.Click+=new EventHandler( btnTag_Click );
        }


        public Dictionary<String , ABCUserInfo> Users=new Dictionary<String , ABCUserInfo>();
        void btnTag_Click ( object sender , EventArgs e )
        {
            List<ABCUserInfo> lstTemps=UserChooserForm.ShowChoose( new List<string>( Users.Keys ) );

            if ( lstTemps!=null )
            {
                Users.Clear();
                foreach ( ABCUserInfo user in lstTemps )
                {
                    if ( Users.ContainsKey( user.User )==false )
                        Users.Add( user.User , user );
                }
                InvalidateTags();
            }
        }

        Dictionary<String , LinkLabel> lstTags=new Dictionary<String , LinkLabel>();
        public void InvalidateTags ( )
        {
            List<string> lstTemps = new List<string>(lstTags.Keys);

            foreach ( String strTag in lstTemps )
            {
                if ( Users.ContainsKey( strTag )==false )
                {
                    lstTags[strTag].Parent.Controls.Remove( lstTags[strTag] );
                    lstTags[strTag].Parent=null;
                    lstTags.Remove( strTag );
                }
            }

            foreach ( String strUser in Users.Keys )
            {
                if ( lstTags.ContainsKey( strUser )==false )
                {
                    LinkLabel link=new LinkLabel();
                    link.AutoSize=true;
                    link.Padding=new System.Windows.Forms.Padding( 1 );
                    link.Text=Users[strUser].Employee;

                    this.flowLayoutPanel1.Controls.Add( link );
                    lstTags.Add( strUser , link );
                }
            }
        }
        public void ClearTags ( )
        {
            Users.Clear();
            foreach ( String strTag in lstTags.Keys )
            {
                lstTags[strTag].Parent.Controls.Remove( lstTags[strTag] );
                lstTags[strTag].Parent=null;
            }
            lstTags.Clear();
        }
        private void simpleButton1_Click ( object sender , EventArgs e )
        {

        }
    }
}
