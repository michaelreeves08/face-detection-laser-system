using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emgu_4._0
{
	public partial class OptionForm : Form
	{
		Form1 cam;
		Calibration cal;

		public OptionForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(cal != null)
			{
				cal.Close();
			}
			cam = new Form1();
			cam.Show();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if(cam != null)
			{
				cam.Close();
			}
			cal = new Calibration();
			cal.Show();

		}
	}
}
