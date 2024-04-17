using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOfferDeveloper;

internal sealed class GetOfferDeveloperHandler(
    IDocumentDataStorage _documentDataStorage,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetOfferDeveloperRequest, GetOfferDeveloperResponse>
{
    public async Task<GetOfferDeveloperResponse> Handle(GetOfferDeveloperRequest request, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageOfferData>(request.OfferId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);
        var inputs = offerData.Data!.SimulationInputs;

        var response = new GetOfferDeveloperResponse
        {
            DeveloperId = inputs.Developer?.DeveloperId,
            ProjectId = inputs.Developer?.ProjectId
        };

        if (inputs.Developer?.DeveloperId != null && inputs.Developer?.ProjectId != null)
        {
            response.IsDeveloperSet = true;

            var project = await _codebookService.GetDeveloperProject(inputs.Developer.DeveloperId.Value, inputs.Developer.ProjectId.Value, cancellationToken);

            response.IsDeveloperAllowed = _allowedMassValuations.Contains(project.MassEvaluation);
        }

        return response;
    }

    private static int[] _allowedMassValuations = [-1, 1];
}
