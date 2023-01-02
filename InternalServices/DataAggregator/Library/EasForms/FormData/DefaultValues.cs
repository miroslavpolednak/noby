using CIS.InternalServices.DataAggregator.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData;

public class DefaultValues
{
    private DefaultValues()
    {
    }

    public static DefaultValues Create(EasFormType formType) =>
        formType switch
        {
            EasFormType.F3700 => new DefaultValues { FormType = "3700A", PasswordCode = "613226" },
            EasFormType.F3601 => new DefaultValues { FormType = "3601A", PasswordCode = "608248" },
            EasFormType.F3602 => new DefaultValues { FormType = "3602A", PasswordCode = "608243" },
            _ => throw new ArgumentOutOfRangeException(nameof(formType), formType, null)
        };

    public required string FormType { get; init; }

    public required string PasswordCode { get; init; }
}