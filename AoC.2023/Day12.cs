using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 12, AdventParts.All)]
[CustomExample(
    """
    ???.### 1,1,3
    .??..??...?##. 1,1,3
    ?#?#?#?#?#?#?#? 1,3,1,6
    ????.#...#... 4,1,1
    ????.######..#####. 1,6,5
    ?###???????? 3,2,1
    """,
    "21",
    "525152"
)]
public class Day12 : AdventSolution
{
    private long[,]? _globalCache;
    private readonly int[] _damagedLeft = new int[300];

    public override object SolvePartOne(AdventInput input) => input
        .Lines
        .Select(SolveLine)
        .Sum();

    [CustomRun("??????????????????????????????????????????????????????? 1,2,1")]
    public override object SolvePartSecond(AdventInput input) => input
        .Lines
        .Select(Expand)
        .Select(SolveLine)
        .Sum();

    private static string Expand(string l)
    {
        var (ll, ss) = l.SmartSplit().Unpack2();
        var newLine = string.Join("?", Enumerable.Range(0, 5).Select(_ => ll));
        var newCond = string.Join(",", Enumerable.Range(0, 5).Select(_ => ss));

        return $"{newLine} {newCond}";
    }

    private long SolveLine(string s)
    {
        // Console.Write($"{s} -> ");

        var (tilesString, groupsString) = s.SmartSplit().Unpack2();
        var groups = groupsString.SmartSplit(",").ToInt();

        var cp = tilesString.Count(c => c is '#');

        for (var i = 0; i < tilesString.Length; i++)
        {
            _damagedLeft[i] = cp;
            if (tilesString[i] == '#') cp--;
        }

        _damagedLeft[tilesString.Length] = cp;

        long localRes = Solve(tilesString, groups, 0, 0, _damagedLeft, CreateCache());

        // Console.WriteLine($"{localRes}");

        return localRes;
    }

    private long Solve(string tiles, int[] groups, int usedTiles, int usedGroups, int[] damagedLeft, long[,] cache)
    {
        if (cache[usedTiles, usedGroups] >= 0) return cache[usedTiles, usedGroups];

        long options = 0L;

        if (usedGroups == groups.Length)
        {
            if (damagedLeft[usedTiles] == 0)
            {
                options = 1;
            }
        }
        else if (usedGroups > groups.Length)
        {
            options = 0;
        }
        else
        {
            var g = groups[usedGroups];

            for (var i = usedTiles; i < tiles.Length && (i == 0 || tiles[i - 1] != '#'); i++)
            {
                var lastChar = TryFit(tiles, i, g);

                if (lastChar is null)
                    continue;

                options += Solve(tiles, groups, lastChar.Value, usedGroups + 1, damagedLeft, cache);
            }
        }

        cache[usedTiles, usedGroups] = options;

        return options;
    }

    private int? TryFit(string tiles, int place, int groupLength)
    {
        int hasFit = 0;
        int brokenCount = 0;

        for (int i = place; i < tiles.Length; i++)
        {
            var c = tiles[i];

            if (c == '.')
            {
                hasFit++;

                break;
            }

            if (c == '#')
            {
                brokenCount++;
            }
            else if (c == '?')
            {
                if (brokenCount < groupLength)
                {
                    brokenCount++;
                }
                else
                {
                    hasFit++;

                    break;
                }
            }

            hasFit++;

            if (brokenCount > groupLength) break;
        }

        int? lastChar = brokenCount == groupLength
            ? place + hasFit
            : null;

        return lastChar;
    }

    private long[,] CreateCache()
    {
        _globalCache ??= new long[500, 150];

        for (var i = 0; i < 500; i++)
        for (var j = 0; j < 150; j++)
            _globalCache[i, j] = -1;

        return _globalCache;
    }
}