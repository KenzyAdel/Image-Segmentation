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
using System.IO;

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

            Stopwatch timer = Stopwatch.StartNew();
            Console.WriteLine("======Time Started=======");
            Segmentation segmentation = new Segmentation(ImageMatrix);
            //segmentation.constructEdges();


            Parallel.Invoke(
                () =>
                {
                    Console.WriteLine($"Thread {Task.CurrentId}: Starting Red Segment..."); 
                    segmentation.ConstructRedEdges();
                    Console.WriteLine($"Thread {Task.CurrentId}: Finished Red Segment.");
                },
                () =>
                {
                    Console.WriteLine($"Thread {Task.CurrentId}: Starting Blue Segment...");
                    segmentation.ConstructBlueEdges();
                    Console.WriteLine($"Thread {Task.CurrentId}: Finished Blue Segment.");
                },
                () =>
                {
                    Console.WriteLine($"Thread {Task.CurrentId}: Starting Green Segment...");
                    segmentation.ConstructGreenEdges();
                    Console.WriteLine($"Thread {Task.CurrentId}: Finished Green Segment.");
                }
            );



            Parallel.Invoke(
        () =>
        {
            Console.WriteLine($"Thread {Task.CurrentId}: Starting Red Segment..."); // Optional logging
            segmentation.Red_Segment();
            Console.WriteLine($"Thread {Task.CurrentId}: Finished Red Segment.");
        },
        () =>
        {
            Console.WriteLine($"Thread {Task.CurrentId}: Starting Blue Segment...");
            segmentation.Blue_Segment();
            Console.WriteLine($"Thread {Task.CurrentId}: Finished Blue Segment.");
        },
        () =>
        {
            Console.WriteLine($"Thread {Task.CurrentId}: Starting Green Segment...");
            segmentation.Green_Segment();
            Console.WriteLine($"Thread {Task.CurrentId}: Finished Green Segment.");
        }
    );

            segmentation.Merge();
            timer.Stop();
            long time = timer.ElapsedMilliseconds;
            Console.WriteLine($" (Total Time: {time})");
          
            int count = 0;
            List<int> pixels=new List<int>();
            foreach (var component in segmentation._componentPixels)
            {
                count++;
                int componentId = component.Key;
                pixels.Add(component.Value.Count);
               
            }
            Console.WriteLine($" (Total Components: {count})");

            pixels.Sort();
            pixels.Reverse();
            string outputFilePath = "results.txt"; 

            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var pixel in pixels)
                {
                    writer.WriteLine(pixel); 
                }
            }
           
            //MessageBox.Show("Red weights printed to console!");
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

        }  

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}