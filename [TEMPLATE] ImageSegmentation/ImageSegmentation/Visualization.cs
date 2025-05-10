using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class Visualization : Form
    {
        private RGBPixel[,] ImageMatrix; 

        public Visualization(RGBPixel[,] matrix)
        {
            InitializeComponent();
            ImageMatrix = matrix; 
        }

        private void btnVis_Click(object sender, EventArgs e)
        {
         

            Segmentation segmentation = new Segmentation(ImageMatrix);

            var components = segmentation.SegmentImage();

            int width = ImageMatrix.GetLength(1); 
            int height = ImageMatrix.GetLength(0); 

            Bitmap coloredImage = segmentation.ColorComponents(components, width, height);
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            pictureBox1.Image = coloredImage;
        }
    }

}
