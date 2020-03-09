using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Audit.AuditService.Models
{
    public class AuditLevel : BaseEntity
    {
         [Column("AuditLevelID")]
        public new int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set;}
    }
}