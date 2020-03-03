﻿using EA.Audit.AuditService.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Audit.AuditService.Model.Admin
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