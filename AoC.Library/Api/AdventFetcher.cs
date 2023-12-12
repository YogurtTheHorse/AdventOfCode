using System.Text.RegularExpressions;
using AoC.Library.Utils;

namespace AoC.Library.Api;

public partial class AdventFetcher
{
    private readonly AdventSession _session;
    
    public int Year { get; set; }
    public int Day { get; set; }

    public AdventFetcher(AdventSession session, int? year = null, int? day = null)
    {
        _session = session;

        var today = DateTime.Now;

        Year = year ?? today.Year;
        Day = day ?? today.Day;
    }

    public async Task<string> GetExampleInput() => await GetInput(new InputDescription(
        "example.txt",
        $"https://adventofcode.com/{Year}/day/{Day}",
        page => ExampleTaskInput().Match(page).Groups[1].Value
    ));

    public async Task<string> GetFullInput() => await GetInput(new InputDescription(
        "input.txt",
        $"https://adventofcode.com/{Year}/day/{Day}/input",
        Helpers.Id
    ));

    public async Task<string> GetFakeInput(string filename) => await GetInput(new InputDescription(
        filename,
        null,
        Helpers.Id
    ));

    public async Task<string> GetInput(InputDescription inputDescription)
    {
        var directory = $"inputs/year{Year}/day{Day}";
        var filepath = $"{directory}/{inputDescription.FileName}";

        if (File.Exists(filepath))
        {
            return (await File.ReadAllTextAsync(filepath)).Replace("\r", string.Empty);
        }

        if (inputDescription.Url is null)
        {
            throw new FileNotFoundException("Test input not found", filepath);
        }

        Directory.CreateDirectory(directory);

        var result = inputDescription.PostProcess(await _session.HttpGet(inputDescription.Url));

        await File.WriteAllTextAsync(filepath, result);

        return result.Replace("\r", string.Empty);
    }

    public async Task<bool?> IsTestCorrect(string answer)
    {
        var task = await GetInput(new InputDescription(
            "task.txt",
            $"https://adventofcode.com/{Year}/day/{Day}",
            Helpers.Id
        ));

        return task.Contains($">{answer}<");
    }

    public async Task<bool?> IsFullCorrect(string answer)
    {
        var task = await GetInput(new InputDescription(
            "task.txt",
            $"https://adventofcode.com/{Year}/day/{Day}",
            Helpers.Id
        ));

        return task.Contains("Your puzzle answer was ") 
            ? task.Contains($">{answer}<") 
            : null;
    }

    [GeneratedRegex(@"<pre><code>(.*?)<\/code><\/pre>", RegexOptions.Singleline)]
    private static partial Regex ExampleTaskInput();
}