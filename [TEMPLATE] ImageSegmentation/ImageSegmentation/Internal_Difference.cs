using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ImageTemplate
{
    public class Internal_Difference
    {
        int[] member;

        public Internal_Difference(long size)
        {
            member = new int[size];
            for (int i = 0; i < size; i++)
                Make_Set(i);
            // member[i] = i;
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

        public int CalculateInternalDifference(List<long> component, Dictionary<long, List<Tuple<long, int>>> graph)
        {
            HashSet<long> componentSet = new HashSet<long>(component);
            var uniqueEdges = new List<(int v1, int v2, int w)>();
            var addedEdges = new HashSet<(int, int)>();

            foreach (var node in component)
            {
                foreach (var neighbor in graph[node])
                {
                    int vertex2 = (int)neighbor.Item1;
                    int weight = neighbor.Item2;

                    if (componentSet.Contains(vertex2))
                    {
                        int a = Math.Min((int)node, vertex2);
                        int b = Math.Max((int)node, vertex2);

                        if (addedEdges.Add((a, b)))
                        {
                            uniqueEdges.Add((a, b, weight));
                        }
                    }
                }
            }

            // Internal_Difference uf = new Internal_Difference(component.Max() + 1);
            /*  member = new int[component.Max() + 1];
              for (int i = 0; i < member.Length; i++)
              {
                  Make_Set(i);
              }
             */

            uniqueEdges.Sort((a, b) => a.w.CompareTo(b.w));


            int maxEdgeWeight = 0;
            int edgesAdded = 0;
            int vertices = component.Count;

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

            return maxEdgeWeight;
        }

        public Dictionary<int, (int maxInternalDifference, List<long> pixels)> CalculateFinalInternalDifferences(
         Dictionary<long, List<long>> components,
         Dictionary<long, List<Tuple<long, int>>> redGraph,
         Dictionary<long, List<Tuple<long, int>>> greenGraph,
         Dictionary<long, List<Tuple<long, int>>> blueGraph)
        {
            var internalDifferences = new Dictionary<int, (int maxInternalDifference, List<long> pixels)>();

            int compId = 0;
            foreach (var c in components)
            {
                long componentId = c.Key;
                List<long> component = c.Value;
                if (componentId == -1) 
                    continue; 
                if (component.Count == 0)
                    continue;

                int redIntDiff = CalculateInternalDifference(component, redGraph);
                int greenIntDiff = CalculateInternalDifference(component, greenGraph);
                int blueIntDiff = CalculateInternalDifference(component, blueGraph);

                int maxInternalDifference = Math.Max(redIntDiff, Math.Max(greenIntDiff, blueIntDiff));
                internalDifferences[compId] = (maxInternalDifference, component);
                compId++;
            }

            return internalDifferences;
        }

    }

}
