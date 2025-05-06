/*using System;
using System.Collections.Generic;

namespace ImageTemplate
{
    public class Components
    {
        private List<Tuple<long, int>> boundaries;
        private List<Tuple<long, int>> inner_pixels;

        public Components()
        {
            boundaries = new List<Tuple<long, int>>();
            inner_pixels = new List<Tuple<long, int>>();
        }

        public List<Tuple<long, int>> Boundaries { get { return boundaries; } }
        public List<Tuple<long, int>> InnerPixels { get { return inner_pixels; } }
    }

    internal class Segmentation
    {
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;

        public Segmentation(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
        }

        private int GetIntensityForPixel(int pixelNumber, string color)
        {
            int row = pixelNumber / columns;
            int col = pixelNumber % columns;

            int intensity = 0;
            if (color == "red")
            {
                intensity = imageMatrix[row, col].red;
            }
            else if (color == "green")
            {
                intensity = imageMatrix[row, col].green;
            }
            else if (color == "blue")
            {
                intensity = imageMatrix[row, col].blue;
            }
            return intensity;
        }

        public List<Components> SegmentChannel(Dictionary<long, List<Tuple<long, int>>> graph, string color)
        {
            List<Components> componentsList = new List<Components>();
            bool[] visited = new bool[rows * columns];
            const int weightThreshold = 30;

            for (int pixel = 0; pixel < rows * columns; pixel++)
            {
                if (visited[pixel]) continue;

                Components component = new Components();
                Queue<long> queue = new Queue<long>();
                queue.Enqueue(pixel);
                visited[pixel] = true;

                int intensity = GetIntensityForPixel(pixel, color);
                component.InnerPixels.Add(new Tuple<long, int>(pixel, intensity));

                while (queue.Count > 0)
                {
                    long currentPixel = queue.Dequeue();
                    int currentIntensity = GetIntensityForPixel((int)currentPixel, color);

                    if (!graph.ContainsKey(currentPixel)) continue;

                    foreach (var neighbor in graph[currentPixel])
                    {
                        long neighborPixel = neighbor.Item1;
                        int weight = neighbor.Item2;

                        bool neighborAlreadySeen = visited[neighborPixel];
                        if (neighborAlreadySeen)
                        {
                            bool isInInnerPixels = false;
                            for (int i = 0; i < component.InnerPixels.Count; i++)
                            {
                                if (component.InnerPixels[i].Item1 == neighborPixel)
                                {
                                    isInInnerPixels = true;
                                    break;
                                }
                            }

                            if (!isInInnerPixels)
                            {
                                bool isInBoundaries = false;
                                for (int i = 0; i < component.Boundaries.Count; i++)
                                {
                                    if (component.Boundaries[i].Item1 == neighborPixel)
                                    {
                                        isInBoundaries = true;
                                        break;
                                    }
                                }

                                if (!isInBoundaries)
                                {
                                    int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                    component.Boundaries.Add(new Tuple<long, int>(neighborPixel, neighborIntensity));
                                }
                            }
                            continue;
                        }

                        if (weight < weightThreshold)
                        {
                            queue.Enqueue(neighborPixel);
                            visited[neighborPixel] = true;
                            int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                            component.InnerPixels.Add(new Tuple<long, int>(neighborPixel, neighborIntensity));
                        }
                        else
                        {
                            bool isInBoundaries = false;
                            for (int i = 0; i < component.Boundaries.Count; i++)
                            {
                                if (component.Boundaries[i].Item1 == neighborPixel)
                                {
                                    isInBoundaries = true;
                                    break;
                                }
                            }

                            if (!isInBoundaries)
                            {
                                int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                component.Boundaries.Add(new Tuple<long, int>(neighborPixel, neighborIntensity));
                            }
                        }
                    }
                }

                componentsList.Add(component);
            }

            return componentsList;
        }

        public (List<Components> redComponents, List<Components> greenComponents, List<Components> blueComponents) SegmentImage()
        {
            GRAPH graphBuilder = new GRAPH(imageMatrix);

            var redGraph = graphBuilder.Red_Weight();
            var greenGraph = graphBuilder.Green_Weight();
            var blueGraph = graphBuilder.Blue_Weight();

            var redComponents = SegmentChannel(redGraph, "red");
            var greenComponents = SegmentChannel(greenGraph, "green");
            var blueComponents = SegmentChannel(blueGraph, "blue");

            return (redComponents, greenComponents, blueComponents);
        }
    }
}*/

using System;
using System.Collections.Generic;

namespace ImageTemplate
{
    public class Components
    {
        private List<Tuple<long, int>> boundaries;
        private List<Tuple<long, int>> inner_pixels;
        private HashSet<long> innerPixelsSet;  // For fast lookup of inner pixels
        private HashSet<long> boundariesSet;   // For fast lookup of boundary pixels

        public Components()
        {
            boundaries = new List<Tuple<long, int>>();
            inner_pixels = new List<Tuple<long, int>>();
            innerPixelsSet = new HashSet<long>();
            boundariesSet = new HashSet<long>();
        }

        public List<Tuple<long, int>> Boundaries { get { return boundaries; } }
        public List<Tuple<long, int>> InnerPixels { get { return inner_pixels; } }

        // Helper method to add a pixel to InnerPixels
        public void AddInnerPixel(long pixel, int intensity)
        {
            innerPixelsSet.Add(pixel);
            inner_pixels.Add(new Tuple<long, int>(pixel, intensity));
        }

        // Helper method to add a pixel to Boundaries
        public void AddBoundary(long pixel, int intensity)
        {
            if (!boundariesSet.Contains(pixel))
            {
                boundariesSet.Add(pixel);
                boundaries.Add(new Tuple<long, int>(pixel, intensity));
            }
        }

        // Check if a pixel is in InnerPixels quickly
        public bool ContainsInnerPixel(long pixel)
        {
            return innerPixelsSet.Contains(pixel);
        }
    }

    internal class Segmentation
    {
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;

        public Segmentation(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
        }

        private int GetIntensityForPixel(int pixelNumber, string color)
        {
            int row = pixelNumber / columns;
            int col = pixelNumber % columns;

            int intensity = 0;
            if (color == "red")
            {
                intensity = imageMatrix[row, col].red;
            }
            else if (color == "green")
            {
                intensity = imageMatrix[row, col].green;
            }
            else if (color == "blue")
            {
                intensity = imageMatrix[row, col].blue;
            }
            return intensity;
        }

        public List<Components> SegmentChannel(Dictionary<long, List<Tuple<long, int>>> graph, string color)
        {
            int M = rows * columns;
            const int weightThreshold = 15;

            // Step 1: Extract and sort edges directly from the graph
            List<(int u, int v, int weight)> edges = new List<(int, int, int)>();
            HashSet<(int, int)> edgeSet = new HashSet<(int, int)>();  // To ensure no duplicates
            foreach (var pixel in graph)
            {
                int u = (int)pixel.Key;
                foreach (var neighbor in pixel.Value)
                {
                    int v = (int)neighbor.Item1;
                    int weight = neighbor.Item2;
                    int min = Math.Min(u, v);
                    int max = Math.Max(u, v);
                    if (u < v && !edgeSet.Contains((min, max)))  // Only add if not already added in reverse
                    {
                        edgeSet.Add((min, max));
                        edges.Add((u, v, weight));
                    }
                }
            }
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));

            // Step 2: Initialize data structures
            List<Components> componentsList = new List<Components>();
            bool[] visited = new bool[M];
            HashSet<int> remainingPixels = new HashSet<int>();
            for (int p = 0; p < M; p++)
            {
                remainingPixels.Add(p);
            }

            // Step 3: Process edges and run BFS
            Queue<int> pixelQueue = new Queue<int>();
            foreach (var edge in edges)
            {
                int u = edge.u;
                int v = edge.v;
                int weight = edge.weight;

                if (!visited[u])
                {
                    pixelQueue.Enqueue(u);
                    remainingPixels.Remove(u);
                }
                if (!visited[v])
                {
                    pixelQueue.Enqueue(v);
                    remainingPixels.Remove(v);
                }

                while (pixelQueue.Count > 0)
                {
                    int startPixel = pixelQueue.Dequeue();
                    if (visited[startPixel]) continue;

                    Components component = new Components();
                    Queue<long> bfsQueue = new Queue<long>();
                    bfsQueue.Enqueue(startPixel);
                    visited[startPixel] = true;

                    int intensity = GetIntensityForPixel(startPixel, color);
                    component.AddInnerPixel(startPixel, intensity);

                    while (bfsQueue.Count > 0)
                    {
                        long currentPixel = bfsQueue.Dequeue();
                        if (!graph.ContainsKey(currentPixel)) continue;

                        foreach (var neighbor in graph[currentPixel])
                        {
                            long neighborPixel = neighbor.Item1;
                            int Lweight = neighbor.Item2;

                            if (visited[neighborPixel])
                            {
                                if (!component.ContainsInnerPixel(neighborPixel))
                                {
                                    int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                    component.AddBoundary(neighborPixel, neighborIntensity);
                                }
                                continue;
                            }

                            if (Lweight < weightThreshold)
                            {
                                bfsQueue.Enqueue(neighborPixel);
                                visited[neighborPixel] = true;
                                remainingPixels.Remove((int)neighborPixel);
                                int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                component.AddInnerPixel(neighborPixel, neighborIntensity);
                            }
                            else
                            {
                                int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                component.AddBoundary(neighborPixel, neighborIntensity);
                            }
                        }
                    }

                    componentsList.Add(component);
                }
            }

            // Step 4: Handle any remaining unvisited pixels
            foreach (int pixel in new List<int>(remainingPixels))
            {
                if (visited[pixel]) continue;

                Components component = new Components();
                visited[pixel] = true;
                int intensity = GetIntensityForPixel(pixel, color);
                component.AddInnerPixel(pixel, intensity);
                componentsList.Add(component);
            }

            return componentsList;
        }

        public (List<Components> redComponents, List<Components> greenComponents, List<Components> blueComponents) SegmentImage()
        {
            GRAPH graphBuilder = new GRAPH(imageMatrix);

            var redGraph = graphBuilder.Red_Weight();
            var greenGraph = graphBuilder.Green_Weight();
            var blueGraph = graphBuilder.Blue_Weight();

            var redComponents = SegmentChannel(redGraph, "red");
            var greenComponents = SegmentChannel(greenGraph, "green");
            var blueComponents = SegmentChannel(blueGraph, "blue");

            return (redComponents, greenComponents, blueComponents);
        }
    }
}