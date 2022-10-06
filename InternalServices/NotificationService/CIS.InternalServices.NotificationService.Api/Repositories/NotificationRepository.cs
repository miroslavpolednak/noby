using CIS.Core;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Repositories.Entities;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Repositories;

[ScopedService, SelfService]
public class NotificationRepository
{
    private readonly NotificationDbContext _dbContext;
    private readonly IDateTime _dateTime;

    public NotificationRepository(NotificationDbContext dbContext, IDateTime dateTime)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
    }

    public async Task<Result> CreateResult(NotificationChannel channel, CancellationToken token = default)
    {
        var result = new Result
        {
            Channel = channel,
            State = NotificationState.Unsent,
            Created = _dateTime.Now,
            Updated = _dateTime.Now,
            ErrorList = new HashSet<string>()
        };

        await _dbContext.AddAsync(result, token);
        await _dbContext.SaveChangesAsync(token);
        return result;
    }

    public async Task<Result> GetResult(Guid notificationId, CancellationToken token = default)
    {
        return await _dbContext.Result.FindAsync(new object?[]{ notificationId }, token) ??
            throw new CisNotFoundException(1, $"Result #{notificationId} not found");
    }
    
    public async Task<Result> UpdateResult(
        Guid notificationId,
        NotificationState state,
        HashSet<string> newErrors,
        CancellationToken token = default)
    {
        var result = await GetResult(notificationId, token);
        var errorList = new HashSet<string>();
        errorList.UnionWith(result.ErrorList);
        errorList.UnionWith(newErrors);
        
        result.State = state;
        result.ErrorList = errorList;
        result.Updated = _dateTime.Now;

        _dbContext.Update(result);
        await _dbContext.SaveChangesAsync(token);
        return result;
    }
}