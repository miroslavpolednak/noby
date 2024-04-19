using CIS.Core.Exceptions;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms;

public static class EasFormTypeFactory
{
    private static readonly List<FormTypeMapItem> _formTypeMap =
    [
        new FormTypeMapItem { DocumentTypeId = 4, SalesArrangementType = SalesArrangementTypes.Mortgage, EasFormType = EasFormType.F3601 },
        new FormTypeMapItem { DocumentTypeId = 5, SalesArrangementType = SalesArrangementTypes.Mortgage, EasFormType = EasFormType.F3602 },
        new FormTypeMapItem { DocumentTypeId = 6, SalesArrangementType = SalesArrangementTypes.Drawing, EasFormType = EasFormType.F3700 },
        new FormTypeMapItem { DocumentTypeId = 11, SalesArrangementType = SalesArrangementTypes.CustomerChange3602C, EasFormType = EasFormType.F3602 },
        new FormTypeMapItem { DocumentTypeId = 12, SalesArrangementType = SalesArrangementTypes.CustomerChange3602A, EasFormType = EasFormType.F3602 },
        new FormTypeMapItem { DocumentTypeId = 16, SalesArrangementType = SalesArrangementTypes.CustomerChange3602B, EasFormType = EasFormType.F3602 }
    ];

    public static EasFormType GetEasFormType(int documentTypeId)
    {
        var formType = _formTypeMap.FirstOrDefault(m => m.DocumentTypeId == documentTypeId);

        return formType?.EasFormType ?? throw new CisValidationException($"The eas form does not support document type {documentTypeId}");
    }

    public static DefaultValues CreateDefaultValues(EasFormType easFormType, SalesArrangementTypes salesArrangementType, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes)
    {
        var formType = _formTypeMap.First(m => m.EasFormType == easFormType && m.SalesArrangementType == salesArrangementType);

        var eaCodeMainId = documentTypes.First(d => d.Id == formType.DocumentTypeId).EACodeMainId;

        var formTypeName = easFormType switch
        {
            EasFormType.F3700 => "3700A",
            EasFormType.F3601 => "3601A",
            EasFormType.F3602 => "3602A",
            _ => throw new ArgumentOutOfRangeException(nameof(easFormType), easFormType, null)
        };

        return new DefaultValues { FormType = formTypeName, EaCodeMainId = eaCodeMainId };
    }

    private class FormTypeMapItem
    {
        public int DocumentTypeId { get; init; }

        public SalesArrangementTypes SalesArrangementType { get; init; }

        public EasFormType EasFormType { get; init; }
    }
}