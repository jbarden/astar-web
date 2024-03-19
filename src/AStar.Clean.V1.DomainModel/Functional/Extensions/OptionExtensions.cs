using AStar.Clean.V1.DomainModel.Functional.Types;

namespace AStar.Clean.V1.DomainModel.Functional.Extensions;

public static class OptionExtensions
{
    public static Option<TResult> Map<T, TResult>(this Option<T> obj, Func<T, TResult> map) => obj is Some<T> some ? new Some<TResult>(map(some)) : new None<TResult>();

    public static Option<T> Filter<T>(this Option<T> obj, Func<T, bool> predicate) => obj is Some<T> some && !predicate(some) ? new None<T>() : obj;

    public static T Reduce<T>(this Option<T> obj, T substitute) => obj is Some<T> some ? some : substitute;

    public static T Reduce<T>(this Option<T> obj, Func<T> substitute) => obj is Some<T> some ? some : substitute();
}
