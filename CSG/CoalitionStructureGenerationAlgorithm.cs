
namespace CSG;

public class CoalitionStructureGenerationAlgorithm : Algorithm
{
    public CoalitionStructureGenerationAlgorithm(List<Goal> goals, List<Car> cars) : base(goals, cars) {}

    public override List<Goal> GetOptimalCoalitionStructure()
    {
        List<Goal> optimalCoalitionStructure = [..Goals];

        for (int i = 0; i < optimalCoalitionStructure.Count; i++)
        {
            foreach (List<int> subset in GetSubsetsOfSize(Enumerable.Range(0, optimalCoalitionStructure.Count).ToList(), i + 1))
            {
                List<Goal> coalition = subset.Select(index => optimalCoalitionStructure[index]).ToList();
                if (coalition.All(goal => goal.Car is not null))
                {
                    int totalCost = coalition.Sum(goal => goal.Car!.Price);
                    if (totalCost < optimalCoalitionStructure.Sum(goal => goal.Car!.Price))
                    {
                        optimalCoalitionStructure = coalition;
                    }
                }
            }
        }

        return optimalCoalitionStructure;
    }

    private static IEnumerable<IEnumerable<int>> GetSubsetsOfSize(List<int> cars, int size)
    {
        if (size == 0)
        {
            yield return Enumerable.Empty<int>();
        }
        else
        {
            for (int i = 0; i < cars.Count; i++)
            {
                int car = cars[i];
                foreach (List<int> subset in GetSubsetsOfSize(cars.Skip(i + 1).ToList(), size - 1))
                {
                    subset.Insert(0, car);
                    yield return subset;
                }
            }
        }
    }
}
