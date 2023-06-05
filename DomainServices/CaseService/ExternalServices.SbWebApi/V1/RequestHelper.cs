using System.Runtime.CompilerServices;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1.Contracts;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

internal static class RequestHelper
{
    public static async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response,
                                                                   Func<TResponse, CommonResult?> commonResultGetter,
                                                                   IList<(int ReturnVal, int ErrorCode)>? returnVal2ErrorCodesMapping = null,
                                                                   CancellationToken cancellationToken = default(CancellationToken),
                                                                   [CallerMemberName] string callerName = "")
    {
        if (!response.IsSuccessStatusCode)
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");

        var responseObject = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

        if (responseObject is null)
            throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, callerName, typeof(TResponse).Name);

        var commonResult = commonResultGetter(responseObject);

        int returnVal = commonResult?.Return_val ?? 0;
        if (returnVal != 0)
        {
            if (returnVal2ErrorCodesMapping?.Any(t => t.ReturnVal == returnVal) ?? false)
            {
                throw ErrorCodeMapper.CreateExtServiceValidationException(returnVal2ErrorCodesMapping.First(t => t.ReturnVal == returnVal).ErrorCode);
            }
            else
            {
                throw new CisExtServiceValidationException(returnVal, $"{StartupExtensions.ServiceName}.{callerName}: {returnVal}: {commonResult?.Return_text}");
            }
        }
        
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