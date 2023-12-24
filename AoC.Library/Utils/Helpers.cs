namespace AoC.Library.Utils;

public static class Helpers
{
    public static T Id<T>(T t) => t;

    public static bool NotNull(object? o) => o != null;
}