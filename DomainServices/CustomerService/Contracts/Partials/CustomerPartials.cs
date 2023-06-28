using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CustomerService.Contracts;

public partial class ProfileCheckRequest
    : MediatR.IRequest<ProfileCheckResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CreateCustomerRequest
    : MediatR.IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CustomerDetailRequest
    : MediatR.IRequest<CustomerDetailResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CustomerListRequest
    : MediatR.IRequest<CustomerListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SearchCustomersRequest
    : MediatR.IRequest<SearchCustomersResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateCustomerRequest
    : MediatR.IRequest<UpdateCustomerResponse>, CIS.Core.Validation.IValidatableRequest 
{ }

public partial class UpdateCustomerIdentifiersRequest
    :MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class ValidateContactRequest
    :MediatR.IRequest<ValidateContactResponse>, CIS.Core.Validation.IValidatableRequest
{ }