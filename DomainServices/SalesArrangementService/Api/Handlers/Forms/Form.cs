namespace DomainServices.SalesArrangementService.Api.Handlers.Forms
{
    public class Form
    {
        public EFormType FormType { get; init; }

        public DynamicValues? DynamicValues { get; init; } = new DynamicValues();

        public string JSON { get; init; } = String.Empty;
    }
}