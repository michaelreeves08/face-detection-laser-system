using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Newtonsoft.Json;
using System.IO;
using System.Media;

namespace Emgu_4._0
{
	/// <summary>
	/// Displays original and processed images, preforms position calculations, sends data to microcontroller
	/// </summary>
	public partial class Form1 : Form
	{
		Capture capture;
		

		List<PointF> positions = new List<PointF>();
		PointF averagedPoint;

		private Stopwatch serialTimoutwatch = new Stopwatch();
		private Stopwatch soundWatch = new Stopwatch();
		Image<Bgr, Byte> originalImage;

		private int detections;
		bool isCapturing = false;

		private float xDivConst; //Scaling factors for range of vision applied to servo ranges
		private float yDivConst;

		private SoundPlayer player = new SoundPlayer(); //For portal turret noises
		private Random random = new Random(); //For picking random noise

		private Settings settings; //Generic settings class to be instantiated by deserializing a Json


		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			initializeJson();
			applySettings();
			serialPort1.Open();
			
			resetPos();

			serialTimoutwatch = Stopwatch.StartNew();
			soundWatch = Stopwatch.StartNew();


			try
			{
				capture = new Emgu.CV.Capture();  //Start Capture object that takes input from default camera
				calculateDivisionConstants(); 

				Application.Idle += processFrameAndUpdateImg; //Alternative to threading or running a continuous loop, adds this method to list of functions that the application calls constantly

				isCapturing = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				this.Close();
			}

			initializeVoiceRecognition();
			playSituationalSound(soundType.Startup); //Portal startup sound


		}


		/// <summary>
		/// Get stored settings and callibrations from a Json, deserialize into Settings object
		/// </summary>
		private void initializeJson()
		{
			try
			{
				settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Properties.Resources.settingsFileName));
			}
			//If no file is found, make a new one with default settings
			catch(FileNotFoundException e)
			{
				MessageBox.Show("No memory file detected, generating one");
				settings = new Settings();
				saveSettings();

			}
			//If for whatever reason the Json is corrupt
			catch (JsonReaderException e)
			{
				MessageBox.Show("Corrupt Memory File");
				File.Delete(Properties.Resources.settingsFileName);
				this.Close();
			}
		}

		/// <summary>
		/// Apply settings using information from Settings object
		/// </summary>
		private void applySettings()
		{
			this.numericUpDown1.Value = settings.xShift;
			this.numericUpDown2.Value = settings.yShift; 
			this.checkBox1.Checked = settings.sendSerial;
			this.checkBox2.Checked = settings.soundsOn;
			this.radioButton1.Checked = settings.faceRecogOn;
			this.radioButton2.Checked = settings.figureRecogOn;
		}

		/// <summary>
		/// Center servos
		/// </summary>
		private void resetPos()
		{
			serialPort1.Write("X" + (90 + settings.xShift).ToString() + ":Y" + (90 + settings.yShift).ToString());
		}

		/// <summary>
		/// Used to scale servo movements to camera's field of view
		/// </summary>
		private void calculateDivisionConstants()
		{
			xDivConst = (capture.QueryFrame().Width) / (settings.xRightCalibration - settings.xLeftCalibration);
			yDivConst = (capture.QueryFrame().Height) / (settings.yTopCalibration - settings.yBotCalibration);
		}

		/// <summary>
		/// Constantly called by application, displays original and processed frames
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="arg"></param>
		private void processFrameAndUpdateImg(object sender, EventArgs arg)
		{

			originalImage = capture.QueryFrame();


			imageBox1.Image = originalImage;

			//These 2 conditions never conflict because both bools are controlled by radio buttons
			if (settings.faceRecogOn)
			{
				imageBox2.Image = Person_Detector.detectFace(originalImage, out detections, out positions);
			}
			else if (settings.figureRecogOn)
			{
				imageBox2.Image = Person_Detector.findPerson(originalImage, out detections, out positions);
			}


			averagedPoint = averagePoints(positions);

			
			if (positions.Count > 0)
			{
				//Only proceed to further processing if setting permit and we havn't sent a message in the last 15ms (arduino gets confuzzled)
				if (checkBox1.Checked && serialTimoutwatch.ElapsedMilliseconds > 15)
				{
					serialTimoutwatch = Stopwatch.StartNew();
					calculatePoint(averagedPoint);
					playSituationalSound(soundType.Detection); //Play portal detection sound
				}
			}
			//If there are no detections returned, play turret loose sight portal sound
			else
			{
				playSituationalSound(soundType.Missing);
			}

		}

		/// <summary>
		/// Average all detection positions returned by the image recognition class
		/// </summary>
		/// <param name="pointList"></param>
		/// <returns></returns>
		private PointF averagePoints(List<PointF> pointList)
		{
			PointF targetPoint = new PointF(0, 0);

			foreach (PointF point in pointList)
			{
				targetPoint.X += point.X;
				targetPoint.Y += point.Y;
			}

			targetPoint.X /= pointList.Count;
			targetPoint.Y /= pointList.Count;
			return targetPoint;
		}

		/// <summary>
		/// Sends signal to the arduino in proper data format (the sitty one I made) and print position to screen
		/// </summary>
		/// <param name="currentPoint"></param>
		private void sendSignal(Point currentPoint)
		{
			
			if(settings.sendSerial && !restFlag)serialPort1.Write("X" + (currentPoint.X + settings.xShift).ToString() + ":Y" + (currentPoint.Y + settings.yShift).ToString()); //Data stream format
				
			textBox1.AppendText("Auto: X" + (currentPoint.X + settings.xShift).ToString() + "    Y" + (currentPoint.Y + settings.yShift).ToString() + Environment.NewLine); //Display detection position
			
		}

		/// <summary>
		/// Calculates the point that will be sent to arduino, factors in scaling constants and settings adjustments
		/// </summary>
		/// <param name="point"></param>
		private void calculatePoint(PointF point)
		{
			if (withinSafeRange(point))
			{
				sendSignal(new Point(((int)(settings.xRightCalibration - (point.X / xDivConst))), ((int)(settings.yTopCalibration - (point.Y / yDivConst)))));
			}

		}

		/// <summary>
		/// Assures that point is not in a position where maving to it could damage hardware, only ever happens if calibration is done wrong
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		private bool withinSafeRange(PointF point)
		{
			if ((settings.yTopCalibration - (point.Y / yDivConst)) + ((float)numericUpDown2.Value) > 65 && (settings.yTopCalibration - (point.Y / yDivConst)) + ((float)numericUpDown2.Value) < 138)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// Takes enum dependant on situation, randomly selects a sound from a set of organized directories
		/// </summary>
		/// <param name="soundType"></param>
		private void playSituationalSound(soundType soundType)
		{
			if (settings.soundsOn)
				if (soundWatch.ElapsedMilliseconds > random.Next(5000,8000) || soundType == soundType.Startup || soundType == soundType.Shutdown || soundType == soundType.Saddness)  //Exceptions for startup and shutdown so they dont have to wait the 5 seconds
			{
				switch (soundType)
				{
					case soundType.Detection:
						player.SoundLocation = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\detection")[random.Next(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\detection").Length)]; // I'm sorry
						break;

					case soundType.Startup:
						player.SoundLocation = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\startup")[random.Next(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\startup").Length)];
						break;

					case soundType.Shutdown:
						player.SoundLocation = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\shutdown")[random.Next(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\shutdown").Length)];
						break;

					case soundType.Missing:
							
						player.SoundLocation = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\noDetection")[random.Next(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\noDetection").Length)];
						break;

					case soundType.Saddness:
							player.SoundLocation = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\saddness")[random.Next(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Sound\saddness").Length)];
						break;


				}
				
				//Need to start the sound in a new thread to that it doesn't stall the main thread and so that souns don't overlap
				Thread thread = new Thread(new ThreadStart(playSound));
				thread.Start();

				soundWatch = Stopwatch.StartNew();

			}

			

		}

		private void playSound()
		{
			player.PlaySync();
		}


		private void laserDown()
		{
			restFlag = true;

			for (int i = 0; i < 10; i++)
			{
				serialPort1.Write("X90:Y50");
				textBox1.AppendText("From Rest\n");
				Thread.Sleep(15);
			}
			playSituationalSound(soundType.Saddness);
			Thread.Sleep(4000);
			restFlag = false;

		}

		/// <summary>
		/// Pauses image processing by removing processing method from Application's idle list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			flipPause();
		}

		private void flipPause()
		{
			if (isCapturing)
			{
				Application.Idle -= processFrameAndUpdateImg;
				isCapturing = false;
				pauseButton.Text = "Resume";
				resetPos();
			}
			else if (!isCapturing)
			{
				Application.Idle += processFrameAndUpdateImg;
				isCapturing = true;
				pauseButton.Text = "Pause";
			}
		}


		
		/// <summary>
		/// Plays shutdown sound, and resets the servos upon shutdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			playSituationalSound(soundType.Shutdown);

			Application.Idle -= processFrameAndUpdateImg;
			resetPos();
			serialPort1.Close();
			capture.Dispose();
			//this.ParentForm.Close();  //Some of the emgu dlls dont like being instantiated twice at runtime, so the entire program needs to shutdown after running this form

		}

		private void button2_Click(object sender, EventArgs e)
		{
			resetPos();
		}
		
		//Calls saveSettings and plays alert
		private void button3_Click(object sender, EventArgs e)
		{
			saveSettings();
			System.Media.SystemSounds.Asterisk.Play();

		}

		/// <summary>
		/// Updates settings object with the state of current controls
		/// </summary>
		private void saveSettings()
		{
			settings.xShift = (int)numericUpDown1.Value;
			settings.yShift = (int)numericUpDown2.Value;
			settings.sendSerial = checkBox1.Checked;
			settings.faceRecogOn = radioButton1.Checked;
			settings.figureRecogOn = radioButton2.Checked;
			settings.soundsOn = checkBox2.Checked;

			saveSettingsToFile();
		}

		private void saveSettingsToFile()
		{
			File.WriteAllText(Properties.Resources.settingsFileName, JsonConvert.SerializeObject(settings));
		}
	}

	/// <summary>
	/// Settings class, constructor initializes value with defaults
	/// </summary>
	public class Settings
	{
		public int xShift { get; set; }
		public int yShift { get; set; }
		public int xLeftCalibration { get; set; }
		public int xRightCalibration { get; set; }
		public int yTopCalibration { get; set; }
		public int yBotCalibration { get; set; }
		public bool sendSerial { get; set; }
		public bool faceRecogOn { get; set; }
		public bool figureRecogOn { get; set; }
		public bool soundsOn { get; set; }

		public Settings()
		{
			xShift = 0;
			yShift = 0;
			xLeftCalibration = 59;
			xRightCalibration = 118;
			yTopCalibration = 110;
			yBotCalibration = 70;
			faceRecogOn = true;
			figureRecogOn = false;
			soundsOn = true;
			sendSerial = true;
		}

	}
}
