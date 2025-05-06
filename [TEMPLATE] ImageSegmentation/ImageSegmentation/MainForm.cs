using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq; // lazm n4ilo 3lashan el doctor hinf5ona 

namespace ImageTemplate
{
    public partial class MainForm : Form
    {

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        public MainForm()
        {
            InitializeComponent();

            // lazm n4ilo 3lashan el doctor hinf5ona 
            AllocConsole();


        }

        RGBPixel[,] ImageMatrix;


        // lazm n4ilo 3lashan el doctor hinf5ona 



        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

            Console.WriteLine("Printing ImageMatrix:");
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)  // Loop through rows
            {
                for (int j = 0; j < ImageMatrix.GetLength(1); j++)  // Loop through columns
                {
                    Console.Write((int)ImageMatrix[i, j].blue + "  ");  // Print each element followed by a tab for better formatting
                }
                Console.WriteLine("\n");  // Move to the next line after each row
                Console.WriteLine();
            }
            GRAPH graph = new GRAPH(ImageMatrix);

            // lazm n4ilo 3lashan el doctor hinf5ona 
            Dictionary<long, List<Tuple<long, int>>> redWeights = graph.Red_Weight();
            Dictionary<long, List<Tuple<long, int>>> Blue_Weight = graph.Blue_Weight();
            Dictionary<long, List<Tuple<long, int>>> Green_Weight = graph.Green_Weight();

            // lazm n4ilo 3lashan el doctor hinf5ona 
            // Segmentation part
            Segmentation segmentation = new Segmentation(ImageMatrix);
            var (redComponents, greenComponents, blueComponents) = segmentation.SegmentImage();

            Console.WriteLine("\nRed Segmentation:");
            for (int i = 0; i < redComponents.Count; i++)
            {
                Console.WriteLine($"Component {i}:");
                Console.WriteLine("Inner Pixels: " + string.Join(", ", redComponents[i].inner_pixels));
                Console.WriteLine("Boundaries: " + string.Join(", ", redComponents[i].boundaries));
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nGreen Segmentation:");
            for (int i = 0; i < greenComponents.Count; i++)
            {
                Console.WriteLine($"Component {i}:");
                Console.WriteLine("Inner Pixels: " + string.Join(", ", greenComponents[i].inner_pixels));
                Console.WriteLine("Boundaries: " + string.Join(", ", greenComponents[i].boundaries));
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nBlue Segmentation:");
            for (int i = 0; i < blueComponents.Count; i++)
            {
                Console.WriteLine($"Component {i}:");
                Console.WriteLine("Inner Pixels: " + string.Join(", ", blueComponents[i].inner_pixels));
                Console.WriteLine("Boundaries: " + string.Join(", ", blueComponents[i].boundaries));
                Console.WriteLine(new string('-', 40));
            }

            MessageBox.Show("Red weights printed to console!");
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}