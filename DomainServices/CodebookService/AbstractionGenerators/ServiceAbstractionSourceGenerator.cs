using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DomainServices.CodebookService.AbstractionGenerators
{
    [Generator]
    public class ServiceAbstractionSourceGenerator : ISourceGenerator
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
            var serviceInterface = assemblySymbol.GetTypeByMetadataName("DomainServices.CodebookService.Contracts.ICodebookService");
            // get all methods in interface
            var endpoints = serviceInterface.GetMembers()
                .Where(t => t is IMethodSymbol)
                .Cast<IMethodSymbol>()
                .Select(t => new Endpoint(t.OriginalDefinition.Name, t.OriginalDefinition.ReturnType.ToString(), t.OriginalDefinition.Parameters.Count() == 2 ? t.OriginalDefinition.Parameters[0].ToString() : ""))
                .ToList();

            var sbImpl = new StringBuilder();
            sbImpl.AppendLine("namespace DomainServices.CodebookService.Abstraction;");
            sbImpl.AppendLine("internal partial class CodebookService {");

            var sbInterface = new StringBuilder();
            sbInterface.AppendLine("namespace DomainServices.CodebookService.Abstraction;");
            sbInterface.AppendLine("public partial interface ICodebookServiceAbstraction {");

            endpoints.ForEach(m =>
            {
                // implementation
                sbImpl.AppendLine($"public async {m.ReturnType} {m.MethodName}(CancellationToken cancellationToken = default(CancellationToken))");
                sbImpl.AppendLine($"=> await _cache.GetOrCreate(\"{m.MethodName}\", async () => await _codebookService.{m.MethodName}(new {m.RequestDtoType}(), cancellationToken));");

                // interface
                sbInterface.AppendLine($"{m.ReturnType} {m.MethodName}(CancellationToken cancellationToken = default(CancellationToken));");
            });

            sbImpl.Append("}");
            sbInterface.Append("}");

            // generate source using targets ...
            context.AddSource("ICodebookServiceAbstraction_generated.cs", sbInterface.ToString());
            context.AddSource("CodebookService_generated.cs", sbImpl.ToString());
        }
    }
}
