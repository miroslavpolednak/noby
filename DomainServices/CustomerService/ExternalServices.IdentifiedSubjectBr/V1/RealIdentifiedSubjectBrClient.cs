﻿using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using CIS.Core.Exceptions;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class RealIdentifiedSubjectBrClient : IIdentifiedSubjectBrClient
{
    private const string BaseAddress = "/v1/identified-subject";

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        //Need that because of a phone IDC "+"
        _serializerOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    }

    public async Task<IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default)
    {
        var address = $"{_httpClient.BaseAddress}{BaseAddress}{(hardCreate ? "?hardCreate=true" : "")}";

        var response = await _httpClient.PostAsJsonAsync(address, request, _serializerOptions, cancellationToken);

        if (response.IsSuccessStatusCode)
            return new IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>
            {
                Result = await response.Content.ReadFromJsonAsync<CreateIdentifiedSubjectResponse>(cancellationToken: cancellationToken)
                         ?? throw new CisExternalServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CreateIdentifiedSubject), nameof(CreateIdentifiedSubjectResponse))
            };

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var schemaGenerator = new JSchemaGenerator();

            var errorSchema = schemaGenerator.Generate(typeof(IdentifiedSubjectError));

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            var errorObject = JObject.Parse(jsonResponse);

            //Error is business validation
            if (errorObject.IsValid(errorSchema))
            {
                return new IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>
                {
                    Error = errorObject.ToObject<IdentifiedSubjectError>()
                };
            }

            var validationErrorSchema = schemaGenerator.Generate(typeof(IdentifiedSubjectValidationError));

            var validationErrorObject = JObject.Parse(jsonResponse);

            //Error is technical validation
            if (validationErrorObject.IsValid(validationErrorSchema))
            {
                var validationError = validationErrorObject.ToObject<IdentifiedSubjectValidationError>();

                var validationErrorDetail = string.Join("; ", validationError?.Detail.ViolatedConstraints?.Select(v => $"Attribute {v.Attribute} (Invalid Value: {v.InvalidValue}) with the error: {v.Message}") ?? Array.Empty<string>());

                throw new CisExternalServiceValidationException($"{validationError?.Message}: {validationErrorDetail}");
            }
        }

        throw new CisExternalServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }

    public async Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"{_httpClient.BaseAddress}{BaseAddress}/{customerId}", request, _serializerOptions, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new CisExternalServiceValidationException($"{error?.Message}: {error?.Detail}"),
                HttpStatusCode.NotFound => new CisNotFoundException(11000, "Customer", customerId),
                _ => new CisExternalServiceValidationException(
                    $"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}")
            };
        }
    }
}