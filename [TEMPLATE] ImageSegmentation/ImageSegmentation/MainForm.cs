using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
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
            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

           
            GRAPH graph = new GRAPH(ImageMatrix);
            Dictionary<long, List<Tuple<long, int>>> Red_Weights = graph.Red_Weight();
            Dictionary<long, List<Tuple<long, int>>> Blue_Weight = graph.Blue_Weight();
            Dictionary<long, List<Tuple<long, int>>> Green_Weight = graph.Green_Weight();

            Segmentation segmentation = new Segmentation(ImageMatrix);
            Dictionary<long, List<long>> components = segmentation.SegmentImage(Red_Weights, Blue_Weight, Green_Weight);
            
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            vsualization_ vis = new vsualization_();
            vis.SetImageMatrix(ImageMatrix);
            vis.SetComponentData(components);
            vis.Show();
            this.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}