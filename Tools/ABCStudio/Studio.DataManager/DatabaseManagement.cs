using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ABCStudio
{
    public partial class DatabaseManagement : DevExpress.XtraEditors.XtraForm
    {
        public enum ActiveTab
        {
            DataTable,
            StoredProcedure,
            TableConfig,
            FieldConfig,
            EnumDefine
        }
      
        public Studio OwnerStudio;

        public DatabaseManagement (ActiveTab activeTab, Studio studio )
        {
            OwnerStudio=studio;

            InitializeComponent();

            this.Load+=new EventHandler( DatabaseManagement_Load );

        }

        void DatabaseManagement_Load ( object sender , EventArgs e )
        {

            TableDesignScreen form=new TableDesignScreen( OwnerStudio );
            form.Parent=DataTablePanel;
            form.Dock=DockStyle.Fill;
            form.InitEvents();

            //StoredProConfigScreen form1=new StoredProConfigScreen();
            //form1.Parent=SPPanel;
            //form1.Dock=DockStyle.Fill;

            //TableConfigScreen form2=new TableConfigScreen();
            //form2.Parent=TableALiasPanel;
            //form2.Dock=DockStyle.Fill;

            //FieldConfigScreen form3=new FieldConfigScreen(OwnerStudio);
            //form3.Parent=ColumnAliasPanel;
            //form3.Dock=DockStyle.Fill;

            EnumDefineScreen form4=new EnumDefineScreen();
            form4.Parent=EnumDefinePanel;
            form4.Dock=DockStyle.Fill;

            DictionaryDefineScreen form5=new DictionaryDefineScreen();
            form5.Parent=DictionaryPanel;
            form5.Dock=DockStyle.Fill;
        }
    }
}