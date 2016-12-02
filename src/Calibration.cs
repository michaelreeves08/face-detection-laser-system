using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Newtonsoft.Json;
using System.IO;
using System.Timers;

namespace Emgu_4._0
{

	/// <summary>
	/// Used to callibrate the outermoust positions of the screen to be used when calculating servo positions
	/// </summary>
	public partial class Calibration : Form
	{
		private buttonState buttonState = new buttonState();
		private System.Timers.Timer buttonHoldtimer;
		private Capture capture;
		private Settings settings;
		private Point virtualPoint = new Point(90, 90);

		public Calibration()
		{
			InitializeComponent();
			initializePerepherals();
			initializeSettings();
			initializeTimer();
			resetPos();
			
			Application.Idle += displayImage;
			MessageBox.Show("Use the arrows to adjust the laser to the outermost part of the screen");

		}

		private void initializeTimer()
		{
			buttonHoldtimer = new System.Timers.Timer();
			buttonHoldtimer.Interval = 1;
			buttonHoldtimer.Enabled = false;
			buttonHoldtimer.Elapsed += timerEllapsed;
			buttonHoldtimer.AutoReset = true;

		}

		/// <summary>
		/// Winforms has no event for buttonhold, this is a makeshift one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerEllapsed(object sender, ElapsedEventArgs e)
		{
			switch (buttonState)
			{
				case buttonState.Right:
					virtualPoint.X--;
					break;

				case buttonState.Up:
					virtualPoint.Y++;
					break;

				case buttonState.Left:
					virtualPoint.X++;
					break;

				case buttonState.Down:
					virtualPoint.Y--;
					break;
			}

			sendSerial();

			buttonHoldtimer.Interval = 100;
		}

		private void displayImage(object sender, EventArgs arg)
		{
			imageBox1.Image = capture.QueryFrame();


		}

		private void initializeSettings()
		{

			try
			{
				settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Properties.Resources.settingsFileName));
			}
			catch (FileNotFoundException e)
			{
				MessageBox.Show("No memory file detected, generating one");
				settings = new Settings();
				saveSettings();

			}
			catch (JsonReaderException e)
			{
				MessageBox.Show("Corrupt Memory File");
				File.Delete(Properties.Resources.settingsFileName);
				this.Close();
			}
		}

		private void saveSettings()
		{
			File.WriteAllText(Properties.Resources.settingsFileName, JsonConvert.SerializeObject(settings));

		}
		private void resetPos()
		{
			serialPort1.Write("X90:Y90");
		}

		private void initializePerepherals()
		{
			try
			{
				capture = new Capture();
			}
			catch (Exception e)
			{
				MessageBox.Show("Cannot initialize camera");
				this.Close();
			}

			try
			{
				serialPort1.Open();
			}
			catch (Exception e)
			{
				MessageBox.Show("Cannot initialize on COM port");
				this.Close();
			}
		}

		private void sendSerial()
		{
			if (virtualPoint.X < 180 && virtualPoint.X > 0 && virtualPoint.Y < 180 && virtualPoint.Y > 0)
			{
				serialPort1.Write("X" + (virtualPoint.X).ToString() + ":Y" + (virtualPoint.Y).ToString()); //Data stream format
			}

		}




		private void Calibration_FormClosing(object sender, FormClosingEventArgs e)
		{
			capture.Dispose();
			resetPos();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Calibrate " + radDropDownList1.Text + "?", "", MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
			{
				if (radDropDownList1.SelectedIndex != -1)
				{
					if (radDropDownList1.SelectedIndex == 0)
					{
						settings.xLeftCalibration = virtualPoint.X;
					}
					else if (radDropDownList1.SelectedIndex == 1)
					{
						settings.xRightCalibration = virtualPoint.X;
					}
					else if (radDropDownList1.SelectedIndex == 2)
					{
						settings.yTopCalibration = virtualPoint.Y;
					}
					else if (radDropDownList1.SelectedIndex == 3)
					{
						settings.yBotCalibration = virtualPoint.Y;
					}

					saveToFile();
				}
			}

		}

		private void saveToFile()
		{
			File.WriteAllText(Properties.Resources.settingsFileName, JsonConvert.SerializeObject(settings));

		}

		private void mouseDown(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender;

			if (button.Equals(upButton))
			{
				buttonState = buttonState.Up;
			}
			else if (button.Equals(rightButton))
			{
				buttonState = buttonState.Right;
			}
			else if (button.Equals(downButton))
			{
				buttonState = buttonState.Down;
			}
			else if (button.Equals(leftButton))
			{
				buttonState = buttonState.Left;
			}


			buttonHoldtimer.Enabled = true;
		}

		private void mouseUp(object sender, MouseEventArgs e)
		{
			buttonHoldtimer.Enabled = false;
		}
	}

	public enum buttonState
	{
		Up, Down, Right, Left	
	}

	public enum soundType
	{
		Detection, Missing, Startup, Shutdown, Saddness
	}
}
