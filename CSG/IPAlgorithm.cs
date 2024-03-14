namespace CSG;

public class IPAlgorithm : Algorithm
{
    private static readonly ListEqualityComparer EqualityComparerInt = new();
    private readonly Dictionary<List<int>, List<List<List<Goal>>>> _ipSubSpacesCoalitions = new(EqualityComparerInt);
    private readonly Dictionary<List<int>, double> _ipMinValuesOfBounds = new(EqualityComparerInt);
    
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
        foreach (var subset in PartitionGen(Goals))
        {
            var key = subset.Select(s => s.Count).ToList();
            key.Sort();

            if (!_ipSubSpacesCoalitions.TryAdd(key, [subset]))
            {
                _ipSubSpacesCoalitions[key].Add(subset);
            }

            CalculateBound(key, subset);
        }
    }

    private void CalculateBound(List<int> key, List<List<Goal>> subset)
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
    private static List<List<List<T>>> PartitionGen<T>(List<T> lst)
    {
        var partitions = new List<List<List<T>>>();
        if (lst.Count == 0)
        {
            return [[]];
        }

        var first = lst[0];
        var smallerPartitions = PartitionGen(lst.Skip(1).ToList());

        foreach (var smaller in smallerPartitions)
        {
            for (var n = 0; n < smaller.Count; n++)
            {
                var newPartition = new List<List<T>>(smaller);
                newPartition[n] = new List<T> { first }.Concat(newPartition[n]).ToList();
                partitions.Add(newPartition);
            }
            partitions.Add(new List<List<T>> { new() { first } }.Concat(smaller).ToList());
        }

        return partitions;
    }

    
}