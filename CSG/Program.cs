using Bogus;

namespace CSG;

public class Program
{
    public static void Main(string[] args)
    {
        var cars = new Faker<Car>()
            .RuleFor(c => c.Capacity, f => f.Random.Int(2, 5))
            .Generate(10);

        List<string> cities = ["Paris", "Lyon", "Marseille"];

        var goals = new Faker<Goal>()
            .RuleFor(g => g.OriginCity, f => cities[f.Random.Int(0, cities.Count - 1)])
            .RuleFor(g => g.DestinationCity, f => cities[f.Random.Int(0, cities.Count - 1)])
            .RuleFor(g => g.DepartureTime, f => f.Date.Future())
            .RuleFor(g => g.ArrivalTime, (f, g) => f.Date.Between(g.DepartureTime.AddHours(1), g.DepartureTime.AddHours(3)))
            .Generate(10);
    }
}
