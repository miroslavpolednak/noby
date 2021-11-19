using Microsoft.Extensions.Logging;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CIS.Infrastructure.Tests
{
    public class DapperRepositorySut : Data.DapperBaseRepository<DapperRepositorySut>
    {
        private const string _createTable = "CREATE TABLE t1 (id int)";
        private const string _insert = "INSERT INTO t1 (id) VALUES (@id)";

        public DapperRepositorySut(ILogger<DapperRepositorySut> logger, CIS.Core.Data.IConnectionProvider connectionProvider)
            : base(logger, connectionProvider) { }

        public async Task Execute(string sql = _createTable)
        {
            await WithConnection(async t =>
            {
                await t.ExecuteAsync(sql);
            });
        }

        public async Task<int> GetSingleValue(int? value = null)
        {
            return await WithConnection<int>(async t =>
            {
                await t.ExecuteAsync(_createTable);
                if (value.HasValue)
                    await t.ExecuteAsync(_insert, new { id = value.Value });
                return await t.QueryFirstAsync<int>("SELECT id FROM t1");
            });
        }

        public async Task<List<int>> GetMultipleValues(int[] values)
        {
            return await WithConnection<List<int>>(async t =>
            {
                await t.ExecuteAsync(_createTable);
                foreach (int id in values)
                    await t.ExecuteAsync(_insert, new { id = id });
                return (await t.QueryAsync<int>("SELECT id FROM t1")).AsList();
            });
        }
    }
}
