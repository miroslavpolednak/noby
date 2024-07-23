namespace NOBY.ApiContracts;

public partial class SharedTypesWorkflowTaskDetailAmendments
{
    public static SharedTypesWorkflowTaskDetailAmendments Create(SharedTypesWorkflowAmendmentsConsultationData? model)
    {
        return new SharedTypesWorkflowTaskDetailAmendments
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendments.ConsultationData),
            ConsultationData = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendments Create(SharedTypesWorkflowAmendmentsPriceException? model)
    {
        return new SharedTypesWorkflowTaskDetailAmendments
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendments.PriceException),
            PriceException = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendments Create(SharedTypesWorkflowAmendmentsRequest? model)
    {
        return new SharedTypesWorkflowTaskDetailAmendments
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendments.Request),
            Request = model
        };
    }

    public static SharedTypesWorkflowTaskDetailAmendments Create(SharedTypesWorkflowAmendmentsSigning? model)
    {
        return new SharedTypesWorkflowTaskDetailAmendments
        {
            Discriminator = nameof(SharedTypesWorkflowTaskDetailAmendments.Signing),
            Signing = model
        };
    }
}
