using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Models
{
    public class AuditType : IEntity
    {
        [Column("AuditTypeID")]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}