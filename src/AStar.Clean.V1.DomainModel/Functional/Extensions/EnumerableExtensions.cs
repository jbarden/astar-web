using AStar.Clean.V1.DomainModel.Functional.Types;

namespace AStar.Clean.V1.DomainModel.Functional.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TResult> Flatten<T, TResult>(this IEnumerable<T> sequence, Func<T, Option<TResult>> map) =>
        sequence.Select(map)
            .OfType<Some<TResult>>()
            .Select(x => (TResult)x);

    public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) =>
        sequence.Where(predicate)
            .Select<T, Option<T>>(x => x)
            .DefaultIfEmpty(None.Value)
            .First();
}
