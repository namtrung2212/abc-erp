namespace ABCControls
{
    partial class ABCCheckPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.panelControl=new DevExpress.XtraEditors.PanelControl();
            this.checkEdit=new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton1=new DevExpress.XtraEditors.SimpleButton();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl ) ).BeginInit();
            this.panelControl.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.checkEdit.Properties ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.BorderStyle=DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add( this.simpleButton1 );
            this.panelControl.Dock=System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location=new System.Drawing.Point( 0 , 19 );
            this.panelControl.Name="panelControl";
            this.panelControl.Size=new System.Drawing.Size( 313 , 129 );
            this.panelControl.TabIndex=0;
            // 
            // checkEdit
            // 
            this.checkEdit.Dock=System.Windows.Forms.DockStyle.Top;
            this.checkEdit.Location=new System.Drawing.Point( 0 , 0 );
            this.checkEdit.Name="checkEdit";
            this.checkEdit.Properties.Caption="checkEdit1";
            this.checkEdit.Size=new System.Drawing.Size( 313 , 19 );
            this.checkEdit.TabIndex=1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location=new System.Drawing.Point( 41 , 36 );
            this.simpleButton1.Name="simpleButton1";
            this.simpleButton1.Size=new System.Drawing.Size( 75 , 23 );
            this.simpleButton1.TabIndex=0;
            this.simpleButton1.Text="simpleButton1";
            // 
            // ABCCheckPanel
            // 
            this.Controls.Add( this.panelControl );
            this.Controls.Add( this.checkEdit );
            this.Name="ABCCheckPanel";
            this.Size=new System.Drawing.Size( 313 , 148 );
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl ) ).EndInit();
            this.panelControl.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.checkEdit.Properties ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl;
        private DevExpress.XtraEditors.CheckEdit checkEdit;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}
