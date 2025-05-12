using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate
{
    public class GRAPH
    {
        private RGBPixel[,] imageMatrix;
        int[] parent;
        public GRAPH(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            parent = new int[imageMatrix.GetLength(0)*imageMatrix.GetLength(1)];
        }

        
        public List<(int v1, int v2, int w)> Red_Weight()
        {
            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            List<(int v1, int v2, int w)> uniqueEdges = new List<(int v1, int v2, int w)>();
            HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;


                    // Top-Left Neighbor
                    //if (i - 1 >= 0 && j - 1 >= 0)
                    //{
                    //    long Index = (i - 1) * columns + (j - 1);
                    //    int weight = 0;//Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                    //    int a = (int)Math.Min(currentIndex, Index);
                    //    int b = (int)Math.Max(currentIndex, Index);

                    //    if (addedEdges.Add((a, b)))
                    //    {
                    //        uniqueEdges.Add((a, b, weight));
                    //    }

                    //}

                    //// Top Neighbor
                    //if (i - 1 >= 0)
                    //{
                    //    long Index = (i - 1) * columns + j;
                    //    int weight = 0;//Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j].red);
                    //    int a = (int)Math.Min(currentIndex, Index);
                    //    int b = (int)Math.Max(currentIndex, Index);

                    //    if (addedEdges.Add((a, b)))
                    //    {
                    //        uniqueEdges.Add((a, b, weight));
                    //    }
                    //}

                    //// Top-Right Neighbor
                    //if (i - 1 >= 0 && j + 1 < columns)
                    //{
                    //    long Index = (i - 1) * columns + (j + 1);
                    //    int weight = 0;//Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);
                    //    int a = (int)Math.Min(currentIndex, Index);
                    //    int b = (int)Math.Max(currentIndex, Index);

                    //    if (addedEdges.Add((a, b)))
                    //    {
                    //        uniqueEdges.Add((a, b, weight));
                    //    }
                    //}

                    //// Left Neighbor
                    //if (j - 1 >= 0)
                    //{
                    //    long Index = i * columns + (j - 1);
                    //    int weight = 0; //Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);
                    //    int a = (int)Math.Min(currentIndex, Index);
                    //    int b = (int)Math.Max(currentIndex, Index);

                    //    if (addedEdges.Add((a, b)))
                    //    {
                    //        uniqueEdges.Add((a, b, weight));
                    //    }
                    //}

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = 0;// Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        int a = (int)Math.Min(currentIndex, Index);
                        int b = (int)Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = 0;// Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        int a = (int)Math.Min(currentIndex, Index);
                        int b = (int)Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = 0;//Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        int a = (int)Math.Min(currentIndex, Index);
                        int b = (int)Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = 0;// Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        int a = (int)Math.Min(currentIndex, Index);
                        int b = (int)Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }
             //   Console.WriteLine(
               // uniqueEdges.Count());
            }
           // Console.WriteLine("Finished");
            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            return uniqueEdges;
        }

        // Console output to print the list of tuples directly
        /*Console.WriteLine("\nRed Channel Weight Graph:");
        Console.WriteLine("(Format: Pixel [row,col] → List of Tuples (Neighbor Index, Weight))");
        Console.WriteLine(new string('=', 60));*/

        /*foreach (var entry in redGraph.OrderBy(e => e.Key))
        {
            long index = entry.Key;
            int row = (int)(index / columns);
            int col = (int)(index % columns);
            Console.Write($"[{row},{col}] → ");

            // Print the list of tuples (neighbor index, weight)
            Console.WriteLine("[ " + string.Join(", ", entry.Value.Select(t => $"({t.Item1}, {t.Item2})")) + " ]");

            Console.WriteLine(new string('-', 60));
        }*/

    



        public List<(long v1, long v2, int w)> Blue_Weight()
        {

            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            var uniqueEdges = new List<(long v1, long v2, int w)>();
            var addedEdges = new HashSet<(long, long)>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;


                    //int a = Math.Min((int)currentIndex, vertex2);
                    //int b = Math.Max((int)currentIndex, vertex2);


                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }

                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }
            }
            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            return uniqueEdges;



        }


        public List<(long v1, long v2, int w)> Green_Weight()
        {

            int rows = imageMatrix.GetLength(0);
            int columns = imageMatrix.GetLength(1);

            var uniqueEdges = new List<(long v1, long v2, int w)>();
            var addedEdges = new HashSet<(long, long)>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    long currentIndex = i * columns + j;


                    //int a = Math.Min((int)currentIndex, vertex2);
                    //int b = Math.Max((int)currentIndex, vertex2);


                    // Top-Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }

                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        long Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        long Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        long Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        long Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        long Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        long Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        long Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        long a = Math.Min(currentIndex, Index);
                        long b = Math.Max(currentIndex, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }
            }
            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            return uniqueEdges;
        }
    }
}
