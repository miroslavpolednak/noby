using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Nastavení Kestrel serveru pro gRPC služby.
/// </summary>
public static class GrpcStartupKestrelExtensions
{
    /// <summary>
    /// Umozni nasatavit kestrel custom konfiguracnim souborem.
    /// Vychozi nazev pro konfiguracni soubor je "kestrel.json". Soubor musi obsahovat root node "CustomeKestrel", pod kterym je struktura CIS.Core.Configuration.KestrelConfiguration.
    /// </summary>
    public static WebApplicationBuilder UseKestrelWithCustomConfiguration(this WebApplicationBuilder builder, string configurationFilename = "kestrel.json")
    {
        string devFilename = Path.GetFileNameWithoutExtension(configurationFilename) + ".development.json";

        Core.Configuration.KestrelConfiguration kestrelConfiguration = new();
        builder.Configuration.AddJsonFile(configurationFilename, false);
        builder.Configuration.AddJsonFile(devFilename, true);
        builder.Configuration.GetSection("CustomKestrel").Bind(kestrelConfiguration);

        if (kestrelConfiguration?.Endpoints == null)
            throw new ArgumentNullException($"Kestrel configuration file ({configurationFilename}) not found or configuration is not valid (does not contain root element CustomKestrel?)");

        builder.WebHost.UseKestrel(serverOptions =>
        {
            serverOptions.ConfigureEndpointDefaults(opts =>
            {
                // pouzit k vytvoreni SSL certifikat
                if (kestrelConfiguration.Certificate != null)
                {
                    switch (kestrelConfiguration.Certificate.Location)
                    {
                        // ulozen na filesystemu
                        case Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.FileSystem:
                            opts.UseHttps(kestrelConfiguration.Certificate?.Path ?? throw new Core.Exceptions.CisConfigurationNotFound("CustomKestrel.Certificate.Path"), kestrelConfiguration.Certificate.Password);
                            break;

                        // ulozen v certstore
                        case Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.CertStore:
                            using (var store = new X509Store(kestrelConfiguration.Certificate.CertStoreName, kestrelConfiguration.Certificate.CertStoreLocation, OpenFlags.ReadOnly))
                            {
                                var cert = store.Certificates
                                    .FirstOrDefault(x => x.Thumbprint.Equals(kestrelConfiguration.Certificate.Thumbprint, StringComparison.OrdinalIgnoreCase))
                                    ?? throw new Core.Exceptions.CisConfigurationException(0, $"Kestrel certificate '{kestrelConfiguration.Certificate.Thumbprint}' not found in '{kestrelConfiguration.Certificate.CertStoreName}' / 'kestrelConfiguration.Certificate.CertStoreLocation'");
                                opts.UseHttps(cert);
                            }

                            break;
                    }
                }
            });

            // pridat endpointy kde kestrel bude poslouchat
            kestrelConfiguration.Endpoints?.ForEach(endpoint =>
            {
                serverOptions.ListenAnyIP(endpoint.Port, listenOptions =>
                {
                    listenOptions.Protocols = (Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols)endpoint.Protocol;
                });
            });
        });

        return builder;
    }
}
