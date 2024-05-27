using SharedAudit;

namespace CIS.InternalServices.NotificationService.Api;

internal static class CommonExtensions
{
    public static ICollection<AuditLoggerHeaderItem> ToAuditLoggerHeaderItems(this Contracts.v2.Product product)
        => [new(product.ProductType.ToAuditLoggerType(), product.ProductId)];

    public static string ToAuditLoggerType(this Contracts.v2.Product.Types.ProductTypes productType)
        => productType switch
        {
            Contracts.v2.Product.Types.ProductTypes.MortgageFormId => AuditConstants.ProductNamesForm,
            Contracts.v2.Product.Types.ProductTypes.MortgageSalesArrangementId => AuditConstants.ProductNamesSalesArrangement,
            Contracts.v2.Product.Types.ProductTypes.MortgageCaseId => AuditConstants.ProductNamesCase,
            _ => throw new ArgumentOutOfRangeException(nameof(productType), $"Not expected productType value: {productType}"),
        };
}
