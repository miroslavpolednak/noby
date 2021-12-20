namespace FOMS.DocumentContracts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class ContractPartAttribute : Attribute
    {
        public Type ContractType { get; init; }
        public int PartId { get; init; }

        public ContractPartAttribute(Type contractType, int partId)
        {
            ContractType = contractType;
            PartId = partId;
        }
    }
}
