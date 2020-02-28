namespace AuditService.Data
{
    public interface IAuditContextFactory
    {
        AuditContext AuditContext { get; }
    }
}
