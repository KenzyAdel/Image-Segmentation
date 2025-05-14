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
    public partial class Bonus2: Form
    {
        private Segmentation rcvComponents;
        public Bonus2(Image imageFromForm1, Segmentation segmentation)
        {
            InitializeComponent();
            pictureBox1_bonus2.Image = imageFromForm1;
            rcvComponents = segmentation; // Store the segmentation object

            // Example: Access segmentation data in Bonus2
            Console.WriteLine($"Total components in received segmentation: {rcvComponents._componentPixels.Count}");
        }

        private List<Point> clickedPoints = new List<Point>(); // Stores clicked coordinates

        private void pictureBox1_bonus2_Click(object sender, EventArgs e)
        {
            //pictureBox1_bonus2.MouseClick()
        }

        private void pictureBox1_bonus2_MouseClick(object sender, MouseEventArgs e)
        {
            // Get the clicked position relative to PictureBox
            Point clickPoint = e.Location;
            int row = clickPoint.X;
            int col = clickPoint.Y;
            Tuple<int, int> pixel_dimensions = new Tuple<int, int>(row, col);
            clickedPoints.Add(clickPoint); // Store the point

            // Optional: Display the coordinates (for debugging)
            Console.WriteLine($"Clicked at: X={clickPoint.X}, Y={clickPoint.Y}");

            // Optional: Draw a small circle at the clicked position
            using (Graphics g = pictureBox1_bonus2.CreateGraphics())
            {
                g.FillEllipse(Brushes.Black, clickPoint.X - 2, clickPoint.Y - 2, 4, 4);
            }
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {

        }
    }
}
