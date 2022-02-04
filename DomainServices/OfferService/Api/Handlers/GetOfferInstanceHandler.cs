using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetOfferInstanceHandler
    : BaseHandler, IRequestHandler<Dto.GetOfferInstanceMediatrRequest, GetOfferInstanceResponse>
{
    #region Construction

    private readonly ILogger<GetOfferInstanceHandler> _logger;

    public GetOfferInstanceHandler(
        Repositories.OfferInstanceRepository repository,
        ILogger<GetOfferInstanceHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetOfferInstanceResponse> Handle(Dto.GetOfferInstanceMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferInstanceId);

        var entity = await LoadOfferInstance(request.OfferInstanceId);

        var model = new GetOfferInstanceResponse
        {
            OfferInstanceId = entity.OfferInstanceId,
            ProductInstanceTypeId = entity.ProductInstanceTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = ToCreated(entity),          
        };

        return model;
    }
  
}