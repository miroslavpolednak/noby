﻿using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using Microsoft.Extensions.DependencyInjection;
using DomainServices;

namespace CIS.InternalServices.DocumentDataAggregator;

public static class StartupExtensions
{
    public static IServiceCollection AddDataAggregator(this IServiceCollection services)
    {
        services.AddDbContext<ConfigurationContext>(ServiceLifetime.Transient);

        services.AddTransient<IDataAggregator, DataAggregator>();

        services.AddAttributedServices(typeof(StartupExtensions));

        services.AddDomainServices();

        return services;
    }

    private static void AddDomainServices(this IServiceCollection services) =>
        services.AddCodebookService()
                .AddSalesArrangementService()
                .AddCaseService()
                .AddOfferService()
                .AddUserService()
                .AddCustomerService()
                .AddProductService();
}