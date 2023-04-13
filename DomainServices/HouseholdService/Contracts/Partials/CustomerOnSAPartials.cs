namespace DomainServices.HouseholdService.Contracts;

public partial class CreateCustomerRequest
    : MediatR.IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CreateIncomeRequest
    : MediatR.IRequest<CreateIncomeResponse>, CIS.Core.Validation.IValidatableRequest, IIncome
{ }

public partial class CreateObligationRequest
    : MediatR.IRequest<CreateObligationResponse>, CIS.Core.Validation.IValidatableRequest, IObligation
{ }

public partial class DeleteCustomerRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteIncomeRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }

public partial class DeleteObligationRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }

public partial class GetCustomerRequest
    : MediatR.IRequest<CustomerOnSA>
{ }

public partial class GetCustomersByIdentityRequest
    : MediatR.IRequest<GetCustomersByIdentityResponse>
{ }

public partial class GetCustomerListRequest
    : MediatR.IRequest<GetCustomerListResponse>
{ }

public partial class GetIncomeRequest
    : MediatR.IRequest<Income>
{ }

public partial class GetIncomeListRequest
    : MediatR.IRequest<GetIncomeListResponse>
{ }

public partial class GetObligationRequest
    : MediatR.IRequest<Obligation>
{ }

public partial class GetObligationListRequest
    : MediatR.IRequest<GetObligationListResponse>
{ }

public partial class UpdateCustomerRequest
    : MediatR.IRequest<UpdateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateIncomeRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest, IIncome
{ }

public partial class UpdateIncomeBaseDataRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class Obligation
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest, IObligation
{ }

public partial class UpdateCustomerDetailRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{ }