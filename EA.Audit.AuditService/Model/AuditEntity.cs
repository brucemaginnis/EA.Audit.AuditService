using EA.Audit.AuditService.Model.Admin;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Audit.AuditService.Models
{
    public class AuditEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuditID")]
        public new int Id { get; set; }
        public int ApplicationId { get; set; }
        public AuditApplication AuditApplication { get; set; }   
        public int AuditTypeId { get; set; }
        public AuditType AuditType { get; set; }
        public string Source { get; set; }        
        public int AuditLevelId { get; set; }
        public AuditLevel AuditLevel { get; set; }
        public string Details { get; set; }

    }
}
