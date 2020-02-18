using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Models
{
    public class AuditLevel : IEntity
    {
         [Column("AuditLevelID")]
        public int Id { get; set; }

        public string Description { get; set;}
    }
}