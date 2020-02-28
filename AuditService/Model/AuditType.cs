using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Models
{
    public class AuditType : BaseEntity
    {
        [Column("AuditTypeID")]
        public new int Id { get; set; }
        public string Description { get; set; }
    }
}