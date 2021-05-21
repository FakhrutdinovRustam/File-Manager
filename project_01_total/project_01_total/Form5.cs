using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;

namespace project_01_total
{
	public partial class ArchiveForm : Form
	{
		private string text;
		private ZipFile zip;
		private TextBox mainTextBox;
		private ListView listView;
		public ArchiveForm(ZipFile zip, TextBox textBox, ListView listView)
		{
			this.zip = zip;
			this.mainTextBox = textBox;
			this.listView = listView;
			InitializeComponent();
		}
		

		private void button1_Click(object sender, EventArgs e)
		{
			text = textBox1.Text;
			this.zip.Save(mainTextBox.Text + text + ".zip");
			this.Close();
		}
	}
}

