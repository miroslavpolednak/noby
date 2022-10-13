namespace DomainServices.SalesArrangementService.Api.Handlers.Shared
{
    public class DefaultFormValues
    {
        #region Static

        private static Dictionary<EFormType, DefaultFormValues> _formValues = new Dictionary<EFormType, DefaultFormValues>();
        public static DefaultFormValues GetInstance(EFormType formType)
        {
            if (!_formValues.ContainsKey(formType))
            {
                _formValues.Add(formType, CreateDefaultFormValues(formType));
            }

            return _formValues[formType];
        }

        private static DefaultFormValues CreateDefaultFormValues(EFormType formType)
        {
            DefaultFormValues? formValues;

            switch (formType)
            {
                case EFormType.F3601:
                    formValues = new DefaultFormValues { TypFormulare = "3601A", HesloKod = "608248" };
                    break;

                case EFormType.F3602:
                    formValues = new DefaultFormValues { TypFormulare = "3602A", HesloKod = "608243" };
                    break;

                case EFormType.F3700:
                    formValues = new DefaultFormValues { TypFormulare = "3700A", HesloKod = "613226" };
                    break;

                default:
                    throw new CisArgumentException(99999, $"Form type #{formType} is not supported.", nameof(formType));
            }

            return formValues;
        }

        #endregion

        private DefaultFormValues() { }

        public string TypFormulare { get; init; } = String.Empty;
        public string HesloKod { get; init; } = String.Empty;
    }
}
