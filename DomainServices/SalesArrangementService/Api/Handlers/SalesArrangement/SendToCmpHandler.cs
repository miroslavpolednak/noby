using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<SendToCmpHandler> _logger;

    public SendToCmpHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<SendToCmpHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(SendToCmpHandler), request.SalesArrangementId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

}