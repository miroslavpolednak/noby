using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

public partial class PrintBuildingSavingsOfferMediatrRequest
    : IRequest<PrintBuildingSavingsOfferResponse>
{
    public PrintBuildingSavingsOfferRequest Request { get; init; }
    public PrintBuildingSavingsOfferMediatrRequest(PrintBuildingSavingsOfferRequest request)
    {
        Request = request;
    }
}
