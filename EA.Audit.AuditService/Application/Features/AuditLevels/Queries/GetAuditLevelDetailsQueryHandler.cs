using EA.Audit.AuditService.Data;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Logging;
using EA.Audit.AuditService.Application.Extensions;

namespace EA.Audit.AuditService.Application.Features.AuditLevels.Queries
{

    public class GetAuditLevelDetailsQuery : IRequest<AuditLevelDto>
    {
        public int Id { get; set; }
    }

    public class GetAuditTypeDetailsQueryHandler : RequestHandler<GetAuditLevelDetailsQuery, AuditLevelDto>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAuditTypeDetailsQueryHandler> _logger;

        public GetAuditTypeDetailsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper, ILogger<GetAuditTypeDetailsQueryHandler> logger)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
            _logger = logger;
        }

        protected override AuditLevelDto Handle(GetAuditLevelDetailsQuery request)
        {
            _logger.LogInformation(
                        "----- Handling query: {RequestName} - ({@Request})",
                        request.GetGenericTypeName(),
                        request);

            return _mapper.Map<AuditLevelDto>(_dbContext.AuditLevels.FirstOrDefault(a => a.Id == request.Id));
        }
    }
}
