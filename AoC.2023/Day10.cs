using AoC.Library.Runner;

namespace AoC._2023;

[DateInfo(2023, 10, AdventParts.All)]
public class Day10 : AdventSolution
{
    private int _startX = -1;
    private int _startY = -1;

    private int[,] _distances;
    private bool[,] _visits;
    private bool[,] _biggerField;

    private readonly (int, int)[] _around = {
        (0, -1), (-1, 0), (1, 0), (0, 1)
    };

    [CustomExample(
        """
        .....
        .S-7.
        .|.|.
        .L-J.
        .....
        """,
        4
    )]
    public override object SolvePartOne()
    {
        for (int i = 0; i < Input.Lines.Length; i++)
        {
            _startX = Input.Lines[i].IndexOf('S');

            if (_startX >= 0)
            {
                _startY = i;

                break;
            }
        }

        _distances = new int[Input.Width, Input.Height];
        _visits = new bool[Input.Width, Input.Height];

        Queue<(int x, int y)> q = new();
        q.Enqueue((_startX, _startY));

        var mx = 0;

        while (q.Any())
        {
            var c = q.Dequeue();
            _visits[c.x, c.y] = true;
            var cd = _distances[c.x, c.y];

            var nn = GetNeig(c.x, c.y);

            foreach (var (x, y) in nn)
            {
                if (_visits[x, y]) continue;

                _distances[x, y] = cd + 1;

                q.Enqueue((x, y));

                if (cd + 1 > mx)
                {
                    mx = cd + 1;
                }
            }
        }

        return mx;
    }

    [CustomExample(
        """
        FF7FSF7F7F7F7F7F---7
        L|LJ||||||||||||F--J
        FL-7LJLJ||||||LJL-77
        F--JF--7||LJLJIF7FJ-
        L---JF-JLJIIIIFJLJJ7
        |F|F-JF---7IIIL7L|7|
        |FFJF7L7F-JF7IIL---7
        7-L-JL7||F7|L7F-7F7|
        L.L7LFJ|||||FJL7||LJ
        L7JLJL-JLJLJL--JLJ.L
        """,
        10
    )]
    public override object SolvePartSecond()
    {
        SolvePartOne();
        _biggerField = new bool[Input.Width * 2, Input.Height * 2];

        for (int i = 0; i < Input.Width; i++)
        for (int j = 0; j < Input.Height; j++)
        {
            if (_visits[i, j])
            {
                _biggerField[i * 2, j * 2] = true;

                var nn = GetNeig(i, j);

                foreach (var (x, y) in nn)
                {
                    var dx = x - i;
                    var dy = y - j;

                    _biggerField[i * 2 + dx, j * 2 + dy] = true;
                }
            }
        }

        int res = 0;

        for (int i = 0; i < Input.Width * 2; i++)
        for (int j = 0; j < Input.Height * 2; j++)
        {
            res += IsClosed(i, j);
        }

        return res;
    }

    int IsClosed(int xx, int yy)
    {
        if (_biggerField![xx, yy]) return 0;

        int rr = 0;
        Queue<(int x, int y)> qq = new();
        qq.Enqueue((xx, yy));

        bool bad = false;

        var added = new List<(int x, int y)>();

        while (qq.Any())
        {
            var c = qq.Dequeue();

            if (_biggerField![c.x, c.y]) continue;

            _biggerField![c.x, c.y] = true;

            if (c.x % 2 == 0 && c.y % 2 == 0)
            {
                rr++;
            }

            added.Add(c);

            var nn = _around.Select(a => (a.Item1 + c.x, a.Item2 + c.y));

            foreach (var cc in nn)
            {
                if (!GoodEnoghM(cc.Item1, cc.Item2, true))
                {
                    bad = true;

                    continue;
                }

                if (_biggerField![cc.Item1, cc.Item2]) continue;

                qq.Enqueue(cc);
            }
        }

        // if (!bad)
        // {
        //     foreach (var (x, y) in added)
        //     {
        //         Console.WriteLine($"{x}, {y}");
        //     }
        //
        //     Console.WriteLine($"---");
        // }

        return bad ? 0 : rr;
    }

    (int, int)[] GetNeig(int x, int y, bool lookForClosed = false)
    {
        if (!GoodEnoghM(x, y)) return Array.Empty<(int, int)>();

        var t = Input.Lines[y][x];

        var r = t switch {
            _ when lookForClosed => _around.Select(a => (a.Item1 + x, a.Item2 + y)),
            '|' => new[] {
                (x, y - 1), (x, y + 1)
            },
            '-' => new[] {
                (x - 1, y), (x + 1, y)
            },
            'L' => new[] {
                (x + 1, y), (x, y - 1)
            },
            'J' => new[] {
                (x - 1, y), (x, y - 1)
            },
            '7' => new[] {
                (x - 1, y), (x, y + 1)
            },
            'F' => new[] {
                (x + 1, y), (x, y + 1)
            },
            'S' => _around
                .Select(a => (a.Item1 + x, a.Item2 + y))
                .Where(a => GetNeig(a.Item1, a.Item2).Contains((x, y))),
            _ => lookForClosed
                ? _around.Select(a => (a.Item1 + x, a.Item2 + y))
                : Array.Empty<(int x, int y)>(),
        };

        return lookForClosed ? r.ToArray() : Filter(r);
    }

    (int, int)[] Filter(IEnumerable<(int, int)> i)
    {
        return i.Where(GoodEnogh).ToArray();
    }

    bool GoodEnogh((int x, int y) p) => GoodEnoghM(p.x, p.y);

    bool GoodEnoghM(int x, int y, bool isBig = false)
    {
        var w = isBig ? Input.Width * 2 : Input.Width;
        var h = isBig ? Input.Height * 2 : Input.Height;

        return x >= 0 && x < w && y >= 0 && y < h;
    }
}