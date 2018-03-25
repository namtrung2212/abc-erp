namespace ABCStudio.Wizard
{
    partial class NewView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing&&( components!=null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.components=new System.ComponentModel.Container();
            this.wizardControl1=new DevExpress.XtraWizard.WizardControl();
            this.InputInfoPage=new DevExpress.XtraWizard.WelcomeWizardPage();
            this.chkToXML=new DevExpress.XtraEditors.CheckEdit();
            this.chkToDatabase=new DevExpress.XtraEditors.CheckEdit();
            this.pnlXML=new DevExpress.XtraEditors.PanelControl();
            this.btnXMLFile=new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl4=new DevExpress.XtraEditors.LabelControl();
            this.pnlDatabase=new DevExpress.XtraEditors.PanelControl();
            this.labelControl1=new DevExpress.XtraEditors.LabelControl();
            this.labelControl2=new DevExpress.XtraEditors.LabelControl();
            this.txtViewName=new DevExpress.XtraEditors.TextEdit();
            this.wizardPage1=new DevExpress.XtraWizard.WizardPage();
            this.completionWizardPage1=new DevExpress.XtraWizard.CompletionWizardPage();
            this.dxErrorProvider1=new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider( this.components );
            this.lkeGroup=new DevExpress.XtraEditors.LookUpEdit();
            ( (System.ComponentModel.ISupportInitialize)( this.wizardControl1 ) ).BeginInit();
            this.wizardControl1.SuspendLayout();
            this.InputInfoPage.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.chkToXML.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chkToDatabase.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pnlXML ) ).BeginInit();
            this.pnlXML.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.btnXMLFile.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pnlDatabase ) ).BeginInit();
            this.pnlDatabase.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.txtViewName.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dxErrorProvider1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.lkeGroup.Properties ) ).BeginInit();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.Controls.Add( this.InputInfoPage );
            this.wizardControl1.Controls.Add( this.wizardPage1 );
            this.wizardControl1.Controls.Add( this.completionWizardPage1 );
            this.wizardControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.ImageWidth=120;
            this.wizardControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.wizardControl1.Name="wizardControl1";
            this.wizardControl1.Pages.AddRange( new DevExpress.XtraWizard.BaseWizardPage[] {
            this.InputInfoPage,
            this.wizardPage1,
            this.completionWizardPage1} );
            this.wizardControl1.Size=new System.Drawing.Size( 461 , 331 );
            this.wizardControl1.Text="Create a new View";
            this.wizardControl1.WizardStyle=DevExpress.XtraWizard.WizardStyle.WizardAero;
            // 
            // InputInfoPage
            // 
            this.InputInfoPage.Controls.Add( this.chkToXML );
            this.InputInfoPage.Controls.Add( this.chkToDatabase );
            this.InputInfoPage.Controls.Add( this.pnlXML );
            this.InputInfoPage.Controls.Add( this.pnlDatabase );
            this.InputInfoPage.IntroductionText="A new View will be automatically created. Please input some information :";
            this.InputInfoPage.Name="InputInfoPage";
            this.InputInfoPage.Size=new System.Drawing.Size( 401 , 170 );
            this.InputInfoPage.Text="View Info";
            // 
            // chkToXML
            // 
            this.chkToXML.Location=new System.Drawing.Point( 3 , 104 );
            this.chkToXML.Name="chkToXML";
            this.chkToXML.Properties.Caption="Store new View in XML file";
            this.chkToXML.Size=new System.Drawing.Size( 204 , 19 );
            this.chkToXML.TabIndex=8;
            this.chkToXML.CheckedChanged+=new System.EventHandler( this.chkToXML_CheckedChanged );
            // 
            // chkToDatabase
            // 
            this.chkToDatabase.EditValue=true;
            this.chkToDatabase.Location=new System.Drawing.Point( 3 , 3 );
            this.chkToDatabase.Name="chkToDatabase";
            this.chkToDatabase.Properties.Caption="Create new View directly to database";
            this.chkToDatabase.Size=new System.Drawing.Size( 204 , 19 );
            this.chkToDatabase.TabIndex=7;
            this.chkToDatabase.CheckedChanged+=new System.EventHandler( this.chkToDatabase_CheckedChanged );
            // 
            // pnlXML
            // 
            this.pnlXML.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.pnlXML.Appearance.BackColor=System.Drawing.Color.Transparent;
            this.pnlXML.Appearance.Options.UseBackColor=true;
            this.pnlXML.BorderStyle=DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.pnlXML.Controls.Add( this.btnXMLFile );
            this.pnlXML.Controls.Add( this.labelControl4 );
            this.pnlXML.Location=new System.Drawing.Point( 3 , 124 );
            this.pnlXML.Name="pnlXML";
            this.pnlXML.Size=new System.Drawing.Size( 395 , 39 );
            this.pnlXML.TabIndex=6;
            // 
            // btnXMLFile
            // 
            this.btnXMLFile.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnXMLFile.Location=new System.Drawing.Point( 89 , 9 );
            this.btnXMLFile.Name="btnXMLFile";
            this.btnXMLFile.Properties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()} );
            this.btnXMLFile.Size=new System.Drawing.Size( 297 , 20 );
            this.btnXMLFile.TabIndex=9;
            this.btnXMLFile.ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler( this.btnXMLFile_ButtonClick );
            // 
            // labelControl4
            // 
            this.labelControl4.Location=new System.Drawing.Point( 16 , 13 );
            this.labelControl4.Name="labelControl4";
            this.labelControl4.Size=new System.Drawing.Size( 41 , 13 );
            this.labelControl4.TabIndex=3;
            this.labelControl4.Text="XML File ";
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.pnlDatabase.Appearance.BackColor=System.Drawing.Color.Transparent;
            this.pnlDatabase.Appearance.Options.UseBackColor=true;
            this.pnlDatabase.BorderStyle=DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.pnlDatabase.Controls.Add( this.labelControl1 );
            this.pnlDatabase.Controls.Add( this.labelControl2 );
            this.pnlDatabase.Controls.Add( this.txtViewName );
            this.pnlDatabase.Controls.Add( this.lkeGroup );
            this.pnlDatabase.Location=new System.Drawing.Point( 3 , 23 );
            this.pnlDatabase.Name="pnlDatabase";
            this.pnlDatabase.Size=new System.Drawing.Size( 395 , 70 );
            this.pnlDatabase.TabIndex=5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location=new System.Drawing.Point( 16 , 15 );
            this.labelControl1.Name="labelControl1";
            this.labelControl1.Size=new System.Drawing.Size( 51 , 13 );
            this.labelControl1.TabIndex=0;
            this.labelControl1.Text="ViewGroup";
            // 
            // labelControl2
            // 
            this.labelControl2.Location=new System.Drawing.Point( 16 , 43 );
            this.labelControl2.Name="labelControl2";
            this.labelControl2.Size=new System.Drawing.Size( 52 , 13 );
            this.labelControl2.TabIndex=3;
            this.labelControl2.Text="View Name";
            // 
            // txtViewName
            // 
            this.txtViewName.Location=new System.Drawing.Point( 89 , 39 );
            this.txtViewName.Name="txtViewName";
            this.txtViewName.Size=new System.Drawing.Size( 197 , 20 );
            this.txtViewName.TabIndex=2;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Name="wizardPage1";
            this.wizardPage1.Size=new System.Drawing.Size( 401 , 170 );
            this.wizardPage1.Text="Binding Configuration";
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Name="completionWizardPage1";
            this.completionWizardPage1.Size=new System.Drawing.Size( 401 , 170 );
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl=this;
            // 
            // cmbGroup
            // 
            this.lkeGroup.Location=new System.Drawing.Point( 89 , 11 );
            this.lkeGroup.Name="cmbGroup";
            this.lkeGroup.Properties.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.lkeGroup.Properties.Columns.AddRange( new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("No", "No"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name")} );
            this.lkeGroup.Properties.DisplayMember="Name";
            this.lkeGroup.Properties.NullText="";
            this.lkeGroup.Properties.PopupSizeable=false;
            this.lkeGroup.Properties.TextEditStyle=DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lkeGroup.Properties.ValueMember="STViewGroupID";
            this.lkeGroup.Size=new System.Drawing.Size( 197 , 20 );
            this.lkeGroup.TabIndex=1;
            // 
            // NewView
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 461 , 331 );
            this.Controls.Add( this.wizardControl1 );
            this.Name="NewView";
            this.Text="XtraForm1";
            ( (System.ComponentModel.ISupportInitialize)( this.wizardControl1 ) ).EndInit();
            this.wizardControl1.ResumeLayout( false );
            this.InputInfoPage.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.chkToXML.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.chkToDatabase.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pnlXML ) ).EndInit();
            this.pnlXML.ResumeLayout( false );
            this.pnlXML.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.btnXMLFile.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pnlDatabase ) ).EndInit();
            this.pnlDatabase.ResumeLayout( false );
            this.pnlDatabase.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.txtViewName.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.dxErrorProvider1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.lkeGroup.Properties ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wizardControl1;
        private DevExpress.XtraWizard.WelcomeWizardPage InputInfoPage;
        private DevExpress.XtraEditors.CheckEdit chkToXML;
        private DevExpress.XtraEditors.CheckEdit chkToDatabase;
        private DevExpress.XtraEditors.PanelControl pnlXML;
        private DevExpress.XtraEditors.ButtonEdit btnXMLFile;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.PanelControl pnlDatabase;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtViewName;
        private DevExpress.XtraWizard.WizardPage wizardPage1;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.LookUpEdit lkeGroup;




    }
}