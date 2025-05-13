using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ImageTemplate
{
    public class Internal_Difference
    {
        Dictionary<int,int> member;
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;
        public Internal_Difference(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            member = new Dictionary<int, int>();
            for (int i = 0; i < rows*columns; i++)
                Make_Set(i);
            //member[i] = i;
        }
        public void Make_Set(int x)
        {
            member[x] = x;
        }
        public int Find_set(int x)
        {
            if (member[x] != x)
                member[x] = Find_set(member[x]);
            return member[x];
        }
        public void Union(int x, int y)
        {
            int xSet = Find_set(x);
            int ySet = Find_set(y);
            if (xSet != ySet)
            {
                member[xSet] = ySet;
            }
        }

        private int choose_color(char color, int i, int j, int i2, int j2)
        {
            if (color == 'r')
                return Math.Abs(imageMatrix[i, j].red - imageMatrix[i2, j2].red);
            else if (color == 'b')
                return Math.Abs(imageMatrix[i, j].blue - imageMatrix[i2, j2].blue);
            else
                return Math.Abs(imageMatrix[i, j].green - imageMatrix[i2, j2].green);



        }

        private int GetCompId(int Index)
        {
            int NeighborComponent;

            int red = Segmentation.red_member[Index];
            int blue = Segmentation.blue_member[Index];
            int green = Segmentation.green_member[Index];

            if (red == green && blue != red)
            {
                NeighborComponent = 20000000 + blue;
            }
            else if (green == blue && green != red)
            {
                NeighborComponent = 10000000 + red;
            }
            else if (red == blue && red != green)
            {
                NeighborComponent = 30000000 + green;
            }
            else
            {
                NeighborComponent = red;
            }
            return NeighborComponent;
        }

        public int CalculateInternalDifference_Red(List<int> component,char color)
        {
           
            int maxEdgeWeight = 0;
            int edgesAdded = 0;
            int vertices = component.Count;
            HashSet<int> componentSet = new HashSet<int>(component);
            List<(int v1, int v2, int w)> uniqueEdges = new List<(int v1, int v2, int w)>();
            HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

            foreach (int v1 in component)
            {
                int i = (v1 / columns);
                int j = (v1 % columns);

                //Top - Left Neighbor
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    int Index = (i - 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {

                        int weight = choose_color(color, i, j, i - 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top Neighbor
                if (i - 1 >= 0)
                {
                    int Index = (i - 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top-Right Neighbor
                if (i - 1 >= 0 && j + 1 < columns)
                {
                    int Index = (i - 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Left Neighbor
                if (j - 1 >= 0)
                {
                    int Index = i * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Right Neighbor
                if (j + 1 < columns)
                {
                    int Index = i * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Left Neighbor
                if (i + 1 < rows && j - 1 >= 0)
                {
                    int Index = (i + 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom Neighbor
                if (i + 1 < rows)
                {
                    int Index = (i + 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Right Neighbor
                if (i + 1 < rows && j + 1 < columns)
                {
                    int Index = (i + 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

            }


            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            foreach (var c in component)
            {
                Make_Set(c);
            }
            foreach (var edge in uniqueEdges)
            {
                if (Find_set(edge.v1) != Find_set(edge.v2))
                {
                    Union(edge.v1, edge.v2);
                    maxEdgeWeight = Math.Max(maxEdgeWeight, edge.w);
                    edgesAdded++;
                    if (edgesAdded == vertices - 1)
                        break;
                }
            }
         //   Console.WriteLine("RED");
            return maxEdgeWeight;
        }
        public int CalculateInternalDifference_Blue(List<int> component, char color)
        {

            int maxEdgeWeight = 0;
            int edgesAdded = 0;
            int vertices = component.Count;
            HashSet<int> componentSet = new HashSet<int>(component);
            List<(int v1, int v2, int w)> uniqueEdges = new List<(int v1, int v2, int w)>();
            HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

            foreach (int v1 in component)
            {
                int i = (v1 / columns);
                int j = (v1 % columns);

                //Top - Left Neighbor
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    int Index = (i - 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {

                        int weight = choose_color(color, i, j, i - 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top Neighbor
                if (i - 1 >= 0)
                {
                    int Index = (i - 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top-Right Neighbor
                if (i - 1 >= 0 && j + 1 < columns)
                {
                    int Index = (i - 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Left Neighbor
                if (j - 1 >= 0)
                {
                    int Index = i * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Right Neighbor
                if (j + 1 < columns)
                {
                    int Index = i * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Left Neighbor
                if (i + 1 < rows && j - 1 >= 0)
                {
                    int Index = (i + 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom Neighbor
                if (i + 1 < rows)
                {
                    int Index = (i + 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Right Neighbor
                if (i + 1 < rows && j + 1 < columns)
                {
                    int Index = (i + 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

            }


            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            foreach(var c in component)
            {
                Make_Set(c);
            }
            foreach (var edge in uniqueEdges)
            {
                if (Find_set(edge.v1) != Find_set(edge.v2))
                {
                    Union(edge.v1, edge.v2);
                    maxEdgeWeight = Math.Max(maxEdgeWeight, edge.w);
                    edgesAdded++;
                    if (edgesAdded == vertices - 1)
                        break;
                }
            }

         //   Console.WriteLine("Blue");
            return maxEdgeWeight;
        }
        public int CalculateInternalDifference_Green(List<int> component, char color)
        {
            int maxEdgeWeight = 0;
            int edgesAdded = 0;
            int vertices = component.Count;
            HashSet<int> componentSet = new HashSet<int>(component);
            List<(int v1, int v2, int w)> uniqueEdges = new List<(int v1, int v2, int w)>();
            HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

            foreach (int v1 in component)
            {
                int i = (v1 / columns);
                int j = (v1 % columns);

                //Top - Left Neighbor
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    int Index = (i - 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {

                        int weight = choose_color(color, i, j, i - 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top Neighbor
                if (i - 1 >= 0)
                {
                    int Index = (i - 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Top-Right Neighbor
                if (i - 1 >= 0 && j + 1 < columns)
                {
                    int Index = (i - 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i - 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Left Neighbor
                if (j - 1 >= 0)
                {
                    int Index = i * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Right Neighbor
                if (j + 1 < columns)
                {
                    int Index = i * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Left Neighbor
                if (i + 1 < rows && j - 1 >= 0)
                {
                    int Index = (i + 1) * columns + (j - 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j - 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom Neighbor
                if (i + 1 < rows)
                {
                    int Index = (i + 1) * columns + j;
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

                // Bottom-Right Neighbor
                if (i + 1 < rows && j + 1 < columns)
                {
                    int Index = (i + 1) * columns + (j + 1);
                    if (componentSet.Contains(Index))
                    {
                        int weight = choose_color(color, i, j, i + 1, j + 1);
                        int a = Math.Min(v1, Index);
                        int b = Math.Max(v1, Index);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }

            }


            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));
            foreach (var c in component)
            {
                Make_Set(c);
            }
            foreach (var edge in uniqueEdges)
            {
                if (Find_set(edge.v1) != Find_set(edge.v2))
                {
                    Union(edge.v1, edge.v2);
                    maxEdgeWeight = Math.Max(maxEdgeWeight, edge.w);
                    edgesAdded++;
                    if (edgesAdded == vertices - 1)
                        break;
                }
            }

         //   Console.WriteLine("Green");
            return maxEdgeWeight;

        }

        public Dictionary<int, (int maxInternalDifference, List<int> pixels)> CalculateFinalInternalDifferences(Dictionary<int, List<int>> pixels)
        {
            var internalDifferences = new Dictionary<int, (int maxInternalDifference, List<int> pixels)>();
            int i = 0;
            int compId = 0;
            foreach (var pixel in pixels.Values)
            {

                int redIntDiff = CalculateInternalDifference_Red(pixel, 'r');
                int greenIntDiff = CalculateInternalDifference_Green(pixel, 'g');
                int blueIntDiff = CalculateInternalDifference_Blue(pixel, 'b');

                int maxInternalDifference = Math.Max(redIntDiff, Math.Max(greenIntDiff, blueIntDiff));
                internalDifferences[compId] = (maxInternalDifference, pixel);
                compId++;

            }
            return internalDifferences;
        }


        public Dictionary<Tuple<int, int>, int> Difference_between_2_components(Dictionary<int,List<int>> component)
        {
            List<HashSet<int>> componentSet = new List<HashSet<int>>();
            foreach(var v in component)
            {
                HashSet<int> set = new HashSet<int>(v.Value);
                componentSet.Add(set);
            }
            Dictionary<Tuple<int, int>, int> bounderies_between_components = new Dictionary<Tuple<int, int>, int>();

            foreach(var l in component)
            {
                var comp = componentSet[0];
                foreach(var p in l.Value)
                {
                    int thisComp = GetCompId(p);

                    int NeighborComponent;
                    int i = (p / columns);
                    int j = (p % columns);

                    //Top - Left Neighbor
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + (j - 1);

                        if (!comp.Contains(Index))
                        {

                            int red_weight = choose_color('r', i, j, i - 1, j - 1);
                            int blue_weight = choose_color('b', i, j, i - 1, j - 1);
                            int green_weight = choose_color('g', i, j, i - 1, j - 1);
                            int final_weight= Math.Max(red_weight,Math.Max(green_weight, blue_weight));
                            
                            NeighborComponent = GetCompId(Index);
                            var key= Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key]<=final_weight)
                                    bounderies_between_components[key] = final_weight;
                                
                            }
                        }
                    }

                    // Top Neighbor
                    if (i - 1 >= 0)
                    {
                        int Index = (i - 1) * columns + j;
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i - 1, j );
                            int blue_weight = choose_color('b', i, j, i - 1, j );
                            int green_weight = choose_color('g', i, j, i - 1, j);
                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Top-Right Neighbor
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        int Index = (i - 1) * columns + (j + 1);
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i - 1, j + 1);
                            int blue_weight = choose_color('b', i, j, i - 1, j + 1);
                            int green_weight = choose_color('g', i, j, i - 1, j + 1);
                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Left Neighbor
                    if (j - 1 >= 0)
                    {
                        int Index = i * columns + (j - 1);
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i , j - 1);
                            int blue_weight = choose_color('b', i, j, i, j - 1);
                            int green_weight = choose_color('g', i, j, i, j - 1);

                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Right Neighbor
                    if (j + 1 < columns)
                    {
                        int Index = i * columns + (j + 1);
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i , j + 1);
                            int blue_weight = choose_color('b', i, j, i , j + 1);
                            int green_weight = choose_color('g', i, j, i, j + 1);

                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Bottom-Left Neighbor
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        int Index = (i + 1) * columns + (j - 1);
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i + 1, j - 1);
                            int blue_weight = choose_color('b', i, j, i + 1, j - 1);
                            int green_weight = choose_color('g', i, j, i + 1, j - 1);

                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Bottom Neighbor
                    if (i + 1 < rows)
                    {
                        int Index = (i + 1) * columns + j;
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i + 1, j);
                            int blue_weight = choose_color('b', i, j, i + 1, j);
                            int green_weight = choose_color('g', i, j, i + 1, j);

                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }

                    // Bottom-Right Neighbor
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        int Index = (i + 1) * columns + (j + 1);
                        if (!comp.Contains(Index))
                        {
                            int red_weight = choose_color('r', i, j, i + 1, j + 1);
                            int blue_weight = choose_color('b', i, j, i + 1, j + 1);
                            int green_weight = choose_color('g', i, j, i + 1, j + 1);
                            
                            int final_weight = Math.Max(red_weight, Math.Max(green_weight, blue_weight));

                            NeighborComponent = GetCompId(Index);

                            var key = Tuple.Create(thisComp, NeighborComponent);
                            if (!bounderies_between_components.ContainsKey(key))
                                bounderies_between_components.Add(key, final_weight);
                            else
                            {
                                if (bounderies_between_components[key] <= final_weight)
                                    bounderies_between_components[key] = final_weight;

                            }
                        }
                    }
                }
            }
            //Console.WriteLine("====================Bounderies=====================");
            //foreach (var p in bounderies_between_components)
            //{
            //    Console.WriteLine("bounderies between component " + p.Key.Item1 + " and " + p.Key.Item2 + " = " + p.Value);
            //}
            //Console.WriteLine("====================================================");
            return bounderies_between_components;
        }

    }

}
