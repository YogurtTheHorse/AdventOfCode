using System.Reflection.Metadata.Ecma335;
using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 9, AdventParts.All)]
public class Day9 : AdventSolution
{
    public override object SolvePartOne() => Solve(true);

    public override object SolvePartTwo() => Solve(false);

    private long Solve(bool isFirst) => Input
        .Lines
        .Select(l => l.SmartSplit().ToInt())
        .Select(nums => Next(nums, isFirst))
        .Sum();

    private int Next(int[] nums, bool isFirst)
    {
        if (nums.Length == 0) return 0;
        if (nums.All(n => nums[0] == n)) return nums[0];

        var nn = new int[nums.Length - 1];

        for (int i = 0; i < nums.Length - 1; i++)
        {
            nn[i] = nums[i + 1] - nums[i];
        }

        return isFirst
            ? nums[^1] + Next(nn, isFirst)
            : nums[0] - Next(nn, isFirst);
    }
}