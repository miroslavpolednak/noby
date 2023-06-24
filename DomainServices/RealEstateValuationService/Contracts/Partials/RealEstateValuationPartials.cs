using Google.Protobuf.WellKnownTypes;

namespace DomainServices.RealEstateValuationService.Contracts;

public partial class CreateRealEstateValuationRequest
    : MediatR.IRequest<CreateRealEstateValuationResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetRealEstateValuationListRequest
    : MediatR.IRequest<GetRealEstateValuationListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class PatchDeveloperOnRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }