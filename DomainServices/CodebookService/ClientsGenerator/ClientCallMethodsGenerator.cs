using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace DomainServices.CodebookService.ClientsGenerators
{
    /// <summary>
    /// TODO predelat na IncrementalGenerator
    /// </summary>
    [Generator]
    public sealed class ServiceClientsSourceGenerator : ISourceGenerator
    {
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
            var serviceClass = assemblySymbol.GetTypeByMetadataName("DomainServices.CodebookService.Contracts.v1.CodebookService");
            
            var endpoints = serviceClass!
                .GetTypeMembers()
                .First(t => t.Name == "CodebookServiceClient")
                .GetMembers()
                .Where(t => t is IMethodSymbol)
                .Cast<IMethodSymbol>()
                .Where(t => t.MethodKind == MethodKind.Ordinary 
                    && t.IsVirtual 
                    && t.ReturnType.Name != "AsyncUnaryCall" 
                    && t.Parameters[0].Type.Name == "Empty"
                    && t.Parameters.Length == 2)
                .Select(t =>
                {
                    var symbol = t.ReturnType.GetMembers("Items").First() as IPropertySymbol;
                    string symbolName = symbol!.Type.ToString();
                    var i = symbolName.IndexOf('<');
                    var name = symbolName.Substring(i + 1, symbolName.Length - i - 2);

                    return new Endpoint(t.OriginalDefinition.Name, name);
                })
                .ToList();

            var sbImpl = new StringBuilder();
            sbImpl.AppendLine("namespace DomainServices.CodebookService.Clients.Services;");
            sbImpl.AppendLine("internal partial class CodebookService {");

            var sbInterface = new StringBuilder();
            sbInterface.AppendLine("using System.Collections.Generic;");
            sbInterface.AppendLine("using System.Threading.Tasks;");
            sbInterface.AppendLine("using System.Threading;");
            sbInterface.AppendLine("namespace DomainServices.CodebookService.Clients;");
            sbInterface.AppendLine("public partial interface ICodebookServiceClient {");

            endpoints.ForEach(m =>
            {
                // implementation
                sbImpl.Append($"public async Task<List<{m.ReturnType}>> {m.MethodName}(CancellationToken cancellationToken = default(CancellationToken))");
                sbImpl.AppendLine($"  => await _cache.GetOrCreate(async () => (await _service.{m.MethodName}Async(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: cancellationToken)).Items.ToList());");

                // interface
                sbInterface.AppendLine($"Task<List<{m.ReturnType}>> {m.MethodName}(CancellationToken cancellationToken = default(CancellationToken));");
            });

            sbImpl.Append("}");
            sbInterface.Append("}");

            // generate source using targets ...
            context.AddSource("ICodebookServiceClients_generated.cs", sbInterface.ToString());
            context.AddSource("CodebookService_generated.cs", sbImpl.ToString());

            File.WriteAllText("c:/temp3/int.cs", sbInterface.ToString());
            File.WriteAllText("c:/temp3/impl.cs", sbImpl.ToString());
        }
    }

    internal class Endpoint
    {
        public Endpoint(string methodName, string returnType)
        {
            MethodName = methodName;
            ReturnType = returnType;
        }

        public string MethodName { get; set; }
        public string ReturnType { get; set; }
    }
}
