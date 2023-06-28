using CIS.Core.Exceptions;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

public static class EasFormTypeFactory
{
    private static readonly Dictionary<int, EasFormType> _formTypeMap = new()
    {
        { 4, EasFormType.F3601 },
        { 5, EasFormType.F3602 },
        { 6, EasFormType.F3700 },
        { 11, EasFormType.F3602 },
        { 12, EasFormType.F3602 },
        { 16, EasFormType.F3602 }
    };

    public static EasFormType GetEasFormType(int documentTypeId)
    {
        if (!_formTypeMap.TryGetValue(documentTypeId, out var formType))
            throw new CisValidationException($"The eas form does not support document type {documentTypeId}");

        return formType;
    }

    public static DefaultValues CreateDefaultValues(EasFormType easFormType, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes)
    {
        var documentTypeId = _formTypeMap.First(v => v.Value == easFormType).Key;

        var eaCodeMainId = documentTypes.First(d => d.Id == documentTypeId).EACodeMainId;

        var formTypeName = easFormType switch
        {
            EasFormType.F3700 => "3700A",
            EasFormType.F3601 => "3601A",
            EasFormType.F3602 => "3602A",
            _ => throw new ArgumentOutOfRangeException(nameof(easFormType), easFormType, null)
        };

        return new DefaultValues { FormType = formTypeName, EaCodeMainId = eaCodeMainId };
    }
}