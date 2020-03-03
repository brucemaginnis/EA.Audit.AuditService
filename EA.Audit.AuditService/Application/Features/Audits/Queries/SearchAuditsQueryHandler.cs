using EA.Audit.AuditService.Application.Extensions;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Data;
using EA.Audit.AuditService.Infrastructure;
using EA.Audit.AuditService.Models;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EA.Audit.AuditService.Application.Features.Audits.Queries
{
    public class SearchAuditsQuery : IRequest<PagedResponse<AuditDto>>
    {
        public SearchAuditsQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }
        public SearchAuditsQuery(string sourceContains, string detailsContains, int pageNumber, int pageSize)
        {
            SourceContains = sourceContains;
            DetailsContains = detailsContains;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string SourceContains { get; set; }
        public string DetailsContains { get; set; }
    }

    public class SearchAuditsQueryHandler : RequestHandler<SearchAuditsQuery, PagedResponse<AuditDto>>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ILogger<SearchAuditsQueryHandler> _logger;

        public SearchAuditsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper, IUriService uriService, ILogger<SearchAuditsQueryHandler> logger)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
            _uriService = uriService;
            _logger = logger;
        }

        protected override PagedResponse<AuditDto> Handle(SearchAuditsQuery request)
        {
            if (request == null)
            {
                var response = _mapper.ProjectTo<AuditDto>(_dbContext.Audits).OrderBy(a => a.Id).ToList();
                return new PagedResponse<AuditDto>(response);
            }

            _logger.LogInformation(
                       "----- Handling query: {RequestName} - ({@Request})",
                       request.GetGenericTypeName(),
                       request);

            var pagination = _mapper.Map<PaginationFilter>(request);

            var skip = (request.PageNumber) * request.PageSize;

            var query = _mapper.ProjectTo<AuditDto>(_dbContext.Audits)
                .Where(a => string.IsNullOrEmpty(request.DetailsContains) || a.Details.Contains(request.DetailsContains))
                .Where(a => string.IsNullOrEmpty(request.SourceContains) || a.Source.Contains(request.SourceContains))
                .OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToSql();

            var audits = _mapper.ProjectTo<AuditDto>(_dbContext.Audits)
                .Where(a => string.IsNullOrEmpty(request.DetailsContains) || a.Details.Contains(request.DetailsContains))
                .Where(a => string.IsNullOrEmpty(request.SourceContains) || a.Source.Contains(request.SourceContains))
                .OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToList();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, audits);

            return paginationResponse;

        }
    }
}


