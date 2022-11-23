﻿using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DomainServices.DocumentArchiveService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddDocumentArchiveService(this WebApplicationBuilder builder)
    {
        // disable default model state validations
        builder.Services.AddSingleton<IObjectModelValidator, CIS.Infrastructure.WebApi.Validation.NullObjectModelValidator>();

        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        // MVC
        builder.Services.AddControllers();

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("default"));

        return builder;
    }
}