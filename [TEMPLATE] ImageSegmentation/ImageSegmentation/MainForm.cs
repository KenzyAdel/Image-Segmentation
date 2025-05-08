using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; // lazm n4ilo 3lashan el doctor hinfo5na 

namespace ImageTemplate
{
    public partial class MainForm : Form
    {

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        public MainForm()
        {
            InitializeComponent();

            // lazm n4ilo 3lashan el doctor hinfo5na 
            AllocConsole();


        }

        RGBPixel[,] ImageMatrix;

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

            // lazm n4ilo 3lashan el doctor hinfo5na
            // Prints the 2-D array of the image
            Console.WriteLine("Printing ImageMatrix:");
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ImageMatrix.GetLength(1); j++)
                {
                    Console.Write((int)ImageMatrix[i, j].blue + "  ");
                }
                Console.WriteLine("\n");
                Console.WriteLine();
            }


            GRAPH graph = new GRAPH(ImageMatrix);

            // lazm n4ilo 3lashan el doctor hinfo5na 

            Dictionary<long, List<Tuple<long, int>>> redGraph = new Dictionary<long, List<Tuple<long, int>>>();
            Dictionary<long, List<Tuple<long, int>>> greenGraph = new Dictionary<long, List<Tuple<long, int>>>();
            Dictionary<long, List<Tuple<long, int>>> blueGraph = new Dictionary<long, List<Tuple<long, int>>>();

            graph.Build_Graph_Weights(redGraph, greenGraph, blueGraph);

            MessageBox.Show("Blue weights printed to console!");
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
