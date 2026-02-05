namespace HolidayApp.ExternalApi;

/// <summary>
/// The request result is the same throughout the day. Therefore, only one is required each day;
/// Thats what this class basically does - it stores current day`s request result;
/// </summary>
public class HolidayApiCacheManager
{
    public bool HasCachedRequest
        => _cachedRequestResult != null && DateTime.Today == _cachedRequestDate;

    public bool TryGetCachedRequest(out string result)
    {
        result = HasCachedRequest ? _cachedRequestResult ?? "" : "";
        return HasCachedRequest;
    }

    /// May be insecure if method is called in 23:59:59:999, because 'DateTime.Today' will be one day newer than should,
    /// but I don`t really care
    public void SetCachedRequest(string value)
    {
        _cachedRequestResult = value;
        _cachedRequestDate = DateTime.Today;
    }

    private string? _cachedRequestResult;
    private DateTime _cachedRequestDate = DateTime.Today.AddDays(-1);
}
