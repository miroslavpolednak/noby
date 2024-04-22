using CIS.InternalServices.NotificationService.Contracts.v2;
using SharedComponents.DocumentDataStorage;
using CIS.InternalServices.NotificationService.Api.Database;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SearchResults;

internal sealed class SearchResultsHandler(
    NotificationDbContext _dbContext, 
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<SearchResultsRequest, SearchResultsResponse>
{
    public async Task<SearchResultsResponse> Handle(SearchResultsRequest request, CancellationToken cancellationToken)
    {
        // vytahnout z databaze
        var notifications = await createQuery(request)
            .AsNoTracking()
            .Take(100)
            .ToListAsync(cancellationToken);

        // mapovat na proto contract
        var mappedNotifications = notifications.Select(t => t.MapToResultDataV2()).ToArray();

        // dotahnout detail notifikace, resime jen sms
        string[] entityIds = mappedNotifications.Where(t => t.Channel == NotificationChannels.Sms).Select(t => t.NotificationId).ToArray();
        var details = await _documentDataStorage.GetList<Database.DocumentDataEntities.SmsData, string>(entityIds, cancellationToken);
        foreach (var detail in details)
        {
            mappedNotifications.First(t => t.NotificationId == detail.EntityId).SmsData = detail.MapToSmsResult();
        }

        var response = new SearchResultsResponse();
        response.Results.AddRange(mappedNotifications);
        return response;
    }

    private IQueryable<Database.Entities.Notification> createQuery(SearchResultsRequest request)
    {
        var query = _dbContext.Notifications.AsQueryable();

        if (!string.IsNullOrEmpty(request.Identifier?.Identity))
        {
            query = query.Where(t => t.Identity == request.Identifier.Identity && t.IdentityScheme == request.Identifier.IdentityScheme.ToString());
        }

        if (request.CaseId.HasValue)
        {
            query = query.Where(t => t.CaseId == request.CaseId);
        }

        if (!string.IsNullOrEmpty(request.CustomId))
        {
            query = query.Where(t => t.CustomId == request.CustomId);
        }

        if (!string.IsNullOrEmpty(request.DocumentId))
        {
            query = query.Where(t => t.DocumentId == request.DocumentId);
        }

        return query;
    }
}
