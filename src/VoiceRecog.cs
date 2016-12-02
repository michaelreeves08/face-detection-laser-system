using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace Emgu_4._0
{
	public partial class Form1 : Form
	{
		private SpeechRecognitionEngine recog = new SpeechRecognitionEngine();
		private Choices choices = new Choices();
		private GrammarBuilder builder = new GrammarBuilder();
		private Grammar grammar;
		private string[] commands = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "command x axis add", "command y axis add", "command x axis subtract", "command y axis subtract", "command clear shift string", "command execute shutdown", "command shut the fuck up", "command flip tracking", "command pause", "command detect face", "command detect body", "command fuck off"};
		
		private string shiftString = "";
		private bool restFlag = false;

		private void initializeVoiceRecognition()
		{
			

			choices.Add(commands);
			builder.Append(choices);
			grammar = new Grammar(builder);
			

			recog.LoadGrammar(grammar);
			recog.SetInputToDefaultAudioDevice();
			recog.RecognizeAsync(RecognizeMode.Multiple);
			recog.SpeechRecognized += recogCommand;

		}

		private void recogCommand(object sender, SpeechRecognizedEventArgs e)
		{
			if( !e.Result.Text.StartsWith("command"))
			{
				shiftString += e.Result.Text;

			}
			else
			{
				try
				{
					switch (e.Result.Text)
					{
						//case "command x axis add":
						//	settings.xShift += Int32.Parse(shiftString);
						//	break;

						//case "command y axis add":
						//	settings.yShift += Int32.Parse(shiftString);

						//	break;

						//case "command x axis subtract":
						//	settings.xShift -= Int32.Parse(shiftString);

						//	break;

						//case "command y axis subtract":
						//	settings.yShift -= Int32.Parse(shiftString);

						//	break;

						//case "command clear shift string":
						//	shiftString = "";
						//	break;

						case "command shut the fuck up":
							settings.soundsOn = !settings.soundsOn;
							break;

						case "command flip tracking":
							settings.sendSerial = !settings.sendSerial;
							break;

						case "command pause":
							flipPause();
							break;

						case "command detect face":
							settings.figureRecogOn = false;
							settings.faceRecogOn = true;
							break;

						case "command detect body":
							settings.faceRecogOn = false;
							settings.figureRecogOn = true;
							break;

						case "command fuck off":

							laserDown();

							break;

						case "command execute shutdown":
							resetPos();
							this.Close();
							break;

					}


					saveSettingsToFile();
					applySettings();
					shiftString = "";
				}
				catch(FormatException ex)
				{
					MessageBox.Show("Voice recognition error " + e.Result.Text + " " + shiftString.ToString());
					shiftString = "";

				}



			}
		}

	}

	

}
