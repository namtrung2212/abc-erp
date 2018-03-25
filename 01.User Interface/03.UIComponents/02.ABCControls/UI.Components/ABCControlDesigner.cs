using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Drawing.Design;
using System.Reflection;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.ViewInfo;


namespace ABCControls
{
    public class ABCControlDesigner : ControlDesigner
    {
        public bool Enabled
        {
            get
            {
                return (bool)ShadowProperties["Enabled"];
            }
            set
            {
                this.ShadowProperties["Enabled"]=value;
            }
        }

        private bool Locked { get; set; }

        public bool Visible
        {
            get
            {
                return (bool)ShadowProperties["Visible"];
            }
            set
            {
                this.ShadowProperties["Visible"]=value;
            }
        }

        public override void Initialize ( IComponent component )
        {
            base.Initialize( component );
            Control control=component as Control;

            if ( control==null )
                throw new ArgumentException();

            this.Visible=control.Visible;
            this.Enabled=control.Enabled;

            control.Visible=true;
            control.Enabled=true;
        }

        protected override void PreFilterProperties ( IDictionary properties )
        {
            base.PreFilterProperties( properties );

            properties["Visible"]=TypeDescriptor.CreateProperty(
                     typeof( ControlDesigner ) ,
                     (PropertyDescriptor)properties["Visible"] ,
                     new Attribute[0] );

            properties["Enabled"]=TypeDescriptor.CreateProperty(
                     typeof( ControlDesigner ) ,
                     (PropertyDescriptor)properties["Enabled"] ,
                     new Attribute[0] );

            properties["Locked"]=TypeDescriptor.CreateProperty(
                     typeof( ControlDesigner ) ,
                     "Locked" ,
                     typeof( bool ) ,
                     CategoryAttribute.Design ,
                     DesignOnlyAttribute.Yes );
        }

    }
}
