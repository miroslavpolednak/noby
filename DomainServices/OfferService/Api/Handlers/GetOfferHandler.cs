using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetOfferHandler
    : BaseHandler, IRequestHandler<Dto.GetOfferMediatrRequest, GetOfferResponse>
{
    #region Construction

    private readonly ILogger<GetOfferHandler> _logger;

    public GetOfferHandler(
        Repositories.OfferRepository repository,
        ILogger<GetOfferHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetOfferResponse> Handle(Dto.GetOfferMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferId);

        var entity = await _repository.Get(request.OfferId);

        var model = new GetOfferResponse
        {
            OfferId = entity.OfferId,
            ProductInstanceTypeId = entity.ProductInstanceTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            //Created = ToCreated(entity),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
        };

        return model;
    }
  
}