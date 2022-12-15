using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using NOBY.Api.SharedDto;
using contracts = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = ServiceCallResult.ResolveAndThrowIfError<contracts.CustomerDetailResponse>(await _customerService.GetCustomerDetail(new Identity(request.Id, request.Schema), cancellationToken));

        Dto.NaturalPersonModel person = new();
        result.NaturalPerson?.FillResponseDto(person);
        person.IsBrSubscribed = result.NaturalPerson?.IsBrSubscribed;
    
        return new GetDetailResponse
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            IdentificationDocument = result.IdentificationDocument?.ToResponseDto(),
            Contacts = result.Contacts?.ToResponseDto(),
            Addresses = result.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList()
        };
    }

    private readonly ICustomerServiceClient _customerService;

    public GetDetailHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}