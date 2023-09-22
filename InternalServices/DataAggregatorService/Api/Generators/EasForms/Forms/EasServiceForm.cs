﻿using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.Forms;

internal class EasServiceForm<TFormData> : EasForm<TFormData> where TFormData : AggregatedData
{
    private readonly List<DocumentTypesResponse.Types.DocumentTypeItem> _documentTypes;

    public EasServiceForm(TFormData formData, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes) : base(formData)
    {
        _documentTypes = documentTypes;
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields)
    {
        var dynamicValues = dynamicFormValues.First();

        dynamicValues.FormId = _formData.Custom.DocumentOnSa.FinalDocument?.FormId;
        dynamicValues.DocumentId = _formData.Custom.DocumentOnSa.FinalDocument?.EArchivId;

        var easFormType = EasFormTypeFactory.GetEasFormType(dynamicValues.DocumentTypeId);

        yield return new Form
        {
            EasFormType = easFormType,
            DynamicFormValues = dynamicValues,
            DefaultValues = EasFormTypeFactory.CreateDefaultValues(easFormType, _documentTypes),
            Json = CreateJson(sourceFields),
            FormIdentifier = CalculateFormIdentifier('S', easFormType, _formData.SalesArrangement.SalesArrangementId)
        };
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = _formData.Case.Data.ContractNumber;
    }
}