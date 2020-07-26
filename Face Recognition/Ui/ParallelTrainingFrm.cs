using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

using System.IO;
using System.Drawing.Imaging;
using System.Xml;
using System.Threading;
using SMARTY.Trainer;
using System.Text.RegularExpressions;
using System.Diagnostics;
using DbSystem.Struct;
using System.Threading.Tasks;
using SMARTY.Properties;
using DbSystem.Controller;
using DbSystem;

namespace SMARTY
{
    public partial class ParallelTrainingFrm : Form 
    {
        Core DbCore = new DbSystem.Core();
        Inserter DbIn; Reader DbOut;
        Image<Bgr, Byte> currentFrame;
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray_frame = null;

        //Classifier : Haar-Like Face Detector
        CascadeClassifier Face;
        List<Image<Gray, byte>> resultImages = new List<Image<Gray, byte>>();

        //Current Player
        player CurrentPlayer;

        //Trainer
        
        //Saving Jpg
        List<Image<Gray, byte>> ImagestoWrite = new List<Image<Gray, byte>>();
        EncoderParameters ENC_Parameters = new EncoderParameters(1);
        EncoderParameter ENC = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
        ImageCodecInfo Image_Encoder_JPG;

        //Saving XAML Data file
        List<string> NamestoWrite = new List<string>();
        List<string> NamesforFile = new List<string>();
        XmlDocument docu = new XmlDocument();
        public ParallelTrainingFrm()
        {
            InitializeComponent();

            //Declaring Database System 
            AppDomain.CurrentDomain.SetData
                ("DataDirectory",Directory.GetCurrentDirectory());

            DbCore.Connect(Resources.Constring);
            DbIn = new Inserter(DbCore.Con);
            DbOut = new Reader(DbCore.Con);

            Face = new CascadeClassifier(Variables.CascadeFile);
            ENC_Parameters.Param[0] = ENC;
            Image_Encoder_JPG = GetEncoder(ImageFormat.Jpeg);
        }
        private bool save_training_data(Image face_data,string name)
        {
            try
            {
                Random rand = new Random();
                bool file_create = true;
                string facename = "face_" + name + "_" + rand.Next().ToString() + ".jpg";
                while (file_create)
                {

                    if (!File.Exists(Application.StartupPath + "/TrainedFaces/" + facename))
                    {
                        file_create = false;
                    }
                    else
                    {
                        facename = "face_" + name + "_" + rand.Next().ToString() + ".jpg";
                    }
                }


                if (Directory.Exists(Application.StartupPath + "/TrainedFaces/"))
                {try
                    {
                        face_data.Save(Application.StartupPath + "/TrainedFaces/" + facename, ImageFormat.Jpeg);
                    }
                    catch (Exception) { return true; }
                }
                else
                {
                    Directory.CreateDirectory(Variables.DataSetFolder);
                    try
                    {
                        face_data.Save(Application.StartupPath + "/TrainedFaces/" + facename, ImageFormat.Jpeg);
                    }
                    catch (Exception) { return true; }
                }
                if (File.Exists(Variables.LabelsFile))
                {
                    //File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", NAME_PERSON.Text + "\n\r");
                    bool loading = true;
                    while (loading)
                    {
                        try
                        {
                            docu.Load(Variables.LabelsFile);
                            loading = false;
                        }
                        catch
                        {
                            docu = null;
                            docu = new XmlDocument();
                            Thread.Sleep(10);
                        }
                    }

                    //Get the root element
                    XmlElement root = docu.DocumentElement;

                    XmlElement face_D = docu.CreateElement("FACE");
                    XmlElement name_D = docu.CreateElement("NAME");
                    XmlElement file_D = docu.CreateElement("FILE");

                    name_D.InnerText = name;
                    file_D.InnerText = facename;


                    face_D.AppendChild(name_D);
                    face_D.AppendChild(file_D);

                    root.AppendChild(face_D);

                    //Save the document
                    docu.Save(Variables.LabelsFile);

                }
                else
                {
                    FileStream FS_Face = File.OpenWrite(Variables.LabelsFile);
                    using (XmlWriter writer = XmlWriter.Create(FS_Face))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Faces_For_Training");

                        writer.WriteStartElement("FACE");
                        writer.WriteElementString("NAME", name);
                        writer.WriteElementString("FILE", facename);
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    FS_Face.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void ParallelTrainingFrm_Load(object sender, EventArgs e)
        {
            
        }

        private async void TrainBtn_Click(object sender, EventArgs e)
        {
            await Task.Run(async() =>
            {
                foreach (PictureBox item in detectedfacespanel.Controls)
                {
                   await Task.Run(() => { save_training_data(item.Image, CurrentPlayer.Name); });
                }
            });
            label1.Text = "Training Done , Face Data Saved";
        }
        private async void BrowseBtn_Click(object sender, EventArgs e)
        {
            detectedfacespanel.Controls.Clear();
            naturalfaces.Controls.Clear();
            label1.Text = "Waiting";

            CurrentPlayer = new player();
            currentFrame = null;
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = true;
            op.ShowDialog();
            string Team = textBox1.Text;
            foreach (var playerfolder in  op.FileNames)
            {
                try
                {
                    string name = Path.GetFileName(Path.GetDirectoryName(playerfolder));
                    int number = Convert.ToInt16(Regex.Match(name, @"\d+").Value);
                    name = name.Replace(number.ToString(), "").TrimEnd().TrimStart();
                    Debug.WriteLine("Name : " + name);
                    Debug.WriteLine("Team : " + Team);
                    Debug.WriteLine("Number:" + number);

                    currentFrame = new Image<Bgr, byte>(playerfolder);
                    addnaturalface(currentFrame);
                    var face = DetectFaces(currentFrame);
                    adddetectedface(face);
                    CurrentPlayer = new player();
                    CurrentPlayer.Name = name;
                    CurrentPlayer.Number = number;
                    CurrentPlayer.Team = Team;
                    await DbIn.InsertFaceElement(name, Team, number);
                    Debug.WriteLine("Saving");
                    
                }
                catch (Exception) { continue; }
                label1.Text = "Ready For Training";
            }
        }
        private void adddetectedface(Image face)
        {
            
            if (face.Size == currentFrame.Size) { return; }
            else
            {
                PictureBox b = new PictureBox();
                b.Image = face;
                b.Size = new Size(174, 138);
                b.SizeMode = PictureBoxSizeMode.AutoSize;
                detectedfacespanel.Controls.Add(b);
            }
        }
        private void addnaturalface(Image<Bgr, byte> currentFrame)
        {
            PictureBox b = new PictureBox();
            b.Image = currentFrame.Bitmap;
            b.Size = new Size(174, 138);
            b.SizeMode = PictureBoxSizeMode.Zoom;
            naturalfaces.Controls.Add(b);
        }
        //Process Frame
        public Image DetectFaces (Image<Bgr,byte> currentframe)
        {
            if (currentFrame != null)
            {
                currentFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                gray_frame = currentFrame.Convert<Gray, Byte>();
                //Face Detector
                Rectangle[] facesDetected = Face.DetectMultiScale(gray_frame, 1.2, 10, new Size(50, 50), Size.Empty);
                if (facesDetected.Length == 0)
                {
                    facesDetected = Face.DetectMultiScale(gray_frame, 1.1, 6, new Size(50, 50), gray_frame.Size);
                    for (int i = 0; i < facesDetected.Length; i++)// (Rectangle face_found in facesDetected)
                    {
                        //This will focus in on the face from the haar results its not perfect but it will remove a majoriy
                        //of the background noise
                        if (facesDetected.Length==0)
                        {
                            return currentFrame.Bitmap;
                        }
                        facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                        result = currentFrame.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        result._EqualizeHist();

                        return result.Bitmap;
                    }
                }
                else
                {
                    //Action for each element detected
                    for (int i = 0; i < facesDetected.Length; i++)// (Rectangle face_found in facesDetected)
                    {
                        //This will focus in on the face from the haar results its not perfect but it will remove a majoriy
                        //of the background noise
                        facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                        result = currentFrame.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        result._EqualizeHist();

                        return result.Bitmap;
                    }
                }
                
            }
            else
            {

                return currentframe.Bitmap;
            }
            return currentframe.Bitmap;
        }

      
    }
}
