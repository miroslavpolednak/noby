using Microsoft.Extensions.DependencyInjection;
using System;

namespace FOMS.DocumentProcessing;

public static class StartupExtensions
{
    public static IServiceCollection AddDocumentProcessing(this IServiceCollection service)
    {
        return service;
    }
}
