using System;
using System.Drawing;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace MainApplicationForm.Model
{
    class Eye
    {
        private MCvAvgComp[][] eyesDetected;

        public MCvAvgComp[][] detectEyes(Image<Gray, byte> grayFrame, HaarCascade eyes)
        {
            try
            {
                this.eyesDetected = grayFrame.DetectHaarCascade(eyes, 1.15, 1,
                            HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                            new Size(20, 20));

                return this.eyesDetected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return null;
            }
        }
    }
}
