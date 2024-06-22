using AoC.Library.Api;
using AoC.Library.Runner;

using var session = new AdventSession();
var fetcher = new AdventFetcher(session);
var runner = new AdventRunner(
    fetcher,
    typeof(AoC._2022.FSharp.Day1).Assembly //,
    //typeof(AoC._2023.Day12).Assembly
);

// await runner.FindAndRunLatest(new RunnerConfig() {
//     // Type = RunType.Custom,
//     // PrintOutput = RunType.Example
// });
await runner.FindAndRunLatest(
    new RunnerConfig()
);