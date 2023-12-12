namespace AoC.Library.Runner;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CustomRunAttribute : Attribute
{
    public string? Correct { get; set; }

    public string? Filename { get; }

    public string? InputString { get; }

    public string Description { get; }

    public CustomRunAttribute(
        string? inputString = null,
        string? filename = null,
        string? correct = null,
        string description = "custom input"
    )
    {
        InputString = inputString;
        Filename = filename;
        Correct = correct;
        Description = description;
    }

    public Task<bool> Check(string answer) => Task.FromResult(answer == Correct);

    public void ThrowIfInvalid()
    {
        if (Filename is null != InputString is null) return;

        throw new InvalidOperationException("Custom Run can't have both filename and input string");
    }
}