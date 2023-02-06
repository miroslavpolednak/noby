using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal class EasProductForm : EasForm
{
    private ProductFormData ProductData => (ProductFormData)FormData;

    public EasProductForm(ProductFormData formData) : base(formData)
    {
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<EasFormSourceField> sourceFields, IEnumerable<DynamicFormValues> dynamicFormValues)
    {
        var dynamicFromValuesEnumerator = dynamicFormValues.GetEnumerator();

        return sourceFields.GroupBy(f => f.FormType)
                           .SelectMany(group => CreateForms(group.Key, group.AsEnumerable(), GetDynamicFormValues(dynamicFromValuesEnumerator)));
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = FormData.SalesArrangement.ContractNumber;

        response.Product = new ProductData
        {
            CustomersOnSa =
            {
                ProductData.HouseholdData.CustomersOnSa.Select(x => new CustomerOnSa
                {
                    CustomerOnSaId = x.CustomerOnSAId,
                    Identities = { x.CustomerIdentifiers }
                })
            },
            Households =
            {
                ProductData.HouseholdData.Households.Select(x => new Household
                {
                    HouseholdId = x.HouseholdId,
                    HouseholdTypeId = (int)x.HouseholdType,
                    CustomerOnSaId1 = x.CustomerOnSaId1,
                    CustomerOnSaId2 = x.CustomerOnSaId2
                })
            },
            EmployementIncomes = { ProductData.HouseholdData.Incomes.Where(i => i.Value.IncomeTypeId == (int)CustomerIncomeTypes.Employement).Select(x => new EmployementIncome
            {
                IncomeId = x.Value.IncomeId,
                IsInProbationaryPeriodHasValue = (x.Value.Employement?.Job?.IsInProbationaryPeriod).HasValue,
                IsInTrialPeriodHasValue = (x.Value.Employement?.Job?.IsInTrialPeriod).HasValue
            }) }
        };
    }

    private IEnumerable<Form> CreateForms(EasFormType type, IEnumerable<EasFormSourceField> sourceFields, DynamicFormValues? dynamicFormValues)
    {
        ProductData.DynamicFormValues = dynamicFormValues;

        if (type == EasFormType.F3602)
            return CreateF3602Form(sourceFields.ToList(), dynamicFormValues);

        return new[]
        {
            new Form
            {
                EasFormType = type,
                DynamicFormValues = dynamicFormValues,
                DefaultValues = DefaultValuesFactory.Create(type),
                Json = CreateJson(sourceFields)
            }
        };
    }

    private IEnumerable<Form> CreateF3602Form(ICollection<EasFormSourceField> sourceFields, DynamicFormValues? dynamicFormValues)
    {
        var householdTypes = new[] { HouseholdTypes.Codebtor, HouseholdTypes.Garantor };

        foreach (var householdType in householdTypes)
        {
            if (!(ProductData).HouseholdData.TrySetHousehold(householdType))
                continue;

            yield return new Form
            {
                EasFormType = EasFormType.F3602,
                DynamicFormValues = dynamicFormValues,
                DefaultValues = DefaultValuesFactory.Create(EasFormType.F3602),
                Json = CreateJson(sourceFields)
            };
        }
    }
}