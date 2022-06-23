using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Google.Protobuf;
using System.Text.Json.Serialization;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateIncomeHandler
    : IRequestHandler<Dto.CreateIncomeMediatrRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(Dto.CreateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.Request.CustomerOnSAId);

        var entity = new Repositories.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            Sum = request.Request.BaseData?.Sum,
            CurrencyCode = request.Request.BaseData?.CurrencyCode,
            IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)request.Request.IncomeTypeId
        };

        var dataObject = getDataObject(request.Request);
        if (dataObject != null)
        {
            var serOpt = new JsonSerializerOptions();
            serOpt.Converters.Add(new CustomJsonConverterForType());
            entity.Data = JsonSerializer.Serialize(dataObject, serOpt);
            entity.DataBin = dataObject.ToByteArray();
        }
        
        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSAIncome), entity.CustomerOnSAIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerOnSAIncomeId
        };
    }

    static IMessage? getDataObject(CreateIncomeRequest request)
        => (CIS.Foms.Enums.CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CIS.Foms.Enums.CustomerIncomeTypes.Employement => request.Employement,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<CreateIncomeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public class CustomJsonConverterForType : JsonConverter<Type>
    {
        public override Type Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
            )
        {
            // Caution: Deserialization of type instances like this 
            // is not recommended and should be avoided
            // since it can lead to potential security issues.

            // If you really want this supported (for instance if the JSON input is trusted):
            // string assemblyQualifiedName = reader.GetString();
            // return Type.GetType(assemblyQualifiedName);
            throw new NotSupportedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Type value,
            JsonSerializerOptions options
            )
        {
            string assemblyQualifiedName = value.AssemblyQualifiedName;
            // Use this with caution, since you are disclosing type information.
            writer.WriteStringValue(assemblyQualifiedName);
        }
    }
}
