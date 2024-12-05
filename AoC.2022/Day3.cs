using System.Reflection.Metadata.Ecma335;
using AoC.Library.Runner;
using AoC.Library.Utils;
using Console = Colorful.Console;

namespace AoC._2022;

[DateInfo(2022, 3, AdventParts.PartOne)]
public class Day3 : AdventSolution
{
    public override object SolvePartOne() => Input
        .Lines
        .Select(SolveLine)
        .Distinct()
        .Sum();

    public override object SolvePartTwo() => throw new NotImplementedException();

    private int SolveLine(string line)
    {
        var fst = line[..(line.Length / 2)];
        var snd = line[(line.Length / 2)..];


        return fst.Where(snd.Contains).Distinct().Select(Convert).Sum();
    }
    
    int Convert(char c) => char.IsLower(c)
        ? c - 'a' + 1
        : c - 'A' + 27;
}