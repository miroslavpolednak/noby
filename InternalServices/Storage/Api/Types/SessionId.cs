using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage;

internal record SessionId
{
    public string Key { get; init; }

    public SessionId(string id)
    {
        if (id is null)
            this.Key = "";
        else if (id.Length > 50)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"'{id}' is not valid Session Id", 203);
        else
            this.Key = id;
    }

    public static implicit operator string(SessionId d) => d.Key;

    public override string ToString() => $"{Key}";
}
