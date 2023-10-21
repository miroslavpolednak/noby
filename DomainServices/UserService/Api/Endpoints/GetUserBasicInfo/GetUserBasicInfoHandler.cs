using Google.Protobuf;
using Microsoft.EntityFrameworkCore;
using SharedTypes.GrpcTypes;

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
        var dbIdentities = (await _dbContext.UserBasicInfos
                                            .FromSqlInterpolated($"EXECUTE [dbo].[getUserIdentities] @identitySchema={UserIdentity.Types.UserIdentitySchemes.V33Id.ToString()}, @identityValue={request.UserId}")
                                            .ToListAsync(cancellationToken)
                           ).FirstOrDefault()
                           ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.UserId}");

        // vytvorit finalni model
        var model = new Contracts.GetUserBasicInfoResponse
        {
            DisplayName = $"{dbIdentities.firstname} {dbIdentities.surname}".Trim()
        };

        await _distributedCache.SetAsync(Helpers.CreateUserBasicCacheKey(request.UserId), model.ToByteArray(), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_minutesInCache),
        },
        cancellationToken);

        return model;
    }

    private const int _minutesInCache = 30;
    private readonly Database.UserServiceDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;

    public GetUserBasicInfoHandler(
        Database.UserServiceDbContext dbContext,
        IDistributedCache distributedCache)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
    }
}
