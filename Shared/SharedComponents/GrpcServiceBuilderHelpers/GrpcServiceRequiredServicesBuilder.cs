namespace SharedComponents.GrpcServiceBuilderHelpers;

public sealed class GrpcServiceRequiredServicesBuilder
{
    internal GrpcServiceRequiredServicesBuilder() { }

    internal bool CaseService;
    internal bool CodebookService;
    internal bool CustomerService;
    internal bool DocumentArchiveService;
    internal bool DocumentOnSAService;
    internal bool HouseholdService;
    internal bool OfferService;
    internal bool ProductService;
    internal bool RealEstateValuationService;
    internal bool RiskIntegrationService;
    internal bool SalesArrangementService;
    internal bool UserService;
    internal bool DataAggregatorService;
    internal bool DocumentGeneratorService;

    public GrpcServiceRequiredServicesBuilder AddCaseService()
    {
        CaseService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddCodebookService()
    {
        CodebookService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddCustomerService()
    {
        CustomerService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddDocumentArchiveService()
    {
        DocumentArchiveService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddDocumentOnSAService()
    {
        DocumentOnSAService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddHouseholdService()
    {
        HouseholdService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddOfferService()
    {
        OfferService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddProductService()
    {
        ProductService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddRealEstateValuationService()
    {
        RealEstateValuationService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddRiskIntegrationService()
    {
        RiskIntegrationService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddSalesArrangementService()
    {
        SalesArrangementService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddUserService()
    {
        UserService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddDataAggregatorService()
    {
        DataAggregatorService = true;
        return this;
    }

    public GrpcServiceRequiredServicesBuilder AddDocumentGeneratorService()
    {
        DocumentGeneratorService = true;
        return this;
    }
}