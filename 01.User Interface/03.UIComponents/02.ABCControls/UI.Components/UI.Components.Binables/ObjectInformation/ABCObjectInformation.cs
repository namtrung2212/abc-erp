using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;


using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{
    public partial class ABCObjectInformation : DevExpress.XtraEditors.XtraForm
    {
        public ABCObjectInformation ( )
        {
            InitializeComponent();
        }

        ABCObjectInfo currentObject;
        public ABCObjectInformation ( ABCObjectInfo obj )
        {
            currentObject=obj;

            InitializeComponent();

            if ( obj.ObjectID==Guid.Empty)
                return;

            txtObjectType.Text=DataConfigProvider.GetTableCaption( obj.TableName );
            txtObjectNo.Text=obj.ObjectNo;

            this.Text="Lịch sử " + txtObjectType.Text+" : "+txtObjectNo.Text;

            txtCreateUser.Text=obj.CreateUser;
            if ( obj.CreateTime.HasValue )
                txtCreateTime.Text=obj.CreateTime.Value.ToString( "dd/MM/yyyy HH:mm" );
            else
                txtCreateTime.Text="";

            txtUpdateUser.Text=obj.UpdateUser;
            if ( obj.UpdateTime.HasValue )
                txtUpdateTime.Text=obj.UpdateTime.Value.ToString( "dd/MM/yyyy HH:mm" );
            else
                txtUpdateTime.Text="";

            txtEditCount.Text=obj.EditCount.ToString();

            ABCComment comment=new ABCComment();
            comment.Dock=DockStyle.Fill;
            comment.LoadComments( obj.TableName , obj.ObjectID );
            xtraTabPage1.Controls.Add( comment );

            ABCLogging logging=new ABCLogging();
            logging.Dock=DockStyle.Fill;
            logging.LoadLogs( obj.TableName , obj.ObjectID );
            xtraTabPage2.Controls.Add( logging );
        }

        public static void ShowObjectInfo ( String strTableName , Guid iID )
        {
            BusinessObject obj=BusinessObjectHelper.GetBusinessObject( strTableName , iID );
            if ( obj!=null )
                ShowObjectInfo( obj );
        }
        public static void ShowObjectInfo ( BusinessObject obj )
        {
            ABCHelper.ABCWaitingDialog.Show( "" , "Đang mở . . .!" );
            ABCObjectInformation dlg=new ABCObjectInformation( new ABCObjectInfo( obj ) );
             ABCScreen.ABCScreenHelper.Instance.ShowForm( dlg , false );
            ABCHelper.ABCWaitingDialog.Close();
        }

        [DllImport( "User32" , CharSet=CharSet.Auto , ExactSpelling=true )]
        internal static extern IntPtr SetParent ( IntPtr hWndChild , IntPtr hWndParent );

        private void btnRunLink_Click ( object sender , EventArgs e )
        {
            if ( currentObject.ObjectID==Guid.Empty )
                return;

             ABCScreen.ABCScreenHelper.Instance.RunLink( currentObject.TableName , ViewMode.Runtime , false , currentObject.ObjectID , ABCScreenAction.None );
     
        }
    }


    public class ABCObjectInfo
    {
        public BusinessObject Object;
        public String TableName;
        public Guid ObjectID;
        public String ObjectNo;
        public String CreateUser;
        public DateTime? CreateTime;
        public String UpdateUser;
        public DateTime? UpdateTime;
        public int EditCount;

        public ABCObjectInfo ( BusinessObject obj )
        {
            TableName=obj.AATableName;
            ObjectID=BusinessObjectHelper.GetIDValue( obj );
            ObjectNo=BusinessObjectHelper.GetDisplayValue( obj );

            object  objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , "CreateUser" );
            if ( objValue!=null )
                CreateUser=objValue.ToString();

            objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , "UpdateUser" );
            if ( objValue!=null )
                UpdateUser=objValue.ToString();

            objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , "CreateTime" );
            if ( objValue!=null )
                CreateTime=(DateTime?)objValue;

            objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , "UpdateTime" );
            if ( objValue!=null )
                UpdateTime=(DateTime?)objValue;

            objValue=ABCBusinessEntities.ABCDynamicInvoker.GetValue( obj , "EditCount" );
            if ( objValue!=null )
                EditCount=Convert.ToInt32( objValue );

        }
    }
}