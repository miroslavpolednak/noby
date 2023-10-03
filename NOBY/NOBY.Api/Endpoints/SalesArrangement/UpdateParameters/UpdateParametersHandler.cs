using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _dto = NOBY.Api.Endpoints.SalesArrangement.Dto;
using FastEnumUtility;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal sealed class UpdateParametersHandler
    : IRequestHandler<UpdateParametersRequest>
{
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
            switch ((SalesArrangementTypes)saInstance.SalesArrangementTypeId)
            {
                case SalesArrangementTypes.Mortgage:
                    updateRequest.Mortgage = (await _helper.DeserializeAndValidate<_dto.ParametersMortgage>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.Mortgage);
                    break;

                case SalesArrangementTypes.Drawing:
                    updateRequest.Drawing = (await _helper.DeserializeAndValidate<_dto.ParametersDrawing>(request.Parameters, saInstance))
                        ?.ToDomainService();
                    break;

                case SalesArrangementTypes.GeneralChange:
                    updateRequest.GeneralChange = (await _helper.DeserializeAndValidate<Dto.GeneralChangeUpdate>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.GeneralChange);
                    break;

                case SalesArrangementTypes.HUBN:
                    updateRequest.HUBN = (await _helper.DeserializeAndValidate<Dto.HUBNUpdate>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.HUBN);
                    break;

                case SalesArrangementTypes.CustomerChange:
                    updateRequest.CustomerChange = (await _helper.DeserializeAndValidate<Dto.CustomerChangeUpdate>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange);
                    break;

                case SalesArrangementTypes.CustomerChange3602A:
                    updateRequest.CustomerChange3602A = (await _helper.DeserializeAndValidate<Dto.CustomerChange3602Update>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange3602A);
                    break;

                case SalesArrangementTypes.CustomerChange3602B:
                    updateRequest.CustomerChange3602B = (await _helper.DeserializeAndValidate<Dto.CustomerChange3602Update>(request.Parameters, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange3602B);
                    break;

                case SalesArrangementTypes.CustomerChange3602C:
                    updateRequest.CustomerChange3602C = (await _helper.DeserializeAndValidate<Dto.CustomerChange3602Update>(request.Parameters, saInstance))
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
                if (documentOnSaToSign.DocumentTypeId != DocumentTypes.DANRESID.ToByte() && documentOnSaToSign.DocumentOnSAId is not null ) // 13
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

    private static int[] _disallowedStates = new[]
    {
        (int)SalesArrangementStates.Cancelled,
        (int)SalesArrangementStates.Disbursed,
        (int)SalesArrangementStates.InApproval
    };

    private readonly UpdateParametersHelper _helper;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;

    public UpdateParametersHandler(
        UpdateParametersHelper helper,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        IDocumentOnSAServiceClient documentOnSaService)
    {
        _helper = helper;
        _codebookService = codebookService;
        _documentOnSaService = documentOnSaService;
        _salesArrangementService = salesArrangementService;
    }
}
