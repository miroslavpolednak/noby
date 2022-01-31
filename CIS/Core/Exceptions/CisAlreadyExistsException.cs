namespace CIS.Core.Exceptions
{
    public sealed class CisAlreadyExistsException : BaseCisException
    {
        public string? EntityName { get; private set; }
        public long? EntityIdLong { get; private set; }
        public int? EntityIdInt { get; private set; }

        public CisAlreadyExistsException(int exceptionCode, string message) 
            : base(exceptionCode, message) 
        { }

        public CisAlreadyExistsException(int exceptionCode, string entityName, int entityId)
            : base(exceptionCode, $"{entityName} {entityId} already exists.")
        {
            EntityName = entityName;
            EntityIdInt = entityId;
        }

        public CisAlreadyExistsException(int exceptionCode, string entityName, long entityId)
            : base(exceptionCode, $"{entityName} {entityId} already exists.")
        {
            EntityName = entityName;
            EntityIdLong = entityId;
        }

        public long GetId() => EntityIdLong ?? EntityIdInt ?? 0;
    }
}
