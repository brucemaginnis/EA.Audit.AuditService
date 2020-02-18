using AuditService.Models;
using AutoMapper;

namespace AuditService.Application.Commands
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAuditCommand, Audit>(MemberList.Source);
        }
    }
}