using CIS.Infrastructure.gRPC;
using DomainServices.ProductService.Contracts;
using Grpc.Core;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetHousingSavingsInstanceHandler
    : IRequestHandler<Dto.GetHousingSavingsInstanceMediatrRequest, GetHousingSavingsInstanceResponse>
{
    public async Task<GetHousingSavingsInstanceResponse> Handle(Dto.GetHousingSavingsInstanceMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _repository.GetSavingsFullDetail(request.ProductInstanceId) ?? throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Product instance ID #{request.ProductInstanceId} does not exist", 12000);

        return new GetHousingSavingsInstanceResponse
        {
            Overview = (BasicDetail)model,
            Detail = (FullDetail)model
        };
    }

    private readonly Repositories.KonsDbRepository _repository;

    public GetHousingSavingsInstanceHandler(Repositories.KonsDbRepository repository)
    {
        _repository = repository;
    }
}
