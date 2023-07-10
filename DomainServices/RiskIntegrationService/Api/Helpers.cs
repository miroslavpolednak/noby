using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api;

internal static class Helpers
{
    private static string[] _kbGroupPersonValues = new[] { "KBAD", "MPAD" };

    public static bool IsDealerSchema(int? dealerCompanyId)
        => dealerCompanyId != null;

    [Obsolete("jeste bude upresneno co s tim")]
    public static bool IsDealerSchema(string? schemaToCheck)
            => string.IsNullOrEmpty(schemaToCheck) ? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.DealerSchemeIsNull) : !_kbGroupPersonValues.Contains(schemaToCheck);

    public static string GetResourceInstanceFromMandant(int? mandantId)
        => !mandantId.HasValue || mandantId == (int)CIS.Foms.Enums.Mandants.Kb ? "KBCZ" : "MPSS";

    public static TResponse? GetEnumFromString<TResponse>(string? enumValue, TResponse? defaultValue = default(TResponse?)) 
        where TResponse : struct
    {
        if (string.IsNullOrEmpty(enumValue)) return defaultValue;

        if (!Enum.TryParse(typeof(TResponse), enumValue, out object? outValue))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.C4MEnumNotFound, $"{typeof(TResponse)} '{enumValue}'");
        return (TResponse)outValue!;
    }

    public static TResponse GetRequiredEnumFromString<TResponse>(string enumValue)
        where TResponse : struct
    {
        if (!Enum.TryParse(typeof(TResponse), enumValue, out object? outValue))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.C4MEnumNotFound, $"{typeof(TResponse)} '{enumValue}'");
        return (TResponse)outValue!;
    }

    public static TResponse? GetEnumFromInt<TResponse>(int? id, TResponse? defaultValue = default(TResponse?))
        where TResponse : struct
        => id.HasValue ? GetEnumFromString<TResponse>(id.ToString(), defaultValue) : default(TResponse?);

    public static RiskApplicationTypesResponse.Types.RiskApplicationTypeItem GetRiskApplicationType(List<RiskApplicationTypesResponse.Types.RiskApplicationTypeItem> riskApplicationTypes, int productTypeId)
        => riskApplicationTypes.FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ProductTypeIdNotFound, productTypeId);
}

