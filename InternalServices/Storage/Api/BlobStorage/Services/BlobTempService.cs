using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.Storage.Contracts;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Services;

[Authorize]
public class BlobTempService : Contracts.v1.BlobTemp.BlobTempBase
{
    private readonly IMediator _mediator;

    public BlobTempService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<BlobSaveResponse> Save(BlobTempSaveRequest request, ServerCallContext context)
    {
        if (request.BlobData is null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Blob data is empty", 207);
        else if (request.BlobData.Data is null || request.BlobData.Data.Length == 0)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Blob data is empty", 207);

        var sessionId = new SessionId(request.SessionId);
        var model = new Dto.SaveRequest(new(request.ApplicationKey), request.BlobData.Name, request.BlobData.ContentType, BlobKinds.Temp, request.BlobData.Data.ToArray(), sessionId);

        string key = await _mediator.Send(model);

        return new BlobSaveResponse
        {
            BlobKey = key
        };
    }

    public override async Task<BlobGetResponse> Get(BlobGetRequest request, ServerCallContext context)
    {
        var model = new Dto.GetRequest(new(request.BlobKey), BlobKinds.Temp);
        return await _mediator.Send(model);
    }

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> MoveSession(BlobMoveSessionFromTempRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.SessionId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Session Id is empty"));

        var model = new Dto.MoveSessionFromTemp(new(request.SessionId));
        await _mediator.Send(model);
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> Move(BlobMoveFromTempRequest request, ServerCallContext context)
    {
        if (request.BlobKey is null || !request.BlobKey.Any())
            throw new RpcException(new(StatusCode.InvalidArgument, "BlobKey collection is empty"));

        var model = new Dto.MoveBlobsFromTemp(request.BlobKey.Select(t => new BlobKey(t)).ToList());
        await _mediator.Send(model);
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
