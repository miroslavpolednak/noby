﻿namespace DomainServices.SalesArrangementService.Api.Services;

internal interface IValidationTransformationService
{
    List<Contracts.ValidationMessage> TransformErrors(string json, Dictionary<string, Eas.CheckFormV2.ErrorDto[]>? errors);
}
