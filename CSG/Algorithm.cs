namespace CSG;

public abstract class Algorithm
{
    protected Algorithm(List<Goal> goals, List<Car> cars)
    {
        Goals = goals;
        Cars = cars;
    }

    protected List<Goal> Goals { get; }

    protected List<Car> Cars { get; }

    public abstract List<List<Goal>> GetOptimalCoalitionStructure();

    protected Car? GetMinCar(List<Goal> goals)
    {
        var filteredCars = Cars
            .Where(car => car.Capacity >= goals.Count)
            .ToList();
        
        if (filteredCars.Count > 0)
        {
            return filteredCars.MinBy(car => car.Price);
        }
        return null;
    }

    protected double CalculateCoalitionValue(List<Goal> goals)
    {
        var cityPenality = (goals.Select(goal => goal.OriginCity).Distinct().Count() +
                            goals.Select(goal => goal.DestinationCity).Distinct().Count() - 2) * 1_000_000;

        var minutePenality = goals.Max(goal => goal.DepartureTime).Subtract(goals.Min(goal => goal.DepartureTime)).TotalMinutes * (goals.Count-1);
        var minCar = GetMinCar(goals);
        if (minCar is not null)
        {
            var carPricePenality = minCar.Price / goals.Count;
            return carPricePenality + minutePenality + cityPenality;
        }
        return double.MaxValue;

    }

    protected List<List<Goal>> GetSubsetsOfSize(int size, List<Goal> goals)
    {
        var subsets = new List<List<Goal>>();
        GetSubsetsOfSizeRecursive(size, 0, goals,[], subsets);
        return subsets;
    }

    private void GetSubsetsOfSizeRecursive(int size, int index, List<Goal> goals, IList<Goal> currentSubset, ICollection<List<Goal>> subsets)
    {
        if (size == 0)
        {
            subsets.Add([..currentSubset]);
            return;
        }

        for (var i = index; i < goals.Count; i++)
        {
            //if ((currentSubset.Count == 0) || (currentSubset[0].OriginCity == Goals[i].OriginCity && currentSubset[0].DestinationCity == Goals[i].DestinationCity))
            currentSubset.Add(goals[i]);
            GetSubsetsOfSizeRecursive(size - 1, i + 1, goals,currentSubset, subsets);
            currentSubset.RemoveAt(currentSubset.Count - 1);
        }
    }

    protected List<List<List<Goal>>> GetAllPossibilitiesToSplitListIntoTwoLists(List<Goal> goals)
    {
        var result = new List<List<List<Goal>>>();
        if (goals.Count == 2) return [[[goals[0]], [goals[1]]]];
        for (var size = 1; size <= goals.Count / 2; size++)
        {
            foreach (var subset in GetSubsetsOfSize(size, goals))
            {
                List<List<Goal>> twoListToAdd = [
                    subset,
                    goals.Where(goal => !subset.Contains(goal)).ToList()
                ];
                result.Add(twoListToAdd);
            }
        }

        return result;
    }
}