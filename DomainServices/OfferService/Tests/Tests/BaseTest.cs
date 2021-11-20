using Grpc.Core;
using ProtoBuf.Grpc;
using System.Text;

namespace DomainServices.OfferService.Tests;

public abstract class BaseTest
{
    protected Contracts.SimulateBuildingSavingsRequest createRequest(int targetAmount)
        => createRequest(targetAmount, true, false);

    protected Contracts.SimulateBuildingSavingsRequest createRequest(int targetAmount, bool clientIsNaturalPerson = true, bool clientIsSVJ = false, int productCode = 1, int actionCode = 1, int loanActionCode = 0, bool isWithLoan = false)
        => new Contracts.SimulateBuildingSavingsRequest
        {
            InputData = new Contracts.BuildingSavingsInput
            {
                TargetAmount = targetAmount,
                ClientIsNaturalPerson = clientIsNaturalPerson,
                ClientIsSVJ = clientIsSVJ,
                ProductCode = productCode,
                ActionCode = actionCode,
                IsWithLoan = isWithLoan,
                LoanActionCode = loanActionCode
            }
        };

    protected CallContext createCallContext()
    {
        var headers = new Metadata();

        // authentication
        var plainTextBytes = Encoding.UTF8.GetBytes($"{Constants.ServiceLogin}:{Constants.ServicePassword}");
        headers.Add("Authorization", "Basic " + Convert.ToBase64String(plainTextBytes));

        var options = new CallOptions(headers: headers);

        return new CallContext(options);
    }
}
