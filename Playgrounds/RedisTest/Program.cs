using Microsoft.Extensions.DependencyInjection;
using CIS.Infrastructure.Caching;

Console.WriteLine("Setting up DI...");

//setup our DI
var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "redistest"
    })
    .AddRedisGlobalCache(opt =>
    {
        opt.ConnectionString = args.Length == 0 ? "vncub6115.os.kb.cz:6379,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass" : args[1];
    })
    .BuildServiceProvider();

Console.WriteLine("Get cache from DI...");
var cache = serviceProvider.GetRequiredService<IGlobalCache>();

Console.WriteLine("Set value...");
cache.Set(args[0] + "test1", "moje_hodnota");

var value = cache.GetString(args.Length == 0 ? "MPSS:FAT:" : args[0] + "test1");
Console.WriteLine($"Retrieved value: {value}");
