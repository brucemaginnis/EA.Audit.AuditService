using System;
using System.ComponentModel.DataAnnotations;

namespace AuditService.Application.Features.Application.Queries
{
    public class ApplicationDto
    {
        [Display(Name = "ApplicationId")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
