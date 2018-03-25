using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using ABCControls;


namespace ABCStudio
{

    public class ComponentObject
    {
        private String name=String.Empty;
        private String type=String.Empty;

        public IComponent Component;
        public String Type
        {
            get { return type; }
            set { type=value; }
        }
        public String Name
        {
            get { return name; }
            set { name=value; }
        }

        public ComponentObject ( String strName , String strType )
        {
            Type=strType;
            Name=strName;
        }
    }
    public partial class ControlCollectionGrid : DevExpress.XtraEditors.XtraUserControl
    {
        Studio Studio;

        List<ComponentObject> DataList=new List<ComponentObject>();

        public ControlCollectionGrid ( Studio parent )
        {
            InitializeComponent();
            Studio=parent;
        }

        public void RefreshList ( )
        {
            DataList.Clear();
            HostSurface surface=(HostSurface)Studio.SurfaceManager.ActiveDesignSurface;
            if ( surface==null )
                return;

            foreach ( IComponent comp in surface.DesignerHost.Container.Components )
            {
                ComponentObject obj=new ComponentObject( comp.Site.Name , comp.GetType().Name );
                obj.Component=comp;
                DataList.Add( obj );
            }

            this.gridControl1.DataSource=DataList;
            this.gridControl1.RefreshDataSource();
            this.gridView1.OptionsBehavior.Editable=false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell=false;
            this.gridView1.RowClick+=new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler( gridView1_RowClick );
            this.gridView1.CustomDrawCell+=new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler( gridView1_CustomDrawCell );
        }

        void gridView1_RowClick ( object sender , DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e )
        {
            ComponentObject obj=(ComponentObject)this.gridView1.GetRow( this.gridView1.FocusedRowHandle );
            if ( obj!=null )
            {
                HostSurface surface=(HostSurface)Studio.SurfaceManager.ActiveDesignSurface;
                surface.ServiceSelection.SetSelectedComponents( new Component[] { (Component)obj.Component } );
            }
        }

        void gridView1_CustomDrawCell ( object sender , DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e )
        {
            if ( e.Column.FieldName=="Name" )
                e.Appearance.Font=new Font( e.Appearance.Font , FontStyle.Bold );
        }
        public void SetFocusComponent ( String strControlName )
        {
            for ( int i=0; i<DataList.Count; i++ )
            {
                if ( DataList[i].Name==strControlName )
                    this.gridView1.FocusedRowHandle=this.gridView1.GetRowHandle( i );
            }
        }
    }
}
