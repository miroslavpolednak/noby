using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Contracts.UpdateMortgageRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public UpdateMortgageHandler(
        ICodebookServiceClients codebookService,
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<UpdateMortgageHandler> logger) : base(codebookService, repository, mpHomeClient, logger) { }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateMortgageRequest request, CancellationToken cancellation)
    {
        await UpdateLoan(request.ProductId, request.Mortgage, false, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

}