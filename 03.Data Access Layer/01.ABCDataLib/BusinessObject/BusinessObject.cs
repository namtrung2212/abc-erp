using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

namespace ABCBusinessEntities
{

    [Serializable()]
    public class BusinessObject : ICloneable , INotifyPropertyChanged , IDisposable , IEditableObject
    {
  
        public String AATableName=String.Empty;
        public Boolean IsItemEntity=false;
        public object ItemEntity;

        public BusinessObject()
        {
        }

        public bool Selected { get; set; }

        #region IEditableObject
        BusinessObject BackupObject;
        bool IsEditing=false;
        public void BeginEdit ( )
        {
            if ( IsEditing==false )
            {
                BackupObject=(BusinessObject)this.Clone();
                IsEditing=true;
            }
        }
        public void CancelEdit ( )
        {
            if ( IsEditing )
            {
                GetFromBusinessObject( BackupObject );
                BackupObject=null;
                IsEditing=false;
            }
        }
        public void EndEdit ( )
        {
            if ( IsEditing )
            {
                BackupObject=null;
                IsEditing=false;
            }
        }
        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged ( PropertyChangedEventArgs e )
        {
            if ( this.PropertyChanged!=null )
                this.PropertyChanged( this , e );
        }
        protected virtual void NotifyChanged(params string[] propertyNames)
        {
            if ( this.PropertyChanged==null )
                return;

            foreach (string name in propertyNames)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(name));                
            }
        }

        #endregion

        #region Clone

        public object Clone()
        {            
            BusinessObject obj=(BusinessObject) this.MemberwiseClone();
            obj.AATableName=this.AATableName;
            return obj;
        }

        public object Clone ( Type destType )
        {
            if ( destType==this.GetType() )
                return this.MemberwiseClone();

            Type orgType=this.GetType();

            BusinessObject newObject=(BusinessObject)Activator.CreateInstance( destType );
            PropertyInfo[] properties=destType.GetProperties();
            foreach ( PropertyInfo prop in properties )
            {
                PropertyInfo orgPro=orgType.GetProperty( prop.Name );
                if ( orgPro!=null )
                {
                    object objValue=ABCDynamicInvoker.GetValue( this , orgPro );
                    ABCDynamicInvoker.SetValue( newObject , prop , objValue );
                }
            }

            return newObject;
        }

        #endregion

        #region Dispose
        ~BusinessObject ( )
        {
            Dispose();
        }
        public void Dispose ( )
        {
            System.GC.SuppressFinalize( this );
        } 
        #endregion

        #region Util
      
        public Guid GetID ( )
        {
          return  BusinessObjectHelper.GetIDValue( this );
        }
        public String GetNoValue ( )
        {
            return BusinessObjectHelper.GetNoValue( this );
        }
        public String GetNameValue ( )
        {
            return BusinessObjectHelper.GetNameValue( this );
        }
    
        public void SetIDValue ( Guid id )
        {
            BusinessObjectHelper.SetIDValue( this , id );
        }
        public void SetNoValue ( String strNo )
        {
            BusinessObjectHelper.SetNOValue( this , strNo );
        }
        public bool IsCleanObject ( )
        {
            return BusinessObjectHelper.IsCleanObject( this );
        }
        public bool IsModifiedObject ( )
        {
            return BusinessObjectHelper.IsModifiedObject( this );
        }
        public bool IsExistObject ( )
        {
            return BusinessObjectHelper.IsExistObject( this );
        }

        public void GetFromBusinessObject ( BusinessObject objBusinessObject )
        {
            BusinessObjectHelper.InitPropertyList( this.AATableName );
            BusinessObjectHelper.InitPropertyList( objBusinessObject.AATableName );

            foreach ( PropertyInfo srcProp in BusinessObjectHelper.PropertyList[objBusinessObject.AATableName].Values )
            {
                PropertyInfo destProp=BusinessObjectHelper.GetProperty(this.AATableName, srcProp.Name );
                if ( destProp!=null )
                {
                    object objValue=ABCDynamicInvoker.GetValue( objBusinessObject , srcProp );
                    ABCDynamicInvoker.SetValue( this , destProp , objValue );
                }
            }
        }

        public BusinessObject SetToBusinessObject ( String strDestTableName )
        {
            BusinessObject objResultObject=BusinessObjectFactory.GetBusinessObject( strDestTableName+"Info" );

            foreach ( PropertyInfo destProp in BusinessObjectHelper.PropertyList[strDestTableName].Values )
            {
                PropertyInfo srcProp=BusinessObjectHelper.GetProperty( this.AATableName , destProp.Name );
                if ( srcProp!=null )
                {
                    object objValue=ABCDynamicInvoker.GetValue( this , srcProp );
                    ABCDynamicInvoker.SetValue( objResultObject , destProp , objValue );
                }
            }

            return objResultObject;
        }

        #endregion
    }
}
