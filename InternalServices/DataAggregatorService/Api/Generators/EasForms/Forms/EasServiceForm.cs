using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
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

        var easFormType = EasFormTypeFactory.GetEasFormType(dynamicValues.DocumentTypeId);

        if (easFormType == EasFormType.F3700)
        {
            dynamicValues.FormId = _formData.Custom.DocumentOnSa.LastSignedDocument?.FormId;
            dynamicValues.DocumentId = _formData.Custom.DocumentOnSa.LastSignedDocument?.EArchivId;
        }
        else
        {
            dynamicValues.FormId = _formData.Custom.DocumentOnSa.FinalDocument?.FormId;
            dynamicValues.DocumentId = _formData.Custom.DocumentOnSa.FinalDocument?.EArchivId;
        }

        yield return new Form
        {
            EasFormType = easFormType,
            DynamicFormValues = dynamicValues,
            DefaultValues = EasFormTypeFactory.CreateDefaultValues(easFormType, (SalesArrangementTypes)_formData.SalesArrangement.SalesArrangementTypeId, _documentTypes),
            Json = CreateJson(sourceFields),
            FormIdentifier = $"S{_formData.SalesArrangement.SalesArrangementId.ToString(CultureInfo.InvariantCulture)}"
        };
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = _formData.Case.Data.ContractNumber;
    }
}