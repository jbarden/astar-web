namespace AStar.Clean.V1.DomainModel.Functional.Types;

public static class Option
{
    public static Option<T> Optional<T>(this T obj) => new Some<T>(obj);
}
