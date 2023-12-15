using System.Collections.Specialized;
using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 15, AdventParts.PartTwo)]
public class Day15 : AdventSolution
{
    public override object SolvePartOne() => Input.Raw.SmartSplit(",").Select(Hash).Sum();

    public override object SolvePartSecond()
    {
        var lenses = new OrderedDictionary();
        var commands = Input.Raw.SmartSplit(",").ToArray();

        foreach (var cmd in commands)
        {
            if (cmd.Contains('-'))
            {
                var name = cmd[..^1];
                lenses.Remove(name);
            }
            else
            {
                var (name, val) = cmd.Split('=').Unpack2();
                lenses[name] = val;
            }
        }

        var boxes = new int[300];
        var keys = new string[lenses.Count];
        var values = new string[lenses.Count];
        lenses.Keys.CopyTo(keys, 0);
        lenses.Values.CopyTo(values, 0);


        var sum = 0;

        for (var i = 0; i < lenses.Count; i++)
        {
            var boxId = Hash(keys[i]);
            var ord = ++boxes[boxId];
            var res = (boxId + 1) * values[i].ToInt() * ord;
            WriteLine($"box {boxId} fl {values[i]} ord {ord} res {res}");
            sum += res;
        }

        return sum;
    }

    private int Hash(string s) => s.Aggregate(0, (current, c) => ((current + c) * 17) % 256);
}