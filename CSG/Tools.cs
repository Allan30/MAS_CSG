namespace CSG;

class ListEqualityComparer<T> : IEqualityComparer<List<T>> where T : class
{
    public bool Equals(List<T> x, List<T> y)
    {
        return x.SequenceEqual(y, EqualityComparer<T>.Default);
    }

    public int GetHashCode(List<T> obj)
    {
        int hash = 17;
        foreach (var item in obj)
        {
            hash = hash * 31 + item.GetHashCode();
        }
        return hash;
    }
}