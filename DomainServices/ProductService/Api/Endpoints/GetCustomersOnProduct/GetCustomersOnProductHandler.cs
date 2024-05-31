using CIS.Core.ErrorCodes;
using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler(
    IMpHomeClient _mpHomeClient, 
    ICodebookServiceClient _codebookService)
    : IRequestHandler<GetCustomersOnProductRequest, GetCustomersOnProductResponse>
{
    public async Task<GetCustomersOnProductResponse> Handle(GetCustomersOnProductRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken)
			?? throw ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // Kontrola, zda se jedná o KB produkt, chyba pokud ne vyhodit chybu
        await CheckIfProductIsKb(loan.ProductUvCode, cancellationToken);

        if (!(loan.LoanRelationships?.Any() ?? false))
        {
            throw ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.InvalidArgument12020);
        }

        return new GetCustomersOnProductResponse
        {
            Customers =
            {
                loan.LoanRelationships.Select(c =>
                {
                    var customerResponse = new GetCustomersOnProductResponseItem
                    {
                        RelationshipCustomerProductTypeId = c.PartnerRelationshipId,
                        Agent = c.Agent ?? false,
                        IsKYCSuccessful = c.KycStatus.GetValueOrDefault() == 1
                    };

                    customerResponse.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(c.PartnerId, IdentitySchemes.Mp));

                    if (c.KbId.HasValue)
                        customerResponse.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(c.KbId!.Value, IdentitySchemes.Kb));

                    return customerResponse;
                })
            }
        };
	}

    private async Task CheckIfProductIsKb(int? productTypeId, CancellationToken cancellationToken)
    {
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        if (productTypes.Any(p => p.Id == productTypeId && p.MandantId == (int)Mandants.Kb))
            return;

        throw ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.InvalidArgument12019);
    }
}
