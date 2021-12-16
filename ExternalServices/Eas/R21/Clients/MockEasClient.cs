﻿using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

internal sealed class MockEasClient : IEasClient
{
    public Versions Version { get; } = Versions.R21;

#pragma warning disable CS1998
    public async Task<IServiceCallResult> GetSavingsLoanId(long caseId)
#pragma warning restore CS1998
    {
        Random random = new Random();
        return new SuccessfulServiceCallResult<long>(random.NextInt64(100, 199));
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IServiceCallResult> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new SuccessfulServiceCallResult<ESBI_SIMULATION_RESULTS>(new ESBI_SIMULATION_RESULTS());
    }

#pragma warning disable CS1998
    public async Task<IServiceCallResult> GetCaseId(CIS.Core.IdentitySchemes mandant, int productInstanceType)
#pragma warning restore CS1998
    {
        Random random = new Random();
        return new SuccessfulServiceCallResult<long>(random.NextInt64(1, 99));
    }
}
