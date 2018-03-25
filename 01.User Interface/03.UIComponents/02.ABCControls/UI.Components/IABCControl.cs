using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABCControls
{
   public interface IABCControl
    {
       void InitControl ( );
       ABCView OwnerView { get; set; }
       Boolean IsVisible { get; set; }
    }

   public interface IABCCustomControl
   {
        void InitLayout ( ABCView view , XmlNode gridNode );
   }

   public interface IABCBindableControl
   {
       String DataSource { get; set; }
       String DataMember  { get; set; }
       String TableName { get; set; } //TableName of DataSource
       String BindingProperty { get; }
   }
}
