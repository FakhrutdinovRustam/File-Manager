using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_01_total
{
    public partial class Form3 : Form
    {
        short formIndex; // 0 la overwrite, 1 là overwriteall, 2 là skip, 3 la skipall
        string existItem = "";
        public Form3(string path)
        {
            InitializeComponent();
            existItem = path;
        }

        private void label1_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formIndex = 0;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formIndex = 1;
            this.Close();
        }
        public short applyOrder()
        {
            return formIndex;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = existItem + " is already exists!!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formIndex = 2;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            formIndex = 3;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            formIndex = 4;
            this.Close();
        }
    }
}
