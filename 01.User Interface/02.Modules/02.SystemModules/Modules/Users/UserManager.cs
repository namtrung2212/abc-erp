using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Text;
using ABCCommon;
using ABCProvider;
using ABCProvider;

using ABCBusinessEntities;

namespace ABCScreen
{
    public class ABCUserManager
    {
        public static ADUsersInfo CurrentUser
        {
            get
            {
                return ABCUserProvider.CurrentUser;
            }
            set
            {
                ABCUserProvider.CurrentUser=value;
            }
        }
        public static ADUserGroupsInfo CurrentUserGroup
        {
            get
            {
                return ABCUserProvider.CurrentUserGroup;
            }
            set
            {
                ABCUserProvider.CurrentUserGroup=value;
            }
        }
        public static String CurrentUserName
        {
            get { return ABCUserProvider.CurrentUserName; }
            set
            {
                ABCUserProvider.CurrentUserName=value;
            }
        }

        static UserLogin LoginForm;
        public static void ShowLogIn ( LoginType type)
        {
            LoginForm=new UserLogin( type );
            LoginForm.ShowDialog();
        }

        static List<String> SynchronizedDBs=new List<string>();
        static String CurrentDatabase=String.Empty;

        public static bool ConnectDatabase ( String strDatabase )
        {
            if ( DataQueryProvider.Connect( strDatabase )==false )
            {
                ABCHelper.ABCMessageBox.Show( "Kiểm tra lại thông tin kết nối" , "Kết nối dữ liệu" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return false;
            }
            if ( strDatabase!=CurrentDatabase )
            {
                DataQueryProvider.InitDataTables();
                if ( !SynchronizedDBs.Contains( strDatabase ) )
                {
                    if ( DataSyncProvider.IsNeedSync() )
                        DataSyncProvider.Synchronize();
                    SynchronizedDBs.Add( strDatabase );
                }
                CurrentDatabase=strDatabase;
            }

            return true;
        }

        public static void Login ( LoginType loginType , String strDatabase , String strUserNo , String strPassword )
        {
            if ( !ConnectDatabase( strDatabase ) )
                return;

            String strEncryptedPass=new Security.Cryptography().Encrypt( strPassword );
            ADUsersInfo user=new ADUsersController().GetObject( String.Format( @"SELECT * FROM ADUsers WHERE No='{0}' AND ABCStatus ='Alive' AND Active =1 " , strUserNo ) ) as ADUsersInfo;
            if ( user==null||strEncryptedPass!=user.Password )
            {
                ABCHelper.ABCMessageBox.Show( LoginForm,"Thông tin người dùng, mật khẩu không đúng" , "Đăng nhập" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return;
            }
            if ( LoginForm!=null )
            {
                LoginForm.Close();
                LoginForm.Dispose();
            }
            Application.DoEvents();

            ABCScreen.SplashUtils.ShowSplash( loginType );
       
            ABCControls.UICaching.InitCachingPresentControls();

            #region Section

            CurrentUser=user;
            if(CurrentUser.FK_ADUserGroupID.HasValue)
                CurrentUserGroup=new ADUserGroupsController().GetObjectByID( CurrentUser.FK_ADUserGroupID.Value ) as ADUserGroupsInfo;
            ABCUserProvider.CurrentUserName=CurrentUser.No;
         
            if ( CurrentUser.FK_HREmployeeID.HasValue )
            {
                try
                {
                    ABCUserProvider.CurrentEmployee=new HREmployeesController().GetObjectByID( CurrentUser.FK_HREmployeeID.Value ) as HREmployeesInfo;
                    if ( ABCUserProvider.CurrentEmployee!=null )
                    {
                        ABCUserProvider.CurrentEmployeeName=ABCUserProvider.CurrentEmployee.Name;
                        if ( ABCUserProvider.CurrentEmployee.FK_GECompanyUnitID.HasValue )
                            ABCUserProvider.CurrentCompanyUnit=new GECompanyUnitsController().GetObjectByID( ABCUserProvider.CurrentEmployee.FK_GECompanyUnitID.Value ) as GECompanyUnitsInfo;
                    }
                }catch(Exception ex)
                {
                }
            }

            StartOnlineTimer();

            if ( loginType==LoginType.ERP )
                ABCApp.ABCAppHelper.Instance.StartSection();
            else if ( loginType==LoginType.Studio )
                ABCStudio.ABCStudioHelper.Instance.StartSection();

            ABCScreen.SplashUtils.CloseSplash();

            #endregion

        }

        public static bool ChangePassword ( String strDatabase , String strUserNo , String strOldPassword , String strNewPassword )
        {
            if ( !ConnectDatabase( strDatabase ) )
                return false;

            Security.Cryptography crypto=new Security.Cryptography();
            String strOldEncryptedPass=crypto.Encrypt( strOldPassword );

            ADUsersInfo user=new ADUsersController().GetObjectByNo( strUserNo ) as ADUsersInfo;
            if ( user==null||strOldEncryptedPass!=user.Password )
            {
                ABCHelper.ABCMessageBox.Show( LoginForm,"Thông tin người dùng, mật khẩu không đúng" , "Đổi mật khẩu" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return false;
            }

            user.Password=crypto.Encrypt( strNewPassword );
            BusinessObjectController.RunQuery( String.Format( @"UPDATE ADUsers SET Password='{0}' ,UpdateTime=GETDATE() WHERE ADUserID = '{1}'" , user.Password , user.ADUserID ) );
            ABCHelper.ABCMessageBox.Show(LoginForm, "Đổi mật khẩu thành công" , "Đổi mật khẩu" , MessageBoxButtons.OK , MessageBoxIcon.Information );
            return true;
        }

        static System.Timers.Timer OnlineTimer;
        public static void StartOnlineTimer ( )
        {
            object obj=BusinessObjectController.GetData( String.Format( @"SELECT COUNT(*)  FROM ADUserStatuss WHERE FK_ADUserID ='{0}' " , CurrentUser.ADUserID ) );
            if ( obj!=null&&obj!=DBNull.Value )
            {
                int iCount=Convert.ToInt32( obj );
                if ( iCount<=0 )
                {
                    String strQuery=String.Format( @"INSERT INTO ADUserStatuss ( ADUserStatusID,CreateTime,UpdateTime,FK_ADUserID , UserName , EmployeeName , LastOnlineTime , IsOnline ,OnlineStatus) 
                                                              VALUES (NEWID(),GETDATE(),GETDATE(),'{0}',N'{1}' ,N'{2}',GetDate(),1,'')" , CurrentUser.ADUserID , CurrentUser.No.Replace( "'" , "''" ) , ABCUserProvider.CurrentEmployeeName.Replace( "'" , "''" ) );
                    if ( DataQueryProvider.IsCompanySQLConnection==false )
                        strQuery=String.Format( @"INSERT INTO ADUserStatuss ( ADUserStatusID,CreateTime,UpdateTime,FK_ADUserID , UserName , EmployeeName , LastOnlineTime , IsOnline ,OnlineStatus) 
                                                              VALUES (NEWID(),GETDATE(),GETDATE(),'{0}',N'{1}' ,N'{2}',DATETIME('now', 'localtime'),1,'')" , CurrentUser.ADUserID , CurrentUser.No.Replace( "'" , "''" ) , ABCUserProvider.CurrentEmployeeName.Replace( "'" , "''" ) );
                    BusinessObjectController.RunQuery( strQuery );
                }
            }

            OnlineUpdate( true );

            if ( OnlineTimer==null )
            {
                OnlineTimer=new System.Timers.Timer();
                OnlineTimer.Interval=60000;
                OnlineTimer.Elapsed+=new System.Timers.ElapsedEventHandler( OnlineTimer_Elapsed );
                OnlineTimer.Start();
            }

        }

        public static void OnlineTimer_Elapsed ( object sender , System.Timers.ElapsedEventArgs e )
        {
            OnlineUpdate(true);
        }
        public static void OnlineUpdate ( bool isOnline )
        {
            if ( DataQueryProvider.IsCompanySQLConnection )
            {
                BusinessObjectController.RunQuery( @"UPDATE ADUserStatuss SET IsOnline =0,UpdateTime=GETDATE() WHERE DATEDIFF(minute,LastOnlineTime, GetDate() ) > 1" );
                if ( isOnline )
                    BusinessObjectController.RunQuery( String.Format( @"UPDATE ADUserStatuss SET IsOnline =1,UpdateTime=GETDATE() , LastOnlineTime =GetDate()  WHERE FK_ADUserID ='{0}' " , CurrentUser.ADUserID ) );
                else
                    BusinessObjectController.RunQuery( String.Format( @"UPDATE ADUserStatuss SET IsOnline =0,UpdateTime=GETDATE() , LastOnlineTime =GetDate()  WHERE FK_ADUserID ='{0}' " , CurrentUser.ADUserID ) );
            }
            else
            {
                BusinessObjectController.RunQuery( @"UPDATE ADUserStatuss SET IsOnline =0 ,UpdateTime=GETDATE() WHERE  (strftime('%M','now') - strftime('%M',LastOnlineTime)) > 1" );
                if ( isOnline )
                    BusinessObjectController.RunQuery( String.Format( @"UPDATE ADUserStatuss SET IsOnline =1,UpdateTime=GETDATE() , LastOnlineTime =DATETIME('now', 'localtime') WHERE FK_ADUserID ='{0}' " , CurrentUser.ADUserID ) );
                else
                    BusinessObjectController.RunQuery( String.Format( @"UPDATE ADUserStatuss SET IsOnline =0,UpdateTime=GETDATE() , LastOnlineTime =DATETIME('now', 'localtime')  WHERE FK_ADUserID ='{0}' " , CurrentUser.ADUserID ) );
            }
        }
       
    }
}
