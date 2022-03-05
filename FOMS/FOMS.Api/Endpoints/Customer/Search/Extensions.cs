using CIS.Foms.Types;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.Search.Dto;
using Google.Protobuf.Collections;

namespace FOMS.Api.Endpoints.Customer.Search;

internal static class Extensions
{
    public static SearchCustomersRequest ToDomainServiceRequest(this Dto.SearchData request)
    {
        var model = new SearchCustomersRequest
        {
            NaturalPerson = new SearchNaturalPerson()
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            }
        };
        return model;
    }
    
    public static List<CustomerInList> ToApiResponse(this RepeatedField<SearchCustomerResult> request)
    {
        return request.Select(t => (new CustomerInList())
            .FillBaseData(t)
            .FillIdentification(t.Identities)
            ).ToList();
    }

    private static CustomerInList FillBaseData(this CustomerInList customer, SearchCustomerResult result)
    {
        customer.FirstName = result.NaturalPerson?.FirstName;
        customer.LastName = result.NaturalPerson?.LastName;
        customer.Street = result.Street;
        customer.City = result.City;
        customer.Postcode = result.Postcode;
        
        return customer;
    }

    private static CustomerInList FillIdentification(this CustomerInList customer, RepeatedField<Identity>? identities)
    {
        if (identities is not null && identities.Any())
        {
            customer.Identity = new CustomerIdentity(identities[0].IdentityId, identities[0].IdentityScheme.ToString());
        }
        return customer;
    } 
    
    private static CustomerInList FillAddress(this CustomerInList customer, RepeatedField<Address>? result)
    {
        if (result is not null && result.Any())
        {
            customer.Street = $"{result[0].Street} {result[0].BuildingIdentificationNumber} {result[0].LandRegistryNumber}";//TODO ???
            customer.City = result[0].City;
            customer.Postcode = result[0].Postcode;
        }
        return customer;
    }
}