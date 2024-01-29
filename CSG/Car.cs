namespace CSG;

public class Car
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Price { get; set; }
    public int Capacity { get; set; }
    public int Occupancy { get; set; }

    public bool StillFree => Occupancy < Capacity;

    public Car(string name, int price, int capacity)
    {
        Name = name;
        Price = price;
        Capacity = capacity;
        Occupancy = 0;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
