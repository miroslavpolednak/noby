namespace DomainServices.CodebookService.ClientsGenerators
{
    internal class Endpoint
    {
        public Endpoint(string methodName, string returnType, string requestDtoType)
        {
            MethodName = methodName;
            ReturnType = returnType;
            RequestDtoType = requestDtoType;
        }

        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public string RequestDtoType { get; set; }
    }
}
