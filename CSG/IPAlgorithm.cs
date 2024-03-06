namespace CSG;

public class IPAlgorithm : Algorithm
{
    private readonly Dictionary<List<int>, List<List<List<Goal>>>> _ipSubSpacesCoalitions = new();
    private readonly Dictionary<List<int>, double> _ipMinValuesOfBounds = new();
    
    public IPAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

    public override List<List<Goal>> Start()
    {
        CalculateCoalitionsValue();
        FillCoalitionType(); 
        SetOptimalCoalitionStructure();
        SetCarOnBestCoalitionStructure();
        return BestCoalitionStructure;
    }

    private void SetOptimalCoalitionStructure()
    {
        var sortedDict = from entry in _ipMinValuesOfBounds orderby entry.Value ascending select entry;
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
        BestCoalitionStructure = bestCoalitionStructure;
    }

    private List<List<Goal>> GetBestCoalitionStructureOfSubspace(List<int> subspace)
    {
        var coalitionStructure = new List<List<Goal>>();
        var valueOfBestCoalition = double.MaxValue;
        foreach (var currentCoalitionStructure in _ipSubSpacesCoalitions[subspace])
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

    private void FillCoalitionType()
    {
        foreach (var subset in GetAllSubsetsFollowingPartitions(Goals))
        {
            var key = subset.Select(s => s.Count).ToList();
            key.Sort();

            if (!_ipSubSpacesCoalitions.TryAdd(key, []))
            {
                _ipSubSpacesCoalitions[key].Add(subset);
            }

            CalculateBound(key, subset);
        }
    }

    private void CalculateBound(List<int> key, IEnumerable<List<Goal>> subset)
    {
        var value = subset.Select(s => ValuesOfCoalitions[s]).Sum();
        if (!_ipMinValuesOfBounds.TryAdd(key, value));
        {
            if (value < _ipMinValuesOfBounds[key])
            {
                _ipMinValuesOfBounds[key] = value;
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