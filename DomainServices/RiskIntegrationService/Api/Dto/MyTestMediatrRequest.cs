using DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Dto;

internal record MyTestMediatrRequest(MyTestRequest Request)
    : IRequest<MyTestResponse>
{
}
