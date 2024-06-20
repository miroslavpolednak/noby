using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Clients;
using NOBY.Dto.Customer;
using NOBY.Services.Customer;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

internal sealed class GetCustomerDetailHandler
    : IRequestHandler<GetCustomerDetailRequest, GetCustomerDetailResponse>
{
    public async Task<GetCustomerDetailResponse> Handle(GetCustomerDetailRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = await _customerService.GetCustomerDetail(new Identity(request.Id, request.Scheme), cancellationToken);

        Dto.NaturalPersonModel person = new();
        result.NaturalPerson?.FillResponseDto(person);
        person.IsBrSubscribed = result.NaturalPerson?.IsBrSubscribed;
    
        return new GetCustomerDetailResponse
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            LegalCapacity = result.NaturalPerson?.LegalCapacity is null ? null : new LegalCapacityItem
            {
                RestrictionTypeId = result.NaturalPerson.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = result.NaturalPerson.LegalCapacity.RestrictionUntil
            },
            IdentificationDocument = result.IdentificationDocument?.ToResponseDto(),
            Contacts = result.Contacts?.ToResponseDto(),
            Addresses = result.Addresses?.Where(t => t.AddressTypeId != (int)AddressTypes.Other)
                              .Select(t => (SharedTypes.Types.Address)t!).ToList()
        };
    }

    private readonly ICustomerServiceClient _customerService;

    public GetCustomerDetailHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}