using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class CustomerTaxResidencyTemplateData : AggregatedData
{
    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer, AddressTypes.Permanent, _codebookManager.Countries);


    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().Countries();
    }
}