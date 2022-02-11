using CIS.Infrastructure.gRPC;
using DomainServices.ProductService.Contracts;
using Grpc.Core;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetHousingSavingsInstanceBasicDetailHandler
    : IRequestHandler<Dto.GetHousingSavingsInstanceBasicDetailMediatrRequest, GetHousingSavingsInstanceBasicDetailResponse>
{
    public async Task<GetHousingSavingsInstanceBasicDetailResponse> Handle(Dto.GetHousingSavingsInstanceBasicDetailMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _repository.GetSavingsBasicDetail(request.ProductInstanceId) ?? throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Product instance ID #{request.ProductInstanceId} does not exist", 12000);

        return new GetHousingSavingsInstanceBasicDetailResponse
        {
            Overview = (BasicDetail)model
        };
    }

    private readonly Repositories.KonsDbRepository _repository;

    public GetHousingSavingsInstanceBasicDetailHandler(Repositories.KonsDbRepository repository)
    {
        _repository = repository;
    }
}
