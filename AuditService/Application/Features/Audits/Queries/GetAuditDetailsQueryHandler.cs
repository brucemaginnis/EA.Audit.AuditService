using AutoMapper;
using System.Linq;
using AuditService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Application.Features.Audits.Queries
{
    public class GetAuditDetailsQuery : IRequest<AuditDto>
    { 
        public int Id { get; set; }
    }
    
    public class GetAuditDetailsQueryHandler : RequestHandler<GetAuditDetailsQuery, AuditDto>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public GetAuditDetailsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper){
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
        }    

        protected override AuditDto Handle(GetAuditDetailsQuery message)
        {
            return _mapper.Map<AuditDto>(_dbContext.Audits.Include(a => a.AuditApplication)
                                                            .Include(a => a.AuditLevel)
                                                            .Include(a => a.AuditType)
                                                            .FirstOrDefault(a => a.Id == message.Id));
        }
    }
}