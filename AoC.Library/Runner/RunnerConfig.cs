namespace AoC.Library.Runner;

public record RunnerConfig(AdventParts Parts = AdventParts.All, RunType Type = RunType.All, RunType PrintExample = RunType.Example);