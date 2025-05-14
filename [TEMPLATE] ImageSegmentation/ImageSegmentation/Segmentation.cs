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
        public float k=1;
        public Dictionary<int, List<int>> _componentPixels;
        public static int [] red_member;
        public static int [] green_member;
        public static int [] blue_member;
        public List<(int a, int b, int w)> red_edges;
        public List<(int a, int b, int w)> green_edges;
        public List<(int a, int b, int w)> blue_edges;
        Internal_Difference internal_Difference;
        public int[] red_size;
        public int[] green_size;
        public int[] blue_size;
        public int[] red_internal_Difference;
        public int[] blue_internal_Difference;
        public int[] green_internal_Difference;
        public int[] final_member;
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
            
            red_edges = new List<(int a, int b, int w)>();
            green_edges = new List<(int a, int b, int w)>();
            blue_edges = new List<(int a, int b, int w)>();
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
        }


        public void constructEdges()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentIndex = i * columns + j;
                    
                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int Index = i * columns + (j + 1);
                        int red_weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i, j + 1].red);
                        int blue_weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i, j + 1].blue);
                        int green_weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i, j + 1].green);
                        red_edges.Add((currentIndex, Index, red_weight));
                        blue_edges.Add((currentIndex, Index, blue_weight));
                        green_edges.Add((currentIndex, Index, green_weight));
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int Index = (i + 1) * columns + (j - 1);
                        int red_weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j - 1].red);
                        int blue_weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j - 1].blue);
                        int green_weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j - 1].green);
                        red_edges.Add((currentIndex, Index, red_weight));
                        blue_edges.Add((currentIndex, Index, blue_weight));
                        green_edges.Add((currentIndex, Index, green_weight));
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int Index = (i + 1) * columns + j;
                        int red_weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j].red);
                        int blue_weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j].blue);
                        int green_weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j].green);
                        red_edges.Add((currentIndex, Index, red_weight));
                        blue_edges.Add((currentIndex, Index, blue_weight));
                        green_edges.Add((currentIndex, Index, green_weight));
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int Index = (i + 1) * columns + (j + 1);
                        int red_weight = Math.Abs(imageMatrix[i, j].red - imageMatrix[i + 1, j + 1].red);
                        int blue_weight = Math.Abs(imageMatrix[i, j].blue - imageMatrix[i + 1, j + 1].blue);
                        int green_weight = Math.Abs(imageMatrix[i, j].green - imageMatrix[i + 1, j + 1].green);
                        red_edges.Add((currentIndex, Index, red_weight));
                        blue_edges.Add((currentIndex, Index, blue_weight));
                        green_edges.Add((currentIndex, Index, green_weight));
                    }
                }
            }

            red_edges.Sort((a, b) => a.w.CompareTo(b.w));
            blue_edges.Sort((a, b) => a.w.CompareTo(b.w));
            green_edges.Sort((a, b) => a.w.CompareTo(b.w));
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
            if (root_x > root_y)
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
        private void union(int x, int y, int[] member)
        {
            int root_x = Find_set(x, member);
            int root_y = Find_set(y, member);
            if (root_x > root_y)
            {
                member[root_x] = root_y;
            }
            // Update the size of the new root (root_y)            }
            else
            {
                member[root_y] = root_x;
            }

        }
        public int CalculateNewInternal(int weight,int minInternal1,int minInternal2)
        {
            return Math.Max(weight, Math.Max(minInternal2, minInternal1));
        }
        public void disjoint_for_red(int currentIndex,int Index,int weight)
        {

            int Parent_of_currentIndex = Find_set(currentIndex,red_member);
            int Parent_of_index= Find_set(Index,red_member);
            
            
            if (Parent_of_currentIndex != Parent_of_index)
             {
                float thershold = Math.Max(k / (float)red_size[Parent_of_currentIndex], 0.1f);
                float thershold2 = Math.Max(k / (float)red_size[Parent_of_index], 0.1f);
                if (weight <= Math.Min(red_internal_Difference[Parent_of_currentIndex] + thershold, red_internal_Difference[Parent_of_index] + thershold2))
                {
                    union(Index, currentIndex, weight, red_member,red_size);
                    int newindex = Find_set(Index, red_member);
                    red_internal_Difference[newindex]=CalculateNewInternal(weight, red_internal_Difference[Parent_of_currentIndex], red_internal_Difference[Parent_of_index]);
                }
            }
        }
        public void disjoint_for_blue(int currentIndex,int Index,int weight)
        {
            int index = Find_set(currentIndex, blue_member);
            int index2 = Find_set(Index, blue_member);
            if (Find_set(Index, blue_member) != Find_set(currentIndex, blue_member))
            {
                float thershold = k / blue_size[index];
                float thershold2 = k / blue_size[index2];
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
                float thershold = k / green_size[index];
                float thershold2 = k / green_size[index2];
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
            foreach(var l in red_edges)
            {
                int currentIndex = l.a;
                int index = l.b;
                int weight = l.w;
                if(weight!=0)
                {
                    int i = 0;
                }
               disjoint_for_red(currentIndex, index, weight);
            }
            for (int i = 0; i < M; i++)
            {
                if (!_componentPixels.ContainsKey(red_member[i]))
                    _componentPixels[red_member[i]] = new List<int>();
                _componentPixels[red_member[i]].Add(i);
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
                if (red_member[l.a] == red_member[l.b] && blue_member[l.a] == blue_member[l.b] && green_member[l.a] == green_member[l.b])
                {
                    if (Find_set(l.a, final_member) != Find_set(l.b, final_member))
                        union(l.a, l.b, final_member);
                }
            }
            //for (int i = 0; i < M; i++)
            //{
            //    if (!_componentPixels.ContainsKey(final_member[i]))
            //        _componentPixels[final_member[i]] = new List<int>();
            //    _componentPixels[final_member[i]].Add(i);
            //}

        }

        //public Dictionary<int, List<int>> GetCombinedComponents()
        //{
        //    Dictionary<int, List<int>> Red_component = new Dictionary<int, List<int>>();
        //    Dictionary<int, List<int>> Blue_component = new Dictionary<int, List<int>>();
        //    Dictionary<int, List<int>> Green_component = new Dictionary<int, List<int>>();
        //    Dictionary<int, List<int>> Different_component = new Dictionary<int, List<int>>();

        //    for (int pixel = 0; pixel < M; pixel++)
        //    {
        //        int redId = Find_set(pixel, red_member);
        //        int greenId = Find_set(pixel, green_member);
        //        int blueId = Find_set(pixel, blue_member);

        //        int componentId = redId;
        //        bool isValidComponent = false;
        //        if ((redId == greenId && redId == blueId)) // All match
        //        {
        //            isValidComponent = true;
        //            componentId = redId;
        //        }

        //        if (isValidComponent)
        //        {
        //            if (!_componentPixels.ContainsKey(componentId))
        //                _componentPixels[componentId] = new List<int>();

        //            if (!_componentPixels[componentId].Contains(pixel))
        //                _componentPixels[componentId].Add(pixel);
        //        }
        //        else
        //        {
        //            if (redId == greenId && redId != blueId)
        //            {
        //                if (!Blue_component.ContainsKey(blueId))
        //                    Blue_component[blueId] = new List<int>();

        //                Blue_component[blueId].Add(pixel);

        //            }
        //            else if (redId == blueId && redId != greenId)// Red and Green
        //            {
        //                if (!Green_component.ContainsKey(greenId))
        //                    Green_component[greenId] = new List<int>();

        //                Green_component[greenId].Add(pixel);
        //            }
        //            else if ((greenId == blueId && greenId != redId))
        //            {
        //                if (!Red_component.ContainsKey(redId))
        //                    Red_component[redId] = new List<int>();

        //                Red_component[redId].Add(pixel);
        //            }
        //            else
        //            {
        //           //     MessageBox.Show("Red weights printed to console!");
        //                if (!Different_component.ContainsKey(blueId))
        //                    Different_component[blueId] = new List<int>();

        //                Different_component[blueId].Add(pixel);
        //            }
        //        }
        //    }
        //    //Console.WriteLine("RED Components: ");
        //    //foreach (var v in Red_component)
        //    //    Console.WriteLine(v.Value.Count);
        //    //Console.WriteLine("Green Components: ");
        //    //foreach (var v in Green_component)
        //    //    Console.WriteLine(v.Value.Count);

        //    int red_key = 10000000;
        //    int blue_key = 20000000;
        //    int green_key = 30000000;
        //    int different_key = 40000000;
        //    foreach (var v in Red_component)
        //    {
        //        if (_componentPixels.ContainsKey(red_key + red_member[v.Value[0]]))
        //            MessageBox.Show("Red Here!");

        //        _componentPixels[red_key + red_member[v.Value[0]]] = new List<int>(v.Value);
        //    }
        //    foreach (var v in Green_component)
        //    {
        //        if (_componentPixels.ContainsKey(green_key + green_member[v.Value[0]]))
        //            MessageBox.Show("Green Here!");
        //        _componentPixels[green_key + green_member[v.Value[0]]] = new List<int>(v.Value);

        //    }
        //    foreach (var v in Blue_component)
        //    {
        //        if (_componentPixels.ContainsKey(blue_key + blue_member[v.Value[0]]))
        //            MessageBox.Show("Blue Here!");

        //        _componentPixels[blue_key + blue_member[v.Value[0]]] = new List<int>(v.Value);

        //    }
        //    foreach (var v in Different_component)
        //    {
        //        if (_componentPixels.ContainsKey(different_key + blue_member[v.Value[0]]))
        //            MessageBox.Show("Blue Here!");

        //        _componentPixels[different_key + blue_member[v.Value[0]]] = new List<int>(v.Value);

        //        //Console.WriteLine("\n=== Components and Pixel Counts ===");
        //        //Console.WriteLine(_componentPixels.Keys.Count);
        //        //foreach (var component in _componentPixels)
        //        //{
        //        //    long componentId = component.Key;
        //        //    List<long> pixels = new List<long>();

        //        //    // Collect pixels with non-zero intensities and count them

        //        //    // Print component details
        //        //    Console.Write($"Component {componentId}: ");
        //        //    //bool first = true;
        //        //    /*foreach (long pixel in pixels)
        //        //    {
        //        //        if (!first) Console.Write(", ");
        //        //        Console.Write($"arr[{pixel}] = {intensities[pixel]}");
        //        //        first = false;
        //        //    }*/
        //        //    Console.WriteLine($" (Count: {component.Value.Count})");
        //        //    //}


        //        //    // }
        //        //}
        //        //Blue_component=null; 
        //        //Red_component=null;
        //        //Green_component=null;

        //    }
        //    return _componentPixels;
        //}
        //public void SegmentImage()
        //{

        //    this.Blue_Segment();
        //    this.Green_Segment();
        //    this.Red_Segment();

        //    //Dictionary<int, List<int>> storered = new Dictionary<int, List<int>>();
        //    //for (int a = 0; a < red_member.Length; a++)
        //    //{
        //    //    if (!storered.ContainsKey(red_member[a]))
        //    //        storered[red_member[a]] = new List<int>();
        //    //    storered[red_member[a]].Add(a);

        //    //}
        //    //Dictionary<int, List<int>> storegreen = new Dictionary<int, List<int>>();
        //    //for (int a = 0; a < green_member.Length; a++)
        //    //{
        //    //    if (!storegreen.ContainsKey(green_member[a]))
        //    //        storegreen[green_member[a]] = new List<int>();
        //    //    storegreen[green_member[a]].Add(a);
        //    //}
        //    //Dictionary<int, List<int>> storeblue = new Dictionary<int, List<int>>();
        //    //for (int a = 0; a < blue_member.Length; a++)
        //    //{
        //    //    if (!storeblue.ContainsKey(blue_member[a]))
        //    //        storeblue[blue_member[a]] = new List<int>();
        //    //    storeblue[blue_member[a]].Add(a);
        //    //}
        //    //Console.WriteLine("RED Components: ");
        //    //foreach (var v in storered)
        //    //    Console.WriteLine(v.Value.Count);

        //    //Console.WriteLine("Green Components: ");
        //    //foreach (var v in storegreen)
        //    //    Console.WriteLine(v.Value.Count);

        //    //Console.WriteLine("Blue Components: ");
        //    //foreach (var v in storeblue)
        //    //    Console.WriteLine(v.Value.Count);

        //    _componentPixels.Clear();
        //     GetCombinedComponents();
          
        //}
    }
}