# CIS.Infrastructure.gRPC
Knihovna obsahující pomocné funkce pro vytváření gRPC služeb a gRPC klientů. Implementace vychází z **grpc-dotnet** https://github.com/grpc/grpc-dotnet.

## Konfigurace


## Startup extensions

### UseKestrelWithCustomConfiguration()
Umožňuje konfiguraci Kestrelu dedikovaným konfiguračním souborem s více možnostmi než standardní *appsettings.json/Kestrel* konfigurace.  
Konfigurační soubor musí být umístěn spolu s ostatnímy *.json konfiguracemi v rootu aplikace. Výchozí název souboru je **kestrel.json**, nicméně je možné použít custom filename, které se předá jako parametr extension metodě.  
Do konfigurace se automaticky načítá i *.*development.json* verze souboru.

**Příklad použití:**

        using CIS.Infrastructure.gRPC;
        ...
        services.UseKestrelWithCustomConfiguration();
        ...

**Možnosti konfigurace:**  
Plný obsah konfiguračního souboru:

        {
          "CustomKestrel": {
            "Endpoints": [
              {
                "Port": 5005,
                "Protocol": 2
              }
            ],
            "Certificate": {
              "Path": "d:\\Visual Studio Projects\\MPSS-FOMS\\adpra191.vsskb.cz.pfx",
              "Password": "mpss123"
            }
          }
        }

 - **Port** port na kterém Kestrel této instance služby poběží.  
 - **Protocol** HTTP verze endpointu (1 = HTTP1.1; 2 = HTTP2)
 - **Certificate** nastavení cesty a hesla k SSL certifikátů. Může se jednat o plně kvalifikovaný nebo self-signed certifikát.

## Validace request modelu
