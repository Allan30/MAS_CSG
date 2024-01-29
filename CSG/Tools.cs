namespace CSG;

class ListEqualityComparer<T> : IEqualityComparer<List<T>>
{
    public bool Equals(List<T> x, List<T> y)
    {
        if (x == null || y == null)
            return false;

        return x.SequenceEqual(y, EqualityComparer<T>.Default);
    }

    public int GetHashCode(List<T> obj)
    {
        if (obj == null)
            return 0;

        int hash = 17;
        foreach (var item in obj)
        {
            hash = hash * 31 + item.GetHashCode();
        }
        return hash;
    }
}