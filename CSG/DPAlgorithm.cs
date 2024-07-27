
namespace CSG;

public class DPAlgorithm : Algorithm
{
    private readonly Dictionary<List<Goal>, List<List<Goal>>> _partitionOfBestCoalitions = new (EqualityComparer);
    public DPAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

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

    public override List<List<Goal>> Start()
    {
        SetOptimalCoalitionStructure();
        SetCarOnBestCoalitionStructure();
        return BestCoalitionStructure;
    }
    
    private void SetOptimalCoalitionStructure()
    {
        
        for (var i = 1; i <= Goals.Count; i++)
        {
            foreach (var subset in GetSubsetsOfSize(i, Goals))
            {
                CarSelectionForCoalitions.Add(subset, GetMinCar(subset));
                ValuesOfCoalitions.Add(subset, CalculateCoalitionValue(subset));
                _partitionOfBestCoalitions.Add(subset, [subset]);
                    
                if (i == 1) continue;
                foreach (var halves in GetAllHalves(subset))
                {
                    var firstHalf = halves[0];
                    var secondHalf = halves[1];
                    
                    if (ValuesOfCoalitions[firstHalf] + ValuesOfCoalitions[secondHalf] < ValuesOfCoalitions[subset])
                    {
                        _partitionOfBestCoalitions[subset] = [firstHalf, secondHalf];
                        ValuesOfCoalitions[subset] =
                            ValuesOfCoalitions[firstHalf] + ValuesOfCoalitions[secondHalf];
                    }
                }
            }
        }
        BestCoalitionStructure = GetRecursiveCoalitionStructure(_partitionOfBestCoalitions, [Goals]);
    }

    
}
