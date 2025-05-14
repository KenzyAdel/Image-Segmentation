using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq; // lazm n4ilo 3lashan el doctor hinf5ona 
using System.Threading.Tasks;
using System.Diagnostics;

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
        Segmentation segmentation;

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
            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

            Stopwatch timer = Stopwatch.StartNew();
            segmentation = new Segmentation(ImageMatrix);
            segmentation.constructEdges();
            segmentation.Red_Segment();
            segmentation.Blue_Segment();
            segmentation.Green_Segment();
            segmentation.Merge();
            timer.Stop();
            long time = timer.ElapsedMilliseconds;

            //Internal_Difference internal_diff = new Internal_Difference(ImageMatrix);
            //segmentation.SegmentImage();

            // internal_diff.CalculateFinalInternalDifferences(segmentation._componentPixels);
            // internal_diff.Difference_between_2_components(segmentation._componentPixels);
            // internal_diff.Merge(internal_diff.bounderies_between_components, internal_diff.internalDifferences, 30000);
            //Console.WriteLine("\n=== Components and Pixel Counts ===");
            int count = 0;
            foreach (var component in segmentation._componentPixels)
            {
                count++;
                /*int componentId = component.Key;
                List<int> pixels = new List<int>();

                // Collect pixels with non-zero intensities and count them


                // Print component details
                Console.Write($"Component {componentId}: ");
                //bool first = true;
                foreach (long pixel in pixels)
                {
                    if (!first) Console.Write(", ");
                    Console.Write($"arr[{pixel}] = {intensities[pixel]}");
                    first = false;
                }
                Console.WriteLine($" (Count: {component.Value.Count})");
                //}
*/
            }
            Console.WriteLine($" (Total Components: {count})");
            Console.WriteLine($" (Total Time: {time})");



            // internal_diff.Difference_between_2_components(segmentation._componentPixels);
            //  List<(long v1, long v2, int w)> Blue_Weight;
            // List<(long v1, long v2, int w)> Green_Weight;

            //     Parallel.Invoke(
            //    () => Red_Weights = graph.Red_Weight(),
            //    () => Blue_Weight = graph.Blue_Weight(),
            //    () => Green_Weight = graph.Green_Weight()
            //);
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

            // Segmentation segmentation = new Segmentation(ImageMatrix);
            // Dictionary<long, List<long>> components = segmentation.SegmentImage(Red_Weights,Blue_Weight,Green_Weight);
            // Internal_Difference internal_Difference = new Internal_Difference(segmentation.M);
            // Dictionary<Tuple<long, long>, int> bounderies_between_components = internal_Difference.Difference_between_2_components(components,segmentation.M, Red_Weights, Green_Weight, Blue_Weight);

            //Console.WriteLine("Red Component Counts:");
            //PrintComponentCounts(segmentation.);

            // Console.WriteLine("Green Component Counts:");
            // PrintComponentCounts(segmentation.greenMap);

            // Console.WriteLine("Blue Component Counts:");
            // PrintComponentCounts(segmentation.blueMap);

            // Print all components and their pixel counts

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

        private void merge_form2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                // Create Form2 and pass the image
                Bonus2 bonus2 = new Bonus2(pictureBox2.Image, segmentation);
                bonus2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("No image in PictureBox!");
            }
        }
    }
}