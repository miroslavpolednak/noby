using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;

namespace FOMS.Services.CreateProduct;

internal static class Extensions
{
    public static MortgageData ToDomainServiceRequest(this GetMortgageDataResponse offerData)
        => new MortgageData
        {
            ContractNumber = ""
        };
}
