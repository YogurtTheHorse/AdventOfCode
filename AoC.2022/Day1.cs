using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2022;

[DateInfo(2022, 1, AdventParts.All)]
public class Day1 : AdventSolution
{
    public override object SolvePartOne()
    {
        int max = 0;
        int current = 0;
        
        foreach (var line in Input.FullLines)
        {
            if (line.Length == 0)
            {
                current = 0;
            }
            else
            {
                current += line.ToInt();
                max = Math.Max(current, max);
            } 
        }

        return max;
    }

    public override object SolvePartTwo()
    {
        List<int> values = new();
        int current = 0;
        
        foreach (var line in Input.FullLines)
        {
            if (line.Length == 0)
            {
                values.Add(current);
                current = 0;
            }
            else
            {
                current += line.ToInt();
            } 
        }

        return values.OrderDescending().Take(3).Sum();
    }
}