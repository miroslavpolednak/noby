﻿using CIS.Core.Extensions;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
using DomainServices.ProductService.ExternalServices.Pcp.V1;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DomainServices.ProductService.ExternalServices.Pcp.V2;
internal sealed class RealSpeedPcpClient : SoapClientBase<ProductInstanceBEServiceClient, ProductInstanceBEService>, IPcpClient
{
    private const string _catalogueItemObjectCode = "30130538920";

    public RealSpeedPcpClient(
       IExternalServiceConfiguration<IPcpClient> configuration,
        ILogger<RealPcpClient> logger)
            : base(configuration, logger)
    {
    }

    protected override string ServiceName => "PcpSpeed";

    public async Task<ProductInstance3[]> GetByOtherIds(long caseId, CancellationToken cancellationToken = default)
    {
        return await callMethod(async () =>
        {
            using (new OperationContextScope(Client.InnerChannel))
            {
                AddSecurityHeader();

                var result = await Client.getByOtherIdsAsync(CreateSystemIdentity(), CreateTraceContext(), GetByOtherIdsRequest(caseId));

                return result.getByOtherIdsResponse?.productInstanceList ?? [];
            }
        });  
    }

    public async Task<string> CreateProduct(long caseId, long customerKbId, string pcpObjectCode, CancellationToken cancellationToken = default)
    {
        return await callMethod(async () =>
        {
            var systemIdetity = CreateSystemIdentity();

            var traceContext = CreateTraceContext();

            var request = CreateRequest(caseId, customerKbId, pcpObjectCode);

            using (new OperationContextScope(Client.InnerChannel))
            {
                AddSecurityHeader();

                var result = await Client.createAsync(systemIdetity, traceContext, request).WithCancellation(cancellationToken);
                return result.createResponse.productInstanceReference.referentialMktItemInstanceId.id;
            }
        });
    }

	public async Task<string> UpdateProduct(string pcpId, long customerKbId, CancellationToken cancellationToken = default)
	{
		return await callMethod(async () =>
		{
			var systemIdetity = CreateSystemIdentity();

			var traceContext = CreateTraceContext();

			var request = UpdateRequest(pcpId, customerKbId);

			using (new OperationContextScope(Client.InnerChannel))
            {
                AddSecurityHeader();

				await Client.updateAsync(systemIdetity, traceContext, request).WithCancellation(cancellationToken);
                return "";
			}
		});
	}

	protected override Binding CreateBinding()
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
        binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.UseDefaultWebProxy = false;
        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;
        binding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        binding.ReceiveTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        return binding;
    }

    private void AddSecurityHeader()
    {
        if (Configuration.Authentication == ExternalServicesAuthenticationTypes.PasswordDigest)
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(new WsseSoapPasswordDigestSecurityHeader(Configuration.Username!, Configuration.Password!));
        }
        else
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(
                new WsseSoapPasswordTextSecurityHeader(Configuration.Username!, Configuration.Password!, NonceGenerator.GetNonce(), DateTime.Now));
        }
    }

    private static getByOtherIdsRequest GetByOtherIdsRequest(long caseId)
    {
        return new getByOtherIdsRequest
        {
            productInstanceReferenceList =
            [
                new ProductInstanceReference1
                {
                    otherMktItemInstanceId = new OtherMktItemInstanceId1
                    {
                        @class = "ID",
                        id = caseId.ToString(CultureInfo.InvariantCulture)
                    }
                }
            ]
        };
    }

	private static updateRequest UpdateRequest(string pcpId, long customerKbId)
    {
        return new updateRequest
        {
            productInstance = new ProductInstance3
            {
                customerInProductInstanceList =
				[
					new CustomerInProductInstance3()
                    {
                        kBCustomer = new KBCustomer3
                        {
                            id = customerKbId.ToString(CultureInfo.InvariantCulture)
                        },
                        partyInProductInstanceRole = new PartyInProductInstanceRole3
                        {
                            partyInproductInstanceRoleCode = new PartyInproductInstanceRoleCode3
                            {
                                @class = "CB_CustomerInMortgageInstanceRole",
                                code = "A"
							}
                        }
                    }
				],
				mktItemInstanceState = new MktItemInstanceState3
                {
                    state = "2",
                    @class = "CB_MortgageInstanceState"
				},
                referentialMktItemInstanceId = new ReferentialMktItemInstanceId3
                {
                    id = pcpId
                },
                lastModifiedOn = DateTime.Now
            }
        };
    }

	private static createRequest CreateRequest(long caseId, long customerKbId, string pcpObjectCode)
    {
        return new createRequest
        {
            productInstance = new ProductInstance
            {
                agreement = new Agreement
                {
                    offer = new Offer
                    {
                        catalogueOffer = new CatalogueOffer
                        {
                            catalogueItemIdentification = new CatalogueItemIdentification
                            {
                                Item = new CatalogueItemObjectCode { objectCode = _catalogueItemObjectCode }
                            }
                        }
                    }
                },

                customerInProductInstanceList =
                                [ new CustomerInProductInstance
                            {
                              kBCustomer = new() { id = customerKbId.ToString(CultureInfo.InvariantCulture) },
                              partyInProductInstanceRole = new()
                              {
                                  partyInproductInstanceRoleCode = new() { code ="A", @class = "CB_CustomerInMortgageInstanceRole"}
                              }
                            }
                                ],

                productInstanceInfo = new ProductInstanceInfo(), // Have to be here because xml schema validation

                mktItemInstanceState = new MktItemInstanceState
                {
                    @class = "CB_MortgageInstanceState",
                    state = "12"
                },

                otherMktItemInstanceIdList = [
                                new OtherMktItemInstanceId{
                            @class ="ID",
                            id = caseId.ToString(CultureInfo.InvariantCulture)
                        }
                                ],

                productInOffer = new ProductInOffer
                {
                    catalogueProductInOffer = new CatalogueProductInOffer
                    {
                        catalogueItemIdentification = new CatalogueItemIdentification { Item = new CatalogueItemObjectCode { objectCode = pcpObjectCode } }
                    }
                },

                sourceMaster = new SourceMaster
                {
                    application = new Application
                    {
                        applicationCode = new ApplicationCode
                        {
                            code = "STARBUILD"
                        }
                    }
                }
            }
        };
    }

    private static TraceContext CreateTraceContext()
    {
        return new TraceContext
        {
            traceId = (Activity.Current?.TraceId.ToHexString() ?? Guid.NewGuid().ToString())[0..15],
            timestamp = DateTime.Now,
            timestampSpecified = true
        };
    }

    private static SystemIdentity CreateSystemIdentity()
    {
        return new SystemIdentity
        {
            caller = new Application1
            {
                application = "NOBY",
                applicationComponent = "NOBY.DS"
            },
            originator = new Application1
            {
                application = "NOBY",
                applicationComponent = "NOBY.FEAPI"
            }
        };
    }
}
