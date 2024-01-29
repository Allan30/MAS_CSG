using Bogus;

namespace CSG;

public class Program
{
    public static void Main(string[] args)
    {
        /*
        var cars = new Faker<Car>()
            .RuleFor(c => c.Capacity, f => f.Random.Int(2, 5))
            .Generate(10);
            */

        List<string> cities = ["Paris", "Lyon", "Marseille"];
        List<Car> cars = 
        [
            new Car("car1", 100, 4), 
            new Car("car2", 80, 3), 
            new Car("car3", 200, 5)
        ];
        List<Goal> goals =
        [
            new Goal("agent1", "Paris", "Lyon", DateTime.Now, DateTime.Now),
            new Goal("agent2","Paris", "Lyon", DateTime.Now, DateTime.Now),
            new Goal("agent3","Paris", "Lyon", DateTime.Now, DateTime.Now),
            new Goal("agent4","Paris", "Lyon", DateTime.Now, DateTime.Now),
            new Goal("agent5","Lyon", "Paris", DateTime.Now, DateTime.Now),
            new Goal("agent6","Lyon", "Paris", DateTime.Now, DateTime.Now),
            new Goal("agent7","Marseille", "Lyon", DateTime.Now, DateTime.Now)
        ];
        

                /*
        var goals = new Faker<Goal>()
            .RuleFor(g => g.OriginCity, f => cities[f.Random.Int(0, cities.Count - 1)])
            .RuleFor(g => g.DestinationCity, f => cities[f.Random.Int(0, cities.Count - 1)])
            .RuleFor(g => g.DepartureTime, f => f.Date.Future())
            .RuleFor(g => g.ArrivalTime, (f, g) => f.Date.Between(g.DepartureTime.AddHours(1), g.DepartureTime.AddHours(3)))
            .Generate(10);
            */

        var algorithm = new CoalitionStructureGenerationAlgorithm(goals, cars);

        var coalitions = algorithm.GetOptimalCoalitionStructure();

        Console.WriteLine("=======================================================");
        foreach (var coalition in coalitions)
        {
            Console.WriteLine("Coalition for car " + coalition[0].Car.Name + ": ");
            foreach (var agent in coalition)
            {
                Console.WriteLine(agent.Name);
            }
        }
        Console.WriteLine("=======================================================");
    }
}
