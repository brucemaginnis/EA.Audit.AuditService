using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Models
{
    public class Audit : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuditID")]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }        
        public int AuditTypeId { get; set; }
        public AuditType AuditType { get; set; }
        public string Source { get; set; }        
        public int AuditLevelId { get; set; }
        public AuditLevel AuditLevel { get; set; }
        public string Details { get; set; }

    }
}
