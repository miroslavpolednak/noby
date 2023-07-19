namespace DomainServices.CodebookService.Contracts.v1;

public partial class GenericCodebookResponse : IItemsResponse<GenericCodebookResponse.Types.GenericCodebookItem> 
{
    public partial class Types
    {
        public partial class GenericCodebookItem : IBaseCodebook { }
    }
}
public partial class BankCodesResponse : IItemsResponse<BankCodesResponse.Types.BankCodeItem> { }
public partial class CollateralTypesResponse : IItemsResponse<CollateralTypesResponse.Types.CollateralTypeItem> { }
public partial class ContactTypesResponse : IItemsResponse<ContactTypesResponse.Types.ContactTypeItem> { }
public partial class CountriesResponse : IItemsResponse<CountriesResponse.Types.CountryItem> { }
public partial class CountryCodePhoneIdcResponse : IItemsResponse<CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem> { }
public partial class CurrenciesResponse : IItemsResponse<CurrenciesResponse.Types.CurrencyItem> { }
public partial class CustomerRolesResponse : IItemsResponse<CustomerRolesResponse.Types.CustomerRoleItem> { }
public partial class DeveloperSearchResponse : IItemsResponse<DeveloperSearchResponse.Types.DeveloperSearchItem> { }
public partial class DocumentFileTypesResponse : IItemsResponse<DocumentFileTypesResponse.Types.DocumentFileTypeItem> { }
public partial class DocumentOnSATypesResponse : IItemsResponse<DocumentOnSATypesResponse.Types.DocumentOnSATypeItem> { }
public partial class DocumentTemplateTypesResponse : IItemsResponse<DocumentTemplateTypesResponse.Types.DocumentTemplateTypeItem> { }
public partial class DocumentTemplateVariantsResponse : IItemsResponse<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem> { }
public partial class DocumentTemplateVersionsResponse : IItemsResponse<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem> { }
public partial class DocumentTypesResponse : IItemsResponse<DocumentTypesResponse.Types.DocumentTypeItem> { }
public partial class DrawingDurationsResponse : IItemsResponse<DrawingDurationsResponse.Types.DrawingDurationItem> { }
public partial class DrawingTypesResponse : IItemsResponse<DrawingTypesResponse.Types.DrawingTypeItem> { }
public partial class EaCodesMainResponse : IItemsResponse<EaCodesMainResponse.Types.EaCodesMainItem> { }
public partial class EducationLevelsResponse : IItemsResponse<EducationLevelsResponse.Types.EducationLevelItem> { }
public partial class FeesResponse : IItemsResponse<FeesResponse.Types.FeeItem> { }
public partial class FixedRatePeriodsResponse : IItemsResponse<FixedRatePeriodsResponse.Types.FixedRatePeriodItem> { }
public partial class FormTypesResponse : IItemsResponse<FormTypesResponse.Types.FormTypeItem> { }
public partial class GendersResponse : IItemsResponse<GendersResponse.Types.GenderItem> { }
public partial class GetGeneralDocumentListResponse : IItemsResponse<GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem> { }
public partial class HouseholdTypesResponse : IItemsResponse<HouseholdTypesResponse.Types.HouseholdTypeItem> { }
public partial class HousingConditionsResponse : IItemsResponse<HousingConditionsResponse.Types.HousingConditionItem> { }
public partial class ChannelsResponse : IItemsResponse<ChannelsResponse.Types.ChannelItem> { }
public partial class IdentificationDocumentTypesResponse : IItemsResponse<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> { }
public partial class IdentitySchemesResponse : IItemsResponse<IdentitySchemesResponse.Types.IdentitySchemeItem> { }
public partial class LoanPurposesResponse : IItemsResponse<LoanPurposesResponse.Types.LoanPurposeItem>
{
    public partial class Types
    {
        public partial class LoanPurposeItem : IBaseCodebook { }
    }
}
public partial class ObligationLaExposuresResponse : IItemsResponse<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> { }
public partial class ObligationTypesResponse : IItemsResponse<ObligationTypesResponse.Types.ObligationTypeItem> { }
public partial class PaymentDaysResponse : IItemsResponse<PaymentDaysResponse.Types.PaymentDayItem> { }
public partial class PostCodesResponse : IItemsResponse<PostCodesResponse.Types.PostCodeItem> { }
public partial class ProductTypesResponse : IItemsResponse<ProductTypesResponse.Types.ProductTypeItem>
{
    public partial class Types
    {
        public partial class ProductTypeItem : IBaseCodebook { }
    }
}
public partial class ProfessionCategoriesResponse : IItemsResponse<ProfessionCategoriesResponse.Types.ProfessionCategoryItem> { }
public partial class ProofTypesResponse : IItemsResponse<ProofTypesResponse.Types.ProofTypeItem> { }
public partial class PropertySettlementsResponse : IItemsResponse<PropertySettlementsResponse.Types.PropertySettlementItem> { }
public partial class RelationshipCustomerProductTypesResponse : IItemsResponse<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem> { }
public partial class RiskApplicationTypesResponse : IItemsResponse<RiskApplicationTypesResponse.Types.RiskApplicationTypeItem> { }
public partial class SalesArrangementStatesResponse : IItemsResponse<SalesArrangementStatesResponse.Types.SalesArrangementStateItem> { }
public partial class SalesArrangementTypesResponse : IItemsResponse<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> { }
public partial class SigningMethodsForNaturalPersonResponse : IItemsResponse<SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem> { }
public partial class SmsNotificationTypesResponse : IItemsResponse<SmsNotificationTypesResponse.Types.SmsNotificationTypeItem> { }
public partial class StatementFrequenciesResponse : IItemsResponse<StatementFrequenciesResponse.Types.StatementFrequencyItem> { }
public partial class StatementTypesResponse : IItemsResponse<StatementTypesResponse.Types.StatementTypeItem> { }
public partial class TinFormatsByCountryResponse : IItemsResponse<TinFormatsByCountryResponse.Types.TinFormatsByCountryItem> { }
public partial class TinNoFillReasonsByCountryResponse : IItemsResponse<TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem> { }
public partial class WorkflowConsultationMatrixResponse : IItemsResponse<WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem> { }
public partial class WorkflowTaskStatesResponse : IItemsResponse<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem> { }
public partial class WorkflowTaskStatesNobyResponse : IItemsResponse<WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem> { }
public partial class WorkflowTaskTypesResponse : IItemsResponse<WorkflowTaskTypesResponse.Types.WorkflowTaskTypesItem> { }
public partial class RealEstateSubtypesResponse : IItemsResponse<RealEstateSubtypesResponse.Types.RealEstateSubtypesResponseItem> { }
public partial class RealEstateTypesResponse : IItemsResponse<RealEstateTypesResponse.Types.RealEstateTypesResponseItem> { }
