
namespace CSG;

public class CoalitionStructureGenerationAlgorithm : Algorithm
{
    public CoalitionStructureGenerationAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

    private List<List<Goal>> GetRecursiveCoalitionStructure(Dictionary<List<Goal>, List<List<Goal>>> partitions, List<List<Goal>> bestCoalitions)
    {
        foreach (var coalition in new List<List<Goal>>(bestCoalitions))
        {
            if (partitions[coalition].Count > 1)
            {
                bestCoalitions.Remove(coalition);
                bestCoalitions.AddRange(GetRecursiveCoalitionStructure(partitions, partitions[coalition]));
            }
        }
        return bestCoalitions;
    }

    public override List<List<Goal>> GetOptimalCoalitionStructure()
    {
        var valuesOfCoalitions = new Dictionary<List<Goal>, double>(new ListEqualityComparer<Goal>());
        var valuesOfBestCoalitions = new Dictionary<List<Goal>, double>(new ListEqualityComparer<Goal>());
        var partitionOfBestCoalitions = new Dictionary<List<Goal>, List<List<Goal>>>(new ListEqualityComparer<Goal>());
        for (var i = 1; i <= Goals.Count; i++)
        {
            foreach (var subset in GetSubsetsOfSize(i))
            {
                var subsetCopie = subset.ToList();
                valuesOfCoalitions.Add(subsetCopie, CalculateCoalitionValue(subsetCopie));
                valuesOfBestCoalitions.Add(subsetCopie, CalculateCoalitionValue(subsetCopie));
                partitionOfBestCoalitions.Add(subsetCopie, [subsetCopie]);

                for (var j = 1; j < subsetCopie.Count; j++)
                {
                    var firstHalf = subset.Take(j).ToList();
                    var secondHalf = subset.Skip(j).ToList();

                    if (valuesOfCoalitions[firstHalf] + valuesOfCoalitions[secondHalf] < valuesOfBestCoalitions[subsetCopie])
                    {
                        partitionOfBestCoalitions[subsetCopie] = [firstHalf, secondHalf];
                        valuesOfBestCoalitions[subsetCopie] =
                            CalculateCoalitionValue(firstHalf) + CalculateCoalitionValue(secondHalf);
                    }
                }
            }
        }

        return GetRecursiveCoalitionStructure(partitionOfBestCoalitions, [Goals]);
    }

    
}
