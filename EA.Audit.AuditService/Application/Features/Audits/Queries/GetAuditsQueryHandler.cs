using System.Linq;
using EA.Audit.AuditService.Data;
using MediatR;
using AutoMapper;
using EA.Audit.AuditService.Models;
using EA.Audit.AuditService.Infrastructure;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace EA.Audit.AuditService.Application.Features.Audits.Queries
{ 
    public class GetAuditsQuery : IRequest<PagedResponse<AuditDto>>
    { 
        public GetAuditsQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }
        public GetAuditsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
    }

    public class GetAuditsQueryHandler : RequestHandler<GetAuditsQuery, PagedResponse<AuditDto>>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ILogger<GetAuditsQueryHandler> _logger;

        public GetAuditsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper, IUriService uriService, ILogger<GetAuditsQueryHandler> logger)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
            _uriService = uriService;
            _logger = logger;
        }       

        protected override PagedResponse<AuditDto> Handle(GetAuditsQuery request)
        {
            _logger.LogInformation(
                        "----- Handling query: {RequestName} - ({@Request})",
                        request.GetGenericTypeName(),
                        request);

            if (request == null)
            {
                var response = _mapper.ProjectTo<AuditDto>(_dbContext.Audits).OrderBy(a => a.Id).ToList();
                return new PagedResponse<AuditDto>(response);
            }

            var pagination = _mapper.Map<PaginationFilter>(request);


            var skip = (request.PageNumber) * request.PageSize;

            var audits = _mapper.ProjectTo<AuditDto>(_dbContext.Audits).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToList();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, audits);

            return paginationResponse;
    
        }
    }
}