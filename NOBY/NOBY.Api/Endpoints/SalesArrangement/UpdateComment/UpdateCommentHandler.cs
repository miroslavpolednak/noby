using CIS.Foms.Types.Enums;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed  class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public async Task Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if ((SalesArrangementTypes)salesArrangement.SalesArrangementTypeId != SalesArrangementTypes.Mortgage)
        {
            throw new NobyValidationException($"Invalid SalesArrangement id = {request.SalesArrangementId}, SalesArrangementTypeId must be {SalesArrangementTypes.Mortgage}");
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
    
    public UpdateCommentHandler(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}