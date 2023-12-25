namespace AoC.Library.Utils;

public static class Helpers
{
    public static T Id<T>(T t) => t;

    public static bool NotNull(this object? o) => o != null;

    public static IEnumerable<T> Single<T>(this T t)
    {
        yield return t;
    }
}