/*using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.RouteEngine.Entities;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.RouteSolvers
{
    /// <summary>
    /// Calculates the best route between various paths, using Dijkstra's algorithm
    /// </summary>
    /// <remarks>
    /// Copied the algorithm's implementation from <see cref="http://www.codeproject.com/Articles/22647/Dijkstra-Shortest-Route-Calculation-Object-Oriente"/>.
    /// Implementation was adjusted to support Generics, and make heavier use of LINQ
    /// </remarks>
    internal class DijkstraRouteSolver : IRouteSolver
    {

        public List<List<Path>> FindRoute(int originCityId, int destinationCityId, List<Path> paths, RouteCostTypes costType)
        {
            var result = CalculateFrom(originCityId, paths)[destinationCityId];
            return new List<List<Path>>
                       {
                           result.ToList()
                       };
        }

        private static Dictionary<int, LinkedList<Path>> CalculateFrom(int source, List<Path> paths)
        {
            // validate the paths
            if (paths.Any(p => p.SourceCityId.Equals(p.DestinationCityId)))
                throw new ArgumentException("No path can have the same source and destination");

            // keep track of the shortest paths identified thus far
            var shortestPaths = new Dictionary<int, KeyValuePair<double, LinkedList<Path>>>();
            // keep track of the locations which have been completely processed
            var locationsProcessed = new List<int>();

            // include all possible steps, with Int.MaxValue cost
            paths.SelectMany(p => new [] {p.SourceCityId, p.DestinationCityId}) // union source and destinations
                .Distinct() // remove duplicates
                .ToList() // ToList exposes ForEach
                .ForEach(s => shortestPaths.Set(s, int.MaxValue, null)); // add to ShortestPaths with MaxValue cost

            // update cost for self-to-self as 0; no path
            shortestPaths.Set(source, 0, null);

            // keep this cached
            var locationCount = shortestPaths.Keys.Count;

            while (locationsProcessed.Count < locationCount)
            {

                int locationToProcess = 0;

                //Search for the nearest location that isn't handled
                foreach (int location in shortestPaths.OrderBy(p => p.Value.Key).Select(p => p.Key).ToList())
                {
                    if (!locationsProcessed.Contains(location))
                    {
                        if (shortestPaths[location].Key == int.MaxValue)
                            return shortestPaths.ToDictionary(k => k.Key, v => v.Value.Value);
                                //ShortestPaths[destination].Value;

                        locationToProcess = location;
                        break;
                    }
                } 

                var selectedPaths = paths.Where(p => p.SourceCityId.Equals(locationToProcess));
                foreach (Path path in selectedPaths)
                {
                    if (shortestPaths[path.DestinationCityId].Key > path.Cost + shortestPaths[path.SourceCityId].Key)
                    {
                        shortestPaths.Set(
                            path.DestinationCityId,
                            path.Cost + shortestPaths[path.SourceCityId].Key,
                            shortestPaths[path.SourceCityId].Value.Union(new[] {path}).ToArray());
                    }
                } 

                //Add the location to the list of processed locations
                locationsProcessed.Add(locationToProcess);

            } // while

            return shortestPaths.ToDictionary(k => k.Key, v => v.Value.Value);
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds or Updates the dictionary to include the destination and its associated cost and complete path (and param arrays make paths easier to work with)
        /// </summary>
        internal static void Set(this Dictionary<int, KeyValuePair<double, LinkedList<Path>>> dictionary, int destination, double cost, params Path[] paths)
        {
            var completePath = paths == null ? new LinkedList<Path>() : new LinkedList<Path>(paths);
            dictionary[destination] = new KeyValuePair<double, LinkedList<Path>>(cost, completePath);
        }
    }
}*/