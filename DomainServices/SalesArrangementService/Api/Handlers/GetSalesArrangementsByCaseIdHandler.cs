using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementsByCaseIdHandler
    : IRequestHandler<Dto.GetSalesArrangementsByCaseIdMediatrRequest, GetSalesArrangementsByCaseIdResponse>
{
    public async Task<GetSalesArrangementsByCaseIdResponse> Handle(Dto.GetSalesArrangementsByCaseIdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get list for CaseId #{id}", request.CaseId);

        var listData = await _repository.GetSalesArrangementsByCaseId(request.CaseId);
        var finalData = listData.Select(t => new SalesArrangementListModel
        {
            SalesArrangementId = t.SalesArrangementId,
            SalesArrangementType = t.SalesArrangementTypeId,
            State = t.State,
            CaseId = t.CaseId,
            OfferInstanceId = t.OfferInstanceId
        }).ToList();

        var model = new GetSalesArrangementsByCaseIdResponse();
        model.SalesArrangements.AddRange(finalData);

        return model;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementsByCaseIdHandler> _logger;

    public GetSalesArrangementsByCaseIdHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementsByCaseIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
