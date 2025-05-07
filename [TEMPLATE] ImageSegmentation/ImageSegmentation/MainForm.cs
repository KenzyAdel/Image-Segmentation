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

            /*Console.WriteLine("Printing ImageMatrix:");
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)  // Loop through rows
            {
                for (int j = 0; j < ImageMatrix.GetLength(1); j++)  // Loop through columns
                {
                    Console.Write((int)ImageMatrix[i, j].blue + "  ");  // Print each element followed by a tab for better formatting
                }
                Console.WriteLine("\n");  // Move to the next line after each row
                Console.WriteLine();
            }*/
            GRAPH graph = new GRAPH(ImageMatrix);

            // lazm n4ilo 3lashan el doctor hinf5ona 
            Dictionary<long, List<Tuple<long, int>>> redWeights = graph.Red_Weight();
            Dictionary<long, List<Tuple<long, int>>> Blue_Weight = graph.Blue_Weight();
            Dictionary<long, List<Tuple<long, int>>> Green_Weight = graph.Green_Weight();

            // lazm n4ilo 3lashan el doctor hinf5ona 
            // Segmentation part

            /* Segmentation segmentation = new Segmentation(ImageMatrix);
             var (redMap, greenMap, blueMap) = segmentation.SegmentImage();

            // Count component IDs
            Console.WriteLine("Red Component Counts:");
            PrintComponentCounts(redMap);

            Console.WriteLine("Green Component Counts:");
            PrintComponentCounts(greenMap);

            Console.WriteLine("Blue Component Counts:");
            PrintComponentCounts(blueMap);*/

            Segmentation segmentation = new Segmentation(ImageMatrix);
            Dictionary<long, int[]> components = segmentation.SegmentImage();

            Console.WriteLine("Red Component Counts:");
            PrintComponentCounts(segmentation.redMap);

            Console.WriteLine("Green Component Counts:");
            PrintComponentCounts(segmentation.greenMap);

            Console.WriteLine("Blue Component Counts:");
            PrintComponentCounts(segmentation.blueMap);

            // Print all components and their pixel counts
            Console.WriteLine("\n=== Components and Pixel Counts ===");
            foreach (var component in components)
            {
                long componentId = component.Key;
                int[] intensities = component.Value;
                List<long> pixels = new List<long>();
                int pixelCount = 0;

                // Collect pixels with non-zero intensities and count them
                for (long pixel = 0; pixel < intensities.Length; pixel++)
                {
                    if (intensities[pixel] != 0)
                    {
                        pixels.Add(pixel);
                        pixelCount++;
                    }
                }

                // Print component details
                Console.Write($"Component {componentId}: ");
                //bool first = true;
                /*foreach (long pixel in pixels)
                {
                    if (!first) Console.Write(", ");
                    Console.Write($"arr[{pixel}] = {intensities[pixel]}");
                    first = false;
                }*/
                Console.WriteLine($" (Count: {pixelCount})");
            }

            MessageBox.Show("Red weights printed to console!");
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void PrintComponentCounts(long[] map)
        {
            Dictionary<long, int> countMap = new Dictionary<long, int>();

            foreach (long id in map)
            {
                if (!countMap.ContainsKey(id))
                    countMap[id] = 0;
                countMap[id]++;
            }

            foreach (var kvp in countMap.OrderBy(k => k.Key))
            {
                Console.WriteLine($"Component ID {kvp.Key}: {kvp.Value} pixels");
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}