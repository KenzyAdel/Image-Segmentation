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

        public Dictionary<long, List<Tuple<long, int>>> Red_Weight()
        {
            Dictionary<long, List<Tuple<long, int>>> redGraph = new Dictionary<long, List<Tuple<long, int>>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    redGraph[currentIndex] = new List<Tuple<long, int>>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));

                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        redGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }
                }

            }

            // Console output to print the list of tuples directly
             Console.WriteLine("\nRed Channel Weight Graph:");
             Console.WriteLine("(Format: Pixel [row,col] → List of Tuples (Neighbor Index, Weight))");
             Console.WriteLine(new string('=', 60));

             foreach (var entry in redGraph.OrderBy(e => e.Key))
             {
                 long index = entry.Key;
                 int row = (int)(index / columns);
                 int col = (int)(index % columns);
                 Console.Write($"[{row},{col}] → ");

                 // Print the list of tuples (neighbor index, weight)
                 Console.WriteLine("[ " + string.Join(", ", entry.Value.Select(t => $"({t.Item1}, {t.Item2})")) + " ]");

                 Console.WriteLine(new string('-', 60));
             }

            return redGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public Dictionary<long, List<Tuple<long, int>>> Blue_Weight()
        {
            Dictionary<long, List<Tuple<long, int>>> blueGraph = new Dictionary<long, List<Tuple<long, int>>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    blueGraph[currentIndex] = new List<Tuple<long, int>>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j - 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j + 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j - 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j + 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j - 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j + 1].blue);
                        blueGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }
                }

            }

            // Console output to print the list of tuples directly
            /*Console.WriteLine("\nRed Channel Weight Graph:");
            Console.WriteLine("(Format: Pixel [row,col] → List of Tuples (Neighbor Index, Weight))");
            Console.WriteLine(new string('=', 60));

            foreach (var entry in blueGraph.OrderBy(e => e.Key))
            {
                long index = entry.Key;
                int row = (int)(index / columns);
                int col = (int)(index % columns);
                Console.Write($"[{row},{col}] → ");

                // Print the list of tuples (neighbor index, weight)
                Console.WriteLine("[ " + string.Join(", ", entry.Value.Select(t => $"({t.Item1}, {t.Item2})")) + " ]");

                Console.WriteLine(new string('-', 60));
            }*/

            return blueGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public Dictionary<long, List<Tuple<long, int>>> Green_Weight()
        {
            Dictionary<long, List<Tuple<long, int>>> greenGraph = new Dictionary<long, List<Tuple<long, int>>>();
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    greenGraph[currentIndex] = new List<Tuple<long, int>>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j - 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j + 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j - 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j + 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j - 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j + 1].green);
                        greenGraph[currentIndex].Add(new Tuple<long, int>(Index, weight));
                    }
                }

            }

            // Console output to print the list of tuples directly
            /* Console.WriteLine("\nRed Channel Weight Graph:");
             Console.WriteLine("(Format: Pixel [row,col] → List of Tuples (Neighbor Index, Weight))");
             Console.WriteLine(new string('=', 60));

             foreach (var entry in greenGraph.OrderBy(e => e.Key))
             {
                 long index = entry.Key;
                 int row = (int)(index / columns);
                 int col = (int)(index % columns);
                 Console.Write($"[{row},{col}] → ");

                 // Print the list of tuples (neighbor index, weight)
                 Console.WriteLine("[ " + string.Join(", ", entry.Value.Select(t => $"({t.Item1}, {t.Item2})")) + " ]");

                 Console.WriteLine(new string('-', 60));
             }*/

            return greenGraph.OrderBy(e => e.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
