using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;
using DevExpress.Utils;
using ABCProvider;

namespace ABCProvider
{

   
    public class DataFormatProvider
    {
        public interface IDontNeedFormatControl
        {
        }

        public class ABCFormatInfo
        {
            public FormatInfo FormatInfo=new FormatInfo();
            public DevExpress.XtraEditors.Mask.MaskType MaskType;
            public String EditMask;
            public FieldFormat ABCFormat=FieldFormat.None;
        }
        public enum FieldFormat
        {
            None ,
            Quantity ,
            Amount ,
            Currency ,
            Percent ,
            DateAndTime ,
            Date ,
            Time,
            MonthAndYear,
            Year,
            Month
        }

        static Dictionary<FieldFormat , ABCFormatInfo> FormatList=new Dictionary<FieldFormat , ABCFormatInfo>();
        static DataFormatProvider ( )
        {
            ABCFormatInfo quantity=new ABCFormatInfo();
            quantity.FormatInfo.FormatType=FormatType.Numeric;
            quantity.FormatInfo.FormatString="D";
            quantity.MaskType=DevExpress.XtraEditors.Mask.MaskType.Numeric;
            quantity.EditMask="D";
            quantity.ABCFormat=FieldFormat.Quantity;
            FormatList.Add( FieldFormat.Quantity , quantity );

            ABCFormatInfo amount=new ABCFormatInfo();
            amount.FormatInfo.FormatType=FormatType.Numeric;
            amount.FormatInfo.FormatString="N";
            amount.MaskType=DevExpress.XtraEditors.Mask.MaskType.Numeric;
            amount.EditMask="N";
            amount.ABCFormat=FieldFormat.Amount;
            FormatList.Add( FieldFormat.Amount , amount );

            ABCFormatInfo currency=new ABCFormatInfo();
            currency.FormatInfo.FormatType=FormatType.Numeric;
            currency.FormatInfo.FormatString="C0";
            currency.MaskType=DevExpress.XtraEditors.Mask.MaskType.Numeric;
            currency.EditMask="C0";
            currency.ABCFormat=FieldFormat.Currency;
            FormatList.Add( FieldFormat.Currency , currency );

            ABCFormatInfo percent=new ABCFormatInfo();
            percent.FormatInfo.FormatType=FormatType.Numeric;
            percent.FormatInfo.FormatString="p0";
            percent.MaskType=DevExpress.XtraEditors.Mask.MaskType.Numeric;
            percent.EditMask="p0";
            percent.ABCFormat=FieldFormat.Percent;
            FormatList.Add( FieldFormat.Percent , percent );

            ABCFormatInfo dateAndtime=new ABCFormatInfo();
            dateAndtime.FormatInfo.FormatType=FormatType.DateTime;
            dateAndtime.FormatInfo.FormatString="g";
            dateAndtime.EditMask="g";
            dateAndtime.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            dateAndtime.ABCFormat=FieldFormat.DateAndTime;
            FormatList.Add( FieldFormat.DateAndTime , dateAndtime );

            ABCFormatInfo date=new ABCFormatInfo();
            date.FormatInfo.FormatType=FormatType.DateTime;
            date.FormatInfo.FormatString="d";//d
            date.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            date.EditMask="d";
            date.ABCFormat=FieldFormat.Date;
            FormatList.Add( FieldFormat.Date , date );

            ABCFormatInfo time=new ABCFormatInfo();
            time.FormatInfo.FormatType=FormatType.DateTime;
            time.FormatInfo.FormatString="t";
            time.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            time.EditMask="d";
            time.ABCFormat=FieldFormat.Time;
            FormatList.Add( FieldFormat.Time , time );


            ABCFormatInfo monthAndYear=new ABCFormatInfo();
            monthAndYear.FormatInfo.FormatType=FormatType.DateTime;
            monthAndYear.FormatInfo.FormatString="MM/yyyy";
            monthAndYear.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            monthAndYear.EditMask="MM/yyyy";
            monthAndYear.ABCFormat=FieldFormat.MonthAndYear;
            FormatList.Add( FieldFormat.MonthAndYear , monthAndYear );

            ABCFormatInfo year=new ABCFormatInfo();
            year.FormatInfo.FormatType=FormatType.DateTime;
            year.FormatInfo.FormatString="yyyy";
            year.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            year.EditMask="yyyy";
            year.ABCFormat=FieldFormat.Year;
            FormatList.Add( FieldFormat.Year , year );

            ABCFormatInfo month=new ABCFormatInfo();
            month.FormatInfo.FormatType=FormatType.DateTime;
            month.FormatInfo.FormatString="MM";
            month.MaskType=DevExpress.XtraEditors.Mask.MaskType.DateTime;
            month.EditMask="MM";
            month.ABCFormat=FieldFormat.Month;
            FormatList.Add( FieldFormat.Month , month );

        }

        public static ABCFormatInfo GetFormatInfo ( FieldFormat format )
        {
            ABCFormatInfo result=null;
            FormatList.TryGetValue( format , out result );
            return result;
        }

        public static ABCFormatInfo GetFormatInfo ( String strTableName , String strFieldString )
        {
            if ( String.IsNullOrWhiteSpace( strFieldString )||String.IsNullOrWhiteSpace( strTableName ) )
                return null;

            String TableName=strTableName;
            String FieldName=strFieldString;

            if ( strFieldString.Contains( ":" ) )
            {
                DataCachingProvider.AccrossStructInfo structInfo=DataCachingProvider.GetAccrossStructInfo( strTableName , strFieldString );
                if ( structInfo!=null )
                {
                    TableName=structInfo.TableName;
                    FieldName=structInfo.FieldName;
                }
            }

            if ( DataStructureProvider.IsTableColumn( TableName , FieldName )==false )
                return null;

            if ( DataStructureProvider.IsForeignKey( TableName , FieldName ) )
            {
                TableName=DataStructureProvider.GetTableNameOfForeignKey( TableName , FieldName );
                FieldName=DataStructureProvider.GetDisplayColumn( TableName );
            }
            if ( DataConfigProvider.TableConfigList.ContainsKey( TableName )
                &&DataConfigProvider.TableConfigList[TableName].FieldConfigList.ContainsKey( FieldName ) )
                return GetFormatInfo( DataConfigProvider.TableConfigList[TableName].FieldConfigList[FieldName].Format );

            return null;
        }

        public static void SetControlFormat ( Control control , String strTableName , String strFieldString )
        {
            if ( control is DevExpress.XtraEditors.BaseEdit&& control is IDontNeedFormatControl ==false )
            {
                DevExpress.XtraEditors.BaseEdit editControl=control as DevExpress.XtraEditors.BaseEdit;
                ABCFormatInfo formatInfo=GetFormatInfo( strTableName , strFieldString );
                if ( formatInfo!=null )
                {
                    editControl.Properties.Appearance.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far;
                    editControl.Properties.DisplayFormat.FormatType=FormatType.None;
                    editControl.Properties.DisplayFormat.FormatString=String.Empty;
                    editControl.Properties.EditFormat.FormatType=FormatType.None;
                    editControl.Properties.EditFormat.FormatString=String.Empty;
                
                    if ( editControl.Properties is DevExpress.XtraEditors.Repository.RepositoryItemTextEdit )
                    {
                        ( editControl.Properties as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.UseMaskAsDisplayFormat=true;
                        ( editControl.Properties as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.EditMask=formatInfo.EditMask;
                        ( editControl.Properties as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ).Mask.MaskType=formatInfo.MaskType;
                    }
                    else
                    {
                        editControl.Properties.DisplayFormat.FormatType=formatInfo.FormatInfo.FormatType;
                        editControl.Properties.DisplayFormat.FormatString=formatInfo.FormatInfo.FormatString;
                        editControl.Properties.EditFormat.FormatType=formatInfo.FormatInfo.FormatType;
                        editControl.Properties.EditFormat.FormatString=formatInfo.FormatInfo.FormatString;
                    }
                }

            }
        }

        public static String DoFormat ( object objValue , String strTableName , String strFieldString )
        {
            if ( objValue==null )
                return String.Empty;

            String strResult=String.Empty;
            ABCFormatInfo formatInfo=GetFormatInfo( strTableName , strFieldString );
            if ( formatInfo!=null )
                strResult=formatInfo.FormatInfo.GetDisplayText( objValue );

            if ( String.IsNullOrWhiteSpace( strResult ) )
                strResult=Convert.ToString( objValue );

            return strResult;
        }

    }

}
