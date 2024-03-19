namespace AStar.Clean.V1.DomainModel.Functional.Types;

public abstract class Either<TLeft, TRight>
{
    public static implicit operator Either<TLeft, TRight>(TLeft obj) => new Left<TLeft, TRight>(obj);

    public static implicit operator Either<TLeft, TRight>(TRight obj) => new Right<TLeft, TRight>(obj);
}
