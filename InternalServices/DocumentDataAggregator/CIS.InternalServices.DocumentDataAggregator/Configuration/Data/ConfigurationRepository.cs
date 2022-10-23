namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

[ScopedService, SelfService]
internal class ConfigurationRepository
{
    public List<DataSourceField> LoadDocumentFields()
    {
        return new List<DataSourceField>()
        {
            new()
            {
                DataSource = DataSource.OfferService,
                Path = "Offer.OfferId",
                TemplateFieldName = "IDNabidky"
            }
        };
    }
}