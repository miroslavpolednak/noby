# Popis startupu aplikace gRPC služby


```
AppConfiguration appConfiguration = new();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
builder.Services.AddSingleton(appConfiguration);
```

Umožní registraci služeb do DI pomocí atributů z CIS.Core.Attributes.
```
builder.Services.AddAttributedServices(typeof(Program));
```

builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisHealthChecks();
