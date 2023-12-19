using AoC.Library.Api;
using AoC.Library.Runner;

using var session = new AdventSession();
var fetcher = new AdventFetcher(session);
var runner = new AdventRunner(fetcher, typeof(AoC._2023.Day12).Assembly);

await runner.FindAndRunLatest();
// await runner.FindAndRun(new RunnerConfig {
//     Parts = AdventParts.PartTwo,
//     Type = RunType.Full
// }, day: 12);