using AoC.Library.Api;
using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 25, AdventParts.PartOne)]
[CustomExample(
    """
    jqt: rhn xhk nvd
    rsh: frs pzl lsr
    xhk: hfx
    cmg: qnr nvd lhk bvb
    rhn: xhk bvb hfx
    bvb: xhk hfx
    pzl: lsr hfx nvd
    qnr: nvd
    ntq: jqt hfx bvb xhk
    nvd: lhk
    lsr: lhk
    rzs: qnr cmg lsr rsh
    frs: qnr lhk lsr
    """,
    54
)]
public class Day25 : AdventSolution
{
    public override object SolvePartOne()
    {
        Dictionary<string, List<string>> cons = new();

        foreach (var line in Input.Lines)
        {
            var (inp, outs) = line.SmartSplit(':').Unpack2();
            var nnn = outs.SmartSplit(" ");

            if (cons.ContainsKey(inp))
            {
                cons[inp].AddRange(nnn);
            }
            else
            {
                cons[inp] = nnn.ToList();
            }

            foreach (var n in nnn)
            {
                if (cons.ContainsKey(n))
                {
                    cons[n].Add(inp);
                }
                else
                {
                    cons[n] = new List<string>() {
                        inp
                    };
                }
            }
        }

        // dot -Kneato -Tsvg temp.graph >temp.svg
        // and look
        var groups = CountGroups(cons, 
            //("hfx", "pzl"), ("bvb", "cmg"), ("nvd", "jqt"),
            ("zcp", "zjm"), ("rsg", "nsk"), ("jks", "rfg")
            );
        
        Console.WriteLine(string.Join(" ", groups));

        return groups[0] * groups[1];
    }

    private List<int> CountGroups(Dictionary<string, List<string>> cons, params (string, string)[] notToCount)
    {
        var res = new List<int>();
        var visits = new HashSet<string>();

        foreach (var s in cons.Keys)
        {
            var g = Bfs(s);

            if (g > 0) res.Add(g);
        }

        return res;

        int Bfs(string s)
        {
            var queue = new Queue<string>();
            queue.Enqueue(s);
            int cnt = 0;

            while (queue.TryDequeue(out var n))
            {
                if (!visits.Add(n)) continue;

                cnt++;

                foreach (var nn in cons[n])
                {
                    if (notToCount.Contains((n, nn)) || notToCount.Contains((nn, n))) continue;

                    queue.Enqueue(nn);
                }
            }

            return cnt;
        }
    }

    public override object SolvePartTwo() => throw new NotImplementedException();
}