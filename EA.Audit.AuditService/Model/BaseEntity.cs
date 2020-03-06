using System;

namespace EA.Audit.AuditService.Models
{
    public abstract class BaseEntity
    {
        protected string _tenantId;
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}