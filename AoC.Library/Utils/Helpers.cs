using System.Numerics;

namespace AoC.Library.Utils;

public static class Helpers
{
    public static T Id<T>(T t) => t;

    public static bool NotNull(this object? o) => o != null;

    public static IEnumerable<T> Single<T>(this T t)
    {
        yield return t;
    }

    public static T Loop<T>(this T value, T bound) where T : INumber<T>
    {
        var b = (value + bound * (T.Abs(value) / bound)) % bound;

        return T.IsNegative(b)
            ? bound + b
            : b;
    }
}
