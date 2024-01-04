using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using DbUp.Engine;
using DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Database.CodeMigrations;

[DatabaseMigrationsSupport.DbUpScriptName("00021_DDS_script")]
public sealed class DDSInitialTransformation : IScript
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public string ProvideScript(Func<IDbCommand> dbCommandFactory)
    {
        const int BatchSize = 300;

        var cmd = dbCommandFactory();

        var index = 0;

        while (true)
        {
            var readSqlCmd = $"""
                              SELECT SalesArrangementId, ParametersBin, SalesArrangementParametersType, CreatedUserId, CreatedTime, ModifiedUserId
                              FROM dbo.SalesArrangementParameters
                              ORDER BY SalesArrangementParametersId
                              OFFSET {index * BatchSize} ROWS
                              FETCH NEXT {BatchSize} ROWS ONLY;
                              """;

            var convertedRecords = new List<SalesArrangementParameters>(BatchSize);

            cmd.CommandText = readSqlCmd;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var record = new SalesArrangementParameters
                    {
                        SalesArrangementId = reader.GetInt32(reader.GetOrdinal("SalesArrangementId")),
                        SalesArrangementParametersType = reader.GetByte(reader.GetOrdinal("SalesArrangementParametersType")),
                        CreatedUserId = reader.GetInt32(reader.GetOrdinal("CreatedUserId")),
                        CreatedTime = reader.GetDateTime(reader.GetOrdinal("CreatedTime")),
                        ModifiedUserId = reader.GetInt32(reader.GetOrdinal("ModifiedUserId"))
                    };

                    record.ParametersObj = GetParametersObj(reader, "ParametersBin", (SalesArrangementTypes)record.SalesArrangementParametersType);

                    convertedRecords.Add(record);
                }
            }


            if (!convertedRecords.Any())
                break;

            var insertSqlCommands = convertedRecords
                .Select(r => $"""
                              INSERT INTO [DDS].[SalesArrangementParameters] ([DocumentDataEntityId],[DocumentDataVersion],[Data],[CreatedUserId],[CreatedTime],[ModifiedUserId])
                              VALUES ({r.SalesArrangementId}, 1, '{JsonSerializer.Serialize(r.ParametersObj, _jsonSerializerOptions)}', {r.CreatedUserId}, '{r.CreatedTime}', {r.ModifiedUserId});
                              """);

            cmd.CommandText = string.Join(Environment.NewLine, insertSqlCommands);
            cmd.ExecuteNonQuery();

            index++;
        }

        return string.Empty;
    }

    private static object GetParametersObj(IDataReader reader, string columnName, SalesArrangementTypes salesArrangementType)
    {
        var fieldIndex = reader.GetOrdinal(columnName);

        if (reader.IsDBNull(fieldIndex))
            return new object();

        var bufferSize = reader.GetBytes(fieldIndex, 0, null, 0, 0);
        var buffer = new byte[bufferSize];
        reader.GetBytes(fieldIndex, 0, buffer, 0, (int)bufferSize);

        return salesArrangementType switch
        {
            SalesArrangementTypes.Mortgage => SalesArrangementParametersMortgage.Parser.ParseFrom(buffer).MapMortgage(),
            SalesArrangementTypes.Drawing => SalesArrangementParametersDrawing.Parser.ParseFrom(buffer).MapDrawing(),
            SalesArrangementTypes.GeneralChange => SalesArrangementParametersGeneralChange.Parser.ParseFrom(buffer).MapGeneralChange(),
            SalesArrangementTypes.HUBN => SalesArrangementParametersHUBN.Parser.ParseFrom(buffer).MapHUBN(),
            SalesArrangementTypes.CustomerChange => SalesArrangementParametersCustomerChange.Parser.ParseFrom(buffer).MapCustomerChange(),
            SalesArrangementTypes.CustomerChange3602A => SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(buffer).MapCustomerChange3602(),
            SalesArrangementTypes.CustomerChange3602B => SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(buffer).MapCustomerChange3602(),
            SalesArrangementTypes.CustomerChange3602C => SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(buffer).MapCustomerChange3602(),
            _ => throw new InvalidOperationException($"Cannot convert sales arrangement type {salesArrangementType}")
        };
    }

    public class SalesArrangementParameters
    {
        public int SalesArrangementId { get; set; }

        public byte SalesArrangementParametersType { get; set; }

        public object ParametersObj { get; set; } = null!;

        public int CreatedUserId { get; set; }

        public DateTime CreatedTime { get; set; }

        public int ModifiedUserId { get; set; }
    }
}