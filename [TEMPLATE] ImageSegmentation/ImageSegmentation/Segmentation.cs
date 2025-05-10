using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ImageTemplate
{
    /*public class Components
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
    }*/

    internal class Segmentation
    {
        private RGBPixel[,] imageMatrix;
        private int rows;
        private int columns;
        public long M;
        public long[] redMap { get; private set; }
        public long[] greenMap { get; private set; }
        public long[] blueMap { get; private set; }
        private Dictionary<long, List<long>> _componentPixels;

        public Segmentation(RGBPixel[,] matrix)
        {
            imageMatrix = matrix;
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            M = rows * columns;
            redMap = new long[M];
            greenMap = new long[M];
            blueMap = new long[M];
            _componentPixels = new Dictionary<long, List<long>>();
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

        public void Segment(Dictionary<long, List<Tuple<long, int>>> graph, string color)
        {
            /*List<(int u, int v, int weight)> pixels = new List<(int, int, int)>();
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
            pixels.Sort((a, b) => a.weight.CompareTo(b.weight));*/

            //List<Components> componentsList = new List<Components>();
            bool[] visited = new bool[M];
            int Id = 0;
            const int weightThreshold = 1;

            for (int pixel = 0; pixel < M; pixel++)
            {
                if (visited[pixel]) continue;

                //Components component = new Components();
                Queue<long> queue = new Queue<long>();
                queue.Enqueue(pixel);
                visited[pixel] = true;

                
                if (color == "red") redMap[pixel] = Id;
                else if (color == "green") greenMap[pixel] = Id;
                else if (color == "blue") blueMap[pixel] = Id;

                while (queue.Count > 0)
                {
                    long currentPixel = queue.Dequeue();

                    if (!graph.ContainsKey(currentPixel)) continue;

                    foreach (var neighbor in graph[currentPixel])
                    {
                        long neighborPixel = neighbor.Item1;
                        int weight = neighbor.Item2;

                        /*if (visited[neighborPixel])
                        {
                            if (!component.ContainsInnerPixel(neighborPixel))
                            {
                                int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                                component.AddBoundary(neighborPixel, neighborIntensity);
                            }
                        }*/
                        if (!visited[neighborPixel] && weight < weightThreshold)
                        {
                            queue.Enqueue(neighborPixel);
                            visited[neighborPixel] = true;
                            //int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                            //component.AddInnerPixel(neighborPixel, neighborIntensity);
                            if (color == "red") redMap[neighborPixel] = Id;
                            else if (color == "green") greenMap[neighborPixel] = Id;
                            else if (color == "blue") blueMap[neighborPixel] = Id;
                        }
                        /*else
                        {
                            int neighborIntensity = GetIntensityForPixel((int)neighborPixel, color);
                            component.AddBoundary(neighborPixel, neighborIntensity);
                        }*/
                    }
                }

                //componentsList.Add(component);
                Id++;
            }

            //return componentsList;
        }

        public Dictionary<long, List<long>> Red_component= new Dictionary<long, List<long>>();
        public Dictionary<long, List<long>> Blue_component = new Dictionary<long, List<long>>();
        public Dictionary<long, List<long>> Green_component = new Dictionary<long, List<long>>();


        long RID = 0;
        private Dictionary<long, List<long>> GetCombinedComponents()
        {
            if (_componentPixels.Count == 0)
            {
                for (long pixel = 0; pixel < M; pixel++)
                {
                    long redId = redMap[pixel];
                    long greenId = greenMap[pixel];
                    long blueId = blueMap[pixel];

                    long componentId = redId;
                    bool isValidComponent = false;
                    if ((redId == greenId && redId == blueId)) // All match
                    {
                        isValidComponent = true;
                        componentId = redId;
                    }

                    if (isValidComponent)
                    {
                        if (!_componentPixels.ContainsKey(componentId))
                            _componentPixels[componentId] = new List<long>();
                        if (!_componentPixels[componentId].Contains(pixel))
                            _componentPixels[componentId].Add(pixel);

                    }
                    else
                    {
                        if (redId == greenId && redId != blueId)
                        {
                            if (!Blue_component.ContainsKey(blueId))
                                Blue_component[blueId] = new List<long>();

                            Blue_component[blueId].Add(pixel);

                        }
                        else if (redId == blueId && redId != greenId)// Red and Green
                        {
                            if (!Green_component.ContainsKey(greenId))
                                Green_component[greenId] = new List<long>();

                            Green_component[greenId].Add(pixel);
                        }
                        else if ((greenId == blueId && greenId != redId))
                        {
                            if (!Red_component.ContainsKey(redId))
                                Red_component[redId] = new List<long>();

                            Red_component[redId].Add(pixel);
                        }


                    }
                }
                long key = -1;
                foreach(var v in  Red_component)
                {
                    if(!_componentPixels.ContainsKey(v.Key))
                        _componentPixels.Add(v.Key, v.Value);
                    else
                    {
                        _componentPixels.Add(key, v.Value);
                        key--;
                    }
                }
                foreach (var v in Green_component)
                {
                    if (!_componentPixels.ContainsKey(v.Key))
                        _componentPixels.Add(v.Key, v.Value);
                    else
                    {
                        _componentPixels.Add(key, v.Value);
                        key--;
                    }
                }
                foreach (var v in Blue_component)
                {
                    if (!_componentPixels.ContainsKey(v.Key))
                        _componentPixels.Add(v.Key, v.Value);
                    else
                    {
                        _componentPixels.Add(key, v.Value);
                        key--;
                    }
                }
            }

           /* Dictionary<long, List<int>> componentIntensities = new Dictionary<long, List<int>>();
            foreach (var component in _componentPixels)
            {
                long componentId = component.Key;
                List<int> intensities = new List<int>();
                int i = 0;
                foreach (long pixel in component.Value)
                {
                    intensities.Add(GetIntensityForPixel((int)pixel, "red"));
                }
                componentIntensities[componentId] = intensities;
            }*/

            return _componentPixels;
        }
        public Dictionary<long, List<long>> SegmentImage(Dictionary<long, List<Tuple<long, int>>> red, Dictionary<long, List<Tuple<long, int>>> blue, Dictionary<long, List<Tuple<long, int>>>green)
        {
            
            var redGraph = red;
            var greenGraph = green;
            var blueGraph = blue;

            _componentPixels.Clear();
        
            Segment(redGraph, "red");
            Segment(greenGraph, "green");
            Segment(blueGraph, "blue");

            return GetCombinedComponents();
        }
    }
}