using Google.Protobuf.WellKnownTypes;

namespace DomainServices.ProductService.Contracts;

public partial class GetProductListRequest
    : MediatR.IRequest<GetProductListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetProductObligationListRequest
    : MediatR.IRequest<GetProductObligationListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetMortgageRequest
    : MediatR.IRequest<GetMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CreateMortgageRequest
    : MediatR.IRequest<CreateMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateMortgageRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CreateContractRelationshipRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteContractRelationshipRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetCustomersOnProductRequest
    : MediatR.IRequest<GetCustomersOnProductResponse>, CIS.Core.Validation.IValidatableRequest
{ }
