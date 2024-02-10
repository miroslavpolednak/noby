namespace DomainServices.OfferService.Contracts;

public partial class ValidateOfferIdRequest
    : MediatR.IRequest<ValidateOfferIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetOfferRequest
    : MediatR.IRequest<GetOfferResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetOfferDetailRequest
    : MediatR.IRequest<GetOfferDetailResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SimulateMortgageRequest
    : MediatR.IRequest<SimulateMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetMortgageOfferFPScheduleRequest
    : MediatR.IRequest<GetMortgageOfferFPScheduleResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetOfferDeveloperRequest
    : MediatR.IRequest<GetOfferDeveloperResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateOfferDocumentIdRequest
    : MediatR.IRequest, CIS.Core.Validation.IValidatableRequest
{
}