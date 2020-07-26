using Bunifu.Framework.UI;
using DbSystem;
using DbSystem.Controller;
using DbSystem.Struct;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LoginSystem;
using LoginSystem.Users;
using SMARTY;
using SMARTY.Properties;
using SMARTY.Trainer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static SMARTY.Trainer.Variables;

namespace SMARTY.Ui
{
    public partial class MainForm : Form
    {
        #region DataBase Variables
        //Players Database Connection
        Core Dbase = new Core();
        // Classes To read from and write on Database
        Inserter DbIn;
        Reader DbOut;

        /// Login System DataBase
        Sys LoginSys;
        #endregion
        #region Detection System Variables
        Image<Bgr, Byte> currentFrame; //current image aquired from file for display
        Image<Gray, byte> result; //used to store the result image and trained face
        Image<Gray, byte> gray_frame = null; //grayscale current image aquired for processing
        public CascadeClassifier Face = new CascadeClassifier(Variables.CascadeFile);//Our face detection method 
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, 0.5); //Our font for writing within the frame                                                                         
        ClassifierTrainer Eigen_Recog = new ClassifierTrainer();
        #endregion
        public MainForm()
        {
            InitializeComponent();

            //Ui Class To Drag the Ui H and V
            BunifuDragControl drag = new
                BunifuDragControl();
            drag.TargetControl = LoginPanel;
        }

        //Method to Write Strings on Splash Screen
        private void FeedBack(string text)
        {
            Splash.spl.feedback.Invoke((MethodInvoker)(() => Splash.spl.feedback.Text = text));

        }

        //Loading Elements
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Declaring Database Variables
                FeedBack("Loading DataBase");
                Dbase.Connect(Resources.Constring);
                DbIn = new Inserter(Dbase.Con);
                DbOut = new Reader(Dbase.Con);
                FeedBack("Loading LoginSystem");
                LoginSys = new Sys(Resources.Constring1);
                FeedBack("Ready");

                //Showimg Login Panel
                LoginPanel.Dock = DockStyle.Fill;

                // Face Detector Training [ Loading Current Data Set]
                if (!Eigen_Recog.Loaded) { Eigen_Recog.LoadTrainingData(); }

            }
            catch (Exception DbaseEx)
            {
                FeedBack("Loading Failed ,Restart");
                Debug.WriteLine(DbaseEx.Message);
            }

        }

        #region Login Panel Buttons
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //Changing Text of Sign in button
            SigninBtn.ButtonText = "Login";
            //Moving The Red Little Line Under Buttons
            mark.Location = new Point(311, 260);
        }
        private void SingUpBtn_Click(object sender, EventArgs e)
        {
            SigninBtn.ButtonText = "Sign Up";
            mark.Location = new Point(381, 260);
        }
        //Submit Button
        private async void SigninBtn_Click(object sender, EventArgs e)
        {
            //Check if Id and password are not empty
            if (NameBox.Text !="" && PassBox.Text !="" )
            {
                //If Login Case
                if (SigninBtn.ButtonText == "Login")
                {
                    //DOLOGIN : by Sending user and pw to reader Class of data base
                    SignInResult resutl =  LoginSys.SignIn(NameBox.Text, PassBox.Text);
                    if (resutl.LoggedIn)
                    {
                        //If Login Operation Successfull , Show the Ui
                        LoginPanel.Dock = DockStyle.None;
                        LoginPanel.Hide();
                        LoginPanel.SendToBack();

                        UserLabel.Show();
                        UserBtn.Show();
                        DetectBtn.Show();
                        TrainBtn.Show();
                        LockBtn.Show();
                        ExitBtn.Show();
                        mark2.Show();

                        UserLabel.Text = resutl.UserData.Name.ToUpperInvariant();
                        DetectionPanel.Show();

                    }
                    else
                    {
                        label3.Text = resutl.Reason;
                    }
                }
                else
                {
                    //If Registration Senario : Sending User and pw to Writer Class
                    bool suResult = await LoginSys.SignUp(NameBox.Text, PassBox.Text, "", 0);
                    if (suResult)
                    {

                        label3.Text = "Registration Done Sucessfully";

                    }
                }
            }
        }
        //Exit Button
        private void ExitBtn2_Click(object sender, EventArgs e)
        { //Environment : it means the whole application
            Environment.Exit(0);
        }
        #endregion

        #region Main Window Buttons
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }    
        //Showing Detection PANEL Button
        private void DetectBtn_Click(object sender, EventArgs e)
        {
            mark2.Location = new Point(312, 29);
            DetectionPanel.Show();
            DetectionPanel.Location = new Point(21, 49);
        }
        //Button Showing Training Panel/Form
        private void TrainBtn_Click(object sender, EventArgs e)
        {
           mark2.Location = new Point(368, 28);
            DetectionPanel.Hide();
            ParallelTrainingFrm Tr = new ParallelTrainingFrm();
            Tr.ShowDialog();
            //Hide all 
        }
        //Logout Button
        private void LockBtn_Click(object sender, EventArgs e)
        {//Hiding all Ui elements

            UserLabel.Hide();
            UserBtn.Hide();
            DetectBtn.Hide();
            TrainBtn.Hide();
            LockBtn.Hide();
            ExitBtn.Hide();
            mark2.Hide();

            LoginPanel.Show();
            LoginPanel.Dock = DockStyle.Fill;
            LoginPanel.BringToFront();

            NameBox.Text = "";
            PassBox.Text = "";
            
        }
        #endregion

        #region Detection Panel Buttons
        //Browse button to get an image to Recognize
        private void BrowseBtn_Click(object sender, EventArgs e)
        {//Clearing Player data
            playerNameR.Text = "";
            PlayerNumberR.Text = "";
            PlayerTeamR.Text = "";

            //Open Choosing window
            OpenFileDialog op = new OpenFileDialog();
            {
                if (op.ShowDialog() == DialogResult.OK)
                {
                    //Assign Current Frame variable by Selected File 
                    currentFrame = new Image<Bgr, byte>(op.FileName);
                    //Putting the loaded image into Ui
                    FaceBox.Image = Image.FromFile(op.FileName);
                }
            }
        }
        //Image Analyzer
        void FrameGrabber_Standard()
        {
            //Resizing The Image to Get better Results
            currentFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            FaceBox.Image = currentFrame.Bitmap;
            //Convert it to Grayscale
            if (currentFrame != null)
            {
                gray_frame = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = Face.DetectMultiScale(gray_frame, 1.2, 10, new Size(50, 50), Size.Empty);
                if (facesDetected.Length == 0)
                {
                    //iN Case of No faces detected , Changing the image scale from 1.2 to 1.1
                    label2.Text = "Scaled At 1.1F and By 6 neighbours";
                    facesDetected = Face.DetectMultiScale(gray_frame, 1.1, 6, new Size(50, 50), gray_frame.Size);
                }

                //Action for each face detected
                for (int i = 0; i < facesDetected.Length; i++)// (Rectangle face_found in facesDetected)
                {
                    //This will focus in on the face from the haar results its not perfect but it will remove a majoriy
                    //of the background noise
                    facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                    facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                    facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                    facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                    //Copying the detected face to a new variable called result , to do Histogram Equalization
                    result = currentFrame.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    result._EqualizeHist();

                    //draw the face detected in the 0th (gray) channel with RED color
                    currentFrame.Draw(facesDetected[i], new Bgr(Color.Red), 2);

                    //Checking The loaded faces previsouly
                    if (Eigen_Recog.Loaded)
                    {
                        //Matching Current face with all faces stored in DataSet
                        RecognitionResult Result = Eigen_Recog.Recognise(result);
                        //If Face Data found , assign it on name , and the simillarity Value
                        string name = Result.Label;
                        int match_value = Result.Int;

                        try
                        {
                            //Here , Matching Found Face name with its name on players database
                            //to get team , number data
                            player found = DbOut.Players().Where(x => x.Name == name).FirstOrDefault();
                            playerNameR.Text = found.Name;
                            PlayerNumberR.Text = found.Number.ToString();
                            PlayerTeamR.Text = found.Team;
                        }
                        catch (NullReferenceException) { }
                        //Draw the label for each face detected and recognized
                        currentFrame.Draw(name + " ", ref font, new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), new Bgr(Color.LightGreen));
                        // ADD_Face_Found(result, name, match_value);
                    }
                }
                
            }
        }
        private void RecFaceBtn_Click(object sender, EventArgs e)
        {
            FrameGrabber_Standard();

        }
        #endregion

      
    }
}
