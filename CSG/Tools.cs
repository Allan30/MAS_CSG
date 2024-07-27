namespace CSG;

public class EnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>> where T : class
{
    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }
        if (ReferenceEquals(x, y))
        {
            return true;
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

public class ListEqualityComparer : IEqualityComparer<List<int>>
{
    public bool Equals(List<int> x, List<int> y)
    {
        if (x == null || y == null)
            return x == y;

        if (x.Count != y.Count)
            return false;

        for (int i = 0; i < x.Count; i++)
        {
            if (x[i] != y[i])
                return false;
        }

        return true;
    }

    public int GetHashCode(List<int> obj)
    {
        int hash = 17;
        foreach (var i in obj)
        {
            hash = hash * 31 + i;
        }
        return hash;
    }
}