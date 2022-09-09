using CIS.Infrastructure.gRPC.CisTypes;
using contracts = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStarted(nameof(GetDetailHandler));

        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = ServiceCallResult.ResolveAndThrowIfError<contracts.CustomerDetailResponse>(await _customerService.GetCustomerDetail(new Identity(request.Id, request.Schema), cancellationToken));

        // transform
        return result.ToResponseDto();
    }

    private readonly ILogger<GetDetailHandler> _logger;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetDetailHandler(
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        ILogger<GetDetailHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}