using Dapper;
using CIS.Core;
using CIS.Infrastructure.Data;
using CIS.Core.Data;

namespace CIS.InternalServices.Storage.Api.BlobStorage;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class BlobRepository : DapperBaseRepository<BlobRepository>
{
    private readonly IDateTime _time;

    public BlobRepository(ILogger<BlobRepository> logger, IConnectionProvider factory, IDateTime time)
        : base(logger, factory) 
    {
        this._time = time;
    }

    public async Task Add(BlobKey blobKey, Dto.SaveRequest request)
    {
        await WithConnection(async c =>
        {
            await c.ExecuteAsync(
                "INSERT INTO Blob (BlobKey, ApplicationKey, SessionId, BlobName, BlobLength, BlobContentType, Kind, InsertTime) VALUES (@blobKey, @appKey, @sessionId, @name, @length, @mime, @kind, @time)", 
                new { blobKey = blobKey.Value, appKey = request.ApplicationKey.ToString(), sessionId = request.SessionId is null ? "" : request.SessionId.ToString(), name = request.Name, length = request.Data.LongLength, mime = request.ContentType, kind = (byte)request.Kind, time = _time.Now });
        });
    }

    public async Task<Dto.Blob> Get(BlobKey blobKey)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.Blob>("SELECT * FROM Blob WHERE BlobKey=@key", new { key = blobKey.Value }));
    }

    public async Task Delete(BlobKey blobKey)
    {
        await WithConnection(async c => await c.ExecuteAsync("DELETE FROM Blob WHERE BlobKey=@key", new { key = blobKey.Value }));
    }

    public async Task<List<Dto.Blob>> GetList(Core.Types.ApplicationKey applicationKey, BlobKinds kind = BlobKinds.Any)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.Blob>("SELECT * FROM Blob WHERE (@kind=0 OR Kind=@kind) AND ApplicationKey=@key", new { kind = kind, key = applicationKey.ToString() })).AsList());
    }

    public async Task<List<Dto.Blob>> GetList(List<BlobKey> blobKey, BlobKinds kind = BlobKinds.Any)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.Blob>("SELECT * FROM Blob WHERE (@kind=0 OR Kind=@kind) AND BlobKey IN @key", new { kind = kind, key = blobKey.Select(t => t.ToString()).ToArray() })).AsList());
    }

    public async Task<List<Dto.Blob>> GetSession(SessionId sessionId)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.Blob>("SELECT * FROM Blob WHERE Kind=@kind AND SessionId=@key", new { kind = BlobKinds.Temp, key = sessionId.ToString() })).AsList());
    }

    public async Task ChangeKind(IEnumerable<string> blobKey, BlobKinds oldKind, BlobKinds newKind)
    {
        await WithConnection(async c => await c.ExecuteAsync("UPDATE Blob SET SessionId=NULL, Kind=@kind WHERE Kind=@oldKind AND BlobKey IN @keys", new { kind = newKind, oldKind = oldKind, keys = blobKey.ToArray() }));
    }
}
