using DomainServices.ProductService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.ProductService.Api.Endpoints;

[Authorize]
internal class ProductService : Contracts.v1.ProductService.ProductServiceBase
{
    private readonly IMediator _mediator;

    public ProductService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<GetProductListResponse> GetProductList(GetProductListRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetProductObligationListResponse> GetProductObligationList(GetProductObligationListRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetMortgageResponse> GetMortgage(GetMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<CreateMortgageResponse> CreateMortgage(CreateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateMortgage(UpdateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> CreateContractRelationship(CreateContractRelationshipRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteContractRelationship(DeleteContractRelationshipRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetCustomersOnProductResponse> GetCustomersOnProduct(GetCustomersOnProductRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetCaseIdResponse> GetCaseId(GetCaseIdRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetCovenantListResponse> GetCovenantList(GetCovenantListRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetCovenantDetailResponse> GetCovenantDetail(GetCovenantDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request);
}