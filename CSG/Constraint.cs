namespace CSG;

public record Constraint
{
    public (bool IsSmoker, double Penalty) SmokerConstraint;
    public (bool IsTalker, double Penalty) TalkerConstraint;
    public (int MaxPassengers, double Penalty) PassengersConstraint;
    public List<(DateTime Date, double Penalty)> AvailableDatesConstraint;
    public double MaxAcceptablePrice;
}
