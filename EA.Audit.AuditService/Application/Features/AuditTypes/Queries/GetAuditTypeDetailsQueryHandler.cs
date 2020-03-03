using EA.Audit.AuditService.Data;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Logging;
using EA.Audit.AuditService.Application.Extensions;

namespace EA.Audit.AuditService.Application.Features.AuditTypes.Queries
{

    public class GetAuditTypeDetailsQuery : IRequest<AuditTypeDto>
    {
        public int Id { get; set; }
    }

    public class GetAuditTypeDetailsQueryHandler : RequestHandler<GetAuditTypeDetailsQuery, AuditTypeDto>
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

        protected override AuditTypeDto Handle(GetAuditTypeDetailsQuery request)
        {
            _logger.LogInformation(
                        "----- Handling query: {RequestName} - ({@Request})",
                        request.GetGenericTypeName(),
                        request);

            return _mapper.Map<AuditTypeDto>(_dbContext.AuditLevels.FirstOrDefault(a => a.Id == request.Id));
        }
    }
}
