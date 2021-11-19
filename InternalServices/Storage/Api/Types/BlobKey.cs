using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage;

internal class BlobKey
{
    public string Key { get; init; }

    public Guid Value { get; init; }

    public BlobKey(Guid key)
    {
        this.Value = key;
        this.Key = key.ToString();
    }

    public BlobKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Blob key is empty", 201);
        else if (!Guid.TryParse(key, out Guid value))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"'{key}' is not valid Blob Key", 202);
        else
        {
            this.Value = value;
            this.Key = key;
        }
    }

    public static implicit operator string(BlobKey d) => d.Key;

    public override string ToString() => $"{Key}";

    public static BlobKey CreateNew() => new BlobKey(Guid.NewGuid());
}
