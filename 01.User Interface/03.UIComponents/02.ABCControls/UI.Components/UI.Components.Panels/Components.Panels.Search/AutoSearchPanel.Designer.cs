namespace ABCControls
{
    partial class ABCAutoSearchPanel
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
            this.btnSearch=new DevExpress.XtraEditors.SimpleButton();
            this.flowLayoutPanel1=new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.btnSearch.Anchor=( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnSearch.Location=new System.Drawing.Point( 310 , 84 );
            this.btnSearch.Name="simpleButton1";
            this.btnSearch.Size=new System.Drawing.Size( 76 , 23 );
            this.btnSearch.TabIndex=0;
            this.btnSearch.Text="Tìm kiếm";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location=new System.Drawing.Point( 0 , 0 );
            this.flowLayoutPanel1.Name="flowLayoutPanel1";
            this.flowLayoutPanel1.Size=new System.Drawing.Size( 389 , 110 );
            this.flowLayoutPanel1.TabIndex=1;
            // 
            // SearchPanel
            // 
            this.Controls.Add( this.btnSearch );
            this.Controls.Add( this.flowLayoutPanel1 );
            this.Name="SearchPanel";
            this.Size=new System.Drawing.Size( 389 , 110 );
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

    }
}
