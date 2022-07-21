using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DomainServices.CodebookService.ApiGenerators
{
    [Generator]
    public class EndpointsSourceGenerator : ISourceGenerator
    {
        private static string[] _hardcodedCodebooks = new[] { "DeveloperSearch", "Reset" };
        private static readonly Regex _castCamelCaseToDashDelimitedRegex = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);

        public void Initialize(GeneratorInitializationContext context)
        {
            /*if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }*/
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // finding Contracts reference assembly Symbols
            IAssemblySymbol assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols.First(q => q.Name == "DomainServices.CodebookService.Contracts");
            var serviceInterface = assemblySymbol.GetTypeByMetadataName("DomainServices.CodebookService.Contracts.ICodebookService");
            // get all methods in interface
            var endpoints = serviceInterface.GetMembers()
                .Where(t => t is IMethodSymbol)
                .Cast<IMethodSymbol>()
                .Where(t => !_hardcodedCodebooks.Contains(t.OriginalDefinition.Name))
                .Select(t => new Endpoint(t.OriginalDefinition.Name, t.OriginalDefinition.ReturnType.ToString(), t.OriginalDefinition.Parameters.Count() == 2 ? t.OriginalDefinition.Parameters[0].ToString() : ""))
                .ToList();

            writeGRPC(endpoints, context);
            writeREST(endpoints, context);
        }

        private void writeGRPC(List<Endpoint> endpoints, GeneratorExecutionContext context)
        {
            var sbGrpc = new StringBuilder(@"
using ProtoBuf.Grpc;
namespace DomainServices.CodebookService.Api.Services;
public partial class CodebookService : DomainServices.CodebookService.Contracts.ICodebookService
{
");
            endpoints.ForEach(t => sbGrpc.AppendLine($"public {t.ReturnType} {t.MethodName}({t.RequestDtoType} request, CallContext context = default) => _mediator.Send(request, context.CancellationToken);"));
            sbGrpc.Append("}");

            context.AddSource("CodebookService_Generated.cs", sbGrpc.ToString());
        }

        private void writeREST(List<Endpoint> endpoints, GeneratorExecutionContext context)
        {
            var sbRest = new StringBuilder(@"
namespace DomainServices.CodebookService.Api.Services;
public partial class CodebookServiceJson
{
partial void RegisterInner()
{
");
            endpoints.ForEach(t =>
            {
                string name = _castCamelCaseToDashDelimitedRegex.Replace(t.MethodName, "-$1");
                int idx = t.ReturnType.IndexOf("<");
                string ret = t.ReturnType.Substring(idx + 1, t.ReturnType.Length - idx - 2);
                sbRest.AppendLine($"_builder.MapGet(\"/codebooks/{name.ToLower()}\", async (CancellationToken cancellationToken) => await _mediatr.Send(new {t.RequestDtoType}(), cancellationToken)).Produces<{ret}>(StatusCodes.Status200OK).RequireAuthorization();");
            });
            sbRest.Append("}}");
            
            context.AddSource("CodebookServiceJson_Generated.cs", sbRest.ToString());
        }
    }
}
