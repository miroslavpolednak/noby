using DbUp.Engine;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Database.CodeMigrations;

[DisplayName("0-DDSInitialTransformation")]
public sealed class DDSInitialTransformation
    : IScript
{
    public string ProvideScript(Func<IDbCommand> dbCommandFactory)
    {
        var worthinessMapper = new Database.DocumentDataEntities.Mappers.CreditWorthinessSimpleDataMapper();
        
        var cmd = dbCommandFactory();
        
        using (SqlConnection connection = new SqlConnection(cmd.Connection!.ConnectionString))
        {
            connection.Open();

            cmd.CommandText = "SELECT OfferId, CreatedUserId, CreatedTime, CreditWorthinessSimpleInputsBin, AdditionalSimulationResultsBin FROM dbo.Offer WHERE CreatedUserId IS NOT NULL AND SimulationInputsBin IS NOT NULL";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var offerId = reader.GetInt32(reader.GetOrdinal("OfferId"));
                    var createdUserId = reader.GetInt32(reader.GetOrdinal("CreatedUserId"));
                    var createdTime = reader.GetDateTime(reader.GetOrdinal("CreatedTime"));

                    // bonita
                    var worthinessBin = getBinary(reader, "CreditWorthinessSimpleInputsBin");
                    if (worthinessBin is not null)
                    {
                        var worthinessInput = Contracts.MortgageCreditWorthinessSimpleInputs.Parser.ParseFrom(worthinessBin);
                        var mappedWorthinessData = worthinessMapper.MapToData(worthinessInput, null);
                        insertDDS(connection, "[CreditWorthinessSimpleData]", offerId, createdUserId, createdTime, mappedWorthinessData);
                    }

                    // additional
                    /*var addBin = getBinary(reader, "AdditionalSimulationResultsBin");
                    if (addBin is not null)
                    {
                        var addResults = Contracts.AdditionalMortgageSimulationResults.Parser.ParseFrom(addBin);
                        var mappedAddData = new Database.DocumentDataEntities.AdditionalSimulationResultsData();
                        insertDDS(connection, "[AdditionalSimulationResultsData]", offerId, createdUserId, createdTime, mappedAddData);
                    }*/

                    // final stamp
                    //updateRow(connection, offerId);
                }
            }
        }

        return string.Empty;
    }

    private static void insertDDS(SqlConnection connection, in string tableName, in int offerId, in int createdUserId, in DateTime createdTime, in object data)
    {
        using (SqlCommand command = new SqlCommand($"INSERT INTO [DDS].{tableName} ([DocumentDataEntityId],[DocumentDataVersion],[Data],[CreatedUserId],[CreatedTime]) VALUES (@id, 1, @data, @userId, @time)", connection))
        {
            command.Parameters.AddWithValue("@id", offerId);
            command.Parameters.AddWithValue("@data", JsonSerializer.Serialize(data, _jsonSerializerOptions));
            command.Parameters.AddWithValue("@userId", createdUserId);
            command.Parameters.AddWithValue("@time", createdTime);

            command.ExecuteNonQuery();
        }
    }

    private static byte[]? getBinary(IDataReader reader, in string field)
    {
        int fieldIndex = reader.GetOrdinal(field);
        if (reader.IsDBNull(fieldIndex))
            return null;

        long bufferSize = reader.GetBytes(fieldIndex, 0, null, 0, 0); // Get the size of the binary data
        byte[] buffer = new byte[bufferSize]; // Create a byte array to hold the binary data
        reader.GetBytes(fieldIndex, 0, buffer, 0, (int)bufferSize); // Read binary data into the byte array
        return buffer;
    }

    private static void updateRow(SqlConnection connection, in int offerId)
    {
        using (SqlCommand command = new SqlCommand("UPDATE dbo.Offer SET CreditWorthinessSimpleInputs=NULL WHERE OfferId=@id", connection))
        {
            command.Parameters.AddWithValue("@id", offerId);

            command.ExecuteNonQuery();
        }
    }

    private static JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Default;
}
