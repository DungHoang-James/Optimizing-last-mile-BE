using System;
using OptimizingLastMile.Entites;

namespace OptimizingLastMile.Services.MachineLearning;

public class Kmeans : IKmeans
{
    public List<List<OrderInformation>> KmeansAlgorithm(List<OrderInformation> dataPoints, int k)
    {
        // Sample data points
        //List<double[]> dataPoints = new List<double[]>
        //{
        //    new double[] {10.850634540813425, 106.77191728528655},
        //    new double[] {10.847899450949335, 106.79182533625458},
        //    new double[] {10.845693002743944, 106.7941723717663},
        //};

        // Number of clusters (k)
        //int k = 4;

        // Run K-means clustering
        List<List<OrderInformation>> clusters = KMeans(dataPoints, k);

        // Display the result
        //for (int i = 0; i < clusters.Count; i++)
        //{
        //    Console.WriteLine($"Cluster {i + 1}:");
        //    foreach (var point in clusters[i])
        //    {
        //        var a = dataPoints.FirstOrDefault(d => d[0] == point[0] && d[1] == point[1]);
        //        if (a is not null)
        //        {
        //            Console.Write("ok - ");
        //        }
        //        Console.WriteLine($"({point[0]}, {point[1]})");
        //    }
        //    Console.WriteLine();
        //}

        return clusters;
    }

    private List<List<OrderInformation>> KMeans(List<OrderInformation> dataPoints, int k)
    {
        Random random = new Random();

        // Initialize centroids randomly
        List<OrderInformation> centroids = new();
        for (int i = 0; i < k; i++)
        {
            int randomIndex = random.Next(dataPoints.Count);
            centroids.Add(dataPoints[randomIndex]);
        }

        while (true)
        {
            // Assign each data point to the nearest centroid
            List<List<OrderInformation>> clusters = new();
            for (int i = 0; i < k; i++)
            {
                clusters.Add(new List<OrderInformation>());
            }

            foreach (var point in dataPoints)
            {
                int nearestCentroid = GetNearestCentroidIndex(point, centroids);
                clusters[nearestCentroid].Add(point);
            }

            // Calculate new centroids
            List<OrderInformation> newCentroids = new();
            for (int i = 0; i < k; i++)
            {
                if (clusters[i].Count > 0)
                {
                    newCentroids.Add(CalculateMean(clusters[i]));
                }
                else
                {
                    newCentroids.Add(centroids[i]);
                }
            }

            // Check for convergence
            if (CentroidsEqual(centroids, newCentroids))
            {
                return clusters;
            }

            centroids = newCentroids;
        }
    }

    private int GetNearestCentroidIndex(OrderInformation point, List<OrderInformation> centroids)
    {
        double minDistance = double.MaxValue;
        int nearestCentroid = -1;

        for (int i = 0; i < centroids.Count; i++)
        {
            double distance = CalculateEuclideanDistance(point, centroids[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCentroid = i;
            }
        }

        return nearestCentroid;
    }

    private double CalculateEuclideanDistance(OrderInformation point1, OrderInformation point2)
    {
        double sum = 0;
        //for (int i = 0; i < point1.Length; i++)
        //{
        //    sum += Math.Pow(point1[i] - point2[i], 2);
        //}

        sum += Math.Pow(point1.Lat - point2.Lat, 2);
        sum += Math.Pow(point1.Lng - point2.Lng, 2);

        return Math.Sqrt(sum);
    }

    private OrderInformation CalculateMean(List<OrderInformation> cluster)
    {
        //int dimensions = cluster[0].Length;
        int dimensions = 2;
        var orderMean = new OrderInformation();
        //double[] mean = new double[dimensions];

        //for (int i = 0; i < dimensions; i++)
        //{
        //    mean[i] = cluster.Select(p => p[i]).Average();
        //}

        orderMean.Lat = cluster.Select(p => p.Lat).Average();
        orderMean.Lng = cluster.Select(p => p.Lng).Average();

        return orderMean;
    }

    private bool CentroidsEqual(List<OrderInformation> centroids1, List<OrderInformation> centroids2)
    {
        //for (int i = 0; i < centroids1.Count; i++)
        //{
        //    for (int j = 0; j < centroids1[i].Length; j++)
        //    {
        //        if (centroids1[i][j] != centroids2[i][j])
        //        {
        //            return false;
        //        }
        //    }
        //}
        //return true;

        for (int i = 0; i < centroids1.Count; i++)
        {
            var c1 = centroids1[i];
            var c2 = centroids2[i];

            if (c1.Lat != c2.Lat)
            {
                return false;
            }

            if (c1.Lng != c2.Lng)
            {
                return false;
            }

            //for (int j = 0; j < centroids1[i].Length; j++)
            //{
            //    if (centroids1[i][j] != centroids2[i][j])
            //    {
            //        return false;
            //    }
            //}
        }
        return true;
    }
}