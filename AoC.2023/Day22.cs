using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 22, AdventParts.All)]
public class Day22 : AdventSolution
{
    // [CustomRun(filename: "sus.txt")]
    public override object SolvePartOne()
    {
        var bricks = ParseAllBricks()
            .OrderBy(b => b.EndZ)
            .ThenBy(b => b.StartZ)
            .ToArray();
        
        CalculateSupport(bricks, out var supports, out var supportedBy);

        var count = 0;

        for (var i = 0; i < bricks.Length; i++)
        {
            if (supports[i].Count == 0 || supports[i].All(j => supportedBy[j].Count > 1))
            {
                count++;
            }
        }


        return count;
    }

    public override object SolvePartTwo()
    {
        var startBrick = ParseAllBricks();
        var bricks = CalculateSupport(startBrick, out var supports, out var supportedBy);

        var res = 0;

        for (int i = 0; i < bricks.Length; i++)
        {
            var d = CalcPart2(i, supports, supportedBy);

            WriteLine($"{i} -> {d}");
            res += d;
        }

        return res;
    }

    private int CalcPart2(int brick, List<int>[] supports, List<int>[] supportedBy)
    {
        var q = new Queue<int>();
        var supportersCount = supportedBy.Select(s => s.Count).ToArray();
        q.Enqueue(brick);

        int count = -1;

        while (q.TryDequeue(out var i))
        {
            count++;

            foreach (var j in supports[i])
            {
                supportersCount[j]--;

                if (supportersCount[j] == 0)
                {
                    q.Enqueue(j);
                }
            }
        }

        return count;
    }
    
    private Brick[] CalculateSupport(IEnumerable<Brick> inputBricks, out List<int>[] supports, out List<int>[] supportedBy)
    {
        var bricks = inputBricks
            .OrderBy(b => b.StartZ)
            .ToArray();

        supports = bricks.Select(_ => new List<int>()).ToArray();
        supportedBy = bricks.Select(_ => new List<int>()).ToArray();

        var max = bricks.Aggregate(Point.Zero, (p, b) => Point.Max(p, b.End));
        var grid = new int[max.X + 1, max.Y + 1];

        for (var i = 0; i < max.X + 1; i++)
        for (var j = 0; j < max.Y + 1; j++)
            grid[i, j] = -1;

        for (var i = 0; i < bricks.Length; i++)
        {
            HashSet<int> supporters = new();
            var b = bricks[i];
            var minZ = 0;

            for (var x = b.Start.X; x <= b.End.X; x++)
            for (var y = b.Start.Y; y <= b.End.Y; y++)
                if (grid[x, y] >= 0 && bricks[grid[x, y]].EndZ > minZ)
                    minZ = bricks[grid[x, y]].EndZ;

            for (var x = b.Start.X; x <= b.End.X; x++)
            for (var y = b.Start.Y; y <= b.End.Y; y++)
                if (grid[x, y] >= 0 && bricks[grid[x, y]].EndZ == minZ)
                    supporters.Add(grid[x, y]);

            for (var x = b.Start.X; x <= b.End.X; x++)
            for (var y = b.Start.Y; y <= b.End.Y; y++)
                grid[x, y] = i;

            minZ++;
            supportedBy[i].AddRange(supporters);
            foreach (var supporter in supporters)
            {
                supports[supporter].Add(i);
            }

            bricks[i] = b with {
                StartZ = minZ,
                EndZ = b.EndZ - b.StartZ + minZ
            };
        }

        return bricks;
    }

    private bool Intersects(Brick b1, Brick b2) => Intersects(b1.Start, b1.End, b2.Start, b2.End);

    private bool Intersects(Point a1, Point a2, Point b1, Point b2)
    {
        (a1, a2) = Point.Bounds(a1, a2);
        (b1, b2) = Point.Bounds(b1, b2);

        return b2.X >= a1.X && b1.X <= a2.X && b2.Y >= a1.Y && b1.Y <= a2.Y;
    }

    private Brick ParseBrick(string s, int id)
    {
        var (starts, ends) = s.SmartSplit('~').Unpack2();
        var start = starts.SmartSplit(',').ToInt();
        var end = ends.SmartSplit(',').ToInt();

        var startp = new Point(start);
        var endp = new Point(end);

        var minz = Math.Min(start[2], end[2]);
        var maxz = Math.Max(start[2], end[2]);

        return new Brick(Point.Min(startp, endp), Point.Max(startp, endp), minz, maxz, id);
    }

    private Brick[] ParseAllBricks() =>
        Input
            .Lines
            .Select(ParseBrick)
            .ToArray();

    private record Brick(Point Start, Point End, int StartZ, int EndZ, int Id);
}