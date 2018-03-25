using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class NotifyProvider
    {
        public static void SetNotifyToViewed ( Guid iNotifyID )
        {
            String strQuery=String.Format( @"UPDATE GENotifys SET Viewed =1 WHERE GENotifyID ='{0}' " , iNotifyID );
            BusinessObjectController.RunQuery( strQuery );
        }

        public static void CreateNewNotify ( String strToUser , String strNotifyTitle , String strNotifyContent , String strTableName , Guid iID , String strPriorityLevel )
        {
            if ( strToUser==ABCUserProvider.CurrentUserName )
                return;

            object obj=BusinessObjectController.GetData( String.Format( @"SELECT HREmployees.Name FROM ADUsers,HREmployees
                                        WHERE ADUsers.ABCStatus ='Alive' AND ADUsers.Active =1 AND FK_HREmployeeID =HREmployeeID AND ADUsers.No =N'{0}'" , strToUser ) );
            if ( obj!=null&&obj!=DBNull.Value )
                CreateNewNotify( strToUser , obj.ToString() , strNotifyTitle , strNotifyContent , strTableName , iID , strPriorityLevel );
        }

        public static void CreateNewNotify ( String strToUser , String strToEmployee , String strNotifyTitle , String strNotifyContent , String strTableName , Guid iID , String strPriorityLevel )
        {
            if ( strToUser==ABCUserProvider.CurrentUserName )
                return;

            strToUser=strToUser.Replace( "'" , "''" );
            strToEmployee=strToEmployee.Replace( "'" , "''" );
            strNotifyTitle=strNotifyTitle.Replace( "'" , "''" );
            strNotifyContent=strNotifyContent.Replace( "'" , "''" );
            strTableName=strTableName.Replace( "'" , "''" );
            strPriorityLevel=strPriorityLevel.Replace( "'" , "''" );
        
            object obj=BusinessObjectController.GetData( String.Format( @"SELECT GENotifyID FROM GENotifys WHERE ToUser=N'{0}' AND TableName ='{1}' AND ID ='{2}'" , strToUser , strTableName , iID ) );
            Guid iNofityID=ABCHelper.DataConverter.ConvertToGuid( obj );

            String strQuery=String.Empty;
            if ( iNofityID==Guid.Empty )
            {
                strQuery=String.Format( @"INSERT INTO GENotifys ( GENotifyID,LastTime , ToUser , ToEmployee , NotifyTitle , NotifyContent ,Viewed,TableName,ID,PriorityLevel ) 
                                                              VALUES ('{0}',GetDate() ,N'{1}' ,N'{2}' ,N'{3}',N'{4}',0,'{5}','{6}',N'{7}')" , Guid.NewGuid() , strToUser , strToEmployee , strNotifyTitle , strNotifyContent , strTableName , iID , strPriorityLevel );
            }
            else
            {
                strQuery=String.Format( @"UPDATE GENotifys SET LastTime = GetDate() , NotifyTitle =N'{0}' , NotifyContent =N'{1}', Viewed =0, PriorityLevel =N'{2}' WHERE GENotifyID ='{3}' " , strNotifyTitle , strNotifyContent , strPriorityLevel , iNofityID );
            }
            BusinessObjectController.RunQuery( strQuery );
        }

        public static void CreateNewNotifyFromComment ( String strTableName , Guid iID )
        {
            if ( DataStructureProvider.IsExistedTable( strTableName )==false )
                return;

            String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );

            #region Get Users
            List<String> lstUsers=new List<string>();
            DataSet ds=BusinessObjectController.RunQuery( String.Format( @"SELECT CreateUser FROM GEComments WHERE TableName ='{0}' AND ID = '{1}' GROUP BY CreateUser" , strTableName , iID ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    if ( lstUsers.Contains( dr[0].ToString() )==false )
                        lstUsers.Add( dr[0].ToString() );
                }
            }
            ds=BusinessObjectController.RunQuery( String.Format( @"SELECT TagString FROM GEComments WHERE TableName ='{0}' AND ID = '{1}'  AND TagString IS NOT NULL AND TagString NOT LIKE '' GROUP BY TagString" , strTableName , iID ) );
            if ( ds!=null&&ds.Tables.Count>0 )
            {
                foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    if ( dr[0]!=null&&dr[0]!=DBNull.Value&&String.IsNullOrWhiteSpace( dr[0].ToString() )==false )
                    {
                        string[] arr= { "::" };
                        arr=dr[0].ToString().Split( arr , StringSplitOptions.None );
                        for ( int i=0; i<arr.Length; i++ )
                            if ( lstUsers.Contains( arr[i] )==false )
                                lstUsers.Add( arr[i] );
                    }
                }
            }



            if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colCreateUser ) )
            {
                ds=BusinessObjectController.RunQuery( String.Format( @"SELECT {0} FROM {1} WHERE {2} ='{3}'" , ABCCommon.ABCConstString.colCreateUser , strTableName , strIDCol , iID ) );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    object objCreateUser=ds.Tables[0].Rows[0][0];
                    if ( objCreateUser!=null&&objCreateUser!=DBNull.Value&&lstUsers.Contains( objCreateUser.ToString() )==false )
                        lstUsers.Add( objCreateUser.ToString() );
                }
            }
            if ( DataStructureProvider.IsTableColumn( strTableName , ABCCommon.ABCConstString.colUpdateUser ) )
            {
                ds=BusinessObjectController.RunQuery( String.Format( @"SELECT {0} FROM {1} WHERE {2} ='{3}'"  , ABCCommon.ABCConstString.colUpdateUser , strTableName , strIDCol , iID ) );
                if ( ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0 )
                {
                    object objUpdateUser=ds.Tables[0].Rows[0][0];
                    if ( objUpdateUser!=null&&objUpdateUser!=DBNull.Value&&lstUsers.Contains( objUpdateUser.ToString() )==false )
                        lstUsers.Add( objUpdateUser.ToString() );
                }
            }

            #endregion

            String strTitle=DataConfigProvider.GetTableCaption( strTableName );
            String strDisplayCol=DataStructureProvider.GetDisplayColumn( strTableName );

            object obj=BusinessObjectController.GetData( String.Format( @"SELECT {0} FROM {1} WHERE {2} ='{3}' " , strDisplayCol , strTableName , strIDCol , iID ) );
            if ( obj!=null&&obj!=DBNull.Value )
                strTitle=strTitle+" : "+obj.ToString();

            foreach ( String strUser in lstUsers )
            {
                if ( strUser!=ABCUserProvider.CurrentUserName )
                    CreateNewNotify( strUser , strTitle , "" , strTableName , iID , "" );
            }

        }

        public static void CreateNewNotifyFromComment ( String strUser , String strTableName , Guid iID )
        {
            if ( strUser!=ABCUserProvider.CurrentUserName )
            {
                String strTitle=DataConfigProvider.GetTableCaption( strTableName );
                String strDisplayCol=DataStructureProvider.GetDisplayColumn( strTableName );
                String strIDCol=DataStructureProvider.GetPrimaryKeyColumn( strTableName );

                object obj=BusinessObjectController.GetData( String.Format( @"SELECT {0} FROM {1} WHERE {2} ='{3}' " , strDisplayCol , strTableName , strIDCol , iID ) );
                if ( obj!=null&&obj!=DBNull.Value )
                    strTitle=strTitle+" : "+obj.ToString();
                CreateNewNotify( strUser , strTitle , "" , strTableName , iID , "" );
            }
        }
    }
}
