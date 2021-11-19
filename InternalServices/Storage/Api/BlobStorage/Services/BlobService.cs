using CIS.Infrastructure.gRPC;
using CIS.InternalServices.Storage.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Services;

[Authorize]
internal class BlobService : Contracts.v1.Blob.BlobBase
{
    private readonly IMediator _mediator;
        
    public BlobService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<BlobSaveResponse> Save(BlobSaveRequest request, ServerCallContext context)
    {
        if (request.BlobData?.Data is null || request.BlobData.Data.Length == 0)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Blob data is empty", 207);

        var model = new Dto.SaveRequest(new(request.ApplicationKey), request.BlobData.Name, request.BlobData.ContentType, BlobKinds.Default, request.BlobData.Data.ToArray());

        string key = await _mediator.Send(model);

        return new BlobSaveResponse
        {
            BlobKey = key
        };
    }

    public override async Task<BlobGetResponse> Get(BlobGetRequest request, ServerCallContext context)
    {
        var model = new Dto.GetRequest(new(request.BlobKey), BlobKinds.Default);
        return await _mediator.Send(model);
    }

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> Delete(BlobDeleteRequest request, ServerCallContext context)
    {
        var model = new Dto.DeleteRequest(new(request.BlobKey));
        await _mediator.Send(model);
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
