using AoC._2023;
using AoC.Library.Api;
using AoC.Library.Runner;

using var session = new AdventSession();
var fetcher = new AdventFetcher(session);
var runner = new AdventRunner(fetcher, typeof(Day12).Assembly);

await runner.FindAndRun(day: 10);