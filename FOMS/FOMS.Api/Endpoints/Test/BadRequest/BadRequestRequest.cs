using CIS.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.Test.Dto;

internal class BadRequestRequest 
    : IRequest<string>, IValidatableRequest
{
    [Display(Name = "Zobrazi se popisek?")]
    public int? Id { get; set; }
}

