using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 20, AdventParts.All)]
public class Day20 : AdventSolution
{
    public override object SolvePartOne()
    {
        var (f, s, _) = GeneralSolve(true);

        return f * s;
    }

    public override object SolvePartTwo() => string.Join(" ", GeneralSolve(false).Cycles);

    private (int Low, int High, int[] Cycles) GeneralSolve(bool first)
    {
        Dictionary<string, ModuleType> types = new();

        Dictionary<string, Dictionary<string, Signal>> conjInputs = new();
        Dictionary<string, Signal> states = new();
        Dictionary<string, string[]> matrix = new();
        Dictionary<string, List<string>> reverseMatrix = new();

        foreach (var line in Input.Lines)
        {
            var (inp, outs) = line.SmartSplit(" -> ").Unpack2();
            var outputs = outs.SmartSplit(",");
            var name = inp.TrimStart('&', '%');

            matrix[name] = outputs;

            foreach (var output in outputs)
            {
                if (!reverseMatrix.ContainsKey(output))
                {
                    reverseMatrix[output] = new();
                }

                reverseMatrix[output].Add(name);
            }

            var type = types[name] = inp[0] switch {
                '&' => ModuleType.Conj,
                '%' => ModuleType.Flip,
                _ => ModuleType.Pass
            };

            types[name] = type;
            states[name] = Signal.Low;
            conjInputs[name] = new Dictionary<string, Signal>();
        }


        var cycles = types
            .ToDictionary(
                t => t.Key,
                t => t.Value == ModuleType.Conj ? 0 : 1
            );

        foreach (var (name, type) in types)
        {
            if (type != ModuleType.Conj) continue;

            foreach (var s in reverseMatrix[name])
            {
                conjInputs[name][s] = Signal.Low;
            }
        }

        var q = new Queue<(Signal Signal, string Dst, string Src)>();
        int high = 0, low = 0;

        for (var i = 1; first ? i <= 1000 : !cycles.Values.All(l => l > 0); i++)
        {
            q.Enqueue((Signal.Low, "broadcaster", "button"));

            while (q.Any())
            {
                var (s, dst, src) = q.Dequeue();

                if (s == Signal.Low) low++;
                else high++;


                if (!matrix.TryGetValue(dst, out var newDest)) continue;

                var type = types[dst];

                switch (type)
                {
                    case ModuleType.Pass:
                        foreach (var des in newDest)
                        {
                            q.Enqueue((s, des, dst));
                        }

                        break;

                    case ModuleType.Flip when s == Signal.Low:
                        states[dst] = Flip(states[dst]);

                        foreach (var des in newDest)
                        {
                            q.Enqueue((states[dst], des, dst));
                        }

                        break;

                    case ModuleType.Conj:
                        conjInputs[dst][src] = s;

                        var pulse = conjInputs[dst].All(kv => kv.Value == Signal.High)
                            ? Signal.Low
                            : Signal.High;

                        if (pulse == Signal.High && cycles[dst] == 0)
                        {
                            cycles[dst] = i;
                        }

                        foreach (var des in newDest)
                        {
                            q.Enqueue((pulse, des, dst));
                        }

                        break;
                }
            }
        }

        return (high, low, cycles.Values.ToArray());
    }

    Signal Flip(Signal s) => s switch {
        Signal.Low => Signal.High,
        _ => Signal.Low
    };

    public enum Signal
    {
        Low,
        High
    }

    public enum ModuleType
    {
        Pass,
        Flip,
        Conj
    }
}