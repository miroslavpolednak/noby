using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Helpers;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.StringInterpolation;

public class StringInterpolationTests
{
    [Fact]
    public void MissingKeyValue()
    {
        var template = "Template {{var_a}} {{var_b}}";
        var keyValues = new Dictionary<string, string>()
        {
            { "var_a", "value A" }
        };

        var exception = Assert.Throws<CisValidationException>(() =>
        {
            template.Validate(keyValues.Select(k => k.Key));
        });

        Assert.Equal("Missing key 'var_b'.", exception.Message);
    }

    [Fact]
    public void NotExpectedKeyValue()
    {
        var template = "Template {{var_a}} {{var_b}}";
        var keyValues = new Dictionary<string, string>()
        {
            { "var_a", "value A" },
            { "var_b", "value B" },
            { "var_c", "value C" },
        };

        var exception = Assert.Throws<CisValidationException>(() =>
        {
            template.Validate(keyValues.Select(k => k.Key));
        });
        
        Assert.Equal("Key 'var_c' is not expected.", exception.Message);
    }
    
    [Fact]
    public void SuccessInterpolation()
    {
        var template = "Template {{var_a}} {{var_b}}";
        var keyValues = new Dictionary<string, string>()
        {
            { "var_a", "value A" },
            { "var_b", "value B" },
        };

        template.Validate(keyValues.Select(k => k.Key));
        var text = template.Interpolate(keyValues);
        Assert.Equal("Template value A value B", text);    
    }
    
}