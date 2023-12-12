using System.Drawing;
using System.Reflection;
using AoC.Library.Api;
using Console = Colorful.Console;

namespace AoC.Library.Runner;

public class AdventRunner
{
    private readonly Assembly _solutionsAssembly;
    private readonly AdventFetcher _fetcher;

    public AdventRunner(AdventFetcher fetcher, Assembly solutionsAssembly)
    {
        _solutionsAssembly = solutionsAssembly;
        _fetcher = fetcher;
    }

    public async Task FindAndRunToday(RunnerConfig? config = null) =>
        await FindAndRun(config ?? new RunnerConfig(), DateTime.Now.Year, DateTime.Now.Day);

    public async Task FindAndRun(RunnerConfig? config = null, int? year = null, int? day = null)
    {
        config ??= new RunnerConfig();
        var solutionTypes = GetSolutions()
            .Where(s => SolutionTypeMatch(s, year, day))
            .ToArray();

        foreach (var solutionType in solutionTypes)
        {
            foreach (var part in new[] {
                         AdventParts.PartOne, AdventParts.PartTwo
                     })
            {
                if (!config.Parts.HasFlag(part)) continue;

                var dateInfo = solutionType.GetCustomAttribute<DateInfoAttribute>();

                if (dateInfo is null) continue;

                if (!dateInfo.PartsReady.HasFlag(part)) continue;

                _fetcher.Year = dateInfo.Year;
                _fetcher.Day = dateInfo.Day;

                var runConfigs = await GetRunConfigs(part, config.Type, solutionType);

                foreach (var runConfig in runConfigs)
                {
                    var solution = Activator.CreateInstance(solutionType) as AdventSolution;

                    await Run(
                        runConfig,
                        config.PrintInput.HasFlag(runConfig.RunType),
                        config.PrintOutput.HasFlag(runConfig.RunType),
                        part,
                        solution!
                    );
                }

                Console.WriteLine();
            }

            Console.WriteLine("===================\n\n");
        }
    }

    private async Task Run(RunConfig config, bool printInput, bool printOutput, AdventParts part, AdventSolution solution)
    {
        Func<object> run = part == AdventParts.PartOne
            ? solution.SolvePartOne
            : solution.SolvePartSecond;

        Console.WriteLine($"Running part {(int)part} for {config.Description}...", Color.Gold);

        if (printInput)
        {
            Console.WriteLine(config.Input);
        }

        Color resultColor;
        var started = DateTime.Now;

        try
        {
            solution.Input = new AdventInput(config.Input, printOutput);
            var result = run().ToString()!;

            Console.Write("Answer is ");
            var checkResult = config.Check is not null
                ? await config.Check(result)
                : null;


            if (checkResult is null)
            {
                resultColor = Color.Yellow;
                WriteAnswer(result);
                Console.WriteLine("Result cannot be validated");
            }
            else
            {
                var (isCorrect, real) = checkResult.Value;

                resultColor = isCorrect ? Color.Green : Color.Red;

                WriteAnswer(result);

                var state = isCorrect ? "correct" : "wrong";

                if (!isCorrect && real is not null)
                {
                    state += $" (Correct answer is {real})";
                }

                Console.WriteLine($"Answer is {state}", resultColor);
            }
        }
        catch (Exception ex)
        {
            resultColor = Color.Red;
            Console.WriteLine("Error while executing solution:", resultColor);
            Console.WriteLine(ex.ToString());
        }

        var ended = DateTime.Now;
        Console.WriteLine($"Time elapsed: {ended - started:g}\n", resultColor);

        void WriteAnswer(string answer)
        {
            Console.WriteLine(answer, resultColor);
        }
    }

    private async Task<RunConfig[]> GetRunConfigs(AdventParts part, RunType type, Type solution)
    {
        var methodInfo = solution.GetMethod(part == AdventParts.PartOne
            ? nameof(AdventSolution.SolvePartOne)
            : nameof(AdventSolution.SolvePartSecond)
        );
        var configs = new List<RunConfig>();

        if (type.HasFlag(RunType.Example))
        {
            var customExample = methodInfo?.GetCustomAttribute<CustomExampleAttribute>()
                                ?? solution.GetCustomAttribute<CustomExampleAttribute>();

            if (customExample is not null)
            {
                configs.Add(new RunConfig(
                    customExample.Input,
                    "custom example input",
                    RunType.Example,
                    part == AdventParts.PartOne
                        ? customExample.AnswerOne
                        : customExample.AnswerTwo
                ));
            }
            else
            {
                configs.Add(new RunConfig(
                    await _fetcher.GetExampleInput(),
                    "example input",
                    RunType.Example,
                    s => _fetcher.IsTestCorrect(s, part == AdventParts.PartOne)
                ));
            }
        }

        if (type.HasFlag(RunType.Full))
        {
            configs.Add(new RunConfig(
                await _fetcher.GetFullInput(),
                "full input",
                RunType.Full,
                s => _fetcher.IsFullCorrect(s, part == AdventParts.PartOne)
            ));
        }

        if (type.HasFlag(RunType.Custom))
        {
            var customRuns = methodInfo
                !.GetCustomAttributes<CustomRunAttribute>()
                .ToArray();

            foreach (var customRun in customRuns)
            {
                customRun.ThrowIfInvalid();

                var input = customRun switch {
                    { Filename: not null } => await _fetcher.GetInput(new InputDescription(customRun.Filename)),
                    { InputString: not null } => customRun.InputString,
                    _ => string.Empty
                };

                configs.Add(new RunConfig(input, customRun.Description, RunType.Custom, customRun.Correct));
            }
        }

        return configs.ToArray();
    }

    private bool SolutionTypeMatch(Type solutionType, int? year, int? day)
    {
        var dateInfo = solutionType.GetCustomAttribute<DateInfoAttribute>();

        return (year is null || year.Value == dateInfo?.Year) && (day is null || day.Value == dateInfo?.Day);
    }

    private IEnumerable<Type> GetSolutions() =>
        _solutionsAssembly
            .GetTypes()
            .Where(typeof(AdventSolution).IsAssignableFrom);
}