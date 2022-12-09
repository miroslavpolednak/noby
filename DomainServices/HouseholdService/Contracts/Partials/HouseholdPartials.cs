namespace DomainServices.HouseholdService.Contracts;

public partial class CreateHouseholdRequest
    : MediatR.IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteHouseholdRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }

public partial class GetHouseholdRequest
    : MediatR.IRequest<Household>
{ }

public partial class GetHouseholdListRequest
    : MediatR.IRequest<GetHouseholdListResponse>
{ }

public partial class LinkCustomerOnSAToHouseholdRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateHouseholdRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }