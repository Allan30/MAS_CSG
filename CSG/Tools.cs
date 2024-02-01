namespace CSG;

public class ListEqualityComparer<T> : IEqualityComparer<IEnumerable<T>> where T : class
{
    public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }
        if (x is null || y is null)
        {
            return false;
        }
        return x.SequenceEqual(y, EqualityComparer<T>.Default);
    }

    public int GetHashCode(IEnumerable<T> obj)
    {
        int hash = 17;
        foreach (var item in obj)
        {
            hash = hash * 31 + item.GetHashCode();
        }
        return hash;
    }
}