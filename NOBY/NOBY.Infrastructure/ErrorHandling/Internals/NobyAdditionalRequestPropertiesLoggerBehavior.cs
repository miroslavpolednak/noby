using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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

        _httpContextAccessor.HttpContext.Request.Body.Position = 0;

        using var stream = new StreamReader(_httpContextAccessor.HttpContext.Request.Body);

        var requestJson = await stream.ReadToEndAsync(cancellationToken);

        var requestProperties = GetPropertyNames(JObject.FromObject(request));
        var contractProperties = string.IsNullOrEmpty(requestJson) ? new List<string>(0) : GetPropertyNames(JObject.Parse(requestJson));

        var extraProperties = contractProperties.Except(requestProperties, StringComparer.OrdinalIgnoreCase).ToList();

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
}