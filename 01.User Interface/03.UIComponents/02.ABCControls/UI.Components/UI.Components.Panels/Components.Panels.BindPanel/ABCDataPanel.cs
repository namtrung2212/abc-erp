using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;

using ABCProvider;
using ABCBusinessEntities;
using ABCCommon;

namespace ABCControls
{

    [Designer( typeof( DataPanelDesigner ) )]
    public partial class ABCDataPanel : System.Windows.Forms.FlowLayoutPanel
    {

        ABCView ViewOwner;

        [Category( "ABC.BindingValue" )]
        public String DataSource { get; set; }

        [ReadOnly( true )]
        public String TableName { get; set; }


        public ABCDataPanel ( )
        {
            this.Margin=new Padding( 1 );
        }

        public void PopulateControls ( ABCView view , String BusName , String strTableName )
        {
            ViewOwner=view;
            DataSource=BusName;
            TableName=strTableName;
            int iCount=0;

            foreach ( DataConfigProvider.FieldConfig fieldConfig in DataConfigProvider.TableConfigList[strTableName].FieldConfigList.Values )
            {
                //if ( fieldConfig.IsDefault )
                //{
                String strDiplay=DataConfigProvider.GetFieldCaption( strTableName , fieldConfig.FieldName );
                if ( strDiplay==fieldConfig.FieldName )
                    continue;

                ABCBindingBaseEdit ctrl;

                if ( view.Mode==ViewMode.Design )
                    ctrl=(ABCBindingBaseEdit)view.Surface.DesignerHost.CreateComponent( typeof( ABCBindingBaseEdit ) );
                else
                    ctrl=(ABCBindingBaseEdit)ABCDynamicInvoker.CreateInstanceObject( typeof( ABCBindingBaseEdit ) );

                ABCBindingInfo bindInfo=new ABCBindingInfo();
                bindInfo.BusName=BusName;
                bindInfo.TableName=TableName;
                bindInfo.FieldName=fieldConfig.FieldName;
                List<Type> lstType=ABCControls.ABCPresentHelper.GetControlTypes( TableName , fieldConfig.FieldName );
                if ( lstType.Count<=0 )
                    continue;
                foreach ( Type type in lstType )
                {
                    if ( type!=typeof( ABCSearchControl ) )
                    {
                        bindInfo.ControlType=lstType[0];
                        break;
                    }
                }
                ctrl.Initialize( view , bindInfo );
                ctrl.Parent=this;
                iCount++;
                //}
            }
            if ( iCount%2!=0 )
                iCount++;
            iCount=iCount/2;
            this.Size=new Size( 229*2+4*3 , 20*iCount+( iCount+1 )*5 );
        }
       

    }

    public class DataPanelDesigner : ParentControlDesigner
    {
     
    }


}
