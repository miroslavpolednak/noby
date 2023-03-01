namespace DomainServices.CodebookService.ClientsGenerators
{
    internal class Endpoint
    {
        public Endpoint(string methodName, string returnType, string requestDtoType)
        {
            MethodName = methodName;
            ReturnType = returnType;
            RequestDtoType = requestDtoType;
            ReturnTypeSync = returnType.Substring(28, returnType.Length - 29);
        }

        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public string ReturnTypeSync { get; set; }
        public string RequestDtoType { get; set; }
    }
}
