using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 15, AdventParts.PartTwo)]
public class Day15 : AdventSolution
{
    public override object SolvePartOne() => Input.Raw.SmartSplit(",").Select(Hash).Sum();

    public override object SolvePartSecond()
    {
        var boxes = new List<(string name, string val)>?[300];
        var commands = Input.Raw.SmartSplit(",").ToArray();

        foreach (var cmd in commands)
        {
            if (cmd.Contains('-'))
            {
                var name = cmd[..^1];
                var h = Hash(name);

                (boxes[h] ??= new()).RemoveAll(n => n.name == name);
            }
            else
            {
                var (name, val) = cmd.Split('=').Unpack2();
                var h = Hash(name);
                var box = (boxes[h] ??= new());
                var i = box.FindIndex(n => n.name == name);

                if (i >= 0)
                {
                    box[i] = (name, val);
                }
                else
                {
                    box.Add((name, val));
                }
            }
        }

        var sum = 0;

        for (var boxInd = 0; boxInd < boxes.Length; boxInd++)
        {
            var box = boxes[boxInd];

            if (box is null) continue;

            for (var i = 0; i < box.Count; i++)
            {
                var res = (boxInd + 1) * box[i].val.ToInt() * (i + 1);
                WriteLine($"box {boxInd} fl {box[i].val} ord {i +1} res {res}");
                sum += res;
            }
        }

        return sum;
    }

    private int Hash(string s) => s.Aggregate(0, (current, c) => ((current + c) * 17) % 256);
}