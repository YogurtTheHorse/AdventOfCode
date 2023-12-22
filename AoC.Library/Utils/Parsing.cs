namespace AoC.Library.Utils;

public static class Parsing
{
    public static string[] SmartSplit(this string input, string sp = " ") =>
        input.Split(sp, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray();

    public static string[] SmartSplit(this string input, char sp) =>
        input.Split(sp, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray();

    public static long[] ToLong(this IEnumerable<string> coll) =>
        coll.Select(long.Parse).ToArray();

    public static int[] ToInt(this IEnumerable<string> coll) =>
        coll.Select(ToInt).ToArray();

    public static int ToInt(this string coll) => int.Parse(coll);

    public static (T, T) Unpack2<T>(this IList<T> l) => (l[0], l[1]);

    public static (T, T, T) Unpack3<T>(this IList<T> l) => (l[0], l[1], l[2]);
}