namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class Helpers
{
    public static async Task<string?> SafeReadAsStringAsync(this HttpResponseMessage? response, CancellationToken cancellationToken = default(CancellationToken))
        => response?.Content == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
}
