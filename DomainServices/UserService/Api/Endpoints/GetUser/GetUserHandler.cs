using DomainServices.UserService.Contracts;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Endpoints.GetUser;

internal class GetUserHandler
    : IRequestHandler<GetUserRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        // zkusit cache
        string cacheKey = Helpers.CreateUserCacheKey(request.UserId);
        if (_distributedCache is not null)
        {
            var cachedBytes = await _distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes != null)
            {
                return Contracts.User.Parser.ParseFrom(cachedBytes);
            }
        }

        // vytahnout info o uzivateli z DB
        var userInstance = await _dbContext.UserIdentities
            .FromSqlInterpolated($"[dbo].[getUserIdentities] @identitySchema=, @identityValue={request}")
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(0, "User", request.UserId);

        // vytvorit finalni model
        var model = new Contracts.User
        {
        };

        if (_distributedCache is not null)
        {
            await _distributedCache.SetAsync(cacheKey, model.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1),
            },
            cancellationToken);
        }

        return model;
    }

    private readonly Database.UserServiceDbContext _dbContext;
    private readonly IDistributedCache? _distributedCache;

    public GetUserHandler(
        Database.UserServiceDbContext dbContext,
        IDistributedCache? distributedCache)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
    }
}
