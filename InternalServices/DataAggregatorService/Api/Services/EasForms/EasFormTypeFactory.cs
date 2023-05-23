using CIS.Core.Exceptions;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

public static class EasFormTypeFactory
{
    private static readonly Dictionary<EasFormType, int> _formTypeMap = new()
    {
        { EasFormType.F3601, 4 },
        { EasFormType.F3602, 5 },
        { EasFormType.F3700, 6 }
    };

    public static EasFormType GetEasFormType(int documentTypeId)
    {
        var keyValuePair = _formTypeMap.FirstOrDefault(d => d.Value == documentTypeId);

        if (keyValuePair.Equals(default(KeyValuePair<EasFormType, int>)))
            throw new CisValidationException($"The eas form does not support document type {documentTypeId}");

        return keyValuePair.Key;
    }

    public static DefaultValues CreateDefaultValues(EasFormType easFormType, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes)
    {
        var documentTypeId = _formTypeMap[easFormType];

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