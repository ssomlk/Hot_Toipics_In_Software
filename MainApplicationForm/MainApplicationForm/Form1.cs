using System;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using MainApplicationForm.Model;

namespace MainApplicationForm
{
    public partial class Form1 : Form
    {
        private Capture capture;
        private Camera cameracapture;

        public Form1()
        {
            InitializeComponent();
            this.cameracapture = new Camera();
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
                    imageBox1.Image = null;
                }
                else
                {
                    button1.Text = "Stop";
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
            Image<Bgr, byte> ImageFrame = this.cameracapture.getFrame();
            imageBox1.Image = ImageFrame;
        }
    }
}
