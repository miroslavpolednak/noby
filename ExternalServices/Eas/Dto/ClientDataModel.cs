﻿namespace ExternalServices.Eas.Dto;

public sealed class ClientDataModel
{
    public string? KbId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirthNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public SharedTypes.Enums.Genders Gender { get; set; }
    public string? Cin { get; set; }
    public ClientTypes ClientType { get; set; }
    
    public enum ClientTypes
    {
        FO = 1,
        PO = 2,
        Foreigner = 3
    }
}
