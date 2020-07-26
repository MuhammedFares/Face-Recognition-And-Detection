using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using static SMARTY.Trainer.Variables;
using SMARTY.Trainer;

namespace SMARTY.Trainer
{
   public class ClassifierTrainer
    {
        //training variables

        //List To Store Training Images as GrayScale Images
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        //Lists To Store Training Labels and Ids
        List<string> ListOFNames = new List<string>(); 
        List<int> ListOfIds = new List<int>();

        //Variables To Store Number Of Labels
        int ContTrain, NumLabels;

        //Egien Recognizer Default Values
        float Eigen_Distance = 0;
        string Eigen_label;
        int Eigen_threshold = 2000;


        /// <summary>
        /// Boolean Value To Check If Trainer Class Has Obtained Training Data
        /// </summary>
        public bool Loaded;

        /// <summary>
        /// Boolean Value To Check Errors Before Doing Any Critical Operation
        /// </summary>
        public bool HasError = false;

        //Classifiers Declaring
        EigenFaceRecognizer eigen = new EigenFaceRecognizer(80, double.PositiveInfinity);
        FisherFaceRecognizer fisher = new FisherFaceRecognizer(0, 3500);
        LBPHFaceRecognizer Lp = new LBPHFaceRecognizer(1, 8, 8, 8, 100);

        public ClassifierTrainer()
        {
            Loaded = LoadTrainingData();
        }
        public bool LoadTrainingData()
        {
            if (File.Exists(LabelsFile))
            {
                try
                {
                    //Clearing Lists In Case of Retrain Operation Occoured.
                    ListOFNames.Clear();
                    ListOfIds.Clear();
                    trainingImages.Clear();

                    //Reading Xml File
                    FileStream filestream = File.OpenRead(LabelsFile);
                    long filelength = filestream.Length;
                    byte[] xmlBytes = new byte[filelength];
                    filestream.Read(xmlBytes, 0, (int)filelength);
                    filestream.Close();

                    MemoryStream xmlStream = new MemoryStream(xmlBytes);
                    using (XmlReader xmlreader = XmlTextReader.Create(xmlStream))
                    {
                        while (xmlreader.Read())
                        {
                            if (xmlreader.IsStartElement())
                            {
                                switch (xmlreader.Name)
                                {
                                    case "NAME":
                                        if (xmlreader.Read())
                                        {
                                            ListOfIds.Add(ListOFNames.Count);
                                            ListOFNames.Add(xmlreader.Value.Trim());
                                            NumLabels += 1;
                                        }
                                        break;
                                    case "FILE":
                                        if (xmlreader.Read())
                                        {
                                            //PROBLEM HERE IF TRAININGG MOVED
                                            trainingImages.Add(new Image<Gray, byte>(Variables.DataSetFolder + "\\" + xmlreader.Value.Trim()));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    ContTrain = NumLabels;

                    if (trainingImages.ToArray().Length != 0)
                    {
                        eigen.Train(trainingImages.ToArray(), ListOfIds.ToArray());
                        fisher.Train(trainingImages.ToArray(), ListOfIds.ToArray());
                        Lp.Train(trainingImages.ToArray(), ListOfIds.ToArray());
                        Loaded = true;

                        return true;
                    }
                    else return false;
                }
                catch (Exception Exp)
                {
                    HasError = true;
                    Debug.WriteLine("Error In Loading Function : " + Exp.Message);
                    return false;
                }
            }
            else return false;
        }
        public Variables.RecognitionResult Recognise(Image<Gray, byte> Image)
        {
            if (Loaded)
            {
                FaceRecognizer.PredictionResult EgienRes = eigen.Predict(Image);
                FaceRecognizer.PredictionResult FisherRes = fisher.Predict(Image);
                FaceRecognizer.PredictionResult LbRes = Lp.Predict(Image);

                if (EgienRes.Label == -1)
                {
                    Eigen_label = "Unknown";
                    Eigen_Distance = 0;
                    return new RecognitionResult() { Label = Eigen_label, Int = 0 };
                }
                else
                {
                    //TODO : Equalize All Labels Problems
                    Eigen_label = ListOFNames[EgienRes.Label];
                    if (EgienRes.Label != -1 && FisherRes.Label != -1 && LbRes.Label != -1)
                    {
                        if (EgienRes.Label == LbRes.Label && FisherRes.Label == EgienRes.Label)
                        {
                            return new RecognitionResult() { Label = Eigen_label, Int = (int)EgienRes.Distance };
                        }
                        else if (EgienRes.Distance > Eigen_threshold
                      && FisherRes.Distance > 3000
                      || LbRes.Distance > 100)
                        {
                            return new RecognitionResult() { Label = Eigen_label, Int = (int)EgienRes.Distance };
                        }
                        else
                        {
                            return new RecognitionResult() { Label = "Unkown", Int = 0 };
                        }
                    }
                    else if (EgienRes.Label != -1)
                    {
                        if (EgienRes.Distance > Eigen_threshold
                      && (int)FisherRes.Distance > 3000
                       && (int)LbRes.Distance > 100)
                        {
                            return new RecognitionResult() { Label = Eigen_label, Int = (int)EgienRes.Distance };
                        }
                    }
                  

                    return new RecognitionResult() { Label = "Unkown", Int = 0 };
                }
            }
            else
            {
                return new RecognitionResult() { Label = "Unkown", Int = 0, HasError = true, ErrorMessage = "Not Trained" };
            }
        }
    }
}
