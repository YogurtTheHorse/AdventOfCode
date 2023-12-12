namespace AoC.Library.Runner;

public record RunnerConfig(
    AdventParts Parts = AdventParts.All,
    RunType Type = RunType.All,
    RunType PrintInput = RunType.Example,
    RunType PrintOutput = RunType.Example
);