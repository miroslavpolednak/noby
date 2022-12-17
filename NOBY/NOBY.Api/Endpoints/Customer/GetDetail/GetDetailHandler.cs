using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;

namespace NOBY.Api.Endpoints.Customer.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = await _customerService.GetCustomerDetail(new Identity(request.Id, request.Schema), cancellationToken);

        // transform
        return result.ToResponseDto();
    }

    private readonly ICustomerServiceClient _customerService;

    public GetDetailHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}