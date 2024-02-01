
namespace CSG;

public class CoalitionStructureGenerationAlgorithm : Algorithm
{
    public CoalitionStructureGenerationAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

    private static List<List<Goal>> GetRecursiveCoalitionStructure(Dictionary<List<Goal>, List<List<Goal>>> partitions, List<List<Goal>> bestCoalitions)
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
        var equalityComparer = new EnumerableEqualityComparer<Goal>();
        var valuesOfBestCoalitions = new Dictionary<List<Goal>, double>(equalityComparer);
        var partitionOfBestCoalitions = new Dictionary<List<Goal>, List<List<Goal>>>(equalityComparer);
        var carSelectionForCoalitions = new Dictionary<List<Goal>, Car?>(equalityComparer);
        for (var i = 1; i <= Goals.Count; i++)
        {
            foreach (var subset in GetSubsetsOfSize(i, Goals))
            {
                carSelectionForCoalitions.Add(subset, GetMinCar(subset));
                valuesOfBestCoalitions.Add(subset, CalculateCoalitionValue(subset));
                partitionOfBestCoalitions.Add(subset, [subset]);
                    
                if (i == 1) continue;
                foreach (var halves in GetAllPossibilitiesToSplitListIntoTwoLists(subset))
                {
                    var firstHalf = halves[0];
                    var secondHalf = halves[1];
                    
                    if (valuesOfBestCoalitions[firstHalf] + valuesOfBestCoalitions[secondHalf] < valuesOfBestCoalitions[subset])
                    {
                        partitionOfBestCoalitions[subset] = [firstHalf, secondHalf];
                        valuesOfBestCoalitions[subset] =
                            valuesOfBestCoalitions[firstHalf] + valuesOfBestCoalitions[secondHalf];
                    }
                }
            }
        }

        var bestCoalitions = GetRecursiveCoalitionStructure(partitionOfBestCoalitions, [Goals]);
        foreach (var coalition in bestCoalitions)
        {
            foreach (var goal in coalition)
            {
                goal.Car = carSelectionForCoalitions[coalition];
            }
        }

        return bestCoalitions;
    }

    
}
