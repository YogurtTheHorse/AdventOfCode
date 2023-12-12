namespace AoC.Library.Runner;

public record RunConfig(string Input, string Description, RunType RunType, Func<string, Task<(bool, string?)?>>? Check = null)
{
    public RunConfig(string input, string description, RunType runType, string? answer) : this(
        input,
        description,
        runType,
        // cries
        s => Task.FromResult(answer is null 
            ? null 
            : ((bool, string?)?)(answer == s, (string?)answer))
    )
    {
    }

    public RunConfig(string input, string description, RunType runType, Func<string, Task<bool?>> check) : this(
        input,
        description,
        runType,
        async s => (await check(s) is {} b) ? (b, null) : null
    )
    {
    }
}