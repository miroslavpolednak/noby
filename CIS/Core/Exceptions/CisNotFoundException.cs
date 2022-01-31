namespace CIS.Core.Exceptions
{
    public sealed class CisNotFoundException : BaseCisException
    {
        public string? EntityName { get; private set; }
        public long? EntityIdLong { get; private set; }
        public int? EntityIdInt { get; private set; }

        public CisNotFoundException(int exceptionCode, string message) 
            : base(exceptionCode, message) 
        { }

        public CisNotFoundException(int exceptionCode, string entityName, int entityId)
            : base(exceptionCode, $"{entityName} {entityId} not found.")
        {
            EntityName = entityName;
            EntityIdInt = entityId;
        }

        public CisNotFoundException(int exceptionCode, string entityName, long entityId)
            : base(exceptionCode, $"{entityName} {entityId} not found.")
        {
            EntityName = entityName;
            EntityIdLong = entityId;
        }

        public long GetId() => EntityIdLong ?? EntityIdInt ?? 0;
    }
}
