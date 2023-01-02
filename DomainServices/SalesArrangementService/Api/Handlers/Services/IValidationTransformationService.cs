namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

internal interface IValidationTransformationService
{
    List<Contracts.ValidationMessage> TransformErrors(string json, Dictionary<string, Eas.CheckFormV2.Error[]>? errors);
}
