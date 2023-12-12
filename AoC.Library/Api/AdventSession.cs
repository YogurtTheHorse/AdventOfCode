namespace AoC.Library.Api;

public class AdventSession : IDisposable
{
    private readonly string _filename;
    private readonly HttpClient _httpClient;

    public AdventSession(string filename = "cookie.txt")
    {
        _filename = filename;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Cookie", GetCookie());
    }

    public async Task<string> HttpGet(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to make GET request. Status code: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    private string GetCookie() => File.ReadAllText(_filename);

    public void Dispose() => _httpClient.Dispose();
}