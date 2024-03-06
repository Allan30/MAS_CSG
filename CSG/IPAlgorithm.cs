namespace CSG;

public class IPAlgorithm : Algorithm
{
    private static readonly EnumerableEqualityComparer<Goal> EqualityComparer = new();
    private Dictionary<List<Goal>, Car?> CarSelectionForCoalitions = new(EqualityComparer);
    private Dictionary<List<Goal>, double> ValuesOfCoalitions = new(EqualityComparer);
    private Dictionary<List<int>, List<List<List<Goal>>>> IPSubSpacesCoalitions = new();
    private Dictionary<List<int>, double> IPMinValuesOfBounds = new();

    
    public IPAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

    public List<List<Goal>> Start()
    {
        CalculateCoalitionsValue();
        FillCoalitionType();
        return GetOptimalCoalitionStructure();
    }


    public override List<List<Goal>> GetOptimalCoalitionStructure()
    {
        var sortedDict = from entry in IPMinValuesOfBounds orderby entry.Value ascending select entry;
        var minValue = double.MaxValue;
        var bestCoalitionStructure = new List<List<Goal>>();
        foreach (var (subspace, currentValue) in sortedDict)
        {
            if (currentValue < minValue)
            {
                var localBestCoalition = GetBestCoalitionStructureOfSubspace(subspace);
                var localBestValue = localBestCoalition.Select(c => ValuesOfCoalitions[c]).Sum();
                if (localBestValue < minValue)
                {
                    minValue = localBestValue;
                    bestCoalitionStructure = localBestCoalition;
                }
            }
        }
        return bestCoalitionStructure;
    }

    private List<List<Goal>> GetBestCoalitionStructureOfSubspace(List<int> subspace)
    {
        var coalitionStructure = new List<List<Goal>>();
        var valueOfBestCoalition = double.MaxValue;
        foreach (var currentCoalitionStructure in IPSubSpacesCoalitions[subspace])
        {
            var currentValue = 0.0;
            foreach (var coalition in currentCoalitionStructure)
            {
                currentValue += ValuesOfCoalitions[coalition];
                if (currentValue > valueOfBestCoalition)
                {
                    break;
                }
            }
            if (currentValue < valueOfBestCoalition)
            {
                valueOfBestCoalition = currentValue;
                coalitionStructure = currentCoalitionStructure;
            }
        }
        return coalitionStructure;
    }

    private void CalculateCoalitionsValue()
    {
        for (var i = 1; i <= Goals.Count; i++)
        {
            foreach (var subset in GetSubsetsOfSize(i, Goals))
            {
                CarSelectionForCoalitions.Add(subset, GetMinCar(subset));
                ValuesOfCoalitions.Add(subset, CalculateCoalitionValue(subset));
            }
        }
    }

    private void FillCoalitionType()
    {
        foreach (var subset in GetAllSubsetsFollowingPartitions(Goals))
        {
            var key = subset.Select(s => s.Count).ToList();
            key.Sort();

            if (!IPSubSpacesCoalitions.TryAdd(key, []))
            {
                IPSubSpacesCoalitions[key].Add(subset);
            }

            CalculateBound(key, subset);
        }
    }

    private void CalculateBound(List<int> key, IEnumerable<List<Goal>> subset)
    {
        var value = subset.Select(s => ValuesOfCoalitions[s]).Sum();
        if (!IPMinValuesOfBounds.TryAdd(key, value));
        {
            if (value < IPMinValuesOfBounds[key])
            {
                IPMinValuesOfBounds[key] = value;
            }
        }
        
    }

    private static List<List<List<Goal>>> GetAllSubsetsFollowingPartitions(List<Goal> goals)
    {
        if (goals.Count == 0)
        {
            return [];
        }

        var result = new List<List<List<Goal>>>();
        for (var i = 1; i < goals.Count; i++)
        {
            foreach (var subset in GetAllSubsetsFollowingPartitions(goals[i..]))
            {
                var newSubset = new List<List<Goal>>(subset);
                newSubset.AddRange([goals[..i]]);
                result.Add(newSubset);
            }
        }

        return result;
    }
}