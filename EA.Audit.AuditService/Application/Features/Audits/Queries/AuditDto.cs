using System;
using System.ComponentModel.DataAnnotations;
using EA.Audit.AuditService.Model.Admin;
using EA.Audit.AuditService.Models;

namespace EA.Audit.AuditService.Application.Features.Audits.Queries
{
    public class AuditDto
    {
        [Display(Name = "AuditId")]
        public int Id { get; set; }

        public AuditApplication Application { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateCreated { get; set; }
        public AuditType AuditType { get; set; }
        public AuditLevel AuditLevel { get; set; }
        public string Source { get; set; }
        public string Details { get; set; }

    }
}