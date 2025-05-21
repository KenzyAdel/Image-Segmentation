using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        public MainForm()
        {
            InitializeComponent();
            AllocConsole();

        }

        RGBPixel[,] ImageMatrix;
        Segmentation segmentation;

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
            // Handle exception
            if (string.IsNullOrEmpty(K.Text))
            {
                MessageBox.Show("No K threshold is passed!");
                return;
            }
            if (!double.TryParse(K.Text, out double kthr))
            {
                MessageBox.Show("Invalid K value! Must be a number.");
                return;
            }

            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

            Stopwatch timer = Stopwatch.StartNew();
            Console.WriteLine("======Time Started=======");
            double k = Convert.ToDouble(K.Text);
            segmentation = new Segmentation(ImageMatrix , k);
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

            int rows = ImageOperations.GetHeight(ImageMatrix);
            int columns = ImageOperations.GetWidth(ImageMatrix);
            Color[,] colorArray = ColorComponentsTo2DArray(segmentation._componentPixels, columns, rows);
            RGBPixel[,] rgbPixelArray = ConvertColorArrayToRgbPixelArray(colorArray);
            ImageOperations.DisplayImage(rgbPixelArray, pictureBox2);

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
                writer.WriteLine(count);
                foreach (var pixel in pixels)
                {
                    writer.WriteLine(pixel); 
                }
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }
            //MessageBox.Show("Red weights printed to console!");
            // ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

        }
        public Color[,] ColorComponentsTo2DArray(Dictionary<int, List<int>> componentPixels, int width, int height)
        {
            Color[,] colorArray = new Color[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    colorArray[y, x] = Color.Black;

            Random random = new Random(componentPixels.Count);

            foreach (var component in componentPixels)
            {
                Color componentColor = GenerateDistinctColor(component.Key, random);

                if (component.Value == null) continue;

                foreach (int pixel in component.Value)
                {
                    int x = pixel % width;
                    int y = pixel / width;

                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        colorArray[y, x] = componentColor;
                    }
                }
            }

            return colorArray;
        }


        private HashSet<Color> usedColors = new HashSet<Color>();

        private Color GenerateDistinctColor(long componentId, Random random)
        {
            Color newColor;
            do
            {

                int r = random.Next(256);
                int g = random.Next(256);
                int b = random.Next(256);

                newColor = Color.FromArgb(r, g, b);

            } while (usedColors.Contains(newColor));

            usedColors.Add(newColor);
            return newColor;
        }


        private RGBPixel[,] ConvertColorArrayToRgbPixelArray(Color[,] colorArray)
        {
            int height = colorArray.GetLength(0);
            int width = colorArray.GetLength(1);
            RGBPixel[,] rgbArray = new RGBPixel[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = colorArray[y, x];
                    rgbArray[y, x] = new RGBPixel
                    {
                        red = color.R,
                        green = color.G,
                        blue = color.B
                    };
                }
            }

            return rgbArray;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MergeForm2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                // Create Form2 and pass the image
                Bonus2 bonus2 = new Bonus2(pictureBox2.Image, segmentation, ImageMatrix);
                bonus2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("No image in PictureBox! Apply gaussian then merge.");
            }
        }
    }
}