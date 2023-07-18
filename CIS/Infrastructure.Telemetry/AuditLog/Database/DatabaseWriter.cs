using Dapper;
using Microsoft.Data.SqlClient;

namespace CIS.Infrastructure.Telemetry.AuditLog.Database;

internal sealed class DatabaseWriter
{
    private const string _insertSql = "INSERT INTO dbo.AuditEvent (EventID, AuditEventTypeId, Detail) VALUES (@EventID, @AuditEventTypeId, @Detail)";

    private const string _nextSequenceId = "SELECT NEXT VALUE FOR dbo.NobyLoggerSequence";

    private readonly string _connectionString;

    internal DatabaseWriter(string connectionString)
    {
        _connectionString = connectionString;
    }

    internal void Write(ref AuditEvent eventObject)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Execute(_insertSql, eventObject);
        }
    }

    internal long GetSequenceId()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingle<long>(_nextSequenceId);
        }
    }
}
