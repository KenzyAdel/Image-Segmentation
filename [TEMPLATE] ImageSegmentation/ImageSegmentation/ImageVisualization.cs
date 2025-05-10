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
    public partial class vsualization_ : Form
    {
        public vsualization_()
        {
            InitializeComponent();
        }

        private RGBPixel[,] ImageMatrix;
        private Dictionary<long, List<long>> componentPixels;
        private int rows;
        private int columns;

        
        public void SetImageMatrix(RGBPixel[,] matrix)
        {
            ImageMatrix = matrix;
            rows = matrix.GetLength(0);  
            columns = matrix.GetLength(1); 
        }

        public void SetComponentData(Dictionary<long, List<long>> components)
        {
            componentPixels = components ;
        }

        private void btnVis_Click(object sender, EventArgs e)
        {
            

                txtWidth.Text = columns.ToString();
                txtHeight.Text = rows.ToString();
                Color[,] colorArray = ColorComponentsTo2DArray(componentPixels, columns, rows);
                RGBPixel[,] rgbPixelArray = ConvertColorArrayToRgbPixelArray(colorArray);
                ImageOperations.DisplayImage(rgbPixelArray, pictureBox1);
            
           
        }

        public Color[,] ColorComponentsTo2DArray(Dictionary<long, List<long>> componentPixels, int width, int height)
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
    }
}