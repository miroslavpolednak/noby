using CIS.Testing.Database;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Testing;

public class TestFixture<TStartup> : IDisposable where TStartup : class
{
    // fixture options - Configure...() se vola pro kazdy test v ramci jedne tridy, proto se pri prvnim volani ulozi options sem a pak uz se znova neassignuji.
    private Action<WebHostBuilderContext, IConfigurationBuilder>? _configureAppConfiguration = null;
    private Action<IServiceCollection>? _configureTestServices = null;

    // instance app factory - bude jedna pro celou kazdou fixture
    private WebApplicationFactory<TStartup>? _webApplicationFactory;

    // instance http clienta - bude jedna pro celou fixture
    private HttpClient? _httpClient;
    private GrpcChannel? _grpcChannel;

    private string? _testName = null;
    private bool _initIsCalled = false;

    // instance nastaveni databaze - pres konstruktor je mozne predat custom nastaveni v ConfigureTestOptions()
    public IDatabaseFixture? DatabaseFixture { get; private set; }

    public TestFixture()
    {
        if (!GlobalTestsSettings.IsInitialized)
            throw new Exception("GlobalTestSettings has not been initialized. Run CIS.Testing.GlobalTestsSettings.Init()");
    }

    /// <summary>
    /// Normalne se webappfactory, databaze, httpClient atd. vytvori jen jednou pro vsechny testy v ramci jende IClassFixture. Zavolani teto metody zajisti vytvoreni nove infrastrukturu pro kazdy test.
    /// </summary>
    /// <returns></returns>
    public TestFixture<TStartup> Recreate()
    {
        return new TestFixture<TStartup>();
    }

    /// <summary>
    /// Inicializace test tridy. Je nutna pro spravnou funkcnost DatabaseFixture, jinak nemusi byt volana.
    /// </summary>
    /// <param name="currentClass">this (instance aktualne poustene test tridy)</param>
    public TestFixture<TStartup> Init(object currentClass)
    {
        var testFullName = currentClass.ToString() ?? "";
        if (testFullName.StartsWith(GlobalTestsSettings.BaseNamespace))
            _testName = testFullName.Substring(GlobalTestsSettings.BaseNamespace.Length + 1, testFullName.Length - GlobalTestsSettings.BaseNamespace.Length - GlobalTestsSettings.TestsClassNameSuffix.Length - 1);
        _initIsCalled = true;

        return this;
    }

    /// <summary>
    /// Konfigurace WebHosta - pridani dalsich konfiguracnich souboru atd.
    /// </summary>
    public TestFixture<TStartup> ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> builder)
    {
        _configureAppConfiguration = builder;
        return this;
    }

    /// <summary>
    /// Konfigurace DI - pridani dalsich services krome tech, ktere jsou v testovanem projektu
    /// </summary>
    public TestFixture<TStartup> ConfigureTestServices(Action<IServiceCollection> services)
    {
        _configureTestServices = services;
        return this;
    }

    /// <summary>
    /// Inicializace Sqlite databaze. Pokud neni tato metoda zavolana, databaze se vubec neicializuje.
    /// </summary>
    public TestFixture<TStartup> ConfigureTestDatabase(Action<DatabaseFixtureOptions>? options = null)
    {
        if (!_initIsCalled)
            throw new Exception("Init() has not been called prior to ConfigureTestDatabase()");
        if (DatabaseFixture is null)
        {
            var defaultOptions = new DatabaseFixtureOptions
            {
                SeedPaths = Path.Combine(getBaseDirectory(), GlobalTestsSettings.TestsFolderName, _testName ?? "", GlobalTestsSettings.DatabaseSeedScriptName)
            };
            if (options is not null)
                options.Invoke(defaultOptions);

            DatabaseFixture = new SqliteDatabaseFixture(defaultOptions);
        }

        return this;
    }

    // instance web app factory. Mela by se vytvorit vzdy jedna pro test tridu
    public WebApplicationFactory<TStartup> WebApplicationFactory
    {
        get
        {
            if (_webApplicationFactory is null)
            {
                _webApplicationFactory = configureWebApplicationFactory();
            }
            return _webApplicationFactory;
        }
    }
        
    /// <summary>
    /// Vytvoreni HttpClienta pro gRPC sluzby
    /// </summary>
    public HttpClient GrpcClient
    {
        get
        {
            if (_httpClient is null)
                _httpClient = WebApplicationFactory.CreateDefaultClient(new ResponseVersionHandler());
            return _httpClient;
        }
    }

    public GrpcChannel GetGrpcChannel()
        => _grpcChannel ?? GrpcChannel.ForAddress(GrpcClient.BaseAddress ?? throw new ArgumentNullException(), new GrpcChannelOptions
            {
                HttpClient = GrpcClient
            });

    /// <summary>
    /// Vraci instanci gRPC sluzby. Interne zavola GrpcClient(), vytvori channel.
    /// </summary>
    /*public TService CreateGrpcService<TService>() where TService : class
    {
        if (_grpcChannel is null)
        {
            _grpcChannel = GrpcChannel.ForAddress(GrpcClient.BaseAddress ?? throw new ArgumentNullException(), new GrpcChannelOptions
            {
                HttpClient = GrpcClient
            });
        }

        return _grpcChannel.CreateGrpcService<TService>();
    }*/

    /// <summary>
    /// Vraci instanci tridy z DI kontajneru
    /// </summary>
    public TService? GetService<TService>()
        => WebApplicationFactory.Services.GetService<TService>();

    public void Dispose()
    {
        if (this.DatabaseFixture is not null) this.DatabaseFixture.Dispose();
        if (this.WebApplicationFactory is not null) this.WebApplicationFactory.Dispose();
        GC.SuppressFinalize(this);
    }
        
    private WebApplicationFactory<TStartup> configureWebApplicationFactory()
    {
        return new TestWebApplicationFactory<TStartup>().WithWebHostBuilder(builder =>
        {
            builder.UseSolutionRelativeContentRoot(GlobalTestsSettings.SolutionRelativeContentRoot);

            // add configuration to web host
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                // pridat konfiguraci per test class
                if (!string.IsNullOrEmpty(_testName))
                {
                    string configPath = Path.Combine(getBaseDirectory(), GlobalTestsSettings.TestsFolderName, _testName, "configuration.json");
                    builder.AddJsonFile(configPath, true);
                }

                // custom konfigurace
                _configureAppConfiguration?.Invoke(context, builder);
            });

            // After TStartup ConfigureServices.
            builder.ConfigureTestServices(services =>
            {
                // pridat Sqlite connection string provider
                if (DatabaseFixture is not null)
                    services.AddSingleton(DatabaseFixture.Provider);

                // custom konfigurace
                _configureTestServices?.Invoke(services);
            });
        });
    }

    private static string getBaseDirectory()
        => Directory.GetCurrentDirectory();

    private sealed class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.Version = request.Version;
            return response;
        }
    }
}
