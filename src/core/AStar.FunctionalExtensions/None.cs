namespace AStar.FunctionalExtensions;

public class None
{
    public static None Value { get; } = new();
    public static None<T> Of<T>() => new();
}