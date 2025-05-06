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
        public void Build_Graph_Weights(Dictionary<long, List<Tuple<long, int>>> Red_Weight, 
                                        Dictionary<long, List<Tuple<long, int>>> Green_Weight, 
                                        Dictionary<long, List<Tuple<long, int>>> Blue_Weight)
        {
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;
                    RGBPixel currentPixel = imageMatrix[i, j];
                    RGBPixel neighborPixel;
                    Red_Weight[currentIndex] = new List<Tuple<long, int>>();
                    Green_Weight[currentIndex] = new List<Tuple<long, int>>();
                    Blue_Weight[currentIndex] = new List<Tuple<long, int>>();

                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long neighbor_Index = (i - 1) * columns + (j - 1);
                        neighborPixel = imageMatrix[i - 1, j - 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long neighbor_Index = (i - 1) * columns + j;
                        neighborPixel = imageMatrix[i - 1, j];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long neighbor_Index = (i - 1) * columns + (j + 1);
                        neighborPixel = imageMatrix[i - 1, j + 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long neighbor_Index = i * columns + (j - 1);
                        neighborPixel = imageMatrix[i, j - 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long neighbor_Index = i * columns + (j + 1);
                        neighborPixel = imageMatrix[i, j + 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long neighbor_Index = (i + 1) * columns + (j - 1);
                        neighborPixel = imageMatrix[i + 1, j - 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long neighbor_Index = (i + 1) * columns + j;
                        neighborPixel = imageMatrix[i + 1, j];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long neighbor_Index = (i + 1) * columns + (j + 1);
                        neighborPixel = imageMatrix[i + 1, j + 1];

                        int weight = Math.Abs(currentPixel.red - neighborPixel.red);
                        Red_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.green - neighborPixel.green);
                        Green_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));

                        weight = Math.Abs(currentPixel.blue - neighborPixel.blue);
                        Blue_Weight[currentIndex].Add(new Tuple<long, int>(neighbor_Index, weight));
                    }
                }

            }
            Red_Weight = Red_Weight.OrderBy(entryPair => entryPair.Key)
                        .ToDictionary(entryPair => entryPair.Key, entryPair => entryPair.Value);

            Green_Weight = Green_Weight.OrderBy(entryPair => entryPair.Key)
                        .ToDictionary(entryPair => entryPair.Key, entryPair => entryPair.Value);

            Blue_Weight = Blue_Weight.OrderBy(entryPair => entryPair.Key)
                        .ToDictionary(entryPair => entryPair.Key, entryPair => entryPair.Value);


            Console.WriteLine("\nBlue Channel Weight Graph:");
            Console.WriteLine("(Format: Pixel [row,col] → List of Tuples (Neighbor Index, Weight))");
            Console.WriteLine(new string('=', 60));

            foreach (var entry in Blue_Weight)
            {
                long index = entry.Key;
                int row = (int)(index / columns);
                int col = (int)(index % columns);
                Console.Write($"[{row},{col}] → ");

                // Print the list of tuples (neighbor index, weight)
                Console.WriteLine("[ " + string.Join(", ", entry.Value.Select(t => $"({t.Item1}, {t.Item2})")) + " ]");

                Console.WriteLine(new string('-', 60));
            }
        }

    }
}
