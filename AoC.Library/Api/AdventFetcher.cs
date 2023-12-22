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
            .Replace("&gt;", ">")
            .Replace("&lt;", "<")
            .Replace("&amp;", "&")
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

    public async Task<string> GetInput(InputDescription inputDescription, bool ignoreFiles = false)
    {
        var directory = $"inputs/year{Year}/day{Day}";
        var filepath = $"{directory}/{inputDescription.FileName}";

        if (!ignoreFiles)
        {
            var isNewFile = (DateTime.Now - File.GetCreationTime(filepath)).TotalMinutes < 15 || inputDescription.Url is null;

            if (File.Exists(filepath) && isNewFile)
            {
                return (await File.ReadAllTextAsync(filepath)).Replace("\r", string.Empty);
            }

            if (inputDescription.Url is null)
            {
                throw new FileNotFoundException("Test input not found", filepath);
            }
        }

        Directory.CreateDirectory(directory);

        var result = inputDescription.PostProcess(await _session.HttpGet(inputDescription.Url));

        await File.WriteAllTextAsync(filepath, result);

        return result.Replace("\r", string.Empty);
    }

    public async Task<bool?> IsTestCorrect(string answer, bool forFirstDay)
    {
        var task = await GetTwoDaysTask(forFirstDay);

        return task.Contains($">{answer}<");
    }

    public async Task<bool?> IsFullCorrect(string answer, bool forFirstTask)
    {
        var task = await GetTwoDaysTask(forFirstTask);

        return task.Contains("Your puzzle answer was ")
            ? task.Contains($">{answer}<")
            : null;
    }

    private async Task<string> GetTwoDaysTask(bool forFirstDay)
    {
        var task = await GetTask(false);

        return forFirstDay || task.Contains("--- Part Two ---")
            ? task
            : await GetTask(true);

        async Task<string> GetTask(bool ignoreFiles) => await GetInput(new InputDescription(
                "task.txt",
                $"https://adventofcode.com/{Year}/day/{Day}",
                Helpers.Id
            ),
            ignoreFiles);
    }

    [GeneratedRegex(@"<pre><code>(.*?)<\/code><\/pre>", RegexOptions.Singleline)]
    private static partial Regex ExampleTaskInput();
}