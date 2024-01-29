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

    protected double CalculateCoalitionValue(List<Goal> goals)
    {
        var filteredCars = Cars
            .Where(car => car.Capacity >= goals.Count).ToList();

        var cityPenality = (goals.Select(goal => goal.OriginCity).Distinct().Count() +
                            goals.Select(goal => goal.DestinationCity).Distinct().Count() - 2) * 1000000;
        
        if(filteredCars.Count > 0)
        {
            var minCar = filteredCars.MinBy(car => car.Price);
            foreach (var goal in goals)
            {
                goal.Car = minCar;
            }
            return (minCar.Price / goals.Count) + cityPenality;
        }
        return double.MaxValue;

        

        /*
        var carsOccupation = new Dictionary<Car, int>();

        foreach (var goal in goals)
        {
            var bestAvailableCar = Cars.Where(v => v.StillFree).MinBy(v => v.Price); ;

            if (bestAvailableCar != null)
            {
                carsOccupation.TryAdd(bestAvailableCar, 0);
                carsOccupation[bestAvailableCar]++;
                bestAvailableCar.Occupancy++;
            }
        }

        var sum = 0;
        foreach (var kvp  in carsOccupation)
        {
            sum += kvp.Key.Price / kvp.Value;
            kvp.Key.Occupancy = 0;
        }
        return sum;

        */

    }

    protected List<List<Goal>> GetSubsetsOfSize(int size)
    {
        var subsets = new List<List<Goal>>();
        GetSubsetsOfSizeRecursive(size, 0, [], subsets);
        return subsets;
    }

    private void GetSubsetsOfSizeRecursive(int size, int index, IList<Goal> currentSubset, ICollection<List<Goal>> subsets)
    {
        if (size == 0)
        {
            subsets.Add([..currentSubset]);
            return;
        }

        for (var i = index; i < Goals.Count; i++)
        {
            //if ((currentSubset.Count == 0) || (currentSubset[0].OriginCity == Goals[i].OriginCity && currentSubset[0].DestinationCity == Goals[i].DestinationCity))
            currentSubset.Add(Goals[i]);
            GetSubsetsOfSizeRecursive(size - 1, i + 1, currentSubset, subsets);
            currentSubset.RemoveAt(currentSubset.Count - 1);
        }
    }
}