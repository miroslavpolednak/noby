using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.GetDetail;

internal sealed class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = await _customerService.GetCustomerDetail(new Identity(request.Id, request.Schema), cancellationToken);

        Dto.NaturalPersonModel person = new();
        result.NaturalPerson?.FillResponseDto(person);
        person.IsBrSubscribed = result.NaturalPerson?.IsBrSubscribed;
    
        return new GetDetailResponse
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            LegalCapacity = result.NaturalPerson?.LegalCapacity is null ? null : new Shared.LegalCapacityItem
            {
                RestrictionTypeId = result.NaturalPerson.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = result.NaturalPerson.LegalCapacity.RestrictionUntil
            },
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