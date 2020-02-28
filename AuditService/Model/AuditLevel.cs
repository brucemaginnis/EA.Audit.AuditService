using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Models
{
    public class AuditLevel : BaseEntity
    {
         [Column("AuditLevelID")]
        public new int Id { get; set; }

        public string Description { get; set;}
    }
}