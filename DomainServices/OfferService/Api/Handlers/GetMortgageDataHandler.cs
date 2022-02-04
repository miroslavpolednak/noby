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
        Repositories.OfferInstanceRepository repository,
        ILogger<GetMortgageDataHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetMortgageDataResponse> Handle(Dto.GetMortgageDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferInstanceId);

        var entity = await LoadOfferInstance(request.OfferInstanceId);

        // kontrola ProductInstanceTypeId (zda je typu Mortgage)
        await checkProductInstanceTypeCategory(
            entity.ProductInstanceTypeId,
            CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.Mortgage
        );

        var data = JsonSerializer.Deserialize<Dto.Models.MortgageDataModel>(entity.Outputs ?? String.Empty);

        var model = new GetMortgageDataResponse
        {
            OfferInstanceId = entity.OfferInstanceId,
            ProductInstanceTypeId = entity.ProductInstanceTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = ToCreated(entity),
            InputData = JsonSerializer.Deserialize<MortgageInput>(entity.Inputs ?? String.Empty),
            Mortgage = data.Mortgage,            
        };

        return model;
    }
  
}