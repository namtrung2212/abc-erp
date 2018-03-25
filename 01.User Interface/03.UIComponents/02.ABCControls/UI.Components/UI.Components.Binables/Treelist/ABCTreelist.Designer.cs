namespace ABCControls
{
    partial class ABCTreeList
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
            this.InnerTreeList=new DevExpress.XtraTreeList.TreeList();
            ( (System.ComponentModel.ISupportInitialize)( this.InnerTreeList ) ).BeginInit();
            this.SuspendLayout();
            // 
            // TreeListCtrl
            // 
            this.InnerTreeList.Dock=System.Windows.Forms.DockStyle.Fill;
            this.InnerTreeList.Location=new System.Drawing.Point( 0 , 0 );
            this.InnerTreeList.Name="TreeListCtrl";
            this.InnerTreeList.Size=new System.Drawing.Size( 293 , 172 );
            this.InnerTreeList.TabIndex=0;

            // 
            // DefaultView
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.InnerTreeList );
            this.Name="DefaultView";
            this.Size=new System.Drawing.Size( 293 , 172 );
            ( (System.ComponentModel.ISupportInitialize)( this.InnerTreeList ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        public DevExpress.XtraTreeList.TreeList InnerTreeList;
    }
}
