using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Api.Database.Entities;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using Microsoft.EntityFrameworkCore;
using Result = CIS.InternalServices.NotificationService.Api.Database.Entities.Result;

namespace CIS.InternalServices.NotificationService.Api.Legacy;

internal class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public EmailResult NewEmailResult() => new()
    {
        Id = Guid.NewGuid(),
        Channel = NotificationChannel.Email,
        State = NotificationState.InProgress,
        ResultTimestamp = null,
        ErrorSet = new HashSet<ResultError>(),
    };

    public SmsResult NewSmsResult() => new()
    {
        Id = Guid.NewGuid(),
        Channel = NotificationChannel.Sms,
        State = NotificationState.InProgress,
        ResultTimestamp = null,
        ErrorSet = new HashSet<ResultError>()
    };

    public async Task AddResult(Result result, CancellationToken token = default)
    {
        await _dbContext.AddAsync(result, token);
    }

    public async Task<Result> GetResult(Guid id, CancellationToken token = default)
    {
        return await _dbContext.Results.FindAsync(new object?[] { id }, token)
               ?? throw new CisNotFoundException(ErrorCodeMapper.ResultNotFound, $"Result with id = '{id}' not found.");
    }

    public async Task<IEnumerable<Result>> SearchResultsBy(string? identity, string? identityScheme, long? caseId, string? customId, string? documentId)
    {
        return await _dbContext.Results
            .Where(r => string.IsNullOrEmpty(identity) || r.Identity == identity)
            .Where(r => string.IsNullOrEmpty(identityScheme) || r.IdentityScheme == identityScheme)
            .Where(r => !caseId.HasValue || r.CaseId == caseId.Value)
            .Where(r => string.IsNullOrEmpty(customId) || r.CustomId == customId)
            .Where(r => string.IsNullOrEmpty(documentId) || r.DocumentId == documentId)
            .ToListAsync();
    }

    public void DeleteResult(Result result)
    {
        _dbContext.Remove(result);
    }

    public async Task<int> SaveChanges(CancellationToken token = default)
    {
        return await _dbContext.SaveChangesAsync(token);
    }
}