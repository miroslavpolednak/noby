using Dapper;
using Microsoft.Data.SqlClient;

namespace SharedAudit.Database;

internal sealed class DatabaseWriter
{
    private const string _insertSql = "INSERT INTO dbo.AuditEvent (EventID, AuditEventTypeId, Detail) VALUES (@EventID, @AuditEventTypeId, @Detail)";

    private const string _nextSequenceId = "SELECT NEXT VALUE FOR dbo.NobyLoggerSequence";

    private readonly string _connectionString;

    internal DatabaseWriter(string connectionString)
    {
        _connectionString = connectionString;
    }

    internal bool Write(ref AuditEvent eventObject)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(_insertSql, eventObject);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal long? GetSequenceId()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<long>(_nextSequenceId);
            }
        }
        catch
        {
            return null;
        }
    }
}
