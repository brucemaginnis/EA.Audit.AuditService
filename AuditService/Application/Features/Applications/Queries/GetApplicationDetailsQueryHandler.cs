using AuditService.Data;
using AutoMapper;
using MediatR;
using System.Linq;

namespace AuditService.Application.Features.Application.Queries
{

    public class GetAuditApplicationDetailsQuery : IRequest<ApplicationDto>
    {
        public int Id { get; set; }
    }

    public class GetApplicationDetailsQueryHandler : RequestHandler<GetAuditApplicationDetailsQuery, ApplicationDto>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public GetApplicationDetailsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
        }

        protected override ApplicationDto Handle(GetAuditApplicationDetailsQuery message)
        {
            return _mapper.Map<ApplicationDto>(_dbContext.AuditApplications.FirstOrDefault(a => a.Id == message.Id));
        }
    }
}
