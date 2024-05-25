namespace AStar.FunctionalExtensions;

public sealed class None<T> : Option<T>
{
    public override string ToString() => $"None";
}
