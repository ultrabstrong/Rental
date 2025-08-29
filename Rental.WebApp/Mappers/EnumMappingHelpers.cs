using System;

namespace Rental.WebApp.Mappers;

public static class EnumMappingHelpers
{
    public static TTarget? MapEnum<TSource, TTarget>(this TSource? value)
        where TSource : struct, Enum
        where TTarget : struct, Enum
    {
        if (!value.HasValue) return null;
        return (TTarget)Enum.ToObject(typeof(TTarget), Convert.ToInt32(value.Value));
    }
}
