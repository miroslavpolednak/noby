using CIS.Core.Data;
using ExternalServices.ESignatures.Abstraction;

namespace ExternalServices.ESignatures.V1.Repositories;

public class RealESignaturesRepository : IESignaturesRepository
{
    private readonly IConnectionProvider<IESignaturesDapperConnectionProvider> _connectionProvider;

    public RealESignaturesRepository(IConnectionProvider<IESignaturesDapperConnectionProvider> connectionProvider)
    {
        Dapper.SqlMapper.Settings.CommandTimeout = 10;
        _connectionProvider = connectionProvider;
    }
}