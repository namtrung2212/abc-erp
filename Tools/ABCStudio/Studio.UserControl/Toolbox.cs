using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;
using System.Reflection;
using ABCControls;
using ABCCommon;

namespace ABCStudio
{

    public class ABCToolboxItem : System.Drawing.Design.ToolboxItem
    {

        public ABCControls.ABCBindingInfo Binding;

        public ABCToolboxItem ( Type type ):base(type)
        {
        }
        protected override IComponent[] CreateComponentsCore (
            IDesignerHost host ,
            IDictionary defaultValues )
        {
            ABCView view=(ABCView)host.RootComponent;

            try
            {
                if ( Binding!=null&&Binding.ViewInfo!=null )
                {
                    bool isOK=view.CheckLoopSurface( Binding.ViewInfo.STViewID );
                    if ( !isOK )
                    {
                        ABCHelper.ABCMessageBox.Show( "Can not include this View to another, there will be loop !" , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Warning );
                        return null;
                    }
                }

                if ( this.TypeName==typeof( ABCDockPanel ).FullName )
                {
                    ABCDockPanel panel=ABCDockPanel.AddNewDockPanel( view );
                    panel.PerformDock( view );
                    return null;
                }
                else
                {
                    IComponent[] comps=null;
                    //if (defaultValues["Parent"] is  System.Windows.Forms.FlowLayoutPanel &&
                    //    (defaultValues["Parent"] as System.Windows.Forms.FlowLayoutPanel ).Parent !=null &&
                    //    (defaultValues["Parent"] as System.Windows.Forms.FlowLayoutPanel ).Parent is ABCSearchPanel&&
                    //    defaultValues.Contains( "ToolboxSnapDragDropEventArgs" ) )
                    //{
                    //    DataObject obj=(DataObject)defaultValues["ToolboxSnapDragDropEventArgs"].GetType().GetProperty( "Data" ).GetValue( defaultValues["ToolboxSnapDragDropEventArgs"] , null );
                    //    ABCToolboxItem toolboxItem=    (ABCToolboxItem) obj.GetData(typeof( ABCToolboxItem ));
                    //    if ( toolboxItem.TypeName!="ABCControls.ABCSearchControl" )
                    //    {
                    //        ABCToolboxItem newToolboxItem=new ABCToolboxItem( typeof( ABCSearchControl ) );
                    //        newToolboxItem.Binding=toolboxItem.Binding;
                    //        comps=newToolboxItem.CreateComponentsCore( host );

                    //    }
                    //}

                    //if(comps==null)
                    comps=base.CreateComponentsCore( host , defaultValues );
                    try
                    {
                        if ( Binding!=null )
                        {
                            if ( comps[0] is DevExpress.XtraEditors.GroupControl )
                                ( (DevExpress.XtraEditors.GroupControl)comps[0] ).Text=Binding.FieldName;


                            if ( comps[0] is ABCView&&Binding.ViewInfo!=null )
                            {
                                ( (ABCView)comps[0] ).Load( Binding.ViewInfo , ViewMode.Test );
                                ( (ABCView)comps[0] ).Enabled=false;
                            }

                            if ( comps[0] is ABCCheckEdit )
                                ( (ABCCheckEdit)comps[0] ).Initialize( view , Binding );

                            if ( comps[0] is ABCCheckPanel)
                                ( (ABCCheckPanel)comps[0] ).Initialize( view , Binding );

                            if ( comps[0] is ABCRadioGroup )
                                ( (ABCRadioGroup)comps[0] ).Initialize( Binding );

                            if ( comps[0] is ABCLabel )
                                ( (ABCLabel)comps[0] ).Initialize( view , Binding );

                            if ( comps[0] is ABCBindingBaseEdit )
                            {
                                ( (ABCBindingBaseEdit)comps[0] ).Initialize( view , Binding );
                            }
                            if ( comps[0] is ABCSearchControl )
                            {
                                ABCSearchInfo searchInfo=new ABCSearchInfo();
                                searchInfo.DataSource=Binding.BusName;
                                searchInfo.TableName=Binding.TableName;
                                searchInfo.DataMember=Binding.FieldName;
                                ( (ABCSearchControl)comps[0] ).Initialize( view , searchInfo );
                            }
                            if ( comps[0] is ABCSearchPanel )
                            {
                                ( (ABCSearchPanel)comps[0] ).PopulateControls( view , Binding.BusName , Binding.TableName );
                            }
                            if ( comps[0] is ABCDataPanel )
                            {
                                ( (ABCDataPanel)comps[0] ).PopulateControls( view , Binding.BusName , Binding.TableName );
                            }
                            if ( comps[0] is ABCGridControl&&view.DataConfig!=null&&view.DataConfig.BindingList.ContainsKey( Binding.BusName ) )
                            {
                                if ( view.DataConfig.BindingList[Binding.BusName].IsList )
                                    ( (ABCGridControl)comps[0] ).Initialize( view , Binding );
                            }
                            if ( comps[0] is ABCGridBandedControl&&view.DataConfig!=null&&view.DataConfig.BindingList.ContainsKey( Binding.BusName ) )
                            {
                                if ( view.DataConfig.BindingList[Binding.BusName].IsList )
                                    ( (ABCGridBandedControl)comps[0] ).Initialize( view , Binding );
                            }
                            if ( comps[0] is ABCPivotGridControl&&view.DataConfig!=null&&view.DataConfig.BindingList.ContainsKey( Binding.BusName ) )
                            {
                                if ( view.DataConfig.BindingList[Binding.BusName].IsList )
                                    ( (ABCPivotGridControl)comps[0] ).Initialize( view , Binding );
                            }
                            if ( comps[0] is ABCTreeList&&view.DataConfig!=null&&view.DataConfig.BindingList.ContainsKey( Binding.BusName ) )
                            {
                                if ( view.DataConfig.BindingList[Binding.BusName].IsList )
                                    ( (ABCTreeList)comps[0] ).Initialize( view , Binding );
                            }
                        }
                    }
                    catch ( Exception ex2 )
                    {

                    }
                    return comps;
                }
               

             
            }
            catch ( Exception ex )
            {
            }
            return null;
        }
    }

    public class Toolbox : DevExpress.XtraEditors.XtraUserControl, IToolboxService
    {
        DevExpress.XtraNavBar.NavBarControl navBarControl;

        public System.Drawing.Design.ToolboxItem SelectedToolboxItem;

        public IDesignerHost DesignerHost;

        public Toolbox ( )
        {
            InitializeComponent();


            this.navBarControl.LinkPressed+=new DevExpress.XtraNavBar.NavBarLinkEventHandler( navBarControl_LinkPressed );
            this.navBarControl.DoubleClick+=new EventHandler( navBarControl_DoubleClick );
        }

        public void InitializeToolbox ( )
        {

            AddGroup( "Layout" , "Layout" );
            AddItem( "Layout" , "GroupControl" , typeof( ABCControls.ABCGroupControl ) );
            AddItem( "Layout" , "TabControl" , typeof( ABCControls.ABCTabControl) );
            AddItem( "Layout" , "TabPage" , typeof( DevExpress.XtraTab.XtraTabPage ) );
            AddItem( "Layout" , "PanelControl" , typeof( ABCControls.ABCPanelControl ) );
            AddItem( "Layout" , "CheckPanel" , typeof( ABCControls.ABCCheckPanel ) );
            AddItem( "Layout" , "ScrollableControl" , typeof( ABCControls.ABCScrollableControl ) );
            AddItem( "Layout" , "CollapseGroupControl" , typeof( ABCControls.ABCCollapseGroupControl ) );
            AddItem( "Layout" , "FlowLayoutPanel" , typeof( System.Windows.Forms.FlowLayoutPanel ) );
            AddItem( "Layout" , "DragableFlowLayoutPanel" , typeof( ABCControls.ABCFlowPanelControl ) );
            AddItem( "Layout" , "SplitterControl" , typeof( ABCControls.ABCSplitterControl ) );
            AddItem( "Layout" , "SplitContainerControl" , typeof( ABCSplitterContainer ) );
            AddItem( "Layout" , "GroupBox" , typeof( System.Windows.Forms.GroupBox ) );
            AddItem( "Layout" , "DockPanel" , typeof( ABCControls.ABCDockPanel ) );
             
            
            AddGroup( "Common" , "Common" );
            AddItem( "Common" , "SimpleButton" , typeof( ABCControls.ABCSimpleButton ) );
            AddItem( "Common" , "Label" , typeof( ABCControls.ABCLabel ) );
            AddItem( "Common" , "LinkLabel" , typeof( System.Windows.Forms.LinkLabel ) );
            AddItem( "Common" , "TextEdit" , typeof( ABCControls.ABCTextEdit ) );
            AddItem( "Common" , "ComboBoxEdit" , typeof( ABCControls.ABCComboBoxEdit ) );
            AddItem( "Common" , "MonthEdit" , typeof( ABCControls.ABCMonthEdit ) );
            AddItem( "Common" , "YearEdit" , typeof( ABCControls.ABCYearEdit ) );
            AddItem( "Common" , "MonthYearEdit" , typeof( ABCControls.ABCMonthYearEdit ) );
            AddItem( "Common" , "PeriodEdit" , typeof( ABCControls.ABCPeriodEdit ) );
            AddItem( "Common" , "LookUpEdit" , typeof( ABCControls.ABCLookUpEdit ) );
            AddItem( "Common" , "GridLookUpEdit" , typeof( DevExpress.XtraEditors.GridLookUpEdit ) );
            AddItem( "Common" , "MemoEdit" , typeof( ABCControls.ABCMemoEdit ) );
            AddItem( "Common" , "MemoExEdit" , typeof( ABCControls.ABCMemoExEdit ) );
            AddItem( "Common" , "RichTextEdit" , typeof( ABCControls.ABCRichTextEdit ) );
            AddItem( "Common" , "DateEdit" , typeof( ABCControls.ABCDateEdit ) );
            AddItem( "Common" , "TimeEdit" , typeof( ABCControls.ABCTimeEdit ) );
            AddItem( "Common" , "ButtonEdit" , typeof( ABCControls.ABCButtonEdit ) );
            AddItem( "Common" , "CalcEdit" , typeof( ABCControls.ABCCalcEdit ) );
            AddItem( "Common" , "SpinEdit" , typeof( ABCControls.ABCSpinEdit ) );
            AddItem( "Common" , "ABCSpinEdit2" , typeof( ABCControls.ABCSpinEdit2 ) );
            AddItem( "Common" , "CheckEdit" , typeof( ABCControls.ABCCheckEdit ) );
            AddItem( "Common" , "CheckedListBox" , typeof( ABCControls.ABCCheckedListBox ) );
            AddItem( "Common" , "RadioGroup" , typeof( ABCControls.ABCRadioGroup ) );
            AddItem( "Common" , "GridControl" , typeof( ABCControls.ABCGridControl ) );
            AddItem( "Common" , "BandedGridControl" , typeof( ABCControls.ABCGridBandedControl ) );
            AddItem( "Common" , "PivotGridControl" , typeof( ABCControls.ABCPivotGridControl ) );
            AddItem( "Common" , "ABCTreeList" , typeof( ABCControls.ABCTreeList ) );

            AddItem( "Common" , "PrintingPreview" , typeof( ABCControls.ABCPrintingPreview ) );
            AddItem( "Common" , "RichEditControl" , typeof( ABCControls.ABCRichEditControl ) );
            AddItem( "Common" , "PieChart" , typeof( ABCControls.ABCChartPieControl ) );
            AddItem( "Common" , "BarChart" , typeof( ABCControls.ABCChartBarControl ) );
            AddItem( "Common" , "LineChart" , typeof( ABCControls.ABCChartLineControl ) );
            AddItem( "Common" , "SplineChart" , typeof( ABCControls.ABCChartSplineControl ) );

            AddItem( "Common" , "OneSeriesChart" , typeof( ABCControls.ABCChartOneSeriesControl ) );
            AddItem( "Common" , "TwoSeriesChart" , typeof( ABCControls.ABCChartTwoSeriesControl ) );
            AddItem( "Common" , "ThreeSeriesChart" , typeof( ABCControls.ABCChartThreeSeriesControl ) );
            AddItem( "Common" , "FourSeriesChart" , typeof( ABCControls.ABCChartFourSeriesControl ) );
            AddItem( "Common" , "FiveSeriesChart" , typeof( ABCControls.ABCChartFiveSeriesControl ) );
            AddItem( "Common" , "SixSeriesChart" , typeof( ABCControls.ABCChartSixSeriesControl ) );

            AddGroup( "Binding" , "Binding" );
            AddItem( "Binding" , "BindingTextEdit" , typeof( ABCControls.ABCBindingBaseEdit ) );
            AddItem( "Binding" , "ABCComment" , typeof( ABCControls.ABCComment ) );
        
            AddGroup( "Others" , "Others" );
            AddItem( "Others" , "WebBrowser" , typeof( System.Windows.Forms.WebBrowser ) );
          

        }

        #region Generate Control

        void navBarControl_LinkPressed ( object sender , DevExpress.XtraNavBar.NavBarLinkEventArgs e )
        {

            SelectedToolboxItem=(System.Drawing.Design.ToolboxItem)e.Link.Item.Tag;

            IToolboxService tbs=this;
            DataObject d=tbs.SerializeToolboxItem( SelectedToolboxItem ) as DataObject;

            try
            {
                this.DoDragDrop( d , DragDropEffects.Copy );
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
            }
            SelectedToolboxItem=null;
        }
        void navBarControl_DoubleClick ( object sender , EventArgs e )
        {
            if ( SelectedToolboxItem!=null )
            {
                IToolboxUser tbu=this.DesignerHost.GetDesigner( this.DesignerHost.RootComponent as IComponent ) as IToolboxUser;
                if ( tbu!=null )
                    tbu.ToolPicked( SelectedToolboxItem );
            }
        }
        
        #endregion

        #region Init Toolbox

        public void AddGroup ( String strGroupName , String strGroupCaption )
        {
            DevExpress.XtraNavBar.NavBarGroup group=new DevExpress.XtraNavBar.NavBarGroup();
            group.Caption=strGroupCaption;
            group.Expanded=true;
            group.Name=strGroupName;

            this.navBarControl.Groups.Add( group );
            this.navBarControl.ActiveGroup=group;
        }

        #region AddItem

        public void AddItem ( String strGroupName , String strItemName , Type type )
        {
            TypeResolutionService.AddType( type );

            if ( DefaultIcon==null )
                DefaultIcon=GetIcon( typeof( ABCControls.ABCGroupControl ) , false );

            DevExpress.XtraNavBar.NavBarItem item=new DevExpress.XtraNavBar.NavBarItem();
            item.Caption=strItemName;
            item.Name=strItemName;

            ABCToolboxItem toolboxItem=new ABCToolboxItem( type );
            toolboxItem.Binding=new ABCControls.ABCBindingInfo();
            toolboxItem.Binding.FieldName=strItemName;
            item.Tag=toolboxItem;

            Bitmap icon=GetIcon( type , false );
            if ( ImageCompareString( icon , DefaultIcon ) )
                icon=GetIcon( type , true );

        //    icon.Save( String.Format(@"Icons\{0}.png" , strItemName ));
            item.SmallImage=icon;
            this.navBarControl.Items.Add( item );
            this.navBarControl.Groups[strGroupName].ItemLinks.Add( item );

            HostSurfaceManager.CreateTemplateComponent( type );
          }

        System.Drawing.Bitmap DefaultIcon=null;
        public Bitmap GetIcon ( Type type , bool isUseParent )
        {
            System.Drawing.ToolboxBitmapAttribute tba;
            if ( isUseParent )
                tba=TypeDescriptor.GetAttributes( type.BaseType )[typeof( System.Drawing.ToolboxBitmapAttribute )] as System.Drawing.ToolboxBitmapAttribute;
            else
                tba=TypeDescriptor.GetAttributes( type )[typeof( System.Drawing.ToolboxBitmapAttribute )] as System.Drawing.ToolboxBitmapAttribute;

            return (System.Drawing.Bitmap)tba.GetImage( type );

        }
        public static bool ImageCompareString ( Bitmap firstImage , Bitmap secondImage )
        {

            MemoryStream ms=new MemoryStream();
            firstImage.Save( ms , System.Drawing.Imaging.ImageFormat.Png );
            String firstBitmap=Convert.ToBase64String( ms.ToArray() );

            ms.Position=0;

            secondImage.Save( ms , System.Drawing.Imaging.ImageFormat.Png );
            String secondBitmap=Convert.ToBase64String( ms.ToArray() );

            if ( firstBitmap.Equals( secondBitmap ) )
                return true;
            else
                return false;

        }

        #endregion 
        #endregion

        #region IToolboxService Members

        public System.Drawing.Design.ToolboxItem GetSelectedToolboxItem ( IDesignerHost host )
        {
            if ( SelectedToolboxItem!=null && SelectedToolboxItem.DisplayName!="<Pointer>" )
                return SelectedToolboxItem;
            else
                return null;
        }


        public System.Drawing.Design.ToolboxItem GetSelectedToolboxItem ( )
        {
            return this.GetSelectedToolboxItem( null );
        }

        public void AddToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem , string category )
        {
        }

        public void AddToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem )
        {
        }

        public bool IsToolboxItem ( object serializedObject , IDesignerHost host )
        {
            return false;
        }

        public bool IsToolboxItem ( object serializedObject )
        {
            return false;
        }

        public void SetSelectedToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem )
        {
        }

        public void SelectedToolboxItemUsed ( )
        {
            //ListBox list=this.ToolsListBox;

            //list.Invalidate( list.GetItemRectangle( selectedIndex ) );
            //selectedIndex=0;
            //list.SelectedIndex=0;
            //list.Invalidate( list.GetItemRectangle( selectedIndex ) );
        }

        public CategoryNameCollection CategoryNames
        {
            get
            {
                return null;
            }
        }

        void IToolboxService.Refresh ( )
        {
        }

        public void AddLinkedToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem , string category , IDesignerHost host )
        {
        }

        public void AddLinkedToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem , IDesignerHost host )
        {
        }

        public bool IsSupported ( object serializedObject , ICollection filterAttributes )
        {
            return false;
        }

        public bool IsSupported ( object serializedObject , IDesignerHost host )
        {
            return false;
        }

        public string SelectedCategory
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public System.Drawing.Design.ToolboxItem DeserializeToolboxItem ( object serializedObject , IDesignerHost host )
        {
            return (ABCToolboxItem)( (DataObject)serializedObject ).GetData(typeof( ABCToolboxItem ) );
        }

        public System.Drawing.Design.ToolboxItem DeserializeToolboxItem ( object serializedObject )
        {
            return this.DeserializeToolboxItem( serializedObject , this.DesignerHost );
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems ( string category , IDesignerHost host )
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems ( string category )
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems ( IDesignerHost host )
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems ( )
        {
            return null;
        }

        public void AddCreator ( ToolboxItemCreatorCallback creator , string format , IDesignerHost host )
        {
        }

        public void AddCreator ( ToolboxItemCreatorCallback creator , string format )
        {
        }

        public bool SetCursor ( )
        {
            return false;
        }

        public void RemoveToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem , string category )
        {
        }

        public void RemoveToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem )
        {
        }

        public object SerializeToolboxItem ( System.Drawing.Design.ToolboxItem toolboxItem )
        {
            return new DataObject( toolboxItem );
        }

        public void RemoveCreator ( string format , IDesignerHost host )
        {
        }

        public void RemoveCreator ( string format )
        {
        }

        #endregion

        private void InitializeComponent ( )
        {
            this.navBarControl=new DevExpress.XtraNavBar.NavBarControl();
            ( (System.ComponentModel.ISupportInitialize)( this.navBarControl ) ).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl1
            // 
            this.navBarControl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.navBarControl.Location=new System.Drawing.Point( 0 , 0 );
            this.navBarControl.Name="navBarControl1";
            this.navBarControl.Size=new System.Drawing.Size( 184 , 174 );
            this.navBarControl.TabIndex=0;
            this.navBarControl.Text="navBarControl1";
            // 
            // Toolbox
            // 
            this.Controls.Add( this.navBarControl );
            this.Name="Toolbox";
            this.Size=new System.Drawing.Size( 184 , 174 );
            ( (System.ComponentModel.ISupportInitialize)( this.navBarControl ) ).EndInit();
            this.ResumeLayout( false );

        }


    }
}
