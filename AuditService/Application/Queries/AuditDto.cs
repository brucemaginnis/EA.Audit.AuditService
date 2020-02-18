using System;
using System.ComponentModel.DataAnnotations;
using AuditService.Models;

namespace AuditService.Application.Queries
{
    public class AuditDto
    {
        [Display(Name = "AuditId")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateCreated { get; set; }
        public AuditType AuditType { get; set; }
        public AuditLevel AuditLevel { get; set; }
        public string Source { get; set; }
        public string Details { get; set; }

    }
}