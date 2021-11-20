using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Handlers;

internal class PrintBuildingSavingsOfferQueryHandler
    : IRequestHandler<Dto.PrintBuildingSavingsOfferMediatrRequest, PrintBuildingSavingsOfferResponse>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<PrintBuildingSavingsOfferResponse> Handle(Dto.PrintBuildingSavingsOfferMediatrRequest request, CancellationToken cancellation)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var model = new PrintBuildingSavingsOfferResponse()
        {
            OfferInstanceId = 1,
            ContentType = "application/pdf",
        };
        model.FileData = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes("d:/my_documents/tok dat.pdf"));
        return model;
    }
}
