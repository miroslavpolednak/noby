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
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferId);

        var entity = await _repository.Get(request.OfferId);

        // kontrola ProductInstanceTypeId (zda je typu Mortgage)
        await checkProductInstanceTypeCategory(
            entity.ProductInstanceTypeId,
            CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.Mortgage
        );

        var data = JsonSerializer.Deserialize<Dto.Models.MortgageDataModel>(entity.Outputs ?? String.Empty);

        var model = new GetMortgageDataResponse
        {
            OfferId = entity.OfferId,
            ProductInstanceTypeId = entity.ProductInstanceTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            //Created = ToCreated(entity),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            InputData = entity.Inputs.ToMortgageInput(),
            Mortgage = data.Mortgage,            
        };

        return model;
    }
  
}