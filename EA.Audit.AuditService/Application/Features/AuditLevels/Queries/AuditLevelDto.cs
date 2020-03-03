﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EA.Audit.AuditService.Application.Features.AuditLevels.Queries
{
    public class AuditLevelDto
    {
        [Display(Name = "AuditLevelId")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}