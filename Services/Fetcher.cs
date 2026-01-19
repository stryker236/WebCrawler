using System.Net.Http;

public class Fetcher
{
    private readonly HttpClient _httpClient;

    public Fetcher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> FetchPageAsync(string url)
    {
        Console.WriteLine($"Fetching URL: {url}");
        var resp = await _httpClient.GetAsync(url);
        if (!resp.IsSuccessStatusCode)
        {
            return string.Empty;
        }
        return await resp.Content.ReadAsStringAsync();
    }

}