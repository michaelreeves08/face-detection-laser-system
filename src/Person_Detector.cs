using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;


namespace Emgu_4._0
{

	public static class Person_Detector
	{
		private static Rectangle[] regeions;
		private static Rectangle[] faces;
		private static Image<Bgr, byte> imageToProcess;
		private static List<PointF> positions = new List<PointF>();
		private static List<PointF> facePositions = new List<PointF>();
		private static GpuCascadeClassifier face = new GpuCascadeClassifier(@"haarcascade_frontalface_default.xml");




		public static Image<Bgr, Byte> findPerson(Image<Bgr, Byte> image, out int detections, out List<PointF> positions)
		{
			//If the gpu has nvidia CUDA
			if (GpuInvoke.HasCuda)
			{
				imageToProcess = new Image<Bgr, byte>(image.Bitmap);    //Value is coppied so the refference of the input image is not changed outside of this class
				Person_Detector.positions.Clear();
				using(GpuHOGDescriptor descriptor = new GpuHOGDescriptor())
				{
					descriptor.SetSVMDetector(GpuHOGDescriptor.GetDefaultPeopleDetector());  

					using(GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(imageToProcess)) //Create gpuImage from image
					{
						using (GpuImage<Bgra, Byte> bgraImage = gpuImage.Convert<Bgra, Byte>())
						{
							regeions = descriptor.DetectMultiScale(bgraImage); //Returns all detected regions in a rectangle array
							
						}

					}

				}
			}
			else
			{
				using (HOGDescriptor des = new HOGDescriptor())
				{
					regeions = des.DetectMultiScale(imageToProcess);
				}
			}


			detections = regeions.Length;
			
			//Draws detected rectangles onto the image being returned
			foreach(Rectangle ped in regeions)
			{
				imageToProcess.Draw(ped, new Bgr(Color.Red), 5);
				imageToProcess.Draw(new Cross2DF(new PointF(ped.Location.X + (ped.Width / 2), ped.Location.Y + (ped.Height / 2)), 30, 30), new Bgr(Color.Green), 3);
				Person_Detector.positions.Add(new PointF(ped.Location.X + (ped.Width / 2), ped.Location.Y + (ped.Height / 2))); //Sets the putput variable
			}


			positions = Person_Detector.positions;
			return imageToProcess;
		}


		public static Image<Bgr, byte> detectFace(Image<Bgr, byte> image, out int detections, out List<PointF> positions)
		{
			Image<Bgr, byte> copyImage = new Image<Bgr, byte>(image.Bitmap); //Copy the image into a new one
			 
			Person_Detector.facePositions.Clear();

			
						using (GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(image))
						using (GpuImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())  //The cascade classifier only takes gray for some reason
						{  
							faces = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);

							//Draws rectanges to the face detection positions
							foreach (Rectangle f in faces)
							{

								copyImage.Draw(f, new Bgr(Color.Red), 4);
								Person_Detector.facePositions.Add(new PointF(f.Location.X + (f.Width / 2), f.Location.Y + (f.Height / 2)));

							}
						detections = faces.Length;
						positions = facePositions;
						}
					





			return  copyImage;
		}


	}
}
