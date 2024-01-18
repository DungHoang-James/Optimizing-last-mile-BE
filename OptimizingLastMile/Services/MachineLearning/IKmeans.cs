using OptimizingLastMile.Entites;

namespace OptimizingLastMile.Services.MachineLearning;

public interface IKmeans
{
    List<List<OrderInformation>> KmeansAlgorithm(List<OrderInformation> dataPoints, int k);
}