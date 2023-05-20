using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainServices.CodebookService.ClientsGenerator;

[Generator]
public class ClientCallMethodsGenerator
    : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        throw new Exception("Test exception!");
    }

}