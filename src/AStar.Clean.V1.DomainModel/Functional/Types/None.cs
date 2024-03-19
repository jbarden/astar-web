namespace AStar.Clean.V1.DomainModel.Functional.Types;

public class None
{
    private None()
    {
    }

    public static None Value { get; } = new();

    public static None<T> Of<T>() => new();
}
