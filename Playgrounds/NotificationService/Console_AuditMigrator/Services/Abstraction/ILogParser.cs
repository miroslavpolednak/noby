using Console_AuditMigrator.Models;

namespace Console_AuditMigrator.Services.Abstraction;

public interface ILogParser
{
    Task<IList<ApplicationLog>> ParseFile(string fileName);
}