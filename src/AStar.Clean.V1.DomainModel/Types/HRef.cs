﻿namespace AStar.Clean.V1.DomainModel.Types;

public class HRef
{
    private string? value;

    public string? Value
    {
        get => value;
        set => this.value = value?.Trim();
    }

    public static implicit operator HRef(string? value) => new() { Value = value };

    public static implicit operator string?(HRef value) => value.Value;
}
