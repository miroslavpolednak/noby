using Google.Protobuf.WellKnownTypes;

namespace DomainServices.OfferService.Contracts;

public partial class ValidateOfferIdRequest
    : MediatR.IRequest<ValidateOfferIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetOfferRequest
    : MediatR.IRequest<GetOfferResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetMortgageDetailRequest
    : MediatR.IRequest<GetMortgageDetailResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SimulateMortgageRequest
    : MediatR.IRequest<SimulateMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SimulateMortgageRetentionRequest
    : MediatR.IRequest<SimulateMortgageRetentionResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SimulateMortgageRefixationRequest
    : MediatR.IRequest<SimulateMortgageRefixationResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SimulateMortgageExtraPaymentRequest
    : MediatR.IRequest<SimulateMortgageExtraPaymentResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetMortgageOfferFPScheduleRequest
    : MediatR.IRequest<GetMortgageOfferFPScheduleResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetOfferDeveloperRequest
    : MediatR.IRequest<GetOfferDeveloperResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateOfferRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class GetInterestRateRequest
    : MediatR.IRequest<GetInterestRateResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class GetOfferListRequest
    : MediatR.IRequest<GetOfferListResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class CreateResponseCodeRequest
    : MediatR.IRequest<CreateResponseCodeResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class GetResponseCodeListRequest
    : MediatR.IRequest<GetResponseCodeListResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class DeleteOfferListRequest : MediatR.IRequest
{
}