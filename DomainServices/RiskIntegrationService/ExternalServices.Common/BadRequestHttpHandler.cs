using CIS.Core.Exceptions.ExternalServices;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public sealed class BadRequestHttpHandler
    : DelegatingHandler
{
    private readonly string _serviceName;

    public BadRequestHttpHandler(string serviceName)
    {
        _serviceName = serviceName;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response!.StatusCode == HttpStatusCode.BadRequest)
        {
            Dto.ErrorModel? result = null;
            try
            {
                result = await response.Content.ReadFromJsonAsync<Dto.ErrorModel>(cancellationToken: cancellationToken);
            }
            catch { }

            if (result is null) // nepodarilo se deserializovat na korektni response type
            {
                var message = await response.SafeReadAsStringAsync(cancellationToken);
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.GeneralServiceResponseValidationError, $"{_serviceName} unknown error 400: {message}");
            }
            else
            {
                switch (result.Code)
                {
                    case "RISK.LOANAPPLICATIONASSESSMENTERRORCODES.RBC_WAITING_FOR_COMMITMENT":
                        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.C4MRBCWaitingForCommitment, result.Message);
                    case "RISK.LOANAPPLICATIONASSESSMENTERRORCODES.CUSTOMER_NOT_FOUND":
                        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.C4MCustomerNotFound, result.Message);
                    case "RISK.LOANAPPLICATIONASSESSMENTERRORCODES.CORRECT_STATE_RULE":
                        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.C4MCorrectStateRule, result.Message);
                    default:
                        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.GeneralServiceResponseValidationError, result.FullMessage);
                };
            }
        }

        return response;
    }
}
