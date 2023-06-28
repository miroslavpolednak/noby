using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed  class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;

    public async Task Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(t => t.Id == salesArrangement.SalesArrangementTypeId);
        
        if (salesArrangementType.SalesArrangementCategory != (int)SalesArrangementCategories.ProductRequest)
        {
            throw new NobyValidationException($"Invalid SalesArrangement id = {request.SalesArrangementId}, must be of type {SalesArrangementCategories.ProductRequest}");
        }
        
        var mortgageParameters = new SalesArrangementParametersMortgage
        {
            Agent = salesArrangement.Mortgage?.Agent,
            Comment = request.Comment.Text ?? string.Empty,
            IncomeCurrencyCode = salesArrangement.Mortgage?.IncomeCurrencyCode ?? string.Empty,
            ResidencyCurrencyCode = salesArrangement.Mortgage?.ResidencyCurrencyCode ?? string.Empty,
            ContractSignatureTypeId = salesArrangement.Mortgage?.ContractSignatureTypeId,
            ExpectedDateOfDrawing = salesArrangement.Mortgage?.ExpectedDateOfDrawing,
            AgentConsentWithElCom = salesArrangement.Mortgage?.AgentConsentWithElCom,
        };

        if (salesArrangement.Mortgage?.LoanRealEstates.Any() ?? false)
        {
            mortgageParameters.LoanRealEstates.AddRange(salesArrangement.Mortgage?.LoanRealEstates);
        }
        
        var updateParametersRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            Mortgage = mortgageParameters
        };
        
        await _salesArrangementService.UpdateSalesArrangementParameters(updateParametersRequest, cancellationToken);
    }
    
    public UpdateCommentHandler(
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService)
    {
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
    }
}