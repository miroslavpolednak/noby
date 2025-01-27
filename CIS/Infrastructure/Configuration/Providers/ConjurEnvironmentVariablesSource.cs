﻿using System.Collections;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace CIS.Infrastructure.Configuration.Providers;

internal sealed class ConjurEnvironmentVariablesProvider : ConfigurationProvider
{
    private readonly Func<IDictionary<string, string?>> _configDataFactory;

    public ConjurEnvironmentVariablesProvider(Func<IDictionary<string, string?>> configDataFactory) =>
        _configDataFactory = configDataFactory;

    public override void Load() => Data = _configDataFactory();
}

internal sealed partial class ConjurEnvironmentVariablesSource : IConfigurationSource
{
    private readonly IConfiguration _configuration;

    public ConjurEnvironmentVariablesSource(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new ConjurEnvironmentVariablesProvider(BuildConfigData);

    private ImmutableDictionary<string, string?> BuildConfigData()
    {
        var configData = ReplaceConjurPlaceholdersWithEnvironmentVariables();

        return configData.ToImmutableDictionary()!;
    }

    private IEnumerable<KeyValuePair<string, string>> ReplaceConjurPlaceholdersWithEnvironmentVariables()
    {
        var environmentVariables = GetEnvironmentVariables();

        var regex = ConjurPlaceholderRegex();

        var placeholders = _configuration.AsEnumerable()
                                         .Where(c => !string.IsNullOrWhiteSpace(c.Value))
                                         .Select(c => new { c.Key, OriginalValue = c.Value, Match = regex.Match(c.Value!) })
                                         .Where(c => c.Match.Success)
                                         .ToList();


        foreach (var placeholder in placeholders)
        {
            var conjurKey = placeholder.Match.Groups[1].Value;

            if (!environmentVariables.ContainsKey(conjurKey))
                continue;

            var sb = new StringBuilder(placeholder.OriginalValue);

            sb.Remove(placeholder.Match.Index, placeholder.Match.Length);
            sb.Insert(placeholder.Match.Index, environmentVariables[conjurKey]);

            yield return KeyValuePair.Create(placeholder.Key, sb.ToString());
        }
    }

    private static Dictionary<string, string?> GetEnvironmentVariables()
    {
        const string NobyEnvPrefix = "NOBY_";

        var envVariablesFiltered = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User)
                                              .Cast<DictionaryEntry>()
                                              .Where(entry => ((string)entry.Key).StartsWith(NobyEnvPrefix, StringComparison.OrdinalIgnoreCase))
                                              .Select(entry => KeyValuePair.Create((string)entry.Key, (string?)entry.Value));

        return new Dictionary<string, string?>(envVariablesFiltered, StringComparer.OrdinalIgnoreCase);
    }

    [GeneratedRegex(@"\$(.*?)\$")]
    private static partial Regex ConjurPlaceholderRegex();
}