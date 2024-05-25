namespace AStar.FunctionalExtensions;

public sealed class Some<T> : Option<T>
{
    public Some(T content) => Content = content;

    public T Content { get; }

    public override string ToString() => $"Some {Content?.ToString() ?? "<null>"}";
}
