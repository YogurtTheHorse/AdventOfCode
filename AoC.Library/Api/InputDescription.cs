using AoC.Library.Utils;

namespace AoC.Library.Api;

public record InputDescription(string FileName, string? Url, Func<string, string> PostProcess)
{
    public InputDescription(string fileName, string? url = null) : this(fileName, url, Helpers.Id) { }
}