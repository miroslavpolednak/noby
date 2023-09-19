Document.AddLicense("DPS10NEDLDCHHBkifnavglbvMUnz6cOsK3rihyH8moPETXqm86GidIy9yKvju+7UztxVoPJRLgKM5MmmDgsKwmDSRjs5hznpB2Lw");

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<CIS.InternalServices.DocumentGeneratorService.Api.Configuration.AppConfiguration>()
    .AddCustomServices((builder, appConfiguration) =>
    {
        CIS.InternalServices.DocumentGeneratorService.Api.GeneratorVariables.Init(appConfiguration);
    })
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<CIS.InternalServices.DocumentGeneratorService.Api.Services.DocumentGeneratorService>();
    })
    .Run();
