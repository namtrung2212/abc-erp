using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;

using System.Windows.Forms;
using DevExpress.XtraEditors;

using ABCBusinessEntities;
using System.Xml;

namespace ABCStudio.Wizard
{
    public partial class NewView : DevExpress.XtraEditors.XtraForm
    {
       
        public Studio OwnerStudio;

        public NewView ( Studio studio )
        {
            OwnerStudio=studio;

            InitializeComponent();
            this.wizardControl1.CancelClick+=new CancelEventHandler( OnCancelClick );
            this.wizardControl1.FinishClick+=new CancelEventHandler( OnFinishClick );
            this.wizardControl1.NextClick+=new DevExpress.XtraWizard.WizardCommandButtonClickEventHandler( OnNextClick );

            pnlDatabase.Enabled=chkToDatabase.Checked;
            chkToXML.Checked=!chkToDatabase.Checked;
            pnlXML.Enabled=chkToXML.Checked;

            lkeGroup.IsModified=false;
            this.Text="New View Wizard....";
            this.StartPosition=FormStartPosition.CenterScreen;

            InitViewGroupList();

        }

        void OnNextClick ( object sender , DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e )
        {
            dxErrorProvider1.ClearErrors();

            if ( e.Page==InputInfoPage )
            {
                #region InputInfoPage
                if ( chkToXML.Checked )
                {
                    FileInfo file=null;
                    try
                    {
                        new FileInfo( btnXMLFile.Text );
                    }
                    catch ( Exception ex )
                    {
                        dxErrorProvider1.SetError( btnXMLFile , "Please choose a location to save XML file" );
                        e.Handled=true;
                        return;
                    }
                }
                if ( chkToDatabase.Checked )
                {
                    if ( String.IsNullOrWhiteSpace( txtViewName.Text ) )
                    {
                        dxErrorProvider1.SetError( txtViewName , "View name can not be empty!" );
                        e.Handled=true;
                    }
                    if ( String.IsNullOrWhiteSpace( lkeGroup.Text ) )
                    {
                        dxErrorProvider1.SetError( lkeGroup , "Please choose a group !" );
                        e.Handled=true;
                    }
                    if ( String.IsNullOrWhiteSpace( txtViewName.Text )==false&&String.IsNullOrWhiteSpace( lkeGroup.Text )==false )
                    {
                            STViewsInfo viewInfo=(STViewsInfo)new STViewsController().GetObjectByNo( txtViewName.Text );
                            if ( viewInfo!=null )
                            {
                                dxErrorProvider1.SetError( txtViewName , "View name has been existed!" );
                                e.Handled=true;
                            }
                    }


                } 
                #endregion
            }
           
        }

        void OnFinishClick ( object sender , CancelEventArgs e )
        {
            if ( chkToXML.Checked )
            {
                #region XML
                FileInfo fileInfo=null;
                try
                {
                    fileInfo=new FileInfo( btnXMLFile.Text );
                }
                catch ( Exception ex )
                {

                }

                XmlDocument doc=ABCControls.ABCView.GetEmptyXMLLayout( fileInfo.Name );
                doc.Save( btnXMLFile.Text );
                OwnerStudio.Worker.OpenFromXMLFile( btnXMLFile.Text );
                #endregion
            }

            if ( chkToDatabase.Checked )
            {
                #region Database
                STViewGroupsController groupCtrl=new STViewGroupsController();
                STViewGroupsInfo groupInfo=(STViewGroupsInfo)groupCtrl.GetObjectByNo( lkeGroup.Text );
                if ( groupInfo==null )
                {
                    groupInfo=new STViewGroupsInfo();
                    groupInfo.No=lkeGroup.Text;
                    groupInfo.Name=lkeGroup.Text;
                    groupCtrl.CreateObject( groupInfo );
                }

                STViewsController viewCtrl=new STViewsController();
                STViewsInfo viewInfo=(STViewsInfo)viewCtrl.GetObjectByNo( txtViewName.Text );
                if ( viewInfo==null )
                {
                    viewInfo=new STViewsInfo();
                    viewInfo.STViewNo=txtViewName.Text;
                    viewInfo.STViewName=txtViewName.Text;
                    viewInfo.FK_STViewGroupID=groupInfo.STViewGroupID;
                    String strXML=ABCControls.ABCView.GetEmptyXMLLayout( txtViewName.Text ).InnerXml;
                    viewInfo.STViewXML=ABCHelper.StringCompressor.CompressString( strXML );
               
                    viewCtrl.CreateObject( viewInfo );
                }

                OwnerStudio.Worker.OpenFromDatabase( viewInfo );
                OwnerStudio.ViewTreeList.RefreshViewList();
                #endregion
            }
            this.Close();

        }

        void OnCancelClick ( object sender , CancelEventArgs e )
        {
            this.Close();
        }

        #region Page 1 - Input Information

        private void InitViewGroupList ( )
        {
            STViewGroupsController groupCtrl=new STViewGroupsController();
            //  List<BusinessObject> lst=(List<BusinessObject>)groupCtrl.GetListFromDataset( groupCtrl.GetAllObjects() );
            DataSet ds=groupCtrl.GetDataSetAllObjects();
            if ( ds!=null&&ds.Tables.Count>0 )
                this.lkeGroup.Properties.DataSource=ds.Tables[0];

        }
        private void chkToXML_CheckedChanged ( object sender , EventArgs e )
        {
            pnlXML.Enabled=chkToXML.Checked;
            chkToDatabase.Checked=!chkToXML.Checked;
            pnlDatabase.Enabled=chkToDatabase.Checked;
        }
        private void chkToDatabase_CheckedChanged ( object sender , EventArgs e )
        {
            pnlDatabase.Enabled=chkToDatabase.Checked;
            chkToXML.Checked=!chkToDatabase.Checked;
            pnlXML.Enabled=chkToXML.Checked;
        }
        private void btnXMLFile_ButtonClick ( object sender , DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e )
        {
            SaveFileDialog dlg=new SaveFileDialog();
            dlg.Filter="xml files (*.xml)|*.xml";
            dlg.DefaultExt="xml";
            dlg.FilterIndex=2;
            dlg.RestoreDirectory=true;
            dlg.Title="Choose XML file location";
            if ( dlg.ShowDialog()==DialogResult.OK )
                btnXMLFile.Text=dlg.FileName;
        } 
        #endregion
    }
}