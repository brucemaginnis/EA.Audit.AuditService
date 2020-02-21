using AuditService.Models;
using AutoMapper;

namespace AuditService.Application.Features.Audits.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Audit, AuditDto>();
            CreateMap<GetAuditsQuery,PaginationFilter>();
        }
    }
}