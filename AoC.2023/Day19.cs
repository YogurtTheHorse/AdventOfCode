using AoC.Library.Runner;
using AoC.Library.Utils;
using PartsRange = System.Collections.Generic.Dictionary<string, (int MinVal, int MaxVal)>;

namespace AoC._2023;

[DateInfo(2023, 19, AdventParts.All)]
public class Day19 : AdventSolution
{
    public override object SolvePartOne()
    {
        var (wfs, ps) = Input.Raw.SmartSplit("\n\n").Unpack2();
        var parts = ps.SmartSplit("\n").Select(s => s[1..^1]
            .Split(',')
            .Select(s =>
            {
                var (k, v) = s.SmartSplit("=").Unpack2();

                return (k, v.ToInt());
            })
            .ToDictionary(v => v.k, v => v.Item2)).ToArray();
        var workflows = ParseWorkflows(wfs);

        return parts.Where(PartMatch).Select(p => p.Values.Sum()).Sum();

        bool PartMatch(Dictionary<string, int> part)
        {
            var wfn = "in";

            while (wfn is not ("R" or "A"))
            {
                var wf = workflows[wfn];

                foreach (var rule in wf.Rules)
                {
                    if (!rule.Cond(part)) continue;

                    wfn = rule.Res;

                    break;
                }
            }

            return wfn == "A";
        }
    }

    public override object SolvePartTwo()
    {
        var wfs = Input.Raw.SmartSplit("\n\n")[0];
        var workflows = ParseWorkflows(wfs);

        var startRange = new PartsRange() {
            {
                "x", (1, 4000)
            }, {
                "m", (1, 4000)
            }, {
                "a", (1, 4000)
            }, {
                "s", (1, 4000)
            },
        };
        var ranges = new List<(PartsRange, string)> {
            (startRange, "in")
        };
        var resultRanges = new List<PartsRange>();

        while (ranges.Count > 0)
        {
            var (ran, wn) = ranges[0];
            ranges.RemoveAt(0);

            if (wn == "A")
            {
                resultRanges.Add(ran);

                continue;
            }
            else if (wn == "R")
            {
                WriteLine($"Rejected: {R2S(ran)}");

                continue;
            }

            var wf = workflows[wn];

            foreach (var rule in wf.Rules)
            {
                var (tr, fa) = rule.Split(ran);

                if (tr is not null)
                {
                    ranges.Add((tr, rule.Res));
                }

                if (fa is not null)
                {
                    ran = fa;
                }
            }
        }

        foreach (var ran in resultRanges)
        {
            WriteLine(R2S(ran));
        }

        return resultRanges
            .Select(r => r.Values.Select(v => (long)v.MaxVal - v.MinVal + 1).Aggregate(1l, (v, acc) => acc * v))
            .Sum();
    }

    private static string R2S(PartsRange ran) => string.Join(" ", ran.Keys.Select(k => $"{k}: ({ran[k].MinVal}-{ran[k].MaxVal})"));

    private Dictionary<string, Workflow> ParseWorkflows(string wfs) =>
        wfs
            .SmartSplit("\n")
            .Select(l =>
            {
                var (name, other) = l.SmartSplit("{").Unpack2();

                return new Workflow(
                    name,
                    other[..^1]
                        .Split(',')
                        .Select(cs =>
                        {
                            if (!cs.Contains(':')) return new Rule(_ => true, d => (d, null), cs);

                            var (cond, res) = cs.SmartSplit(":").Unpack2();
                            var param = cond[0].ToString();
                            var op = cond[1];
                            var val = cond[2..].ToInt();
                            
                            Func<Dictionary<string, int>, bool> condf = op == '>'
                                ? d => d[param] > val
                                : d => d[param] < val;

                            // better to close you eyes
                            Func<PartsRange, (PartsRange?, PartsRange?)> split = op == '>'
                                ? r =>
                                {
                                    var tr = new PartsRange(r);
                                    tr[param] = (val + 1, tr[param].MaxVal);
                                    var fa = new PartsRange(r);
                                    fa[param] = (fa[param].MinVal, val);

                                    return (IsValid(tr[param]) ? tr : null, IsValid(fa[param]) ? fa : null);
                                }
                                : r =>
                                {
                                    var tr = new PartsRange(r);
                                    tr[param] = (tr[param].MinVal, val - 1);
                                    var fa = new PartsRange(r);
                                    fa[param] = (val, fa[param].MaxVal);

                                    return (IsValid(tr[param]) ? tr : null, IsValid(fa[param]) ? fa : null);
                                };

                            return new Rule(condf, split, res);
                        })
                        .ToArray()
                );
            }).ToDictionary(w => w.Name, w => w);

    private bool IsValid((int MinValue, int MaxValue) val) => val.MinValue < val.MaxValue;

    record Part(int X, int M, int A, int S);

    record Rule(Func<Dictionary<string, int>, bool> Cond, Func<PartsRange, (PartsRange?, PartsRange?)> Split, string Res);

    record Workflow(string Name, Rule[] Rules);
}