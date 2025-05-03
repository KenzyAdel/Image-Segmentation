using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageTemplate
{
    public class GRAPH
    {

        private RGBPixel[,] imageMatrix;

        public GRAPH(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
        }

        public Dictionary<long, List<int>> Red_Weight()
        {
            Dictionary<long, List<int>> redGraph = new Dictionary<long, List<int>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    redGraph[currentIndex] = new List<int>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j].red));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        redGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red));
                    }
                }

            }

            Console.WriteLine("\nRed Channel Weight Graph:");
            Console.WriteLine("(Format: Pixel [row,col] → Neighbor Directions with Weights)");
            Console.WriteLine(new string('=', 60));
            string[] directions = { "Top-Left", "Top", "Top-Right", "Left", "Right", "Bottom-Left", "Bottom", "Bottom-Right" };
            foreach (var entry in redGraph.OrderBy(e => e.Key))
            {
                long index = entry.Key;
                int row = (int)(index / columns);
                int col = (int)(index % columns);
                Console.Write($"[{row},{col}]: ");
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    Console.Write($"{directions[i]}({entry.Value[i]})");
                    if (i < entry.Value.Count - 1) Console.Write(", ");
                }

                Console.WriteLine($"\n{new string('-', 60)}");
            }

            return redGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public Dictionary<long, List<int>> Blue_Weight()
        {
            Dictionary<long, List<int>> blueGraph = new Dictionary<long, List<int>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    blueGraph[currentIndex] = new List<int>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j - 1].blue));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j].blue));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j + 1].blue));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j - 1].blue));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j + 1].blue));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j - 1].blue));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j].blue));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        blueGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j + 1].blue));
                    }
                }

            }

            /*Console.WriteLine("\nRed Channel Weight Graph:");
            Console.WriteLine("(Format: Pixel [row,col] → Neighbor Directions with Weights)");
            Console.WriteLine(new string('=', 60));
            string[] directions = { "Top-Left", "Top", "Top-Right","Left", "Right","Bottom-Left", "Bottom", "Bottom-Right" };
            foreach (var entry in redGraph.OrderBy(e => e.Key))
            {
                long index = entry.Key;
                int row = (int)(index / columns);
                int col = (int)(index % columns);
                Console.Write($"[{row},{col}]: ");
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    Console.Write($"{directions[i]}({entry.Value[i]})");
                    if (i < entry.Value.Count - 1) Console.Write(", ");
                }

                Console.WriteLine($"\n{new string('-', 60)}");
            }*/

            return blueGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public Dictionary<long, List<int>> Green_Weight()
        {
            Dictionary<long, List<int>> greenGraph = new Dictionary<long, List<int>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    greenGraph[currentIndex] = new List<int>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j - 1].green));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j].green));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j + 1].green));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j - 1].green));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j + 1].green));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j - 1].green));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j].green));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        greenGraph[currentIndex].Add(Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j + 1].green));
                    }
                }

            }

            /*Console.WriteLine("\nRed Channel Weight Graph:");
            Console.WriteLine("(Format: Pixel [row,col] → Neighbor Directions with Weights)");
            Console.WriteLine(new string('=', 60));
            string[] directions = { "Top-Left", "Top", "Top-Right","Left", "Right","Bottom-Left", "Bottom", "Bottom-Right" };
            foreach (var entry in redGraph.OrderBy(e => e.Key))
            {
                long index = entry.Key;
                int row = (int)(index / columns);
                int col = (int)(index % columns);
                Console.Write($"[{row},{col}]: ");
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    Console.Write($"{directions[i]}({entry.Value[i]})");
                    if (i < entry.Value.Count - 1) Console.Write(", ");
                }

                Console.WriteLine($"\n{new string('-', 60)}");
            }*/

            return greenGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
