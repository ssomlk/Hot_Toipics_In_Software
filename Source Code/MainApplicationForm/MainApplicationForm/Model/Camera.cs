using Emgu.CV;
using Emgu.CV.Structure;

namespace MainApplicationForm.Model
{
    class Camera
    {
        private Capture capture;
        private bool captureInProgress;
        private Image<Bgr, byte> imageFrame;

        public Capture getNewCapture()
        {
            this.capture = new Capture();
            return this.capture;
        }

        public void setNewCapture(Capture capture)
        {
            this.capture = capture;
        }

        public void setCaptureInProgress(bool status)
        {
            this.captureInProgress = status;
        }

        public bool getCaptureInProgress()
        {
            return this.captureInProgress;
        }

        public Image<Bgr, byte> getFrame()
        {
            this.imageFrame = this.capture.QueryFrame();
            return imageFrame;
        }

        public Image<Gray, byte> getGrayFrame(Image<Bgr, byte> image)
        {
            return image.Convert<Gray, byte>();
        }
    }
}
