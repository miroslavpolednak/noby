namespace CIS.Core;

public static class HttpHeadersExtensions
{
    public static void AddIfNotExists(this System.Net.Http.Headers.HttpRequestHeaders headers, in string headerKey, in string? headerValue) 
    {
        if (!headers.Contains(headerKey))
        {
            headers.Add(headerKey, headerValue);
        }
    }

    public static void Replace(this System.Net.Http.Headers.HttpRequestHeaders headers, in string headerKey, in string? headerValue)
    {
        if (string.IsNullOrEmpty(headerValue))
        {
            return;
        }

        if (headers.Contains(headerKey))
        {
            headers.Remove(headerKey);
        }

        headers.Add(headerKey, headerValue);
    }
}
