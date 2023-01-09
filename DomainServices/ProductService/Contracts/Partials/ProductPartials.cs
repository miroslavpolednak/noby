using Google.Protobuf.WellKnownTypes;

namespace DomainServices.ProductService.Contracts;

public partial class CreateProductInstanceRequest
    : MediatR.IRequest<CreateProductInstanceResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetHousingSavingsInstanceRequest
    : MediatR.IRequest<GetHousingSavingsInstanceResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetHousingSavingsInstanceBasicDetailRequest
    : MediatR.IRequest<GetHousingSavingsInstanceBasicDetailResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetProductInstanceListRequest
    : MediatR.IRequest<GetProductInstanceListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateHousingSavingsInstanceRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

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
    : MediatR.IRequest<ProductIdReqRes>, CIS.Core.Validation.IValidatableRequest
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
