using AoC.Library.Runner;

namespace AoC._2023;

[DateInfo(2023, 14, AdventParts.All)]
public class Day14 : AdventSolution
{
    public override object SolvePartOne()
    {
        var inp = Input.SquareMap();
        MoveNorth(inp);

        return Calc(inp);
    }

    public override object SolvePartTwo()
    {
        var inp = Input.SquareMap();
        var nearest = new int[Math.Max(Input.Width, Input.Height)];
        var cnt = 1000000000;
        int i;
        var memory = new Dictionary<int, int>();


        for (i = 0; i < cnt; i++)
        {
            MoveNorth(inp, nearest);
            MoveWest(inp, nearest);
            MoveSouth(inp, nearest);
            var res = MoveEast(inp, nearest);

            if (memory.ContainsKey(res) && (cnt - i - 1) % (i - memory[res]) == 0)
            {
                break;
            }

            memory[res] = i;

            if (i % 100000 == 0)
            {
                WriteLine(i);
            }
        }

        return Calc(inp);
    }

    private void WriteMap(char[,] inp)
    {
        for (int y = 0; y < inp.GetLength(1); y++)
        {
            for (int x = 0; x < inp.GetLength(0); x++)
                Write(inp[x, y]);
            WriteLine("");
        }

        WriteLine("\n\n");
    }

    private int Calc(char[,] map)
    {
        var w = map.GetLength(0);
        var h = map.GetLength(1);
        var res = 0;

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            if (map[x, y] == 'O')
            {
                res += h - y;
            }
        }

        return res;
    }

    private int MoveNorth(char[,] map, int[]? nearest = null)
    {
        var w = map.GetLength(0);
        var h = map.GetLength(1);
        nearest ??= new int[w];
        var res = 0;

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            if (y == 0) nearest[x] = 0;

            switch (map[x, y])
            {
                case '#':
                    nearest[x] = y + 1;

                    break;

                case 'O':
                    map[x, y] = '.';
                    map[x, nearest[x]] = 'O';
                    res += h - nearest[x];
                    nearest[x]++;

                    break;
            }
        }

        return res;
    }

    private int MoveEast(char[,] map, int[]? nearest = null)
    {
        var w = map.GetLength(0);
        var h = map.GetLength(1);
        nearest ??= new int[h];
        var res = 0;

        for (int x = w - 1; x >= 0; x--)
        for (int y = 0; y < h; y++)
        {
            if (x == w - 1) nearest[y] = w - 1;

            switch (map[x, y])
            {
                case '#':
                    nearest[y] = x - 1;

                    break;

                case 'O':
                    map[x, y] = '.';
                    map[nearest[y], y] = 'O';
                    res += h - x;
                    nearest[y]--;

                    break;
            }
        }

        return MapHashCode(map);
    }

    public int MapHashCode(char[,] map)
    {
        int hash = 0;

        for (int y = 0; y < map.GetLength(1); y++)
        for (int x = 0; x < map.GetLength(0); x++)
            hash = HashCode.Combine(hash, map[x, y]);

        return hash;
    }


    private int MoveSouth(char[,] map, int[]? nearest = null)
    {
        var w = map.GetLength(0);
        var h = map.GetLength(1);
        nearest ??= new int[w];
        var res = 0;

        for (int y = h - 1; y >= 0; y--)
        for (int x = 0; x < w; x++)
        {
            if (y == h - 1) nearest[x] = h - 1;

            switch (map[x, y])
            {
                case '#':
                    nearest[x] = y - 1;

                    break;

                case 'O':
                    map[x, y] = '.';
                    map[x, nearest[x]] = 'O';
                    res += h - nearest[x];
                    nearest[x]--;

                    break;
            }
        }

        return res;
    }

    private int MoveWest(char[,] map, int[]? nearest = null)
    {
        var w = map.GetLength(0);
        var h = map.GetLength(1);
        nearest ??= new int[h];
        var res = 0;

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
        {
            if (x == 0) nearest[y] = 0;

            switch (map[x, y])
            {
                case '#':
                    nearest[y] = x + 1;

                    break;

                case 'O':
                    map[x, y] = '.';
                    map[nearest[y], y] = 'O';
                    res += h - x;
                    nearest[y]++;

                    break;
            }
        }

        return res;
    }
}