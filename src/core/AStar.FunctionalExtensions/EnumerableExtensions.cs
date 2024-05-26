namespace AStar.FunctionalExtensions;

public static class EnumerableExtensions
{
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
                        => sequence.Where(predicate)
                                   .Select<T, Option<T>>(x => x)
                                   .DefaultIfEmpty(None.Value)
                                   .First();
}
