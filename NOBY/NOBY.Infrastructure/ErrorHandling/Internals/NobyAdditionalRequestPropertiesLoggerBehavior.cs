using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NOBY.Dto.Attributes;

namespace NOBY.Infrastructure.ErrorHandling.Internals;

public class NobyAdditionalRequestPropertiesLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TRequest> _logger;

    public NobyAdditionalRequestPropertiesLoggerBehavior(IHttpContextAccessor httpContextAccessor, ILogger<TRequest> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
            return await next();

        _httpContextAccessor.HttpContext!.Request.Body.Position = 0;

        using var stream = new StreamReader(_httpContextAccessor.HttpContext.Request.Body);

        var requestJson = await stream.ReadToEndAsync(cancellationToken);
        var oneOfProperties = new List<string>();

        var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            ContractResolver = new ExcludeOneOfContractResolver(oneOfProperties)
        });

        var contractProperties = GetPropertyNames(JObject.FromObject(request, jsonSerializer));
        var requestProperties = string.IsNullOrEmpty(requestJson) ? new List<string>(0) : GetPropertyNames(JObject.Parse(requestJson));

        var extraProperties = requestProperties.Where(prop =>
        {
            if (contractProperties.Contains(prop, StringComparer.OrdinalIgnoreCase))
                return false;

            if (oneOfProperties.Any(oneOf => prop.Equals(oneOf, StringComparison.OrdinalIgnoreCase) ||
                                             prop.StartsWith($"{oneOf}.", StringComparison.OrdinalIgnoreCase) ||
                                             prop.Contains($".{oneOf}.", StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }).ToList();


        if (extraProperties.Count != 0)
        {
            _logger.LogWarning("Request {RequestName} has additional properties than contract. Differences: {ExtraProperties}", 
                               typeof(TRequest).FullName, 
                               extraProperties);
        }

        return await next();
    }

    private static List<string> GetPropertyNames(JObject obj, string parentName = "")
    {
        List<string> propertyNames = [];

        foreach (var property in obj.Properties())
        {
            var propertyName = string.IsNullOrEmpty(parentName) ? property.Name : $"{parentName}.{property.Name}";
            propertyNames.Add(propertyName);

            if (property.Value.Type == JTokenType.Object) 
                propertyNames.AddRange(GetPropertyNames((JObject)property.Value, propertyName));
        }

        return propertyNames;
    }

    private class ExcludeOneOfContractResolver : DefaultContractResolver
    {
        private readonly ICollection<string> _oneOfProperties;

        public ExcludeOneOfContractResolver(ICollection<string> oneOfProperties)
        {
            _oneOfProperties = oneOfProperties;
        }

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