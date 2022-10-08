using CIS.Infrastructure.StartupExtensions;
using DomainServices.RiskIntegrationService.Api.Clients;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DomainServices.RiskIntegrationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRipService(this WebApplicationBuilder builder)
    {
        // disable default model state validations
        builder.Services.AddSingleton<IObjectModelValidator, CIS.Infrastructure.WebApi.Validation.NullObjectModelValidator>();

        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        // MVC
        builder.Services.AddControllers();

        // register c4m clients
        builder.AddRiskBusinessCaseClient();
        builder.AddCreditWorthinessClient();
        builder.AddCustomersExposureClient();
        builder.AddLoanApplicationClient();
        builder.AddLoanApplicationAssessmentClient();
        builder.AddRiskCharakteristicsClient();

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxv"));

        return builder;
    }
}