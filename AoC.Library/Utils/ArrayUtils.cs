namespace AoC.Library.Utils;

public static class ArrayUtils
{
    public static IEnumerable<T> Enumerate<T>(this T[,] arr) => arr.Cast<T>();
}