namespace FOMS.FormContracts.Form1001
{
    internal class UiTemplate
    {
        [UiTemplateDescriptor(1, "Klient")]
        public Step1Model Step1 { get; set; }

        [UiTemplateDescriptor(2, "Obchod")]
        public Step2Model Step2 { get; set; }

        public class Step1Model
        {
            public SharedModels.ClientDetail Client { get; set; }
        }

        public class Step2Model
        {
            public Models.SalesData SalesData { get; set; }
        }
    }
}
