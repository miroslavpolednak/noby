using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;
using NOBY.Services.SigningHelper;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal sealed class UpdateParametersHandler(
    UpdateParametersValidator _helper,
    ICodebookServiceClient _codebookService,
    ISalesArrangementServiceClient _salesArrangementService,
    IDocumentOnSAServiceClient _documentOnSaService,
    ISigningHelperService _signingHelperService)
        : IRequestHandler<SalesArrangementUpdateParametersRequest>
{
    public async Task Handle(SalesArrangementUpdateParametersRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // validace stavu
        if (saInstance.IsInState(_disallowedStates))
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
                    updateRequest.Mortgage = (await _helper.Validate(request.Parameters?.Mortgage, saInstance))
                        ?.ToDomainService(saInstance.Mortgage);
                    break;

                case SalesArrangementTypes.Drawing:
                    updateRequest.Drawing = (await _helper.Validate(request.Parameters?.Drawing, saInstance))
                        ?.ToDomainService();
                    break;

                case SalesArrangementTypes.GeneralChange:
                    updateRequest.GeneralChange = (await _helper.Validate(request.Parameters?.GeneralChange, saInstance))
                        ?.ToDomainService(saInstance.GeneralChange);
                    break;

                case SalesArrangementTypes.HUBN:
                    updateRequest.HUBN = (await _helper.Validate(request.Parameters?.Hubn, saInstance))
                        ?.ToDomainService(saInstance.HUBN);
                    break;

                case SalesArrangementTypes.CustomerChange:
                    updateRequest.CustomerChange = (await _helper.Validate(request.Parameters?.CustomerChange, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange);
                    break;

                case SalesArrangementTypes.CustomerChange3602A:
                    updateRequest.CustomerChange3602A = (await _helper.Validate(request.Parameters?.CustomerChange3602, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange3602A);
                    break;

                case SalesArrangementTypes.CustomerChange3602B:
                    updateRequest.CustomerChange3602B = (await _helper.Validate(request.Parameters?.CustomerChange3602, saInstance))
                        ?.ToDomainService(saInstance.CustomerChange3602B);
                    break;

                case SalesArrangementTypes.CustomerChange3602C:
                    updateRequest.CustomerChange3602C = (await _helper.Validate(request.Parameters?.CustomerChange3602, saInstance))
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
                if (documentOnSaToSign.DocumentTypeId != DocumentTypes.DANRESID.ToByte() && documentOnSaToSign.DocumentOnSAId is not null) // 13
                {
                    await _signingHelperService.StopSinningAccordingState(new()
                    {
                        DocumentOnSAId = documentOnSaToSign.DocumentOnSAId!.Value,
                        SignatureTypeId = documentOnSaToSign.SignatureTypeId,
                        SalesArrangementId = documentOnSaToSign.SalesArrangementId
                    }, cancellationToken);

                    // We have to actualise SA after stop Signing (because stop Signing may change SA state)
                    _salesArrangementService.ClearSalesArrangementCache();
                    saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
                }
            }

            if (saInstance.IsInState(_saStatesForUpdate))
            {
                await _salesArrangementService.UpdateSalesArrangementState(request.SalesArrangementId, EnumSalesArrangementStates.InProgress, cancellationToken);
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
        await _salesArrangementService.SetFlowSwitch(saInstance.SalesArrangementId, FlowSwitches.ParametersSavedAtLeastOnce, true, cancellationToken);
    }

	public static readonly EnumSalesArrangementStates[] _saStatesForUpdate =
	[
		EnumSalesArrangementStates.NewArrangement,
		EnumSalesArrangementStates.InApproval,
		EnumSalesArrangementStates.Disbursed,
		EnumSalesArrangementStates.InSigning,
		EnumSalesArrangementStates.ToSend,
		EnumSalesArrangementStates.Finished,
		EnumSalesArrangementStates.Cancelled,
		EnumSalesArrangementStates.RC2
	];

	private static readonly EnumSalesArrangementStates[] _disallowedStates =
    [
        EnumSalesArrangementStates.Cancelled,
        EnumSalesArrangementStates.Disbursed,
        EnumSalesArrangementStates.InApproval
    ];
}
