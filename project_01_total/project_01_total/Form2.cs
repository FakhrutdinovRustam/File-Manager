using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_01_total
{
    public partial class Form2 : Form
    {
        string strFrm = "";
        public Form2(string str, string name)
        {
            InitializeComponent();
            strFrm = str;
            this.Text = name;
        }
        public Form2(string str)
        {
            InitializeComponent();
            strFrm = str;
           
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = strFrm;
            
        }
    }
}
