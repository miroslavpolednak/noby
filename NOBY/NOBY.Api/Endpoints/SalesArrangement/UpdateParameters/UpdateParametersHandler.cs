using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _dto = NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal sealed class UpdateParametersHandler
    : IRequestHandler<UpdateParametersRequest>
{
    private static TModel? deserializeModel<TModel>(in string dataString)
        where TModel : class
    {
        if (string.IsNullOrEmpty(dataString)) return null;

        return System.Text.Json.JsonSerializer.Deserialize<TModel>(dataString, _jsonSerializerOptions);
    }

    public async Task Handle(UpdateParametersRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // validace stavu
        if (_disallowedStates.Contains(saInstance.State))
        {
            throw new NobyValidationException(90028);
        }

        var updateRequest = new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = request.SalesArrangementId
        };

        if (request.Parameters is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Parameters).GetRawText();

            switch ((SalesArrangementTypes)saInstance.SalesArrangementTypeId)
            {
                case SalesArrangementTypes.Mortgage:
                    var realEstateTypes = await _codebookService.RealEstateTypes(cancellationToken);
                    updateRequest.Mortgage = deserializeModel<_dto.ParametersMortgage>(dataString)
                        ?.Validate(realEstateTypes)
                        ?.ToDomainService(saInstance.Mortgage);
                    break;

                case SalesArrangementTypes.Drawing:
                    updateRequest.Drawing = deserializeModel<_dto.ParametersDrawing>(dataString)
                        ?.ToDomainService();
                    break;

                case SalesArrangementTypes.GeneralChange:
                    updateRequest.GeneralChange = deserializeModel<Dto.GeneralChangeUpdate>(dataString)
                        ?.ToDomainService(saInstance.GeneralChange);
                    break;

                case SalesArrangementTypes.HUBN:
                    var realEstateTypes2 = await _codebookService.RealEstateTypes(cancellationToken);
                    updateRequest.HUBN = deserializeModel<Dto.HUBNUpdate>(dataString)
                        ?.Validate(realEstateTypes2)
                        ?.ToDomainService(saInstance.HUBN);
                    break;

                case SalesArrangementTypes.CustomerChange:
                    updateRequest.CustomerChange = deserializeModel<Dto.CustomerChangeUpdate>(dataString)
                        ?.ToDomainService(saInstance.CustomerChange);
                    break;

                case SalesArrangementTypes.CustomerChange3602A:
                    updateRequest.CustomerChange3602A = deserializeModel<Dto.CustomerChange3602Update>(dataString)
                        ?.ToDomainService(saInstance.CustomerChange3602A);
                    break;

                case SalesArrangementTypes.CustomerChange3602B:
                    updateRequest.CustomerChange3602B = deserializeModel<Dto.CustomerChange3602Update>(dataString)
                        ?.ToDomainService(saInstance.CustomerChange3602B);
                    break;

                case SalesArrangementTypes.CustomerChange3602C:
                    updateRequest.CustomerChange3602C = deserializeModel<Dto.CustomerChange3602Update>(dataString)
                        ?.ToDomainService(saInstance.CustomerChange3602C);
                    break;

                default:
                    throw new NotImplementedException($"SalesArrangementTypeId {saInstance.SalesArrangementTypeId} parameters model cast to domain service is not implemented");
            }
        }

        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(t => t.Id == saInstance.SalesArrangementTypeId);

        if (salesArrangementType.SalesArrangementCategory != (int)SalesArrangementCategories.ProductRequest)
        {
            var documentResponse = await _documentOnSaService.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

            foreach (var documentOnSaToSign in documentResponse.DocumentsOnSAToSign)
            {
                if (documentOnSaToSign is { IsValid: true, IsSigned: false, DocumentOnSAId: not null })
                {
                    await _documentOnSaService.StopSigning(new() { DocumentOnSAId = documentOnSaToSign.DocumentOnSAId.Value }, cancellationToken);
                    // We have to actualise SA after stop Signing (because stop Signing may change SA state)
                    _salesArrangementService.ClearSalesArrangementCache();
                    saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
                }
            }
            
            if (saInstance.State != (int)SalesArrangementStates.InProgress)
            {
                await _salesArrangementService.UpdateSalesArrangementState(request.SalesArrangementId, (int)SalesArrangementStates.InProgress, cancellationToken);
            }
        }
        else
        {
            // nastavit flowSwitch ParametersSavedAtLeastOnce pouze pro NE servisni SA
            await setFlowSwitches(saInstance, cancellationToken);
        }

        // update SA
        await _salesArrangementService.UpdateSalesArrangementParameters(updateRequest, cancellationToken);
    }

    private async Task setFlowSwitches(_SA.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        await _salesArrangementService.SetFlowSwitches(saInstance.SalesArrangementId, new()
        {
            new()
            {
                FlowSwitchId = (int)FlowSwitches.ParametersSavedAtLeastOnce,
                Value = true
            }
        }, cancellationToken);
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private static int[] _disallowedStates = new[]
    {
        (int)SalesArrangementStates.Cancelled,
        (int)SalesArrangementStates.Disbursed,
        (int)SalesArrangementStates.InApproval
    };

    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;

    public UpdateParametersHandler(
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        IDocumentOnSAServiceClient documentOnSaService)
    {
        _codebookService = codebookService;
        _documentOnSaService = documentOnSaService;
        _salesArrangementService = salesArrangementService;
    }
}
