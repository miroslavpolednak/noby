namespace FOMS.FormContracts
{
    internal class UiTemplateDescriptorAttribute : Attribute
    {
        public int Order { get; init; }

        public string ShortName { get; init; }

        public UiTemplateDescriptorAttribute(int order, string shortName)
        {
            Order = order;
            ShortName = shortName;
        }
    }
}
