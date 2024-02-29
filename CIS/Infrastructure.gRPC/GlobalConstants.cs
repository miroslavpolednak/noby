namespace CIS.Infrastructure.gRPC;

public static class GlobalConstants
{
    /// <summary>
    /// gRPC ErrorInfo.domain pro ErrorInfo objekt kde je ulozeny CIS kod chyby
    /// </summary>
    public const string ErrorInfoDomainForCisExceptionCode = "cis_error_code";

    /// <summary>
    /// gRPC ResourceInfo.resourceType pro chyby zpusobene integracemi na externi sluzby
    /// </summary>
    public const string ResourceTypeForExternalService = "external_service";
}
