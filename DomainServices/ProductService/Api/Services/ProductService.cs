using DomainServices.ProductService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.ProductService.Api.Services;

[Authorize]
internal class ProductService : Contracts.v1.ProductService.ProductServiceBase
{
    private readonly IMediator _mediator;

    public ProductService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<GetProductListResponse> GetProductList(CaseIdRequest request, ServerCallContext context)
      => await _mediator.Send(new Dto.GetProductListMediatrRequest(request));

    public override async Task<GetMortgageResponse> GetMortgage(ProductIdReqRes request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetMortgageMediatrRequest(request));

    public override async Task<ProductIdReqRes> CreateMortgage(CreateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateMortgageMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateMorgage(UpdateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateMortgageMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> CreateContractRelationship(CreateContractRelationshipRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateContractRelationshipMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteContractRelationship(DeleteContractRelationshipRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteContractRelationshipMediatrRequest(request));

}