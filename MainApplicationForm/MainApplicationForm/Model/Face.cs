using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System;
using System.Diagnostics;

namespace MainApplicationForm.Model
{
    class Face
    {
        private MCvAvgComp[][] faceDetected;

        public MCvAvgComp[][] detectFace(Image<Gray, byte> grayFrame, HaarCascade faces)
        {
            try
            {
                this.faceDetected = grayFrame.DetectHaarCascade(faces, 1.15, 1,
                            HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                            new Size(20, 20));

                return this.faceDetected;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return null;
            }
        }
    }
}
