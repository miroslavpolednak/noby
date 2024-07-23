using System.Runtime.CompilerServices;
using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbWebApi.V1.Contracts;

namespace ExternalServices.SbWebApi.V1;

internal static class RequestHelper
{
    private static async Task<TResponse> getResponseObject<TResponse>(HttpResponseMessage response, string callerName, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new CisExternalServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }

        var responseObject = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

        if (responseObject is null)
            throw new CisExternalServiceResponseDeserializationException(0, StartupExtensions.ServiceName, callerName, typeof(TResponse).Name);

        return responseObject;
    }

    private static void validateResponse(int returnVal, in string? callerName, in string? returnText, IList<(int ReturnVal, int ErrorCode)>? returnVal2ErrorCodesMapping)
    {
        if (returnVal != 0)
        {
            if (returnVal2ErrorCodesMapping?.Any(t => t.ReturnVal == returnVal) ?? false)
            {
                throw ErrorCodeMapper.CreateExternalServiceValidationException(returnVal2ErrorCodesMapping.First(t => t.ReturnVal == returnVal).ErrorCode);
            }
            else if (returnVal < 0)
            {
                throw new CisExternalServiceServerErrorException(returnVal, StartupExtensions.ServiceName, $"{StartupExtensions.ServiceName}.{callerName}: {returnVal}: {returnText}");
            }
            else
            {
                throw new CisExternalServiceValidationException(returnText ?? "Unknown error");
            }
        }
    }

    public static async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response,
                                                                   Func<TResponse, CommonResultEx?> commonResultGetter,
                                                                   IList<(int ReturnVal, int ErrorCode)>? returnVal2ErrorCodesMapping = null,
                                                                   CancellationToken cancellationToken = default,
                                                                   [CallerMemberName] string callerName = "")
    {
        var responseObject = await getResponseObject<TResponse>(response, callerName, cancellationToken);
        var commonResult = commonResultGetter(responseObject);
        validateResponse(commonResult?.Return_val ?? -999, callerName, commonResult?.Return_text, returnVal2ErrorCodesMapping);

        return responseObject;
    }

    public static async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response,
                                                                   Func<TResponse, CommonResult?> commonResultGetter,
                                                                   IList<(int ReturnVal, int ErrorCode)>? returnVal2ErrorCodesMapping = null,
                                                                   CancellationToken cancellationToken = default,
                                                                   [CallerMemberName] string callerName = "")
    {
        var responseObject = await getResponseObject<TResponse>(response, callerName, cancellationToken);
        var commonResult = commonResultGetter(responseObject);
        validateResponse(commonResult?.Return_val ?? -999, callerName, commonResult?.Return_text, returnVal2ErrorCodesMapping);

        return responseObject;
    }

    public static WFS_Header MapEasHeader(string headerLogin)
    {
        const string HeaderSystem = "NOBY";

        return new WFS_Header
        {
            System = HeaderSystem,
            Login = headerLogin
        };
    }

    public static IList<IReadOnlyDictionary<string, string>> MapTasksToDictionary(ICollection<WFS_FindItem>? tasks)
    {
        if (tasks is null)
            return new List<IReadOnlyDictionary<string, string>>();

        return tasks.Where(t => t.Task is not null)
                    .Select(t => t.Task!.Where(v => v.Mtdt_def is not null).ToDictionary(v => v.Mtdt_def!, v => v.Mtdt_val!))
                    .ToList<IReadOnlyDictionary<string, string>>();
    }
}