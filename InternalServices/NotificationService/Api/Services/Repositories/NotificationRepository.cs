using CIS.Core;
using CIS.Core.Attributes;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<Entities.SmsResult> CreateSmsResult(CancellationToken token = default)
    {
        var result = new Entities.SmsResult
        {
            Channel = NotificationChannel.Sms, 
            State = NotificationState.InProgress,
            CustomId = "",
            Identity = "",
            IdentityScheme = "",
            DocumentId  = "",
            RequestTimestamp = _dateTime.Now,
            HandoverToMcsTimestamp = null,
            ErrorSet = new HashSet<string>(),
            Text = "",
            CountryCode = "",
            PhoneNumber = ""
        };

        await _dbContext.AddAsync(result, token);
        await _dbContext.SaveChangesAsync(token);
        return result;
    }

    public async Task<Entities.EmailResult> CreateEmailResult(CancellationToken token = default)
    {
        var result = new Entities.EmailResult
        {
            Channel = NotificationChannel.Email, 
            State = NotificationState.InProgress,
            CustomId = "",
            Identity = "",
            IdentityScheme = "",
            DocumentId  = "",
            RequestTimestamp = _dateTime.Now,
            HandoverToMcsTimestamp = null,
            ErrorSet = new HashSet<string>(),
        };
        
        await _dbContext.AddAsync(result, token);
        await _dbContext.SaveChangesAsync(token);
        return result; 
    }
    
    public async Task<Entities.Abstraction.Result> GetResult(Guid notificationId, CancellationToken token = default)
    {
        // todo: Cis Exception code 300-399
        return await _dbContext.Results.FindAsync(new object?[] { notificationId }, token) ??
               throw new CisNotFoundException(300, $"Results #{notificationId} not found");
    }

    public async Task<IEnumerable<Entities.Abstraction.Result>> SearchResultsBy(string identity, string identityScheme, string customId, string documentId)
    {
        return await _dbContext.Results
            .Where(r => string.IsNullOrEmpty(identity) || r.Identity == identity)
            .Where(r => string.IsNullOrEmpty(identityScheme) || r.IdentityScheme == identityScheme)
            .Where(r => string.IsNullOrEmpty(customId) || r.CustomId == customId)
            .Where(r => string.IsNullOrEmpty(documentId) || r.DocumentId == documentId)
            .ToListAsync();
    }
    
    public async Task<Entities.Abstraction.Result> UpdateResult(
        Guid notificationId,
        NotificationState? state = null,
        DateTime? handoverToMcsTimestamp = null,
        HashSet<string>? newErrors = null,
        CancellationToken token = default)
    {
        var result = await GetResult(notificationId, token);

        if (state.HasValue)
        {
            result.State = state.Value;
        }

        if (handoverToMcsTimestamp.HasValue)
        {
            result.HandoverToMcsTimestamp = handoverToMcsTimestamp.Value;
        }

        if (!newErrors.IsNullOrEmpty())
        {
            var errorSet = new HashSet<string>();
            errorSet.UnionWith(result.ErrorSet);
            errorSet.UnionWith(newErrors!);
            result.ErrorSet = errorSet;
        }
        
        await _dbContext.SaveChangesAsync(token);
        return result;
    }
}