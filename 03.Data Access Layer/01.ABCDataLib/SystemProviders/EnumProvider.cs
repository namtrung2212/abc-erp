using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;

using System.Xml;

namespace ABCProvider
{
   public class EnumProvider
    {
       public class ABCEnumItem
       {
           public String ItemName { get; set; }
           public String CaptionVN { get; set; }
           public String CaptionEN { get; set; }
       }
       public class ABCEnum
       {
           public String EnumName=String.Empty;
           public Dictionary<String,ABCEnumItem> Items = new Dictionary<string,ABCEnumItem>();
       }

       static Dictionary<String , ABCEnum> enumList;
       public static Dictionary<String , ABCEnum> EnumList
       {
           get
           {
               if ( enumList==null||enumList.Count<=0 )
                   return GetAllEnums();

               return enumList;
           }
           set
           {
               enumList=value;
           }
       }
       [ABCRefreshTable( "STEnumDefines" )]
       public static Dictionary<String , ABCEnum> GetAllEnums ( )
       {
           if ( enumList!=null )
               enumList.Clear();
           else
               enumList=new Dictionary<String , ABCEnum>();

           DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( @"SELECT *  FROM [STEnumDefines] ORDER BY EnumName " );
           if ( ds==null||ds.Tables.Count<=0 )
               return enumList;

           foreach ( DataRow dr in ds.Tables[0].Rows )
           {
               if ( dr["EnumName"]!=DBNull.Value&&dr["ItemName"]!=DBNull.Value )
               {
                   String strEnum=dr["EnumName"].ToString();
                   String strItem=dr["ItemName"].ToString();
                   String strItemVN=dr["ItemDisplayVN"].ToString();
                   String strItemEN=dr["ItemDisplayEN"].ToString();

                   if ( String.IsNullOrWhiteSpace( strEnum )||String.IsNullOrWhiteSpace( strItem ) )
                       continue;

                   if ( enumList.ContainsKey( strEnum )==false )
                       enumList.Add( strEnum , new ABCEnum() );

                   if ( enumList[strEnum].Items.ContainsKey( strItem )==false )
                   {
                       ABCEnumItem item=new ABCEnumItem();
                       item.ItemName=strItem;
                       item.CaptionVN=strItemVN;
                       item.CaptionEN=strItemEN;
                       enumList[strEnum].Items.Add( strItem , item );
                   }
               }
           }

           return enumList;
       }

    }

   public class EnumGenerator : BaseGenerator
   {
       public static Dictionary<String , List<String>> EnumList;

       public static bool DetectModifyEnumDefineTable ( )
       {

           String strCheckCondition=String.Empty;
           if ( DataQueryProvider.IsSystemSQLConnection )
               strCheckCondition="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='STEnumDefines'";
           else
               strCheckCondition="SELECT tbl_name FROM sqlite_master WHERE type='table' AND tbl_name='STEnumDefines'";

           DataSet dsTemp=DataQueryProvider.RunQuery( strCheckCondition );
           if ( dsTemp==null||dsTemp.Tables.Count<=0||dsTemp.Tables[0].Rows.Count<=0 )
               return false;

           GetEnumListFromDB();

           bool isModified=IsEnumDefineTableModified();
           if ( isModified )
           {
               GenerateEnumToDLL();
               WriteToXML( @"EnumDefine.xml" );
           }

           return isModified;
       }
       public static bool IsEnumDefineTableModified ( )
       {
           String strFileName=@"EnumDefine.xml";
           if ( System.IO.File.Exists( strFileName )==false )
               return true;

           XmlDocument doc=new XmlDocument();
           doc.Load( strFileName );

           XmlNodeList nodeEnumList=doc.GetElementsByTagName( "Enum" );
           if ( nodeEnumList.Count!=EnumList.Count )
               return true;

           foreach ( XmlNode nodeEnum in nodeEnumList )
           {
               String strEnumName=nodeEnum.Attributes["name"].Value.ToString();
               if ( EnumList.ContainsKey( strEnumName )==false||nodeEnum.ChildNodes.Count!=EnumList[strEnumName].Count )
                   return true;

               #region Check Items
               foreach ( XmlNode nodeItem in nodeEnum.ChildNodes )
               {
                   String strItemName=nodeItem.InnerText;
                   if ( EnumList[strEnumName].Contains( strItemName )==false )
                       return true;
               }
               #endregion

           }

           return false;
       }

       public static void GetEnumListFromDB ( )
       {
           if ( EnumList!=null )
               EnumList.Clear();
           else
               EnumList=new Dictionary<string , List<string>>();

           DataSet ds=DataQueryProvider.SystemDatabaseHelper.RunQuery( @"SELECT EnumName ,ItemName  FROM [STEnumDefines] ORDER BY EnumName " );
           if ( ds==null||ds.Tables.Count<=0 )
               return;

           foreach ( DataRow dr in ds.Tables[0].Rows )
           {
               if ( dr["EnumName"]!=DBNull.Value&&dr["ItemName"]!=DBNull.Value )
               {
                   String strEnumName=dr["EnumName"].ToString();
                   String strItemName=dr["ItemName"].ToString();

                   if ( String.IsNullOrWhiteSpace( strEnumName )||String.IsNullOrWhiteSpace( strItemName ) )
                       continue;

                   if ( EnumList.ContainsKey( strEnumName )==false )
                       EnumList.Add( strEnumName , new List<String>() );

                   if ( EnumList[strEnumName].Contains( strItemName )==false )
                       EnumList[strEnumName].Add( strItemName );
               }
           }

       }

       public static void GenerateEnumToDLL ( )
       {

           if ( EnumList==null )
               return;

           #region Generate String
           StringBuilder strBuilder=new StringBuilder();

           strBuilder.Append( "using System;"+NewLine );
           strBuilder.Append( "using System.Text;"+NewLine );
           strBuilder.Append( "using System.Collections.Generic;"+NewLine );
           strBuilder.Append( ""+NewLine );

           strBuilder.Append( "namespace ABCProvider" );
           strBuilder.Append( NewLine+"{"+NewLine );

           strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
           strBuilder.Append( Tab+"//Generated By: ABC Studio"+NewLine );
           strBuilder.Append( Tab+String.Format( "//Created Date:{0}" , DateTime.Today.ToLongDateString() )+NewLine );
           strBuilder.Append( Tab+"//-----------------------------------------------------------"+NewLine );
           strBuilder.Append( Tab+NewLine );

           foreach ( String strEnumName in EnumList.Keys )
           {
               #region ENUM
               strBuilder.Append( Tab+String.Format( "public enum Enum{0} " , strEnumName )+NewLine );
               strBuilder.Append( Tab+"{"+NewLine );

               int i=0;
               foreach ( String strItemName in EnumList[strEnumName] )
               {
                   i++;
                   strBuilder.Append( Tab+Tab+strItemName.Trim()+String.Format( @" = {0} " , i) ) ;
                   if ( i<EnumList[strEnumName].Count )
                       strBuilder.Append( "," );
                   strBuilder.Append( NewLine );
               }

               strBuilder.Append( Tab+"}"+NewLine );
               #endregion
           }

           strBuilder.Append( NewLine+"}"+NewLine );

           #endregion

           CompileProvider.CompiledAssembly ass=(CompileProvider.CompiledAssembly )CompileProvider.Compiler.CompileAssembly( strBuilder.ToString() , CompileProvider.CodeType.CSharp , @"EnumDefine.dll" , new string[] { "System.dll" , "dll" } );

       }

       public static void WriteToXML ( String strFileName )
       {
           XmlDocument doc=new XmlDocument();
           XmlDeclaration dec=doc.CreateXmlDeclaration( "1.0" , null , null );
           doc.AppendChild( dec );

           XmlElement root=doc.CreateElement( "Enums" );
           doc.AppendChild( root );

           foreach ( String strEnumName in EnumList.Keys )
           {
               XmlElement elEnums=GetEnumXmlElement( doc , strEnumName );

               root.AppendChild( elEnums );
           }
           doc.Save( strFileName );
       }
       private static XmlElement GetEnumXmlElement ( XmlDocument doc , String strEnumName )
       {
           XmlElement eleEnum=doc.CreateElement( "Enum" );
           eleEnum.SetAttribute( "name" , strEnumName );

           #region Items
           foreach ( String strItemName in EnumList[strEnumName] )
           {
               XmlElement eleItem=doc.CreateElement( "Item" );
               eleItem.InnerText=strItemName;
               eleEnum.AppendChild( eleItem );
           }
           #endregion

           return eleEnum;
       }
   }

}
