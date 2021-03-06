using EA.Audit.AuditService.Model.Admin;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Audit.AuditService.Models
{
    public class AuditEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuditID")]
        public new long Id { get; set; }
        public AuditApplication AuditApplication { get; set; } 
        public long SubjectId { get; set; }
        public string Subject { get; set; }
        public long ActorId { get; set; }
        public string Actor { get; set; }
        public string Description { get; set; }     
        public string Properties { get; set; }

    }
}
