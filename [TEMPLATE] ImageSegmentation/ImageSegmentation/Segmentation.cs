using System;
using System.Collections.Generic;

namespace ImageTemplate
{
    public class Components
    {
        public HashSet<long> inner_pixels { get; private set; }
        public HashSet<long> boundaries { get; private set; }

        public Components()
        {
            inner_pixels = new HashSet<long>();
            boundaries = new HashSet<long>();
        }

        public void AddInnerPixel(long pixel, int intensity)
        {
            inner_pixels.Add(pixel);
        }

        public void AddBoundary(long pixel, int intensity)
        {
            if (!boundaries.Contains(pixel))
            {
                boundaries.Add(pixel);
            }
        }

        public bool ContainsInnerPixel(long pixel)
        {
            return inner_pixels.Contains(pixel);
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

        public List<Components> Segment(Dictionary<long, List<Tuple<long, int>>> graph, string color)
        {
            long M = rows * columns;
            List<(int u, int v, int weight)> pixels = new List<(int, int, int)>();
            HashSet<(int, int)> pixelSet = new HashSet<(int, int)>();
            foreach (var pixel in graph)
            {
                int u = (int)pixel.Key;
                foreach (var neighbor in pixel.Value)
                {
                    int v = (int)neighbor.Item1;
                    int weight = neighbor.Item2;
                    int min = Math.Min(u, v);
                    int max = Math.Max(u, v);
                    if (u < v && !pixelSet.Contains((min, max)))
                    {
                        pixelSet.Add((min, max));
                        pixels.Add((u, v, weight));
                    }
                }
            }
            pixels.Sort((a, b) => a.weight.CompareTo(b.weight));

            List<Components> componentsList = new List<Components>();
            bool[] visited = new bool[M];
            
            const int weightThreshold = 20;

            for (int pixel = 0; pixel < M; pixel++)
            {
                if (visited[pixel]) continue;

                Components component = new Components();
                Queue<long> queue = new Queue<long>();
                queue.Enqueue(pixel);
                visited[pixel] = true;

                int intensity = GetIntensityForPixel(pixel, color);
                component.AddInnerPixel(pixel, intensity);

                while (queue.Count > 0)
                {
                    long currentPixel = queue.Dequeue();
                    
                    if (!graph.ContainsKey(currentPixel)) continue;

                    foreach (var neighbor in graph[currentPixel])
                    {
                        long neighborPixel = neighbor.Item1;
                        int weight = neighbor.Item2;

                        if (visited[neighborPixel])
                        {
                            if (!component.ContainsInnerPixel(neighborPixel))
                            {
                                int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                component.AddBoundary(neighborPixel, neighborIntensity);
                            }
                        }
                        else if (weight < weightThreshold)
                        {
                            queue.Enqueue(neighborPixel);
                            visited[neighborPixel] = true;
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

            return componentsList;
        }

        public (List<Components> redComponents, List<Components> greenComponents, List<Components> blueComponents) SegmentImage()
        {
            GRAPH graphBuilder = new GRAPH(imageMatrix);

            var redGraph = graphBuilder.Red_Weight();
            var greenGraph = graphBuilder.Green_Weight();
            var blueGraph = graphBuilder.Blue_Weight();

            var redComponents = Segment(redGraph, "red");
            var greenComponents = Segment(greenGraph, "green");
            var blueComponents = Segment(blueGraph, "blue");

            return (redComponents, greenComponents, blueComponents);
        }
    }
}