namespace ABCStudio
{
    partial class DictionaryDefineScreen
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
            System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager( typeof( DictionaryDefineScreen ) );
            this.panelControl1=new DevExpress.XtraEditors.PanelControl();
            this.btnCancel=new DevExpress.XtraEditors.SimpleButton();
            this.btnSave=new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1=new DevExpress.XtraGrid.GridControl();
            this.gridView1=new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColKey=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColTranslateVN=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColTranslateEN=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColIsContain=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColIsStartWith=new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColIsEndWith=new DevExpress.XtraGrid.Columns.GridColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).BeginInit();
            this.panelControl1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add( this.btnCancel );
            this.panelControl1.Controls.Add( this.btnSave );
            this.panelControl1.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location=new System.Drawing.Point( 0 , 281 );
            this.panelControl1.Name="panelControl1";
            this.panelControl1.Size=new System.Drawing.Size( 557 , 33 );
            this.panelControl1.TabIndex=3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.Image=( (System.Drawing.Image)( resources.GetObject( "btnCancel.Image" ) ) );
            this.btnCancel.Location=new System.Drawing.Point( 477 , 3 );
            this.btnCancel.Name="btnCancel";
            this.btnCancel.Size=new System.Drawing.Size( 75 , 26 );
            this.btnCancel.TabIndex=3;
            this.btnCancel.Text="&Cancel";
            this.btnCancel.Click+=new System.EventHandler( this.btnCancel_Click );
            // 
            // btnSave
            // 
            this.btnSave.Anchor=( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom )
                        |System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnSave.Image=( (System.Drawing.Image)( resources.GetObject( "btnSave.Image" ) ) );
            this.btnSave.Location=new System.Drawing.Point( 399 , 3 );
            this.btnSave.Name="btnSave";
            this.btnSave.Size=new System.Drawing.Size( 75 , 26 );
            this.btnSave.TabIndex=2;
            this.btnSave.Text="&Save";
            this.btnSave.Click+=new System.EventHandler( this.btnSave_Click );
            // 
            // gridControl1
            // 
            this.gridControl1.Dock=System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location=new System.Drawing.Point( 0 , 0 );
            this.gridControl1.MainView=this.gridView1;
            this.gridControl1.Name="gridControl1";
            this.gridControl1.Size=new System.Drawing.Size( 557 , 281 );
            this.gridControl1.TabIndex=4;
            this.gridControl1.ViewCollection.AddRange( new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1} );
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange( new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColKey,
            this.gridColTranslateVN,
            this.gridColTranslateEN,
            this.gridColIsContain,
            this.gridColIsStartWith,
            this.gridColIsEndWith} );
            this.gridView1.GridControl=this.gridControl1;
            this.gridView1.Name="gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows=DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AllowDeleteRows=DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.NewItemRowPosition=DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView1.OptionsView.ShowGroupPanel=false;
            this.gridView1.OptionsView.ShowViewCaption=true;
            this.gridView1.ViewCaption="Dictionary Definition";
            // 
            // gridColKey
            // 
            this.gridColKey.Caption="Key String";
            this.gridColKey.FieldName="KeyString";
            this.gridColKey.Name="gridColKey";
            this.gridColKey.Visible=true;
            this.gridColKey.VisibleIndex=0;
            // 
            // gridColTranslateVN
            // 
            this.gridColTranslateVN.Caption="Translation (VN)";
            this.gridColTranslateVN.FieldName="TranslateVN";
            this.gridColTranslateVN.Name="gridColTranslateVN";
            this.gridColTranslateVN.Visible=true;
            this.gridColTranslateVN.VisibleIndex=1;
            // 
            // gridColTranslateEN
            // 
            this.gridColTranslateEN.Caption="Translation (EN)";
            this.gridColTranslateEN.FieldName="TranslateEN";
            this.gridColTranslateEN.Name="gridColTranslateEN";
            this.gridColTranslateEN.Visible=true;
            this.gridColTranslateEN.VisibleIndex=2;
            // 
            // gridColIsContain
            // 
            this.gridColIsContain.Caption="Contain";
            this.gridColIsContain.FieldName="IsContain";
            this.gridColIsContain.Name="gridColIsContain";
            this.gridColIsContain.Visible=true;
            this.gridColIsContain.VisibleIndex=3;  
            // 
            // gridColIsStartWith
            // 
            this.gridColIsStartWith.Caption="StartWith";
            this.gridColIsStartWith.FieldName="IsStartWith";
            this.gridColIsStartWith.Name="gridColIsStartWith";
            this.gridColIsStartWith.Visible=true;
            this.gridColIsStartWith.VisibleIndex=4;  
            // 
            // gridColIsEndWith
            // 
            this.gridColIsEndWith.Caption="EndWith";
            this.gridColIsEndWith.FieldName="IsEndWith";
            this.gridColIsEndWith.Name="gridColIsEndWith";
            this.gridColIsEndWith.Visible=true;
            this.gridColIsEndWith.VisibleIndex=5;
            // 
            // DictionaryDefine
            // 
            this.AutoScaleDimensions=new System.Drawing.SizeF( 6F , 13F );
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.gridControl1 );
            this.Controls.Add( this.panelControl1 );
            this.Name="DictionaryDefine";
            this.Size=new System.Drawing.Size( 557 , 314 );
            ( (System.ComponentModel.ISupportInitialize)( this.panelControl1 ) ).EndInit();
            this.panelControl1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.gridControl1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.gridView1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTranslateVN;
        private DevExpress.XtraGrid.Columns.GridColumn gridColTranslateEN;
        private DevExpress.XtraGrid.Columns.GridColumn gridColIsContain;
        private DevExpress.XtraGrid.Columns.GridColumn gridColIsStartWith;
        private DevExpress.XtraGrid.Columns.GridColumn gridColIsEndWith;
    }
}
