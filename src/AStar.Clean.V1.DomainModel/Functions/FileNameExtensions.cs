﻿using AStar.Clean.V1.DomainModel.Types;

namespace AStar.Clean.V1.DomainModel.Functions;

public static class FileNameExtensions
{
    public static bool HasValue(this FileName tagToUse) => !tagToUse.HasNoValue();

    public static bool HasNoValue(this FileName tagToUse) => string.IsNullOrWhiteSpace(tagToUse.Value);
}
