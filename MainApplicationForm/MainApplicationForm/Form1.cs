using System;
using System.Windows.Forms;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using MainApplicationForm.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace MainApplicationForm
{
    public partial class Form1 : Form
    {
        private Capture capture;
        private Camera cameracapture;
        private HaarCascade haarface;
        private HaarCascade haareyes;
        private FaceEyeTemplate template;
        private Face face;
        private Eye eye;
        private Alert alert = null;
        private float percentage = 50;
        private int sequence = 1;
        private float countframes = 0;
        private int seconds = 0;
        private float eyes_count = 0;
        private float actual_percentage = 0;
        public Thread t = null;

        public Form1()
        {
            InitializeComponent();
            this.cameracapture = new Camera();
            this.template = new FaceEyeTemplate();
            this.face = new Face();
            this.eye = new Eye();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                try
                {
                    capture = cameracapture.getNewCapture();
                    this.cameracapture.setNewCapture(capture);
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (capture != null)
            {
                if (cameracapture.getCaptureInProgress())
                {
                    button1.Text = "Start";
                    Application.Idle -= ProcessFrame;
                    timer1.Enabled = false;
                    imageBox1.Image = null;
                }
                else
                {
                    button1.Text = "Stop";
                    timer1.Enabled = true;
                    Application.Idle += ProcessFrame;
                }

                cameracapture.setCaptureInProgress(!(cameracapture.getCaptureInProgress()));
            }
        }

        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> ImageFrame = this.cameracapture.getFrame();
            if (ImageFrame != null)
            {
                countframes += 1;
                Image<Gray, byte> grayframe = cameracapture.getGrayFrame(ImageFrame);
                grayframe._EqualizeHist();

                MCvAvgComp[][] facesDetected = this.face.detectFace(grayframe, haarface);

                if (facesDetected[0].Length == 1)
                {
                    MCvAvgComp face = facesDetected[0][0];
                    //Set the region of interest on the faces                

                    // Our Region of interest where find eyes will start with a sample estimation using face metric
                    Int32 yCoordStartSearchEyes = face.rect.Top + (face.rect.Height * 3 / 11);
                    Point startingPointSearchEyes = new Point(face.rect.X, yCoordStartSearchEyes);
                    Point endingPointSearchEyes = new Point((face.rect.X + face.rect.Width), yCoordStartSearchEyes);

                    Size searchEyesAreaSize = new Size(face.rect.Width, (face.rect.Height * 2 / 9));
                    Point lowerEyesPointOptimized = new Point(face.rect.X, yCoordStartSearchEyes + searchEyesAreaSize.Height);
                    Size eyeAreaSize = new Size(face.rect.Width / 2, (face.rect.Height * 2 / 9));
                    Point startingLeftEyePointOptimized = new Point(face.rect.X + face.rect.Width / 2, yCoordStartSearchEyes);

                    Rectangle possibleROI_eyes = new Rectangle(startingPointSearchEyes, searchEyesAreaSize);
                    Rectangle possibleROI_rightEye = new Rectangle(startingPointSearchEyes, eyeAreaSize);
                    Rectangle possibleROI_leftEye = new Rectangle(startingLeftEyePointOptimized, eyeAreaSize);

                    grayframe.ROI = possibleROI_leftEye;
                    MCvAvgComp[][] leftEyesDetected = this.eye.detectEyes(grayframe, haareyes);
                    grayframe.ROI = Rectangle.Empty;

                    grayframe.ROI = possibleROI_rightEye;
                    MCvAvgComp[][] rightEyesDetected = this.eye.detectEyes(grayframe, haareyes);
                    grayframe.ROI = Rectangle.Empty;

                    //If we are able to find eyes inside the possible face, it should be a face, maybe we find also a couple of eyes
                    if (leftEyesDetected[0].Length != 0 && rightEyesDetected[0].Length != 0)
                    {
                        eyes_count += 1;
                        //draw the face
                        ImageFrame.Draw(face.rect, new Bgr(Color.Red), 2);

                        MCvAvgComp eyeLeft = leftEyesDetected[0][0];
                        MCvAvgComp eyeRight = leftEyesDetected[0][0];

                        //Uncomment this to draw all rectangle eyes

                        Rectangle eyeRectL = eyeLeft.rect;
                        eyeRectL.Offset(startingLeftEyePointOptimized.X, startingLeftEyePointOptimized.Y);
                        ImageFrame.Draw(eyeRectL, new Bgr(Color.Red), 2);

                        Rectangle eyeRectR = eyeRight.rect;
                        eyeRectR.Offset(startingPointSearchEyes.X, startingPointSearchEyes.Y);
                        ImageFrame.Draw(eyeRectR, new Bgr(Color.Red), 2);

                    }
                    imageBox1.Image = ImageFrame;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            haarface = this.template.getFaceTemplate();
            haareyes = this.template.getEyeTemplate();
            listBox1.Items.Add("Act.Time        War. Seq     Percentage     Tot. Frames      Dete. Frames       Actual Per");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.percentage = (this.trackBar1.Value * 10);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.sequence = this.trackBar2.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string time = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
            seconds += 1;
            if (this.seconds % this.sequence == 0)
            {
                this.actual_percentage = (eyes_count / countframes) * 100;
                listBox1.Items.Add(time + "         " + sequence + "                   " + percentage + "                     " + countframes + "                " + eyes_count + "                 " + actual_percentage);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                eyes_count = 0;
                countframes = 0;
                if (actual_percentage < percentage)
                {
                    this.alert = new Alert();
                    t = new Thread(this.alert.generateWarnings);
                    t.Start();
                }
            }
        }
    }
}
