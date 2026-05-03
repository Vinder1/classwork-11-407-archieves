namespace AspNetApp;

public class HolidayApiClient
{
    private const string ApiKey = "0c27ebed-afee-42e1-ac29-d9d7e01218f6";
    private static readonly HttpClient httpClient = new();

    public static async Task<string> RequestAsString()
    {
        var date = DateTime.Now;
        string url =
            $"https://holidayapi.com/v1/holidays?key={ApiKey}" +
            $"&country=ru&year={date.Year-1}&language=ru&month={date.Month}&day={date.Day}&upcoming&public";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
