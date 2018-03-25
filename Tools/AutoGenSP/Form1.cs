using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoGenSP
{
    public partial class Form1 : Form
    {
        StoredProConfigScreen control;
        public Form1 ( )
        {
            InitializeComponent();
             control=new StoredProConfigScreen();
             control.Dock=DockStyle.Fill;
             control.Parent=this;

        }
    }
}
