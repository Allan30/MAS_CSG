namespace CSG;

public class Car
{
    public Guid Id { get; } = Guid.NewGuid();
    public int Price { get; set; }
    public int Capacity { get; set; }
    public int Occupancy { get; set; }

    public override int GetHashCode() => Id.GetHashCode();
}
