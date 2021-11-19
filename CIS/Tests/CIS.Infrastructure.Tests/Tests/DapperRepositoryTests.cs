using AutoFixture.Xunit2;
using CIS.Testing.xunit;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CIS.Infrastructure.Tests
{
    [TestCaseOrderer(PriorityOrderer.AttTypeName, PriorityOrderer.AttAssemblyName)]
    public class DapperRepositoryTests
    {
        private readonly DapperRepositorySut _repository;

        public DapperRepositoryTests()
        {
            var connection = new Testing.Database.SqliteConnectionProvider("Data Source=:memory:");
            _repository = new DapperRepositorySut(null, connection);
        }

        [Fact]
        public async Task Execute_ShouldCreateTable()
        {
            // act
            await _repository.Execute();
        }

        [Fact]
        public async Task ExecuteWrongSql_ShouldThrowException()
        {
            // act
            await Assert.ThrowsAsync<Microsoft.Data.Sqlite.SqliteException>(async () => { await _repository.Execute("DELETEx FROM t1"); });
        }

        [Fact]
        public async Task SelectFirst_ShouldReturnSingleValue()
        {
            // act
            int result = await _repository.GetSingleValue(5);

            // assert
            Assert.Equal(5, result);
        }

        [Theory]
        [AutoData]
        public async Task SelectMultiple_ShouldReturnValues(int[] values)
        {
            // act
            var result = await _repository.GetMultipleValues(values);

            // assert
            Assert.Equal(values, result.ToArray());
        }

        [Fact]
        public async Task SelectFirstFromEmptyTable_ShouldThrowException()
        {
            // act, assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.GetSingleValue());
        }
    }
}
