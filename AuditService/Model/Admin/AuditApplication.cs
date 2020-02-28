using AuditService.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Model.Admin
{
    public class AuditApplication : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ApplicationID")]
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
