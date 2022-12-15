using CIS.Core;
using CIS.Core.Attributes;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories;

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
            State = NotificationState.InProgress,
            Created = _dateTime.Now,
            Updated = _dateTime.Now,
            ErrorSet = new HashSet<string>()
        };

        await _dbContext.AddAsync(result, token);
        await _dbContext.SaveChangesAsync(token);
        return result;
    }

    public async Task<Result> GetResult(Guid notificationId, CancellationToken token = default)
    {
        // todo: Cis Exception code 300-399
        return await _dbContext.Results.FindAsync(new object?[] { notificationId }, token) ??
               throw new CisNotFoundException(300, $"Results #{notificationId} not found");
    }

    public async Task<IEnumerable<Result>> GetResultsBy(string clientId, string customerId, string documentId)
    {
        return await _dbContext.Results.Include(r => r.TrackingData)
            .Where(r => r.TrackingData != null)
            .Where(r => string.IsNullOrEmpty(clientId) || r.TrackingData!.ClientId == clientId)
            .Where(r => string.IsNullOrEmpty(customerId) || r.TrackingData!.CustomId == customerId)
            .Where(r => string.IsNullOrEmpty(documentId) || r.TrackingData!.DocumentId == documentId)
            .ToListAsync();
    }
    
    public async Task<Result> UpdateResult(
        Guid notificationId,
        NotificationState state,
        HashSet<string> newErrors,
        CancellationToken token = default)
    {
        var result = await GetResult(notificationId, token);

        if (result.State <= state)
        {
            result.State = state;
        }
        
        var errorSet = new HashSet<string>();
        errorSet.UnionWith(result.ErrorSet);
        errorSet.UnionWith(newErrors);
        result.ErrorSet = errorSet;
        
        result.Updated = _dateTime.Now;
        
        await _dbContext.SaveChangesAsync(token);
        return result;
    }
}