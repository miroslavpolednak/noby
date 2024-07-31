namespace NOBY.ApiContracts;

public partial class SharedTypesWorkflowTaskDetailAmendmentsOneOf
{
    public static SharedTypesWorkflowTaskDetailAmendmentsOneOf Create(SharedTypesWorkflowAmendmentsConsultationData? model)
    {
        return new()
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendmentsOneOf.ConsultationData),
            ConsultationData = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendmentsOneOf Create(SharedTypesWorkflowAmendmentsPriceException? model)
    {
        return new()
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendmentsOneOf.PriceException),
            PriceException = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendmentsOneOf Create(SharedTypesWorkflowAmendmentsRequest? model)
    {
        return new()
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendmentsOneOf.Request),
            Request = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendmentsOneOf Create(SharedTypesWorkflowAmendmentsSigning? model)
    {
        return new()
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendmentsOneOf.Signing),
            Signing = model
        };
    }
}
