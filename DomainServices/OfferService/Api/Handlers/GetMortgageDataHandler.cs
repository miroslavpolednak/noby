using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageDataHandler
    : BaseHandler, IRequestHandler<Dto.GetMortgageDataMediatrRequest, GetMortgageDataResponse>
{
    #region Construction

    private readonly ILogger<GetMortgageDataHandler> _logger;

    public GetMortgageDataHandler(
        Repositories.OfferRepository repository,
        ILogger<GetMortgageDataHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetMortgageDataResponse> Handle(Dto.GetMortgageDataMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _repository.Get(request.OfferId, cancellation);

        // kontrola ProductTypeId (zda je typu Mortgage)
        await CheckProductTypeCategory(
            entity.ProductTypeId,
            CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage
        );

        var model = new GetMortgageDataResponse
        {
            OfferId = entity.OfferId,
            ProductTypeId = entity.ProductTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            Inputs = entity.Inputs?.ToMortgageInput(),
            Outputs = entity.Outputs?.ToMortgageOutput(),            
        };

        return model;
    }
  
}