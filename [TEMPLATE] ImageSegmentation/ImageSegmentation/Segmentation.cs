using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace ImageTemplate
{
    public class Segmentation
    {
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;
        public long M;
 
        public Dictionary<int, List<int>> _componentPixels;
        public static int [] red_member;
        public static int [] green_member;
        public static int [] blue_member;
        public List<List<(int, int, int)>> red_edges;
        public List<List<(int, int, int)>> green_edges;
        public List<List<(int, int, int)>> blue_edges;
        Internal_Difference internal_Difference;
        public Segmentation(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            M = rows * columns;
            _componentPixels = new Dictionary<int, List<int>>();
            red_member = new int[M];
            blue_member = new int[M];
            green_member = new int[M];
            
            red_edges = new List<List<(int, int, int)>>();
            green_edges = new List<List<(int, int, int)>>();
            blue_edges = new List<List<(int, int, int)>>();

            for (int i = 0; i < M; i++)
            {
                Make_Set(i, red_member);
                Make_Set(i, blue_member);
                Make_Set(i, green_member);
            }
        }
       
        public void Make_Set(int x,int[] member)
        {
           member[x] = x;
        }
        public int Find_set(int x,int []member)
        {
            if (member[x] != x)
                member[x] = Find_set(member[x],member);
            return member[x];
        }
      
       
        private void union(int x, int y,int weight,int []member)
        { 

            int root_x = Find_set(x, member);
            int root_y = Find_set(y, member);
            if(root_x>root_y)
                member[root_x] = root_y;
            else
                member[root_y] = root_x;
        }
        private int choose_color(char color,int i,int j,int i2,int j2)
        {
            if (color == 'r')
                return Math.Abs(imageMatrix[i, j].red - imageMatrix[i2, j2].red);
            else if (color == 'b')
                return Math.Abs(imageMatrix[i, j].blue - imageMatrix[i2, j2].blue);
            else
                return Math.Abs(imageMatrix[i, j].green - imageMatrix[i2, j2].green);



        }
        //public List<(int, int, int)> edges(List<int> component,char color)
        //{
        //    HashSet<int> componentSet = new HashSet<int>(component);
        //    List<(int v1, int v2, int w)> uniqueEdges = new List<(int v1, int v2, int w)>();
        //    HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

        //    foreach (int v1 in component)
        //    {
        //        int i = (v1 / columns);
        //        int j = (v1 % columns);

        //        //Top - Left Neighbor
        //        if (i - 1 >= 0 && j - 1 >= 0 )
        //        {
        //            int Index = (i - 1) * columns + (j - 1);
        //            if (red_member[v1] == red_member[Index])
        //            {

        //                int weight = choose_color(color, i, j, i - 1, j - 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Top Neighbor
        //        if (i - 1 >= 0)
        //        {
        //            int Index = (i - 1) * columns + j;
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i - 1, j);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Top-Right Neighbor
        //        if (i - 1 >= 0 && j + 1 < columns)
        //        {
        //            int Index = (i - 1) * columns + (j + 1);
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i - 1, j + 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Left Neighbor
        //        if (j - 1 >= 0)
        //        {
        //            int Index = i * columns + (j - 1);
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i, j - 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Right Neighbor
        //        if (j + 1 < columns)
        //        {
        //            int Index = i * columns + (j + 1);
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i, j + 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Bottom-Left Neighbor
        //        if (i + 1 < rows && j - 1 >= 0)
        //        {
        //            int Index = (i + 1) * columns + (j - 1);
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i + 1, j - 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Bottom Neighbor
        //        if (i + 1 < rows)
        //        {
        //            int Index = (i + 1) * columns + j;
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i + 1, j);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //        // Bottom-Right Neighbor
        //        if (i + 1 < rows && j + 1 < columns)
        //        {
        //            int Index = (i + 1) * columns + (j + 1);
        //            if (componentSet.Contains(Index))
        //            {
        //                int weight = choose_color(color, i, j, i + 1, j + 1);
        //                int a = Math.Min(v1, Index);
        //                int b = Math.Max(v1, Index);

        //                if (addedEdges.Add((a, b)))
        //                {
        //                    uniqueEdges.Add((a, b, weight));
        //                }
        //            }
        //        }

        //    }

        
        //    uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
        //    //internal_Difference.CalculateInternalDifference(uniqueEdges);
        //    return ;
        //}

        ////public void Red_Segment2()
        ////{

        ////    for (int i = 0; i < rows; i++)
        ////    {
        ////        for (int j = 0; j < columns; j++)
        ////        {
        ////            int currentIndex = i * columns + j;

        ////            //Top - Left Neighbor
        ////            if (i - 1 >= 0 && j - 1 >= 0)
        ////            {
        ////                int Index = (i - 1) * columns + (j - 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);

        ////                red_edge2.Add((currentIndex,Index,weight));
        ////                // union()


        ////            }

        ////            // Top Neighbor
        ////            if (i - 1 >= 0)
        ////            {
        ////                int Index = (i - 1) * columns + j;
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j].red);

        ////                red_edge2.Add((currentIndex, Index, weight));


        ////                // union()

        ////            }

        ////            // Top-Right Neighbor
        ////            if (i - 1 >= 0 && j + 1 < columns)
        ////            {
        ////                int Index = (i - 1) * columns + (j + 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);

        ////                red_edge2.Add((currentIndex, Index, weight));

        ////                // union()

        ////            }

        ////            // Left Neighbor
        ////            if (j - 1 >= 0)
        ////            {
        ////                int Index = i * columns + (j - 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);

        ////                red_edge2.Add((currentIndex, Index, weight));


        ////                // union()

        ////            }

        ////            // Right Neighbor
        ////            if (j + 1 < columns)
        ////            {
        ////                int Index = i * columns + (j + 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);

        ////                red_edge2.Add((currentIndex, Index, weight));

        ////                // union()

        ////            }

        ////            // Bottom-Left Neighbor
        ////            if (i + 1 < rows && j - 1 >= 0)
        ////            {
        ////                int Index = (i + 1) * columns + (j - 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
        ////                red_edge2.Add((currentIndex, Index, weight));

        ////            }

        ////            // Bottom Neighbor
        ////            if (i + 1 < rows)
        ////            {
        ////                int Index = (i + 1) * columns + j;
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);

        ////                red_edge2.Add((currentIndex, Index, weight));

        ////                // union()

        ////            }

        ////            // Bottom-Right Neighbor
        ////            if (i + 1 < rows && j + 1 < columns)
        ////            {
        ////                int Index = (i + 1) * columns + (j + 1);
        ////                int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);

        ////                red_edge2.Add((currentIndex, Index, weight));

        ////                // union()

        ////            }
        ////        }

        ////    }
        ////}
        public void Red_Segment()
        {

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;

                    //Top - Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j - 1].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        

                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);
                                union(low, high, weight, red_member);
                            }

                            // union()
                        
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        int Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i - 1, j + 1].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        int Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j - 1].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }

                            // union()
                        
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        if (weight < 1)
                        {
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        if(weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        if (weight < 1)
                            if (Find_set(Index, red_member) != Find_set(currentIndex, red_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, red_member);
                            }
                            // union()
                        
                    }
                }

            }

            //Dictionary<int,Tuple<int,List<int>>> componentsList = new Dictionary<int, Tuple<int, List<int>>>();
            //for (int i = 0; i < rows * columns; i++)
            //{
            //    if (!componentsList.ContainsKey(member[i]))
            //        componentsList.Add(member[i],new Tuple<int, List<int>>(weights[Find_set(i)],new List<int>()));
            //    else
            //        componentsList[member[i]].Item2.Add(i);
            //}

            //  return componentsList;
        }

        public void Blue_Segment()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;

                    //Top - Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j - 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        

                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j].blue);
                        if (weight < 1)
                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }

                            // union()
                        
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        int Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i - 1, j + 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        int Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j - 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);

                            }

                            // union()
                        
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j + 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j - 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j].blue);
                        if (weight < 1)
                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j + 1].blue);
                        if (weight < 1)

                            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, blue_member);
                            }
                            // union()
                        
                    }
                }
            }
        }

        public void Green_Segment()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;

                    //Top - Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j - 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                        
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        int Index = (i - 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i - 1, j + 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                        
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        int Index = i * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j - 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int Index = i * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j + 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int Index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j - 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                            // union()
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int Index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);

                                union(low, high, weight, green_member);
                            }
                        }
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int Index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j + 1].green);
                        if (weight < 1)
                        {
                            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
                            {
                                int low = Math.Min(Index, currentIndex);
                                int high = Math.Max(Index, currentIndex);
                                union(low, high, weight, green_member);
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<int, List<int>> GetCombinedComponents()
        {
            Dictionary<int, List<int>> Red_component = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> Blue_component = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> Green_component = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> Different_component = new Dictionary<int, List<int>>();

            for (int pixel = 0; pixel < M; pixel++)
            {
                int redId = Find_set(pixel, red_member);
                int greenId = Find_set(pixel, green_member);
                int blueId = Find_set(pixel, blue_member);

                int componentId = redId;
                bool isValidComponent = false;
                if ((redId == greenId && redId == blueId)) // All match
                {
                    isValidComponent = true;
                    componentId = redId;
                }

                if (isValidComponent)
                {
                    if (!_componentPixels.ContainsKey(componentId))
                        _componentPixels[componentId] = new List<int>();

                    if (!_componentPixels[componentId].Contains(pixel))
                        _componentPixels[componentId].Add(pixel);
                }
                else
                {
                    if (redId == greenId && redId != blueId)
                    {
                        if (!Blue_component.ContainsKey(blueId))
                            Blue_component[blueId] = new List<int>();

                        Blue_component[blueId].Add(pixel);

                    }
                    else if (redId == blueId && redId != greenId)// Red and Green
                    {
                        if (!Green_component.ContainsKey(greenId))
                            Green_component[greenId] = new List<int>();

                        Green_component[greenId].Add(pixel);
                    }
                    else if ((greenId == blueId && greenId != redId))
                    {
                        if (!Red_component.ContainsKey(redId))
                            Red_component[redId] = new List<int>();

                        Red_component[redId].Add(pixel);
                        //Console.WriteLine("HEyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
                    }
                    else
                    {
                        if (!Different_component.ContainsKey(blueId))
                            Different_component[blueId] = new List<int>();

                        Different_component[blueId].Add(pixel);
                    }
                }
            }
            Console.WriteLine("RED Components: ");
            foreach (var v in Red_component)
                Console.WriteLine(v.Value.Count);
            Console.WriteLine("Green Components: ");
            foreach (var v in Green_component)
                Console.WriteLine(v.Value.Count);

            int red_key = 10000000;
            int blue_key = 20000000;
            int green_key = 30000000;
            int different_key = 40000000;
            foreach (var v in Red_component)
            {
                if (_componentPixels.ContainsKey(red_key + red_member[v.Value[0]]))
                    MessageBox.Show("Red Here!");

                _componentPixels[red_key + red_member[v.Value[0]]] = new List<int>(v.Value);
            }
            foreach (var v in Green_component)
            {
                if (_componentPixels.ContainsKey(green_key + green_member[v.Value[0]]))
                    MessageBox.Show("Green Here!");
                _componentPixels[green_key + green_member[v.Value[0]]] = new List<int>(v.Value);

            }
            foreach (var v in Blue_component)
            {
                if (_componentPixels.ContainsKey(blue_key + blue_member[v.Value[0]]))
                    MessageBox.Show("Blue Here!");

                _componentPixels[blue_key + blue_member[v.Value[0]]] = new List<int>(v.Value);

            }
            foreach (var v in Different_component)
            {
                if (_componentPixels.ContainsKey(different_key + blue_member[v.Value[0]]))
                    MessageBox.Show("Blue Here!");

                _componentPixels[different_key + blue_member[v.Value[0]]] = new List<int>(v.Value);

                Console.WriteLine("\n=== Components and Pixel Counts ===");
                foreach (var component in _componentPixels)
                {
                    long componentId = component.Key;
                    List<long> pixels = new List<long>();
                    int pixelCount = 0;

                    // Collect pixels with non-zero intensities and count them


                    // Print component details
                    Console.Write($"Component {componentId}: ");
                    //bool first = true;
                    /*foreach (long pixel in pixels)
                    {
                        if (!first) Console.Write(", ");
                        Console.Write($"arr[{pixel}] = {intensities[pixel]}");
                        first = false;
                    }*/
                    Console.WriteLine($" (Count: {component.Value.Count})");
                    //}


                    // }
                }
                //Blue_component=null; 
                //Red_component=null;
                //Green_component=null;

            }
            return _componentPixels;
        }
        public void SegmentImage()
        {

            this.Blue_Segment();
            this.Green_Segment();
            this.Red_Segment();

            Dictionary<int, List<int>> storered = new Dictionary<int, List<int>>();
            for (int a = 0; a < red_member.Length; a++)
            {
                if (!storered.ContainsKey(red_member[a]))
                    storered[red_member[a]] = new List<int>();
                storered[red_member[a]].Add(a);

            }
            Dictionary<int, List<int>> storegreen = new Dictionary<int, List<int>>();
            for (int a = 0; a < green_member.Length; a++)
            {
                if (!storegreen.ContainsKey(green_member[a]))
                    storegreen[green_member[a]] = new List<int>();
                storegreen[green_member[a]].Add(a);
            }
            Dictionary<int, List<int>> storeblue = new Dictionary<int, List<int>>();
            for (int a = 0; a < blue_member.Length; a++)
            {
                if (!storeblue.ContainsKey(blue_member[a]))
                    storeblue[blue_member[a]] = new List<int>();
                storeblue[blue_member[a]].Add(a);
            }
            Console.WriteLine("RED Components: ");
            foreach (var v in storered)
                Console.WriteLine(v.Value.Count);

            Console.WriteLine("Green Components: ");
            foreach (var v in storegreen)
                Console.WriteLine(v.Value.Count);

            Console.WriteLine("Blue Components: ");
            foreach (var v in storeblue)
                Console.WriteLine(v.Value.Count);

            _componentPixels.Clear();
             GetCombinedComponents();
          
        }
    }
}