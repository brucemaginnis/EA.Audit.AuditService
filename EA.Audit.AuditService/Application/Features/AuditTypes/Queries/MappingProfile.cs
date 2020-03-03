using EA.Audit.AuditService.Model.Admin;
using EA.Audit.AuditService.Models;
using AutoMapper;

namespace EA.Audit.AuditService.Application.Features.AuditTypes.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuditType, AuditTypeDto>();
            CreateMap<GetAuditTypesQuery, PaginationFilter>();
        }
    }
}
