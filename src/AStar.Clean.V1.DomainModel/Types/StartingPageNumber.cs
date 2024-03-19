namespace AStar.Clean.V1.DomainModel.Types;

public record StartingPageNumber
{
    private int value;

    public int Value
    {
        get => value;
        set
        {
            if (value <= 0)
            {
                value = 1;
            }

            this.value = value;
        }
    }

    public static implicit operator StartingPageNumber(int value) => new() { Value = value };

    public static implicit operator int(StartingPageNumber value) => value.Value;
}
