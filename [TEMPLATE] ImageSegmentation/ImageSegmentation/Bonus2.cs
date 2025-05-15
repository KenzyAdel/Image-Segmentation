using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class Bonus2 : Form
    {
        private Segmentation rcvComponents;
        private HashSet<int> parentComponents;
        private RGBPixel[,] ImageMatrix;
        private Image InitialImageState;
        public Bonus2(Image imageFromForm1, Segmentation segmentation, RGBPixel[,] ImageMatrix)
        {
            InitializeComponent();

            InitialImageState = imageFromForm1;
            // Receive the output of the mainForm in the constructor 
            pictureBox1_bonus2.Image = InitialImageState;
            rcvComponents = segmentation;
            this.ImageMatrix = ImageMatrix;
            parentComponents = new HashSet<int>();
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
                g.FillEllipse(Brushes.Black, column - 3, row - 3, 5, 5);
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

            int columns = ImageMatrix.GetLength(1);
            int rows = ImageMatrix.GetLength(0);

            // Initialize the dimensions of the selected components image
            Bitmap extractedComponent = new Bitmap(columns, rows);

            // For each selected component, restore original pixels
            foreach (int component in parentComponents)
            {

                foreach (int pixelIndex in rcvComponents._componentPixels[component])
                {

                    int pixelColumn = pixelIndex % columns;
                    int pixelRow = pixelIndex / columns;

                    // Get original RGB values from ImageMatrix
                    RGBPixel originalPixelColors = ImageMatrix[pixelRow, pixelColumn];

                    // Set the pixel in the current index (row, col) to it's RGB original colors
                    extractedComponent.SetPixel(pixelColumn, pixelRow, Color.FromArgb(
                        originalPixelColors.red,
                        originalPixelColors.green,
                        originalPixelColors.blue
                    ));
                }
            }

            // Finally display the selected component with it's original colors in the picture box
            pictureBox2_bonus2.Image = extractedComponent;
        }

        private void clearSelections_Click(object sender, EventArgs e)
        {
            parentComponents.Clear();
            pictureBox1_bonus2.Image = InitialImageState;
            pictureBox2_bonus2.Image = null;

            // Remove the drawn black circles
            pictureBox1_bonus2.Invalidate();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
