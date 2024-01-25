namespace CSG;

public abstract class Algorithm
{
    protected Algorithm(List<Goal> goals, List<Car> cars)
    {
        Goals = goals;
        Cars = cars;
    }

    public List<Goal> Goals { get; }

    public List<Car> Cars { get; }

    public abstract List<Goal> GetOptimalCoalitionStructure();

    static IEnumerable<List<int>> GetSubsetsOfSize(List<int> cars, int size)
    {
        int n = cars.Length;

        for (int i = 0; i < (1 << n); i++)
        {
            if (CountBits(i) == size)
            {
                List<int> subset = new List<int>();
                for (int j = 0; j < n; j++) 
                {
                    if ((i & (1 << j)) != 0)
                    {
                        subset.Add(array[j]);
                    }
                }
                yield return subset;
            }
        }
    }
}