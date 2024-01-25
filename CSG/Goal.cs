namespace CSG;

public class Goal
{
    public Goal(string originCity, string destinationCity, DateTime departureTime, DateTime arrivalTime)
    {
        OriginCity = originCity;
        DestinationCity = destinationCity;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
    }
    public string OriginCity { get; }
    public string DestinationCity { get; }
    public DateTime DepartureTime { get; }
    public DateTime ArrivalTime { get; }
    public Car? Car { get; set; }
}