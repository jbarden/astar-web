namespace AStar.Clean.V1.DomainModel.Functional.Types;

public sealed class Some<T> : Option<T>
{
    public Some(T content)
    {
        Content = content;
    }

    private T Content { get; }

    public static implicit operator T(Some<T> value) => value.Content;

    public override string ToString() => $"Some {Content?.ToString() ?? "<null>"}";
}
