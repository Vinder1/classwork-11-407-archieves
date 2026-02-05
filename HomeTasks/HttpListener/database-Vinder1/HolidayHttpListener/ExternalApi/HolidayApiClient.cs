namespace HolidayHttpListener.ExternalApi;

public class HolidayApiClient
{
    private const string ApiKey = "0c27ebed-afee-42e1-ac29-d9d7e01218f6";
    private readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://holidayapi.com")
    };

    private readonly HolidayApiCacheManager cacheManager = new();

    public async Task<string> RequestAsString()
    {
        if (cacheManager.TryGetCachedRequest(out string result))
        {
            return result;
        }

        var date = DateTime.Today;
        string url = "/v1/holidays?" +
            $"key={ApiKey}&country=ru&year=2024&language=ru&" +
            $"month={date.Month}&day={date.Day}&upcoming&public";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        result = await response.Content.ReadAsStringAsync();
        cacheManager.SetCachedRequest(result);
        return result;
    }
}
