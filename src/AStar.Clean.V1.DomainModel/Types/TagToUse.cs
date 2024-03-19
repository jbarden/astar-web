﻿namespace AStar.Clean.V1.DomainModel.Types;

public record TagToUse
{
    private string? value;

    public string? Value
    {
        get => value;
        set => this.value = value?.Trim();
    }

    public static implicit operator TagToUse(string? value) => new() { Value = value };

    public static implicit operator string?(TagToUse value) => value.Value;
}
