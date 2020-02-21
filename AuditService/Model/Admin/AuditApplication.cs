using AuditService.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditService.Model.Admin
{
    public class AuditApplication : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ApplicationID")]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
