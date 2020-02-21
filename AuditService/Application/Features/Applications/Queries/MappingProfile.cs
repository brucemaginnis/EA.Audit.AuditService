using AuditService.Model.Admin;
using AuditService.Models;
using AutoMapper;

namespace AuditService.Application.Features.Application.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuditApplication, ApplicationDto>();
            CreateMap<GetAuditApplicationsQuery, PaginationFilter>();
        }
    }
}
