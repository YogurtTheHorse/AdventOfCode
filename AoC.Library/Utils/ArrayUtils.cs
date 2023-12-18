namespace AoC.Library.Utils;

public static class ArrayUtils
{
    public static IEnumerable<T> Enumerate<T>(this T[,] arr) => arr.Cast<T>();

    public static V[,] SquareMap<T, V>(this T[,] array,  Func<T, V> map)
    {
        var w = array.GetLength(0);
        var h = array.GetLength(1);
        var arr = new V[w, h];

        for (var x = 0; x < w; x++)
        for (var y = 0; y < h; y++)
            arr[x, y] = map(array[x, y]);

        return arr;
    }
}