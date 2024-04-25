using System.Net.Mime;
using System.Reflection;
using CIS.Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NOBY.Dto.Attributes;

namespace NOBY.Infrastructure.ErrorHandling.Internals;

public class NobyAdditionalRequestPropertiesLoggerBehavior<TRequest, TResponse>(
    IHttpContextAccessor _httpContextAccessor, 
    ILogger<TRequest> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null || _httpContextAccessor.HttpContext.Request.ContentType != MediaTypeNames.Application.Json)
            return await next();

        _httpContextAccessor.HttpContext.Request.Body.Position = 0;

        using var stream = new StreamReader(_httpContextAccessor.HttpContext.Request.Body);

        var requestJson = await stream.ReadToEndAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(requestJson))
            return await next();

        var oneOfProperties = new List<string>();

        var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            ContractResolver = new ExcludeOneOfContractResolver(oneOfProperties)
        });

        var extraProperties = GetMissingProperties(JToken.Parse(requestJson), JToken.FromObject(request, jsonSerializer), oneOfProperties);

        if (extraProperties.Count != 0)
        {
            _logger.RequestHasAdditionalPropsThanContract(typeof(TRequest).FullName, extraProperties);
        }

        return await next();
    }

    private static List<string> GetMissingProperties(JToken requestToken, JToken contractToken, List<string> oneOfProperties)
    {
        List<string> missingProperties = [];

        if (requestToken.Type == JTokenType.Array && contractToken.Type == JTokenType.Array && ((JArray)requestToken).Count != 0 && ((JArray)contractToken).Count != 0)
        {
            requestToken = ((JArray)requestToken).First();
            contractToken = ((JArray)contractToken).First();
        }

        if (requestToken.Type != JTokenType.Object || contractToken.Type != JTokenType.Object)
            return missingProperties;

        foreach (JProperty requestProperty in ((JObject)requestToken).Properties())
        {
            var propertyName = requestProperty.Name;
            var childToken1 = requestProperty.Value;

            if (oneOfProperties.Any(oneOf => propertyName.Equals(oneOf, StringComparison.OrdinalIgnoreCase) ||
                                             propertyName.StartsWith($"{oneOf}.", StringComparison.OrdinalIgnoreCase) ||
                                             propertyName.Contains($".{oneOf}.", StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            var property2 = ((JObject)contractToken).Property(propertyName, StringComparison.OrdinalIgnoreCase);
            if (property2 == null)
            {
                missingProperties.Add(propertyName);
            }
            else
            {
                var childToken2 = property2.Value;
                if (childToken1.Type == JTokenType.Object && childToken2.Type == JTokenType.Object)
                {
                    missingProperties.AddRange(GetMissingProperties(childToken1, childToken2, oneOfProperties).Select(childProperty => propertyName + "." + childProperty));
                }
            }
        }

        return missingProperties;
    }

    private sealed class ExcludeOneOfContractResolver(ICollection<string> _oneOfProperties) 
        : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (member.GetCustomAttribute<SwaggerOneOfAttribute>() != null)
            {
                _oneOfProperties.Add(member.Name);

                return null!;
            }

            return base.CreateProperty(member, memberSerialization);
        }
    }
}