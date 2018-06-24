using Emgu.CV;
using System;
using System.Windows.Forms;

namespace MainApplicationForm.Model
{
    class FaceEyeTemplate
    {
        private HaarCascade haarface;
        private HaarCascade haareyes;

        public FaceEyeTemplate()
        {
            try
            {
                this.haarface = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
                this.haareyes = new HaarCascade("haarcascade_eye.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading datasets.","Contact your Administrator");
            }
        }

        public HaarCascade getFaceTemplate()
        {
            return this.haarface;
        }

        public HaarCascade getEyeTemplate()
        {
            return this.haareyes;
        }
    }
}
