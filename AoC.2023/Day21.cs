using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 21, AdventParts.PartTwo)]
public class Day21 : AdventSolution
{
    private readonly Direction[] _dirs = {
        Direction.Up, Direction.Right, Direction.Down, Direction.Left
    };

    public override object SolvePartOne()
    {
        var canVisit = Input.SquareMap(c => c is '.' or 'S');
        var visisted = Input.SquareMap(_ => false);
        var stepsToGet = Input.SquareMap(_ => -1);

        var start = new Point(0, 0);

        for (int x = 0; x < Input.Width; x++)
        for (int y = 0; y < Input.Height; y++)
            if (Input[x, y] == 'S')
            {
                start = (x, y);
            }

        int cnt = 0;

        WriteLine(start);
        var q = new Queue<(Point, int)>();
        q.Enqueue((start, 0));
        int stepsAllowed = 64;

        while (q.Any())
        {
            var (p, steps) = q.Dequeue();

            if (steps > stepsAllowed) break;

            if (!p.InBounds(Input.Width, Input.Height)) continue;
            if (visisted[p.X, p.Y]) continue;
            if (!canVisit[p.X, p.Y]) continue;

            stepsToGet[p.X, p.Y] = steps;
            visisted[p.X, p.Y] = true;

            if ((p.X + p.Y) % 2 == (start.X + start.Y) % 2)
            {
                WriteLine($"{p.X} {p.Y}");
                cnt++;
            }

            foreach (var dir in _dirs)
            {
                q.Enqueue((p + dir, steps + 1));
            }
        }

        return cnt;
    }

    public override object SolvePartTwo()
    {
        var canVisit = Input.SquareMap(c => c is '.' or 'S');
        var visisted = new HashSet<ValueTuple<Point, int>>();

        var start = new Point(0, 0);

        for (int x = 0; x < Input.Width; x++)
        for (int y = 0; y < Input.Height; y++)
            if (Input[x, y] == 'S')
            {
                start = (x, y);
            }

        var visitedAtStep = new Dictionary<int, int> {
            [0] = 1
        };

        WriteLine(start);
        var q = new Queue<(Point, int)>();
        q.Enqueue((start, 0));
        int stepsAllowed = start.X + Input.Width * 2;
        
        // LUCY IN THE SKY WITH DIAMONDS
        // https://i.imgur.com/j3tgjyR.jpg
        // I MEAN IT
        // I CAN SEE IT
        // https://i.imgur.com/SpIZIxG.jpg
        // I AM LUCY

        while (q.Any())
        {
            var (p, steps) = q.Dequeue();
            
            if (steps > stepsAllowed) continue;

            ValueTuple<Point, int> c = (p, steps);

            var l = p.Loop(Input.Width, Input.Height);

            if (!canVisit[l.X, l.Y]) continue;
            if (visisted.Contains(c)) continue;

            visisted.Add(c);

            visitedAtStep.TryAdd(steps, 0);
            visitedAtStep[steps]++;
            
            foreach (var dir in _dirs)
            {
                q.Enqueue((p + dir, steps + 1));
            }
        }

        var nums = new List<long>();
        for (int step = start.X + Input.Width; step <= stepsAllowed; step += Input.Width)
        {
            var n = visitedAtStep[step] - visitedAtStep[step - Input.Width];
            WriteLine(n);
            nums.Add(n);
        }
        
        var inc = nums[1] - nums[0];

        var s = stepsAllowed;
        long cnt = visitedAtStep[s];
        long currInc = visitedAtStep[s] - visitedAtStep[s - Input.Width];

        while (s != 26501365)
        {
            currInc += inc;
            cnt += currInc;
            s += Input.Width;
        }
        
        return cnt;
    }
}