using System;

namespace AuditService.Models
{
    public abstract class BaseEntity
    {
        protected Guid _tenantId;
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}