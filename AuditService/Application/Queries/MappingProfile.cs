using AuditService.Models;
using AutoMapper;

namespace AuditService.Application.Queries
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