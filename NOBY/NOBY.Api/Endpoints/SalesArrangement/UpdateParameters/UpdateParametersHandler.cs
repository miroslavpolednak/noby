using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal sealed class UpdateParametersHandler
    : IRequestHandler<UpdateParametersRequest>
{
    public async Task Handle(UpdateParametersRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var updateRequest = new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = request.SalesArrangementId
        };

        if (request.Parameters is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Parameters).GetRawText();

            switch ((CIS.Foms.Types.Enums.SalesArrangementTypes)saInstance.SalesArrangementTypeId)
            {
                case CIS.Foms.Types.Enums.SalesArrangementTypes.Mortgage:
                    var o1 = System.Text.Json.JsonSerializer.Deserialize<SalesArrangement.Dto.ParametersMortgage>(dataString, _jsonSerializerOptions);
                    if (o1 is not null)
                    {
                        if (string.IsNullOrEmpty(o1.IncomeCurrencyCode) || string.IsNullOrEmpty(o1.ResidencyCurrencyCode))
                        {
                            throw new NobyValidationException(90019);
                        }
                        updateRequest.Mortgage = o1.ToDomainService();
                    }
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.Drawing:
                    var o2 = System.Text.Json.JsonSerializer.Deserialize<SalesArrangement.Dto.ParametersDrawing>(dataString, _jsonSerializerOptions);
                    if (o2 is not null)
                        updateRequest.Drawing = o2.ToDomainService();
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.GeneralChange:
                    var o3 = System.Text.Json.JsonSerializer.Deserialize<Dto.GeneralChangeUpdate>(dataString, _jsonSerializerOptions);
                    if (o3 is not null)
                        updateRequest.GeneralChange = o3.ToDomainService(saInstance.GeneralChange);
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.HUBN:
                    var o4 = System.Text.Json.JsonSerializer.Deserialize<Dto.HUBNUpdate>(dataString, _jsonSerializerOptions);
                    if (o4 is not null)
                        updateRequest.HUBN = o4.ToDomainService(saInstance.HUBN);
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.CustomerChange:
                    var o5 = System.Text.Json.JsonSerializer.Deserialize<Dto.CustomerChangeUpdate>(dataString, _jsonSerializerOptions);
                    if (o5 is not null)
                        updateRequest.CustomerChange = o5.ToDomainService(saInstance.CustomerChange);
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.CustomerChange3602A:
                    var o6 = System.Text.Json.JsonSerializer.Deserialize<Dto.CustomerChange3602Update>(dataString, _jsonSerializerOptions);
                    if (o6 is not null)
                        updateRequest.CustomerChange3602A = o6.ToDomainService(saInstance.CustomerChange3602A);
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.CustomerChange3602B:
                    var o7 = System.Text.Json.JsonSerializer.Deserialize<Dto.CustomerChange3602Update>(dataString, _jsonSerializerOptions);
                    if (o7 is not null)
                        updateRequest.CustomerChange3602B = o7.ToDomainService(saInstance.CustomerChange3602B);
                    break;

                case CIS.Foms.Types.Enums.SalesArrangementTypes.CustomerChange3602C:
                    var o8 = System.Text.Json.JsonSerializer.Deserialize<Dto.CustomerChange3602Update>(dataString, _jsonSerializerOptions);
                    if (o8 is not null)
                        updateRequest.CustomerChange3602C = o8.ToDomainService(saInstance.CustomerChange3602C);
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
                    await _documentOnSaService.StopSigning(documentOnSaToSign.DocumentOnSAId.Value, cancellationToken);
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
