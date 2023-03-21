namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

public static class EasFormTypeFactory
{
    public static EasFormType GetEasFormType(int documentTypeId) =>
        documentTypeId switch
        {
            4 => EasFormType.F3601,
            5 => EasFormType.F3602,
            6 => EasFormType.F3700,
            _ => throw new NotImplementedException()
        };
}