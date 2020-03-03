using EA.Audit.AuditService.Models;
using AutoMapper;

namespace EA.Audit.AuditService.Application.Features.AuditLevels.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuditLevel, AuditLevelDto>();
            CreateMap<GetAuditLevelsQuery, PaginationFilter>();
        }
    }
}
