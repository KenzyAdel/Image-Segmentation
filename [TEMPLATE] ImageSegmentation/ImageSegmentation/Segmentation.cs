using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ImageTemplate
{
    public class Segmentation
    {
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;
        public long M;
        public double k;
        public Dictionary<int, List<int>> _componentPixels;
        public static int [] red_member;
        public static int [] green_member;
        public static int [] blue_member;
        public List<(int a, int b, short w)> red_edges;
        public List<(int a, int b, int w)> green_edges;
        public List<(int a, int b, int w)> blue_edges;
        public int[] red_size;
        public int[] green_size;
        public int[] blue_size;
        public int[] red_internal_Difference;
        public int[] blue_internal_Difference;
        public int[] green_internal_Difference;
        public int[] final_member;
        public Segmentation(RGBPixel[,] matrix , double KK)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            M = rows * columns;
            _componentPixels = new Dictionary<int, List<int>>();
            red_member = new int[M];
            blue_member = new int[M];
            green_member = new int[M];
            
            red_edges = new List<(int a, int b, short w)>((int)M * 4);
            green_edges = new List<(int a, int b, int w)>((int)M * 4);
            blue_edges = new List<(int a, int b, int w)>((int)M * 4);
            red_internal_Difference=new int[M];
            blue_internal_Difference = new int[M];
            green_internal_Difference = new int[M];
            red_size = new int[M];
            blue_size=new int[M];
            green_size=new int[M];
            final_member = new int[M];
            for (int i = 0; i < M; i++)
            {
                Make_Set(i, red_member,red_size,red_internal_Difference);
                Make_Set(i, blue_member,blue_size,blue_internal_Difference);
                Make_Set(i, green_member, green_size,green_internal_Difference);
                Make_Set(i, final_member);
            }
            k = KK;
        }

        public void ConstructRedEdges()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int index = currentIndex + 1;
                        short weight = (short)Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        red_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int index = (i + 1) * columns + (j - 1);
                        short weight = (short)Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        red_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int index = (i + 1) * columns + j;
                        short weight = (short)Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        red_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int index = (i + 1) * columns + (j + 1);
                        short weight = (short)Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        red_edges.Add((currentIndex, index, weight));
                    }
                }
            }
            red_edges.Sort((a, b) => a.w.CompareTo(b.w));

        }

        public void ConstructGreenEdges()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int index = currentIndex + 1;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j + 1].green);
                        green_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j - 1].green);
                        green_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j].green);
                        green_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j + 1].green);
                        green_edges.Add((currentIndex, index, weight));
                    }
                }
            }
            green_edges.Sort((a, b) => a.w.CompareTo(b.w));

        }

        public void ConstructBlueEdges()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;
                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int index = currentIndex + 1;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j + 1].blue);
                        blue_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int index = (i + 1) * columns + (j - 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j - 1].blue);
                        blue_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int index = (i + 1) * columns + j;
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j].blue);
                        blue_edges.Add((currentIndex, index, weight));
                    }
                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int index = (i + 1) * columns + (j + 1);
                        int weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j + 1].blue);
                        blue_edges.Add((currentIndex, index, weight));
                    }
                }
            }
            blue_edges.Sort((a, b) => a.w.CompareTo(b.w));
        }
       
        public void Make_Set(int x, int[] member)
        {
            member[x] = x;
        }
        public void Make_Set(int x,int[] member,int[] size,int[] internal_Difference)
        {
           member[x] = x;
           size[x] = 1;
           internal_Difference[x] = 0;
        }
        public int Find_set(int x,int []member)
        {
            if (member[x] != x)
                member[x] = Find_set(member[x],member);
            return member[x];
        }

        private void union(int x, int y, int weight, int[] member, int[] size)
        {
            int root_x = Find_set(x, member);
            int root_y = Find_set(y, member);
            
            if (root_x != root_y)
            {
                if (size[root_x] < size[root_y])
                {
                    member[root_x] = root_y;
                    size[root_y] += size[root_x];
                }
                else
                {
                    member[root_y] = root_x;
                    size[root_x] += size[root_y];
                }
            }

        }
        private void union(int x, int y, int[] member)
        {
            int root_x = Find_set(x, member);
            int root_y = Find_set(y, member);
            if (root_x > root_y)
                member[root_x] = root_y;
            
            else
                member[root_y] = root_x;
            

        }
        public int CalculateNewInternal(int weight,int minInternal1,int minInternal2)
        {
            return Math.Max(weight, Math.Max(minInternal2, minInternal1));
        }
        public void disjoint_for_red(int currentIndex,int Index,int weight)
        {

            int Parent_of_currentIndex = Find_set(currentIndex,red_member);
            int Parent_of_index= Find_set(Index,red_member);

            if (Parent_of_currentIndex == Parent_of_index) return;

            double thershold = k / red_size[Parent_of_currentIndex];
            double thershold2 = k / red_size[Parent_of_index];

            int diff1 = red_internal_Difference[Parent_of_currentIndex];
            int diff2 = red_internal_Difference[Parent_of_index];

            if (weight <= Math.Min(diff1 + thershold, diff2 + thershold2))
            {
                union(Parent_of_currentIndex, Parent_of_index, weight, red_member, red_size);
                int newRoot = Find_set(Parent_of_currentIndex, red_member);
                red_internal_Difference[newRoot] = CalculateNewInternal(weight, diff1, diff2);
            }
            
        }
        public void disjoint_for_blue(int currentIndex,int Index,int weight)
        {
            int index = Find_set(currentIndex, blue_member);
            int index2 = Find_set(Index, blue_member);
            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
            {
                double thershold = k / blue_size[index];
                double thershold2 = k / blue_size[index2];
                if (weight <= Math.Min(blue_internal_Difference[index] + thershold, blue_internal_Difference[index2] + thershold2))
                {
                    union(Index, currentIndex, weight, blue_member, blue_size);
                    int newindex = Find_set(Index, blue_member);
                    blue_internal_Difference[newindex] = CalculateNewInternal(weight, blue_internal_Difference[index], blue_internal_Difference[index2]);
                }
            }
        }
        public void disjoint_for_green(int currentIndex, int Index, int weight)
        {
            int index = Find_set(currentIndex, green_member);
            int index2 = Find_set(Index, green_member);
            if (Find_set(Index, green_member) != Find_set(currentIndex, green_member))
            {
                double thershold = k / green_size[index];
                double thershold2 = k / green_size[index2];
                if (weight <= Math.Min(green_internal_Difference[index] + thershold, green_internal_Difference[index2] + thershold2))
                {
                    union(Index, currentIndex, weight, green_member, green_size);
                    int newindex = Find_set(Index, green_member);
                    green_internal_Difference[newindex] = CalculateNewInternal(weight, green_internal_Difference[index], green_internal_Difference[index2]);
                }
            }
        }

        public void Red_Segment()
        {

            foreach (var l in red_edges)
            {
                int currentIndex = l.a;
                int index = l.b;
                int weight = l.w;
                /*if(weight!=0)
                {
                    int i = 0;
                }*/
               disjoint_for_red(currentIndex, index, weight);
            }
            
        }

        public void Blue_Segment()
        {
            foreach (var l in blue_edges)
            {
                int currentIndex = l.a;
                int index = l.b;
                int weight = l.w;
                disjoint_for_blue(currentIndex, index, weight);
            }
        }

        public void Green_Segment()
        {
            foreach (var l in green_edges)
            {
                int currentIndex = l.a;
                int index = l.b;
                int weight = l.w;
                disjoint_for_green(currentIndex, index, weight);
            }
        }

        public void Merge()
        {
            foreach (var l in red_edges)
            {
                int a = l.a;
                int b = l.b;
                if (Find_set(a, red_member) == Find_set(b, red_member) &&
                    Find_set(a, blue_member) == Find_set(b, blue_member) &&
                    Find_set(a, green_member) == Find_set(b, green_member))
                {
                    if (Find_set(a, final_member) != Find_set(b, final_member))
                        union(a, b, final_member);
                }

            }
            for (int i = 0; i < M; i++)
            {
                int root = Find_set(i, final_member);
                if (!_componentPixels.ContainsKey(root))
                    _componentPixels[root] = new List<int>();
                _componentPixels[root].Add(i);
            }
        }
    }
}