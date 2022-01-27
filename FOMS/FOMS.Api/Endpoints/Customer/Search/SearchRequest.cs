﻿namespace FOMS.Api.Endpoints.Customer.Dto;

internal sealed class SearchRequest
    : IRequest<SearchResponse>
{
    public SearchFilter? Filter { get; set; }
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }

    public class SearchFilter
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? BirthNumber { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? IdentificationDocumentType { get; set; }
        public string? IdentificationDocumentNumber { get; set; }
        public string? TaxId { get; set; }
        public bool IsNaturalPerson { get; set; }
    }
}
