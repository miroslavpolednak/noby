using DbUp.Engine;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Database.CodeMigrations;

[DatabaseMigrationsSupport.DbUpScriptName("00004_DDS_script")]
public sealed class DDSInitialTransformation
    : IScript
{
    public string ProvideScript(Func<IDbCommand> dbCommandFactory)
    {
        var worthinessMapper = new Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper();
        
        var cmd = dbCommandFactory();
        int rows = 0;

        using (SqlConnection connection = new SqlConnection(DatabaseMigrationsSupport.Settings.Options.ConnectionString))
        {
            connection.Open();
            
            cmd.CommandText = @"
SELECT OfferId, CreatedUserId, CreatedTime, CreditWorthinessSimpleInputsBin, AdditionalSimulationResultsBin, SimulationInputsBin, SimulationResultsBin, BasicParametersBin 
FROM dbo.Offer
WHERE CreatedUserId IS NOT NULL AND OfferId NOT IN (SELECT DocumentDataEntityId FROM [DDS].[OfferData])";

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
                        var worthinessInput = Contracts.MortgageOfferCreditWorthinessSimpleInputs.Parser.ParseFrom(worthinessBin);
                        var mappedWorthinessData = worthinessMapper.MapToData(worthinessInput, null);
                        insertDDS(connection, "[CreditWorthinessSimpleData]", offerId, createdUserId, createdTime, mappedWorthinessData);
                    }

                    // additional
                    var addBin = getBinary(reader, "AdditionalSimulationResultsBin");
                    if (addBin is not null)
                    {
                        var addResults = Contracts.MortgageOfferAdditionalSimulationResults.Parser.ParseFrom(addBin);
                        insertDDS(connection, "[AdditionalSimulationResultsData]", offerId, createdUserId, createdTime, mapAdditionalData(addResults));
                    }

                    var inputsBin = getBinary(reader, "SimulationInputsBin");
                    var resultsBin = getBinary(reader, "SimulationResultsBin");
                    var paramsBin = getBinary(reader, "BasicParametersBin");
                    if (paramsBin is not null)
                    {
                        var inputsModel = Contracts.MortgageOfferSimulationInputs.Parser.ParseFrom(inputsBin);
                        var resultsModel = Contracts.MortgageOfferSimulationResults.Parser.ParseFrom(resultsBin);
                        var paramsModel = Contracts.MortgageOfferBasicParameters.Parser.ParseFrom(paramsBin);
                        
                        var mappedAddData = mapOfferData(inputsModel, resultsModel, paramsModel);
                        insertDDS(connection, "[OfferData]", offerId, createdUserId, createdTime, mappedAddData);
                    }

                    rows++;
                }
            }
        }

        Console.WriteLine($"00003_DDSInitialTransformation: Processed {rows} rows");

        return string.Empty;
    }

    private static Database.DocumentDataEntities.MortgageOfferData mapOfferData(Contracts.MortgageOfferSimulationInputs inputs, Contracts.MortgageOfferSimulationResults simulationOutputs, Contracts.MortgageOfferBasicParameters basic)
    {
        var mapper = new Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper();

        return new Database.DocumentDataEntities.MortgageOfferData
        {
            SimulationInputs = mapper.MapToDataInputs(inputs),
            BasicParameters = mapper.MapToDataBasicParameters(basic),
            SimulationOutputs = new DocumentDataEntities.MortgageOfferData.SimulationOutputsData
            {
                ContractSignedDate = simulationOutputs.ContractSignedDate,
                AnnuityPaymentsCount = simulationOutputs.AnnuityPaymentsCount,
                AnnuityPaymentsDateFrom = simulationOutputs.AnnuityPaymentsDateFrom,
                Aprc = simulationOutputs.Aprc,
                DrawingDateTo = simulationOutputs.DrawingDateTo,
                EmployeeBonusDeviation = simulationOutputs.EmployeeBonusDeviation,
                EmployeeBonusLoanCode = simulationOutputs.EmployeeBonusLoanCode,
                LoanAmount = simulationOutputs.LoanAmount,
                LoanDueDate = simulationOutputs.LoanDueDate,
                LoanInterestRate = simulationOutputs.LoanInterestRate,
                LoanInterestRateAnnounced = simulationOutputs.LoanInterestRateAnnounced,
                LoanInterestRateAnnouncedType = simulationOutputs.LoanInterestRateAnnouncedType,
                LoanInterestRateProvided = simulationOutputs.LoanInterestRateProvided,
                LoanPaymentAmount = simulationOutputs.LoanPaymentAmount,
                LoanToValue = simulationOutputs.LoanToValue,
                LoanTotalAmount = simulationOutputs.LoanTotalAmount,
                MarketingActionsDeviation = simulationOutputs.MarketingActionsDeviation,
                LoanDuration = simulationOutputs.LoanDuration,
                Warnings = simulationOutputs
                    .Warnings?
                    .Select(t => new DocumentDataEntities.MortgageOfferData.SimulationResultWarningData
                    {
                        WarningCode = t.WarningCode,
                        WarningInternalMessage = t.WarningInternalMessage,
                        WarningText = t.WarningText
                    })
                    .ToList()
    }
        };
    }

    private static Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData mapAdditionalData(Contracts.MortgageOfferAdditionalSimulationResults data)
    {
        return new Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData
        {
            Fees = data
                .Fees?
                .Select(t => new Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData.FeeData
                {
                    FeeId = t.FeeId,
                    DiscountPercentage = t.DiscountPercentage,
                    TariffSum = t.TariffSum,
                    ComposedSum = t.ComposedSum,
                    FinalSum = t.FinalSum,
                    MarketingActionId = t.MarketingActionId,
                    Name = t.Name,
                    ShortNameForExample = t.ShortNameForExample,
                    TariffName = t.TariffName,
                    UsageText = t.UsageText,
                    TariffTextWithAmount = t.TariffTextWithAmount,
                    CodeKB = t.CodeKB,
                    DisplayAsFreeOfCharge = t.DisplayAsFreeOfCharge,
                    IncludeInRPSN = t.IncludeInRPSN,
                    Periodicity = t.Periodicity,
                    AccountDateFrom = t.AccountDateFrom
                })
                .ToList(),
            MarketingActions = data
                .MarketingActions?
                .Select(t => new Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData.MarketingActionData
                {
                    Code = t.Code,
                    Requested = t.Requested,
                    Applied = t.Applied,
                    MarketingActionId = t.MarketingActionId,
                    Deviation = t.Deviation,
                    Name = t.Name
                })
                .ToList(),
            PaymentScheduleSimple = data
                .PaymentScheduleSimple?
                .Select(t => new Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData.PaymentScheduleData
                {
                    PaymentIndex = t.PaymentIndex,
                    PaymentNumber = t.PaymentNumber,
                    Date = t.Date,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .ToList()
        };
    }

    private static void insertDDS(SqlConnection connection, in string tableName, in int offerId, in int createdUserId, in DateTime createdTime, in object data)
    {
        using (SqlCommand command = new SqlCommand($"INSERT INTO [DDS].{tableName} ([DocumentDataEntityId],[DocumentDataVersion],[Data],[CreatedUserId],[CreatedTime]) VALUES (@id, 1, @data, @userId, @time)", connection))
        {
            command.CommandTimeout = 5;
            
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

    private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IgnoreReadOnlyProperties = true
    };
}
