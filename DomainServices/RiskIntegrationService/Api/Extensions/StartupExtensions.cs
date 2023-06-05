using CIS.Infrastructure.StartupExtensions;
using DomainServices.RiskIntegrationService.ExternalServices;
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
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.CisMediatR.GrpcValidationBehavior<,>));

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
        builder.AddExternalService<ExternalServices.CreditWorthiness.V3.ICreditWorthinessClient>();
        builder.AddExternalService<ExternalServices.CustomerExposure.V3.ICustomerExposureClient>();
        builder.AddExternalService<ExternalServices.LoanApplication.V3.ILoanApplicationClient>();
        builder.AddExternalService<ExternalServices.LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient>();
        builder.AddExternalService<ExternalServices.RiskBusinessCase.V3.IRiskBusinessCaseClient>();
        builder.AddExternalService<ExternalServices.RiskCharacteristics.V2.IRiskCharacteristicsClient>();

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxv")!);

        return builder;
    }
}