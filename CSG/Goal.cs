namespace CSG;

public class Goal
{
    public Goal(string? name, string? originCity, string? destinationCity, DateTime departureTime, DateTime arrivalTime)
    {
        Name = name;
        OriginCity = originCity;
        DestinationCity = destinationCity;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        IsEmpty = false;
    }

    public Goal()
    {
        IsEmpty = true;
    }
    public string? Name { get; }
    public string? OriginCity { get; }
    public string? DestinationCity { get; }
    public DateTime DepartureTime { get; }
    public DateTime ArrivalTime { get; }
    public Car? Car { get; set; }
    
    public bool IsEmpty { get; set; }
}