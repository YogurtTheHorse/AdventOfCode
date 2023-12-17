using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2022;

[DateInfo(2022, 2, AdventParts.All)]
public class Day2 : AdventSolution
{
    public override object SolvePartOne() => Input
        .Lines
        .Select(SolveLinePart1)
        .Sum();

    public override object SolvePartTwo()  => Input
        .Lines
        .Select(SolveLinePart2)
        .Sum();

    private int SolveLinePart1(string line)
    {
        var (opp, you) = line.SmartSplit().Select(Parse).ToArray().Unpack2();

        if (opp == you) return 3 + (int)you;
        if ((int)opp == ((int)you) % 3 + 1) return (int)you;

        return 6 + (int)you;
    }

    private int SolveLinePart2(string line)
    {
        var inps = line.SmartSplit().Select(Parse).ToArray();
        var opp = inps[0];
        var act = (Act)inps[1];

        var you = act switch {
            Act.Draw => opp,
            Act.Lose => opp switch {
                RPS.Rock => RPS.Scissors,
                RPS.Paper => RPS.Rock,
                _ => RPS.Paper
            },
            _ => opp switch {
                RPS.Rock => RPS.Paper,
                RPS.Paper => RPS.Scissors,
                _ => RPS.Rock
            }
        };

        WriteLine($"{opp} {you} {act}");
        return ((int)act - 1) * 3 + (int)(you);
    }

    private RPS Parse(string c) => c[0] switch {
        'A' or 'X' => RPS.Rock,
        'B' or 'Y' => RPS.Paper,
        _ => RPS.Scissors
    };
    
    private enum RPS
    {
        Rock = 1, Paper = 2, Scissors = 3
    }
    
    private enum Act
    {
        Lose = 1, Draw = 2, Win = 3
    }
    
}