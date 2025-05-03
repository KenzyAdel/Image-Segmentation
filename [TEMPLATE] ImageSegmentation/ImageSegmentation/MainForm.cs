using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; // lazm n4ilo 3lashan el doctor hinf5ona 

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
            GRAPH graph = new GRAPH(ImageMatrix);

            // lazm n4ilo 3lashan el doctor hinf5ona 
            Dictionary<long, List<int>> redWeights = graph.Red_Weight();

            // Print red weight dictionary to console
            foreach (var pair in redWeights)
            {
                Console.WriteLine($"Index: {pair.Key}, Weight: {pair.Value}");
            }

            MessageBox.Show("Red weights printed to console!");
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}