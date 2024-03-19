namespace AStar.Clean.V1.DomainModel.Functional.Types;

public sealed class None<T> : Option<T>
{
    public override string ToString() => "None";
}
