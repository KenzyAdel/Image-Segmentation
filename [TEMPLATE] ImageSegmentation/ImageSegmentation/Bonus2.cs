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
        private HashSet<int> parentComponents = new HashSet<int>();

        public Bonus2(Image imageFromForm1, Segmentation segmentation)
        {
            InitializeComponent();

            // Receive the output of the mainForm in the constructor 
            pictureBox1_bonus2.Image = imageFromForm1;
            rcvComponents = segmentation;

            // Debug the number of received components
            Console.WriteLine($"Total components in received segmentation: {rcvComponents._componentPixels.Count}");
        }

        private void pictureBox1_bonus2_MouseClick(object sender, MouseEventArgs e)
        {
            // convert the image of pictureBox1_bonus2 to a Bitmap to access and manipulate the pixel data (same as what DR made in open image helper)
            Bitmap pictureBox1Image = (Bitmap)pictureBox1_bonus2.Image;
            int column = e.X;
            int row = e.Y;

            //Highlight the clicked position
            using (Graphics g = pictureBox1_bonus2.CreateGraphics())
            {
                //Draw black circles on the selected pixels
                g.FillEllipse(Brushes.Black, column - 2, row - 2, 4, 4);
            }

            // Get the pixel index to get its component
            int pixelIdx = row * pictureBox1Image.Width + column;
            int pixelParentComp = rcvComponents.Find_set(pixelIdx, rcvComponents.final_member);

            // Store the component number of the selected pixel
            parentComponents.Add(pixelParentComp);
            Console.WriteLine($"Selected pixel parent component: {pixelParentComp}");

        }

        private void Merge_Visualize_Click(object sender, EventArgs e)
        {
            Bitmap pictureBox_original = (Bitmap)pictureBox1_bonus2.Image;

            // Initialize the dimensions of the selected components image
            Bitmap extractedComponent = new Bitmap(pictureBox_original.Width, pictureBox_original.Height);

            foreach (int component in parentComponents)
            {
                if (!rcvComponents._componentPixels.ContainsKey(component))
                    continue;

                // Get all indices that belong to the current component
                var pixelIndices = rcvComponents._componentPixels[component];


                foreach (int index in pixelIndices)
                {
                    // Converting a 1D index into 2D coordinates as we store it in a format index = i * columns + j;
                    int width = pictureBox_original.Width;
                    int pixelColumn = index % width;
                    int pixelRow = index / width;
                    // Get the original colors of the pixel and replace the RGB colors with the original colors
                    Color pixelColors = pictureBox_original.GetPixel(pixelColumn, pixelRow);
                    extractedComponent.SetPixel(pixelColumn, pixelRow, pixelColors);
                }
            }
            // Finally fill the picture box with the original pixel colors of the selected components
            pictureBox2_bonus2.Image = extractedComponent;
        }
    }
}
