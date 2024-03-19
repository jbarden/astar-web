namespace AStar.Clean.V1.DomainModel.Functional.Types;

public abstract class Option<T>
{
    public static implicit operator Option<T>(T value) => new Some<T>(value);

    public static implicit operator Option<T>(None _) => new None<T>();
}
