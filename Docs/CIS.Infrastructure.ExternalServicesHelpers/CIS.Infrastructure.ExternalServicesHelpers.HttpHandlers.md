#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')

## CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers Namespace

Custom HttpHandlery k použití v IHttpHandlerFactory pipeline.

| Classes | |
| :--- | :--- |
| [BasicAuthenticationHttpHandler](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.BasicAuthenticationHttpHandler.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.BasicAuthenticationHttpHandler') | Middleware pridavajici Authorization header do requestu. Username a password je zadavan do konstruktoru handleru pri pridavani HttpClienta. |
| [CorrelationIdForwardingHttpHandler](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.CorrelationIdForwardingHttpHandler.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.CorrelationIdForwardingHttpHandler') | Přidá do requestu hlavičku s Correlation Id. |
| [ErrorHandlingHttpHandler](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.ErrorHandlingHttpHandler.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.ErrorHandlingHttpHandler') | Mění výchozí HTTP vyjímky na jejich CIS ekvivalenty. |
| [KbHeadersHttpHandler](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler') | Middleware přidávájící KB hlavičky do requestu. |
| [LoggingHttpHandler](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.LoggingHttpHandler.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.LoggingHttpHandler') | Middleware pro logování payloadu a hlavičke requestu a responsu. |
