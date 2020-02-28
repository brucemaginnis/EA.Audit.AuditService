using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Audit.AuditService.Models
{
    public class AuditType : BaseEntity
    {
        [Column("AuditTypeID")]
        public new int Id { get; set; }
        public string Description { get; set; }
    }
}