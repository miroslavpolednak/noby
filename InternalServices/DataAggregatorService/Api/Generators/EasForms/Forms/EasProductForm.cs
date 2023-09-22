using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.Forms;

internal class EasProductForm : EasForm<ProductFormData>
{
    private readonly List<DocumentTypesResponse.Types.DocumentTypeItem> _documentTypes;

    public EasProductForm(ProductFormData formData, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes) : base(formData)
    {
        _documentTypes = documentTypes;
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields)
    {
        var sourceFieldsGroup = sourceFields.ToLookup(f => f.EasFormType);

        foreach (var dynamicValues in dynamicFormValues)
        {
            var easFormType = EasFormTypeFactory.GetEasFormType(dynamicValues.DocumentTypeId);

            _formData.Custom.DocumentOnSa.Configure(dynamicValues.DocumentTypeId);
            _formData.HouseholdData.SetHouseholdData(dynamicValues.HouseholdId!.Value);

            dynamicValues.FormId = _formData.Custom.DocumentOnSa.FinalDocument?.FormId;
            dynamicValues.DocumentId = _formData.Custom.DocumentOnSa.FinalDocument?.EArchivId;

            yield return new Form
            {
                EasFormType = easFormType,
                DynamicFormValues = dynamicValues,
                DefaultValues = EasFormTypeFactory.CreateDefaultValues(easFormType, _documentTypes),
                Json = CreateJson(sourceFieldsGroup[easFormType]),
                FormIdentifier = CalculateFormIdentifier('P', easFormType, (long)_formData.SalesArrangement.SalesArrangementId << 32 | (uint)_formData.HouseholdData.HouseholdDto.HouseholdId)
            };
        }
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = _formData.SalesArrangement.ContractNumber;

        response.Product = new ProductData
        {
            CustomersOnSa =
            {
                _formData.HouseholdData.CustomersOnSa.Select(x => new ProductCustomerOnSa
                {
                    CustomerOnSaId = x.CustomerOnSAId,
                    Identities = { x.CustomerIdentifiers }
                })
            },
            Households =
            {
                _formData.HouseholdData.Households.Select(x => new ProductHousehold
                {
                    HouseholdId = x.HouseholdId,
                    HouseholdTypeId = (int)x.HouseholdType,
                    CustomerOnSaId1 = x.CustomerOnSaId1,
                    CustomerOnSaId2 = x.CustomerOnSaId2
                })
            },
            EmployementIncomes =
            {
                _formData.HouseholdData.Incomes.Where(i => i.Value.IncomeTypeId == (int)CustomerIncomeTypes.Employement).Select(x => new ProductEmployementIncome
                {
                    IncomeId = x.Value.IncomeId,
                    IsInProbationaryPeriodHasValue = (x.Value.Employement?.Job?.IsInProbationaryPeriod).HasValue,
                    IsInTrialPeriodHasValue = (x.Value.Employement?.Job?.IsInTrialPeriod).HasValue
                })
            }
        };
    }
}