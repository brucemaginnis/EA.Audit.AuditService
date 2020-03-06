using EA.Audit.AuditService.Application.Extensions;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Data;
using EA.Audit.AuditService.Infrastructure;
using EA.Audit.AuditService.Models;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EA.Audit.AuditService.Application.Features.AuditTypes.Queries
{
    public class GetAuditTypesQuery : IRequest<PagedResponse<AuditTypeDto>>
    {
        public GetAuditTypesQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }
        public GetAuditTypesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAuditTypesQueryHandler : RequestHandler<GetAuditTypesQuery, PagedResponse<AuditTypeDto>>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ILogger<GetAuditTypesQueryHandler> _logger;

        public GetAuditTypesQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper, IUriService uriService, ILogger<GetAuditTypesQueryHandler> logger)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
            _uriService = uriService;
            _logger = logger;
        }

        protected override PagedResponse<AuditTypeDto> Handle(GetAuditTypesQuery request)
        {
            int total = 0;
            if (request == null)
            {
                var response = _mapper.ProjectTo<AuditTypeDto>(_dbContext.AuditTypes).OrderBy(a => a.Id).ToList();
                total = _dbContext.AuditTypes.Count();
                return new PagedResponse<AuditTypeDto>(response, total);
            }

            _logger.LogInformation(
                        "----- Handling query: {RequestName} - ({@Request})",
                        request.GetGenericTypeName(),
                        request);

            var pagination = _mapper.Map<PaginationFilter>(request);

            var skip = (request.PageNumber) * request.PageSize;
            var query = _mapper.ProjectTo<AuditTypeDto>(_dbContext.AuditTypes).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToSql();

            var audits = _mapper.ProjectTo<AuditTypeDto>(_dbContext.AuditTypes).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToList();

            total = _dbContext.AuditTypes.Count();
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, audits, total);

            return paginationResponse;

        }
    }
}
