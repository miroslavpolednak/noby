namespace DomainServices.CodebookService.Api
{
    internal static class CodebookServiceJsonExtensions
    {
        public static IEndpointRouteBuilder MapCodebookJsonApi(this IEndpointRouteBuilder builder)
            => (new Services.CodebookServiceJson(builder)).Register();
    }
}

namespace DomainServices.CodebookService.Api.Services
{
    public partial class CodebookServiceJson
    {
        private readonly IEndpointRouteBuilder _builder;
        private readonly IMediator _mediatr;

        public CodebookServiceJson(IEndpointRouteBuilder builder)
        {
            _builder = builder;
            _mediatr = _builder.ServiceProvider.GetRequiredService<IMediator>();
        }

        // to be generated
        partial void RegisterInner();

        public IEndpointRouteBuilder Register()
        {
            RegisterInner();
            return _builder;
        }
    }
}
