using DomainServices.SalesArrangementService.Api.Handlers.Forms;

namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

internal interface IValidationTransformationService
{
    List<Contracts.ValidationMessage> TransformErrors(Form form, Dictionary<string, ExternalServices.Eas.R21.CheckFormV2.Error[]>? errors);
}
