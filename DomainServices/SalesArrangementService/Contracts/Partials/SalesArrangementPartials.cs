namespace DomainServices.SalesArrangementService.Contracts;

public partial class CreateSalesArrangementRequest
    : MediatR.IRequest<CreateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetSalesArrangementRequest
    : MediatR.IRequest<SalesArrangement>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetSalesArrangementByOfferIdRequest
    : MediatR.IRequest<GetSalesArrangementByOfferIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateSalesArrangementStateRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class LinkModelationToSalesArrangementRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetSalesArrangementListRequest
    : MediatR.IRequest<GetSalesArrangementListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateSalesArrangementRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SendToCmpRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class ValidateSalesArrangementRequest
    : MediatR.IRequest<ValidateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateSalesArrangementParametersRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateLoanAssessmentParametersRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteSalesArrangementRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }

public partial class UpdateOfferDocumentIdRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }

public partial class GetFlowSwitchesRequest
    : MediatR.IRequest<GetFlowSwitchesResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SetFlowSwitchesRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }