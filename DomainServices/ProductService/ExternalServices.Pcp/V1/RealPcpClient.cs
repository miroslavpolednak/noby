using CIS.Infrastructure.ExternalServicesHelpers.BaseClasses;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.Extensions.Logging;
using System.ServiceModel.Channels;
using System.ServiceModel;
using DomainServices.ProductService.ExternalServices.Pcp.V1.Client;
using System.Diagnostics;
using CIS.Core.Extensions;

namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

internal sealed class RealPcpClient 
    : SoapClientBase<ProductInstanceBEServiceClient, IProductInstanceBEService>, IPcpClient
{
    public async Task CreateProduct(long caseId, long customerKbId, string pcpProductId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await callMethod<createResponse>(async () =>
        {
            var result = await Client.createAsync(createSystemIdentity(), createTraceContext(), new createRequest
            {
                productInstance = new ProductInstance
                {
                    otherMktItemInstanceIdList = new OtherMktItemInstanceId[1]
                    {
                        new OtherMktItemInstanceId { @class = "ID", id = caseId.ToString() }
                    },
                    mktItemInstanceState = new MktItemInstanceState
                    {
                        @class = "CB_AgreementState",
                        state = "PROPOSED"
                    },
                    customerInProductInstanceList = new CustomerInProductInstance[]
                    {
                        new CustomerInProductInstance
                        {
                            partyInProductInstanceRole = new PartyInProductInstanceRole
                            {
                                partyInproductInstanceRoleCode = new PartyInproductInstanceRoleCode
                                {
                                    @class = "CB_CustomerLoanProductRole",
                                    code = "A"
                                }
                            },
                            kBCustomer = new KBCustomer { id = customerKbId.ToString() }
                        }
                    },
                    productInOffer = new ProductInOffer
                    {
                        catalogueProductInOffer = new CatalogueProductInOffer
                        {
                            catalogueItemId = new CatalogueItemId
                            {
                                id = pcpProductId
                            }
                        }

                    }
                }
            }).WithCancellation(cancellationToken);

            return result.createResponse;
        });
    }

    private SystemIdentity createSystemIdentity()
    {
        return new SystemIdentity
        {
            caller = new Application
            {
                application = "NOBY",
                applicationComponent = "NOBY.DS"
            },
            originator = new Application
            {
                application = "NOBY",
                applicationComponent = "NOBY.FEAPI"
            }
        };
    }

    private TraceContext createTraceContext()
    {
        return new TraceContext
        {
            traceId = Activity.Current?.TraceId.ToHexString() ?? Guid.NewGuid().ToString(),
            timestamp = DateTime.Now
        };
    }

    protected override Binding CreateBinding()
    {
        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

        if (Configuration.RequestTimeout.HasValue)
        {
            basicHttpBinding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
            basicHttpBinding.CloseTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
        }
        basicHttpBinding.MaxReceivedMessageSize = 1500000;
        basicHttpBinding.ReaderQuotas.MaxArrayLength = 1500000;

        return basicHttpBinding;
    }

    protected override string ServiceName => global::ExternalServices.StartupExtensions.ServiceName;

    public RealPcpClient(
        ILogger<RealPcpClient> logger,
        IExternalServiceConfiguration<IPcpClient> configuration)
        : base(configuration, logger)
    {
    }
}
