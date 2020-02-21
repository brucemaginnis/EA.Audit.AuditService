using AuditService.Application.Commands;
using AuditService.Model.Admin;
using AuditService.Models;
using AutoMapper;

namespace AuditService.Application.Features.Audits.Commands
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAuditCommand, Audit>(MemberList.Source);
            CreateMap<CreateAuditApplicationCommand, AuditApplication>(MemberList.Source);
        }
    }
}