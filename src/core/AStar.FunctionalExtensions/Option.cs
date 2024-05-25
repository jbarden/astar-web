namespace AStar.FunctionalExtensions;

public static class Option
{
    public static Option<T> Optional<T>(this T obj) => new Some<T>(obj);
}
