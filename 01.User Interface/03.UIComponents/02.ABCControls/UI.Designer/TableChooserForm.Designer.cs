namespace ABCControls
{
    partial class TableChooserForm
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( TableChooserForm ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.chkAllTables=new DevExpress.XtraEditors.CheckEdit();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnNext=new DevExpress.XtraEditors.SimpleButton();
            this.TableListCtrl=new DevExpress.XtraEditors.CheckedListBoxControl();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.chkAllTables.Properties ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.TableListCtrl ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.chkAllTables );
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnNext );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 243 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 365 , 33 );
            this.panelControl1.TabIndex=1;
            // 
            // chkAllTables
            // 
            this.chkAllTables.Location=new System.Drawing.Point( 12 , 7 );
            this.chkAllTables.Name="chkAllTables";
            this.chkAllTables.Properties.Caption="All Tables";
            this.chkAllTables.Size=new System.Drawing.Size( 75 , 19 );
            this.chkAllTables.TabIndex=2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 286 , 3 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=1;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnNext
            // 
            this.btnNext.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnNext.Image=( (System.Drawing.Image)( resources.GetObject( "btnNext.Image" ) ) );
            this.btnNext.Location=new System.Drawing.Point( 208 , 3 );
            this.btnNext.Name="btnNext";
            this.btnNext.Size=new System.Drawing.Size( 75 , 26 );
            this.btnNext.TabIndex=0;
            this.btnNext.Text="&Next";
            this.btnNext.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // TableListCtrl
            // 
            this.TableListCtrl.CheckOnClick=true;
            this.TableListCtrl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.TableListCtrl.Location=new System.Drawing.Point( 0 , 0 );
            this.TableListCtrl.Margin=new System.Windows.Forms.Padding( 10 );
            this.TableListCtrl.MultiColumn=true;
            this.TableListCtrl.Name="TableListCtrl";
            this.TableListCtrl.SelectionMode=System.Windows.Forms.SelectionMode.MultiExtended;
            this.TableListCtrl.Size=new System.Drawing.Size( 365 , 243 );
            this.TableListCtrl.TabIndex=2;
            // 
            // TableChooserForm
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size( 365 , 276 );
            this.Controls.Add( this.TableListCtrl );
            this.Controls.Add( this.panelControl1 );
            this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name="TableChooserForm";
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text="Choose TableName from ListBox below";
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.chkAllTables.Properties ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.TableListCtrl ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private DevExpress.XtraEditors.CheckedListBoxControl TableListCtrl;
        private DevExpress.XtraEditors.CheckEdit chkAllTables;
    }
}