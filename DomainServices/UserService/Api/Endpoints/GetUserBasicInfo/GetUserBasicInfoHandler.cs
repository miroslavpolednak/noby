using Google.Protobuf;

namespace DomainServices.UserService.Api.Endpoints.GetUserBasicInfo;

internal sealed class GetUserBasicInfoHandler
    : IRequestHandler<Contracts.GetUserBasicInfoRequest, Contracts.GetUserBasicInfoResponse>
{
    public async Task<Contracts.GetUserBasicInfoResponse> Handle(Contracts.GetUserBasicInfoRequest request, CancellationToken cancellationToken)
    {
        // zkusit cache
        string cacheKey = Helpers.CreateUserBasicCacheKey(request.UserId);
        var cachedBytes = await _distributedCache.GetAsync(cacheKey, cancellationToken);
        if (cachedBytes != null)
        {
            return Contracts.GetUserBasicInfoResponse.Parser.ParseFrom(cachedBytes);
        }
    
        // vytahnout info o uzivateli z DB
        var userInfo = (await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<dynamic>(
            "[dbo].[getUserDisplayName]", 
            new { v33id = request.UserId }, 
            cancellationToken))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.UserId}");

        // vytvorit finalni model
        var model = new Contracts.GetUserBasicInfoResponse
        {
            DisplayName = userInfo.DisplayName
        };

        await _distributedCache.SetAsync(Helpers.CreateUserBasicCacheKey(request.UserId), model.ToByteArray(), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_minutesInCache),
        },
        cancellationToken);

        return model;
    }

    private const int _minutesInCache = 30;
    private readonly IConnectionProvider _db;
    private readonly IDistributedCache _distributedCache;

    public GetUserBasicInfoHandler(
        IConnectionProvider db,
        IDistributedCache distributedCache)
    {
        _db = db;
        _distributedCache = distributedCache;
    }
}
